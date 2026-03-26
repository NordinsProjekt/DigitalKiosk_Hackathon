document.addEventListener("DOMContentLoaded", function () {
    let activeSection = "";

    const canvas = document.getElementById("store-map");
    const ctx = canvas.getContext("2d");
    const storeSections = {
        0: { id: "FruitsAndVegetables", displayName: "Fruits & Vegetables", mapArea: { x: 0.000, y: 0.000, w: 0.12, h: 0.32 } },
        1: { id: "Dairy", displayName: "Dairy & Eggs", mapArea: { x: 0.125, y: 0.000, w: 0.12, h: 0.32 } },
        2: { id: "Meat", displayName: "Meat & Poultry", mapArea: { x: 0.250, y: 0.000, w: 0.12, h: 0.32 } },
        3: { id: "Fish", displayName: "Fish & Seafood", mapArea: { x: 0.375, y: 0.000, w: 0.12, h: 0.32 } },
        4: { id: "Deli", displayName: "Deli & Cheese", mapArea: { x: 0.500, y: 0.000, w: 0.12, h: 0.32 } },
        5: { id: "Bread", displayName: "Bread & Bakery", mapArea: { x: 0.625, y: 0.000, w: 0.12, h: 0.32 } },
        6: { id: "Beverages", displayName: "Beverages", mapArea: { x: 0.750, y: 0.000, w: 0.12, h: 0.32 } },
        7: { id: "Frozen", displayName: "Frozen Foods", mapArea: { x: 0.875, y: 0.000, w: 0.12, h: 0.32 } },
        8: { id: "Grocery", displayName: "General Grocery", mapArea: { x: 0.000, y: 0.333, w: 0.12, h: 0.32 } },
        9: { id: "Pantry", displayName: "Pantry Staples", mapArea: { x: 0.125, y: 0.333, w: 0.12, h: 0.32 } },
        10: { id: "Candy", displayName: "Candy & Snacks", mapArea: { x: 0.250, y: 0.333, w: 0.12, h: 0.32 } },
        11: { id: "IceCream", displayName: "Ice Cream", mapArea: { x: 0.375, y: 0.333, w: 0.12, h: 0.32 } },
        12: { id: "Baking", displayName: "Baking Supplies", mapArea: { x: 0.500, y: 0.333, w: 0.12, h: 0.32 } },
        13: { id: "Spices", displayName: "Spices & Seasonings", mapArea: { x: 0.625, y: 0.333, w: 0.12, h: 0.32 } },
        14: { id: "CannedGoods", displayName: "Canned Goods", mapArea: { x: 0.750, y: 0.333, w: 0.12, h: 0.32 } },
        15: { id: "Pasta", displayName: "Pasta & Rice", mapArea: { x: 0.875, y: 0.333, w: 0.12, h: 0.32 } },
        16: { id: "Breakfast", displayName: "Breakfast & Cereal", mapArea: { x: 0.000, y: 0.666, w: 0.12, h: 0.32 } },
        17: { id: "Coffee", displayName: "Coffee & Tea", mapArea: { x: 0.125, y: 0.666, w: 0.12, h: 0.32 } },
        18: { id: "Household", displayName: "Household Items", mapArea: { x: 0.250, y: 0.666, w: 0.12, h: 0.32 } },
        19: { id: "Kitchenware", displayName: "Kitchenware", mapArea: { x: 0.375, y: 0.666, w: 0.12, h: 0.32 } },
        20: { id: "Hygiene", displayName: "Health & Beauty", mapArea: { x: 0.500, y: 0.666, w: 0.12, h: 0.32 } },
        21: { id: "Pets", displayName: "Pet Care", mapArea: { x: 0.625, y: 0.666, w: 0.12, h: 0.32 } },
        22: { id: "Baby", displayName: "Baby Care", mapArea: { x: 0.750, y: 0.666, w: 0.12, h: 0.32 } },
        23: { id: "Other", displayName: "Other", mapArea: { x: 0.875, y: 0.666, w: 0.12, h: 0.32 } }
    };

    const sectionButtons = document.querySelectorAll(".ListedItems");

    sectionButtons.forEach(button => {
        button.addEventListener('click', ()=> {
            const sectionId = button.getAttribute("data-section");
            updateActiveSection(sectionId);
        })
    })

    drawMap();

    function drawMap() {
        Object.values(storeSections).forEach(section => {
            drawSection(ctx, section);
        });
    }

    function drawSection(ctx, section, index) {
        const canvasWidth = ctx.canvas.width;
        const canvasHeight = ctx.canvas.height;
        const w = canvasWidth * section.mapArea.w;
        const h = canvasWidth * section.mapArea.h;
        const x = canvasWidth * section.mapArea.x;
        const y = canvasWidth * section.mapArea.y;
        if (section.id == activeSection) {
            ctx.fillStyle = "green";
        } else {
            ctx.fillStyle = "gray";
        }
        ctx.fillRect(x, y, w, h);
        ctx.fillStyle = "black";
        ctx.font = "14px sans-serif";
        const textHeight = 14;
        const cx = x + w / 2;
        const cy = y + h / 2;
        const splitText = section.displayName.split(" ");
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        splitText.forEach((text, i) => {
            const offsetIndex = i - (splitText.length - 1) / 2;
            const textY = offsetIndex * textHeight + cy;
            ctx.fillText(text, cx, textY);
        });
    }

    function updateActiveSection(sectionId) {
        activeSection = sectionId;
        drawMap();
    }
});

