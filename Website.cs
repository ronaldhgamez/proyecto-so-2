using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace console_proyecto_so_2
{
    class Website
    {
        public string link;
        public string description;
        public string category;
        public string img_url;

        public Website(string link, string description, string category, string img_url)
        {
            this.link = link;
            this.category = category;
            this.img_url = img_url;
            this.description = description;
        }
    }
}
