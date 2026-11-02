using DenoXfs.Api.Errors;
using DenoXfs.Idc.Errors;

namespace DenoXfs.TestTool.Operations;

/// <summary>Readable name of an XFS result code (0 = success, generic errors, IDC errors).</summary>
public static class ResultNames
{
    public static string Describe(int result)
    {
        if (result == 0)
            return "WFS_SUCCESS";
        if (Enum.IsDefined(typeof(XfsError), result))
            return $"{(XfsError)result} ({result})";
        if (Enum.IsDefined(typeof(IdcError), result))
            return $"{(IdcError)result} ({result})";
        return result.ToString();
    }
}
