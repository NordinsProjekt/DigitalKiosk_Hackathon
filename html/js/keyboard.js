document.addEventListener("DOMContentLoaded", function () {
  const keyboardContainer = document.querySelector(".keyboard-containter");
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
    key.addEventListener("mousedown", (e) => {
      handleKeyboardInput(e);
    });
    key.addEventListener("touchstart", (e) => {
      handleKeyboardInput(e);
    });
  });
  function handleKeyboardInput(event) {
    event.preventDefault();
    const char = event.target.dataset.char;
    if (char === "Backspace") {
      activeInput.value = activeInput.value.slice(0, -1);
      console.log(activeInput.value);

      return;
    }
    activeInput.value += char;
    console.log(activeInput.value);
  }
});
