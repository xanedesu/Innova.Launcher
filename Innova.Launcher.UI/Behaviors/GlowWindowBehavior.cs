// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.GlowWindowBehavior
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Native;
using ControlzEx.Standard;
using Innova.Launcher.UI.Controls;
using Microsoft.Xaml.Behaviors;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace Innova.Launcher.UI.Behaviors
{
  public class GlowWindowBehavior : Behavior<Window>
  {
    private static readonly TimeSpan GlowTimerDelay = TimeSpan.FromMilliseconds(200.0);
    private IntPtr _handle;
    private HwndSource _hwndSource;
    private GlowWindow _left;
    private GlowWindow _right;
    private GlowWindow _top;
    private GlowWindow _bottom;
    private DispatcherTimer _makeGlowVisibleTimer;
    private WINDOWPOS _prevWindowPos;

    private bool IsGlowDisabled => this.AssociatedObject is ModernWindow associatedObject && associatedObject.GlowBrush == null;

    protected override void OnAttached()
    {
      base.OnAttached();
      this.AssociatedObject.SourceInitialized += (EventHandler) ((o, args) =>
      {
        this._handle = new WindowInteropHelper(this.AssociatedObject).Handle;
        this._hwndSource = HwndSource.FromHwnd(this._handle);
        this._hwndSource?.AddHook(new HwndSourceHook(this.AssociatedObjectWindowProc));
      });
      this.AssociatedObject.Loaded += new RoutedEventHandler(this.AssociatedObjectOnLoaded);
      this.AssociatedObject.Unloaded += new RoutedEventHandler(this.AssociatedObjectUnloaded);
    }

    private void AssociatedObjectStateChanged(object sender, EventArgs e)
    {
      this._makeGlowVisibleTimer?.Stop();
      if (this.AssociatedObject.WindowState == WindowState.Normal)
      {
        bool flag = this.AssociatedObject is ModernWindow associatedObject && associatedObject.IgnoreTaskbarOnMaximize;
        if (this._makeGlowVisibleTimer != null && SystemParameters.MinimizeAnimation && !flag)
          this._makeGlowVisibleTimer.Start();
        else
          this.RestoreGlow();
      }
      else
        this.HideGlow();
    }

    private void AssociatedObjectUnloaded(object sender, RoutedEventArgs e)
    {
      if (this._makeGlowVisibleTimer == null)
        return;
      this._makeGlowVisibleTimer.Stop();
      this._makeGlowVisibleTimer.Tick -= new EventHandler(this.GlowVisibleTimerOnTick);
      this._makeGlowVisibleTimer = (DispatcherTimer) null;
    }

    private void GlowVisibleTimerOnTick(object sender, EventArgs e)
    {
      this._makeGlowVisibleTimer?.Stop();
      this.RestoreGlow();
    }

    private void RestoreGlow()
    {
      if (this._left != null)
        this._left.IsGlowing = true;
      if (this._top != null)
        this._top.IsGlowing = true;
      if (this._right != null)
        this._right.IsGlowing = true;
      if (this._bottom != null)
        this._bottom.IsGlowing = true;
      this.Update();
    }

    private void HideGlow()
    {
      if (this._left != null)
        this._left.IsGlowing = false;
      if (this._top != null)
        this._top.IsGlowing = false;
      if (this._right != null)
        this._right.IsGlowing = false;
      if (this._bottom != null)
        this._bottom.IsGlowing = false;
      this.Update();
    }

    private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
    {
      if (this.IsGlowDisabled)
        return;
      this.AssociatedObject.StateChanged -= new EventHandler(this.AssociatedObjectStateChanged);
      this.AssociatedObject.StateChanged += new EventHandler(this.AssociatedObjectStateChanged);
      if (this._makeGlowVisibleTimer == null)
      {
        this._makeGlowVisibleTimer = new DispatcherTimer()
        {
          Interval = GlowWindowBehavior.GlowTimerDelay
        };
        this._makeGlowVisibleTimer.Tick += new EventHandler(this.GlowVisibleTimerOnTick);
      }
      this._left = new GlowWindow(this.AssociatedObject, GlowDirection.Left);
      this._right = new GlowWindow(this.AssociatedObject, GlowDirection.Right);
      this._top = new GlowWindow(this.AssociatedObject, GlowDirection.Top);
      this._bottom = new GlowWindow(this.AssociatedObject, GlowDirection.Bottom);
      this.Show();
      this.Update();
      this.AssociatedObject.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, (Delegate) (() => this.SetOpacityTo(1.0)));
    }

    private void Update()
    {
      if ((this._left == null || this._right == null || this._top == null ? 0 : (this._bottom != null ? 1 : 0)) == 0)
        return;
      this._left.Update();
      this._right.Update();
      this._top.Update();
      this._bottom.Update();
    }

    private void UpdateCore()
    {
      ControlzEx.Standard.RECT lpRect;
      if ((this._left == null || this._right == null || this._top == null || this._bottom == null || !this._left.CanUpdateCore() || !this._right.CanUpdateCore() || !this._top.CanUpdateCore() ? 0 : (this._bottom.CanUpdateCore() ? 1 : 0)) == 0 || !(this._handle != IntPtr.Zero) || !UnsafeNativeMethods.GetWindowRect(this._handle, out lpRect))
        return;
      this._left.UpdateCore(lpRect);
      this._right.UpdateCore(lpRect);
      this._top.UpdateCore(lpRect);
      this._bottom.UpdateCore(lpRect);
    }

    private void SetOpacityTo(double newOpacity)
    {
      if ((this._left == null || this._right == null || this._top == null ? 0 : (this._bottom != null ? 1 : 0)) == 0)
        return;
      this._left.Opacity = newOpacity;
      this._right.Opacity = newOpacity;
      this._top.Opacity = newOpacity;
      this._bottom.Opacity = newOpacity;
    }

    private void Show()
    {
      this._left?.Show();
      this._right?.Show();
      this._top?.Show();
      this._bottom?.Show();
    }

    private IntPtr AssociatedObjectWindowProc(
      IntPtr hwnd,
      int msg,
      IntPtr wParam,
      IntPtr lParam,
      ref bool handled)
    {
      if (this._hwndSource?.RootVisual == null)
        return IntPtr.Zero;
      switch ((WM) msg)
      {
        case WM.SIZE:
        case WM.SIZING:
          this.UpdateCore();
          break;
        case WM.WINDOWPOSCHANGING:
        case WM.WINDOWPOSCHANGED:
          WINDOWPOS structure = (WINDOWPOS) Marshal.PtrToStructure(lParam, typeof (WINDOWPOS));
          if (!structure.Equals(this._prevWindowPos))
            this.UpdateCore();
          this._prevWindowPos = structure;
          break;
      }
      return IntPtr.Zero;
    }
  }
}
