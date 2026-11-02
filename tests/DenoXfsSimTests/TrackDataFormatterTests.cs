using System.Text;
using DenoXfs.Sim.Core.Protocol;
using DenoXfs.Sim.Idc;
using DenoXfs.Sim.Idc.Forms;

namespace DenoXfs.Sim.Tests;

public class TrackDataFormatterTests
{
    private readonly TrackDataFormatter _formatter = new();

    [Fact]
    public void Track2_fields_are_keyword_value_pairs_separated_by_nulls_as_in_the_spec_example()
    {
        var result = _formatter.Format(FakeFormCatalog.Find("READTRACK2"),
            new Dictionary<int, string?> { [2] = "4111111111111111=25122010000000000000" });

        Assert.Equal(XfsResultCodes.Success, result.HResult);
        Assert.Equal("TRACK2:PAN=4111111111111111\0EXPIRY=2512\0SERVICECODE=201\0DISCRETIONARY=0000000000000\0\0\0",
            Encoding.Latin1.GetString(result.TrackData));
        Assert.Empty(result.MissingTracks);
    }

    [Fact]
    public void Or_form_succeeds_with_one_track_and_reports_the_missing_one()
    {
        var result = _formatter.Format(FakeFormCatalog.Find("READTRACK1ORTRACK2"),
            new Dictionary<int, string?> { [1] = null, [2] = "6045551234567890=2612101123456789" });

        Assert.Equal(XfsResultCodes.Success, result.HResult);
        Assert.Equal(new[] { 1 }, result.MissingTracks);
        Assert.StartsWith("TRACK2:PAN=6045551234567890\0EXPIRY=2612\0", Encoding.Latin1.GetString(result.TrackData));
    }

    [Fact]
    public void And_form_fails_with_invalid_data_when_a_track_is_missing()
    {
        var result = _formatter.Format(FakeFormCatalog.Find("READALLTRACKS"),
            new Dictionary<int, string?> { [1] = "B4111^TEST^2512", [2] = "4111=2512" });

        Assert.Equal(IdcConstants.ErrInvalidData, result.HResult);
        Assert.Equal(new[] { 3 }, result.MissingTracks);
    }

    [Fact]
    public void Track1_name_and_expiry_are_split_on_the_caret()
    {
        var result = _formatter.Format(FakeFormCatalog.Find("READTRACK1ORTRACK2"),
            new Dictionary<int, string?> { [1] = "B4111111111111111^TEST/CARD^25122010000000000000" });

        Assert.StartsWith("TRACK1:PAN=4111111111111111\0NAME=TEST/CARD\0EXPIRY=2512\0\0", Encoding.Latin1.GetString(result.TrackData));
    }
}
