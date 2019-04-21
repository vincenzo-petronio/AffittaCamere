using AffittaCamere.DataAccess.Entities;
using AffittaCamere.WebStateless.DTO;
using AutoMapper;

namespace AffittaCamere.RestApiStateless.Helpers.AutoMapperProfiles
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<Room, RoomDTO>()
                //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                ;
            //CreateMap<RoomDTO, Room>();
        }
    }
}
