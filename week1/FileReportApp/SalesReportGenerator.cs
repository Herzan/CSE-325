using System.Text;

namespace FileReportApp;

/// <summary>
/// Additional function required by the "Work with files and directories in a
/// .NET app" module: reads every sales file in a directory, totals the
/// figures, and writes a plain-text Sales Summary report file.
/// </summary>
public static class SalesReportGenerator
{
    /// <summary>
    /// Reads every *.txt file in salesDirectory (one dollar amount per line),
    /// builds a Sales Summary report, and writes it to reportFilePath.
    /// </summary>
    public static void GenerateSalesSummaryReport(string salesDirectory, string reportFilePath)
    {
        var salesFiles = Directory.GetFiles(salesDirectory, "*.txt");
        var fileTotals = new Dictionary<string, decimal>();
        decimal grandTotal = 0m;

        foreach (var filePath in salesFiles)
        {
            decimal fileTotal = 0m;

            foreach (var line in File.ReadLines(filePath))
            {
                if (decimal.TryParse(line.Trim(), out var amount))
                {
                    fileTotal += amount;
                }
            }

            fileTotals[Path.GetFileName(filePath)] = fileTotal;
            grandTotal += fileTotal;
        }

        var report = new StringBuilder();
        report.AppendLine("Sales Summary");
        report.AppendLine("--------------------------");
        report.AppendLine($"Total Sales: {grandTotal:C}");
        report.AppendLine();
        report.AppendLine("Details:");

        foreach (var entry in fileTotals)
        {
            report.AppendLine($"{entry.Key}: {entry.Value:C}");
        }

        File.WriteAllText(reportFilePath, report.ToString());
    }
}
