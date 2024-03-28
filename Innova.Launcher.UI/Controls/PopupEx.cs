// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.PopupEx
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

namespace Innova.Launcher.UI.Controls
{
  public class PopupEx : Popup
  {
    private Window _hostWindow;
    private bool? _appliedTopMost;
    public static readonly DependencyProperty CloseOnMouseLeftButtonDownProperty = DependencyProperty.Register(nameof (CloseOnMouseLeftButtonDown), typeof (bool), typeof (PopupEx), new PropertyMetadata((object) false));
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
    private static readonly IntPtr HWND_TOP = new IntPtr(0);
    private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);

    public bool CloseOnMouseLeftButtonDown
    {
      get => (bool) this.GetValue(PopupEx.CloseOnMouseLeftButtonDownProperty);
      set => this.SetValue(PopupEx.CloseOnMouseLeftButtonDownProperty, (object) value);
    }

    public PopupEx()
    {
      this.Loaded += new RoutedEventHandler(this.PopupExLoaded);
      this.Opened += new EventHandler(this.PopupExOpened);
    }

    public void RefreshPosition()
    {
      double horizontalOffset = this.HorizontalOffset;
      this.SetCurrentValue(Popup.HorizontalOffsetProperty, (object) (horizontalOffset + 1.0));
      this.SetCurrentValue(Popup.HorizontalOffsetProperty, (object) horizontalOffset);
    }

    private void PopupExLoaded(object sender, RoutedEventArgs e)
    {
      if (!(this.PlacementTarget is FrameworkElement placementTarget))
        return;
      this._hostWindow = Window.GetWindow((DependencyObject) placementTarget);
      if (this._hostWindow == null)
        return;
      this._hostWindow.LocationChanged -= new EventHandler(this.HostWindowSizeOrLocationChanged);
      this._hostWindow.LocationChanged += new EventHandler(this.HostWindowSizeOrLocationChanged);
      this._hostWindow.SizeChanged -= new SizeChangedEventHandler(this.HostWindowSizeOrLocationChanged);
      this._hostWindow.SizeChanged += new SizeChangedEventHandler(this.HostWindowSizeOrLocationChanged);
      placementTarget.SizeChanged -= new SizeChangedEventHandler(this.HostWindowSizeOrLocationChanged);
      placementTarget.SizeChanged += new SizeChangedEventHandler(this.HostWindowSizeOrLocationChanged);
      this._hostWindow.StateChanged -= new EventHandler(this.HostWindowStateChanged);
      this._hostWindow.StateChanged += new EventHandler(this.HostWindowStateChanged);
      this._hostWindow.Activated -= new EventHandler(this.HostWindowActivated);
      this._hostWindow.Activated += new EventHandler(this.HostWindowActivated);
      this._hostWindow.Deactivated -= new EventHandler(this.HostWindowDeactivated);
      this._hostWindow.Deactivated += new EventHandler(this.HostWindowDeactivated);
      this.Unloaded -= new RoutedEventHandler(this.PopupExUnloaded);
      this.Unloaded += new RoutedEventHandler(this.PopupExUnloaded);
    }

    private void PopupExOpened(object sender, EventArgs e) => this.SetTopmostState(true);

    private void HostWindowActivated(object sender, EventArgs e) => this.SetTopmostState(true);

    private void HostWindowDeactivated(object sender, EventArgs e) => this.SetTopmostState(false);

    private void PopupExUnloaded(object sender, RoutedEventArgs e)
    {
      if (this.PlacementTarget is FrameworkElement placementTarget)
        placementTarget.SizeChanged -= new SizeChangedEventHandler(this.HostWindowSizeOrLocationChanged);
      if (this._hostWindow != null)
      {
        this._hostWindow.LocationChanged -= new EventHandler(this.HostWindowSizeOrLocationChanged);
        this._hostWindow.SizeChanged -= new SizeChangedEventHandler(this.HostWindowSizeOrLocationChanged);
        this._hostWindow.StateChanged -= new EventHandler(this.HostWindowStateChanged);
        this._hostWindow.Activated -= new EventHandler(this.HostWindowActivated);
        this._hostWindow.Deactivated -= new EventHandler(this.HostWindowDeactivated);
      }
      this.Unloaded -= new RoutedEventHandler(this.PopupExUnloaded);
      this.Opened -= new EventHandler(this.PopupExOpened);
      this._hostWindow = (Window) null;
    }

    private void HostWindowStateChanged(object sender, EventArgs e)
    {
      if (this._hostWindow == null || this._hostWindow.WindowState == WindowState.Minimized)
        return;
      AdornedElementPlaceholder dataContext = this.PlacementTarget is FrameworkElement placementTarget ? placementTarget.DataContext as AdornedElementPlaceholder : (AdornedElementPlaceholder) null;
      if (dataContext?.AdornedElement == null)
        return;
      this.PopupAnimation = PopupAnimation.None;
      this.IsOpen = false;
      object obj = dataContext.AdornedElement.GetValue(Validation.ErrorTemplateProperty);
      dataContext.AdornedElement.SetValue(Validation.ErrorTemplateProperty, (object) null);
      dataContext.AdornedElement.SetValue(Validation.ErrorTemplateProperty, obj);
    }

    private void HostWindowSizeOrLocationChanged(object sender, EventArgs e) => this.RefreshPosition();

