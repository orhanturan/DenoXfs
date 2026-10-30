namespace HtmlToXfs2.Model;

public class StructDefinition
{
    public string RenamedName { get; set; } = "";
    public Dictionary<string, string> Fields { get; set; } = new();
}

