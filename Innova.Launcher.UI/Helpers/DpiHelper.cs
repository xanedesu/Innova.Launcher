// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Helpers.DpiHelper
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace Innova.Launcher.UI.Helpers
{
  internal static class DpiHelper
  {
    private static readonly int DpiX;
    private static readonly int DpiY;
    private const double StandardDpiX = 96.0;
    private const double StandardDpiY = 96.0;

    static DpiHelper()
    {
      PropertyInfo property1 = typeof (SystemParameters).GetProperty(nameof (DpiX), BindingFlags.Static | BindingFlags.NonPublic);
      PropertyInfo property2 = typeof (SystemParameters).GetProperty("Dpi", BindingFlags.Static | BindingFlags.NonPublic);
      DpiHelper.DpiX = (int) property1.GetValue((object) null, (object[]) null);
      DpiHelper.DpiY = (int) property2.GetValue((object) null, (object[]) null);
    }

    public static double TransformToDeviceY(Visual visual, double y)
    {
      PresentationSource presentationSource = PresentationSource.FromVisual(visual);
      return presentationSource?.CompositionTarget != null ? y * presentationSource.CompositionTarget.TransformToDevice.M22 : DpiHelper.TransformToDeviceY(y);
    }

    public static double TransformToDeviceX(Visual visual, double x)
    {
      PresentationSource presentationSource = PresentationSource.FromVisual(visual);
      return presentationSource?.CompositionTarget != null ? x * presentationSource.CompositionTarget.TransformToDevice.M11 : DpiHelper.TransformToDeviceX(x);
    }

    public static double TransformToDeviceY(double y) => y * (double) DpiHelper.DpiY / 96.0;

    public static double TransformToDeviceX(double x) => x * (double) DpiHelper.DpiX / 96.0;
  }
}
