// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Extensions.WindowExtensions
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.Windows;
using System.Windows.Interop;

namespace Innova.Launcher.UI.Extensions
{
  public static class WindowExtensions
  {
    public static IntPtr GetHandle(this Window window) => new WindowInteropHelper(window).Handle;

    public static void MakeResizable(
      this Window window,
      double? width,
      double? height,
      double? minWidth,
      double? minHeight)
    {
      Window window1 = window;
      double? nullable = width;
      double width1 = nullable ?? window.Width;
      nullable = height;
      double height1 = nullable ?? window.Height;
      nullable = minWidth;
      double? minWidth1 = new double?(nullable ?? window.MinWidth);
      nullable = minHeight;
      double? minHeight1 = new double?(nullable ?? window.MinHeight);
      ResizeMode? resizeMode = new ResizeMode?(ResizeMode.CanResize);
      window1.ChangeWindowSize(width1, height1, minWidth1, minHeight1, resizeMode);
    }

    public static void MakeNoResizable(this Window window, double? width, double? height)
    {
      ref double? local1 = ref width;
      double? nullable = width;
      double num1 = nullable ?? window.ActualWidth;
      local1 = new double?(num1);
      ref double? local2 = ref height;
      nullable = height;
      double num2 = nullable ?? window.ActualHeight;
      local2 = new double?(num2);
      window.ChangeWindowSize(width.Value, height.Value, width, height, new ResizeMode?(ResizeMode.NoResize));
    }

    public static void StartNativeDrag(this Window window)
    {
      IntPtr handle = window.GetHandle();
      UnsafeNativeMethods.ReleaseCapture();
      IntPtr wParam = new IntPtr(2);
      IntPtr zero = IntPtr.Zero;
      NativeMethods.SendMessage(handle, WM.NCLBUTTONDOWN, wParam, zero);
    }

    public static void ChangeWindowSize(
      this Window window,
      double width,
      double height,
      double? minWidth,
      double? minHeight,
      ResizeMode? resizeMode)
    {
      window.ResizeMode = (ResizeMode) ((int) resizeMode ?? (int) window.ResizeMode);
      double num1 = width - window.ActualWidth;
      window.Left -= num1 / 2.0;
      double num2 = height - window.ActualHeight;
      window.Top -= num2 / 2.0;
      window.Width = width;
      window.Height = height;
      Window window1 = window;
      double? nullable = minWidth;
      double num3 = nullable ?? window.MinWidth;
      window1.MinWidth = num3;
      Window window2 = window;
      nullable = minHeight;
      double num4 = nullable ?? window.MinHeight;
      window2.MinHeight = num4;
    }
  }
}
