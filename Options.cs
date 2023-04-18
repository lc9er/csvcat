using CommandLine;
using CommandLine.Text;

namespace csvcat;

public class Options
{
    [Value(0, MetaName = "csv file", Required = true)]
    public string Filename { get; set; }

    [Option('n', "number", Default = 10, HelpText = "Number of lines. Default = 10")]
    public int Lines { get; set; }

    [Option('t', "tail", Default = false, HelpText = "Tail csv file")]
    public bool Tail { get; set; }

    [Option('d', "delimiter", Default = ',', HelpText = "Delimiting character. Default = ','")]
    public char Delimiter { get; set; }

    [Usage(ApplicationAlias = "csvcat")]
    public static IEnumerable<Example> Examples
    {
        get
        {
            yield return new Example("cat csv file", new Options { Filename = "file.csv", Lines = 20});
            yield return new Example("tail csv file", new Options { Filename = "file.csv", Lines = 30, Tail = true, Delimiter = '|' });
        }
    }
}
