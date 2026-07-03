using FileReportApp;

// Base module work: create/verify the sales data directory
string salesDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "SalesData");
salesDirectory = Path.GetFullPath(salesDirectory);

if (!Directory.Exists(salesDirectory))
{
    Console.WriteLine($"Sales directory not found: {salesDirectory}");
    return;
}

Console.WriteLine($"Reading sales files from: {salesDirectory}");
foreach (var file in Directory.GetFiles(salesDirectory, "*.txt"))
{
    Console.WriteLine($" - {Path.GetFileName(file)}");
}

// Additional function required by the assignment
string reportPath = Path.Combine(salesDirectory, "..", "SalesSummary.txt");
reportPath = Path.GetFullPath(reportPath);

SalesReportGenerator.GenerateSalesSummaryReport(salesDirectory, reportPath);

Console.WriteLine();
Console.WriteLine($"Report written to: {reportPath}");
Console.WriteLine();
Console.WriteLine(File.ReadAllText(reportPath));
