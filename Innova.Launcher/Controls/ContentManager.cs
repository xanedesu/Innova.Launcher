// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Controls.TabContent
// Assembly: Innova.Launcher, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 96BDB793-4E55-45E4-810A-315A4AB9F905
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.dll

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace Innova.Launcher.Controls
{
  public static class TabContent
  {
    public static readonly DependencyProperty IsCachedProperty = DependencyProperty.RegisterAttached("IsCached", typeof (bool), typeof (TabContent), (PropertyMetadata) new UIPropertyMetadata((object) false, new PropertyChangedCallback(TabContent.OnIsCachedChanged)));
    public static readonly DependencyProperty TemplateProperty = DependencyProperty.RegisterAttached("Template", typeof (DataTemplate), typeof (TabContent), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty TemplateSelectorProperty = DependencyProperty.RegisterAttached("TemplateSelector", typeof (DataTemplateSelector), typeof (TabContent), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static readonly DependencyProperty InternalTabControlProperty = DependencyProperty.RegisterAttached("InternalTabControl", typeof (TabControl), typeof (TabContent), (PropertyMetadata) new UIPropertyMetadata((object) null, new PropertyChangedCallback(TabContent.OnInternalTabControlChanged)));
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static readonly DependencyProperty InternalCachedContentProperty = DependencyProperty.RegisterAttached("InternalCachedContent", typeof (ContentControl), typeof (TabContent), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));
    public static readonly DependencyProperty InternalContentManagerProperty = DependencyProperty.RegisterAttached("InternalContentManager", typeof (object), typeof (TabContent), (PropertyMetadata) new UIPropertyMetadata((PropertyChangedCallback) null));

    public static bool GetIsCached(DependencyObject obj) => (bool) obj.GetValue(TabContent.IsCachedProperty);

    public static void SetIsCached(DependencyObject obj, bool value) => obj.SetValue(TabContent.IsCachedProperty, (object) value);

    public static DataTemplate GetTemplate(DependencyObject obj) => (DataTemplate) obj.GetValue(TabContent.TemplateProperty);

    public static void SetTemplate(DependencyObject obj, DataTemplate value) => obj.SetValue(TabContent.TemplateProperty, (object) value);

    public static DataTemplateSelector GetTemplateSelector(DependencyObject obj) => (DataTemplateSelector) obj.GetValue(TabContent.TemplateSelectorProperty);

    public static void SetTemplateSelector(DependencyObject obj, DataTemplateSelector value) => obj.SetValue(TabContent.TemplateSelectorProperty, (object) value);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static TabControl GetInternalTabControl(DependencyObject obj) => (TabControl) obj.GetValue(TabContent.InternalTabControlProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetInternalTabControl(DependencyObject obj, TabControl value) => obj.SetValue(TabContent.InternalTabControlProperty, (object) value);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static ContentControl GetInternalCachedContent(DependencyObject obj) => (ContentControl) obj.GetValue(TabContent.InternalCachedContentProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetInternalCachedContent(DependencyObject obj, ContentControl value) => obj.SetValue(TabContent.InternalCachedContentProperty, (object) value);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static object GetInternalContentManager(DependencyObject obj) => obj.GetValue(TabContent.InternalContentManagerProperty);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void SetInternalContentManager(DependencyObject obj, object value) => obj.SetValue(TabContent.InternalContentManagerProperty, value);

    private static void OnIsCachedChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      if (obj == null)
        return;
      if (!(obj is TabControl tabControl))
        throw new InvalidOperationException("Cannot set TabContent.IsCached on object of type " + args.NewValue.GetType().Name + ". Only objects of type TabControl can have TabContent.IsCached property.");
      if (!(bool) args.NewValue)
      {
        if (args.OldValue != null && (bool) args.OldValue)
          throw new NotImplementedException("Cannot change TabContent.IsCached from True to False. Turning tab caching off is not implemented");
      }
      else
      {
        TabContent.EnsureContentTemplateIsNull(tabControl);
        tabControl.ContentTemplate = TabContent.CreateContentTemplate();
        TabContent.EnsureContentTemplateIsNotModified(tabControl);
      }
    }

    private static DataTemplate CreateContentTemplate()
    {
      ParserContext parserContext = new ParserContext();
      parserContext.XamlTypeMapper = new XamlTypeMapper(new string[0]);
      parserContext.XamlTypeMapper.AddMappingProcessingInstruction("b", typeof (TabContent).Namespace, typeof (TabContent).Assembly.FullName);
      parserContext.XmlnsDictionary.Add("", "http://schemas.microsoft.com/winfx/2006/xaml/presentation");
      parserContext.XmlnsDictionary.Add("b", "b");
      return (DataTemplate) XamlReader.Parse("<DataTemplate><Border b:TabContent.InternalTabControl=\"{Binding RelativeSource={RelativeSource AncestorType=TabControl}}\" /></DataTemplate>", parserContext);
    }

    private static void OnInternalTabControlChanged(
      DependencyObject obj,
      DependencyPropertyChangedEventArgs args)
    {
      if (obj == null)
        return;
      if (!(obj is Decorator container))
        throw new InvalidOperationException("Cannot set TabContent.InternalTabControl on object of type " + ((object) obj).GetType().Name + ". Only controls that derive from Decorator, such as Border can have a TabContent.InternalTabControl.");
      if (args.NewValue == null)
        return;
      if (!(args.NewValue is TabControl))
        throw new InvalidOperationException("Value of TabContent.InternalTabControl cannot be of type " + args.NewValue.GetType().Name + ", it must be of type TabControl");
      TabContent.GetContentManager((TabControl) args.NewValue, container).UpdateSelectedTab();
    }

    private static TabContent.ContentManager GetContentManager(
      TabControl tabControl,
      Decorator container)
    {
      TabContent.ContentManager contentManager = (TabContent.ContentManager) TabContent.GetInternalContentManager((DependencyObject) tabControl);
      if (contentManager != null)
      {
        contentManager.ReplaceContainer(container);
      }
      else
      {
        contentManager = new TabContent.ContentManager(tabControl, container);
        TabContent.SetInternalContentManager((DependencyObject) tabControl, (object) contentManager);
      }
      return contentManager;
    }

    private static void EnsureContentTemplateIsNull(TabControl tabControl)
    {
      if (tabControl.ContentTemplate != null)
        throw new InvalidOperationException("TabControl.ContentTemplate value is not null. If TabContent.IsCached is True, use TabContent.Template instead of ContentTemplate");
    }

    private static void EnsureContentTemplateIsNotModified(TabControl tabControl) => ((PropertyDescriptor) DependencyPropertyDescriptor.FromProperty(TabControl.ContentTemplateProperty, typeof (TabControl))).AddValueChanged((object) tabControl, (EventHandler) ((sender, args) =>
    {
      throw new InvalidOperationException("Cannot assign to TabControl.ContentTemplate when TabContent.IsCached is True. Use TabContent.Template instead");
    }));

    public class ContentManager
    {
      private TabControl _tabControl;
      private Decorator _border;

      public ContentManager(TabControl tabControl, Decorator border)
      {
        this._tabControl = tabControl;
        this._border = border;
        this._tabControl.SelectionChanged += (SelectionChangedEventHandler) ((sender, args) => this.UpdateSelectedTab());
      }

      public void ReplaceContainer(Decorator newBorder)
      {
        if (this._border == newBorder)
          return;
        this._border.Child = (UIElement) null;
        this._border = newBorder;
      }

      public void UpdateSelectedTab() => this._border.Child = (UIElement) this.GetCurrentContent();

      private ContentControl GetCurrentContent()
      {
        object selectedItem = this._tabControl.SelectedItem;
        if (selectedItem == null)
          return (ContentControl) null;
        DependencyObject dependencyObject = this._tabControl.ItemContainerGenerator.ContainerFromItem(selectedItem);
        if (dependencyObject == null)
          return (ContentControl) null;
        ContentControl currentContent = TabContent.GetInternalCachedContent(dependencyObject);
        if (currentContent == null)
        {
          ContentControl contentControl = new ContentControl();
          contentControl.DataContext = selectedItem;
          contentControl.ContentTemplate = TabContent.GetTemplate((DependencyObject) this._tabControl);
          contentControl.ContentTemplateSelector = TabContent.GetTemplateSelector((DependencyObject) this._tabControl);
          currentContent = contentControl;
          currentContent.SetBinding(ContentControl.ContentProperty, (BindingBase) new Binding());
          TabContent.SetInternalCachedContent(dependencyObject, currentContent);
        }
        return currentContent;
      }
    }
  }
}
