using AutoMapper;
using Repositories.Models;
using Service.BusinessModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapper
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<Car, CarResponModel>().ForMember(t => t.CarTypeName, c => c.MapFrom(s => s.CarType.TypeName));
            CreateMap<CarRequestModel, Car>();
            CreateMap<CarResponModel, Car>();
            CreateMap<CarTypeResponModel, CarType>().ReverseMap();
            CreateMap<CarType, CarRequestModel>().ReverseMap();
            CreateMap<CarTypeRequestModel, CarType>();
            CreateMap<TransactionRequestModel, Transaction>().ReverseMap();
            CreateMap<TransactionResponModel, Transaction>().ReverseMap();
            CreateMap<Transaction, TransactionResponModel>().ReverseMap();
            CreateMap<UserRequestModel, User>().ReverseMap();
            CreateMap<UserResponModel, User>().ReverseMap();

        }
    }
}
