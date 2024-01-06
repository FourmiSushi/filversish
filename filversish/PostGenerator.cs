using System.Diagnostics.CodeAnalysis;
using filversish.Utils;
using Scriban;
using Tomlyn;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace filversish;

public class PostGenerator(Configuration configuration, IFileAccess fileAccess)
{
    public List<Post> Generate()
    {
        var files = fileAccess.GetFilesIn(configuration.PostsPath);
        var metas = files.Where(f => Path.GetExtension(f) == ".toml").OrderBy(f => f).ToArray();
        var mds = files.Where(f => Path.GetExtension(f) == ".md").OrderBy(f => f).ToArray();


        var result = new List<Post>();
        var template = fileAccess.ReadFile($"{configuration.ThemePath}/post.html");
        for (int i = 0; i < metas.Length; i++)
        {
            var meta = Toml.ToModel<PostMetaModel>(fileAccess.ReadFile(metas[i]));

            if (meta.Draft ?? false)
            {
                continue;
            }

            var bodyRaw = fileAccess.ReadFile(mds[i]);
            var p =
                new Post(
                    meta.Author ?? configuration.Author,
                    DateTime.Parse(meta.PublishedAt ?? DateTime.Now.ToShortDateString()),
                    meta.LastModified == null ? null : DateTime.Parse(meta.LastModified),
                    meta.Tags ?? new[] { configuration.DefaultTag },
                    meta.Title ?? "untitled",
                    bodyRaw,
                    $"{configuration.DestPath}/posts/{Path.GetFileNameWithoutExtension(metas[i])}/index.html",
                    $"/posts/{Path.GetFileNameWithoutExtension(metas[i])}",
                    meta.Description ?? "",
                    meta.PickUp ?? false
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
        public string? LastModified { get; set; }
        public string[]? Tags { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool? Draft { get; set; }
        public bool? PickUp { get; set; }

        public static PostMetaModel GetDefault(string title, Configuration configuration) =>
            new()
            {
                Author = configuration.Author,
                PublishedAt = DateTime.Now.ToShortDateString(),
                Tags = [configuration.DefaultTag],
                Title = title,
                Description = "",
                Draft = true,
                PickUp = false,
            };
    }
}