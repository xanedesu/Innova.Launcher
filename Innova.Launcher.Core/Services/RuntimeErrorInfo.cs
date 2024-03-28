// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.RuntimeErrorInfo
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models.Errors;
using System;

namespace Innova.Launcher.Core.Services
{
  public class RuntimeErrorInfo
  {
    public string GameKey { get; }

    public string ErrorType { get; }

    public string ErrorCode { get; }

    public string ErrorMessage { get; }

    public BaseError ErrorData { get; set; }

    public RuntimeErrorInfo(
      string errorType,
      string gameKey,
      string errorCode,
      string errorMessage)
    {
      this.ErrorType = errorType ?? throw new ArgumentNullException(nameof (errorType));
      this.GameKey = gameKey ?? throw new ArgumentNullException(nameof (gameKey));
      this.ErrorCode = errorCode ?? throw new ArgumentNullException(nameof (errorCode));
      this.ErrorMessage = errorMessage ?? throw new ArgumentNullException(nameof (errorMessage));
    }
  }
}
