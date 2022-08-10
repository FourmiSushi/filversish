using filversish.Utils;
using Scriban;

namespace filversish;

public class Publisher
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;

    public Publisher(IFileAccess fileAccess, Configuration configuration)
    {
        _fileAccess = fileAccess;
        _configuration = configuration;
    }

    private void PublishPosts(List<Post> posts)
    {
        foreach (var post in posts)
        {
            var html = ApplyCommonTemplate(post.Title, post.Html);
            _fileAccess.WriteFile(post.SavePath, html);
        }
    }

    private void PublishPages(List<Page> pages)
    {
        foreach (var page in pages)
        {
            var html = ApplyCommonTemplate("", page.Html);
            _fileAccess.WriteFile(page.SavePath, html);
        }
    }

    private void PublishTagList(TagList tagList)
    {
        var html = ApplyCommonTemplate("タグ一覧", tagList.Html);
        _fileAccess.WriteFile(tagList.SavePath, html);
    }

    private void PublishAssets()
    {
        var s = $"{_configuration.AssetsPath}";
        var d = $"{_configuration.DestPath}/assets";

        if (!Directory.Exists(_configuration.AssetsPath)) return;


        _fileAccess.CopyDirectory(s, d);
    }

    public void Publish(List<Post> posts, List<Page> pages, TagList tagList)
    {
        DeleteOldDest();

        PublishPosts(posts);
        PublishPages(pages);
        PublishTagList(tagList);

        _fileAccess.CopyFile($"{_configuration.DestPath}/pages/0/index.html", $"{_configuration.DestPath}/index.html");

        PublishAssets();
    }

    private string ApplyCommonTemplate(string title, string body)
    {
        var template = _fileAccess.ReadFile($"{_configuration.ThemePath}/common.html");
        var html = Template.Parse(template).Render(new
        {
            _configuration.Title, _configuration.Description, PostTitle = title, Body = body
        });

        if (html == null)
        {
            throw new Exception("共通テンプレートへの適用に失敗しました");
        }

        return html;
    }

    private void DeleteOldDest()
    {
        foreach (var s in _fileAccess.GetDirectoriesIn(_configuration.DestPath))
        {
            _fileAccess.DeleteDirectory(s);
        }

        foreach (var s in _fileAccess.GetFilesIn(_configuration.DestPath))
        {
            _fileAccess.DeleteFile(s);
        }
    }
}