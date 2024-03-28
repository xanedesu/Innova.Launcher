// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Views.PushNotificationView
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using Innova.Launcher.Events;
using Innova.Launcher.ViewModels;
using ReactiveUI;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Innova.Launcher.Views
{
  public partial class PushNotificationView : UserControl, IComponentConnector
  {
    internal System.Windows.Media.Animation.BeginStoryboard CloseStoryboard;
    internal Button PrimaryButton;
    internal Button SecondaryButton;
    internal Button CloseButton;
    private bool _contentLoaded;

    public PushNotificationViewModel ViewModel
    {
      get => (PushNotificationViewModel) this.DataContext;
      set => this.DataContext = (object) value;
    }

    public PushNotificationView() => this.InitializeComponent();

    protected override void OnMouseEnter(MouseEventArgs e)
    {
      base.OnMouseEnter(e);
      this.ViewModel.OnMouseEnter();
    }

    private void CloseStoryboardCompleted(object sender, EventArgs e) => MessageBus.Current.SendMessage<PushNotificationClosedEvent>(new PushNotificationClosedEvent(this.ViewModel.Id));

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    public void InitializeComponent()
    {
      if (this._contentLoaded)
        return;
      this._contentLoaded = true;
      Application.LoadComponent((object) this, new Uri("/Innova.Launcher;component/views/pushnotificationview.xaml", UriKind.Relative));
    }

    [DebuggerNonUserCode]
    [GeneratedCode("PresentationBuildTasks", "4.8.1.0")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    void IComponentConnector.Connect(int connectionId, object target)
    {
      switch (connectionId)
      {
        case 1:
          this.CloseStoryboard = (System.Windows.Media.Animation.BeginStoryboard) target;
          break;
        case 2:
          ((Timeline) target).Completed += new EventHandler(this.CloseStoryboardCompleted);
          break;
        case 3:
          this.PrimaryButton = (Button) target;
          break;
        case 4:
          this.SecondaryButton = (Button) target;
          break;
        case 5:
          this.CloseButton = (Button) target;
          break;
        default:
          this._contentLoaded = true;
          break;
      }
    }
  }
}
