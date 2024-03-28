// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.ModernWindow
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Behaviors;
using Innova.Launcher.UI.Behaviors;
using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Media;

namespace Innova.Launcher.UI.Controls
{
  public class ModernWindow : Window
  {
    public static readonly DependencyProperty ShowMinButtonProperty = DependencyProperty.Register(nameof (ShowMinButton), typeof (bool), typeof (ModernWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ShowMaxRestoreButtonProperty = DependencyProperty.Register(nameof (ShowMaxRestoreButton), typeof (bool), typeof (ModernWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ShowCloseButtonProperty = DependencyProperty.Register(nameof (ShowCloseButton), typeof (bool), typeof (ModernWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ShowTitleProperty = DependencyProperty.Register(nameof (ShowTitle), typeof (bool), typeof (ModernWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty ShowTitleBarProperty = DependencyProperty.Register(nameof (ShowTitleBar), typeof (bool), typeof (ModernWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) true, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty OverlapsTitleProperty = DependencyProperty.Register(nameof (OverlapsTitle), typeof (bool), typeof (ModernWindow), (PropertyMetadata) new FrameworkPropertyMetadata((object) false, FrameworkPropertyMetadataOptions.AffectsRender));
    public static readonly DependencyProperty GlowBrushProperty = DependencyProperty.Register(nameof (GlowBrush), typeof (Brush), typeof (ModernWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty NonActiveGlowBrushProperty = DependencyProperty.Register(nameof (NonActiveGlowBrush), typeof (Brush), typeof (ModernWindow), new PropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty IgnoreTaskbarOnMaximizeProperty = DependencyProperty.Register(nameof (IgnoreTaskbarOnMaximize), typeof (bool), typeof (ModernWindow), new PropertyMetadata((object) false));
    public static readonly DependencyProperty ResizeBorderThicknessProperty = DependencyProperty.Register(nameof (ResizeBorderThickness), typeof (Thickness), typeof (ModernWindow), new PropertyMetadata((object) WindowChromeBehavior.GetDefaultResizeBorderThickness()));
    public static readonly DependencyProperty SaveWindowPositionProperty = DependencyProperty.Register(nameof (SaveWindowPosition), typeof (bool), typeof (ModernWindow), new PropertyMetadata((object) false));
    public static readonly DependencyProperty WindowPlacementSettingsProperty = DependencyProperty.Register(nameof (WindowPlacementSettings), typeof (IWindowPlacementSettings), typeof (ModernWindow), new PropertyMetadata((PropertyChangedCallback) null));

    public bool ShowMinButton
    {
      get => (bool) this.GetValue(ModernWindow.ShowMinButtonProperty);
      set => this.SetValue(ModernWindow.ShowMinButtonProperty, (object) value);
    }

    public bool SaveWindowPosition
    {
      get => (bool) this.GetValue(ModernWindow.SaveWindowPositionProperty);
      set => this.SetValue(ModernWindow.SaveWindowPositionProperty, (object) value);
    }

    public IWindowPlacementSettings WindowPlacementSettings
    {
      get => (IWindowPlacementSettings) this.GetValue(ModernWindow.WindowPlacementSettingsProperty);
      set => this.SetValue(ModernWindow.WindowPlacementSettingsProperty, (object) value);
    }

    public bool IgnoreTaskbarOnMaximize
    {
      get => (bool) this.GetValue(ModernWindow.IgnoreTaskbarOnMaximizeProperty);
      set => this.SetValue(ModernWindow.IgnoreTaskbarOnMaximizeProperty, (object) value);
    }

    public Thickness ResizeBorderThickness
    {
      get => (Thickness) this.GetValue(ModernWindow.ResizeBorderThicknessProperty);
      set => this.SetValue(ModernWindow.ResizeBorderThicknessProperty, (object) value);
    }

    public Brush GlowBrush
    {
      get => (Brush) this.GetValue(ModernWindow.GlowBrushProperty);
      set => this.SetValue(ModernWindow.GlowBrushProperty, (object) value);
    }

    public Brush NonActiveGlowBrush
    {
      get => (Brush) this.GetValue(ModernWindow.NonActiveGlowBrushProperty);
      set => this.SetValue(ModernWindow.NonActiveGlowBrushProperty, (object) value);
    }

    public bool ShowMaxRestoreButton
    {
      get => (bool) this.GetValue(ModernWindow.ShowMaxRestoreButtonProperty);
      set => this.SetValue(ModernWindow.ShowMaxRestoreButtonProperty, (object) value);
    }

    public bool ShowCloseButton
    {
      get => (bool) this.GetValue(ModernWindow.ShowCloseButtonProperty);
      set => this.SetValue(ModernWindow.ShowCloseButtonProperty, (object) value);
    }

    public bool ShowTitle
    {
      get => (bool) this.GetValue(ModernWindow.ShowTitleProperty);
      set => this.SetValue(ModernWindow.ShowTitleProperty, (object) value);
    }

    public bool ShowTitleBar
    {
      get => (bool) this.GetValue(ModernWindow.ShowTitleBarProperty);
      set => this.SetValue(ModernWindow.ShowTitleBarProperty, (object) value);
    }

    public bool OverlapsTitle
    {
      get => (bool) this.GetValue(ModernWindow.OverlapsTitleProperty);
      set => this.SetValue(ModernWindow.OverlapsTitleProperty, (object) value);
    }

    public ModernWindow()
    {
      StylizedBehaviorCollection behaviorCollection = new StylizedBehaviorCollection();
      behaviorCollection.Add((Behavior) new BorderlessWindowBehavior());
      behaviorCollection.Add((Behavior) new Innova.Launcher.UI.Behaviors.GlowWindowBehavior());
      behaviorCollection.Add((Behavior) new WindowsSettingBehaviour());
      StylizedBehaviors.SetBehaviors((DependencyObject) this, behaviorCollection);
    }

    public virtual IWindowPlacementSettings GetWindowPlacementSettings() => this.WindowPlacementSettings ?? (IWindowPlacementSettings) new WindowApplicationSettings((Window) this);

    internal T GetPart<T>(string name) where T : class => this.GetTemplateChild(name) as T;

    internal DependencyObject GetPart(string name) => this.GetTemplateChild(name);
  }
}
