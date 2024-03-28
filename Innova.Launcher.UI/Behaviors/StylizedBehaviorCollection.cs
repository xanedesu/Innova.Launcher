// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.StylizedBehaviorCollection
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using Microsoft.Xaml.Behaviors;
using System.Windows;

namespace Innova.Launcher.UI.Behaviors
{
  public class StylizedBehaviorCollection : FreezableCollection<Behavior>
  {
    protected override Freezable CreateInstanceCore() => (Freezable) new StylizedBehaviorCollection();
  }
}
