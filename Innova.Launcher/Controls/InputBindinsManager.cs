// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Controls.InputBindingsManager
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Innova.Launcher.Controls
{
  public static class InputBindingsManager
  {
    public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty = DependencyProperty.RegisterAttached("UpdatePropertySourceWhenEnterPressed", typeof (DependencyProperty), typeof (InputBindingsManager), new PropertyMetadata((object) null, new PropertyChangedCallback(InputBindingsManager.OnUpdatePropertySourceWhenEnterPressedPropertyChanged)));

    public static void SetUpdatePropertySourceWhenEnterPressed(
      DependencyObject dp,
      DependencyProperty value)
    {
      dp.SetValue(InputBindingsManager.UpdatePropertySourceWhenEnterPressedProperty, (object) value);
    }

    public static DependencyProperty GetUpdatePropertySourceWhenEnterPressed(DependencyObject dp) => (DependencyProperty) dp.GetValue(InputBindingsManager.UpdatePropertySourceWhenEnterPressedProperty);

    private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged(
      DependencyObject dp,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(dp is UIElement uiElement))
        return;
      if (e.OldValue != null)
        uiElement.PreviewKeyDown -= new KeyEventHandler(InputBindingsManager.HandlePreviewKeyDown);
      if (e.NewValue == null)
        return;
      uiElement.PreviewKeyDown += new KeyEventHandler(InputBindingsManager.HandlePreviewKeyDown);
    }

    private static void HandlePreviewKeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key != Key.Return)
        return;
      InputBindingsManager.DoUpdateSource(e.Source);
    }

    private static void DoUpdateSource(object source)
    {
      DependencyProperty whenEnterPressed = InputBindingsManager.GetUpdatePropertySourceWhenEnterPressed(source as DependencyObject);
      if (whenEnterPressed == null || !(source is UIElement target))
        return;
      BindingOperations.GetBindingExpression((DependencyObject) target, whenEnterPressed)?.UpdateSource();
    }
  }
}
