// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.ShortcutHelper
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace Innova.Launcher.Shared.Utils
{
  public static class ShortcutHelper
  {
    public static void CreateOrReplaceDesktopShortcut(
      string name,
      string iconPath,
      string iconTarget,
      string targetArgs = null)
    {
      ShortcutHelper.CreateOrReplaceShortcut(ShortcutHelper.GetDesktopShortcutLocation(name), name, iconPath, iconTarget, targetArgs);
    }

    public static void DeleteDesktopShortcut(string name) => FileSystemHelper.DeleteFileIfExists(ShortcutHelper.GetDesktopShortcutLocation(name));

    public static void CreateOrReplaceStartMenuShortcut(
      string publisher,
      string name,
      string iconPath,
      string iconTarget)
    {
      ShortcutHelper.CreateOrReplaceShortcut(ShortcutHelper.GetStartMenuShortcutLocation(publisher, name), name, iconPath, iconTarget);
    }

    public static void DeleteStartMenuShortcut(string publisher, string name) => FileSystemHelper.DeleteFileIfExists(ShortcutHelper.GetStartMenuShortcutLocation(publisher, name));

    public static void CreateOrReplaceShortcut(
      string shortcutPath,
      string name,
      string iconPath,
      string iconTarget,
      string targetArgs = null)
    {
      try
      {
        ShortcutHelper.IShellLink shellLink = (ShortcutHelper.IShellLink) new ShortcutHelper.ShellLink();
        FileSystemHelper.DeleteFileIfExists(shortcutPath);
        shellLink.SetDescription(name);
        shellLink.SetArguments(targetArgs);
        shellLink.SetPath(iconTarget);
        shellLink.SetIconLocation(iconPath, 0);
        ((IPersistFile) shellLink).Save(shortcutPath, false);
      }
      catch
      {
      }
    }

    private static string GetDesktopShortcutLocation(string name) => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), name + ".lnk");

    private static string GetStartMenuShortcutLocation(string publisher, string name)
    {
      string str = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms), publisher);
      FileSystemHelper.CreateDirectoryIfNotExists(str);
      return Path.Combine(str, name + ".lnk");
    }

    [Guid("00021401-0000-0000-C000-000000000046")]
    [ComImport]
    internal class ShellLink
    {
      [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
      public extern ShellLink();
    }

    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214F9-0000-0000-C000-000000000046")]
    [ComImport]
    internal interface IShellLink
    {
      void GetPath([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszFile, int cchMaxPath, out IntPtr pfd, int fFlags);

      void GetIDList(out IntPtr ppidl);

      void SetIDList(IntPtr pidl);

      void GetDescription([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszName, int cchMaxName);

      void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string pszName);

      void GetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszDir, int cchMaxPath);

      void SetWorkingDirectory([MarshalAs(UnmanagedType.LPWStr)] string pszDir);

      void GetArguments([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszArgs, int cchMaxPath);

      void SetArguments([MarshalAs(UnmanagedType.LPWStr)] string pszArgs);

      void GetHotkey(out short pwHotkey);

      void SetHotkey(short wHotkey);

      void GetShowCmd(out int piShowCmd);

      void SetShowCmd(int iShowCmd);

      void GetIconLocation([MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder pszIconPath, int cchIconPath, out int piIcon);

      void SetIconLocation([MarshalAs(UnmanagedType.LPWStr)] string pszIconPath, int iIcon);

      void SetRelativePath([MarshalAs(UnmanagedType.LPWStr)] string pszPathRel, int dwReserved);

      void Resolve(IntPtr hwnd, int fFlags);

      void SetPath([MarshalAs(UnmanagedType.LPWStr)] string pszFile);
    }
  }
}
