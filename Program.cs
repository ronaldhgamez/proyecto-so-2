using System;

namespace console_proyecto_so_2
{
    class Program
    {
        public static string bbc = "https://www.bbc.com/mundo/topics/c67q9nnn8z7t";

        private const string ElMundo = "https://www.elmundo.cr/costa-rica";

        static void Main(string[] args)
        {
            WebScraping ws = new WebScraping();

            // BBC News
            // string text = ws.GetBBCNews(bbc);
            // Console.WriteLine(text);
            
            // El Mundo CR
            var text = ws.GetElMundo(ElMundo);
            Console.WriteLine(text);
        }
    }
}
