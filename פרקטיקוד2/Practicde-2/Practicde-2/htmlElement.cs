using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography;

namespace Practicde_2
{
    public class htmlElement
    {
        //מאפייני המחלקה
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; } = new List<string>();
        public List<string> Classes { get; set; } = new List<string>();
        public string InnerHtml { get; set; }
        public htmlElement parent { get; set; }
        public List<htmlElement> chiLdren { get; set; } = new List<htmlElement>();
        public htmlElement root { get; set; }

        //פונקציה המחזירה את כל הצאצאים
        public IEnumerable<htmlElement> Descendants()
        {//תור של האלמנטים
            Queue<htmlElement> q = new Queue<htmlElement>();
            //כל עוד יש אלמנטים בתור
            q.Enqueue(this);
            while (q.Count > 0)
            {
                var e = q.Dequeue();
                foreach (var c in e.chiLdren)
                {
                    q.Enqueue(c);
                }
                yield return e;
            }
        }
        //פונקציה המחזירה את כל האבות
        public static List<string> Ancestors(htmlElement h)
        {
            List<string> parents = new List<string>();
            while (h.parent.Name != null)
            {
                parents.Add(h.parent.Name);
                h = h.parent;
            }
            return parents;
        }
        public static HashSet<htmlElement> Find(htmlElement h, Selector s, HashSet<htmlElement> answer)
        {//כל האלמנטים העונים על הדרישות כרגע ברמה הנוכחית
            HashSet<htmlElement> he = new HashSet<htmlElement>();
            //זמון הפונקציה שמחזירה את הצאצאים
            var descendants = h.Descendants();
            //עובר על הפונקציה הזו ומשווה עם הסלקטור
            foreach (var element in descendants)
            {
                if (s.Children == null)
                {//הרשימה הסופית שמביאה את מה שענה על הדרישות בכל הרמות
                    answer.Add(element);
                    break;
                }

                var flag = false;
                //בדיקות
                if (s.Children.TagName != null && element.Name == s.Children.TagName)
                {
                    flag = true;
                }

                if (s.Children.Id != null && element.Id != s.Children.Id)
                {
                    flag = false;
                }
                if (s.Children.Id != null && element.Id == s.Children.Id)
                {
                    flag = true;
                }

                if (s.Children.Classes != null && element.Classes.Count == s.Children.Classes.Count)
                {
                    foreach (var c in s.Children.Classes)
                    {
                        if (!element.Classes.Contains(c))
                        {
                            flag = false;
                        }
                    }
                }
                if (flag)
                {
                    he.Add(element);
                    foreach (var matchedElement in he)
                    {//קריאה רקורסיבית
                        Find(matchedElement, s.Children, answer);
                    }
                }
            }

            return answer;
            Console.WriteLine();
        }

    }
}


