// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.Localization.Services.ResourceLocalizationProvider
// Assembly: Innova.Launcher.Shared.Localization, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 75860252-4FA3-4057-81DA-EE75EDD3C78E
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.Localization.dll

using Innova.Launcher.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text.RegularExpressions;
using System.Windows;
using WPFLocalizeExtension.Engine;
using WPFLocalizeExtension.Providers;

namespace Innova.Launcher.Shared.Localization.Services
{
  public class ResourceLocalizationProvider : ILocalizationProvider
  {
    public static string LocalizationDictionaryName = "lang";
    public static readonly DependencyProperty DesignProvider = DependencyProperty.RegisterAttached(nameof (DesignProvider), typeof (ILocalizationProvider), typeof (DependencyObject), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(ResourceLocalizationProvider.DesignProviderPropertyChanged)));
    private readonly Dictionary<string, ResourceDictionary> _dictionaries = new Dictionary<string, ResourceDictionary>();
    private Type _dictionaryType;

    public static ILocalizationProvider GetDesignProvider(DependencyObject obj) => DesignerProperties.GetIsInDesignMode(obj) ? (ILocalizationProvider) obj.GetValue(ResourceLocalizationProvider.DesignProvider) : (ILocalizationProvider) null;

    public static void SetDesignProvider(DependencyObject obj, ILocalizationProvider value)
    {
      if (!DesignerProperties.GetIsInDesignMode(obj))
        return;
      obj.SetValue(ResourceLocalizationProvider.DesignProvider, (object) value);
    }

    public static void DesignProviderPropertyChanged(
      DependencyObject o,
      DependencyPropertyChangedEventArgs e)
    {
      try
      {
        LocalizeDictionary.SetProvider(o, (ILocalizationProvider) e.NewValue);
        LocalizeDictionary.SetDisableCache(o, true);
      }
      catch (Exception ex)
      {
      }
    }

    public Type DictionaryType
    {
      get => this._dictionaryType;
      set
      {
        this._dictionaryType = value;
        Type type = this.DictionaryType;
        if ((object) type == null)
          type = typeof (ResourceLocalizationProvider);
        this.LoadFromAssemblyResources(type.Assembly);
      }
    }

    public ResourceLocalizationProvider()
    {
      if (!DesignerProperties.GetIsInDesignMode(new DependencyObject()))
        return;
      try
      {
        Type type = this.DictionaryType;
        if ((object) type == null)
          type = typeof (ResourceLocalizationProvider);
        this.LoadFromAssemblyResources(type.Assembly);
      }
      catch (Exception ex)
      {
      }
    }

    public void LoadFromApplicationResources()
    {
      foreach (ResourceDictionary dictionary in Application.Current.Resources.MergedDictionaries.Where<ResourceDictionary>((Func<ResourceDictionary, bool>) (s => Path.GetFileName(s.Source.AbsolutePath).Contains(ResourceLocalizationProvider.LocalizationDictionaryName))))
        this.AddResourceDictionary(dictionary);
    }

    public void LoadFromAssemblyResources(Assembly assembly)
    {
      List<string> stringList = new List<string>();
      using (Stream manifestResourceStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".g.resources"))
      {
        ResourceReader resourceReader = new ResourceReader(manifestResourceStream ?? throw new InvalidOperationException());
        Regex regex = new Regex(ResourceLocalizationProvider.LocalizationDictionaryName + "\\.?[a-zA-Z-]*\\.baml$");
        foreach (DictionaryEntry dictionaryEntry in resourceReader)
        {
          if (regex.IsMatch(dictionaryEntry.Key.ToString()))
          {
            string str = Path.ChangeExtension(dictionaryEntry.Key.ToString(), ".xaml");
            stringList.Add(str);
          }
        }
      }
      foreach (string str in stringList)
        this.AddResourceDictionary(new ResourceDictionary()
        {
          Source = new Uri("pack://application:,,,/" + assembly.GetName().Name + ";component/" + str)
        });
    }

    public FullyQualifiedResourceKeyBase GetFullyQualifiedResourceKey(
      string key,
      DependencyObject target)
    {
      return (FullyQualifiedResourceKeyBase) new FQAssemblyDictionaryKey(key);
    }

    public object GetLocalizedObject(string key, DependencyObject target, CultureInfo culture)
    {
      try
      {
        ResourceDictionary resourceDictionary1;
        if (this._dictionaries.TryGetValue(culture.Name.ToLower(), out resourceDictionary1))
          return resourceDictionary1[(object) key];
        ResourceDictionary resourceDictionary2;
        if (this._dictionaries.TryGetValue(string.Empty, out resourceDictionary2))
          return resourceDictionary2[(object) key];
      }
      catch (KeyNotFoundException ex)
      {
        return (object) key;
      }
      throw new Exception(string.Format("Culture {0} is not supported, and there is no default dictionary.", (object) culture));
    }

    public void AddResourceDictionary(ResourceDictionary dictionary)
    {
      try
      {
        string name = Path.GetFileNameWithoutExtension(dictionary.Source.AbsolutePath).Replace(ResourceLocalizationProvider.LocalizationDictionaryName ?? "", "").RemoveLeadingChar('.');
        if (name != string.Empty)
          this.AvailableCultures.Add(CultureInfo.GetCultureInfo(name));
        this._dictionaries[name.ToLower()] = dictionary;
      }
      catch (Exception ex)
      {
      }
    }

    public ObservableCollection<CultureInfo> AvailableCultures { get; } = new ObservableCollection<CultureInfo>();

    public event ProviderChangedEventHandler ProviderChanged;

    public event ProviderErrorEventHandler ProviderError;

    public event ValueChangedEventHandler ValueChanged;
  }
}
