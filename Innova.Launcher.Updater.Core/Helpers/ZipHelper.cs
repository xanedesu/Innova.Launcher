// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Updater.Core.Helpers.ZipHelper
// Assembly: Innova.Launcher.Updater.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 55CF4013-1E26-4FB5-BC71-D3CB5796263D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Updater.Core.dll

using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace Innova.Launcher.Updater.Core.Helpers
{
  public static class ZipHelper
  {
    public static void UnzipFromStream(Stream archiveStream, string outFolder)
    {
      ZipFile zipFile = (ZipFile) null;
      try
      {
        zipFile = new ZipFile(archiveStream);
        foreach (ZipEntry zipEntry in zipFile)
        {
          string name = zipEntry.Name;
          using (FileStream destination = File.OpenWrite(Path.Combine(outFolder, name)))
            zipFile.GetInputStream(zipEntry).CopyTo((Stream) destination);
        }
      }
      finally
      {
        if (zipFile != null)
        {
          zipFile.IsStreamOwner = true;
          zipFile.Close();
        }
      }
    }
  }
}
