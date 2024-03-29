﻿using Spectre.Console;

namespace csvcat;

public class OutputTable
{
    private string _oddLine  = "[blue]";
    private string _evenLine = "[orange1]";

    public void PrintTable(List<string> header, List<List<string>>csvlines)
    {
        Table table = new()
        {
            Border = TableBorder.Simple
        };
        table.AddColumns(header.ToArray());

        // Set line colors
        Enumerable.Range(0, csvlines.Count)
            .Select((line, index) => line % 2 == 0 ? (_oddLine, index) : (_evenLine, index))
            .ToList()
            .ForEach(x => table.AddRow(FormatTableRow(x.Item1, csvlines[index: x.Item2])));

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
