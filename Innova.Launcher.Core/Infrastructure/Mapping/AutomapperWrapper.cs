// Decompiled with JetBrains decompiler
// Type: Innova.Launcher.Core.Infrastructure.Mapping.AutomapperWrapper
// Assembly: Innova.Launcher.Core, Version=1.0.0.279, Culture=neutral, PublicKeyToken=null
// MVID: C2445957-1F4B-446A-96F1-46D18C567E85
// Assembly location: C:\Program Files (x86)\Innova\4game2.0\bin\Innova.Launcher.Core.dll

using AutoMapper;
using Innova.Launcher.Core.Infrastructure.Crypto;
using Innova.Launcher.Core.Models;
using Innova.Launcher.Core.Services.MessageHandlers.Status;
using System;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Innova.Launcher.Core.Infrastructure.Mapping
{
  public class AutomapperWrapper : Innova.Launcher.Core.Infrastructure.Mapping.Interfaces.IMapper
  {
    private readonly Mapper _mapper;

    public AutomapperWrapper() => this._mapper = new Mapper((IConfigurationProvider) new MapperConfiguration((Action<IMapperConfigurationExpression>) (v =>
    {
      v.CreateMap<Game, ServiceStatus>().ForMember<string>((Expression<Func<ServiceStatus, string>>) (status => status.GameKey), (Action<IMemberConfigurationExpression<Game, ServiceStatus, string>>) (expression => expression.MapFrom<string>((Expression<Func<Game, string>>) (game => game.Key)))).ForMember<object>((Expression<Func<ServiceStatus, object>>) (status => status.Info), (Action<IMemberConfigurationExpression<Game, ServiceStatus, object>>) (expression => expression.MapFrom<object>((Expression<Func<Game, object>>) (game => (object) game.ProgressInfo ?? "info")))).ForMember<bool>((Expression<Func<ServiceStatus, bool>>) (status => status.Configurable), (Action<IMemberConfigurationExpression<Game, ServiceStatus, bool>>) (expression => expression.MapFrom<bool>((Expression<Func<Game, bool>>) (game => !string.IsNullOrEmpty(game.OptionsExe)))));
      v.CreateMap<Innova.Launcher.Shared.Models.GameConfig.GameConfig, Game>().ForMember<string>((Expression<Func<Game, string>>) (game => game.Name), (Action<IMemberConfigurationExpression<Innova.Launcher.Shared.Models.GameConfig.GameConfig, Game, string>>) (expression => expression.MapFrom<string>((Expression<Func<Innova.Launcher.Shared.Models.GameConfig.GameConfig, string>>) (config => config.DisplayName))));
      v.CreateMap<RSAParameters, RSAParametersSerializable>().ReverseMap();
    })));

    public TDestination Map<TSource, TDestination>(TSource source) => this._mapper.Map<TSource, TDestination>(source);
  }
}
