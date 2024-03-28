// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Infrastructure.Internet.DefaultInternetConnectionConfigProvider
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

namespace Innova.Launcher.Shared.Infrastructure.Internet
{
  public class DefaultInternetConnectionConfigProvider : IInternetConnectionCheckerConfigProvider
  {
    public string CheckAddress { get; } = "http://cdn.inn.ru/cdn_status.html";

    public string SuccessResponseText { get; } = "4good";

    public int CheckInterval { get; } = 1000;
  }
}
