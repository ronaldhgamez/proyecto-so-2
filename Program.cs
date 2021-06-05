using System;

namespace console_proyecto_so_2
{
    class Program
    {
        static void Main(string[] args)
        {
            WebScraping ws = new WebScraping();
            string text = ws.GetTextFromCRHoy("https://www.crhoy.com/site/dist/portada-tecnologia.php");
            Console.WriteLine(text);
        }
    }
}
