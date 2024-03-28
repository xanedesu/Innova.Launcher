// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Services.LauncherTrackingService
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using CommonServiceLocator;
using Innova.Launcher.Core.Models.Errors;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Core.Services.MessageHandlers.Tracking;
using Innova.Launcher.Shared.Logging;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Tracking.Interfaces;
using Innova.Launcher.Shared.Tracking.Models;
using Innova.Launcher.Shared.Tracking.Models.Application;
using Innova.Launcher.Shared.Utils;
using NLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;

namespace Innova.Launcher.Services
{
  public class LauncherTrackingService : ILauncherTrackingService
  {
    private const string CriticalUpdateStarted = "users.launcher.update.critical.started";
    private const string CriticalUpdateFinished = "users.launcher.update.critical.finished";
    private const string CasualUpdateStarted = "users.launcher.update.regular.started";
    private const string CasualUpdateFinished = "users.launcher.update.regular.finished";
    private const string LauncherLaunched = "users.launcher.launched";
    private const string LauncherClosed = "users.launcher.closed";
    private const string MainWindowMinimized = "users.launcher.window.minimized";
    private const string MainWindowRestored = "users.launcher.window.restored";
    private const string LauncherHidedToTray = "users.launcher.hided";
    private const string LauncherRestoredFromTray = "users.launcher.restored";
    private readonly TrackingEventMessageHandler _trackingEventMessageHandler;
    private static readonly string[] _applicationsToLog = new string[7]
    {
      "Innova.Launcher",
      "Innova.Launcher.Runner",
      "Innova.Launcher.Installer",
      "Innova.Launcher.GameManager",
      "4game",
      "gameManager",
      "4updater"
    };
    private readonly string _eventLogAppFilter = string.Join(" or ", ((IEnumerable<string>) LauncherTrackingService._applicationsToLog).Select<string, string>((Func<string, string>) (e => "Data='" + e + "'")));
    private ITrackingService _trackingService;
    private ILauncherVersionProvider _launcherVersionProvider;
    private IHardwareProvider _hardwareProvider;

    public LauncherTrackingService()
    {
    }

    public LauncherTrackingService(
      ILoggerFactory loggerFactory,
      ITrackingService trackingService,
      ILauncherVersionProvider launcherVersionProvider,
      TrackingEventMessageHandler trackingEventMessageHandler,
      IHardwareProvider hardwareProvider)
    {
      this._trackingService = trackingService ?? throw new ArgumentNullException(nameof (trackingService));
      this._hardwareProvider = hardwareProvider ?? throw new ArgumentNullException(nameof (hardwareProvider));
      this._launcherVersionProvider = launcherVersionProvider ?? throw new ArgumentNullException(nameof (launcherVersionProvider));
      this._trackingEventMessageHandler = trackingEventMessageHandler;
      this._trackingService.Start();
    }

    public void LogException(Exception exception, string message)
    {
      try
      {
        ServiceLocator.Current.GetInstance<ILoggerFactory>().GetCurrentClassLogger<App>().Fatal(exception, message);
      }
      catch (Exception ex)
      {
      }
      try
      {
        this.SendTrackingError(message, exception.ToString());
      }
      catch (Exception ex1)
      {
        try
        {
          using (EventLog eventLog = new EventLog("Application"))
          {
            eventLog.Source = "4game";
            eventLog.WriteEntry(string.Format("{0}. {1}", (object) message, (object) exception), EventLogEntryType.Error);
          }
        }
        catch (Exception ex2)
        {
        }
      }
    }

    public void SendUntrackedErrors()
    {
      try
      {
        DateTime lastCheckDate = this.GetLastCheckDate() ?? DateTime.UtcNow;
        if ((DateTime.UtcNow - lastCheckDate).TotalDays > 14.0)
          lastCheckDate = DateTime.UtcNow - TimeSpan.FromDays(14.0);
        List<LauncherError> launcherErrorList = new List<LauncherError>();
        this.UpdateLastErrorCheckDate();
        try
        {
          IEnumerable<LauncherError> collection = this.GetEventLogErrors(lastCheckDate).Select<LogMessage, LauncherError>((Func<LogMessage, LauncherError>) (error => new LauncherError()
          {
            Error = "EventLog. Unhandled error",
            Description = error.Message,
            Time = error.Time
          }));
          launcherErrorList.AddRange(collection);
        }
        catch (Exception ex)
        {
        }
        try
        {
          IEnumerable<LauncherError> collection = this.GetLauncherLogErrors(lastCheckDate).Select<LogMessage, LauncherError>((Func<LogMessage, LauncherError>) (error => new LauncherError()
          {
            Error = "Log. Unhandled error",
            Description = error.Message,
            Time = error.Time
          }));
          launcherErrorList.AddRange(collection);
        }
        catch (Exception ex)
        {
        }
        foreach (LauncherError error in launcherErrorList)
          this.SendTrackingError(error);
      }
      catch (Exception ex)
      {
      }
    }

    public void UpdateLastErrorCheckDate() => RegistryHelper.TryUpdateLastErrorCheckDate("Innova Co. SARL", "4game2.0", DateTime.UtcNow);

    public void SendTrackingInfo(string type, bool sendAsync = true) => this._trackingEventMessageHandler.SendTrackingEvent(type, sendAsync);

