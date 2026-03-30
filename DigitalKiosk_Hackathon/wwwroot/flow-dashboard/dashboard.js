(function () {
    "use strict";

    // ── Configuration ──
    const HUB_URL = "/flow-hub";
    let TIME_WINDOW_SEC = 10;

    // ── Layer layout (left→right) ──
    const LAYERS = ["Controller", "Service", "Factory", "Repository", "Database"];
    const LAYER_COLORS = {
        Controller: "#1f6feb",
        Service: "#8957e5",
        Factory: "#d29922",
        Repository: "#2ea043",
        Database: "#f85149"
    };

    // ── State ──
    const events = [];          // all received events
    const nodes = new Map();    // key → { id, layer, className, x, y }
    const edges = new Map();    // "src→tgt" → { source, target, events:[], errorCount, totalCount }
    let selectedCorrelation = null;

    // ── DOM ──
    const svg = d3.select("#graph");
    const tooltip = d3.select("#tooltip");
    const statusEl = document.getElementById("connection-status");
    const eventCountEl = document.getElementById("event-count");
    const btnClear = document.getElementById("btn-clear");
    const timeWindowSlider = document.getElementById("time-window");
    const windowLabel = document.getElementById("window-label");
    const errorsOnlyCheckbox = document.getElementById("show-errors-only");

    let width, height;

    // ── SVG groups ──
    let gLayers, gEdges, gNodes, gParticles;

    function initSvg() {
        const rect = svg.node().getBoundingClientRect();
        width = rect.width;
        height = rect.height;
        svg.attr("viewBox", `0 0 ${width} ${height}`);

        gLayers = svg.append("g").attr("class", "layers");
        gEdges = svg.append("g").attr("class", "edges");
        gNodes = svg.append("g").attr("class", "nodes");
        gParticles = svg.append("g").attr("class", "particles");

        drawLayerLabels();
    }

    function drawLayerLabels() {
        const colW = width / LAYERS.length;
        gLayers.selectAll("text.layer-label")
            .data(LAYERS)
            .enter()
            .append("text")
            .attr("class", "layer-label")
            .attr("x", (d, i) => colW * i + colW / 2)
            .attr("y", 30)
            .attr("text-anchor", "middle")
            .text(d => d);

        // Vertical dividers
        gLayers.selectAll("line.divider")
            .data(LAYERS.slice(1))
            .enter()
            .append("line")
            .attr("class", "divider")
            .attr("x1", (d, i) => colW * (i + 1))
            .attr("x2", (d, i) => colW * (i + 1))
            .attr("y1", 0)
            .attr("y2", height)
            .attr("stroke", "#21262d")
            .attr("stroke-width", 1);
    }

    // ── Node management ──
    function getOrCreateNode(className, layerName) {
        const key = `${layerName}::${className}`;
        if (nodes.has(key)) return nodes.get(key);

        const layerIdx = LAYERS.indexOf(layerName);
        if (layerIdx === -1) return null;

        const colW = width / LAYERS.length;
        const layerNodes = [...nodes.values()].filter(n => n.layer === layerName);
        const ySlot = 70 + layerNodes.length * 60;

        const node = {
            id: key,
            className,
            layer: layerName,
            x: colW * layerIdx + colW / 2,
            y: ySlot
        };
        nodes.set(key, node);
        return node;
    }

    function getEdgeKey(sourceNode, targetNode) {
        return `${sourceNode.id}→${targetNode.id}`;
    }

    // ── Process incoming event ──
    function processEvent(ev) {
        events.push(ev);
        eventCountEl.textContent = `${events.length} events`;

        // Determine layers for source/target
        const srcLayer = guessLayer(ev.sourceClass, ev.layerName, "source");
        const tgtLayer = ev.layerName;

        const srcNode = getOrCreateNode(ev.sourceClass, srcLayer);
        const tgtNode = getOrCreateNode(ev.targetClass, tgtLayer);
        if (!srcNode || !tgtNode) return;

        const edgeKey = getEdgeKey(srcNode, tgtNode);
        if (!edges.has(edgeKey)) {
            edges.set(edgeKey, {
                source: srcNode,
                target: tgtNode,
                events: [],
                errorCount: 0,
                totalCount: 0
            });
        }

        const edge = edges.get(edgeKey);
        edge.events.push(ev);
        edge.totalCount++;
        if (ev.isError) edge.errorCount++;

        render();
        animateParticle(srcNode, tgtNode, ev.isError);
    }

    function guessLayer(className, eventLayer, role) {
        // The source is typically one layer "above" the event layer
        if (role === "source") {
            const idx = LAYERS.indexOf(eventLayer);
            if (idx > 0) return LAYERS[idx - 1];
        }
        return eventLayer;
    }

    // ── Rendering ──
    function render() {
        renderNodes();
        renderEdges();
    }

    function renderNodes() {
        const nodeData = [...nodes.values()];
        const groups = gNodes.selectAll("g.node")
            .data(nodeData, d => d.id);

        const enter = groups.enter()
            .append("g")
            .attr("class", "node")
            .attr("transform", d => `translate(${d.x}, ${d.y})`)
            .on("click", function(event, d) {
                event.stopPropagation();
            });

        const rectW = 130, rectH = 36;

        enter.append("rect")
            .attr("x", -rectW / 2)
            .attr("y", -rectH / 2)
            .attr("width", rectW)
            .attr("height", rectH)
            .attr("fill", d => {
                const base = LAYER_COLORS[d.layer] || "#30363d";
                return base + "33"; // semi-transparent fill
            })
            .attr("stroke", d => LAYER_COLORS[d.layer] || "#30363d");

        enter.append("text")
            .attr("y", 0)
            .text(d => d.className);

        // Update highlight state
        groups.merge(enter)
            .classed("highlighted", d => {
                if (!selectedCorrelation) return false;
                return isNodeInCorrelation(d, selectedCorrelation);
            });
    }

    function renderEdges() {
        const now = Date.now();
        const windowMs = TIME_WINDOW_SEC * 1000;
        const errOnly = errorsOnlyCheckbox.checked;

        const edgeData = [...edges.values()].map(e => {
            const recentEvents = e.events.filter(ev => (now - new Date(ev.timestamp).getTime()) < windowMs);
            const recentErrors = recentEvents.filter(ev => ev.isError);
            return {
                ...e,
                recentCount: recentEvents.length,
                recentErrorCount: recentErrors.length,
                hasError: recentErrors.length > 0,
                recentEvents
            };
        }).filter(e => !errOnly || e.hasError);

        // Scale thickness: 1-8px based on recent traffic
        const maxTraffic = Math.max(1, d3.max(edgeData, d => d.recentCount) || 1);
        const thicknessScale = d3.scaleLinear().domain([0, maxTraffic]).range([1.5, 8]).clamp(true);

        const lines = gEdges.selectAll("path.edge")
            .data(edgeData, d => `${d.source.id}→${d.target.id}`);

        lines.enter()
            .append("path")
            .attr("class", "edge")
            .merge(lines)
            .attr("d", d => {
                const sx = d.source.x, sy = d.source.y;
                const tx = d.target.x, ty = d.target.y;
                const mx = (sx + tx) / 2;
                return `M${sx},${sy} C${mx},${sy} ${mx},${ty} ${tx},${ty}`;
            })
            .attr("stroke-width", d => d.recentCount === 0 ? 1 : thicknessScale(d.recentCount))
            .attr("class", d => {
                let cls = "edge";
                if (d.recentCount === 0) cls += " idle";
                else if (d.hasError) cls += " error";
                else cls += " ok";
                if (selectedCorrelation && d.recentEvents.some(ev => ev.correlationId === selectedCorrelation))
                    cls += " highlighted";
                return cls;
            })
            .on("mouseover", function (event, d) {
                showTooltip(event, d);
            })
            .on("mouseout", function () {
                tooltip.classed("hidden", true);
            })
            .on("click", function (event, d) {
                event.stopPropagation();
                // Highlight correlation of most recent event
                if (d.recentEvents.length > 0) {
                    const lastEv = d.recentEvents[d.recentEvents.length - 1];
                    selectedCorrelation = selectedCorrelation === lastEv.correlationId ? null : lastEv.correlationId;
                    render();
                }
            });

        lines.exit().remove();
    }

    function isNodeInCorrelation(node, corrId) {
        for (const edge of edges.values()) {
            if (edge.source.id === node.id || edge.target.id === node.id) {
                if (edge.events.some(ev => ev.correlationId === corrId)) return true;
            }
        }
        return false;
    }

    // ── Particle animation ──
    function animateParticle(srcNode, tgtNode, isError) {
        const particle = gParticles.append("circle")
            .attr("class", isError ? "particle error" : "particle")
            .attr("r", isError ? 5 : 4)
            .attr("cx", srcNode.x)
            .attr("cy", srcNode.y);

        const mx = (srcNode.x + tgtNode.x) / 2;

        particle.transition()
            .duration(600)
            .ease(d3.easeCubicInOut)
            .attrTween("cx", () => {
                const interp = d3.interpolate(srcNode.x, tgtNode.x);
                return t => {
                    // Follow the bezier curve
                    const p0x = srcNode.x, p1x = mx, p2x = mx, p3x = tgtNode.x;
                    const u = 1 - t;
                    return u*u*u*p0x + 3*u*u*t*p1x + 3*u*t*t*p2x + t*t*t*p3x;
                };
            })
            .attrTween("cy", () => {
                return t => {
                    const p0y = srcNode.y, p1y = srcNode.y, p2y = tgtNode.y, p3y = tgtNode.y;
                    const u = 1 - t;
                    return u*u*u*p0y + 3*u*u*t*p1y + 3*u*t*t*p2y + t*t*t*p3y;
                };
            })
            .attr("opacity", 0)
            .remove();
    }

    // ── Tooltip ──
    function showTooltip(event, d) {
        const avgDuration = d.recentEvents.length > 0
            ? (d.recentEvents.reduce((sum, ev) => sum + ev.durationMs, 0) / d.recentEvents.length).toFixed(1)
            : "—";

        let html = `<div class="tt-title">${d.source.className} → ${d.target.className}</div>`;
        html += `<div class="tt-row"><span class="tt-label">Requests (window)</span><span class="tt-value">${d.recentCount}</span></div>`;
        html += `<div class="tt-row"><span class="tt-label">Total calls</span><span class="tt-value">${d.totalCount}</span></div>`;
        html += `<div class="tt-row"><span class="tt-label">Avg duration</span><span class="tt-value">${avgDuration} ms</span></div>`;
        html += `<div class="tt-row"><span class="tt-label">Errors (window)</span><span class="tt-value" style="color:${d.recentErrorCount > 0 ? '#f85149' : '#2ea043'}">${d.recentErrorCount}</span></div>`;

        if (d.recentEvents.length > 0) {
            const last = d.recentEvents[d.recentEvents.length - 1];
            if (last.isError && last.errorMessage) {
                html += `<div class="tt-error">${escapeHtml(last.errorMessage)}</div>`;
            }
        }

        tooltip.html(html)
            .classed("hidden", false)
            .style("left", (event.pageX + 12) + "px")
            .style("top", (event.pageY - 12) + "px");
    }

    function escapeHtml(text) {
        const div = document.createElement("div");
        div.textContent = text;
        return div.innerHTML;
    }

    // ── Controls ──
    btnClear.addEventListener("click", () => {
        events.length = 0;
        nodes.clear();
        edges.clear();
        selectedCorrelation = null;
        gNodes.selectAll("*").remove();
        gEdges.selectAll("*").remove();
        gParticles.selectAll("*").remove();
        eventCountEl.textContent = "0 events";
    });

    timeWindowSlider.addEventListener("input", () => {
        TIME_WINDOW_SEC = parseInt(timeWindowSlider.value, 10);
        windowLabel.textContent = `${TIME_WINDOW_SEC}s`;
        render();
    });

    errorsOnlyCheckbox.addEventListener("change", () => render());

    svg.on("click", () => {
        selectedCorrelation = null;
        render();
    });

    // Periodic re-render to age out old events
    setInterval(() => render(), 2000);

    // ── Resize ──
    window.addEventListener("resize", () => {
        const rect = svg.node().getBoundingClientRect();
        width = rect.width;
        height = rect.height;
        svg.attr("viewBox", `0 0 ${width} ${height}`);
        gLayers.selectAll("*").remove();
        drawLayerLabels();
        // Recalculate node positions
        recalcNodePositions();
        render();
    });

    function recalcNodePositions() {
        const colW = width / LAYERS.length;
        const layerCounts = {};
        for (const node of nodes.values()) {
            if (!layerCounts[node.layer]) layerCounts[node.layer] = 0;
            const layerIdx = LAYERS.indexOf(node.layer);
            node.x = colW * layerIdx + colW / 2;
            node.y = 70 + layerCounts[node.layer] * 60;
            layerCounts[node.layer]++;
        }
    }

    // ── SignalR ──
    const connection = new signalR.HubConnectionBuilder()
        .withUrl(HUB_URL)
        .withAutomaticReconnect([0, 1000, 2000, 5000, 10000])
        .build();

    connection.on("ReceiveFlowEvent", (ev) => {
        processEvent(ev);
    });

    connection.onreconnecting(() => {
        statusEl.textContent = "Reconnecting...";
        statusEl.className = "disconnected";
    });

    connection.onreconnected(() => {
        statusEl.textContent = "Connected";
        statusEl.className = "connected";
    });

    connection.onclose(() => {
        statusEl.textContent = "Disconnected";
        statusEl.className = "disconnected";
    });

    async function start() {
        initSvg();
        try {
            await connection.start();
            statusEl.textContent = "Connected";
            statusEl.className = "connected";
            console.log("FlowVisualizer connected to hub");
        } catch (err) {
            statusEl.textContent = "Disconnected";
            statusEl.className = "disconnected";
            console.error("FlowVisualizer connection error:", err);
            setTimeout(start, 3000);
        }
    }

    start();
})();
