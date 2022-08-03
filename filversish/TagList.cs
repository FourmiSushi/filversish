// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable NotAccessedField.Global

namespace filversish;

public class TagList
{
    public string Html;

    public string Link;
    public string SavePath;

    public TagList(string html, string link, string savePath)
    {
        Html = html;
        Link = link;
        SavePath = savePath;
    }
}