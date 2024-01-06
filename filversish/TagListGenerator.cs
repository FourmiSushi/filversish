using filversish.Utils;
using Scriban;

namespace filversish;

public class TagListGenerator(Configuration configuration, IFileAccess fileAccess)
{
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

        var template = fileAccess.ReadFile($"{configuration.ThemePath}/tags.html");
        var html = Template.Parse(template).Render(new { tags });
        return new TagList(html, "/tags", $"{configuration.DestPath}/tags/index.html");
    }
}
