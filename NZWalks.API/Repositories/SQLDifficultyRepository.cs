using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLDifficultyRepository : IDifficultyRepository
    {
        private readonly NZWalksDbContext dbContext;

        public SQLDifficultyRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Difficulty>> GetAllAsync()
        {
            return await dbContext.Difficulties.ToListAsync();
        }
    }
}
