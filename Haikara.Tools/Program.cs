using ConsoleAppFramework;
using Haikara.Tools;
using Haikara.Tools.Releaser;

var app = ConsoleApp.Create();
app.Add<SourceGeneratorBuildCommand>();
app.Add<VersionUpdateCommand>();
app.Run(args);