// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageHandlers.Play.PlayMessageHandler
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Infrastructure.Crypto.Interfaces;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Newtonsoft.Json;

namespace Innova.Launcher.Core.Services.MessageHandlers.Play
{
  [WebMessageHandlerFilter(new string[] {"play", "playEncrypted"})]
  public class PlayMessageHandler : IWebMessageHandler
  {
    private readonly IGameManager _gameManager;
    private readonly ICryptoManager _cryptoManager;
    private readonly IOutputMessageDispatcherProvider _outputMessageDispatcherProvider;

    public PlayMessageHandler(
      IGameManager gameManager,
      ICryptoManager cryptoManager,
      IOutputMessageDispatcherProvider outputMessageDispatcherProvider)
    {
      this._gameManager = gameManager;
      this._cryptoManager = cryptoManager;
      this._outputMessageDispatcherProvider = outputMessageDispatcherProvider;
    }

    public void Handle(WebMessage webMessage)
    {
      try
      {
        if (webMessage.Type == "play")
        {
          PlayMessageHandler.PlayInputData playInputData = webMessage.Data.ToObject<PlayMessageHandler.PlayInputData>();
          GameLaunchData data = new GameLaunchData()
          {
            Login = playInputData.Login,
            AccountId = playInputData.AccountId,
            Password = playInputData.Password,
            Culture = playInputData.Culture
          };
          this._gameManager.Launch(playInputData.GameKey, data);
        }
        else if (webMessage.Type == "playEncrypted")
        {
          PlayMessageHandler.PlayEncryptedInputData encryptedInputData = webMessage.Data.ToObject<PlayMessageHandler.PlayEncryptedInputData>();
          PlayMessageHandler.EncryptedPassword encryptedPassword = JsonConvert.DeserializeObject<PlayMessageHandler.EncryptedPassword>(this._cryptoManager.Decrypt(encryptedInputData.PayloadEncrypted));
          GameLaunchData data = new GameLaunchData()
          {
            Login = encryptedInputData.Login,
            AccountId = encryptedInputData.AccountId,
            Password = encryptedPassword.Password,
            Culture = encryptedInputData.Culture
          };
          this._gameManager.Launch(encryptedInputData.GameKey, data);
        }
        this.SendPlaySuccessMessage(webMessage);
      }
      catch
      {
        this.SendPlayFailedMessage(webMessage);
      }
    }

    private void SendPlaySuccessMessage(WebMessage webMessage) => this._outputMessageDispatcherProvider.Get(webMessage.WindowId ?? "main").Dispatch(new LauncherMessage(webMessage.Id, "playSuccess"));

    private void SendPlayFailedMessage(WebMessage webMessage) => this._outputMessageDispatcherProvider.Get(webMessage.WindowId ?? "main").Dispatch(new LauncherMessage(webMessage.Id, "playFailed"));

    private abstract class PlayInputBaseData
    {
      [JsonProperty("serviceId")]
      public string GameKey { get; set; }

      [JsonProperty("login")]
      public string Login { get; set; }

      [JsonProperty("accountId")]
      public string AccountId { get; set; }
    }

    private class PlayInputData : PlayMessageHandler.PlayInputBaseData
    {
      [JsonProperty("password")]
      public string Password { get; set; }

      [JsonProperty("culture")]
      public string Culture { get; set; }
    }

    private class PlayEncryptedInputData : PlayMessageHandler.PlayInputBaseData
    {
      [JsonProperty("payloadEncrypted")]
      public string PayloadEncrypted { get; set; }

      [JsonProperty("culture")]
      public string Culture { get; set; }
    }

    private class EncryptedPassword
    {
      [JsonProperty("password")]
      public string Password { get; set; }

      [JsonProperty("hwId")]
      public string HardwareId { get; set; }
    }
  }
}
