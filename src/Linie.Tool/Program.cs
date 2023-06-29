namespace Linie.Tool;

using Antlr4.StringTemplate;

public class Program 
{
    public static void Main(string[] args)
    {
        var path = @"D:\basp\linie\src\Linie.Tool\vectors.stg";
        var g = new TemplateGroupFile(path);
        var t = g.GetInstanceOf("vec2");
        var s = t.Render();
        Console.WriteLine(s);
    }
}