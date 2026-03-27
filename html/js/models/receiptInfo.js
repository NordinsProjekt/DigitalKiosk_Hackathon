export class ReceiptInfo {
    constructor(items, totalItems, totalPrice) {
        this.items = items;
        this.totalItems = totalItems;
        this.totalPrice = totalPrice;
        this.createdAt = new Date();
    }
}