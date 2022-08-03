using filversish.Utils;
using Scriban;

namespace filversish;

public class TagListGenerator
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;


    public List<string> GetTags(List<Post> posts)
    {
        var tags = new Dictionary<string, int>();
        foreach (var post in posts)
        {
            foreach (var tag in post.Tags)
            {
                if (tags.ContainsKey(tag))
                {
                    tags[tag] += 1;
                }
                else
                {
                    tags[tag] = 1;
                }
            }
        }

        return tags.Keys.ToList();
    }


    public TagListGenerator(Configuration configuration, IFileAccess fileAccess)
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
                if (tags.ContainsKey(tag))
                {
                    tags[tag] += 1;
                }
                else
                {
                    tags[tag] = 1;
                }
            }
        }

        var template = _fileAccess.ReadFile($"{_configuration.ThemePath}/tags.html");
        var html = Template.Parse(template).Render(new { tags });
        return new TagList(html, "/tags", $"{_configuration.DestPath}/tags/index.html");
    }
}