// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.GlowWindow
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx;
using ControlzEx.Native;
using ControlzEx.Standard;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Innova.Launcher.UI.Controls
{
  public partial class GlowWindow : Window, IComponentConnector
  {
    private const double edgeSize = 20.0;
    private const double glowSize = 6.0;
    private readonly Func<System.Windows.Point, ControlzEx.Standard.RECT, Cursor> getCursor;
    private readonly Func<ControlzEx.Standard.RECT, double> getHeight;
    private readonly Func<System.Windows.Point, ControlzEx.Standard.RECT, HT> getHitTestValue;
    private readonly Func<ControlzEx.Standard.RECT, double> getLeft;
    private readonly Func<ControlzEx.Standard.RECT, double> getTop;
    private readonly Func<ControlzEx.Standard.RECT, double> getWidth;
    private readonly Window _owner;
    private bool closing;
    private IntPtr handle;
    private HwndSource hwndSource;
    private IntPtr ownerHandle;
    private PropertyChangeNotifier resizeModeChangeNotifier;
    internal GlowWindow glowWindow;
    private Glow glow;
    private bool _contentLoaded;

    public Storyboard OpacityStoryboard { get; set; }

    public bool IsGlowing { set; get; }

    public GlowWindow(Window owner, GlowDirection direction)
    {
      this.InitializeComponent();
      this.Owner = owner;
      this._owner = owner;
      this.IsGlowing = true;
      this.AllowsTransparency = true;
      this.Closing += (CancelEventHandler) ((sender, e) => e.Cancel = !this.closing);
      this.ShowInTaskbar = false;
      this.glow.Visibility = Visibility.Collapsed;
      Binding binding1 = new Binding("GlowBrush")
      {
        Source = (object) owner
      };
      this.glow.SetBinding(Glow.GlowBrushProperty, (BindingBase) binding1);
      Binding binding2 = new Binding("NonActiveGlowBrush")
      {
        Source = (object) owner
      };
      this.glow.SetBinding(Glow.NonActiveGlowBrushProperty, (BindingBase) binding2);
      Binding binding3 = new Binding("BorderThickness")
      {
        Source = (object) owner
      };
      this.glow.SetBinding(Control.BorderThicknessProperty, (BindingBase) binding3);
      this.glow.Direction = direction;
      switch (direction)
      {
        case GlowDirection.Left:
          this.glow.Orientation = Orientation.Vertical;
          this.glow.HorizontalAlignment = HorizontalAlignment.Right;
          this.getLeft = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) rect.Left - 6.0 + 1.0);
          this.getTop = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Top - 2));
          this.getWidth = (Func<ControlzEx.Standard.RECT, double>) (rect => 6.0);
          this.getHeight = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Height + 4));
          this.getHitTestValue = (Func<System.Windows.Point, ControlzEx.Standard.RECT, HT>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, (double) rect.Width, 20.0).Contains(p))
              return HT.TOPLEFT;
            return !new Rect(0.0, (double) (rect.Height + 4) - 20.0, (double) rect.Width, 20.0).Contains(p) ? HT.LEFT : HT.BOTTOMLEFT;
          });
          this.getCursor = (Func<System.Windows.Point, ControlzEx.Standard.RECT, Cursor>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, (double) rect.Width, 20.0).Contains(p))
              return Cursors.SizeNWSE;
            return !new Rect(0.0, (double) (rect.Height + 4) - 20.0, (double) rect.Width, 20.0).Contains(p) ? Cursors.SizeWE : Cursors.SizeNESW;
          });
          break;
        case GlowDirection.Right:
          this.glow.Orientation = Orientation.Vertical;
          this.glow.HorizontalAlignment = HorizontalAlignment.Left;
          this.getLeft = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Right - 1));
          this.getTop = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Top - 2));
          this.getWidth = (Func<ControlzEx.Standard.RECT, double>) (rect => 6.0);
          this.getHeight = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Height + 4));
          this.getHitTestValue = (Func<System.Windows.Point, ControlzEx.Standard.RECT, HT>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, (double) rect.Width, 20.0).Contains(p))
              return HT.TOPRIGHT;
            return !new Rect(0.0, (double) (rect.Height + 4) - 20.0, (double) rect.Width, 20.0).Contains(p) ? HT.RIGHT : HT.BOTTOMRIGHT;
          });
          this.getCursor = (Func<System.Windows.Point, ControlzEx.Standard.RECT, Cursor>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, (double) rect.Width, 20.0).Contains(p))
              return Cursors.SizeNESW;
            return !new Rect(0.0, (double) (rect.Height + 4) - 20.0, (double) rect.Width, 20.0).Contains(p) ? Cursors.SizeWE : Cursors.SizeNWSE;
          });
          break;
        case GlowDirection.Top:
          this.PreviewMouseDoubleClick += (MouseButtonEventHandler) ((sender, e) =>
          {
            if (!(this.ownerHandle != IntPtr.Zero))
              return;
            NativeMethods.SendMessage(this.ownerHandle, WM.NCLBUTTONDBLCLK, (IntPtr) 12, IntPtr.Zero);
          });
          this.glow.Orientation = Orientation.Horizontal;
          this.glow.VerticalAlignment = VerticalAlignment.Bottom;
          this.getLeft = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Left - 2));
          this.getTop = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) rect.Top - 6.0 + 1.0);
          this.getWidth = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Width + 4));
          this.getHeight = (Func<ControlzEx.Standard.RECT, double>) (rect => 6.0);
          this.getHitTestValue = (Func<System.Windows.Point, ControlzEx.Standard.RECT, HT>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, 14.0, (double) rect.Height).Contains(p))
              return HT.TOPLEFT;
            return !new Rect((double) (rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double) rect.Height).Contains(p) ? HT.TOP : HT.TOPRIGHT;
          });
          this.getCursor = (Func<System.Windows.Point, ControlzEx.Standard.RECT, Cursor>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, 14.0, (double) rect.Height).Contains(p))
              return Cursors.SizeNWSE;
            return !new Rect((double) (rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double) rect.Height).Contains(p) ? Cursors.SizeNS : Cursors.SizeNESW;
          });
          break;
        case GlowDirection.Bottom:
          this.PreviewMouseDoubleClick += (MouseButtonEventHandler) ((sender, e) =>
          {
            if (!(this.ownerHandle != IntPtr.Zero))
              return;
            NativeMethods.SendMessage(this.ownerHandle, WM.NCLBUTTONDBLCLK, (IntPtr) 15, IntPtr.Zero);
          });
          this.glow.Orientation = Orientation.Horizontal;
          this.glow.VerticalAlignment = VerticalAlignment.Top;
          this.getLeft = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Left - 2));
          this.getTop = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Bottom - 1));
          this.getWidth = (Func<ControlzEx.Standard.RECT, double>) (rect => (double) (rect.Width + 4));
          this.getHeight = (Func<ControlzEx.Standard.RECT, double>) (rect => 6.0);
          this.getHitTestValue = (Func<System.Windows.Point, ControlzEx.Standard.RECT, HT>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, 14.0, (double) rect.Height).Contains(p))
              return HT.BOTTOMLEFT;
            return !new Rect((double) (rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double) rect.Height).Contains(p) ? HT.BOTTOM : HT.BOTTOMRIGHT;
          });
          this.getCursor = (Func<System.Windows.Point, ControlzEx.Standard.RECT, Cursor>) ((p, rect) =>
          {
            if (new Rect(0.0, 0.0, 14.0, (double) rect.Height).Contains(p))
              return Cursors.SizeNESW;
            return !new Rect((double) (rect.Width + 4) - 20.0 + 6.0, 0.0, 14.0, (double) rect.Height).Contains(p) ? Cursors.SizeNS : Cursors.SizeNWSE;
          });
          break;
      }
      owner.ContentRendered += (EventHandler) ((sender, e) => this.glow.Visibility = Visibility.Visible);
      owner.Activated += (EventHandler) ((sender, e) =>
      {
        this.Update();
        this.glow.IsGlow = true;
      });
      owner.Deactivated += (EventHandler) ((sender, e) => this.glow.IsGlow = false);
      owner.StateChanged += (EventHandler) ((sender, e) => this.Update());
      owner.IsVisibleChanged += (DependencyPropertyChangedEventHandler) ((sender, e) => this.Update());
      owner.Closing += (CancelEventHandler) ((sender, e) =>
      {
        if (e.Cancel)
          return;
        this.closing = true;
      });
      owner.Closed += (EventHandler) ((sender, e) => this.Close());
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.OpacityStoryboard = this.TryFindResource((object) "OpacityStoryboard") as Storyboard;
    }

    protected virtual void OnSourceInitialized(EventArgs e)
    {
      base.OnSourceInitialized(e);
      this.hwndSource = (HwndSource) PresentationSource.FromVisual((Visual) this);
      if (this.hwndSource == null)
        return;
      WS windowStyle = NativeMethods.GetWindowStyle(this.hwndSource.Handle);
      WS_EX windowStyleEx = NativeMethods.GetWindowStyleEx(this.hwndSource.Handle);
      WS dwNewLong1 = windowStyle | WS.POPUP;
      WS_EX dwNewLong2 = windowStyleEx & ~WS_EX.APPWINDOW | WS_EX.TOOLWINDOW;
      if (this._owner.ResizeMode == ResizeMode.NoResize || this._owner.ResizeMode == ResizeMode.CanMinimize)
        dwNewLong2 |= WS_EX.TRANSPARENT;
      int num1 = (int) NativeMethods.SetWindowStyle(this.hwndSource.Handle, dwNewLong1);
      int num2 = (int) NativeMethods.SetWindowStyleEx(this.hwndSource.Handle, dwNewLong2);
      this.hwndSource.AddHook(new HwndSourceHook(this.WndProc));
      this.handle = this.hwndSource.Handle;
      this.ownerHandle = new WindowInteropHelper(this._owner).Handle;
      this.resizeModeChangeNotifier = new PropertyChangeNotifier((DependencyObject) this._owner, Window.ResizeModeProperty);
      this.resizeModeChangeNotifier.ValueChanged += new EventHandler(this.ResizeModeChanged);
    }

    private void ResizeModeChanged(object sender, EventArgs e)
    {
      WS_EX windowStyleEx = NativeMethods.GetWindowStyleEx(this.hwndSource.Handle);
      int num = (int) NativeMethods.SetWindowStyleEx(this.hwndSource.Handle, this._owner.ResizeMode == ResizeMode.NoResize || this._owner.ResizeMode == ResizeMode.CanMinimize ? windowStyleEx | WS_EX.TRANSPARENT : windowStyleEx ^ WS_EX.TRANSPARENT);
    }

    public void Update()
    {
      if (this.closing)
        return;
      if (this._owner.Visibility == Visibility.Hidden)
      {
        int num1 = (int) this._owner.Dispatcher.Invoke<Visibility>((Func<Visibility>) (() => this.glow.Visibility = Visibility.Collapsed));
        int num2 = (int) this._owner.Dispatcher.Invoke<Visibility>((Func<Visibility>) (() => this.Visibility = Visibility.Collapsed));
        ControlzEx.Standard.RECT lpRect;
        if (!this.IsGlowing || !(this.ownerHandle != IntPtr.Zero) || !UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out lpRect))
          return;
        this.UpdateCore(lpRect);
      }
      else if (this._owner.WindowState == WindowState.Normal)
      {
        int num3 = (int) this._owner.Dispatcher.Invoke<Visibility>((Func<Visibility>) (() => this.glow.Visibility = this.IsGlowing ? Visibility.Visible : Visibility.Collapsed));
        int num4 = (int) this._owner.Dispatcher.Invoke<Visibility>((Func<Visibility>) (() => this.Visibility = this.IsGlowing ? Visibility.Visible : Visibility.Collapsed));
        ControlzEx.Standard.RECT lpRect;
        if (!this.IsGlowing || !(this.ownerHandle != IntPtr.Zero) || !UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out lpRect))
          return;
        this.UpdateCore(lpRect);
      }
      else
      {
        int num5 = (int) this._owner.Dispatcher.Invoke<Visibility>((Func<Visibility>) (() => this.glow.Visibility = Visibility.Collapsed));
        int num6 = (int) this._owner.Dispatcher.Invoke<Visibility>((Func<Visibility>) (() => this.Visibility = Visibility.Collapsed));
      }
    }

    internal bool CanUpdateCore() => this.ownerHandle != IntPtr.Zero && this.handle != IntPtr.Zero;

    internal void UpdateCore(ControlzEx.Standard.RECT rect) => NativeMethods.SetWindowPos(this.handle, this.ownerHandle, (int) this.getLeft(rect), (int) this.getTop(rect), (int) this.getWidth(rect), (int) this.getHeight(rect), ControlzEx.Standard.SWP.NOACTIVATE | ControlzEx.Standard.SWP.NOZORDER);

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
      switch ((WM) msg)
      {
        case WM.SHOWWINDOW:
          if ((int) lParam == 3 && this.Visibility != Visibility.Visible)
          {
            handled = true;
            break;
          }
          break;
        case WM.MOUSEACTIVATE:
          handled = true;
          if (this.ownerHandle != IntPtr.Zero)
            NativeMethods.SendMessage(this.ownerHandle, WM.ACTIVATE, wParam, lParam);
          return new IntPtr(3);
        case WM.NCHITTEST:
          Cursor cursor = (Cursor) null;
          if (this._owner.ResizeMode == ResizeMode.NoResize || this._owner.ResizeMode == ResizeMode.CanMinimize)
          {
            cursor = this._owner.Cursor;
          }
          else
          {
            ControlzEx.Standard.RECT lpRect;
            System.Windows.Point point;
            if (this.ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out lpRect) && WinApiHelper.TryGetRelativeMousePosition(this.handle, out point))
              cursor = this.getCursor(point, lpRect);
          }
          if (cursor != null && cursor != this.Cursor)
          {
            this.Cursor = cursor;
            break;
          }
          break;
        case WM.LBUTTONDOWN:
          ControlzEx.Standard.RECT lpRect1;
          System.Windows.Point point1;
          if (this.ownerHandle != IntPtr.Zero && UnsafeNativeMethods.GetWindowRect(this.ownerHandle, out lpRect1) && WinApiHelper.TryGetRelativeMousePosition(this.handle, out point1))
          {
            NativeMethods.PostMessage(this.ownerHandle, WM.NCLBUTTONDOWN, (IntPtr) (int) this.getHitTestValue(point1, lpRect1), IntPtr.Zero);
            break;
          }
          break;
      }
      return IntPtr.Zero;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher.UI;component/controls/glowwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          this.glow = (Glow) target;
        else
          this._contentLoaded = true;
      }
      else
        this.glowWindow = (GlowWindow) target;
    }
  }
}
