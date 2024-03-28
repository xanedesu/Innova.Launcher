// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Models.Errors.GameNotEnoughSpaceError
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Newtonsoft.Json;

namespace Innova.Launcher.Core.Models.Errors
{
  public class GameNotEnoughSpaceError : BaseError
  {
    [JsonProperty("data")]
    public GameNotEnoughSpaceError.NotEnoughSpaceErrorData Data { get; set; }

    [JsonConstructor]
    public GameNotEnoughSpaceError()
      : base("not_enough_space")
    {
    }

    public GameNotEnoughSpaceError(long freeSpace, long gameSize)
      : this(new GameNotEnoughSpaceError.NotEnoughSpaceErrorData(freeSpace, gameSize))
    {
    }

    private GameNotEnoughSpaceError(
      GameNotEnoughSpaceError.NotEnoughSpaceErrorData errorData)
      : this()
    {
      this.Data = errorData;
    }

    public class NotEnoughSpaceErrorData
    {
      [JsonProperty("freeSpace")]
      public long FreeSpace { get; set; }

      [JsonProperty("gameSize")]
      public long GameSize { get; set; }

      public NotEnoughSpaceErrorData(long freeSpace, long gameSize)
      {
        this.FreeSpace = freeSpace;
        this.GameSize = gameSize;
      }

      [JsonConstructor]
      public NotEnoughSpaceErrorData()
      {
      }
    }
  }
}
