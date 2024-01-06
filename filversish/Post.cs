// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable NotAccessedField.Global

using Markdig;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace filversish;

public class Post
{
    public string Author;
    public DateTime PublishedAt;
    public DateTime? LastModified;
    public string[] Tags;
    public string Title;
    public string Description;
    public bool PickUp;
    public string BodyRaw;
    public string BodyPlain;
    public string BodyHtml;
    public string Html = null!;
    public string SavePath;
    public string Link;

    public Post(
        string author,
        DateTime publishedAt,
        DateTime? lastModified,
        string[] tags,
        string title,
        string bodyRaw,
        string savePath, 
        string link,
        string description,
        bool pickUp
    )
    {
        var pipeline = new MarkdownPipelineBuilder()
            .UseSoftlineBreakAsHardlineBreak()
            .UseEmphasisExtras()
            .UseAutoLinks()
            .UsePipeTables()
            .Build();

        var document = Markdown.Parse(bodyRaw, pipeline);
        foreach (var d in document.Descendants())
        {
            if (d is AutolinkInline || d is LinkInline)
            {
                d.GetAttributes().AddPropertyIfNotExist("target", "_blank");
                d.GetAttributes().AddPropertyIfNotExist("rel", "noopener");
            }
        }

        Author = author;
        PublishedAt = publishedAt;
        LastModified = lastModified;
        Tags = tags;
        Title = title;
        BodyRaw = bodyRaw;
        SavePath = savePath;
        Link = link;
        Description = description;
        PickUp = pickUp;
        BodyPlain = Markdown.ToPlainText(bodyRaw, pipeline);
        BodyHtml = document.ToHtml(pipeline);
    }
}