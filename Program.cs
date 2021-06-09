using System;
using System.Collections.Generic;

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
            
            WebScraping ws = new WebScraping();
            List<Website> list = new List<Website>();

            // Scraping BBC news
            list = ws.GetBBCNews(bbc_1, list);
            list = ws.GetBBCNews(bbc_2, list);
            list = ws.GetBBCNews(bbc_3, list);

            // Scraping El Mundo CR news
            list = ws.GetElMundo(ElMundo, list);

            foreach (var news in list)
            {
                news.ToString();
            }

            /* Starts Server */
            //Server server = new Server("localhost", 3000);
            //server.Start();
        }

    }
}
