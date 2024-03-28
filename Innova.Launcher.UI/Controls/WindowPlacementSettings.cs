// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.WindowApplicationSettings
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Standard;
using System;
using System.Configuration;
using System.Windows;

namespace Innova.Launcher.UI.Controls
{
  internal class WindowApplicationSettings : ApplicationSettingsBase, IWindowPlacementSettings
  {
    public WindowApplicationSettings(Window window)
      : base(((object) window).GetType().FullName)
    {
    }

    [UserScopedSetting]
    public WINDOWPLACEMENT Placement
    {
      get => this[nameof (Placement)] != null ? (WINDOWPLACEMENT) this[nameof (Placement)] : (WINDOWPLACEMENT) null;
      set => this[nameof (Placement)] = (object) value;
    }

    [UserScopedSetting]
    public bool UpgradeSettings
    {
      get
      {
        try
        {
          if (this[nameof (UpgradeSettings)] != null)
            return (bool) this[nameof (UpgradeSettings)];
        }
        catch (ConfigurationErrorsException ex)
        {
          ConfigurationErrorsException innerException = ex;
          string str = (string) null;
          while (innerException != null && (str = innerException.Filename) == null)
            innerException = ((Exception) innerException).InnerException as ConfigurationErrorsException;
          throw new InvalidOperationException("The settings file '" + (str ?? "<unknown>") + "' seems to be corrupted", (Exception) innerException);
        }
        return true;
      }
      set => this[nameof (UpgradeSettings)] = (object) value;
    }

    void IWindowPlacementSettings.Reload() => this.Reload();
  }
}
