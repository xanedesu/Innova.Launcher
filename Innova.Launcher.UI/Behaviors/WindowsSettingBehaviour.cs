// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.UI.Behaviors.WindowsSettingBehaviour
// Assembly: Innova.Launcher.UI, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 0A08B436-53AA-4564-9604-DD72AF713AB7
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.UI.dll

using ControlzEx.Native;
using ControlzEx.Standard;
using Innova.Launcher.UI.Controls;
using Microsoft.Xaml.Behaviors;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;

namespace Innova.Launcher.UI.Behaviors
{
  public class WindowsSettingBehaviour : Behavior<ModernWindow>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
      this.AssociatedObject.SourceInitialized += new EventHandler(this.AssociatedObject_SourceInitialized);
    }

    protected override void OnDetaching()
    {
      this.CleanUp("from OnDetaching");
      base.OnDetaching();
    }

    private void AssociatedObject_Closed(object sender, EventArgs e) => this.CleanUp("from AssociatedObject closed event");

    private void AssociatedObject_Closing(object sender, CancelEventArgs e) => this.SaveWindowState();

    private void AssociatedObject_SourceInitialized(object sender, EventArgs e)
    {
      this.LoadWindowState();
      ModernWindow associatedObject = this.AssociatedObject;
      if (associatedObject == null)
      {
        Trace.TraceError(string.Format("{0}: Can not attach to nested events, cause the AssociatedObject is null.", (object) this));
      }
      else
      {
        associatedObject.StateChanged += new EventHandler(this.AssociatedObject_StateChanged);
        associatedObject.Closing += new CancelEventHandler(this.AssociatedObject_Closing);
        associatedObject.Closed += new EventHandler(this.AssociatedObject_Closed);
      }
    }

    private void AssociatedObject_StateChanged(object sender, EventArgs e)
    {
      ModernWindow associatedObject = this.AssociatedObject;
      if ((associatedObject != null ? (associatedObject.WindowState == WindowState.Minimized ? 1 : 0) : 0) == 0)
        return;
      this.SaveWindowState();
    }

    private void CleanUp(string fromWhere)
    {
      ModernWindow associatedObject = this.AssociatedObject;
      if (associatedObject == null)
      {
        Trace.TraceWarning(string.Format("{0}: Can not clean up {1}, cause the AssociatedObject is null. This can maybe happen if this Behavior was already detached.", (object) this, (object) fromWhere));
      }
      else
      {
        Trace.TraceInformation(string.Format("{0}: Clean up {1}.", (object) this, (object) fromWhere));
        associatedObject.StateChanged -= new EventHandler(this.AssociatedObject_StateChanged);
        associatedObject.Closing -= new CancelEventHandler(this.AssociatedObject_Closing);
        associatedObject.Closed -= new EventHandler(this.AssociatedObject_Closed);
        associatedObject.SourceInitialized -= new EventHandler(this.AssociatedObject_SourceInitialized);
      }
    }

    private void LoadWindowState()
    {
      ModernWindow associatedObject = this.AssociatedObject;
      IWindowPlacementSettings placementSettings = associatedObject?.GetWindowPlacementSettings();
      if (placementSettings == null)
        return;
      if (!associatedObject.SaveWindowPosition)
        return;
      try
      {
        placementSettings.Reload();
      }
      catch (Exception ex)
      {
        Trace.TraceError(string.Format("{0}: The settings could not be reloaded! {1}", (object) this, (object) ex));
        return;
      }
      if (placementSettings.Placement == null)
        return;
      if (placementSettings.Placement.normalPosition.IsEmpty)
        return;
      try
      {
        WINDOWPLACEMENT placement = placementSettings.Placement;
        placement.flags = 0;
        placement.showCmd = placement.showCmd == SW.SHOWMINIMIZED ? SW.SHOWNORMAL : placement.showCmd;
        associatedObject.Left = (double) placement.normalPosition.Left;
        associatedObject.Top = (double) placement.normalPosition.Top;
        if (NativeMethods.SetWindowPlacement(new WindowInteropHelper((Window) associatedObject).Handle, placement))
          return;
        Trace.TraceWarning(string.Format("{0}: The WINDOWPLACEMENT {1} could not be set by SetWindowPlacement.", (object) this, (object) placement));
      }
      catch (Exception ex)
      {
        throw new InvalidOperationException("Failed to set the window state from the settings file", ex);
      }
    }

    private void SaveWindowState()
    {
      ModernWindow associatedObject = this.AssociatedObject;
      IWindowPlacementSettings placementSettings = associatedObject?.GetWindowPlacementSettings();
      if (placementSettings == null || !associatedObject.SaveWindowPosition)
        return;
      IntPtr handle = new WindowInteropHelper((Window) associatedObject).Handle;
      WINDOWPLACEMENT windowPlacement = NativeMethods.GetWindowPlacement(handle);
      if (windowPlacement.showCmd != SW.HIDE && windowPlacement.length > 0)
      {
        ControlzEx.Standard.RECT lpRect;
        if (windowPlacement.showCmd == SW.SHOWNORMAL && UnsafeNativeMethods.GetWindowRect(handle, out lpRect))
          windowPlacement.normalPosition = lpRect;
        if (!windowPlacement.normalPosition.IsEmpty)
          placementSettings.Placement = windowPlacement;
      }
      try
      {
        placementSettings.Save();
      }
      catch (Exception ex)
      {
        Trace.TraceError(string.Format("{0}: The settings could not be saved! {1}", (object) this, (object) ex));
      }
    }
  }
}
