using Microsoft.Playwright;

// Resolve paths
var scriptDir = AppContext.BaseDirectory;
var htmlPath = Path.GetFullPath(Path.Combine(scriptDir, "..", "..", "..", "..", "Beställardokument.html"));
var pdfPath = Path.GetFullPath(Path.Combine(scriptDir, "..", "..", "..", "..", "Beställardokument.pdf"));

if (!File.Exists(htmlPath))
{
    Console.WriteLine($"ERROR: HTML file not found at: {htmlPath}");
    return;
}

Console.WriteLine($"HTML source: {htmlPath}");
Console.WriteLine($"PDF output:  {pdfPath}");
Console.WriteLine("Launching browser...");

using var playwright = await Playwright.CreateAsync();
await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
var page = await browser.NewPageAsync();

var fileUri = new Uri(htmlPath).AbsoluteUri;
await page.GotoAsync(fileUri, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

// Small delay to let fonts load
await page.WaitForTimeoutAsync(2000);

await page.PdfAsync(new PagePdfOptions
{
    Path = pdfPath,
    Format = "A4",
    PrintBackground = true,
    Margin = new Margin
    {
        Top = "18mm",
        Bottom = "18mm",
        Left = "20mm",
        Right = "20mm"
    }
});

Console.WriteLine($"PDF successfully created: {pdfPath}");
