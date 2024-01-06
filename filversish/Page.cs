// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable NotAccessedField.Global

namespace filversish;

public class Page(
    List<Post> posts,
    string? tagName,
    int pageNumber,
    string savePath,
    string link,
    string? previousPageLink,
    string? nextPageLink)
{
    public List<Post> Posts = posts;
    public int PageNumber = pageNumber;

    public string Link = link;
    public string? NextPageLink = nextPageLink;
    public string? PreviousPageLink = previousPageLink;

    public string SavePath = savePath;
    public string Html = null!;

    public string? TagName = tagName;
}
