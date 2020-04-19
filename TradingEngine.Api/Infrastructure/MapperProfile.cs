using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingEngine.Api.DTOs.Request;
using TradingEngine.Api.DTOs.Response;
using TradingEngine.Logic.Domain;
using TradingEngine.Logic.SharedKernel;

namespace TradingEngine.Api.Infrastructure
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Currency, GetAllCurrency>();
        }
    }
}
