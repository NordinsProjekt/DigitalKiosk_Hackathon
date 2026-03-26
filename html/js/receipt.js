import { ReceiptItem } from "./models/receiptItem.js";
import { ReceiptInfo } from "./models/receiptInfo.js";

export function generateReceipt(cart) {

    const receiptItems = cart.items.map(i =>
        new ReceiptItem(i.name, i.price, i.quantity)
    );

    const totalItems = cart.items.reduce((sum, i) => sum + i.quantity, 0);

    const totalPrice = cart.items.reduce(
        (sum, i) => sum + (i.price * i.quantity),
        0
    );

    return new ReceiptInfo(receiptItems, totalItems, totalPrice);
}