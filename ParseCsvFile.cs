using System.IO;
using System.Linq;
using Sylvan.Data.Csv;

namespace csvcat;

public class ParseCsv
{
    private string _filename;
    private int    _lines;
    private bool   _tail;
    private char   _delimiter;
    // Make composable with their own classes. Pass these to output, instead of entire ParseCsv Obj
    public List<string> Header { get; set; } = new ();
    public List<List<string>> CsvLines { get; private set; } = new ();

    public ParseCsv(string fileName, int lines, bool tail, char delimiter)
    {
        _filename  = fileName;
        _lines     = lines;
        _tail      = tail;
        _delimiter = delimiter;
    }

    public ParseCsv GetHeaders()
    {
        var headerRow = JoinLines(_filename, 1);
        var csvOpts = new CsvDataReaderOptions { Delimiter = _delimiter };

        using CsvDataReader csv = CsvDataReader.Create(new StringReader(headerRow), csvOpts);

        for (int i = 0; i < csv.FieldCount; i++)
            Header.Add(csv.GetName(i));

        return this;
    }

    public ParseCsv GetCsvLines()
    {
        try
        {
            var csvOpts = new CsvDataReaderOptions { Delimiter = _delimiter };
            using CsvDataReader csv = CsvDataReader.Create(new StringReader(GetFileLines()), csvOpts);

            while (csv.Read())
            {
                List<string> currentLine = GetCsvFields(csv);
                CsvLines.Add(currentLine);
            }
        }
        catch (FileNotFoundException ex)
        {
            Console.WriteLine(ex.Message);
            Environment.Exit(1);
        }

        return this;
    }

    public void Sort(int? index)
    {
        if (index.HasValue)
        {
            // Test for numeric or alpha chars in field
            bool result = int.TryParse(CsvLines[0][index.Value], out _);

            CsvLines = 
                result == true ? CsvLines.OrderBy(lst => int.Parse(lst[index.Value])).ToList()
                : CsvLines.OrderBy(lst => lst[index.Value]).ToList();
        }
    }

    private static List<string> GetCsvFields(CsvDataReader line) =>
        Enumerable.Range(0, line.FieldCount)
            .Select(x => line.GetString(x))
            .ToList();

    private string GetFileLines() =>
        // take extra line to account for header
        _tail == true ? JoinLastLines(_filename, _lines + 1) 
            : JoinLines(_filename, _lines + 1); 

    private string JoinLastLines(string fileName, int linesCount) =>
        string.Join(Environment.NewLine, File.ReadLines(fileName).TakeLast(linesCount));

    private string JoinLines(string fileName, int linesCount) =>
        string.Join(Environment.NewLine, File.ReadLines(fileName).Take(linesCount));

}
