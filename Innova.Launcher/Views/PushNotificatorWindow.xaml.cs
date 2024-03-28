// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Views.PushNotificatorWindow
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Events;
using Innova.Launcher.ViewModels;
using Microsoft.Win32;
using ReactiveUI;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;

namespace Innova.Launcher.Views
{
  public partial class PushNotificatorWindow : Window, IComponentConnector
  {
    internal PushNotificationView PushNotification;
    private bool _contentLoaded;

    public PushNotificationViewModel ViewModel
    {
      get => (PushNotificationViewModel) this.DataContext;
      set => this.DataContext = (object) value;
    }

    public PushNotificatorWindow()
    {
      this.InitializeComponent();
      if (this.Owner != null)
        this.Owner.Unloaded += new RoutedEventHandler(this.OwnerUnloaded);
      this.Closing += new CancelEventHandler(this.PushNotificatorWindowClosing);
      SystemEvents.DisplaySettingsChanged += new EventHandler(this.DisplaySettingsChanged);
      MessageBus.Current.Listen<PushNotificationClosedEvent>().Where<PushNotificationClosedEvent>((Func<PushNotificationClosedEvent, bool>) (v => v.Id == this.ViewModel.Id)).Subscribe<PushNotificationClosedEvent>((Action<PushNotificationClosedEvent>) (_ => this.Close()));
    }

    private void PushNotificatorWindowClosing(object sender, CancelEventArgs e) => e.Cancel = true;

    public new void Close()
    {
      this.Closing -= new CancelEventHandler(this.PushNotificatorWindowClosing);
      SystemEvents.DisplaySettingsChanged -= new EventHandler(this.DisplaySettingsChanged);
      base.Close();
    }

    private void PushNotificationLoaded(object sender, RoutedEventArgs e)
    {
      this.UpdateLayout();
      this.RecomputeLayout();
    }

    private void RecomputeLayout() => this.Dispatcher.Invoke((Action) (() =>
    {
      if (System.Windows.Application.Current.MainWindow == null)
        return;
      Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
      Matrix? transformFromDevice = PresentationSource.FromVisual((Visual) System.Windows.Application.Current.MainWindow)?.CompositionTarget?.TransformFromDevice;
      if (!transformFromDevice.HasValue)
        return;
      this.PushNotification.Measure(new System.Windows.Size(double.PositiveInfinity, double.PositiveInfinity));
      System.Windows.Point point = transformFromDevice.Value.Transform(new System.Windows.Point((double) workingArea.Right, (double) workingArea.Bottom));
      System.Windows.Size desiredSize = this.PushNotification.DesiredSize;
      this.Left = point.X - desiredSize.Width - 25.0;
      this.Top = point.Y - desiredSize.Height - 25.0;
    }), DispatcherPriority.Render);

    private void OwnerUnloaded(object sender, RoutedEventArgs e) => this.Close();

    private void DisplaySettingsChanged(object sender, EventArgs e)
    {
      if (Innova.Launcher.UI.Behaviors.WindowPos.GetIsLocked((DependencyObject) this))
      {
        Innova.Launcher.UI.Behaviors.WindowPos.SetIsLocked((DependencyObject) this, false);
        this.RecomputeLayout();
        Innova.Launcher.UI.Behaviors.WindowPos.SetIsLocked((DependencyObject) this, true);
      }
      else
        this.RecomputeLayout();
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      System.Windows.Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/views/pushnotificatorwindow.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    internal Delegate _CreateDelegate(System.Type delegateType, string handler) => Delegate.CreateDelegate(delegateType, (object) this, handler);

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      if (connectionId == 1)
        this.PushNotification = (PushNotificationView) target;
      else
        this._contentLoaded = true;
    }
  }
}
