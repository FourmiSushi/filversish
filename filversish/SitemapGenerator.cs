using System.Web;
using Scriban;

namespace filversish;

public class SitemapGenerator(Configuration configuration)
{
    public string Generate(List<Post> posts)
    {
        var urls = new List<Url>
        {
            new("", DateTime.Now)
        };

        foreach (var post in posts)
        {
            urls.Add(new Url(post.Link, post.LastModified ?? post.PublishedAt));
        }

        var html = Template.Parse(XmlTemplate).Render(new
        {
            configuration.Host, urls
        });
        
        return html;
    }

    private const string XmlTemplate = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<urlset xmlns=""http://www.sitemaps.org/schemas/sitemap/0.9"">
{{ for u in urls }}    <url>
        <loc>https://{{ host }}{{ u.link }}/</loc>
        <lastmod>{{ u.last_modified }}</lastmod>
    </url>
{{ end }}
</urlset>
";

    public class Url(string link, DateTime lastModified)
    {
        public string Link = HttpUtility.UrlEncode(link).Replace("%2f", "/");
        public string LastModified = lastModified.ToString("yyyy-MM-ddTHH:mmzzz");
    }
}