using HtmlToXfs2.Services;
using Xunit;

namespace HtmlToXfs2.Tests;

public class TypePlanningTests
{
    private const string Header = """
        #define IDC_SERVICE_OFFSET (200)
        #define WFS_ERR_IDC_NOMEDIA (-(IDC_SERVICE_OFFSET + 1))
        #define WFS_STAT_DEVONLINE (0)

        /* values of WFSIDCSTATUS.fwMedia,
           WFSIDCCARDACT.wPosition */
        #define WFS_IDC_MEDIAPRESENT (1)
        #define WFS_IDC_MEDIAJAMMED (2)

        /* values of WFSIDCSTATUS.fwRetainBin */
        #define WFS_IDC_RETAINBINOK (1)
        #define WFS_IDC_RETAINBINFULL (2)

        /* Values of WFSIDCCAPS.fwReadTracks */
        #define WFS_IDC_TRACK1 0x0001
        #define WFS_IDC_TRACK2 0x0002

        typedef struct _wfs_idc_status
        {
            WORD fwMedia;
            WORD fwRetainBin;
            ULONG ulLength;
            LPBYTE lpbData;
        } WFSIDCSTATUS, *LPWFSIDCSTATUS;

        typedef struct _wfs_idc_card_act
        {
            WORD wPosition;
        } WFSIDCCARDACT, *LPWFSIDCCARDACT;

        typedef struct _wfs_idc_caps
        {
            WORD fwReadTracks;
        } WFSIDCCAPS, *LPWFSIDCCAPS;
        """;

    private static SectionDoc Doc(string file, string type, string text, string? command = null) => new(file, type, text, command);

    private static TypePlan Plan(params SectionDoc[] docs)
    {
        var model = HeaderParser.Parse(Header, "idc");
        return TypePlanner.Plan(model, null, null, docs, new NamingOverrides());
    }

    [Theory]
    [InlineData("wfs_idc_status", "IdcStatus")]
    [InlineData("wfs_idc_emvcless_tx_data_output", "IdcEmvclessTxDataOutput")]
    [InlineData("wfs_idc_app_data", "IdcAppData")]
    public void DeviceStructName_UsesTagUnderscores(string tag, string expected)
    {
        Assert.Equal(expected, NameConverter.DeviceStructName(tag));
    }

    [Theory]
    [InlineData("fwRetainBin", "RetainBin")]
    [InlineData("lpwParkingStationMedia", "ParkingStationMedia")]
    [InlineData("wIFMAuthority", "IFMAuthority")]
    [InlineData("RequestID", "RequestID")]
    public void ToPropertyName_StripsHungarianPrefix(string field, string expected)
    {
        Assert.Equal(expected, NameConverter.ToPropertyName(field));
    }

    [Theory]
    [InlineData("SRCNOTSUPP", "SrcNotSupp")]
    [InlineData("SRCMISSING", "SrcMissing")]
    [InlineData("TOOLONG", "TooLong")]
    [InlineData("TOOSHORT", "TooShort")]
    [InlineData("EXITPOSITION", "ExitPosition")]
    [InlineData("TRANSPORTPOSITION", "TransportPosition")]
    [InlineData("INPOSITION", "InPosition")]
    [InlineData("EJECTTHENRETAIN", "EjectThenRetain")]
    [InlineData("TYPELATCHEDDIP", "TypeLatchedDip")]
    [InlineData("TYPEINTELLIGENTCONTACTLESS", "TypeIntelligentContactless")]
    [InlineData("APPLICATIONS", "Applications")]
    [InlineData("PROTOCOLNOTSUPP", "ProtocolNotSupp")]
    [InlineData("SHUTTERFAIL", "ShutterFail")]
    [InlineData("PROCESSINGERROR", "ProcessingError")]
    [InlineData("CONFIRMATIONREQUIRED", "ConfirmationRequired")]
    [InlineData("CONFIRMATIONCODEVERIFIED", "ConfirmationCodeVerified")]
    [InlineData("ENDAPPLICATION", "EndApplication")]
    [InlineData("INTELLIGENTCONTACTLESS", "IntelligentContactless")]
    [InlineData("HOLDCARD", "HoldCard")]
    [InlineData("TRYAGAIN", "TryAgain")]
    [InlineData("MULTIPLECARDS", "MultipleCards")]
    [InlineData("INF_IDC_STATUS", "InfIdcStatus")]
    [InlineData("NOTINPOSITION", "NotInPosition")]
    [InlineData("INVALIDDATA", "InvalidData")]
    [InlineData("READERNOTCONFIGURED", "ReaderNotConfigured")]
    [InlineData("INVALIDAIDDATA", "InvalidAidData")]
    [InlineData("APPROVE", "Approve")]
    [InlineData("NOCVM", "NoCvm")]
    [InlineData("NOCVMPREFERENCE", "NoCvmPreference")]
    [InlineData("ONLINEREQUESTCOMPLETIONREQUIRED", "OnlineRequestCompletionRequired")]
    public void ToPascalCase_SplitsKnownXfsFragments(string input, string expected)
    {
        Assert.Equal(expected, NameConverter.ToPascalCase(input));
    }

