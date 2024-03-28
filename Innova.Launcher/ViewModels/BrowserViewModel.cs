// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.BrowserViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Dragablz;
using DynamicData;
using DynamicData.Aggregation;
using Innova.Launcher.Events;
using Innova.Launcher.Services.Interfaces;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace Innova.Launcher.ViewModels
{
  public class BrowserViewModel : ReactiveObject
  {
    private readonly SourceList<BrowserTabViewModel> _tabsSource;
    private IWindowController _window;
    private readonly ReadOnlyObservableCollection<BrowserTabViewModel> _tabs;
    private BrowserTabViewModel _selected;
    private ItemActionCallback _closeCallback;

    public ReadOnlyObservableCollection<BrowserTabViewModel> Tabs => this._tabs;

    public BrowserTabViewModel Selected
    {
      get => this._selected;
      set => this.RaiseAndSetIfChanged<BrowserViewModel, BrowserTabViewModel>(ref this._selected, value, nameof (Selected));
    }

    public ItemActionCallback CloseCallback
    {
      get => this._closeCallback;
      set => this.RaiseAndSetIfChanged<BrowserViewModel, ItemActionCallback>(ref this._closeCallback, value, nameof (CloseCallback));
    }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    public ReactiveCommand<Unit, Unit> MinimizeCommand { get; }

    public ReactiveCommand<Unit, Unit> MaximizeCommand { get; }

    public BrowserViewModel()
    {
      this._tabsSource = new SourceList<BrowserTabViewModel>((IObservable<IChangeSet<BrowserTabViewModel>>) null);
      ObservableListEx.DisposeMany<BrowserTabViewModel>(ObservableListEx.Bind<BrowserTabViewModel>(this._tabsSource.Connect((Func<BrowserTabViewModel, bool>) null), ref this._tabs, 25)).Subscribe<IChangeSet<BrowserTabViewModel>>();
      ObservableListEx.OnItemAdded<BrowserTabViewModel>(this._tabsSource.Connect((Func<BrowserTabViewModel, bool>) null).SubscribeOnDispatcher<IChangeSet<BrowserTabViewModel>>(), (Action<BrowserTabViewModel>) (v => this.Selected = v)).Subscribe<IChangeSet<BrowserTabViewModel>>();
      CountEx.IsEmpty<BrowserTabViewModel>(this._tabsSource.Connect((Func<BrowserTabViewModel, bool>) null)).Where<bool>((Func<bool, bool>) (v => v)).Subscribe<bool>((Action<bool>) (_ => MessageBus.Current.SendMessage<BrowserTabsEmptyEvent>(new BrowserTabsEmptyEvent())));
      this._tabsSource.Connect((Func<BrowserTabViewModel, bool>) null).CountChanged<BrowserTabViewModel>().Subscribe<IChangeSet<BrowserTabViewModel>>((Action<IChangeSet<BrowserTabViewModel>>) (_ => MessageBus.Current.SendMessage<BrowserTabsCountChangedEvent>(new BrowserTabsCountChangedEvent())));
      this.CloseCommand = ReactiveCommand.Create((Action) (() =>
      {
        try
        {
          Mouse.OverrideCursor = Cursors.Wait;
          this._window.Close();
        }
        finally
        {
          Mouse.OverrideCursor = (Cursor) null;
        }
      }));
      this.MinimizeCommand = ReactiveCommand.Create((Action) (() => this._window.Minimize()));
      this.MaximizeCommand = ReactiveCommand.Create((Action) (() => this._window.Maximize()));
      this.CloseCallback = (ItemActionCallback) (s =>
      {
        if (!(s.DragablzItem.DataContext is BrowserTabViewModel dataContext2))
          return;
        this._tabsSource.Remove<BrowserTabViewModel>(dataContext2);
        s.Cancel();
      });
    }

    public void Init(IWindowController window)
    {
      this._window = window ?? throw new ArgumentNullException(nameof (window));
      this._window.Closing += (CancelEventHandler) ((_param1, _param2) =>
      {
        foreach (BrowserTabViewModel browserTabViewModel in this.Tabs.ToArray<BrowserTabViewModel>())
          browserTabViewModel.Dispose();
      });
    }

    public BrowserTabViewModel CreateTab(string url) => new BrowserTabViewModel(this._tabsSource, url);

    public BrowserTabViewModel Open(BrowserTabViewModel viewModel)
    {
      this._tabsSource.Add<BrowserTabViewModel>(viewModel);
      return viewModel;
    }

    public BrowserTabViewModel Open(string url)
    {
      BrowserTabViewModel browserTabViewModel = new BrowserTabViewModel(this._tabsSource, url);
      this._tabsSource.Add<BrowserTabViewModel>(browserTabViewModel);
      return browserTabViewModel;
    }
  }
}
