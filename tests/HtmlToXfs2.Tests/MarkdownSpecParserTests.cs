using HtmlToXfs2.Services;
using Xunit;

namespace HtmlToXfs2.Tests;

public class MarkdownSpecParserTests
{
    [Fact]
    public void ParseDevice_ShouldExtractStatusCommandFromMarkdown()
    {
        var mdPath = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "tools", "HtmlToXfs2", "pdf", "340", "md_output", "XfsIdc.md");
        var parser = new MarkdownSpecParser();

        var commands = parser.ParseDevice(mdPath, "idc");
        var status = commands.FirstOrDefault(c => c.CommandName == "WFS_INF_IDC_STATUS");

        Assert.NotNull(status);
        Assert.NotNull(status!.OutputParam);
        Assert.Contains("fwDevice", status.OutputParam.Fields!.Keys);
        // Enum membership comes from the C header (TypePlanner); the markdown parser only reports spec-only groups.
    }
}