    [Fact]
    public void Parser_ReadsDefinesGroupsAndStructs()
    {
        var model = HeaderParser.Parse(Header, "idc");

        var media = Assert.Single(model.Groups, g => g.DefineNames.Contains("WFS_IDC_MEDIAPRESENT"));
        Assert.Equal(new[] { "WFSIDCSTATUS.fwMedia", "WFSIDCCARDACT.wPosition" }, media.Owners.Select(o => $"{o.TypedefName}.{o.Field}"));
        Assert.Equal(3, model.Structs.Count);
        Assert.Equal("wfs_idc_status", model.Structs[0].Tag);
        Assert.Equal("LPWFSIDCSTATUS", model.Structs[0].PointerTypedefName);
    }

    [Fact]
    public void Resolver_EvaluatesNegatedExpressions()
    {
        var resolver = new ConstantResolver(HeaderParser.Parse(Header, "idc"));

        Assert.Equal(-201, resolver.Resolve("WFS_ERR_IDC_NOMEDIA")?.Number);
        Assert.True(resolver.Resolve("WFS_IDC_TRACK1")?.IsHex);
    }

    [Fact]
    public void Placement_SingleSectionIsLocal_MultipleSectionsAreShared()
    {
        var plan = Plan(
            Doc("4.1-status.md", "info_command", "typedef struct _wfs_idc_status ... WFSIDCSTATUS", "WFS_INF_IDC_STATUS"),
            Doc("6.1-act.md", "event", "typedef struct _wfs_idc_card_act ... WFSIDCCARDACT"),
            Doc("5.4-retain.md", "execute_command", "LPWFSIDCCARDACT lpAct"));

        var status = plan.Structs.Single(s => s.Name == "XfsIdcStatus");
        Assert.Equal("Infos", status.Group);
        Assert.Equal("Infos/Status", status.Folder);
        Assert.Equal("Shared", plan.Structs.Single(s => s.Name == "XfsIdcCardAct").Group);
        Assert.Equal("Shared", plan.Structs.Single(s => s.Name == "XfsIdcCaps").Group);
    }

    [Fact]
    public void Enums_AreNamedAfterOwnerAndPlacedByOwnerGroups()
    {
        var plan = Plan(
            Doc("4.1-status.md", "info_command", "WFSIDCSTATUS", "WFS_INF_IDC_STATUS"),
            Doc("6.1-act.md", "event", "WFSIDCCARDACT"));

        var retainBin = plan.Enums.Single(e => e.Name == "IdcRetainBinStatus");
        Assert.Equal("Infos", retainBin.Group);
        Assert.Equal("Infos/Status", retainBin.Folder);
        Assert.Equal(new[] { "Full", "Ok" }.Order(), retainBin.Members.Select(m => m.Name).Where(n => n is "Full" or "Ok").Order());

        var media = plan.Enums.Single(e => e.Name == "IdcMedia");
        Assert.Equal("Shared", media.Group);
        Assert.Equal(2, media.Owners.Count);

        Assert.True(plan.Enums.Single(e => e.Name == "IdcReadTracksCapabilities").IsFlags);
    }

