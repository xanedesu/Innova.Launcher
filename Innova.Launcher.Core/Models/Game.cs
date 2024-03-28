// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.Game
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Innova.Launcher.Core.Models.Errors;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using LiteDB;
using System;

namespace Innova.Launcher.Core.Models
{
  public class Game
  {
    [BsonId]
    public string Key { get; set; }

    public string EnvKey { get; set; }

    public string FrostKey { get; set; }

    public string Environment { get; set; }

    public string Name { get; set; }

    public string Path { get; set; }

    public ProgressInfo ProgressInfo { get; set; }

    public GameStatus Status { get; set; }

    public UpdateType UpdateType { get; set; }

    public string ExtendedStatus { get; set; }

    public string OptionsExe { get; set; }

    public string Version { get; set; }

    public string PreviousVersion { get; set; }

    public string AvailableVersion { get; set; }

    public DateTime? InstallationStartDate { get; set; }

    public string Url { get; set; }

    public string PreviousUrl { get; set; }

    public Trigger LastAction { get; set; }

    public string CurrentGameEvent { get; set; }

    public string Culture { get; set; }

    public string ErrorType { get; set; }

    public string Error { get; set; }

    public string ErrorDescription { get; set; }

    public BaseError ErrorData { get; set; }

    public bool NeedUpdate() => this.Status == GameStatus.Installed && this.UpdateType != 0;
  }
}
