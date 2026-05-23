using System.CommandLine;

namespace csvcat;

public class MyApp
{
    public static RootCommand BuildRootCommand()
    {
        Argument<FileInfo> fileName = new("file")
        {
            Description = "File to head",
        };

        Option<int> lines = new("--lines", "-l", "-n")
        {
            Description = "Number of lines to print from start of file.",
            DefaultValueFactory = parseResult => 0,
        };

        Option<bool> tail = new("--tail", "-t")
        {
            Description = "Tail csv file",
            DefaultValueFactory = parseResult => false,
        };

        Option<int> sort = new("--sort", "-s")
        {
            Description = "Sort by filed index (0-based)",
        };

        Option<bool> reverse = new("--reverse", "-r")
        {
            Description = "Reverse results",
            DefaultValueFactory = parseResult => false,
        };

        Option<string> delimiter = new("--delimiter", "-d")
        {
            Description = "Delimiting character (wrapped in quotes)",
            DefaultValueFactory = parseResult => ",",
        };

        RootCommand rootCommand = new("cat a csv file");
        rootCommand.Arguments.Add(fileName);
        rootCommand.Options.Add(lines);
        rootCommand.Options.Add(tail);
        rootCommand.Options.Add(sort);
        rootCommand.Options.Add(reverse);
        rootCommand.Options.Add(delimiter);

        rootCommand.SetAction(parseResult =>
        {
            var file      = parseResult.GetValue(fileName);
            var linecount = parseResult.GetValue(lines);
            var tailOpt   = parseResult.GetValue(tail);
            var sortOpt   = parseResult.GetValue(sort);
            var revOpt    = parseResult.GetValue(reverse);
            var delimOpt  = parseResult.GetValue(delimiter);
            char delimChar = delimOpt[0];

            var catlines = new ParseCsv(file, linecount, tailOpt, 
                                    delimChar, sortOpt, revOpt);
            Run(catlines);
        });
    
        return rootCommand;
    }
    
    private static void Run(ParseCsv catLines)
    {
        catLines
            .VerifyFile()
            .GetHeaders()
            .GetCsvLines()
            .Sort()
            .ReverseSort();

        // display result table
        var outTable = new OutputTable();
        OutputTable.PrintTable(catLines.csvObj);
    }
}