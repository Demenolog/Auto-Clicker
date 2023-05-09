﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using WpfTest.Infrastructure.Constans.MouseClass;

namespace WpfTest.Models.UnsafeCode
{
    internal static class User32
    {
        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(out Point point);

        [DllImport("user32.dll")]
        internal static extern short GetKeyState(MouseClassConstans.VirtualKeyStates nVirtKey);

        [DllImport("user32.dll")]
        internal static extern void mouse_event(int dwFlags, int xAxis, int yAxis, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        internal static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        internal static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
  
        [DllImport("user32.dll")]
        internal static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}