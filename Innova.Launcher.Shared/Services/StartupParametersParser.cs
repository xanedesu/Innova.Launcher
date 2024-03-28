// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Services.StartupParametersParser
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Innova.Launcher.Shared.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Innova.Launcher.Shared.Services
{
  public class StartupParametersParser : IStartupParametersParser
  {
    public string GetParameterValue(string[] args, string parameterName)
    {
      if (this.ParameterExists(args, parameterName))
      {
        int num = Array.IndexOf<string>(args, parameterName);
        if (num >= 0)
        {
          int index1 = num + 1;
          string parameterValue = index1 < args.Length ? args[index1] : (string) null;
          if (parameterValue != null)
          {
            int index2 = index1 + 1;
            for (string empty = string.Empty; index2 < args.Length && !empty.StartsWith("/"); ++index2)
            {
              empty = args[index2];
              if (!empty.StartsWith("/"))
                parameterValue = parameterValue + " " + empty;
            }
          }
          return parameterValue;
        }
      }
      return (string) null;
    }

    public bool ParameterExists(string[] args, string parameterName) => ((IEnumerable<string>) args).Contains<string>(parameterName);
  }
}
