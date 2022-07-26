using filversish.Utils;
using Scriban;

namespace filversish.Page;

public class Generator
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;

    public Generator(Configuration configuration, IFileAccess fileAccess)
    {
        _configuration = configuration;
        _fileAccess = fileAccess;
    }

    public List<Page> Generate(List<Post.Post> posts)
    {
        var pagePosts = posts.Select((post, i) => new { post, i }).GroupBy(x => x.i / 10)
            .Select(a => a.Select(b => b.post).ToList()).ToList();

        var template = _fileAccess.ReadFile($"{_configuration.ThemesPath}/page.html");
        return pagePosts.Select((a, i) =>
        {
            var page = new Page(
                a,
                i,
                pagePosts.Count,
                $"{_configuration.DestPath}/pages/{i}/index.html"
            );
            var html = Template.Parse(template).Render(page);
            page.Html = html;
            return page;
        }).ToList();
    }
}