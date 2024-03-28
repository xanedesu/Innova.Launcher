// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Overlay.OverlayHostEx
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Innova.Launcher.UI.Controls.Overlay
{
  public static class OverlayHostEx
  {
    public static async Task<object> ShowOverlay(this Window window, object content) => await OverlayHostEx.GetFirstOverlayHost(window).ShowInternal(content, (EventHandler<OverlayOpenedEventArgs>) null, (EventHandler<OverlayClosingEventArgs>) null);

    public static async Task<object> ShowOverlay(
      this Window window,
      object content,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler)
    {
      return await OverlayHostEx.GetFirstOverlayHost(window).ShowInternal(content, openedEventHandler, (EventHandler<OverlayClosingEventArgs>) null);
    }

    public static async Task<object> ShowOverlay(
      this Window window,
      object content,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      return await OverlayHostEx.GetFirstOverlayHost(window).ShowInternal(content, (EventHandler<OverlayOpenedEventArgs>) null, closingEventHandler);
    }

    public static async Task<object> ShowOverlay(
      this Window window,
      object content,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      return await OverlayHostEx.GetFirstOverlayHost(window).ShowInternal(content, openedEventHandler, closingEventHandler);
    }

    public static async Task<object> ShowOverlay(
      this DependencyObject childDependencyObject,
      object content)
    {
      return await OverlayHostEx.GetOwningOverlayHost(childDependencyObject).ShowInternal(content, (EventHandler<OverlayOpenedEventArgs>) null, (EventHandler<OverlayClosingEventArgs>) null);
    }

    public static async Task<object> ShowOverlay(
      this DependencyObject childDependencyObject,
      object content,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler)
    {
      return await OverlayHostEx.GetOwningOverlayHost(childDependencyObject).ShowInternal(content, openedEventHandler, (EventHandler<OverlayClosingEventArgs>) null);
    }

    public static async Task<object> ShowOverlay(
      this DependencyObject childDependencyObject,
      object content,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      return await OverlayHostEx.GetOwningOverlayHost(childDependencyObject).ShowInternal(content, (EventHandler<OverlayOpenedEventArgs>) null, closingEventHandler);
    }

    public static async Task<object> ShowOverlay(
      this DependencyObject childDependencyObject,
      object content,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      return await OverlayHostEx.GetOwningOverlayHost(childDependencyObject).ShowInternal(content, openedEventHandler, closingEventHandler);
    }

    private static OverlayHost GetFirstOverlayHost(Window window) => (window != null ? window.VisualDepthFirstTraversal().OfType<OverlayHost>().FirstOrDefault<OverlayHost>() : throw new ArgumentNullException(nameof (window))) ?? throw new InvalidOperationException("Unable to find a OverlayHost in visual tree");

    private static OverlayHost GetOwningOverlayHost(DependencyObject childDependencyObject) => (childDependencyObject != null ? childDependencyObject.GetVisualAncestry().OfType<OverlayHost>().FirstOrDefault<OverlayHost>() : throw new ArgumentNullException(nameof (childDependencyObject))) ?? throw new InvalidOperationException("Unable to find a OverlayHost in visual tree ancestory");
  }
}
