// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Controls.Loader
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System.Windows;
using System.Windows.Controls;

namespace Innova.Launcher.UI.Controls
{
  public class Loader : Control
  {
    public static readonly DependencyProperty LoaderTypeProperty = DependencyProperty.Register(nameof (LoaderType), typeof (LoaderType), typeof (Loader), (PropertyMetadata) new UIPropertyMetadata((object) LoaderType.Large));

    public LoaderType LoaderType
    {
      get => (LoaderType) this.GetValue(Loader.LoaderTypeProperty);
      set => this.SetValue(Loader.LoaderTypeProperty, (object) value);
    }

    public Loader() => this.OverridesDefaultStyle = true;
  }
}
