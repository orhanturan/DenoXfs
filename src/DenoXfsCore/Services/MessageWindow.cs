using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Core.Services;

public class MessageWindow
{
    private const uint WM_QUIT = 0x0012;
    private const int ERROR_CLASS_ALREADY_EXISTS = 1410;
    private const uint WS_OVERLAPPED = 0x00000000;
    private const int CW_USEDEFAULT = unchecked((int)0x80000000);

    // Kept in a field: the native window procedure points into this delegate, so it must not be garbage collected.
    private readonly WndProc windowProc;

    public MessageWindow()
    {
        windowProc = WindowProc;
    }

    /// <summary>Creates the hidden window on the calling thread. Returns zero on failure; see <c>Marshal.GetLastPInvokeError</c>.</summary>
    public IntPtr Create()
    {
        const string className = "XfsMessageWindow";

        WNDCLASS wc = new WNDCLASS
        {
            lpfnWndProc = windowProc,
            hInstance = GetModuleHandle(null),
            lpszClassName = className
        };

        if (RegisterClass(ref wc) == 0 && Marshal.GetLastPInvokeError() != ERROR_CLASS_ALREADY_EXISTS)
            return IntPtr.Zero;

        return CreateWindowEx(
            0,
            className,
            "XFS Message Window",
            WS_OVERLAPPED,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            CW_USEDEFAULT,
            IntPtr.Zero,
            IntPtr.Zero,
            GetModuleHandle(null),
            IntPtr.Zero
        );
    }

    public void Destroy(IntPtr windowHandle)
    {
        if (windowHandle != IntPtr.Zero)
        {
            DestroyWindow(windowHandle);
        }
    }

    public void PostQuit(IntPtr windowHandle)
    {
        if (windowHandle != IntPtr.Zero)
        {
            PostMessage(windowHandle, WM_QUIT, IntPtr.Zero, IntPtr.Zero);
        }
    }

    private IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        return DefWindowProc(hWnd, msg, wParam, lParam);
    }

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern IntPtr CreateWindowEx(
        uint dwExStyle,
        string lpClassName,
        string lpWindowName,
        uint dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam
    );

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern IntPtr DefWindowProc(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    private static extern ushort RegisterClass(ref WNDCLASS lpWndClass);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string? lpModuleName);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct WNDCLASS
    {
        public uint style;
        [MarshalAs(UnmanagedType.FunctionPtr)]
        public WndProc lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string? lpszMenuName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string lpszClassName;
    }

    private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}






