// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.BrowserTabViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CefSharp;
using CefSharp.WinForms;
using CommonServiceLocator;
using DynamicData;
using Innova.Launcher.Handlers;
using Innova.Launcher.Shared.Extensions;
using Innova.Launcher.UI.Extensions;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Innova.Launcher.ViewModels
{
  public class BrowserTabViewModel : ReactiveObject, IDisposable
  {
    private readonly Subject<Uri> _addressSubject;
    private readonly SourceList<BrowserTabViewModel> _list;
    private string _title;
    private string _address;
    private readonly ObservableAsPropertyHelper<string> _faviconUrl;
    private ObservableAsPropertyHelper<bool> _isLoading;
    private ChromiumWebBrowser _browserView;
    private ReactiveCommand<Unit, Unit> _goBackCommand;
    private ReactiveCommand<Unit, Unit> _goForwardCommand;

    public string Url { get; }

    public string Title
    {
      get => this._title;
      set => this.RaiseAndSetIfChanged<BrowserTabViewModel, string>(ref this._title, value, nameof (Title));
    }

    public string Address
    {
      get => this._address;
      set => this.RaiseAndSetIfChanged<BrowserTabViewModel, string>(ref this._address, value, nameof (Address));
    }

    public string FaviconUrl => this._faviconUrl.Value;

    public bool IsLoading
    {
      get
      {
        ObservableAsPropertyHelper<bool> isLoading = this._isLoading;
        return isLoading != null && isLoading.Value;
      }
    }

    public ChromiumWebBrowser BrowserView
    {
      get => this._browserView;
      set => this.RaiseAndSetIfChanged<BrowserTabViewModel, ChromiumWebBrowser>(ref this._browserView, value, nameof (BrowserView));
    }

    public ReactiveCommand<BrowserTabViewModel, Unit> CloseTabCommand { get; }

    public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

    public ReactiveCommand<Unit, Unit> StopCommand { get; }

    public ReactiveCommand<Unit, Unit> GoBackCommand
    {
      get => this._goBackCommand;
      set => this.RaiseAndSetIfChanged<BrowserTabViewModel, ReactiveCommand<Unit, Unit>>(ref this._goBackCommand, value, nameof (GoBackCommand));
    }

    public ReactiveCommand<Unit, Unit> GoForwardCommand
    {
      get => this._goForwardCommand;
      set => this.RaiseAndSetIfChanged<BrowserTabViewModel, ReactiveCommand<Unit, Unit>>(ref this._goForwardCommand, value, nameof (GoForwardCommand));
    }

    public BrowserTabViewModel(SourceList<BrowserTabViewModel> list, string url)
    {
      this._list = list ?? throw new ArgumentNullException(nameof (list));
      this._addressSubject = new Subject<Uri>();
      this.Url = url;
      this.StopCommand = ReactiveCommand.Create((Action) (() => this.Gui((Action) (() => this.BrowserView.Stop()))));
      this.ReloadCommand = ReactiveCommand.Create((Action) (() => this.Gui((Action) (() => this.BrowserView.Reload()))));
      this.CloseTabCommand = ReactiveCommand.Create<BrowserTabViewModel>((Action<BrowserTabViewModel>) (v =>
      {
        this._list.Remove<BrowserTabViewModel>(v);
        this.Dispose();
      }));
      this._addressSubject.Select<Uri, string>((Func<Uri, string>) (v => v.ToString())).Subscribe<string>((Action<string>) (v => this.Address = v));
      this._addressSubject.Select<Uri, string>((Func<Uri, string>) (v => v.Authority.ToLowerInvariant())).DistinctUntilChanged<string>().Select<string, string>((Func<string, string>) (v => "https://www.google.com/s2/favicons?domain=" + v.RemoveTrailingSlash())).ObserveOnDispatcher<string>().ToProperty<BrowserTabViewModel, string>(this, (Expression<Func<BrowserTabViewModel, string>>) (v => v.FaviconUrl), out this._faviconUrl);
    }

    public void LoadBrowser(ChromiumWebBrowser browser)
    {
      this.BrowserView = browser;
      IObservable<Unit> source = Observable.FromEventPattern<FrameLoadEndEventArgs>((Action<EventHandler<FrameLoadEndEventArgs>>) (v => this.BrowserView.FrameLoadEnd += v), (Action<EventHandler<FrameLoadEndEventArgs>>) (v => this.BrowserView.FrameLoadEnd -= v)).Select<EventPattern<FrameLoadEndEventArgs>, Unit>((Func<EventPattern<FrameLoadEndEventArgs>, Unit>) (v => Unit.Default));
      this.GoBackCommand = ReactiveCommand.Create((Action) (() => this.BrowserView.Back()), source.ObserveOnDispatcher<Unit>().Select<Unit, bool>((Func<Unit, bool>) (_ => this.BrowserView.CanGoBack)).Catch<bool>(Observable.Return<bool>(false)));
      this.GoForwardCommand = ReactiveCommand.Create((Action) (() => this.BrowserView.Forward()), source.ObserveOnDispatcher<Unit>().Select<Unit, bool>((Func<Unit, bool>) (_ => this.BrowserView.CanGoForward)).Catch<bool>(Observable.Return<bool>(false)));
      browser.MenuHandler = (IContextMenuHandler) ServiceLocator.Current.GetInstance<ForgameContextMenuHandler>();
      browser.LifeSpanHandler = (ILifeSpanHandler) ServiceLocator.Current.GetInstance<InternalBrowserLifeSpanHandler>();
      Observable.Merge<bool>(Observable.FromEventPattern<FrameLoadStartEventArgs>((Action<EventHandler<FrameLoadStartEventArgs>>) (h => browser.FrameLoadStart += h), (Action<EventHandler<FrameLoadStartEventArgs>>) (h => browser.FrameLoadStart -= h)).Where<EventPattern<FrameLoadStartEventArgs>>((Func<EventPattern<FrameLoadStartEventArgs>, bool>) (v => v.EventArgs.Frame.IsMain)).Select<EventPattern<FrameLoadStartEventArgs>, bool>((Func<EventPattern<FrameLoadStartEventArgs>, bool>) (v => true)), Observable.FromEventPattern<FrameLoadEndEventArgs>((Action<EventHandler<FrameLoadEndEventArgs>>) (h => browser.FrameLoadEnd += h), (Action<EventHandler<FrameLoadEndEventArgs>>) (h => browser.FrameLoadEnd -= h)).Where<EventPattern<FrameLoadEndEventArgs>>((Func<EventPattern<FrameLoadEndEventArgs>, bool>) (v => v.EventArgs.Frame.IsMain)).Select<EventPattern<FrameLoadEndEventArgs>, bool>((Func<EventPattern<FrameLoadEndEventArgs>, bool>) (v => false)), Observable.FromEventPattern<FrameLoadEndEventArgs>((Action<EventHandler<FrameLoadEndEventArgs>>) (h => browser.FrameLoadEnd += h), (Action<EventHandler<FrameLoadEndEventArgs>>) (h => browser.FrameLoadEnd -= h)).Where<EventPattern<FrameLoadEndEventArgs>>((Func<EventPattern<FrameLoadEndEventArgs>, bool>) (v => v.EventArgs.Frame.IsMain)).Select<EventPattern<FrameLoadEndEventArgs>, bool>((Func<EventPattern<FrameLoadEndEventArgs>, bool>) (v => false))).DistinctUntilChanged<bool>().ToProperty<BrowserTabViewModel, bool>(this, (Expression<Func<BrowserTabViewModel, bool>>) (v => v.IsLoading), out this._isLoading, true);
      ((Component) browser).Disposed += (EventHandler) ((s, e) => this.Gui<bool>((Func<bool>) (() => this._list.Remove<BrowserTabViewModel>(this))));
      browser.TitleChanged += (EventHandler<TitleChangedEventArgs>) ((s, ea) => this.GuiNoWait((Action) (() => this.Title = ea.Title)));
      browser.AddressChanged += new EventHandler<AddressChangedEventArgs>(this.BrowserAddressChanged);
      if (string.IsNullOrEmpty(this.Url))
        return;
      browser.Load(this.Url);
    }

    private void BrowserAddressChanged(object sender, AddressChangedEventArgs e)
    {
      Uri result;
      if (!Uri.TryCreate(e.Address, UriKind.Absolute, out result))
        return;
      this._addressSubject.OnNext(result);
    }

    public void Dispose()
    {
      try
      {
        if (!this.BrowserView.IsDisposed)
          ((Component) this.BrowserView).Dispose();
        if (this.BrowserView.IsDisposed)
          return;
        ((Component) this.BrowserView).Dispose();
      }
      catch
      {
      }
    }
  }
}
