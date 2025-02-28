using AutoMapper;
using Int20h2025.Auth.Interfaces;
using Int20h2025.Auth.Models.DTO;
using Int20h2025.Auth.Services;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.DTO.Profile;
using Int20h2025.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Profile = Int20h2025.DAL.Entities.Profile;

namespace Int20h2025.BLL.Services
{
    public class ProfileService(Int20h2025Context context, IUserContextService userContextService, IAuthService authService, IMapper mapper): IProfileService
    {
        public async Task<ProfileDTO> EnsureProfileCreatedAsync()
        {
            var userId = userContextService.UserId;
            var user = await context.Profiles.FindAsync(userId);

            if (user == null)
            {
                var profile = new Profile()
                {
                    Id = userId
                };
                var entityState = await context.Profiles.AddAsync(profile);
                await context.SaveChangesAsync();
                user = entityState.Entity;
            }

            return mapper.Map<ProfileDTO>(user);
        }
    }
}
