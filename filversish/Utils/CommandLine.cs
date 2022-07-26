using System.Globalization;
using Tomlyn;

namespace filversish.Utils;

public static class CommandLine
{
    public static void Help(string command)
    {
        switch (command)
        {
            case "build":
                Console.WriteLine(
                    @"Usage:
    filversish build [dir]

Build specified directory using config.toml in it.
If 'dir' is null, filversish builds current directory.");
                break;
            case "server":
                Console.WriteLine(
                    @"Usage:
    filversish server [dir]

Start a local server using config.toml in specified directory.");
                break;
            case "init":
                Console.WriteLine(
                    @"Usage:
    filversish init <dir>

Create template filversish site to specified directory.");
                break;
            case "new":
                Console.WriteLine(
                    @"Usage:
    filversish new <post-title>

Create a new post in current project.");
                break;
            default:
                Console.WriteLine(
                    @"Usage:
    filversish <command>

Available Commands:
    new       Create new things.
    build     Build the directory using config.toml.
    server    Build and start a local server.
    help      Show help for commands.");
                break;
        }
    }

    public static bool IsFilversishDir(string path)
    {
        var f = new RealFileAccess();

        var configExist = f.IsExist($"{path}/config.toml");
        return configExist;
    }

    public static void Build(string path = ".")
    {
        Console.WriteLine("Building...");
        if (!IsFilversishDir(path))
        {
            Console.WriteLine($"{path} is not filversish directory.");
            return;
        }

        var f = new RealFileAccess();
        var c = Configuration.GetConfig(path);

        var pub = new Publisher(f, c);
        var posts = new Post.Generator(c, f).Generate();
        var pages = new Page.Generator(c, f).Generate(posts);

        pub.Publish(posts, pages);
        Console.WriteLine("Completed.");
    }

    public static void Init(string path = ".")
    {
        var f = new RealFileAccess();
        var defaultConfig = new Configuration
        {
            ThemesPath = "./theme",
            PostsPath = "./posts",
            DestPath = "./dest",
            AssetsPath = "./assets",
            Title = "Untitled",
            Description = "description of the site",
            Author = Environment.UserName
        };
        f.WriteFile($"{path}/config.toml", Toml.FromModel(defaultConfig));
    }

    public static void StartServer(string path = ".")
    {
        if (!IsFilversishDir(path))
        {
            Console.WriteLine($"{path} is not filversish directory.");
            return;
        }

        var watcher = new FileSystemWatcher(path);
        watcher.IncludeSubdirectories = true;
        watcher.Filters.Add("posts/*");
        watcher.Filters.Add("assets/*");
        watcher.Filters.Add("theme/*");
        watcher.Filters.Add("config.toml");

        watcher.Changed += (_, _) => Build(path);
        watcher.Created += (_, _) => Build(path);
        watcher.Deleted += (_, _) => Build(path);
        watcher.Renamed += (_, _) => Build(path);

        Build(path);

        watcher.EnableRaisingEvents = true;
        var c = Configuration.GetConfig();


        LocalServer.Start(c.DestPath);
        Console.WriteLine("Server started at http://localhost:8080");
    }

    public static void NewPost(string name)
    {
        if (!IsValidFileName(name))
        {
            Console.WriteLine("invalid file name.");
            return;
        }

        var f = new RealFileAccess();
        var c = Configuration.GetConfig();

        var defaultMeta = new Post.Generator.PostMetaModel
        {
            Author = c.Author,
            PublishedAt = DateTime.Now.ToString(CultureInfo.CurrentCulture),
            Tags = Array.Empty<string>(),
            Title = name
        };

        f.WriteFile($"./{c.PostsPath}/{name}.md", "");
        f.WriteFile($"./{c.PostsPath}/{name}.toml", Toml.FromModel(defaultMeta));
    }

    private static bool IsValidFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return name.Any(c => !invalidChars.Contains(c));
    }
}