using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Practicde_2;
using System.IO;


namespace Practicde_2
{
    public class HtmlHelper
    {//singleton
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }
        private HtmlHelper()
        {
            //קריאת הקבצים
            string jsonContent1 = File.ReadAllText("filesJson/HtmlTags.json");
            string jsonContent2 = File.ReadAllText("filesJson/HtmlVoidTags.json");
            //דיסרלזציה-הצבה במערכים שיכילו את הקבצים
            HtmlTags = JsonSerializer.Deserialize<string[]>(jsonContent1);
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(jsonContent2);
        }

    }


}
