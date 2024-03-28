// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Models.Notifications.Notification
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using ReactiveUI;
using System;
using System.Linq.Expressions;
using System.Reactive.Linq;

namespace Innova.Launcher.Models.Notifications
{
  public sealed class Notification : ReactiveObject
  {
    private string _title;
    private string _message;
    private string _link;
    private readonly ObservableAsPropertyHelper<bool> _isLinkPresent;

    public string Title
    {
      get => this._title;
      set => this.RaiseAndSetIfChanged<Notification, string>(ref this._title, value, nameof (Title));
    }

    public string Message
    {
      get => this._message;
      set => this.RaiseAndSetIfChanged<Notification, string>(ref this._message, value, nameof (Message));
    }

    public string Link
    {
      get => this._link;
      set => this.RaiseAndSetIfChanged<Notification, string>(ref this._link, value, nameof (Link));
    }

    public bool IsLinkPresent => this._isLinkPresent.Value;

    public Notification() => this.WhenAnyValue<Notification, string>((Expression<Func<Notification, string>>) (v => v.Link)).Select<string, bool>((Func<string, bool>) (v => !string.IsNullOrWhiteSpace(v))).ToProperty<Notification, bool>(this, (Expression<Func<Notification, bool>>) (v => v.IsLinkPresent), out this._isLinkPresent);
  }
}
