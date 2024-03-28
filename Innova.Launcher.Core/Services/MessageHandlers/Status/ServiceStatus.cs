// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Status.ServiceStatus
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services.MessageHandlers.Status
{
  public class ServiceStatus
  {
    [JsonProperty("serviceId")]
    public string GameKey { get; set; }

    [JsonProperty("status")]
    public GameStatus Status { get; set; }

    [JsonProperty("lastAction")]
    public Trigger LastAction { get; set; }

    [JsonProperty("environment")]
    public string Environment { get; set; }

    [JsonProperty("extendedStatus")]
    public string ExtendedStatus { get; set; }

    [JsonProperty("info")]
    public object Info { get; set; }

    [JsonProperty("configurable")]
    public bool Configurable { get; set; }

    [JsonProperty("action")]
    public string Action { get; set; }

    [JsonProperty("installationStartDate")]
    public DateTime? InstallationStartDate { get; set; }

    [JsonProperty("errorType")]
    public string ErrorType { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("errorDescription")]
    public string ErrorDescription { get; set; }

    [JsonProperty("errorData")]
    public object ErrorData { get; set; }

    [JsonProperty("version")]
    public string Version { get; set; }

    [JsonProperty("availableVersion")]
    public string AvailableVersion { get; set; }

    [JsonProperty("currentGameEvent")]
    public string CurrentGameEvent { get; set; }

    [JsonProperty("availableEvents")]
    public List<AvailableEvent> AvailableEvents { get; set; } = new List<AvailableEvent>();

    [JsonProperty("languages")]
    public string[] Languages { get; set; }

    [JsonProperty("culture")]
    public string Culture { get; set; }
  }
}
