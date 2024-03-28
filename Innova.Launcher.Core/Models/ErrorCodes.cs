// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.ErrorCodes
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

namespace Innova.Launcher.Core.Models
{
  public static class ErrorCodes
  {
    public static class GameInstall
    {
      public const string NotEnoughSpace = "not_enough_space";
      public const string InvalidPath = "invalid_path";
      public const string StartInstallError = "start_install_error";
    }

    public static class GameInstallProgress
    {
      public const string GameInstallProgressError = "game_install_progress_error";
      public const string AdditionalComponentsInstallError = "additional_component_install_error";
      public const string AdditionalComponentsStartInstallError = "additional_component_start_install_error";
      public const string GameRegistrationError = "game_registration_error";
    }

    public static class GameLaunch
    {
      public const string FrostExeNotFound = "frost_exe_not_found";
      public const string FrostUpdateError = "frost_update_error";
      public const string GameExeNotFound = "game_exe_not_found";
      public const string GameExeNotStarted = "game_exe_not_started";
      public const string UnknownException = "game_launcher_unknown_error";
    }

    public static class GameRepair
    {
      public const string StartRepairError = "start_repair_error";
    }

    public class GameUpdate
    {
      public const string StartUpdateError = "start_update_error";
    }
  }
}
