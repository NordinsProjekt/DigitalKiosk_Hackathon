export class ShoppingCart {
    constructor() {
        this.items = [];
    }

    addProduct(product) {
        const existing = this.items.find(i => i.productId === product.id);

        if (existing) {
            existing.quantity++;
        } else {
            this.items.push({
                productId: product.id,
                name: product.name,
                price: product.price,
                quantity: 1
            });
        }
    }

    removeProduct(productId) {
        const item = this.items.find(i => i.productId === productId);

        if (!item) return;

        if (item.quantity > 1) {
            item.quantity--;
        } else {
            this.items = this.items.filter(i => i.productId !== productId);
        }
    }

    clear() {
        this.items = [];
    }
}