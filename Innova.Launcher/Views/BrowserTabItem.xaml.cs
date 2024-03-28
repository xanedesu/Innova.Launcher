// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Views.BrowserTabItem
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp.WinForms;
using Innova.Launcher.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Innova.Launcher.Views
{
  public partial class BrowserTabItem : UserControl, IComponentConnector
  {
    internal RotateTransform ButtonRotateTarnsform;
    internal ChromiumWebBrowser Browser;
    private bool _contentLoaded;

    public BrowserTabItem()
    {
      this.InitializeComponent();
      this.DataContextChanged += new DependencyPropertyChangedEventHandler(this.TabItemDataContextChanged);
    }

    private void TabItemDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
      if (!(e.NewValue is BrowserTabViewModel newValue))
        return;
      newValue.LoadBrowser(this.Browser);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/views/browsertabitem.xaml", UriKind.Relative));
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
          this.Browser = (ChromiumWebBrowser) target;
        else
          this._contentLoaded = true;
      }
      else
        this.ButtonRotateTarnsform = (RotateTransform) target;
    }
  }
}
