document.addEventListener("DOMContentLoaded", function () {
  const keyboardContainer = document.querySelector(".keyboard-container");
  keyboardContainer.style.display = "none";
  const inputs = document.querySelectorAll("input[type='text']");
  let activeInput;

  inputs.forEach((input) => {
    input.addEventListener("focus", () => {
      keyboardContainer.style.display = "block";
      activeInput = input;
    });

    input.addEventListener("blur", () => {
      keyboardContainer.style.display = "none";
      activeInput = null;
    });
  });
  const keys = document.querySelectorAll(".key");

  keys.forEach((key) => {
    key.addEventListener("pointerdown", (e) => {
      handleKeyboardInput(e);
    });
  });
  function handleKeyboardInput(event) {
    if (!activeInput) return;
    event.preventDefault();
    const char = event.currentTarget.dataset.char;
    if (char === "Backspace") {
      activeInput.value = activeInput.value.slice(0, -1);
      return;
    }
    activeInput.value += char;
  }
});
