using Spectre.Console;

namespace csvcat;

public class OutputTable
{
    public static void PrintTable(ParseCsv CsvLines)
    {
        Table table = new()
        {
            Border = TableBorder.Simple
        };
        table.AddColumns(CsvLines.Header.ToArray());

        for (int i = 0; i < CsvLines.CsvLines.Count; i++)
        {
            if (i % 2 == 0)
                table.AddRow(FormatTableRow("[blue]", CsvLines.CsvLines[i]));
            else
                table.AddRow(FormatTableRow("[orange1]", CsvLines.CsvLines[i]));
        }

        AnsiConsole.Write(table);
    }
    private static Markup[] FormatTableRow(string color, List<string> row)
    {
        int rowSize = row.Count;
        var formattedRow = new Markup[rowSize];

        for (int i = 0; i < rowSize; i++)
            formattedRow[i] = new Markup(color + Markup.Escape(row[i]) + "[/]");

        return formattedRow;
    }
}
