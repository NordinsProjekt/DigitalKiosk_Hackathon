 //export function ReceiptPrintReady(){  <-- byt till denna efter testning
export function getTestReceiptPrintReady() {
  
    return `
        <div style="font-family: monospace; width: 250px;">
            <h3 style="text-align:center;">Anderssons Livs</h3>
            <p style="text-align:center;">2026-03-26 14:32</p>

            <hr/>

            <div>Mjölk x2</div>
            <div>15 kr → 30 kr</div>

            <div>Bröd x1</div>
            <div>25 kr → 25 kr</div>

            <hr/>

            <strong>Total: 55 kr</strong>

            <hr/>

            <p style="text-align:center;">Tack för ditt köp!</p>
        </div>
    `;
}