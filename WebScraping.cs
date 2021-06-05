using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Linq;

namespace console_proyecto_so_2
{
    class WebScraping
    {
        private HtmlWeb web;

        public WebScraping()
        {
            this.web = new HtmlWeb();
        }

        /* Get text from CRHoy page */
        public string GetTextFromCRHoy(string url)
        {
            HtmlDocument doc = this.web.Load(url);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("/html/body/div[2]/div/div/span[2]/a[1]");
            string text = (node == null) ? "name not found" : node.InnerText;
            return text;
        }

  

    }
}
