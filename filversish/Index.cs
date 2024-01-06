namespace filversish;

public class Index(List<Post> posts, string savePath, string link)
{
    public List<Post> Posts = posts;
    public string Link = link;
    public string SavePath = savePath;

    public string Html = null!;
}
