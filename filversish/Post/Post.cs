using Markdig;

namespace filversish.Post;

public class Post
{
    public string Author;
    public DateTime PublishedAt;
    public string[] Tags;
    public string Title;
    public string BodyRaw;
    public string BodyPlain;
    public string BodyHtml;
    public string Html;
    public string SavePath;
    public string Link;

    public Post(string author, DateTime publishedAt, string[] tags, string title, string bodyRaw, string savePath,
        string link)
    {
        var pipeline = new MarkdownPipelineBuilder().UseSoftlineBreakAsHardlineBreak().Build();

        Author = author;
        PublishedAt = publishedAt;
        Tags = tags;
        Title = title;
        BodyRaw = bodyRaw;
        SavePath = savePath;
        Link = link;
        BodyPlain = Markdown.ToPlainText(bodyRaw, pipeline);
        BodyHtml = Markdown.ToHtml(bodyRaw, pipeline);
    }
}