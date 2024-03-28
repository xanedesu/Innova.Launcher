// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.StylizedBehaviors
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using Microsoft.Xaml.Behaviors;
using System;
using System.Windows;

namespace Innova.Launcher.UI.Behaviors
{
  public class StylizedBehaviors
  {
    public static readonly DependencyProperty BehaviorsProperty = DependencyProperty.RegisterAttached("Behaviors", typeof (StylizedBehaviorCollection), typeof (StylizedBehaviors), (PropertyMetadata) new FrameworkPropertyMetadata((object) null, new PropertyChangedCallback(StylizedBehaviors.OnPropertyChanged)));
    private static readonly DependencyProperty OriginalBehaviorProperty = DependencyProperty.RegisterAttached("OriginalBehavior", typeof (Behavior), typeof (StylizedBehaviors), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public static StylizedBehaviorCollection GetBehaviors(DependencyObject uie) => (StylizedBehaviorCollection) uie.GetValue(StylizedBehaviors.BehaviorsProperty);

    public static void SetBehaviors(DependencyObject uie, StylizedBehaviorCollection value) => uie.SetValue(StylizedBehaviors.BehaviorsProperty, (object) value);

    private static void OnPropertyChanged(
      DependencyObject dpo,
      DependencyPropertyChangedEventArgs e)
    {
      if (!(dpo is FrameworkElement frameworkElement))
        return;
      StylizedBehaviorCollection newValue = e.NewValue as StylizedBehaviorCollection;
      StylizedBehaviorCollection oldValue = e.OldValue as StylizedBehaviorCollection;
      if (newValue == oldValue)
        return;
      BehaviorCollection behaviors = Interaction.GetBehaviors((DependencyObject) frameworkElement);
      frameworkElement.Unloaded -= new RoutedEventHandler(StylizedBehaviors.FrameworkElementUnloaded);
      if (oldValue != null)
      {
        using (FreezableCollection<Behavior>.Enumerator enumerator = oldValue.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Behavior current = enumerator.Current;
            int indexOf = StylizedBehaviors.GetIndexOf(behaviors, current);
            if (indexOf >= 0)
              behaviors.RemoveAt(indexOf);
          }
        }
      }
      if (newValue != null)
      {
        using (FreezableCollection<Behavior>.Enumerator enumerator = newValue.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Behavior current = enumerator.Current;
            if (StylizedBehaviors.GetIndexOf(behaviors, current) < 0)
            {
              Behavior behavior = (Behavior) current.Clone();
              StylizedBehaviors.SetOriginalBehavior((DependencyObject) behavior, current);
              behaviors.Add(behavior);
            }
          }
        }
      }
      if (behaviors.Count > 0)
        frameworkElement.Unloaded += new RoutedEventHandler(StylizedBehaviors.FrameworkElementUnloaded);
      frameworkElement.Dispatcher.ShutdownStarted += new EventHandler(StylizedBehaviors.Dispatcher_ShutdownStarted);
    }

    private static void Dispatcher_ShutdownStarted(object sender, EventArgs e)
    {
    }

    private static void FrameworkElementUnloaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      using (FreezableCollection<Behavior>.Enumerator enumerator = Interaction.GetBehaviors((DependencyObject) frameworkElement).GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Detach();
      }
      frameworkElement.Loaded += new RoutedEventHandler(StylizedBehaviors.FrameworkElementLoaded);
    }

    private static void FrameworkElementLoaded(object sender, RoutedEventArgs e)
    {
      if (!(sender is FrameworkElement frameworkElement))
        return;
      frameworkElement.Loaded -= new RoutedEventHandler(StylizedBehaviors.FrameworkElementLoaded);
      using (FreezableCollection<Behavior>.Enumerator enumerator = Interaction.GetBehaviors((DependencyObject) frameworkElement).GetEnumerator())
      {
        while (enumerator.MoveNext())
          enumerator.Current.Attach((DependencyObject) frameworkElement);
      }
    }

    private static int GetIndexOf(BehaviorCollection itemBehaviors, Behavior behavior)
    {
      int indexOf = -1;
      Behavior originalBehavior1 = StylizedBehaviors.GetOriginalBehavior((DependencyObject) behavior);
      for (int index = 0; index < itemBehaviors.Count; ++index)
      {
        Behavior itemBehavior = itemBehaviors[index];
        if (itemBehavior == behavior || itemBehavior == originalBehavior1)
        {
          indexOf = index;
          break;
        }
        Behavior originalBehavior2 = StylizedBehaviors.GetOriginalBehavior((DependencyObject) itemBehavior);
        if (originalBehavior2 == behavior || originalBehavior2 == originalBehavior1)
        {
          indexOf = index;
          break;
        }
      }
      return indexOf;
    }

    private static Behavior GetOriginalBehavior(DependencyObject obj) => obj.GetValue(StylizedBehaviors.OriginalBehaviorProperty) as Behavior;

    private static void SetOriginalBehavior(DependencyObject obj, Behavior value) => obj.SetValue(StylizedBehaviors.OriginalBehaviorProperty, (object) value);
  }
}