    [Fact]
    public void Fields_BindEnumsAndDetectLengthPrefixedBytes()
    {
        var plan = Plan(Doc("4.1-status.md", "info_command", "WFSIDCSTATUS"));
        var fields = plan.Structs.Single(s => s.Name == "XfsIdcStatus").Fields.ToDictionary(f => f.Name, f => f.Type);

        Assert.Equal("#IdcMedia", fields["fwMedia"]);
        Assert.Equal("*BYTE[ulLength]", fields["lpbData"]);
    }

    [Fact]
    public void Overrides_RenameAndMergeEnums()
    {
        var model = HeaderParser.Parse(Header, "idc");
        var overrides = new NamingOverrides
        {
            EnumNames = { ["IdcMedia"] = "Media" },
            EnumMerges = { ["Tracks"] = new List<string> { "IdcRetainBinStatus", "IdcReadTracksCapabilities" } }
        };

        var plan = TypePlanner.Plan(model, null, null, Array.Empty<SectionDoc>(), overrides);

        Assert.Contains(plan.Enums, e => e.Name == "Media");
        var tracks = plan.Enums.Single(e => e.Name == "Tracks");
        Assert.Equal(4, tracks.Members.Count);
    }

    [Fact]
    public void Errors_AreCollectedFromNegativeDefines()
    {
        var plan = Plan();

        var errors = plan.Enums.Single(e => e.Name == "IdcError");
        Assert.Equal("Errors", errors.Group);
        Assert.Equal("-201", Assert.Single(errors.Members).Value);
    }

    [Fact]
    public void CapsEnum_UsesCapabilityOrCapabilities()
    {
        var plan = Plan(Doc("4.2-caps.md", "info_command", "WFSIDCCAPS", "WFS_INF_IDC_CAPABILITIES"));

        var tracks = plan.Enums.Single(e => e.Name == "IdcReadTracksCapabilities");
        Assert.Equal("Infos/Capabilities", tracks.Folder);
        Assert.Equal("Infos", tracks.Group);
    }

    [Fact]
    public void CommandIds_ReferenceConstantsAndDropCategoryPrefix()
    {
        const string header = """
            #define IDC_SERVICE_OFFSET (200)
            #define WFS_INF_IDC_STATUS (IDC_SERVICE_OFFSET + 1)
            #define WFS_INF_IDC_CAPABILITIES (IDC_SERVICE_OFFSET + 2)
            #define WFS_CMD_IDC_EJECT_CARD (IDC_SERVICE_OFFSET + 3)
            #define WFS_EXEE_IDC_MEDIARETAINED (IDC_SERVICE_OFFSET + 8)
            #define WFS_SRVE_IDC_MEDIARETAINED (IDC_SERVICE_OFFSET + 17)
            """;

        var plan = TypePlanner.Plan(HeaderParser.Parse(header, "idc"), null, null, Array.Empty<SectionDoc>(), new NamingOverrides());

        var info = plan.Enums.Single(e => e.Name == "IdcInfoId");
        Assert.Equal("", info.Group);
        Assert.Equal("Status", info.Members[0].Name);
        Assert.Equal("Idc.InfIdcStatus", info.Members[0].Value);
        Assert.Equal("Capabilities", info.Members[1].Name);

        var command = plan.Enums.Single(e => e.Name == "IdcCommandId");
        Assert.Equal("EjectCard", Assert.Single(command.Members).Name);
        Assert.Equal("Idc.CmdIdcEjectCard", command.Members[0].Value);

        var events = plan.Enums.Single(e => e.Name == "IdcEventId");
        Assert.Contains(events.Members, m => m.Name == "ExeeMediaRetained" && m.Value == "Idc.ExeeIdcMediaRetained");
        Assert.Contains(events.Members, m => m.Name == "SrveMediaRetained" && m.Value == "Idc.SrveIdcMediaRetained");
    }
}
