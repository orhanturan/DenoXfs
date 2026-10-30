namespace HtmlToXfs2.Model;

public class EnumInfo
{
    public string Name { get; set; } = ""; // Final enum name (after collision handling, same as key if not renamed)
    public string OriginalName { get; set; } = "";
    public string Type { get; set; } = "";
    public List<string> Commands { get; set; } = new();
    public List<string> Values { get; set; } = new();
}


