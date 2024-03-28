// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.EventToCommand
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;
using System.Windows.Input;

namespace Innova.Launcher.UI.Behaviors
{
  public class EventToCommand : TriggerAction<FrameworkElement>
  {
    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.Register(nameof (CommandParameter), typeof (object), typeof (EventToCommand), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, e) =>
    {
      if (!(s is EventToCommand eventToCommand2) || eventToCommand2.AssociatedObject == null)
        return;
      eventToCommand2.EnableDisableElement();
    })));
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(nameof (Command), typeof (ICommand), typeof (EventToCommand), new PropertyMetadata((object) null, (PropertyChangedCallback) ((s, e) => EventToCommand.OnCommandChanged(s as EventToCommand, e))));
    public static readonly DependencyProperty MustToggleIsEnabledProperty = DependencyProperty.Register(nameof (MustToggleIsEnabled), typeof (bool), typeof (EventToCommand), new PropertyMetadata((object) false, (PropertyChangedCallback) ((s, e) =>
    {
      if (!(s is EventToCommand eventToCommand4) || eventToCommand4.AssociatedObject == null)
        return;
      eventToCommand4.EnableDisableElement();
    })));
    private object _commandParameterValue;
    private bool? _mustToggleValue;

    public ICommand Command
    {
      get => (ICommand) this.GetValue(EventToCommand.CommandProperty);
      set => this.SetValue(EventToCommand.CommandProperty, (object) value);
    }

    public object CommandParameter
    {
      get => this.GetValue(EventToCommand.CommandParameterProperty);
      set => this.SetValue(EventToCommand.CommandParameterProperty, value);
    }

    public object CommandParameterValue
    {
      get => this._commandParameterValue ?? this.CommandParameter;
      set
      {
        this._commandParameterValue = value;
        this.EnableDisableElement();
      }
    }

    public bool MustToggleIsEnabled
    {
      get => (bool) this.GetValue(EventToCommand.MustToggleIsEnabledProperty);
      set => this.SetValue(EventToCommand.MustToggleIsEnabledProperty, (object) value);
    }

    public bool MustToggleIsEnabledValue
    {
      get => this._mustToggleValue.HasValue ? this._mustToggleValue.Value : this.MustToggleIsEnabled;
      set
      {
        this._mustToggleValue = new bool?(value);
        this.EnableDisableElement();
      }
    }

    protected override void OnAttached()
    {
      base.OnAttached();
      this.EnableDisableElement();
    }

    private FrameworkElement GetAssociatedObject() => this.AssociatedObject;

    private ICommand GetCommand() => this.Command;

    public bool PassEventArgsToCommand { get; set; }

    public bool PassSenderAndEventArgsToCommand { get; set; }

    public void Invoke() => this.Invoke((object) null);

    protected override void Invoke(object parameter)
    {
      if (this.AssociatedElementIsDisabled())
        return;
      ICommand command = this.GetCommand();
      object parameter1 = this.CommandParameterValue;
      if (parameter1 == null && this.PassEventArgsToCommand)
        parameter1 = parameter;
      if (parameter1 == null && this.PassSenderAndEventArgsToCommand)
        parameter1 = (object) new SenderEventArgsCommandParameter()
        {
          Sender = this.AssociatedObject,
          Args = (parameter as EventArgs)
        };
      if (command == null || !command.CanExecute(parameter1))
        return;
      command.Execute(parameter1);
    }

    private static void OnCommandChanged(
      EventToCommand element,
      DependencyPropertyChangedEventArgs e)
    {
      if (element == null)
        return;
      if (e.OldValue != null)
        ((ICommand) e.OldValue).CanExecuteChanged -= new EventHandler(element.OnCommandCanExecuteChanged);
      ICommand newValue = (ICommand) e.NewValue;
      if (newValue != null)
        newValue.CanExecuteChanged += new EventHandler(element.OnCommandCanExecuteChanged);
      element.EnableDisableElement();
    }

    private bool AssociatedElementIsDisabled()
    {
      FrameworkElement associatedObject = this.GetAssociatedObject();
      return associatedObject != null && !associatedObject.IsEnabled;
    }

    private void EnableDisableElement()
    {
      FrameworkElement associatedObject = this.GetAssociatedObject();
      if (associatedObject == null)
        return;
      ICommand command = this.GetCommand();
      if (!this.MustToggleIsEnabledValue || command == null)
        return;
      associatedObject.IsEnabled = command.CanExecute(this.CommandParameterValue);
    }

    private void OnCommandCanExecuteChanged(object sender, EventArgs e) => this.EnableDisableElement();
  }
}
