using Practicde_2;
using System.Text.RegularExpressions;
//טעינת הhtml
//var html = await Load("http://localhost:60229/");
var html = await Load("http://localhost:4200/");
//כל הhtml
var cleanHtml = new Regex("[\\r\\t\\n]").Replace(html, " ");
//מחלק לשורות
var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0);
//אובייקט שורש
var root = new htmlElement();
//משתנה המכיל את האלמנט הנוכחי
var current = root;
var l = "";
// שמחולקhtml עובר על המשתנה שמכיל את כל ה
foreach (var line in htmlLines)
{
    //מציאת רווח
    int r = line.IndexOf(" ");
    //אם הוא מצא רווח
    if (r > 0)
    {
        //חותך את התגית
        l = line.Substring(0, r);
    }
    else
    {
        //כל השורה ז"א התגית
        l = line;
    }
    //אם הגיעו לסוף 
    if (l == "/html")
    {
        break;
    }
    //אם מצא תגית סוגרת לפי/
    else if (line.IndexOf('/') == 0)
    {
        //האלמנט הנוכחי מוצב באב
        current = current.parent;
    }
    //בדיקה אם התגית קיימת ברשימה של התגיות שגם נסגרות, ולא נמצאת בתגיות שאינן נסגרות
    else if (HtmlHelper.Instance.HtmlTags.Contains(l) && !HtmlHelper.Instance.HtmlVoidTags.Contains(l))
    {//אובייקט חדש שמוסיף אותו כבן של הסלקטור הנוכחי
        htmlElement newElement = new htmlElement();
        //הוספת האלמנט החדש לאלמנט הנוכחי לרשימה של הילדים
        current.chiLdren.Add(newElement);
        //הצבה לאלמנט הנוכחי את האב 
        newElement.parent = current;
        //הסלקטור הנוכחי מצביע על הסלקטור החדש
        current = newElement;
        current.Name = l;
        //רשימה שמקבלת את כל הatrribute
        List<string> attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line).ToList().ConvertAll(z => z.ToString());
        //הוספה של הרשימה למאפיין הatrribute
        current.Attributes.AddRange(attributes);
        foreach (var a in current.Attributes)
        {
            //idאז הוא מתווסף למאפיין ה  id אם הרשימה מכילה 
            if (a.IndexOf("id") != -1)
            {
                //הצבה
                var a1 = a.Substring(a.IndexOf('"') + 1);
                 current.Id = a1.TrimEnd('\"');
                Console.WriteLine(current.Id);
            }
            //המכיל רשימה class אז הוא מתווסף למאפיין  class בודק אם הרשימה מכילה 
            if (a.IndexOf("class") != -1)
            {
                //הצבה
                var cc = a.Substring(a.IndexOf('"') + 1);
               var tc = cc.TrimEnd('\"');
                current.Classes.Add(tc);

            }

        }
    }
    //אם זה בקובץ של התגיות הסוגרות  
    else if (HtmlHelper.Instance.HtmlVoidTags.Contains(l))
    {//אובייקט חדש שמוסיף אותו כבן של האובייקט הנוכחי והנוכחי אבא של החדש
        htmlElement newElement = new htmlElement();
        current.chiLdren.Add(newElement);
        newElement.parent = current;
        //האלמנט הנוכחי מצביע על האלמנט החדש
        current = newElement;
        current.Name = l;
        List<string> attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(line).ToList().ConvertAll(z => z.ToString());
        //הוספת רשימה על רשימה כל האטריבויטים למאפיין אטריביוט שמכיל רשימה.-רשימה על רשימה 
        current.Attributes.AddRange(attributes);
        //בכל אלמנט נוכחי עוברים על הרשימה של האטריביוטים
        foreach (var a in current.Attributes)
        {  //בדיקה האם קיים id
            if (a.IndexOf("id") != -1)
            {
                //הוספה למאפיין ה
                //id
                var a1 = a.Substring(a.IndexOf('"')+1);
                var result2=current.Id=a1.TrimEnd('\"');
                Console.WriteLine(result2);
            }
            //בדיקה האם קיים class
            if (a.IndexOf("class") != -1)
            {
                // הוספה לרשימת
                // classes
                var cc = a.Substring(a.IndexOf('"') + 1);
                var tc = cc.TrimEnd('\"');
                current.Classes.Add(tc);
            }

        }
        //האובייקט הנוכחי מצביע על האבא
        current = current.parent;
    }
    else
    //אם השורה ריקה
        if  (!string.IsNullOrEmpty(line))
        current.InnerHtml = line;
}


async Task<string> Load(string url)
{
    HttpClient client = new HttpClient();
    var response = await client.GetAsync(url);
    var html = await response.Content.ReadAsStringAsync();
    return html;
}
foreach (var message1 in HtmlHelper.Instance.HtmlTags)
{
    Console.WriteLine(message1);
}
foreach (var message2 in HtmlHelper.Instance.HtmlVoidTags)
{
    Console.WriteLine(message2);
}
//הרצות - והפעלת הפונקציות
List<htmlElement> childrens = new List<htmlElement>();
var c = root.Descendants();

foreach (var ch in c)
{
    childrens.Add(ch);
}
htmlElement.Ancestors(root.chiLdren[0].chiLdren[0]);
HashSet<htmlElement> list = new HashSet<htmlElement>();
Selector se = Selector.Convert("link");
var result=htmlElement.Find(root, se,list);
result.ToList().ForEach(e =>Console.WriteLine(e));
//var set = new HashSet<Selector>();
