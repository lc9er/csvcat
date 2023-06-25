using Sylvan.Data.Csv;

namespace csvcat;

public class ParseCsv
{
    private string _filename;
    private int    _lines;
    private bool   _tail;
    private char   _delimiter;
    private int?   _sort;
    private bool   _reverse;
    public List<string> Header { get; set; } = new();
    public List<List<string>> CsvLines { get; private set; } = new();

    public ParseCsv(string fileName, int lines, bool tail, char delimiter, int? sort, bool reverse)
    {
        _filename  = fileName;
        _lines     = lines;
        _tail      = tail;
        _delimiter = delimiter;
        _sort      = sort;
        _reverse   = reverse;
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

    public ParseCsv VerifyFile()
    {
        if (!File.Exists(_filename))
        {
            Console.WriteLine($"File: {_filename} does not exist");
            Environment.Exit(1);
        }

        return this;
    }

    public ParseCsv GetCsvLines()
    {
        var csvOpts = new CsvDataReaderOptions { Delimiter = _delimiter };
        using CsvDataReader csv = CsvDataReader.Create(new StringReader(GetFileLines()), csvOpts);

        while (csv.Read())
        {
            List<string> currentLine = GetCsvFields(csv);
            CsvLines.Add(currentLine);
        }

        return this;
    }

    public ParseCsv Sort()
    {
        if (_sort.HasValue)
        {
            // Test for numeric or alpha chars in field
            bool result = int.TryParse(CsvLines[0][_sort.Value], out _);

            CsvLines = 
                result == true ? CsvLines.OrderBy(lst => int.Parse(lst[_sort.Value])).ToList()
                : CsvLines.OrderBy(lst => lst[_sort.Value]).ToList();
        }

        return this;
    }

    public ParseCsv ReverseSort()
    {
        if (_reverse)
            CsvLines.Reverse();

        return this;
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
