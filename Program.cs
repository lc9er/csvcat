using System.Linq;
using CommandLine;
using CommandLine.Text;
using System.ComponentModel.Design;

namespace csvcat;

class Program
{
    static void Main(string[] args)
    {
        var parserResults = new CommandLine.Parser(with => with.HelpWriter = null)
                                    .ParseArguments<Options>(args);
        parserResults
            .WithParsed<Options>(opts =>
                {
                    Run(opts.Filename, opts.Lines, opts.Tail, opts.Sort, opts.Delimiter);
                })
            .WithNotParsed(errs => Options.DisplayHelp(parserResults, errs));
    }

    static void Run(string fileName, int lines, bool tail, int? sort, char delimiter)
    {
        var catLines = new ParseCsv(fileName, lines, tail, delimiter);
        catLines.ReadCsvFile();

        if (sort.HasValue)
            catLines.Sort(sort);

        // display result table
        var outTable = new OutputTable();
        outTable.PrintTable(catLines);
    }
}