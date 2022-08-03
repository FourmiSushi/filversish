using Tomlyn;

namespace filversish.Utils;

public static class CommandLine
{
    private static Dictionary<string, Action<string[]>> _commands = new()
    {
        { "help", Help },
        { "init", Init },
        { "new", NewPost },
        { "build", Build }
    };

    public static void InvokeCommand(string[] args)
    {
        if (args.Length == 0)
        {
            _commands["help"](args);
        }
        else
        {
            _commands[args[0]](args[1..]);
        }
    }

    public static bool IsValidFileName(string name)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        return name.Any(c => !invalidChars.Contains(c));
    }

    public static bool IsFilversishDir(string path)
    {
        var f = new RealFileAccess();

        var configExist = f.IsExist($"{path}/config.toml");
        return configExist;
    }

    public static void Build(string[] args)
    {
        var path = args.Length == 0 ? "." : args[0];

        Console.WriteLine("Building...");
        if (!IsFilversishDir(path))
        {
            Console.WriteLine($"{path} is not filversish directory.");
            return;
        }

        var f = new RealFileAccess();
        var c = Configuration.GetConfig(path);

        var pub = new Publisher(f, c);
        var posts = new PostGenerator(c, f).Generate();

        var tagListGenerator = new TagListGenerator(c, f);
        var tagList = tagListGenerator.Generate(posts);
        var tags = tagListGenerator.GetTags(posts);

        var pageGenerator = new PageGenerator(c, f);
        var pages = new List<Page>();
        pages.AddRange(pageGenerator.Generate(posts, null));
        foreach (var tag in tags)
        {
            pages.AddRange(pageGenerator.Generate(posts, tag));
        }

        pub.Publish(posts, pages, tagList);
        Console.WriteLine("Completed.");
    }

    public static void Init(string[] args)
    {
        var path = args.Length == 0 ? "." : args[0];

        var f = new RealFileAccess();
        var defaultConfig = Configuration.Default;
        f.WriteFile($"{path}/config.toml", Toml.FromModel(defaultConfig));
        Console.WriteLine($"Created {path}/config.toml.");
    }

    public static void NewPost(string[] args)
    {
        var name = args[0];

        if (!IsValidFileName(name))
        {
            Console.WriteLine("invalid file name.");
            return;
        }

        var f = new RealFileAccess();
        var c = Configuration.GetConfig();

        var defaultMeta = PostGenerator.PostMetaModel.GetDefault(name, c);

        f.WriteFile($"./{c.PostsPath}/{name}.md", "");
        f.WriteFile($"./{c.PostsPath}/{name}.toml", Toml.FromModel(defaultMeta));
        Console.WriteLine($"Created ./{c.PostsPath}/{name}.md, ./{c.PostsPath}/{name}.toml");
    }

    public static void Help(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine(
                @"Usage:
    filversish <command>

Available Commands:
    init      Initialize filversish directory.
    new       Create a new post.
    build     Build the directory using config.toml.
    help      Show help for commands.");
            return;
        }

        switch (args[0])
        {
            case "build":
                Console.WriteLine(
                    @"Usage:
    filversish build [dir]

Build specified directory using config.toml in it.
If 'dir' is null, filversish builds current directory.");
                break;
            case "init":
                Console.WriteLine(
                    @"Usage:
    filversish init [dir]

Create template filversish site to specified directory.");
                break;
            case "new":
                Console.WriteLine(
                    @"Usage:
    filversish new <post-title>

Create a new post in current project.");
                break;
        }
    }
}