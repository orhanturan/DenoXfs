namespace HtmlToXfs2.Model;

public class AdditionalStruct
{
    public string OriginalStructureName { get; set; } = "";
    public string StructureName { get; set; } = "";
    public Dictionary<string, string>? Fields { get; set; }
    public Dictionary<string, EnumInfo>? Enums { get; set; }
}











