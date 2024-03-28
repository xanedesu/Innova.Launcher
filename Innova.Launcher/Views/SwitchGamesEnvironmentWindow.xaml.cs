// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Views.SwitchGamesEnvironmentWindow
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.UI.Controls;
using Innova.Launcher.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Markup;

namespace Innova.Launcher.Views
{
  public partial class SwitchGamesEnvironmentWindow : ModernWindow, IComponentConnector
  {
    internal ComboBox Selector;
    private bool _contentLoaded;

    public SwitchGamesEnvironmentWindow() => this.InitializeComponent();

    public SwitchGamesEnvironmentWindow(SwitchGamesEnvironmentViewModel viewModel)
      : this()
    {
      this.DataContext = (object) viewModel;
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      this.DialogResult = new bool?(true);
      this.Close();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/views/switchgamesenvironmentwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId != 1)
      {
        if (connectionId == 2)
          ((ButtonBase) target).Click += new RoutedEventHandler(this.OkClick);
        else
          this._contentLoaded = true;
      }
      else
        this.Selector = (ComboBox) target;
    }
  }
}
