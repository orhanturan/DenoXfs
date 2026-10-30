namespace HtmlToXfs2.Model;

public class InputParam
{
    public string Value { get; set; } = "";
    public string? OriginalParamType { get; set; }
    public string ParamType { get; set; } = "";
    public string? OriginalName { get; set; }
    public string Name { get; set; } = "";
    public string? OriginalStructureName { get; set; }
    public string? StructureName { get; set; }
    public Dictionary<string, string>? Fields { get; set; }
    public Dictionary<string, EnumInfo>? Enums { get; set; }
}

