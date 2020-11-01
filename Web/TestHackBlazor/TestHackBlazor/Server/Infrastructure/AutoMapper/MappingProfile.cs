using AutoMapper;
using TestHackBlazor.Server.Areas.Api.Controllers.Auth;
using TestHackBlazor.Server.Areas.Api.Controllers.Constructions.Models;
using TestHackBlazor.Server.Entities;
using TestHackBlazor.Shared.DTO;

namespace TestHackBlazor.Server.Infrastructure.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserInfoDTO>();

            CreateMap<BorderPointDTO, BorderPointEntity>().ReverseMap();
            CreateMap<ConstructionCreateDTO, ConstructionEntity>();

            CreateMap<ConstructionEntity, ConstructionBase>();

            CreateMap<ConstructionEntity, ConstructionDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserBaseInfoDTO>();

            CreateMap<EmergencyEntity, EmergencyDTO>();

        }
    }
}
