using GenHTTP.Engine;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.StaticWebsites;

namespace filversish.Utils;

public class LocalServer
{
    public static void Start(string path)
    {
        var t = ResourceTree.FromDirectory(path);
        var app = StaticWebsite.From(t);

        Host.Create().Console().Defaults().Handler(app).Run();
    }
}