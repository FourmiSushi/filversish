using System.Diagnostics.CodeAnalysis;
using filversish.Utils;
using Markdig;
using Scriban;
using Tomlyn;

namespace filversish.Post;

public class Generator
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;

    public Generator(Configuration configuration, IFileAccess fileAccess)
    {
        _configuration = configuration;
        _fileAccess = fileAccess;
    }

    public List<Post> Generate()
    {
        var files = _fileAccess.GetFilesIn(_configuration.PostsPath);
        var metas = files.Where(f => Path.GetExtension(f) == ".toml").OrderBy(f => f).ToArray();
        var mds = files.Where(f => Path.GetExtension(f) == ".md").OrderBy(f => f).ToArray();


        var result = new List<Post>();
        var template = _fileAccess.ReadFile($"{_configuration.ThemesPath}/post.html");
        for (int i = 0; i < metas.Length; i++)
        {
            var meta = Toml.ToModel<PostMetaModel>(_fileAccess.ReadFile(metas[i]));
            var bodyRaw = _fileAccess.ReadFile(mds[i]);
            var p =
                new Post(
                    meta.Author ?? "Anonymous",
                    DateTime.Parse(meta.PublishedAt ?? "0001-01-01"),
                    meta.Tags ?? Array.Empty<string>(),
                    meta.Title ?? "Untitled",
                    bodyRaw,
                    $"{_configuration.DestPath}/posts/{Path.GetFileNameWithoutExtension(metas[i])}/index.html",
                    $"/posts/{Path.GetFileNameWithoutExtension(metas[i])}"
                );
            var html = Template.Parse(template).Render(p);
            p.Html = html;
            result.Add(p);
        }

        return result;
    }


    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
    public class PostMetaModel
    {
        public string? Author { get; set; }
        public string? PublishedAt { get; set; }
        public string[]? Tags { get; set; }
        public string? Title { get; set; }
    }
}