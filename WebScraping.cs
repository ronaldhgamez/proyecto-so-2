using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Collections.Generic;
using System.Linq;
using System;

namespace console_proyecto_so_2
{
    class WebScraping
    {
        private HtmlWeb web;

        public WebScraping()
        {
            this.web = new HtmlWeb();
        }

        /* Get text from BBC home page */
        public List<Website> GetBBCNews(string url, List<Website> list)
        {
            HtmlDocument doc = web.Load(url);
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//*[@class=\"lx-stream__post-container\"]");

            /* Go through every li html tags */
            foreach (HtmlNode li_node in nodes)
            {
                /* get title from news */
                var html_a = li_node.CssSelect(".qa-heading-link").First();
                string title = html_a.InnerText.Trim();

                /* get paparagraph from news */
                var html_p = li_node.CssSelect(".lx-stream-related-story--summary").First();
                string paragraph = html_p.InnerText.Trim();
                
                /* get image from news */
                var htmlDiv = li_node.CssSelect(".qa-story-image-link").FirstOrDefault();
                var html_img = htmlDiv.Element("div").Element("img");
                string img_url = html_img.Attributes["src"].Value;

                list.Add(new Website(title, paragraph, img_url));
            }
            return list;
        }

        public List<Website> GetElMundo(string url, List<Website> list)
        {
            var doc = web.Load(url);

            var newsContainer = doc.DocumentNode.SelectNodes("//*[@id='main-content']/div[3]");

            var news = newsContainer.CssSelect(".mh-posts-grid-col");

            foreach (var node in news)
            {
                var title = node.CssSelect(".mh-posts-grid-title").First().InnerText;

                var summary = node.CssSelect(".mh-excerpt").First()
                    .CssSelect("p").First().InnerText;

                var imagePath = node.CssSelect(".wp-post-image").First()
                    .Attributes["src"].Value;

                var imageUrl = "https://www.elmundo.cr" + imagePath;

                list.Add(new Website(title.Trim(), summary.Trim(), imageUrl));
            }
            return list;
        }
        
    }
}
