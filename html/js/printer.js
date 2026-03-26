export function printReceipt(htmlContent) {

    const printWindow = window.open('', '', 'width=260,height=600');

    printWindow.document.write(`
        <html>
        <head>
            <title>Kvitto</title>
            <style>
                body {
                    font-family: monospace;
                    width: 220px;
                    margin: 0;
                    padding: 5px;
                    font-size: 11px;
                }

                hr {
                    border: none;
                    border-top: 1px dashed black;
                }
            </style>
        </head>
        <body>
            ${htmlContent}
        </body>
        </html>
    `);

    printWindow.document.close();

    printWindow.onload = function () {
        printWindow.focus();
        printWindow.print();
    };
}

export function formatLine(name, total) {
    name = name.length > 18 ? name.substring(0, 18) : name;
    return name.padEnd(18) + total.toString().padStart(10);
}