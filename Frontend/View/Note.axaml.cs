using Avalonia.Controls;
using Avalonia.Input;
using System;
using System.Runtime.InteropServices;

namespace StickieNotes.View;

public partial class Note : Window
{
    public event Action<Note>? NoteActivated;


    public Note()
    {
        InitializeComponent();

        ShowInTaskbar = false;

        PointerPressed += (_, _) => NoteActivated?.Invoke(this);
        Activated += (_, _) => NoteActivated?.Invoke(this);

        Opened += (_, _) => ApplyWindowStyles();


    }
    private void ApplyWindowStyles()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        var platformHandle = TryGetPlatformHandle()?.Handle;
        if (platformHandle.HasValue && platformHandle.Value != IntPtr.Zero)
        {
            IntPtr windowHandle = platformHandle.Value;

            // Keeps sticky notes out of Alt+Tab while preserving main app behavior.
            int exStyle = WinApi.GetWindowExStyle(windowHandle);
            WinApi.SetWindowExStyle(windowHandle, exStyle | WinApi.WS_EX_TOOLWINDOW);
        }
    }

    private void CloseWindow(object? sender, Avalonia.Interactivity.RoutedEventArgs e) => Close();
    private void DragWindow(object? sender, PointerPressedEventArgs e) => BeginMoveDrag(e);
}

public static class WinApi
{
    public const int WS_EX_TOOLWINDOW = 0x00000080;
    public const int GWL_EXSTYLE = -20;

    [DllImport("user32.dll")]
    public static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    public static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    public static int GetWindowExStyle(IntPtr hwnd) =>
        (int)GetWindowLongPtr(hwnd, GWL_EXSTYLE);

    public static int SetWindowExStyle(IntPtr hwnd, int newStyle) =>
        (int)SetWindowLongPtr(hwnd, GWL_EXSTYLE, (IntPtr)newStyle);
}