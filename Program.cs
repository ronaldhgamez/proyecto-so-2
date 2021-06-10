using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;

namespace console_proyecto_so_2
{
    class Program
    {
        public const string bbc_1 = "https://www.bbc.com/mundo/topics/c67q9nnn8z7t"; // covid
        public const string bbc_2 = "https://www.bbc.com/mundo/topics/c06gq9v4xp3t"; // economia
        public const string bbc_3 = "https://www.bbc.com/mundo/topics/cyx5krnw38vt"; // tecnologia
        public const string ElMundo = "https://www.elmundo.cr/costa-rica";

        static void Main(string[] args)
        {
            var categories = LoadCategories();
            
            var classifier = new Classifier(categories);
            
            WebScraping ws = new WebScraping();
            
            List<Website> list = new List<Website>();

            // Scraping BBC news
            list = ws.GetBBCNews(bbc_1, list);
            list = ws.GetBBCNews(bbc_2, list);
            list = ws.GetBBCNews(bbc_3, list);

            // Scraping El Mundo CR news
            list = ws.GetElMundo(ElMundo, list);

            classifier.Classify(list);

            foreach (var news in list)
            {
                news.ToString();
            }
            
            /* Starts Server */
            //Server server = new Server("localhost", 3000);
            //server.Start();
        }
        
        private static Dictionary<string, Category> LoadCategories()
        {
            using var reader = new StreamReader("../../../data.json");
            
            var json = reader.ReadToEnd();

            var jsonObject = JObject.Parse(json);

            var jsonCategories = (JObject) jsonObject.SelectToken("categorias");
            
            var categories = new Dictionary<string, Category>();
            
            foreach (var category in jsonCategories)
            {
                var cat = new Category(category.Key);

                var words = (JArray) category.Value;

                foreach (var word in words)
                {
                    if (cat.Words.ContainsKey(word.ToString().ToLower()))
                    {
                        cat.Words[word.ToString().ToLower()] += 1;
                    }
                    else
                    {
                        cat.Words.Add(word.ToString().ToLower(), 1);
                    }
                }
                
                categories.Add(category.Key, cat);
            }

            return categories;
        }

    }
}
