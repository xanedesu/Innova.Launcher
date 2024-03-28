// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Shared.PrismUnityContainer
// Assembly: Innova.Launcher.Shared, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: 20DF0516-E71E-465E-9BFF-8BD10660A67D
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Shared.dll

using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity;
using Unity.Resolution;

namespace Innova.Launcher.Shared
{
  public class PrismUnityContainer : 
    IContainerExtension<IUnityContainer>,
    IContainerExtension,
    IContainerProvider,
    IContainerRegistry
  {
    public IUnityContainer Instance { get; }

    public PrismUnityContainer()
      : this((IUnityContainer) new UnityContainer())
    {
    }

    public PrismUnityContainer(IUnityContainer container) => this.Instance = container;

    public void FinalizeExtension()
    {
    }

    public IContainerRegistry RegisterInstance(Type type, object instance)
    {
      this.Instance.RegisterInstance(type, instance);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterInstance(Type type, object instance, string name)
    {
      this.Instance.RegisterInstance(type, name, instance);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterSingleton(Type from, Type to)
    {
      this.Instance.RegisterSingleton(from, to);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
    {
      this.Instance.RegisterSingleton(from, to, name);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry Register(Type from, Type to)
    {
      this.Instance.RegisterType(from, to);
      return (IContainerRegistry) this;
    }

    public IContainerRegistry Register(Type from, Type to, string name)
    {
      this.Instance.RegisterType(from, to, name);
      return (IContainerRegistry) this;
    }

    public object Resolve(Type type) => this.Instance.Resolve(type);

    public object Resolve(Type type, string name) => this.Instance.Resolve(type, name);

    public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
    {
      DependencyOverride[] array = ((IEnumerable<(Type, object)>) parameters).Select<(Type, object), DependencyOverride>((Func<(Type, object), DependencyOverride>) (p => new DependencyOverride(p.Type, p.Instance))).ToArray<DependencyOverride>();
      return this.Instance.Resolve(type, (ResolverOverride[]) array);
    }

    public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
    {
      DependencyOverride[] array = ((IEnumerable<(Type, object)>) parameters).Select<(Type, object), DependencyOverride>((Func<(Type, object), DependencyOverride>) (p => new DependencyOverride(p.Type, p.Instance))).ToArray<DependencyOverride>();
      return this.Instance.Resolve(type, name, (ResolverOverride[]) array);
    }

    public bool IsRegistered(Type type) => this.Instance.IsRegistered(type);

    public bool IsRegistered(Type type, string name) => this.Instance.IsRegistered(type, name);
  }
}
