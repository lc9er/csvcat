using System.CommandLine;
using csvcat;

var rootCommand = MyApp.BuildRootCommand();
ParseResult parseResult = rootCommand.Parse(args);
parseResult.Invoke();