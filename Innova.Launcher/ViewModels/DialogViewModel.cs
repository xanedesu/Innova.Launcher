// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.ViewModels.DialogViewModel
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using ReactiveUI;

namespace Innova.Launcher.ViewModels
{
  public class DialogViewModel : ReactiveObject
  {
    private string _message;
    private string _title;

    public string Message
    {
      get => this._message;
      set => this.RaiseAndSetIfChanged<DialogViewModel, string>(ref this._message, value, nameof (Message));
    }

    public string Title
    {
      get => this._title;
      set => this.RaiseAndSetIfChanged<DialogViewModel, string>(ref this._title, value, nameof (Title));
    }
  }
}
