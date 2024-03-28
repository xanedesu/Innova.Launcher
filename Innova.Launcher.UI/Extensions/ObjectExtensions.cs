// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Extensions.ObjectExtensions
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace Innova.Launcher.UI.Extensions
{
  public static class ObjectExtensions
  {
    public static void Gui(this object sender, Action action) => ObjectExtensions.GetDispatcher().Invoke(action);

    public static T Gui<T>(this object sender, Func<T> action) => ObjectExtensions.GetDispatcher().Invoke<T>(action);

    public static void GuiNoWait(this object sender, Action action) => ObjectExtensions.GetDispatcher().BeginInvoke((Delegate) action, Array.Empty<object>());

    public static async Task GuiAsync(
      this object sender,
      Action action,
      DispatcherPriority priority)
    {
      await ObjectExtensions.GetDispatcher().InvokeAsync(action, priority);
    }

    public static async Task GuiAsync(this object sender, Action action) => await ObjectExtensions.GetDispatcher().InvokeAsync(action);

    public static async Task<T> GuiAsync<T>(
      this object sender,
      Func<T> action,
      DispatcherPriority priority)
    {
      return await ObjectExtensions.GetDispatcher().InvokeAsync<T>(action, priority);
    }

    public static async Task<T> GuiAsync<T>(this object sender, Func<T> action) => await ObjectExtensions.GetDispatcher().InvokeAsync<T>(action);

    private static Dispatcher GetDispatcher() => Application.Current.Dispatcher ?? Dispatcher.CurrentDispatcher;
  }
}
