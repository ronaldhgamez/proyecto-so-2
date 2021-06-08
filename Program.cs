using System;
using System.Collections.Generic;

namespace console_proyecto_so_2
{
    class Program
    {
        public static string bbc_1 = "https://www.bbc.com/mundo/topics/c67q9nnn8z7t"; // covid
        public static string bbc_2 = "https://www.bbc.com/mundo/topics/c06gq9v4xp3t"; // economia
        public static string bbc_3 = "https://www.bbc.com/mundo/topics/cyx5krnw38vt"; // tecnologia
        private const string ElMundo = "https://www.elmundo.cr/costa-rica";

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

            foreach (var n in list)
            {
                n.ToString();
            }
        }

    }
}
