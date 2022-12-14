using System.IO;
using Sylvan.Data.Csv;

namespace csvcat
{
    public class ParseCsv
    {
        private string _filename;
        private int _lines;
        private bool _tail;
        private char _delimiter;
        public List<string> Header { get; set; } = new List<string>();
        public List<List<string>> CsvLines { get; private set; } = new List<List<string>>();

        public ParseCsv(string fileName, int lines, bool tail, char delimiter)
        {
            _filename = fileName;
            _lines = lines;
            _tail = tail;
            _delimiter = delimiter;
        }

        public void ReadCsvFile()
        {
            string[] csvLines = new string[_lines];

            try
            {
                var csvOpts = new CsvDataReaderOptions { Delimiter = _delimiter };
                using CsvDataReader csv = CsvDataReader.Create(new StringReader(GetFileLines()), csvOpts);

                GetHeaders();

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
        }

        private void GetHeaders()
        {
            var headerRow = string.Join("\n", File.ReadLines(_filename).Take(1));
            var csvOpts = new CsvDataReaderOptions { Delimiter = _delimiter };

            using CsvDataReader csv = CsvDataReader.Create(new StringReader(headerRow), csvOpts);

            for (int i = 0; i < csv.FieldCount; i++)
                Header.Add(csv.GetName(i));
        }

        private List<string> GetCsvFields(CsvDataReader line)
        {
            var CsvValues = new List<string>();

            for (int i = 0; i < line.FieldCount; i++)
                CsvValues.Add(line.GetString(i));

            return CsvValues.ToList();
        }

        private string GetFileLines()
        {
            string lines;

            // take extra line to account for header
            if (_tail)
                lines = string.Join("\n", File.ReadLines(_filename).TakeLast(_lines + 1));
            else
                lines = string.Join("\n", File.ReadLines(_filename).Take(_lines + 1));

            return lines;
        }
    }
}
