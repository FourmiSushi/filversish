namespace filversish;

public class Index
{
    public List<Post> Posts;
    public string Link;
    public string SavePath;

    public string Html = null!;

    public Index(List<Post> posts, string savePath, string link)
    {
        Posts = posts;
        SavePath = savePath;
        Link = link;
    }
}
