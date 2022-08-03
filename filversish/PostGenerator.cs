﻿using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using filversish.Utils;
using Scriban;
using Tomlyn;

// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace filversish;

public class PostGenerator
{
    private readonly IFileAccess _fileAccess;
    private readonly Configuration _configuration;

    public PostGenerator(Configuration configuration, IFileAccess fileAccess)
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
        var template = _fileAccess.ReadFile($"{_configuration.ThemePath}/post.html");
        for (int i = 0; i < metas.Length; i++)
        {
            var meta = Toml.ToModel<PostMetaModel>(_fileAccess.ReadFile(metas[i]));
            var bodyRaw = _fileAccess.ReadFile(mds[i]);
            var p =
                new Post(
                    meta.Author ?? _configuration.Author,
                    DateTime.Parse(meta.PublishedAt ?? DateTime.Now.ToShortDateString()),
                    meta.Tags ?? new[] { _configuration.DefaultTag },
                    meta.Title ?? "untitled",
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

        public static  PostMetaModel GetDefault(string title, Configuration configuration) =>
            new PostMetaModel()
            {
                Author = configuration.Author,
                PublishedAt = DateTime.Now.ToShortDateString(),
                Tags = new[] { configuration.DefaultTag },
                Title = title
            };
    }
}