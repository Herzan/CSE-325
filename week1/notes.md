# W01 Notes — Build .NET Applications with C#

## Part 1: Pizzas Web API (ASP.NET Core Controllers)

**Additional record added:** `Id: 3, Name: "Veggie Feast", IsGlutenFree: true`
(see `PizzaApi/Controllers/PizzasController.cs`, seeded list)

### Example requests / responses

**GET /Pizzas** — list all (existing + new record)
```
Request:  GET https://localhost:{port}/Pizzas
Response: 200 OK
[
  { "id": 1, "name": "Classic Italian", "isGlutenFree": false },
  { "id": 2, "name": "Seafood Delight", "isGlutenFree": false },
  { "id": 3, "name": "Veggie Feast",    "isGlutenFree": true  }
]
```

**POST /Pizzas** — create a pizza
```
Request:  POST https://localhost:{port}/Pizzas
Body:     { "name": "Pepperoni Classic", "isGlutenFree": false }
Response: 201 Created
Location: /Pizzas/4
{ "id": 4, "name": "Pepperoni Classic", "isGlutenFree": false }
```

**PUT /Pizzas/4** — update a pizza
```
Request:  PUT https://localhost:{port}/Pizzas/4
Body:     { "name": "Pepperoni Deluxe", "isGlutenFree": false }
Response: 204 No Content
```

**DELETE /Pizzas/4** — remove a pizza
```
Request:  DELETE https://localhost:{port}/Pizzas/4
Response: 204 No Content
```

> Run with `dotnet run` inside `PizzaApi/`, then exercise these with a REST
> client (Postman, VS Code REST Client, `curl`) to capture real
> screenshots for submission — the responses above show the expected
> shape/status codes. The project has no external NuGet dependencies, so
> `dotnet run` works immediately with no `dotnet restore` needed.

## Part 2: Sales Summary Report Function

Added `SalesReportGenerator.GenerateSalesSummaryReport()` in
`FileReportApp/SalesReportGenerator.cs`, which reads every `*.txt` sales
file in a directory, sums each file's amounts and a grand total using a
`StringBuilder`, and writes the report to disk.

```csharp
using System.Text;

namespace FileReportApp;

public static class SalesReportGenerator
{
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
```

### Sample output (SalesSummary.txt) with the included sample data

```
Sales Summary
--------------------------
Total Sales: $6,677.29

Details:
store1.txt: $1,668.85
store2.txt: $2,749.99
store3.txt: $2,258.45
```
