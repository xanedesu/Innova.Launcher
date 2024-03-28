// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.FileSystemHelper
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;

namespace Innova.Launcher.Shared.Utils
{
  public static class FileSystemHelper
  {
    private static readonly string DefaultGameFolderName = "Games";
    private static readonly string DefaultGameFolderPath = "C:\\" + FileSystemHelper.DefaultGameFolderName + "\\";

    public static void DeleteFileIfExists(string filePath)
    {
      if (!File.Exists(filePath))
        return;
      File.Delete(filePath);
    }

    public static void DeleteDirectoryIfExists(string directoryPath, bool recursive = false)
    {
      if (!Directory.Exists(directoryPath))
        return;
      Directory.Delete(directoryPath, recursive);
    }

    public static void CreateDirectoryIfNotExists(string directoryPath)
    {
      if (Directory.Exists(directoryPath))
        return;
      Directory.CreateDirectory(directoryPath);
    }

    public static void RemoveEmptyDirectories(string directoryPath)
    {
      foreach (string directory in Directory.GetDirectories(directoryPath))
      {
        FileSystemHelper.RemoveEmptyDirectories(directory);
        FileSystemHelper.RemoveDirectoryIfEmpty(directory);
      }
    }

    public static void RemoveDirectoryIfEmpty(string directoryPath)
    {
      if (Directory.GetFiles(directoryPath).Length != 0 || Directory.GetDirectories(directoryPath).Length != 0)
        return;
      Directory.Delete(directoryPath, false);
    }

    public static string GetDefaultGameInstallPath(string gameName = null) => gameName == null ? FileSystemHelper.DefaultGameFolderPath : ((IEnumerable<DriveInfo>) DriveInfo.GetDrives()).Where<DriveInfo>((Func<DriveInfo, bool>) (v =>
    {
      try
      {
        return v.Name.Length > 0 && v.DriveType == DriveType.Fixed && v.TotalFreeSpace > 0L;
      }
      catch
      {
        return false;
      }
    })).OrderByDescending<DriveInfo, long>((Func<DriveInfo, long>) (v => v.TotalFreeSpace)).Select<DriveInfo, string>((Func<DriveInfo, string>) (v => string.Format("{0}:\\{1}\\{2}", (object) v.Name[0], (object) FileSystemHelper.DefaultGameFolderName, (object) gameName))).DefaultIfEmpty<string>(FileSystemHelper.DefaultGameFolderPath + gameName).First<string>();

    public static void CopyDirectory(string sourcePath, string destinationPath)
    {
      FileSystemHelper.CreateDirectoryIfNotExists(destinationPath);
      foreach (string directory in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        Directory.CreateDirectory(directory.Replace(sourcePath, destinationPath));
      foreach (string file in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        File.Copy(file, file.Replace(sourcePath, destinationPath), true);
    }

    public static long GetDirectorySize(
      string parentDirectory,
      string exceptContains = null,
      bool onlyVisible = false)
    {
      return ((IEnumerable<FileInfo>) new DirectoryInfo(parentDirectory).GetFiles("*", SearchOption.AllDirectories)).Where<FileInfo>((Func<FileInfo, bool>) (file => string.IsNullOrWhiteSpace(exceptContains) || !file.FullName.Contains(exceptContains))).Where<FileInfo>((Func<FileInfo, bool>) (file =>
      {
        if (!onlyVisible)
          return true;
        if (file.Attributes == FileAttributes.Hidden)
          return false;
        DirectoryInfo directory = file.Directory;
        return directory == null || directory.Attributes != FileAttributes.Hidden;
      })).Sum<FileInfo>((Func<FileInfo, long>) (file => file.Length));
    }

    public static long GetFreeSpace(string path)
    {
      if (!FileSystemHelper.IsValidPath(path, false))
        return 0;
      try
      {
        string pathRoot = Path.GetPathRoot(path);
        return string.IsNullOrEmpty(pathRoot) ? 0L : new DriveInfo(pathRoot).TotalFreeSpace;
      }
      catch
      {
        return 0;
      }
    }

    public static bool IsValidPath(string path, bool checkForDirectoryExists = true)
    {
      if (string.IsNullOrEmpty(path))
        return false;
      try
      {
        if (!Path.IsPathRooted(path))
          return false;
        DirectoryInfo directoryInfo = new DirectoryInfo(path);
        int num1;
        if (checkForDirectoryExists)
        {
          DirectoryInfo parent = directoryInfo.Parent;
          num1 = parent != null ? (parent.Exists ? 1 : 0) : 1;
        }
        else
          num1 = 1;
        int num2 = directoryInfo.Root.Exists ? 1 : 0;
        return (num1 & num2) != 0;
      }
      catch (SecurityException ex)
      {
      }
      catch (ArgumentException ex)
      {
      }
      catch (PathTooLongException ex)
      {
      }
      catch (NotSupportedException ex)
      {
      }
      return false;
    }

    public static string FindExistingDirectoryInPath(string path)
    {
      while (!Directory.Exists(path) && !string.IsNullOrWhiteSpace(path))
        path = Path.GetDirectoryName(path);
      return path;
    }

    public static string GetProgramFilesX86Folder() => !Environment.Is64BitOperatingSystem ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
  }
}
