global using aoc_csharp.helper;
using aoc_csharp.helper.spectre;

// Use Spectre.Console.Cli to parse args and run the program flow.
// This improves the visual quality of the application and allows for better command-line argument handling.
// Also allows for future expansion of commands.
var spectreConsoleApp = CommandAppFactory.Create();

// Actually run the app with the provided args
spectreConsoleApp.Run(args);
