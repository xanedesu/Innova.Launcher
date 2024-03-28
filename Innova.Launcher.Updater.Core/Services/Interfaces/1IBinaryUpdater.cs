// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Services.Interfaces.UpdateProgressError
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

namespace Innova.Launcher.Updater.Core.Services.Interfaces
{
  public class UpdateProgressError
  {
    public int Code { get; }

    public string Error { get; }

    public UpdateProgressError(int code, string error)
    {
      this.Code = code;
      this.Error = error;
    }
  }
}
