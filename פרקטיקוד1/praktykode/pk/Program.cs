using System.Collections.Generic;
using System.CommandLine;
using System.ComponentModel;
using System.Timers;
//bundel
var bundelCommaned = new Command("bundel", "bundel code file to a signal file");
var bundelcreatersp = new Command("create-rsp", "eazy for the user");
// output
var bundleOutput = new Option<FileInfo>("--output", "file path and name");
bundelCommaned.AddOption(bundleOutput);
bundleOutput.AddAlias("-o");

// language
var bundlelanguage = new Option<string>("--language", "language that are allow");
bundelCommaned.AddOption(bundlelanguage);
bundlelanguage.AddAlias("-l");

//note
var bundlenote = new Option<bool>("--note", "note");
bundelCommaned.AddOption(bundlenote);
bundlenote.AddAlias("-n");

//sort
var bundleSort = new Option<bool>("--sort", "sort the files by abc");
bundelCommaned.AddOption(bundleSort);
bundleSort.AddAlias("-s");

//remove-empty-lines
var bundlerml = new Option<bool>("--rml", "remove empty lines");
bundelCommaned.AddOption(bundlerml);
bundlerml.AddAlias("-r");

//author
var bundleauthor = new Option<string>("--author", "the name of the outher");
bundelCommaned.AddOption(bundleauthor);
bundleauthor.AddAlias("-a");

bool sort = false;
bool note = false;
bool rml = false;

bundelcreatersp.SetHandler((creatersp) =>

{
    string p = "";
    //output
    Console.WriteLine("input a location");
    string l = Console.ReadLine();
    p += "-o " + l + " ";
    //language
    Console.WriteLine("input a language");
    string l1 = Console.ReadLine();
    p += "-l " + l1 + " ";
    Console.WriteLine("input note (t/f)");
    string l2 = Console.ReadLine();
    if (l2 == "t")
        p += "-n ";
    Console.WriteLine("input remove empty lines (t/f)");
    string l3 = Console.ReadLine();
    if (l3 == "t")
        p += "-r ";
    Console.WriteLine("input sort (t/f)");
    string l4 = Console.ReadLine();
    if (l4 == "t")
        p += "-s ";

    Console.WriteLine("input aouther name");
    string l5 = Console.ReadLine();
    if (l5 != null)
    {
        p += "-a " + l5;
    }
    File.WriteAllText("rsp.rsp", p);
}
);

// handler
bundelCommaned.SetHandler((output, language, sort, rml, author, note) =>
   {

       if (sort)
           sort = true;
       if (note)
           note = true;
       if (rml)
           rml = true;
       //output
       try
       {
           var file = File.Create(output.FullName);
           file.Close();
           Console.WriteLine("was created")
           ;
       }
       catch
       {
           Console.WriteLine("file path was not good");
       }


       string s = "";
       //author

       if (author!=null)
       {
           s += "author-" + author + "\n";
       }



   //language

   string[] languages = { "*.html", "*.css", "*.js", "*.sql", "*.java", "*.docx" };
   List<string> files = new List<string>();

       if (language.Contains(','))
       { 


           string[] lan = language.Split(',');
           foreach (var item in lan)
           {
               Console.WriteLine(item);
               files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*." + item, SearchOption.AllDirectories).ToList();

             
           }
       }





       else if (language == "all")
       {
           files = languages.SelectMany(e => Directory.GetFiles(Directory.GetCurrentDirectory(), e)).ToList();
           Console.WriteLine("the files are join!");
       }
       else if (languages.Contains("*." + language))
       {
           files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*." + language, SearchOption.AllDirectories).ToList();
           Console.WriteLine("the files are join!!" + "");

       }
       else {
           Console.WriteLine("language not exist");
       }
       foreach (var file in files)
       {
           s += File.ReadAllText(file);
       }
       //sort
       if (sort)
       {
           files = files.OrderBy(x => x).ToList();
       }
       Console.WriteLine("files:");
       foreach (string file in files)
       {

           Console.WriteLine(file);
       }

       foreach (string file in files)
       {
           //note
           if (note)
           {
               s += "note-" + output.FullName + "\n";
           }
           //lines
           if (rml)
           {
               var lines = File.ReadAllLines(file);
               string[] s1 = lines.Where(arg => !string.IsNullOrWhiteSpace(arg)).ToArray();
               foreach (string line in s1)
               {
                   s += line.ToString() + "\n";
               }
           }
           else
           {
               s += File.ReadAllText(file);
           }
       }

       File.WriteAllText(output.FullName, s);


   }, bundleOutput, bundlelanguage, bundleSort, bundlerml, bundleauthor, bundlenote);



//root
var rootCommaned = new RootCommand("root command for bundle");
rootCommaned.AddCommand(bundelCommaned);
rootCommaned.AddCommand(bundelcreatersp);
rootCommaned.InvokeAsync(args);
