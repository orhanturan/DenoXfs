using System;
using DenoXfs.Core.Convert;

namespace DenoXfs.Core.Extensions;

public static class XfsStructExtensions
{
    public static T ToStruct<T>(this IntPtr ptr) where T : struct
    {
        return IntPtrConvert.ToStruct<T>(ptr);
    }
    
    public static T? ToStructSafe<T>(this IntPtr ptr) where T : struct
    {
        return IntPtrConvert.ToStructSafe<T>(ptr);
    }
}

