using AffittaCamere.RoomsService.Interfaces;
using AffittaCamere.WebStateless.DTO;
using AutoMapper;

namespace AffittaCamere.RestApiStateless.Helpers.AutoMapperProfiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<RoomData, RoomDTO>()
                .ForMember(dest => dest.IsAvailable, opt => opt.MapFrom(src => src.IsAvailable ? "Free" : "Busy"))
                .ReverseMap()
                ;
        }
    }
}
