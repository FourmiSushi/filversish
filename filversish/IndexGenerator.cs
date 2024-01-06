using filversish.Utils;
using Scriban;

namespace filversish;

public class IndexGenerator(Configuration configuration, IFileAccess fileAccess)
{
    public Index Generate(IEnumerable<Post> posts)
    {
        var pickups = posts
            .Where(p => p.PickUp)
            .OrderByDescending(p => p.PublishedAt).ToList();

        var template = fileAccess.ReadFile($"{configuration.ThemePath}/index.html");

        const string link = "";
        var result = new Index(pickups, $"{configuration.DestPath}{link}/index.html", link);
        var html = Template.Parse(template).Render(result);
        result.Html = html;

        return result;
    }
}