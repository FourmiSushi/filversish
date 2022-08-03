using filversish.Utils;
using Scriban;

namespace filversish;

public class TagListGenerator
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;


    public TagListGenerator(IFileAccess fileAccess, Configuration configuration)
    {
        _fileAccess = fileAccess;
        _configuration = configuration;
    }

    public TagList Generate(List<Post> posts)
    {
        var tags = new Dictionary<string, int>();
        foreach (var post in posts)
        {
            foreach (var tag in post.Tags)
            {
                tags[tag] += 1;
            }
        }

        var template = _fileAccess.ReadFile($"{_configuration.ThemePath}/tags.html");
        var html = Template.Parse(template).Render(new { tags });
        return new TagList(html, "/tags", $"{_configuration.DestPath}/tags/index.html");
    }
}