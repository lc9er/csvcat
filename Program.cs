using CommandLine;
using CommandLine.Text;
using Spectre.Console;

namespace csvcat
{
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new CommandLine.Parser(with => with.HelpWriter = null);
            var parserResults = parser.ParseArguments<Options>(args);
            parserResults
                .WithParsed<Options>(opts =>
                    {
                        Run(opts.Filename, opts.Lines, opts.Tail, opts.Delimiter);
                    })
                .WithNotParsed(errs => DisplayHelp(parserResults, errs));
        }

        static void Run(string fileName, int lines, bool tail, char delimiter)
        {
            var catLines = new ParseCsv(fileName, lines, tail, delimiter);
            catLines.ReadCsvFile();

            // display result table
            OutputTable table = new();
            OutputTable.PrintTable(catLines);
        }

        static void DisplayHelp<T>(ParserResult<T> result, IEnumerable<Error> errs)
        {
            var helpText = HelpText.AutoBuild(result, h =>
            {
                h.AdditionalNewLineAfterOption = false;
                h.Heading = "csvcat 0.0.1";
                h.Copyright = "Copyright (c) 2022 lc9er";
                return HelpText.DefaultParsingErrorsHandler(result, h);
            }, e => e);
            Console.WriteLine(helpText);
        }
    }
}