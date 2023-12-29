using filversish.Utils;
using Scriban;

namespace filversish;

public class IndexGenerator
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;

    public IndexGenerator(Configuration configuration, IFileAccess fileAccess)
    {
        _configuration = configuration;
        _fileAccess = fileAccess;
    }

    public Index Generate(List<Post> posts)
    {
        // TODO: ピックアップ記事機能の実装
        var pickups = posts.OrderByDescending(p => p.PublishedAt).ToList();

        var template = _fileAccess.ReadFile($"{_configuration.ThemePath}/index.html");

        var link = "";
        var result = new Index(pickups, $"{_configuration.DestPath}{link}/index.html", link);
        var html = Template.Parse(template).Render(result);
        result.Html = html;

        return result;
    }
}
