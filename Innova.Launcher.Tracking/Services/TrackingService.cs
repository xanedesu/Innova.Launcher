// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Tracking.Services.TrackingService
// Assembly: Innova.Launcher.Tracking, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: DA384C86-6E9B-47C9-B483-AED3A5709C44
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Tracking.dll

using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using Innova.Launcher.Shared.Tracking.Interfaces;
using Innova.Launcher.Shared.Tracking.Models;
using Innova.Launcher.Tracking.Models;
using Newtonsoft.Json;
using NLog;
using Polly;
using Polly.Retry;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Tracking.Services
{
  public class TrackingService : ITrackingService
  {
    private readonly ILauncherIdProvider _launcherIdProvider;
    private readonly IHardwareIdProvider _hardwareIdProvider;
    private string _launcherId;
    private string _hardwareId;

    public string TrackingId { get; set; }

    public string LauncherId => this._launcherId ?? (this._launcherId = this._launcherIdProvider.Get());

    public string HardwareId => this._hardwareId ?? (this._hardwareId = this._hardwareIdProvider.Get());

    public TrackingService(
      ILoggerFactory loggerFactory,
      ITrackingConfiguration configuration,
      ILauncherIdProvider launcherIdProvider,
      IHardwareIdProvider hardwareIdProvider,
      IComputerNameProvider computerNameProvider)
    {
      TrackingService trackingService = this;
      this._hardwareIdProvider = hardwareIdProvider ?? throw new ArgumentNullException(nameof (hardwareIdProvider));
      this._launcherIdProvider = launcherIdProvider ?? throw new ArgumentNullException(nameof (launcherIdProvider));
      ILogger logger = loggerFactory.GetCurrentClassLogger<TrackingService>();
      HttpClient httpClient = new HttpClient();
      AsyncRetryPolicy policy = AsyncRetrySyntax.WaitAndRetryAsync(Policy.Handle<HttpRequestException>(), 5, (Func<int, TimeSpan>) (v => TimeSpan.FromMilliseconds(100.0)));
      MessageBus.Current.ListenIncludeLatest<TrackingEvent>().Where<TrackingEvent>((Func<TrackingEvent, bool>) (v => v != null)).Buffer<TrackingEvent>(TimeSpan.FromSeconds(1.0)).Where<IList<TrackingEvent>>((Func<IList<TrackingEvent>, bool>) (buffer => buffer.Count > 0)).SubscribeOn<IList<TrackingEvent>>((IScheduler) ThreadPoolScheduler.Instance).SelectMany<IList<TrackingEvent>, bool>((Func<IList<TrackingEvent>, Task<bool>>) (async events =>
      {
        try
        {
          AddEventsRequest addEventsRequest = new AddEventsRequest()
          {
            Events = events.Select<TrackingEvent, UserEventBase>((Func<TrackingEvent, UserEventBase>) (v => v.UserEvent.TrimData(20000))).ToList<UserEventBase>()
          };
          foreach (UserEventBase userEventBase in addEventsRequest.Events)
            userEventBase.ServiceId = configuration.ServiceId;
          string serialized = JsonConvert.SerializeObject((object) addEventsRequest);
          HttpResponseMessage httpResponseMessage = await policy.ExecuteAsync<HttpResponseMessage>((Func<Task<HttpResponseMessage>>) (() =>
          {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, configuration.Url)
            {
              Content = (HttpContent) new StringContent(serialized, Encoding.UTF8, "application/json")
            };
            // ISSUE: explicit non-virtual call
            request.Headers.TryAddWithoutValidation("X-Tracking-Id", __nonvirtual (trackingService.TrackingId));
            // ISSUE: explicit non-virtual call
            request.Headers.TryAddWithoutValidation("Launcher-Id", __nonvirtual (trackingService.LauncherId));
            // ISSUE: explicit non-virtual call
            request.Headers.TryAddWithoutValidation("Hardware-Id", __nonvirtual (trackingService.HardwareId));
            request.Headers.TryAddWithoutValidation("X-Request-Id", Guid.NewGuid().ToString());
            return httpClient.SendAsync(request);
          }));
        }
        catch (Exception ex)
        {
          logger.Error(ex, "Can't send tracking info");
        }
        return true;
      })).Subscribe<bool>((Action<bool>) (_ => { }));
    }

    public void Flush() => Thread.Sleep(1000);

    public void Start()
    {
    }
  }
}
