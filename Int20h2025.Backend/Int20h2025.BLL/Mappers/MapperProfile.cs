using AutoMapper;
using Int20h2025.Common.Models.DTO.Profile;

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
            CreateMap<DAL.Entities.Profile, ProfileDTO>();
        }
    }
}
