// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Extensions.Extensions
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Innova.Launcher.UI.Extensions
{
  public static class Extensions
  {
    public static IEnumerable<DependencyObject> VisualDepthFirstTraversal(this DependencyObject node)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      yield return node;
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(node); ++i)
      {
        foreach (DependencyObject dependencyObject in VisualTreeHelper.GetChild(node, i).VisualDepthFirstTraversal())
          yield return dependencyObject;
      }
    }

    public static IEnumerable<DependencyObject> VisualBreadthFirstTraversal(
      this DependencyObject node)
    {
      if (node == null)
        throw new ArgumentNullException(nameof (node));
      int i;
      for (i = 0; i < VisualTreeHelper.GetChildrenCount(node); ++i)
        yield return VisualTreeHelper.GetChild(node, i);
      for (i = 0; i < VisualTreeHelper.GetChildrenCount(node); ++i)
      {
        foreach (DependencyObject dependencyObject in VisualTreeHelper.GetChild(node, i).VisualDepthFirstTraversal())
          yield return dependencyObject;
      }
    }

    public static bool IsAncestorOf(this DependencyObject parent, DependencyObject node) => node != null && parent.VisualDepthFirstTraversal().Contains<DependencyObject>(node);

    public static IEnumerable<DependencyObject> GetVisualAncestry(this DependencyObject leaf)
    {
      for (; leaf != null; leaf = VisualTreeHelper.GetParent(leaf))
        yield return leaf;
    }

    public static IEnumerable<DependencyObject> GetLogicalAncestry(this DependencyObject leaf)
    {
      for (; leaf != null; leaf = LogicalTreeHelper.GetParent(leaf))
        yield return leaf;
    }

    public static bool IsDescendantOf(this DependencyObject leaf, DependencyObject ancestor)
    {
      DependencyObject leaf1 = (DependencyObject) null;
      foreach (DependencyObject objA in leaf.GetVisualAncestry())
      {
        if (object.Equals((object) objA, (object) ancestor))
          return true;
        leaf1 = objA;
      }
      return leaf1.GetLogicalAncestry().Contains<DependencyObject>(ancestor);
    }
  }
}
