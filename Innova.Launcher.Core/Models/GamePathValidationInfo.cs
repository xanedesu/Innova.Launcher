// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.GamePathValidationInfo
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models.Errors;
using System;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Models
{
  public class GamePathValidationInfo
  {
    public string Path { get; set; }

    public long? GameSize { get; set; }

    public long? FreeSpace { get; set; }

    public string DriveLetter { get; set; }

    public List<BaseError> Errors { get; set; }

    public bool HasErrors
    {
      get
      {
        List<BaseError> errors = this.Errors;
        return errors != null && __nonvirtual (errors.Count) > 0;
      }
    }

    public void AddError(BaseError error)
    {
      if (this.Errors == null)
        this.Errors = new List<BaseError>();
      this.Errors.Add(error);
    }

    public void RemoveErrors<T>() where T : BaseError => this.Errors.RemoveAll((Predicate<BaseError>) (e => e.GetType() == typeof (T)));
  }
}
