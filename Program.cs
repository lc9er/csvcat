using CommandLine;
using csvcat;

var parserResults = new CommandLine.Parser(with => with.HelpWriter = null)
                            .ParseArguments<Options>(args);
parserResults
    .WithParsed<Options>(opts =>
        {
            Run(opts);
        })
    .WithNotParsed(errs => Options.DisplayHelp(parserResults, errs));

static void Run(Options opts)
{
    ParseCsv catLines = new(opts);
    catLines
        .VerifyFile()
        .GetHeaders()
        .GetCsvLines()
        .Sort()
        .ReverseSort();

    // display result table
    var outTable = new OutputTable();
    outTable.PrintTable(catLines.Header, catLines.CsvLines);
}