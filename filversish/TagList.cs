// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable NotAccessedField.Global

namespace filversish;

public class TagList(string html, string link, string savePath)
{
    public string Html = html;

    public string Link = link;
    public string SavePath = savePath;
}