// Products Page JavaScript

function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

document.addEventListener('DOMContentLoaded', function() {
    loadProducts();
});

function loadProducts() {
    const productsList = document.getElementById('products-list');
    const errorMessage = document.getElementById('error-message');
    
    // Show loading state
    productsList.innerHTML = '<div class="loading">Loading products...</div>';
    
    apiService.getAllProducts()
        .then(products => {
            displayProducts(products);
        })
        .catch(error => {
            productsList.innerHTML = '';
            errorMessage.innerHTML = `
                <div class="error-message">
                    <p><strong>Unable to load products.</strong></p>
                    <p>Error: ${error.message}</p>
                    <p>Make sure the API is running at https://localhost:5001</p>
                    <p>Check the browser console (F12) for more details.</p>
                </div>
            `;
            if (error && error.stack) {
                console.error('Full error details (stack):', error.stack);
            } else {
                console.error('Error loading products:', error);
            }
        });
}

function displayProducts(products) {
    const productsList = document.getElementById('products-list');
    
    if (!products || products.length === 0) {
        productsList.innerHTML = '<p class="empty-state">No products available.</p>';
        return;
    }
    
    let html = '';
    
    products.forEach(product => {
        html += `
            <div class="product-card">
                <div class="product-info">
                    <h3 class="product-name">${escapeHtml(product.name)}</h3>
                    <p class="product-description">${escapeHtml(product.description || 'No description available')}</p>
                    <p class="product-price">$${product.price.toFixed(2)}</p>
                </div>
            </div>
        `;
    });
    
    productsList.innerHTML = `<div class="product-grid">${html}</div>`;
}
