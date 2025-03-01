using AutoMapper;
using Int20h2025.Auth.Interfaces;
using Int20h2025.BLL.Interfaces;
using Int20h2025.Common.Models.DTO.Prompt;
using Int20h2025.DAL.Context;
using Int20h2025.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Int20h2025.BLL.Services
{
    public class PromptService(Int20h2025Context context, IUserContextService userContext, IMapper mapper): IPromptService
    {
        public async Task<ICollection<PromptDTO>> GetHistoryAsync()
        {
            var prompts = await context.Prompts
                .Where(x => x.ProfileId == userContext.UserId)
                .OrderByDescending(x => x.UpdatedOn)
                .Take(10)
                .ToListAsync();

            return mapper.Map<ICollection<PromptDTO>>(prompts);
        }

        public async Task CreateAsync(PromptDTO promptDTO)
        {
            var prompt = mapper.Map<Prompt>(promptDTO);
            var promptHistory = mapper.Map<PromptHistory>(promptDTO);
            prompt.ProfileId = userContext.UserId;
            promptHistory.ProfileId = userContext.UserId;
            prompt.History.Add(promptHistory);
            await context.Prompts.AddAsync(prompt);
            await context.SaveChangesAsync();
        }
    }
}
