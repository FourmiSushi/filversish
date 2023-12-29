using filversish.Utils;
using Scriban;

namespace filversish;

public class PageGenerator
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;

    public PageGenerator(Configuration configuration, IFileAccess fileAccess)
    {
        _configuration = configuration;
        _fileAccess = fileAccess;
    }

    public List<Page> Generate(List<Post> posts, string? tagName)
    {
        var pagePosts = posts.OrderByDescending(p => p.PublishedAt).ToList();
        if (tagName != null)
        {
            pagePosts = pagePosts.Where(p => p.Tags.Contains(tagName)).ToList();
        }

        var template = _fileAccess.ReadFile($"{_configuration.ThemePath}/page.html");

        var result = new List<Page>();
        var pageCount = Math.Ceiling(pagePosts.Count / 10.0);
        for (var i = 0; i < pageCount; i++)
        {
            var link = tagName != null ? $"/tags/{tagName}/{i}" : $"/pages/{i}";
            var nextPageLink = tagName != null ? $"/tags/{tagName}/{i + 1}" : $"/pages/{i + 1}";
            var previousPageLink = tagName != null ? $"/tags/{tagName}/{i - 1}" : $"/pages/{i - 1}";

            var p = pagePosts.GetRange(i * 10, Math.Min(10, pagePosts.Count % 10));
            var page = new Page(
                p,
                tagName,
                i,
                $"{_configuration.DestPath}{link}/index.html",
                link,
                i > 0 ? previousPageLink : null,
                i < pageCount - 1 ? nextPageLink : null
            );
            var html = Template.Parse(template).Render(page);
            page.Html = html;

            if (i == 0)
            {
                var firstLink = tagName != null ? $"/tags/{tagName}" : $"/pages";
                var firstPage = new Page(
                    p,
                    tagName,
                    i,
                    $"{_configuration.DestPath}{firstLink}/index.html",
                    firstLink,
                    null,
                    nextPageLink
                );
                firstPage.Html = html;
                result.Add(firstPage);
            }

            result.Add(page);
        }

        return result;
    }
}
