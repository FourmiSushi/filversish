// See https://aka.ms/new-console-template for more information

// filversish

using filversish.Utils;

if (args.Length == 0)
{
    CommandLine.Help("");
}
else

// filversish help
if (args[0] == "help")
{
    CommandLine.Help(args.Length == 1 ? "" : args[1]);
}
else
// filversish build
if (args[0] == "build")
{
    if (args.Length == 1) CommandLine.Build();
    else CommandLine.Build(args[1]);
}
else

// filversish server
if (args[0] == "server")
{
    if (args.Length == 1) CommandLine.StartServer();
    else CommandLine.StartServer(args[1]);
}
else
// filversish init
if (args[0] == "init")
{
    if (args.Length == 1) CommandLine.Init();
    if (args.Length == 2)
    {
        CommandLine.Init(args[1]);
    }
}
else
// filversish new
if (args[0] == "new")
{
    if (args.Length == 1) CommandLine.Help("new");
    else CommandLine.NewPost(args[1]);
}