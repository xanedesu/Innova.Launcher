// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.IWindowPlacementSettings
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Standard;

namespace Innova.Launcher.UI.Controls
{
  public interface IWindowPlacementSettings
  {
    WINDOWPLACEMENT Placement { get; set; }

    void Reload();

    bool UpgradeSettings { get; set; }

    void Upgrade();

    void Save();
  }
}
