using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Practicde_2
{
    public class Selector
    {//מאפייני המחלקה
        public string TagName;
        public string Id;
        public List<string> Classes = new List<string>();
        public Selector Parent;
        public Selector Children;

        public static Selector Convert(String query)
        {
            //מחלק את המחרוזת שהתקבלה לפי רווחים
            string[] str = query.Split(" ");
            //אובייקט שורש
            var root = new Selector();
            //var Selector = new Selector();
            //סלקטור הנוכחי בכל איטרציה
            var current = root;
            //עובר על המחרוזת המחולקת -כלומר על כל רמה ורמה
            foreach (var q in str)
            {//אובייקט חדש שמוסיף אותו כבן של הסלקטור הנוכחי
                Selector selector = new Selector();
                //כנ"ל-ההוספה
                current.Children = selector;
                selector.Parent = current;
                //הסלקטור הנוכחי מצביע על הסלקטור החדש
                current = selector;
                //ביטוי רגולרי המחלק לפי # ונקודה
                string[] split = q.Replace("#", "-#").Replace(".", "-.").Split("-");
                //עובר על המחרוזת המחולקת 
                foreach (var r in split)
                {//בדיקה האם קיים class
                    if (r.IndexOf('.') != -1)
                    {// הוספה לרשימת
                     // classes
                        current.Classes.Add(r.Substring(1));
                    }
                    //בדיקה האם קיים id
                    else if (r.IndexOf('#') != -1)
                        //הוספה למאפיין ה
                        //id
                        current.Id = r.Substring(1);
                    else
                    {//בודק אם היא תגית
                        if (HtmlHelper.Instance.HtmlTags.Contains(r))
                            current.TagName = r;
                    }

                }

            }
            return root;
        }
    }
}
