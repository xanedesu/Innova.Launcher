// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.PushNotificationViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Events;
using Innova.Launcher.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive;
using System.Reactive.Linq;

namespace Innova.Launcher.ViewModels
{
  public sealed class PushNotificationViewModel : ReactiveObject
  {
    private int _id;
    private string _message;
    private string _svgImageUrl;
    private string _imageUrl;
    private PushNotificationLink _primaryLink;
    private PushNotificationLink _secondaryLink;
    private readonly ObservableAsPropertyHelper<bool> _hasLinks;
    private readonly ObservableAsPropertyHelper<int> _buttonCount;

    public int Id
    {
      get => this._id;
      set => this.RaiseAndSetIfChanged<PushNotificationViewModel, int>(ref this._id, value, nameof (Id));
    }

    public string Message
    {
      get => this._message;
      set => this.RaiseAndSetIfChanged<PushNotificationViewModel, string>(ref this._message, value, nameof (Message));
    }

    public string SVGImageUrl
    {
      get => this._svgImageUrl;
      set => this.RaiseAndSetIfChanged<PushNotificationViewModel, string>(ref this._svgImageUrl, value, nameof (SVGImageUrl));
    }

    public string ImageUrl
    {
      get => this._imageUrl;
      set => this.RaiseAndSetIfChanged<PushNotificationViewModel, string>(ref this._imageUrl, value, nameof (ImageUrl));
    }

    public PushNotificationLink PrimaryLink
    {
      get => this._primaryLink;
      set => this.RaiseAndSetIfChanged<PushNotificationViewModel, PushNotificationLink>(ref this._primaryLink, value, nameof (PrimaryLink));
    }

    public PushNotificationLink SecondaryLink
    {
      get => this._secondaryLink;
      set => this.RaiseAndSetIfChanged<PushNotificationViewModel, PushNotificationLink>(ref this._secondaryLink, value, nameof (SecondaryLink));
    }

    public bool HasLinks => this._hasLinks.Value;

    public int ButtonCount => this._buttonCount.Value;

    public ReactiveCommand<Unit, Unit> GoToPrimaryLinkCommand { get; }

    public ReactiveCommand<Unit, Unit> GoToSecondaryLinkCommand { get; }

    public ReactiveCommand<Unit, Unit> CloseCommand { get; }

    public PushNotificationViewModel(PushNotification source)
    {
      IObservable<bool> canExecute1 = this.WhenAnyValue<PushNotificationViewModel, PushNotificationLink>((Expression<Func<PushNotificationViewModel, PushNotificationLink>>) (v => v.PrimaryLink)).Select<PushNotificationLink, bool>((Func<PushNotificationLink, bool>) (v => v != null));
      IObservable<bool> canExecute2 = this.WhenAnyValue<PushNotificationViewModel, PushNotificationLink>((Expression<Func<PushNotificationViewModel, PushNotificationLink>>) (v => v.SecondaryLink)).Select<PushNotificationLink, bool>((Func<PushNotificationLink, bool>) (v => v != null));
      this.GoToPrimaryLinkCommand = ReactiveCommand.Create((Action) (() => MessageBus.Current.SendMessage<NavigateMainWindowEvent>(new NavigateMainWindowEvent(this.PrimaryLink.Url))), canExecute1);
      this.GoToSecondaryLinkCommand = ReactiveCommand.Create((Action) (() => MessageBus.Current.SendMessage<NavigateMainWindowEvent>(new NavigateMainWindowEvent(this.SecondaryLink.Url))), canExecute2);
      this.CloseCommand = ReactiveCommand.Create((Action) (() => MessageBus.Current.SendMessage<PushNotificationClosedEvent>(new PushNotificationClosedEvent(this.Id))));
      Observable.CombineLatest<bool>(this.GoToPrimaryLinkCommand.CanExecute, this.GoToSecondaryLinkCommand.CanExecute).Select<IList<bool>, int>((Func<IList<bool>, int>) (values => values.Count<bool>((Func<bool, bool>) (v => v)))).ToProperty<PushNotificationViewModel, int>(this, (Expression<Func<PushNotificationViewModel, int>>) (v => v.ButtonCount), out this._buttonCount);
      Observable.CombineLatest<bool>(this.GoToPrimaryLinkCommand.CanExecute, this.GoToSecondaryLinkCommand.CanExecute).Select<IList<bool>, bool>((Func<IList<bool>, bool>) (values => values.Any<bool>((Func<bool, bool>) (v => v)))).ToProperty<PushNotificationViewModel, bool>(this, (Expression<Func<PushNotificationViewModel, bool>>) (v => v.HasLinks), out this._hasLinks);
      if (source.ImageUrl == null && source.SVG != null)
      {
        string path = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        File.WriteAllText(path, source.SVG);
        this.SVGImageUrl = path;
      }
      this.Id = source.Id;
      this.ImageUrl = source.ImageUrl;
      this.Message = source.Message;
      this.PrimaryLink = source.PrimaryLink;
      this.SecondaryLink = source.SecondaryLink;
    }

    public void OnMouseEnter() => MessageBus.Current.SendMessage<PushNotificationReadEvent>(new PushNotificationReadEvent(this.Id));
  }
}
