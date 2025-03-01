using Int20h2025.Common.Models.DTO.Profile;
using Int20h2025.Common.Models.DTO.Prompt;
using Int20h2025.DAL.Entities;

namespace Int20h2025.BLL.Mappers
{
    public class MapperProfile : AutoMapper.Profile
    {
        public MapperProfile()
        {
            CreateMapForProfile();
            CreateMapForPrompts();
        }

        public void CreateMapForProfile()
        {
            CreateMap<Profile, ProfileDTO>();
        }

        public void CreateMapForPrompts()
        {
            CreateMap<Prompt, PromptDTO>()
                .ReverseMap();

            CreateMap<PromptDTO, PromptHistory>();
        }
    }
}
