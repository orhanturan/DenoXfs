using System;
using System.Runtime.InteropServices;

namespace DenoXfs.Core.Services;

public class MessageWindow
{
    private const uint WM_QUIT = 0x0012;
    private const uint WS_OVERLAPPED = 0x00000000;
    private const int CW_USEDEFAULT = unchecked((int)0x80000000);

    public IntPtr Create()
    {
        const string className = "XfsMessageWindow";

        WNDCLASS wc = new WNDCLASS
        {
            lpfnWndProc = WindowProc,
            hInstance = GetModuleHandle(null),
            lpszClassName = className
        };

        RegisterClass(ref wc);

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

    [DllImport("user32.dll", SetLastError = true)]
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

    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    private static extern ushort RegisterClass(ref WNDCLASS lpWndClass);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string? lpModuleName);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
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
        [MarshalAs(UnmanagedType.LPStr)]
        public string? lpszMenuName;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpszClassName;
    }

    private delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
}






