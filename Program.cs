using System;

namespace console_proyecto_so_2
{
    class Program
    {
        //public static string bbc = "https://www.bbc.com/mundo/topics/c67q9nnn8z7t";



        static void Main(string[] args)
        {
            WebScraping ws = new WebScraping();
            string text = ws.GetBBCNews("https://www.bbc.com/mundo/topics/cpzd498zkxgt");
            Console.WriteLine(text);
        }
    }
}
