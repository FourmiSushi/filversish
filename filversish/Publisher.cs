using filversish.Utils;
using Scriban;

namespace filversish;

public class Publisher(IFileAccess fileAccess, Configuration configuration)
{
    private void PublishPosts(List<Post> posts)
    {
        foreach (var post in posts)
        {
            var html = ApplyCommonTemplate(post.Title, post.Html, post.Description);
            fileAccess.WriteFile(post.SavePath, html);
        }
    }

    private void PublishPages(List<Page> pages)
    {
        foreach (var page in pages)
        {
            var html = ApplyCommonTemplate("", page.Html, configuration.Description);
            fileAccess.WriteFile(page.SavePath, html);
        }
    }

    private void PublishIndex(Index index)
    {
        var html = ApplyCommonTemplate("", index.Html, configuration.Description);
        fileAccess.WriteFile(index.SavePath, html);
    }

    private void PublishTagList(TagList tagList)
    {
        var html = ApplyCommonTemplate("タグ一覧", tagList.Html, configuration.Description);
        fileAccess.WriteFile(tagList.SavePath, html);
    }

    private void PublishAssets()
    {
        var s = $"{configuration.AssetsPath}";
        var d = $"{configuration.DestPath}/assets";

        if (!Directory.Exists(configuration.AssetsPath))
            return;

        fileAccess.CopyDirectory(s, d);
    }

    private void PublishSitemap(string sitemap)
    {
        fileAccess.WriteFile($"{configuration.DestPath}/sitemap.xml", sitemap);
    }

    public void Publish(
        List<Post> posts,
        List<Page> pages,
        Index? index,
        TagList tagList,
        string sitemap
    )
    {
        DeleteOldDest();

        PublishPosts(posts);
        PublishPages(pages);
        PublishTagList(tagList);
        PublishSitemap(sitemap);

        if (!configuration.UseIndexPage || index == null)
        {
            fileAccess.CopyFile(
                $"{configuration.DestPath}/pages/index.html",
                $"{configuration.DestPath}/index.html"
            );
        }
        else
        {
            PublishIndex(index);
        }

        PublishAssets();
    }

    private string ApplyCommonTemplate(string title, string body, string description)
    {
        var template = fileAccess.ReadFile($"{configuration.ThemePath}/common.html");
        var html = Template
            .Parse(template)
            .Render(
                new
                {
                    configuration.Title,
                    configuration.Description,
                    PostTitle = title,
                    Body = body,
                    PageDescription = description
                }
            );

        if (html == null)
        {
            throw new Exception("共通テンプレートへの適用に失敗しました");
        }

        return html;
    }

    private void DeleteOldDest()
    {
        if (!fileAccess.IsExist(configuration.DestPath))
            return;
        foreach (var s in fileAccess.GetDirectoriesIn(configuration.DestPath))
        {
            fileAccess.DeleteDirectory(s);
        }

        foreach (var s in fileAccess.GetFilesIn(configuration.DestPath))
        {
            fileAccess.DeleteFile(s);
        }
    }
}
