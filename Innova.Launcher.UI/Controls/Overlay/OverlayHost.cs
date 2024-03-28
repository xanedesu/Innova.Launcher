// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Overlay.OverlayHost
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace Innova.Launcher.UI.Controls.Overlay
{
  [TemplatePart(Name = "PART_Popup", Type = typeof (Popup))]
  [TemplatePart(Name = "PART_PopupContentElement", Type = typeof (ContentControl))]
  [TemplatePart(Name = "PART_ControlPopup", Type = typeof (Popup))]
  [TemplatePart(Name = "PART_Title", Type = typeof (ContentControl))]
  [TemplatePart(Name = "PART_ContentOverlayControl", Type = typeof (ContentControl))]
  [TemplatePart(Name = "PART_ContentCoverGrid", Type = typeof (Grid))]
  [TemplateVisualState(GroupName = "PopupStates", Name = "Open")]
  [TemplateVisualState(GroupName = "PopupStates", Name = "Closed")]
  public class OverlayHost : ContentControl
  {
    public const string PopupControlPartName = "PART_ControlPopup";
    public const string PopupControlContentPartName = "PART_ContentOverlayControl";
    public const string PopupPartName = "PART_Popup";
    public const string PopupContentPartName = "PART_PopupContentElement";
    public const string ContentCoverGridName = "PART_ContentCoverGrid";
    public const string Title = "PART_Title";
    public const string OpenStateName = "Open";
    public const string ClosedStateName = "Closed";
    public static RoutedCommand OpenOverlayCommand = new RoutedCommand();
    public static RoutedCommand CloseOverlayCommand = new RoutedCommand();
    private static readonly HashSet<OverlayHost> LoadedInstances = new HashSet<OverlayHost>();
    private readonly ManualResetEvent _asyncShowWaitHandle = new ManualResetEvent(false);
    private EventHandler<OverlayOpenedEventArgs> _asyncShowOpenedEventHandler;
    private EventHandler<OverlayClosingEventArgs> _asyncShowClosingEventHandler;
    private Popup _popup;
    private ContentControl _popupContentControl;
    private Grid _contentCoverGrid;
    private OverlaySession _session;
    private EventHandler<OverlayOpenedEventArgs> _attachedOverlayOpenedEventHandler;
    private EventHandler<OverlayClosingEventArgs> _attachedOverlayClosingEventHandler;
    private object _closeOverlayExecutionParameter;
    private IInputElement _restoreFocusOverlayClose;
    private IInputElement _restoreFocusWindowReactivation;
    private Action _closeCleanUp = (Action) (() => { });
    public static readonly DependencyProperty ControlOverlapsProperty = DependencyProperty.Register(nameof (ControlOverlaps), typeof (bool), typeof (OverlayHost), new PropertyMetadata((object) false));
    public static readonly DependencyProperty IdentifierProperty = DependencyProperty.Register(nameof (Identifier), typeof (object), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty IsOpenProperty = DependencyProperty.Register(nameof (IsOpen), typeof (bool), typeof (OverlayHost), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OverlayHost.IsOpenPropertyChangedCallback)));
    public static readonly DependencyProperty TitleContentProperty = DependencyProperty.Register(nameof (TitleContent), typeof (object), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OverlayContentProperty = DependencyProperty.Register(nameof (OverlayContent), typeof (object), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OverlayContentTemplateProperty = DependencyProperty.Register(nameof (OverlayContentTemplate), typeof (DataTemplate), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OverlayContentTemplateSelectorProperty = DependencyProperty.Register(nameof (OverlayContentTemplateSelector), typeof (DataTemplateSelector), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OverlayContentStringFormatProperty = DependencyProperty.Register(nameof (OverlayContentStringFormat), typeof (string), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty ContentVerticalOffsetProperty = DependencyProperty.Register(nameof (ContentVerticalOffset), typeof (double), typeof (OverlayHost), new PropertyMetadata((object) 0.0));
    public static readonly DependencyProperty OverlayMarginProperty = DependencyProperty.Register(nameof (OverlayMargin), typeof (Thickness), typeof (OverlayHost), new PropertyMetadata((object) new Thickness()));
    public static readonly DependencyProperty OpenOverlayCommandDataContextSourceProperty = DependencyProperty.Register(nameof (OpenOverlayCommandDataContextSource), typeof (OverlayHostOpenOverlayCommandDataContextSource), typeof (OverlayHost), new PropertyMetadata((object) OverlayHostOpenOverlayCommandDataContextSource.SenderElement));
    public static readonly DependencyProperty CloseOnClickAwayProperty = DependencyProperty.Register(nameof (CloseOnClickAway), typeof (bool), typeof (OverlayHost), new PropertyMetadata((object) false));
    public static readonly DependencyProperty CloseOnClickAwayParameterProperty = DependencyProperty.Register(nameof (CloseOnClickAwayParameter), typeof (object), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty PopupStyleProperty = DependencyProperty.Register(nameof (PopupStyle), typeof (Style), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly RoutedEvent OverlayOpenedEvent = EventManager.RegisterRoutedEvent("OverlayOpened", RoutingStrategy.Bubble, typeof (EventHandler<OverlayOpenedEventArgs>), typeof (OverlayHost));
    public static readonly DependencyProperty OverlayOpenedAttachedProperty = DependencyProperty.RegisterAttached("OverlayOpenedAttached", typeof (EventHandler<OverlayOpenedEventArgs>), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OverlayOpenedCallbackProperty = DependencyProperty.Register(nameof (OverlayOpenedCallback), typeof (EventHandler<OverlayOpenedEventArgs>), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly RoutedEvent OverlayClosingEvent = EventManager.RegisterRoutedEvent("OverlayClosing", RoutingStrategy.Bubble, typeof (EventHandler<OverlayClosingEventArgs>), typeof (OverlayHost));
    public static readonly DependencyProperty OverlayClosingAttachedProperty = DependencyProperty.RegisterAttached("OverlayClosingAttached", typeof (EventHandler<OverlayClosingEventArgs>), typeof (OverlayHost), new PropertyMetadata((object) null));
    public static readonly DependencyProperty OverlayClosingCallbackProperty = DependencyProperty.Register(nameof (OverlayClosingCallback), typeof (EventHandler<OverlayClosingEventArgs>), typeof (OverlayHost), new PropertyMetadata((object) null));

    static OverlayHost() => FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (OverlayHost), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (OverlayHost)));

    public static async Task<object> Show(object content) => await OverlayHost.Show(content, (EventHandler<OverlayOpenedEventArgs>) null, (EventHandler<OverlayClosingEventArgs>) null);

    public static async Task<object> Show(
      object content,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler)
    {
      return await OverlayHost.Show(content, (object) null, openedEventHandler, (EventHandler<OverlayClosingEventArgs>) null);
    }

    public static async Task<object> Show(
      object content,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      return await OverlayHost.Show(content, (object) null, (EventHandler<OverlayOpenedEventArgs>) null, closingEventHandler);
    }

    public static async Task<object> Show(
      object content,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      return await OverlayHost.Show(content, (object) null, openedEventHandler, closingEventHandler);
    }

    public static async Task<object> Show(object content, object OverlayIdentifier) => await OverlayHost.Show(content, OverlayIdentifier, (EventHandler<OverlayOpenedEventArgs>) null, (EventHandler<OverlayClosingEventArgs>) null);

    public static Task<object> Show(
      object content,
      object OverlayIdentifier,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler)
    {
      return OverlayHost.Show(content, OverlayIdentifier, openedEventHandler, (EventHandler<OverlayClosingEventArgs>) null);
    }

    public static Task<object> Show(
      object content,
      object OverlayIdentifier,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      return OverlayHost.Show(content, OverlayIdentifier, (EventHandler<OverlayOpenedEventArgs>) null, closingEventHandler);
    }

    public static async Task<object> Show(
      object content,
      object OverlayIdentifier,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      if (content == null)
        throw new ArgumentNullException(nameof (content));
      if (OverlayHost.LoadedInstances.Count == 0)
        throw new InvalidOperationException("No loaded OverlayHost instances.");
      OverlayHost.LoadedInstances.First<OverlayHost>().Dispatcher.VerifyAccess();
      List<OverlayHost> list = OverlayHost.LoadedInstances.Where<OverlayHost>((Func<OverlayHost, bool>) (dh => OverlayIdentifier == null || object.Equals(dh.Identifier, OverlayIdentifier))).ToList<OverlayHost>();
      if (list.Count == 0)
        throw new InvalidOperationException("No loaded OverlayHost have an Identifier property matching OverlayIndetifier argument.");
      if (list.Count > 1)
        throw new InvalidOperationException("Multiple viable OverlayHosts.  Specify a unique Identifier on each OverlayHost, especially where multiple Windows are a concern.");
      return await list[0].ShowInternal(content, openedEventHandler, closingEventHandler);
    }

    internal async Task<object> ShowInternal(
      object content,
      EventHandler<OverlayOpenedEventArgs> openedEventHandler,
      EventHandler<OverlayClosingEventArgs> closingEventHandler)
    {
      OverlayHost overlayHost = this;
      if (overlayHost.IsOpen)
        throw new InvalidOperationException("OverlayHost is already open.");
      overlayHost.AssertTargetableContent();
      overlayHost.OverlayContent = content;
      overlayHost._asyncShowOpenedEventHandler = openedEventHandler;
      overlayHost._asyncShowClosingEventHandler = closingEventHandler;
      overlayHost.SetCurrentValue(OverlayHost.IsOpenProperty, (object) true);
      // ISSUE: reference to a compiler-generated method
      Task task = new Task(new Action(overlayHost.\u003CShowInternal\u003Eb__33_0));
      task.Start();
      await task;
      overlayHost._asyncShowOpenedEventHandler = (EventHandler<OverlayOpenedEventArgs>) null;
      overlayHost._asyncShowClosingEventHandler = (EventHandler<OverlayClosingEventArgs>) null;
      return overlayHost._closeOverlayExecutionParameter;
    }

    public OverlayHost()
    {
      this.Loaded += new RoutedEventHandler(this.OnLoaded);
      this.Unloaded += new RoutedEventHandler(this.OnUnloaded);
      this.CommandBindings.Add(new CommandBinding((ICommand) OverlayHost.CloseOverlayCommand, new ExecutedRoutedEventHandler(this.CloseOverlayHandler), new CanExecuteRoutedEventHandler(this.CloseOverlayCanExecute)));
      this.CommandBindings.Add(new CommandBinding((ICommand) OverlayHost.OpenOverlayCommand, new ExecutedRoutedEventHandler(this.OpenOverlayHandler)));
    }

    public bool ControlOverlaps
    {
      get => (bool) this.GetValue(OverlayHost.ControlOverlapsProperty);
      set => this.SetValue(OverlayHost.ControlOverlapsProperty, (object) value);
    }

    public object Identifier
    {
      get => this.GetValue(OverlayHost.IdentifierProperty);
      set => this.SetValue(OverlayHost.IdentifierProperty, value);
    }

    private static void IsOpenPropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      OverlayHost OverlayHost = (OverlayHost) dependencyObject;
      if (OverlayHost._popupContentControl != null)
        ValidationAssist.SetSuppress((DependencyObject) OverlayHost._popupContentControl, !OverlayHost.IsOpen);
      VisualStateManager.GoToState((FrameworkElement) OverlayHost, OverlayHost.SelectState(), !TransitionAssist.GetDisableTransitions((DependencyObject) OverlayHost));
      if (OverlayHost.IsOpen)
      {
        OverlayHost.WatchWindowActivation(OverlayHost);
        OverlayHost._asyncShowWaitHandle.Reset();
        OverlayHost._session = new OverlaySession(OverlayHost);
        Window window = Window.GetWindow((DependencyObject) OverlayHost);
        OverlayHost._restoreFocusOverlayClose = window != null ? FocusManager.GetFocusedElement((DependencyObject) window) : (IInputElement) null;
        OverlayOpenedEventArgs overlayOpenedEventArgs = new OverlayOpenedEventArgs(OverlayHost._session, OverlayHost.OverlayOpenedEvent);
        OverlayHost.OnOverlayOpened(overlayOpenedEventArgs);
        EventHandler<OverlayOpenedEventArgs> openedEventHandler1 = OverlayHost._attachedOverlayOpenedEventHandler;
        if (openedEventHandler1 != null)
          openedEventHandler1((object) OverlayHost, overlayOpenedEventArgs);
        EventHandler<OverlayOpenedEventArgs> overlayOpenedCallback = OverlayHost.OverlayOpenedCallback;
        if (overlayOpenedCallback != null)
          overlayOpenedCallback((object) OverlayHost, overlayOpenedEventArgs);
        EventHandler<OverlayOpenedEventArgs> openedEventHandler2 = OverlayHost._asyncShowOpenedEventHandler;
        if (openedEventHandler2 != null)
          openedEventHandler2((object) OverlayHost, overlayOpenedEventArgs);
        OverlayHost.Dispatcher.BeginInvoke(DispatcherPriority.Background, (Delegate) (() =>
        {
          CommandManager.InvalidateRequerySuggested();
          UIElement child = OverlayHost.FocusPopup();
          Task.Delay(300).ContinueWith<DispatcherOperation>((Func<Task, DispatcherOperation>) (t => child.Dispatcher.BeginInvoke((Delegate) (() => child.InvalidateVisual()), Array.Empty<object>())));
        }));
      }
      else
      {
        OverlayHost._asyncShowWaitHandle.Set();
        OverlayHost._attachedOverlayClosingEventHandler = (EventHandler<OverlayClosingEventArgs>) null;
        OverlayHost._session.IsEnded = true;
        OverlayHost._session = (OverlaySession) null;
        OverlayHost._closeCleanUp();
        OverlayHost.Dispatcher.InvokeAsync<bool?>((Func<bool?>) (() => OverlayHost._restoreFocusOverlayClose?.Focus()), DispatcherPriority.Input);
      }
    }

    public bool IsOpen
    {
      get => (bool) this.GetValue(OverlayHost.IsOpenProperty);
      set => this.SetValue(OverlayHost.IsOpenProperty, (object) value);
    }

    public object TitleContent
    {
      get => this.GetValue(OverlayHost.TitleContentProperty);
      set => this.SetValue(OverlayHost.TitleContentProperty, value);
    }

    public object OverlayContent
    {
      get => this.GetValue(OverlayHost.OverlayContentProperty);
      set => this.SetValue(OverlayHost.OverlayContentProperty, value);
    }

    public DataTemplate OverlayContentTemplate
    {
      get => (DataTemplate) this.GetValue(OverlayHost.OverlayContentTemplateProperty);
      set => this.SetValue(OverlayHost.OverlayContentTemplateProperty, (object) value);
    }

    public DataTemplateSelector OverlayContentTemplateSelector
    {
      get => (DataTemplateSelector) this.GetValue(OverlayHost.OverlayContentTemplateSelectorProperty);
      set => this.SetValue(OverlayHost.OverlayContentTemplateSelectorProperty, (object) value);
    }

    public string OverlayContentStringFormat
    {
      get => (string) this.GetValue(OverlayHost.OverlayContentStringFormatProperty);
      set => this.SetValue(OverlayHost.OverlayContentStringFormatProperty, (object) value);
    }

    public double ContentVerticalOffset
    {
      get => (double) this.GetValue(OverlayHost.ContentVerticalOffsetProperty);
      set => this.SetValue(OverlayHost.ContentVerticalOffsetProperty, (object) value);
    }

    public Thickness OverlayMargin
    {
      get => (Thickness) this.GetValue(OverlayHost.OverlayMarginProperty);
      set => this.SetValue(OverlayHost.OverlayMarginProperty, (object) value);
    }

    public OverlayHostOpenOverlayCommandDataContextSource OpenOverlayCommandDataContextSource
    {
      get => (OverlayHostOpenOverlayCommandDataContextSource) this.GetValue(OverlayHost.OpenOverlayCommandDataContextSourceProperty);
      set => this.SetValue(OverlayHost.OpenOverlayCommandDataContextSourceProperty, (object) value);
    }

    public bool CloseOnClickAway
    {
      get => (bool) this.GetValue(OverlayHost.CloseOnClickAwayProperty);
      set => this.SetValue(OverlayHost.CloseOnClickAwayProperty, (object) value);
    }

    public object CloseOnClickAwayParameter
    {
      get => this.GetValue(OverlayHost.CloseOnClickAwayParameterProperty);
      set => this.SetValue(OverlayHost.CloseOnClickAwayParameterProperty, value);
    }

    public Style PopupStyle
    {
      get => (Style) this.GetValue(OverlayHost.PopupStyleProperty);
      set => this.SetValue(OverlayHost.PopupStyleProperty, (object) value);
    }

    public override void OnApplyTemplate()
    {
      if (this._contentCoverGrid != null)
        this._contentCoverGrid.MouseLeftButtonUp -= new MouseButtonEventHandler(this.ContentCoverGridOnMouseLeftButtonUp);
      this._popup = this.GetTemplateChild("PART_Popup") as Popup;
      this._popupContentControl = this.GetTemplateChild("PART_PopupContentElement") as ContentControl;
      this._contentCoverGrid = this.GetTemplateChild("PART_ContentCoverGrid") as Grid;
      if (this._contentCoverGrid != null)
        this._contentCoverGrid.MouseLeftButtonUp += new MouseButtonEventHandler(this.ContentCoverGridOnMouseLeftButtonUp);
      VisualStateManager.GoToState((FrameworkElement) this, this.SelectState(), false);
      base.OnApplyTemplate();
    }

    public event EventHandler<OverlayOpenedEventArgs> OverlayOpened
    {
      add => this.AddHandler(OverlayHost.OverlayOpenedEvent, (Delegate) value);
      remove => this.RemoveHandler(OverlayHost.OverlayOpenedEvent, (Delegate) value);
    }

    public static void SetOverlayOpenedAttached(
      DependencyObject element,
      EventHandler<OverlayOpenedEventArgs> value)
    {
      element.SetValue(OverlayHost.OverlayOpenedAttachedProperty, (object) value);
    }

    public static EventHandler<OverlayOpenedEventArgs> GetOverlayOpenedAttached(
      DependencyObject element)
    {
      return (EventHandler<OverlayOpenedEventArgs>) element.GetValue(OverlayHost.OverlayOpenedAttachedProperty);
    }

    public EventHandler<OverlayOpenedEventArgs> OverlayOpenedCallback
    {
      get => (EventHandler<OverlayOpenedEventArgs>) this.GetValue(OverlayHost.OverlayOpenedCallbackProperty);
      set => this.SetValue(OverlayHost.OverlayOpenedCallbackProperty, (object) value);
    }

    protected void OnOverlayOpened(OverlayOpenedEventArgs eventArgs) => this.RaiseEvent((RoutedEventArgs) eventArgs);

    public event EventHandler<OverlayClosingEventArgs> OverlayClosing
    {
      add => this.AddHandler(OverlayHost.OverlayClosingEvent, (Delegate) value);
      remove => this.RemoveHandler(OverlayHost.OverlayClosingEvent, (Delegate) value);
    }

    public static void SetOverlayClosingAttached(
      DependencyObject element,
      EventHandler<OverlayClosingEventArgs> value)
    {
      element.SetValue(OverlayHost.OverlayClosingAttachedProperty, (object) value);
    }

    public static EventHandler<OverlayClosingEventArgs> GetOverlayClosingAttached(
      DependencyObject element)
    {
      return (EventHandler<OverlayClosingEventArgs>) element.GetValue(OverlayHost.OverlayClosingAttachedProperty);
    }

    public EventHandler<OverlayClosingEventArgs> OverlayClosingCallback
    {
      get => (EventHandler<OverlayClosingEventArgs>) this.GetValue(OverlayHost.OverlayClosingCallbackProperty);
      set => this.SetValue(OverlayHost.OverlayClosingCallbackProperty, (object) value);
    }

    protected void OnOverlayClosing(OverlayClosingEventArgs eventArgs) => this.RaiseEvent((RoutedEventArgs) eventArgs);

    public static CustomPopupPlacementCallback OverlayPopupPlacementCallback { get; } = new CustomPopupPlacementCallback(OverlayHost.OverlayPopupPlacementCallbackBody);

    public static CustomPopupPlacement[] OverlayPopupPlacementCallbackBody(
      Size popupSize,
      Size targetSize,
      System.Windows.Point offset)
    {
      return new CustomPopupPlacement[1]
      {
        new CustomPopupPlacement(new System.Windows.Point(0.0, 0.0), PopupPrimaryAxis.Vertical)
      };
    }

    public static CustomPopupPlacementCallback OverlayControlPopupPlacementCallback { get; } = new CustomPopupPlacementCallback(OverlayHost.OverlayControlPopupPlacementCallbackBody);

    public static CustomPopupPlacement[] OverlayControlPopupPlacementCallbackBody(
      Size popupSize,
      Size targetSize,
      System.Windows.Point offset)
    {
      return new CustomPopupPlacement[1]
      {
        new CustomPopupPlacement(new System.Windows.Point(0.0, targetSize.Height - popupSize.Height), PopupPrimaryAxis.Vertical)
      };
    }

    internal void AssertTargetableContent()
    {
      if (BindingOperations.GetBindingExpression((DependencyObject) this, OverlayHost.OverlayContentProperty) != null)
        throw new InvalidOperationException("Content cannot be passed to a Overlay via the OpenOverlay if OverlayContent already has a binding.");
    }

    internal void Close(object parameter)
    {
      try
      {
        Mouse.OverrideCursor = Cursors.Wait;
        OverlayClosingEventArgs closingEventArgs = new OverlayClosingEventArgs(this._session, parameter, OverlayHost.OverlayClosingEvent);
        this._session.IsEnded = true;
        this.OnOverlayClosing(closingEventArgs);
        EventHandler<OverlayClosingEventArgs> closingEventHandler1 = this._attachedOverlayClosingEventHandler;
        if (closingEventHandler1 != null)
          closingEventHandler1((object) this, closingEventArgs);
        EventHandler<OverlayClosingEventArgs> overlayClosingCallback = this.OverlayClosingCallback;
        if (overlayClosingCallback != null)
          overlayClosingCallback((object) this, closingEventArgs);
        EventHandler<OverlayClosingEventArgs> closingEventHandler2 = this._asyncShowClosingEventHandler;
        if (closingEventHandler2 != null)
          closingEventHandler2((object) this, closingEventArgs);
        if (!closingEventArgs.IsCancelled)
          this.SetCurrentValue(OverlayHost.IsOpenProperty, (object) false);
        else
          this._session.IsEnded = false;
        this._closeOverlayExecutionParameter = parameter;
      }
      finally
      {
        Mouse.OverrideCursor = (Cursor) null;
      }
    }

    internal static void CloseAll()
    {
      foreach (OverlayHost loadedInstance in OverlayHost.LoadedInstances)
      {
        try
        {
          OverlayHost.CloseOverlayCommand.Execute((object) null, (IInputElement) loadedInstance);
        }
        catch (Exception ex)
        {
        }
      }
    }

    internal UIElement FocusPopup()
    {
      UIElement child = this._popup?.Child;
      if (child == null)
        return (UIElement) null;
      CommandManager.InvalidateRequerySuggested();
      UIElement focusable = child.VisualDepthFirstTraversal().OfType<UIElement>().FirstOrDefault<UIElement>((Func<UIElement, bool>) (ui => ui.Focusable && ui.IsVisible));
      focusable?.Dispatcher.InvokeAsync((Action) (() =>
      {
        if (!focusable.Focus())
          return;
        focusable.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
      }), DispatcherPriority.Background);
      return child;
    }

    protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
    {
      Window window = Window.GetWindow((DependencyObject) this);
      if (window != null && !window.IsActive)
        window.Activate();
      base.OnPreviewMouseDown(e);
    }

    private void ContentCoverGridOnMouseLeftButtonUp(
      object sender,
      MouseButtonEventArgs mouseButtonEventArgs)
    {
      if (!this.CloseOnClickAway)
        return;
      this.Close(this.CloseOnClickAwayParameter);
    }

    private void OpenOverlayHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
    {
      if (executedRoutedEventArgs.Handled)
        return;
      if (executedRoutedEventArgs.OriginalSource is DependencyObject originalSource1)
      {
        this._attachedOverlayOpenedEventHandler = OverlayHost.GetOverlayOpenedAttached(originalSource1);
        this._attachedOverlayClosingEventHandler = OverlayHost.GetOverlayClosingAttached(originalSource1);
      }
      if (executedRoutedEventArgs.Parameter != null)
      {
        this.AssertTargetableContent();
        if (this._popupContentControl != null)
        {
          switch (this.OpenOverlayCommandDataContextSource)
          {
            case OverlayHostOpenOverlayCommandDataContextSource.SenderElement:
              this._popupContentControl.DataContext = executedRoutedEventArgs.OriginalSource is FrameworkElement originalSource2 ? originalSource2.DataContext : (object) null;
              break;
            case OverlayHostOpenOverlayCommandDataContextSource.OverlayHostInstance:
              this._popupContentControl.DataContext = this.DataContext;
              break;
            case OverlayHostOpenOverlayCommandDataContextSource.None:
              this._popupContentControl.DataContext = (object) null;
              break;
            default:
              throw new ArgumentOutOfRangeException();
          }
        }
        this.OverlayContent = executedRoutedEventArgs.Parameter;
      }
      this.SetCurrentValue(OverlayHost.IsOpenProperty, (object) true);
      executedRoutedEventArgs.Handled = true;
    }

    private void CloseOverlayCanExecute(
      object sender,
      CanExecuteRoutedEventArgs canExecuteRoutedEventArgs)
    {
      canExecuteRoutedEventArgs.CanExecute = this._session != null;
    }

    private void CloseOverlayHandler(object sender, ExecutedRoutedEventArgs executedRoutedEventArgs)
    {
      if (executedRoutedEventArgs.Handled)
        return;
      this.Close(executedRoutedEventArgs.Parameter);
      executedRoutedEventArgs.Handled = true;
    }

    private string SelectState() => !this.IsOpen ? "Closed" : "Open";

    private void OnUnloaded(object sender, RoutedEventArgs routedEventArgs)
    {
      OverlayHost.LoadedInstances.Remove(this);
      this.SetCurrentValue(OverlayHost.IsOpenProperty, (object) false);
    }

    private void OnLoaded(object sender, RoutedEventArgs routedEventArgs) => OverlayHost.LoadedInstances.Add(this);

    private static void WatchWindowActivation(OverlayHost OverlayHost)
    {
      Window window = Window.GetWindow((DependencyObject) OverlayHost);
      if (window != null)
      {
        window.Activated += new EventHandler(OverlayHost.WindowOnActivated);
        window.Deactivated += new EventHandler(OverlayHost.WindowOnDeactivated);
        OverlayHost._closeCleanUp = (Action) (() =>
        {
          window.Activated -= new EventHandler(OverlayHost.WindowOnActivated);
          window.Deactivated -= new EventHandler(OverlayHost.WindowOnDeactivated);
        });
      }
      else
        OverlayHost._closeCleanUp = (Action) (() => { });
    }

    private void WindowOnDeactivated(object sender, EventArgs eventArgs) => this._restoreFocusWindowReactivation = this._popup != null ? FocusManager.GetFocusedElement((DependencyObject) sender) : (IInputElement) null;

    private void WindowOnActivated(object sender, EventArgs eventArgs)
    {
      if (this._restoreFocusWindowReactivation == null)
        return;
      this.Dispatcher.BeginInvoke((Delegate) (() => Keyboard.Focus(this._restoreFocusWindowReactivation)), Array.Empty<object>());
    }
  }
}
