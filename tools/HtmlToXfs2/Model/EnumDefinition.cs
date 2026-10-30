namespace HtmlToXfs2.Model;

public class EnumDefinition
{
    public string RenamedName { get; set; } = "";
    public string FinalName { get; set; } = ""; // Final name after collision handling (may differ from RenamedName)
    public string Type { get; set; } = "";
    public List<string> Commands { get; set; } = new();
    public List<string> Values { get; set; } = new();
}

