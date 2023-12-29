// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable NotAccessedField.Global

namespace filversish;

public class Page
{
    public List<Post> Posts;
    public int PageNumber;

    public string Link;
    public string? NextPageLink;
    public string? PreviousPageLink;

    public string SavePath;
    public string Html = null!;

    public string? TagName;

    public Page(
        List<Post> posts,
        string? tagName,
        int pageNumber,
        string savePath,
        string link,
        string? previousPageLink,
        string? nextPageLink
    )
    {
        Posts = posts;
        PageNumber = pageNumber;
        SavePath = savePath;

        TagName = tagName;

        Link = link;
        PreviousPageLink = previousPageLink;
        NextPageLink = nextPageLink;
    }
}
