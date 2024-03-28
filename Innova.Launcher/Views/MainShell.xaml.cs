// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Views.MainShell
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp.WinForms;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Services.Interfaces;
using Innova.Launcher.UI.Controls;
using Innova.Launcher.UI.Extensions;
using Innova.Launcher.ViewModels;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Innova.Launcher.Views
{
  public partial class MainShell : ModernWindow, IMainShellPresenter, IComponentConnector
  {
    private readonly IWindowsService _windowsService;
    private volatile bool _internalClose = true;
    internal ChromiumWebBrowser Browser;
    private bool _contentLoaded;

    public MainViewModel ViewModel { get; }

    public MainShell(MainViewModel viewModel, IWindowsService windowsService)
    {
      this.ViewModel = viewModel ?? throw new ArgumentNullException(nameof (viewModel));
      this._windowsService = windowsService ?? throw new ArgumentNullException(nameof (windowsService));
      this.InitializeComponent();
      this.Closing += new CancelEventHandler(this.MainShellClosing);
      this.Activated += new EventHandler(this.MainShellActivated);
      this.Deactivated += new EventHandler(this.MainShellDeactivated);
      this.DataContext = (object) this.ViewModel;
      this.ViewModel.Init((IMainShellPresenter) this, this.Browser);
    }

    private void MainShellClosing(object sender, CancelEventArgs e)
    {
      if ((!this._internalClose ? 0 : (this._windowsService.ShouldWindowHideOnClose(this.ViewModel.Id) ? 1 : 0)) == 0)
        return;
      e.Cancel = true;
      this._windowsService.CloseWindow(this.ViewModel.Id);
    }

    private void MainShellActivated(object sender, EventArgs e) => this.ViewModel.WindowActive = true;

    private void MainShellDeactivated(object sender, EventArgs e) => this.ViewModel.WindowActive = false;

    public void Show(string url)
    {
      this.GoToUrl(url);
      this.Show();
    }

    public void GoToUrl(string url) => this.ViewModel.OpenUrl(url);

    public void HideToTray()
    {
      this.ViewModel.Hide();
      this.Hide();
    }

    public void Maximize() => this.WindowState = WindowState.Maximized;

    public void MaximizeOrNormalize() => this.WindowState = this.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

    public void Minimize() => this.WindowState = WindowState.Minimized;

    public void Normalize()
    {
      if (this.WindowState == WindowState.Normal)
        return;
      this.WindowState = WindowState.Normal;
    }

    public WindowStorage GetWindowStorage() => this.ViewModel.GetWindowStorage();

    public void RestoreWindowStorage(WindowStorage storage) => this.ViewModel.RestoreWindowStorage(storage);

    public void Rise()
    {
      if (this.IsActive && this.IsVisible)
        return;
      this.Show();
      this.Activate();
      if (this.WindowState != WindowState.Minimized)
        return;
      this.WindowState = WindowState.Normal;
    }

    public void HideAndClose()
    {
      this._internalClose = false;
      this.Close();
      this._internalClose = true;
    }

    public void MakeResizable(double? width, double? height, double? minWidth, double? minHeight) => WindowExtensions.MakeResizable(this, width, height, minWidth, minHeight);

    public void MakeNoResizable(double? width, double? height) => WindowExtensions.MakeNoResizable(this, width, height);

    public void StartNativeDrag() => WindowExtensions.StartNativeDrag(this);

    private void WindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (!this.ViewModel.HasProblem && !this.ViewModel.ServicePaused)
        return;
      this.StartNativeDrag();
      e.Handled = false;
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/views/mainshell.xaml", UriKind.Relative));
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
        ((UIElement) target).MouseLeftButtonDown += new MouseButtonEventHandler(this.WindowMouseLeftButtonDown);
    }

    [SpecialName]
    WindowStartupLocation IMainShellPresenter.get_WindowStartupLocation() => this.WindowStartupLocation;

    [SpecialName]
    void IMainShellPresenter.set_WindowStartupLocation(WindowStartupLocation value) => this.WindowStartupLocation = value;

    [SpecialName]
    bool IMainShellPresenter.get_IsLoaded() => this.IsLoaded;

    [SpecialName]
    bool IMainShellPresenter.get_IsVisible() => this.IsVisible;

    [SpecialName]
    bool IMainShellPresenter.get_SaveWindowPosition() => this.SaveWindowPosition;

    [SpecialName]
    void IMainShellPresenter.set_SaveWindowPosition(bool value) => this.SaveWindowPosition = value;

    [SpecialName]
    WindowState IMainShellPresenter.get_WindowState() => this.WindowState;

    [SpecialName]
    void IMainShellPresenter.set_WindowState(WindowState value) => this.WindowState = value;

    [SpecialName]
    void IMainShellPresenter.add_StateChanged(EventHandler value) => this.StateChanged += value;

    [SpecialName]
    void IMainShellPresenter.remove_StateChanged(EventHandler value) => this.StateChanged -= value;

    [SpecialName]
    void IMainShellPresenter.add_Closed(EventHandler value) => this.Closed += value;

    [SpecialName]
    void IMainShellPresenter.remove_Closed(EventHandler value) => this.Closed -= value;

    [SpecialName]
    void IMainShellPresenter.add_Closing(CancelEventHandler value) => this.Closing += value;

    [SpecialName]
    void IMainShellPresenter.remove_Closing(CancelEventHandler value) => this.Closing -= value;

    bool IMainShellPresenter.Activate() => this.Activate();
  }
}
