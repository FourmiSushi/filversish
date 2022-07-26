using Scriban;

namespace filversish.Page;

public class Page
{
    public List<Post.Post> Posts;
    public int PageNumber;
    public string Link;

    public string? NextPageLink;
    public string? PreviousPageLink;

    public string SavePath;
    public string Html = null!;

    public Page(List<Post.Post> posts, int pageNumber, int pageCount, string savePath)

    {
        Posts = posts;
        PageNumber = pageNumber;
        SavePath = savePath;
        Link = $"/pages/{pageNumber}";

        if (pageNumber != 0)
        {
            PreviousPageLink = $"/pages/{pageNumber - 1}";
        }

        if (pageNumber != pageCount - 1)
        {
            NextPageLink = $"/pages/{pageNumber + 1}";
        }
    }
}