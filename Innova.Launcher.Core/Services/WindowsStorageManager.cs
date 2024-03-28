// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Services.WindowsStorageManager
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using Innova.Launcher.Core.Services.Interfaces;
using System.Collections.Generic;

namespace Innova.Launcher.Core.Services
{
  public class WindowsStorageManager : IWindowsStorageManager
  {
    private readonly Dictionary<string, WindowStorage> _storages = new Dictionary<string, WindowStorage>();

    public void Save(string windowId, WindowStorage storage) => this._storages[windowId] = storage;

    public WindowStorage Get(string windowId)
    {
      WindowStorage windowStorage;
      return !this._storages.TryGetValue(windowId, out windowStorage) ? (WindowStorage) null : windowStorage;
    }
  }
}
