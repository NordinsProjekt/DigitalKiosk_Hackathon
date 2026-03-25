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
  let canvasWidth = ctx.canvas.width;
  let canvasHeight = ctx.canvas.height;
  let w = (canvasWidth - 80) / 8 - 20;
  let h = (canvasHeight - 80) / 3 - 20;
  let x = (index % 8) * (w + 20) + 40;
  let y = Math.floor(index / 8) * (h + 20) + 40;
  ctx.fillRect(x, y, w, h);
  ctx.fillStyle = "black";
  ctx.font = "16px sans-serif";
  let textHeight = 32;
  let cx = x + w / 2;
  let cy = y + h / 2;
  let splitText = section.split(" ");
  ctx.textAlign = "center";
  ctx.textBaseline = "middle";
  splitText.forEach((text, i) => {
    let textY = (Math.floor(splitText.length / 2) - i) * textHeight + cy;
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
