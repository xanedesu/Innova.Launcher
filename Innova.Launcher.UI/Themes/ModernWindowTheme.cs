// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Themes.ModernWindowTheme
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using Innova.Launcher.UI.Extensions;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;

namespace Innova.Launcher.UI.Themes
{
  public class ModernWindowTheme : ResourceDictionary, IComponentConnector, IStyleConnector
  {
    private bool _contentLoaded;

    public ModernWindowTheme() => this.InitializeComponent();

    private void CloseClick(object sender, RoutedEventArgs e) => ((Window) ((FrameworkElement) sender).TemplatedParent).Close();

    private void MaximizeRestoreClick(object sender, RoutedEventArgs e)
    {
      Window templatedParent = (Window) ((FrameworkElement) sender).TemplatedParent;
      if (templatedParent.WindowState == WindowState.Normal)
        templatedParent.WindowState = WindowState.Maximized;
      else
        templatedParent.WindowState = WindowState.Normal;
    }

    private void MinimizeClick(object sender, RoutedEventArgs e) => ((Window) ((FrameworkElement) sender).TemplatedParent).WindowState = WindowState.Minimized;

    private void WindowLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      Window templatedParent = (Window) ((FrameworkElement) sender).TemplatedParent;
      if (e.ClickCount == 2)
      {
        templatedParent.WindowState = templatedParent.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
      }
      else
      {
        e.Handled = true;
        templatedParent.StartNativeDrag();
      }
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher.UI;component/themes/modernwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    internal Delegate _CreateDelegate(Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target) => this._contentLoaded = true;

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IStyleConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          ((UIElement) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.WindowLeftButtonDown);
          break;
        case 2:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.MinimizeClick);
          break;
        case 3:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.MaximizeRestoreClick);
          break;
        case 4:
          ((ButtonBase) target).Click += new RoutedEventHandler(this.CloseClick);
          break;
      }
    }
  }
}
