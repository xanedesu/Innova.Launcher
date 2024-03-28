// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Underline
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Controls;

namespace Innova.Launcher.UI.Controls
{
  [TemplateVisualState(GroupName = "ActivationStates", Name = "Active")]
  [TemplateVisualState(GroupName = "ActivationStates", Name = "Inactive")]
  [TemplateVisualState(GroupName = "ActivationStates", Name = "Error")]
  public class Underline : Control
  {
    public const string ActiveStateName = "Active";
    public const string InactiveStateName = "Inactive";
    public const string ErrorStateName = "Error";
    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(nameof (IsActive), typeof (bool), typeof (Underline), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender, new System.Windows.PropertyChangedCallback(Underline.PropertyChangedCallback)));
    public static readonly DependencyProperty IsErrorProperty = DependencyProperty.Register(nameof (IsError), typeof (bool), typeof (Underline), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender, new System.Windows.PropertyChangedCallback(Underline.PropertyChangedCallback)));
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof (CornerRadius), typeof (CornerRadius), typeof (Underline), (PropertyMetadata) new FrameworkPropertyMetadata((object) new CornerRadius(0.0), FrameworkPropertyMetadataOptions.AffectsRender, (System.Windows.PropertyChangedCallback) null));

    static Underline() => FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof (Underline), (PropertyMetadata) new FrameworkPropertyMetadata((object) typeof (Underline)));

    private static void PropertyChangedCallback(
      DependencyObject dependencyObject,
      DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
    {
      ((Underline) dependencyObject).GotoVisualState(true);
    }

    public bool IsActive
    {
      get => (bool) this.GetValue(Underline.IsActiveProperty);
      set => this.SetValue(Underline.IsActiveProperty, (object) value);
    }

    public bool IsError
    {
      get => (bool) this.GetValue(Underline.IsErrorProperty);
      set => this.SetValue(Underline.IsErrorProperty, (object) value);
    }

    public CornerRadius CornerRadius
    {
      get => (CornerRadius) this.GetValue(Underline.CornerRadiusProperty);
      set => this.SetValue(Underline.CornerRadiusProperty, (object) value);
    }

    public override void OnApplyTemplate()
    {
      base.OnApplyTemplate();
      this.GotoVisualState(false);
    }

    private void GotoVisualState(bool useTransitions) => VisualStateManager.GoToState((FrameworkElement) this, this.SelectStateName(), useTransitions);

    private string SelectStateName()
    {
      if (this.IsError)
        return "Error";
      return !this.IsActive ? "Inactive" : "Active";
    }
  }
}
