document.addEventListener("DOMContentLoaded", function () {
    const canvas = document.getElementById("store-map");
    const ctx = canvas.getContext("2d");
    const sections = loadSections();
    sections.forEach((section, index) => {
        drawSection(ctx, section, index);
    });
});

function drawSection(ctx, section, index) {
    ctx.fillStyle = "green";
    const canvasWidth = ctx.canvas.width;
    const canvasHeight = ctx.canvas.height;
    const w = (canvasWidth - 80) / 8 - 20;
    const h = (canvasHeight - 80) / 3 - 20;
    const x = (index % 8) * (w + 20) + 40;
    const y = Math.floor(index / 8) * (h + 20) + 40;
    ctx.fillRect(x, y, w, h);
    ctx.fillStyle = "black";
    ctx.font = "16px sans-serif";
    const textHeight = 32;
    const cx = x + w / 2;
    const cy = y + h / 2;
    const splitText = section.split(" ");
    ctx.textAlign = "center";
    ctx.textBaseline = "middle";
    splitText.forEach((text, i) => {
        const offsetIndex = i - (splitText.length - 1) / 2;
        const textY = offsetIndex * textHeight + cy;
        ctx.fillText(text, cx, textY);
    });
}

function loadSections() {
    return [
        "Fruits and Vegetables",
        "Dairy",
        "Meat",
        "Fish",
        "Deli",
        "Bread",
        "Beverages",
        "Frozen",
        "Grocery",
        "Pantry",
        "Candy",
        "Ice Cream",
        "Baking",
        "Spices",
        "Canned Goods",
        "Pasta",
        "Breakfast",
        "Coffee",
        "Household",
        "Kitchenware",
        "Hygiene",
        "Pets",
        "Baby",
        "Other",
    ];
}
