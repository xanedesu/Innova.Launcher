// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Properties.Resources
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Innova.Launcher.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (Innova.Launcher.Properties.Resources.resourceMan == null)
          Innova.Launcher.Properties.Resources.resourceMan = new ResourceManager("Innova.Launcher.Properties.Resources", typeof (Innova.Launcher.Properties.Resources).Assembly);
        return Innova.Launcher.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get => Innova.Launcher.Properties.Resources.resourceCulture;
      set => Innova.Launcher.Properties.Resources.resourceCulture = value;
    }

    internal static string GothamProLightWoff2Front => Innova.Launcher.Properties.Resources.ResourceManager.GetString(nameof (GothamProLightWoff2Front), Innova.Launcher.Properties.Resources.resourceCulture);

    internal static string GothamProMediumFront => Innova.Launcher.Properties.Resources.ResourceManager.GetString(nameof (GothamProMediumFront), Innova.Launcher.Properties.Resources.resourceCulture);

    internal static string GothamProMediumWoff2Front => Innova.Launcher.Properties.Resources.ResourceManager.GetString(nameof (GothamProMediumWoff2Front), Innova.Launcher.Properties.Resources.resourceCulture);
  }
}
