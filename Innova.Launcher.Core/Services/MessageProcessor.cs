// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.MessageProcessor
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.Interfaces;
using Innova.Launcher.Shared.Logging.Interfaces;
using Innova.Launcher.Shared.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Innova.Launcher.Core.Services
{
  public class MessageProcessor
  {
    private readonly ImmutableArray<MessageProcessor.FilteredHandler> _handlers;
    private readonly ILifetimeManager _lifetimeManager;
    private readonly BrowserInputMessagesQueue _inputQueue;
    private readonly Task _worker;

    public MessageProcessor(
      ILifetimeManager lifetimeManager,
      BrowserInputMessagesQueue inputQueue,
      IWebMessageHandler[] handlers,
      ILoggerFactory loggerFactory)
    {
      ILogger logger = loggerFactory.GetCurrentClassLogger<MessageProcessor>();
      this._inputQueue = inputQueue;
      this._lifetimeManager = lifetimeManager;
      this._handlers = ((IEnumerable<IWebMessageHandler>) handlers).Select<IWebMessageHandler, MessageProcessor.FilteredHandler>((Func<IWebMessageHandler, MessageProcessor.FilteredHandler>) (h => new MessageProcessor.FilteredHandler(h, logger))).ToImmutableArray<MessageProcessor.FilteredHandler>();
      this._worker = this.StartHandling();
    }

    private Task StartHandling() => Task.Factory.StartNew((Action) (() =>
    {
      try
      {
        while (this._lifetimeManager.IsAlive)
        {
          WebMessage webMessage = this._inputQueue.Take(this._lifetimeManager.CancellationToken);
          try
          {
            ImmutableArray<MessageProcessor.FilteredHandler>.Enumerator enumerator = this._handlers.GetEnumerator();
            while (enumerator.MoveNext())
            {
              MessageProcessor.FilteredHandler current = enumerator.Current;
              if (current.CanHandle(webMessage))
                current.Handle(webMessage);
            }
          }
          catch
          {
          }
        }
      }
      catch (OperationCanceledException ex)
      {
      }
    }), CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);

    private class FilteredHandler
    {
      private readonly IWebMessageHandler _handler;
      private readonly ILogger _logger;
      private readonly string[] _filter;

      public FilteredHandler(IWebMessageHandler handler, ILogger logger)
      {
        this._handler = handler;
        this._logger = logger;
        this._filter = TypeDescriptor.GetAttributes(handler.GetType()).OfType<WebMessageHandlerFilterAttribute>().FirstOrDefault<WebMessageHandlerFilterAttribute>()?.MessageTypes;
      }

      public bool CanHandle(WebMessage item) => this._filter == null || ((IEnumerable<string>) this._filter).Contains<string>(item.Type);

      public void Handle(WebMessage item)
      {
        try
        {
          this._handler.Handle(item);
        }
        catch (Exception ex)
        {
          this._logger.Error<Exception>(ex);
        }
      }
    }
  }
}
