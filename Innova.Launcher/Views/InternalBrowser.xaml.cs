// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Views.InternalBrowser
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Dragablz;
using Innova.Launcher.Events;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.UI.Controls;
using Innova.Launcher.ViewModels;
using ReactiveUI;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Innova.Launcher.Views
{
  public partial class InternalBrowser : ModernWindow, IWindowController, IComponentConnector
  {
    private BrowserViewModel _viewModel;
    internal TabablzControl BrowserTabControl;
    private bool _contentLoaded;

    public InternalBrowser()
    {
      this.InitializeComponent();
      MessageBus.Current.Listen<BrowserTabsEmptyEvent>().Subscribe<BrowserTabsEmptyEvent>((Action<BrowserTabsEmptyEvent>) (_ => this.Close()));
      MessageBus.Current.Listen<BrowserTabsCountChangedEvent>().Subscribe<BrowserTabsCountChangedEvent>((Action<BrowserTabsCountChangedEvent>) (_ => this.ForceTabsWidthUpdate()));
    }

    [Unity.Dependency]
    public BrowserViewModel ViewModel
    {
      get => this._viewModel;
      set
      {
        value.Init((IWindowController) this);
        this.DataContext = (object) (this._viewModel = value);
      }
    }

    public void Maximize() => this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    public void Minimize() => this.WindowState = WindowState.Minimized;

    private void BrowserTabControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
      if (!e.WidthChanged)
        return;
      this.ForceTabsWidthUpdate();
    }

    private void ForceTabsWidthUpdate()
    {
      foreach (DependencyObject target in this.VisualDepthFirstTraversal().OfType<BrowserTabHeader>())
        BindingOperations.GetMultiBindingExpression(target, FrameworkElement.WidthProperty)?.UpdateTarget();
    }

    private void GridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ClickCount != 2 || e.LeftButton != MouseButtonState.Pressed)
        return;
      this.Maximize();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/views/internalbrowser.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
      {
        this.BrowserTabControl = (TabablzControl) target;
        this.BrowserTabControl.SizeChanged += new SizeChangedEventHandler(this.BrowserTabControl_SizeChanged);
      }
      else
        this._contentLoaded = true;
    }

    [SpecialName]
    Dispatcher IWindowController.get_Dispatcher() => this.Dispatcher;

    [SpecialName]
    object IWindowController.get_Content() => this.Content;

    [SpecialName]
    void IWindowController.set_Content(object value) => this.Content = value;

    [SpecialName]
    void IWindowController.add_Closing(CancelEventHandler value) => this.Closing += value;

    [SpecialName]
    void IWindowController.remove_Closing(CancelEventHandler value) => this.Closing -= value;

    void IWindowController.Hide() => this.Hide();

    void IWindowController.Close() => this.Close();
  }
}
