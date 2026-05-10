using Sylvan.Data.Csv;

namespace csvcat;

public class ParseCsv(Options opts)
{
    private readonly string _filename = opts.Filename;
    private readonly int _lines       = opts.Lines;
    private readonly bool _tail       = opts.Tail;
    private readonly char _delimiter  = opts.Delimiter;
    private readonly int? _sort       = opts.Sort;
    private readonly bool _reverse    = opts.Reverse;
    public  CsvObj csvObj             = new ();

    public ParseCsv GetHeaders()
    {
        var headerRow = JoinLines(_filename, 1);
        var csvOpts = new CsvDataReaderOptions { Delimiter = _delimiter };

        using CsvDataReader csv = CsvDataReader.Create(new StringReader(headerRow), csvOpts);

        for (int i = 0; i < csv.FieldCount; i++)
            csvObj.AddHeader(csv.GetName(i));

        return this;
    }

    public ParseCsv VerifyFile()
    {
        if (!File.Exists(_filename))
        {
            Console.WriteLine($"File: {_filename} not found.");
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
            csvObj.AddRow(currentLine);
        }

        return this;
    }

    public ParseCsv Sort()
    {
        if (_sort.HasValue)
            csvObj.Sort(_sort.Value);

        return this;
    }

    public ParseCsv ReverseSort()
    {
        if (_reverse)
            csvObj.Reverse();

        return this;
    }

    private static List<string> GetCsvFields(CsvDataReader line) =>
        [.. Enumerable.Range(0, line.FieldCount).Select(x => line.GetString(x))];

    private string GetFileLines()
    {
        // take extra line to account for header
        if (_lines > 0)
            return _tail ? JoinLastLines(_filename, _lines + 1) : JoinLines(_filename, _lines + 1);
        else
            return _tail ? JoinLastLines(_filename) : JoinLines(_filename);
    }

    private static string JoinLastLines(string fileName, int linesCount) =>
        string.Join(Environment.NewLine,
            File.ReadLines(fileName).TakeLast(linesCount));

    private static string JoinLastLines(string fileName) =>
        string.Join(Environment.NewLine, File.ReadLines(fileName));

    private static string JoinLines(string fileName, int linesCount) =>
        string.Join(Environment.NewLine,
            File.ReadLines(fileName).Take(linesCount));

    private static string JoinLines(string fileName) =>
        string.Join(Environment.NewLine, File.ReadLines(fileName));
}