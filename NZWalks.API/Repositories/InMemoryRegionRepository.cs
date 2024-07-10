using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    // if i need to use a mock service like this one or another implementation,
    // it's as easy as changing the implementation in the injection of IRegionRepository
    // in the Program.cs file.
    public class InMemoryRegionRepository : IRegionRepository
    {
        public async Task<List<Region>> GetAllAsync()
        {
            return new List<Region>() {
                new Region()
                {
                    Id = Guid.NewGuid(),
                    Code = "CRN",
                    Name = "Cande's Region Name"
                }
            };
        }

        public Task<Region?> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Region> CreateAsync(Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> UpdateAsync(Guid id, Region region)
        {
            throw new NotImplementedException();
        }

        public Task<Region?> DeleteAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
