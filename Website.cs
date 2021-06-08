using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_proyecto_so_2
{
    class Website
    {
        public string title;
        public string description;
        public string img_url;

        public Dictionary<string, int> categories;
        public string category;

        public Website(string title, string description, string img_url)
        {
            this.title = title;
            this.description = description;
            this.img_url = img_url;
            this.category = null;
            this.categories = new Dictionary<string, int>();
        }

        public void SetCategory(string category)
        {
            this.category = category;
        }

        //private void initialize()
        //{
        //    Dictionary<string, int> categories = new Dictionary<string, int>();

        //    categories.Add("", 0);
        //    categories.Add("", 0);
        //    categories.Add("", 0);
        //    categories.Add("", 0);
        //    categories.Add("", 0);
        //}

        public void ToString()
        {
            Console.WriteLine("\"{0}\"", this.title);
            Console.WriteLine("\"{0}\"", this.description);
            Console.WriteLine("\"{0}\"", this.img_url);
            Console.WriteLine("-------------------------\n");
        }
    }
}
