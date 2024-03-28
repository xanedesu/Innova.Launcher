// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.CommandLineArgsProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;

namespace Innova.Launcher.Shared.Services
{
  public sealed class CommandLineArgsProvider : ICommandLineArgsProvider
  {
    private string[] _value;

    public string[] GetCommandLineArgs() => this._value ?? Environment.GetCommandLineArgs();

    public void SetCommandLineArgs(string[] value) => this._value = value;
  }
}
