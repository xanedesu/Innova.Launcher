﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.Errors.AdditionalComponentInstallError
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Core.Models.Errors
{
  public class AdditionalComponentInstallError : BaseError
  {
    [JsonProperty("component")]
    public string Component { get; set; }

    [JsonConstructor]
    public AdditionalComponentInstallError()
    {
    }

    public AdditionalComponentInstallError(string code, string component)
      : base(code)
    {
      this.Component = component;
    }

    public AdditionalComponentInstallError(string component) => this.Component = component;
  }
}
