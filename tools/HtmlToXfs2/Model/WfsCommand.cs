namespace HtmlToXfs2.Model;

public class WfsCommand
{
    public string CommandName { get; set; } = "";
    public string Description { get; set; } = "";
    public string Comments { get; set; } = "";
    public InputParam? InputParam { get; set; }
    public OutputParam? OutputParam { get; set; }
    public Dictionary<string, AdditionalStruct>? AdditionalStructs { get; set; }
}
