// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Views.SystemTrayControl
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Hardcodet.Wpf.TaskbarNotification;
using Innova.Launcher.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Markup;
using Unity;

namespace Innova.Launcher.Views
{
  public partial class SystemTrayControl : TaskbarIcon, IComponentConnector
  {
    private SystemTrayViewModel _viewModel;
    internal SystemTrayControl Icon;
    private bool _contentLoaded;

    public SystemTrayControl() => this.InitializeComponent();

    public SystemTrayControl(SystemTrayViewModel viewModel)
      : this()
    {
      this.ViewModel = viewModel;
    }

    [Dependency]
    public SystemTrayViewModel ViewModel
    {
      get => this._viewModel;
      set => this.DataContext = (object) (this._viewModel = value);
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/controls/systemtraycontrol.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.Icon = (SystemTrayControl) target;
      else
        this._contentLoaded = true;
    }
  }
}