    private void SetTopmostState(bool isTop)
    {
      if (this._appliedTopMost.HasValue)
      {
        bool? appliedTopMost = this._appliedTopMost;
        bool flag = isTop;
        if (appliedTopMost.GetValueOrDefault() == flag & appliedTopMost.HasValue)
          return;
      }
      if (this.Child == null || !(PresentationSource.FromVisual((Visual) this.Child) is HwndSource hwndSource))
        return;
      IntPtr handle = hwndSource.Handle;
      PopupEx.RECT lpRect;
      if (!PopupEx.GetWindowRect(handle, out lpRect))
        return;
      int left = lpRect.Left;
      int top = lpRect.Top;
      int width = lpRect.Width;
      int height = lpRect.Height;
      if (isTop)
      {
        PopupEx.SetWindowPos(handle, PopupEx.HWND_TOPMOST, left, top, width, height, PopupEx.SWP.TOPMOST);
      }
      else
      {
        PopupEx.SetWindowPos(handle, PopupEx.HWND_BOTTOM, left, top, width, height, PopupEx.SWP.TOPMOST);
        PopupEx.SetWindowPos(handle, PopupEx.HWND_TOP, left, top, width, height, PopupEx.SWP.TOPMOST);
        PopupEx.SetWindowPos(handle, PopupEx.HWND_NOTOPMOST, left, top, width, height, PopupEx.SWP.TOPMOST);
      }
      this._appliedTopMost = new bool?(isTop);
    }

    protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
    {
      if (!this.CloseOnMouseLeftButtonDown)
        return;
      this.IsOpen = false;
    }

    internal static int LOWORD(int i) => (int) (short) (i & (int) ushort.MaxValue);

    [SecurityCritical]
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool GetWindowRect(IntPtr hWnd, out PopupEx.RECT lpRect);

    [SecurityCritical]
    [DllImport("user32.dll", EntryPoint = "SetWindowPos", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool _SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int x,
      int y,
      int cx,
      int cy,
      PopupEx.SWP uFlags);

    [SecurityCritical]
    private static bool SetWindowPos(
      IntPtr hWnd,
      IntPtr hWndInsertAfter,
      int x,
      int y,
      int cx,
      int cy,
      PopupEx.SWP uFlags)
    {
      return PopupEx._SetWindowPos(hWnd, hWndInsertAfter, x, y, cx, cy, uFlags);
    }

    [Flags]
    private enum SWP
    {
      ASYNCWINDOWPOS = 16384, // 0x00004000
      DEFERERASE = 8192, // 0x00002000
      DRAWFRAME = 32, // 0x00000020
      FRAMECHANGED = DRAWFRAME, // 0x00000020
      HIDEWINDOW = 128, // 0x00000080
      NOACTIVATE = 16, // 0x00000010
      NOCOPYBITS = 256, // 0x00000100
      NOMOVE = 2,
      NOOWNERZORDER = 512, // 0x00000200
      NOREDRAW = 8,
      NOREPOSITION = NOOWNERZORDER, // 0x00000200
      NOSENDCHANGING = 1024, // 0x00000400
      NOSIZE = 1,
      NOZORDER = 4,
      SHOWWINDOW = 64, // 0x00000040
      TOPMOST = NOSIZE | NOSENDCHANGING | NOREPOSITION | NOREDRAW | NOMOVE | NOACTIVATE, // 0x0000061B
    }

    internal struct POINT
    {
      public int x;
      public int y;
    }

    internal struct SIZE
    {
      public int cx;
      public int cy;
    }

    internal struct RECT
    {
      private int _left;
      private int _top;
      private int _right;
      private int _bottom;

      public void Offset(int dx, int dy)
      {
        this._left += dx;
        this._top += dy;
        this._right += dx;
        this._bottom += dy;
      }

      public int Left
      {
        get => this._left;
        set => this._left = value;
      }

      public int Right
      {
        get => this._right;
        set => this._right = value;
      }

      public int Top
      {
        get => this._top;
        set => this._top = value;
      }

      public int Bottom
      {
        get => this._bottom;
        set => this._bottom = value;
      }

      public int Width => this._right - this._left;

      public int Height => this._bottom - this._top;

      public PopupEx.POINT Position => new PopupEx.POINT()
      {
        x = this._left,
        y = this._top
      };

      public PopupEx.SIZE Size => new PopupEx.SIZE()
      {
        cx = this.Width,
        cy = this.Height
      };

      public static PopupEx.RECT Union(PopupEx.RECT rect1, PopupEx.RECT rect2) => new PopupEx.RECT()
      {
        Left = Math.Min(rect1.Left, rect2.Left),
        Top = Math.Min(rect1.Top, rect2.Top),
        Right = Math.Max(rect1.Right, rect2.Right),
        Bottom = Math.Max(rect1.Bottom, rect2.Bottom)
      };

      public override bool Equals(object obj)
      {
        try
        {
          PopupEx.RECT rect = (PopupEx.RECT) obj;
          return rect._bottom == this._bottom && rect._left == this._left && rect._right == this._right && rect._top == this._top;
        }
        catch (InvalidCastException ex)
        {
          return false;
        }
      }

      public override int GetHashCode() => (this._left << 16 | PopupEx.LOWORD(this._right)) ^ (this._top << 16 | PopupEx.LOWORD(this._bottom));
    }
  }
}
