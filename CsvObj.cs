namespace csvcat;

public class CsvObj
{
    public List<string> Header { get; } = new();
    public List<List<string>> CsvLines { get; set; } = new();

    public void AddHeader(string header) => Header.Add(header);
    public void AddRow(List<string> row) => CsvLines.Add(row);
    public void Reverse() => CsvLines.Reverse();
    
    public void Sort(int sortBy)
    {
        // Test for numeric or alpha chars in field
        bool result = int.TryParse(CsvLines[0][sortBy], out _);

        CsvLines =
            result == true ? [.. CsvLines.OrderBy(lst => int.Parse(lst[sortBy]))]
            : [.. CsvLines.OrderBy(lst => lst[sortBy])];
    }
}