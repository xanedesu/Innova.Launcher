// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Models.GameConfig.Operations.CopyFolderOperation
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.IO;
using System.Xml.Serialization;

namespace Innova.Launcher.Shared.Models.GameConfig.Operations
{
  public class CopyFolderOperation : Operation
  {
    [XmlAttribute("from")]
    public string From { get; set; }

    [XmlAttribute("to")]
    public string To { get; set; }

    [XmlAttribute("check")]
    public string CheckFile { get; set; }

    public override void Execute()
    {
      if (this.IsSameControlFile())
        return;
      Directory.Delete(this.To, true);
      this.CopyFolder();
    }

    private void CopyFolder()
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(this.From);
      string fullCheckFileName = Path.Combine(directoryInfo1.FullName, this.CheckFile);
      DirectoryInfo directoryInfo2 = new DirectoryInfo(this.To);
      CopyFolderOperation.DirectoryCopy(directoryInfo1.FullName, directoryInfo2.FullName, new Func<string, bool>(SkipCheckFile));
      File.Copy(fullCheckFileName, Path.Combine(directoryInfo2.FullName, this.CheckFile), true);

      bool SkipCheckFile(string fullName) => StringComparer.InvariantCultureIgnoreCase.Equals(fullName, fullCheckFileName);
    }

    private static void DirectoryCopy(
      string sourceDirName,
      string destDirName,
      Func<string, bool> skipFile)
    {
      DirectoryInfo directoryInfo1 = new DirectoryInfo(sourceDirName);
      DirectoryInfo[] directoryInfoArray = directoryInfo1.Exists ? directoryInfo1.GetDirectories() : throw new DirectoryNotFoundException("Source directory does not exist or could not be found: " + sourceDirName);
      Directory.CreateDirectory(destDirName);
      foreach (FileInfo file in directoryInfo1.GetFiles())
      {
        if (!skipFile(file.FullName))
        {
          string destFileName = Path.Combine(destDirName, file.Name);
          file.CopyTo(destFileName, false);
        }
      }
      foreach (DirectoryInfo directoryInfo2 in directoryInfoArray)
      {
        string destDirName1 = Path.Combine(destDirName, directoryInfo2.Name);
        CopyFolderOperation.DirectoryCopy(directoryInfo2.FullName, destDirName1, skipFile);
      }
    }

    private bool IsSameControlFile()
    {
      FileInfo fileInfo1 = new FileInfo(Path.Combine(this.From, this.CheckFile));
      if (!fileInfo1.Exists)
        throw new Exception("Invalid from directory content: missing check file");
      FileInfo fileInfo2 = new FileInfo(Path.Combine(this.To, this.CheckFile));
      if (!fileInfo2.Exists || fileInfo1.Length != fileInfo2.Length)
        return false;
      using (FileStream fileStream1 = fileInfo1.OpenRead())
      {
        using (FileStream fileStream2 = fileInfo2.OpenRead())
        {
          for (int index = 0; (long) index < fileInfo1.Length; ++index)
          {
            if (fileStream1.ReadByte() != fileStream2.ReadByte())
              return false;
          }
          return true;
        }
      }
    }
  }
}
