using Spectre.Console;

namespace csvcat;

public class OutputTable
{
    const string _oddLine   = "[blue]";
    const string _evenLine  = "[orange1]";
    const string _lineBreak = "[/]";

    public static void PrintTable(CsvObj csvobj)
    {
        var header = csvobj.Header;
        var csvlines = csvobj.CsvLines;

        Table table = new()
        {
            Border = TableBorder.Simple
        };
        table.AddColumns(header.ToArray());

        // Set line colors
        Enumerable.Range(0, csvlines.Count)
            .Select((line, index) => line % 2 == 0 ? (_oddLine, index) : (_evenLine, index))
            .ToList()
            .ForEach(x => table.AddRow(FormatTableRow(x.Item1, csvlines[index: x.index])));

        AnsiConsole.Write(table);
    }

    private static Markup[] FormatTableRow(string color, List<string> row)
    {
        int rowSize = row.Count;
        var formattedRow = new Markup[rowSize];

        for (int i = 0; i < rowSize; i++)
            formattedRow[i] = new Markup(color + Markup.Escape(row[i]) + _lineBreak);

        return formattedRow;
    }
}