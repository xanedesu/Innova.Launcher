﻿// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.Interfaces.IBackendApi
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

namespace Innova.Launcher.Services.Interfaces
{
  public interface IBackendApi
  {
    void send(string message);

    void dragMove();

    void close();

    void maximize();

    void minimize();

    string getCulture();

    void InitShell(IMainShellPresenter shell);
  }
}
