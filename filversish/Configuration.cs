// ReSharper disable UnusedAutoPropertyAccessor.Global

using filversish.Utils;
using Tomlyn;

#pragma warning disable CS8618
namespace filversish;

public class Configuration
{
    public string ThemePath { get; set; }
    public string PostsPath { get; set; }
    public string DestPath { get; set; }
    public string AssetsPath { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Author { get; set; }
    public string DefaultTag { get; set; }
    public string Host { get; set; }

    public static Configuration GetConfig(string path = ".")
    {
        if (!CommandLine.IsFilversishDir(path))
        {
            throw new Exception($"{path} is not filversish directory.");
        }

        var f = new RealFileAccess();
        var c = Toml.ToModel<Configuration>(f.ReadFile($"{path}/config.toml"));

        return c;
    }

    public static Configuration Default => new Configuration
    {
        ThemePath = "./theme",
        PostsPath = "./posts",
        DestPath = "./dest",
        AssetsPath = "./assets",
        Title = "untitled blog",
        Description = "description of the site",
        Author = Environment.UserName,
        DefaultTag = "untagged",
        Host = "example.com"
    };
}