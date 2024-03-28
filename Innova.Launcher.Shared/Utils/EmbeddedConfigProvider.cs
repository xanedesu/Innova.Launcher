// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Utils.EmbeddedConfigProvider`1
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Innova.Launcher.Shared.Utils
{
  public class EmbeddedConfigProvider<T>
  {
    private readonly string _resourseName;
    private Dictionary<string, string> _parsedConfig;

    public EmbeddedConfigProvider(string resourseName)
    {
      this._resourseName = resourseName ?? throw new ArgumentNullException(nameof (resourseName));
      this.LoadConfiguration();
    }

    public string Get(string configKey)
    {
      if (string.IsNullOrWhiteSpace(configKey))
        throw new ArgumentOutOfRangeException(nameof (configKey));
      return !this._parsedConfig.ContainsKey(configKey) ? (string) null : this._parsedConfig[configKey];
    }

    private void LoadConfiguration()
    {
      Assembly assembly = Assembly.GetAssembly(typeof (T));
      string name = assembly.GetName().Name + "." + this._resourseName;
      using (Stream manifestResourceStream = assembly.GetManifestResourceStream(name))
      {
        if (manifestResourceStream == null)
          throw new Exception("There is no embeded resource " + name + "!");
        using (StreamReader reader = new StreamReader(manifestResourceStream))
          this._parsedConfig = this.ParseConfigFile(reader);
      }
    }

    private Dictionary<string, string> ParseConfigFile(StreamReader reader)
    {
      Dictionary<string, string> configFile = new Dictionary<string, string>();
      string str1;
      while ((str1 = reader.ReadLine()) != null)
      {
        int length = str1.IndexOf(" ", StringComparison.Ordinal);
        if (length != -1)
        {
          string key = str1.Substring(0, length);
          string str2 = str1.Substring(length + 1);
          configFile[key] = str2;
        }
      }
      return configFile;
    }
  }
}