    public void SendLauncherUpdated(bool updateIsCritical) => this.SendTrackingInfo(updateIsCritical ? "users.launcher.update.critical.finished" : "users.launcher.update.regular.finished", true);

    public void SendLauncherUpdateStarted(bool updateIsCritical) => this.SendTrackingInfo(updateIsCritical ? "users.launcher.update.critical.started" : "users.launcher.update.regular.started", true);

    public void SendLauncherLaunched() => this.SendTrackingInfo("users.launcher.launched", true);

    public void SendLauncherClosed() => this.SendTrackingInfo("users.launcher.closed", true);

    public void SendMainWindowMinimized() => this.SendTrackingInfo("users.launcher.window.minimized", true);

    public void SendMainWindowRestored() => this.SendTrackingInfo("users.launcher.window.restored", true);

    public void SendLauncherHidedToTray() => this.SendTrackingInfo("users.launcher.hided", true);

    public void SendLauncherRestoredFromTray() => this.SendTrackingInfo("users.launcher.restored", true);

    public void SendHardware()
    {
      this.InitTrackingService();
      MessageBus.Current.SendMessage<TrackingEvent>(new TrackingEvent((UserEventBase) new LauncherHardwareEvent()
      {
        LauncherVersion = this._launcherVersionProvider.CurrentLauncherVersion,
        Hardware = this._hardwareProvider.Get()
      }));
    }

    private DateTime? GetLastCheckDate() => RegistryHelper.GetLastErrorCheckDate("Innova Co. SARL", "4game2.0");

    private List<LogMessage> GetEventLogErrors(DateTime lastCheckDate)
    {
      using (EventLogReader eventLogReader = new EventLogReader(new EventLogQuery("Application", PathType.LogName, string.Format("\r\n*[\r\n    (\r\n      EventData[{0}] \r\n      or \r\n      (System[Level = 2 and Provider[@Name = '.NET Runtime']])\r\n    )\r\n    and \r\n    System[TimeCreated[@SystemTime>='{1:O}']]\r\n]", (object) this._eventLogAppFilter, (object) lastCheckDate.ToLocalTime()))))
      {
        List<LogMessage> eventLogErrors = new List<LogMessage>();
        while (true)
        {
          EventRecord eventRecord;
          DateTime? nullable;
          do
          {
            eventRecord = eventLogReader.ReadEvent(TimeSpan.FromSeconds(10.0));
            if (eventRecord != null)
            {
              if (eventRecord.ProviderName == ".NET Runtime")
              {
                string evntXml = eventRecord.ToXml();
                if (!((IEnumerable<string>) LauncherTrackingService._applicationsToLog).Any<string>((Func<string, bool>) (e => evntXml.Contains(e))))
                  continue;
              }
              DateTime? timeCreated = eventRecord.TimeCreated;
              ref DateTime? local = ref timeCreated;
              nullable = local.HasValue ? new DateTime?(local.GetValueOrDefault().ToUniversalTime()) : new DateTime?();
            }
            else
              goto label_7;
          }
          while (!nullable.HasValue);
          eventLogErrors.Add(new LogMessage()
          {
            Message = eventRecord.ToXml(),
            LogLevel = LogLevel.Error,
            Time = nullable.Value
          });
        }
label_7:
        return eventLogErrors;
      }
    }

    private List<LogMessage> GetLauncherLogErrors(DateTime lastCheckDate)
    {
      IEnumerable<LogMessage> logFile = ServiceLocator.Current.GetInstance<ILoggerFactory>() is LoggerFactory instance ? instance.ParseLogFile() : (IEnumerable<LogMessage>) null;
      return (logFile != null ? logFile.Where<LogMessage>((Func<LogMessage, bool>) (e => e.LogLevel == LogLevel.Error || e.LogLevel == LogLevel.Fatal)).Where<LogMessage>((Func<LogMessage, bool>) (e => e.Time > lastCheckDate)).ToList<LogMessage>() : (List<LogMessage>) null) ?? new List<LogMessage>();
    }

    private void SendTrackingError(string error, string info) => this.SendTrackingError(new LauncherError()
    {
      Error = error,
      Description = info,
      Time = DateTime.UtcNow
    });

    public void SendTrackingError(LauncherError error)
    {
      this.InitTrackingService();
      IMessageBus current = MessageBus.Current;
      LauncherRuntimeErrorUserEvent userEvent = new LauncherRuntimeErrorUserEvent();
      userEvent.When = error.Time;
      userEvent.Error = error.Error;
      userEvent.ExtraInfo = error.Description;
      userEvent.LauncherVersion = this._launcherVersionProvider.CurrentLauncherVersion;
      TrackingEvent message = new TrackingEvent((UserEventBase) userEvent);
      current.SendMessage<TrackingEvent>(message);
    }

    private void InitTrackingService()
    {
      if (this._trackingService == null)
      {
        this._trackingService = ServiceLocator.Current.GetInstance<ITrackingService>();
        this._trackingService.Start();
      }
      if (this._launcherVersionProvider == null)
        this._launcherVersionProvider = ServiceLocator.Current.GetInstance<ILauncherVersionProvider>();
      if (this._hardwareProvider != null)
        return;
      this._hardwareProvider = ServiceLocator.Current.GetInstance<IHardwareProvider>();
    }
  }
}
