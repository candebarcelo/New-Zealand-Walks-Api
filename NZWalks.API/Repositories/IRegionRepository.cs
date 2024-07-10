using NZWalks.API.Models.Domain;

// the repository pattern is this one, where a repository works as an
// intermediary between the controller and the db, so as to ensure decoupling
// the data access layer from the rest of the app, consistency, performance
// as it allows caching, batching, etc. and easier switching between multiple
// data sources (via the update of the implementation of the injection in
// Program.cs) without modifying the logic of the app.
namespace NZWalks.API.Repositories
{
    // this is the interface which can have multiple implementations that implement
    // different data sources. such as a db, another db, mock hardcoded data, etc.
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();
        Task<Region?> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region?> UpdateAsync(Guid id, Region region);
        Task<Region?> DeleteAsync(Guid id);
    }
}
