// API Configuration
const API_BASE_URL = 'http://localhost:5155/api';

// API Service Object
const apiService = {
    /**
     * Fetch all products from the API
     * @returns {Promise<Array>} Array of product objects
     */
    async getAllProducts() {
        try {
            const response = await fetch(`${API_BASE_URL}/products`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const products = await response.json();
            return products;
        } catch (error) {
            console.error('Error fetching products:', error);
            throw error;
        }
    },

    /**
     * Fetch a single product by ID
     * @param {number} id - Product ID
     * @returns {Promise<Object>} Product object
     */
    async getProductById(id) {
        try {
            const response = await fetch(`${API_BASE_URL}/products/${id}`);
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const product = await response.json();
            return product;
        } catch (error) {
            console.error(`Error fetching product ${id}:`, error);
            throw error;
        }
    },

    /**
     * Create a new order
     * @param {Object} orderData - Order data with customerName and items
     * @returns {Promise<Object>} Created order response
     */
    async createOrder(orderData) {
        try {
            const response = await fetch(`${API_BASE_URL}/orders`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(orderData)
            });
            
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            
            const result = await response.json();
            return result;
        } catch (error) {
            console.error('Error creating order:', error);
            throw error;
        }
    }
};

