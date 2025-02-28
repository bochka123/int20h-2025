using AutoMapper;

namespace Int20h2025.BLL.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMapForProfile();
        }

        public void CreateMapForProfile()
        {
            //CreateMap<BobrProfile, MyBobrProfileDTO>()
            //.ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.Level));

            //CreateMap<UpdateBobrProfileDTO, BobrProfile>();

            //CreateMap<BobrProfile, AuthorDTO>();

            //CreateMap<BobrLevel, BobrLevelDTO>();
        }
    }
}
