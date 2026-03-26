// Products Page JavaScript

function escapeHtml(text) {
  const div = document.createElement("div");
  div.textContent = text;
  return div.innerHTML;
}

document.addEventListener("DOMContentLoaded", function () {
  loadProducts();

  const searchBar = document.getElementById("myInput");
  searchBar.addEventListener("input", () => {
    loadProductsByName(searchBar.value);
  });
});

function loadProductsByName(name) {
  const productsList = document.getElementsByClassName("product-grid")[0];
  const errorMessage = document.getElementById("error-message");

  productsList.innerHTML = '<div class="loading">Loading products...</div>';
  apiService
    .getAllProducts()
    .then((products) => {
      products = products.filter((product) =>
        product.name.toUpperCase().includes(name.toUpperCase()),
      );
      displayProducts(products);
    })
    .catch((error) => {
      productsList.innerHTML = "";
      errorMessage.innerHTML = `
                <div class="error-message">
                    <p><strong>Unable to load products.</strong></p>
                    <p>Error: ${error.message}</p>
                    <p>Make sure the API is running at https://localhost:5001</p>
                    <p>Check the browser console (F12) for more details.</p>
                </div>
            `;
      if (error && error.stack) {
        console.error("Full error details (stack):", error.stack);
      } else {
        console.error("Error loading products:", error);
      }
    });
}

function loadProducts() {
  const productsList = document.getElementsByClassName("product-grid")[0];
  const errorMessage = document.getElementById("error-message");

  // Show loading state
  productsList.innerHTML = '<div class="loading">Loading products...</div>';

  apiService
    .getAllProducts()
    .then((products) => {
      displayProducts(products);
    })
    .catch((error) => {
      productsList.innerHTML = "";
      errorMessage.innerHTML = `
                <div class="error-message">
                    <p><strong>Unable to load products.</strong></p>
                    <p>Error: ${error.message}</p>
                    <p>Make sure the API is running at https://localhost:5001</p>
                    <p>Check the browser console (F12) for more details.</p>
                </div>
            `;
      if (error && error.stack) {
        console.error("Full error details (stack):", error.stack);
      } else {
        console.error("Error loading products:", error);
      }
    });
}

function displayProducts(products) {
  const productsList = document.getElementsByClassName("product-grid")[0];

  if (!products || products.length === 0) {
    productsList.innerHTML =
      '<p class="empty-state">No products available.</p>';
    return;
  }

  let html = "";

  products.slice(-6).forEach((product) => {
    html += `
        <article class="product-card">
            <img src="https://picsum.photos/300/150" alt="Produktbild" />
            <div class="product-info">
              <h2 class="product-name">${escapeHtml(product.name)}</h2>
              <p class="product-price">${product.price.toFixed(2)} kr</p>
              <section class="product-button">
                <button data-product-id="${product.id}" class="product-button show-button">Visa produkt</button>
                <button class="add-button">Lägg till</button>
              </section>
            </div>
        </article>
        `;
  });

  productsList.innerHTML = html;

  const detailsButtons = document.querySelectorAll(".product-button");

  detailsButtons.forEach((button) => {
    button.addEventListener("pointerdown", (e) => {
      const productId = button.getAttribute("data-product-id");
      showProductDetails(productId);
    });
  });
  function showProductDetails(productId) {
    const productDetails = document.getElementsByClassName("productview")[0];

    apiService
      .getProductById(productId)
      .then((product) => {
        productDetails.innerHTML = `
            <article class="productview">
                  <div id="product-overlay" class="product-detail-overlay">
                    <div class="detail-container">
                      <button class="back-button" onclick="hideProductView()">← Tillbaka</button>
                      <div id="detail-content">
                        <img id="detail-image" src="https://picsum.photos/300/150" alt="Produktbild" />
                        <h2 id="detail-name">${escapeHtml(product.name)}</h2>
                        <p id="detail-description">${escapeHtml(product.description || "No description available")}</p>
                        <p id="ShelfLocation">Hyllaplats: ${escapeHtml(product.shelfLocation || "No shelf location available")}</p>
                        <p id="Section">Avdelning: ${escapeHtml(product.section || "No section available")}</p>
                        <p id="detail-price">Pris: ${escapeHtml(product.price.toFixed(2))} kr</p>
                      </div>
                    </div>
                </div>
            </article>
        `;
      })
      .catch((error) => {
        productDetails.innerHTML = `
        <div class="error-message">Det finns ingen tillgänglig information för denna produkt.</div>
        `;
        if (error && error.stack) {
          console.error("Full error details (stack):", error.stack);
        } else {
          console.error("Error loading products:", error);
        }
      });
    productDetails.style.display = "block";
  }
}

function hideProductView() {
  document.querySelector(".productview").style.display = "none";
}
