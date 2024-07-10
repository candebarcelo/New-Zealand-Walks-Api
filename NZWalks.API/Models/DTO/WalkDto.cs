namespace NZWalks.API.Models.DTO
{
    public class WalkDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? WalkImageUrl { get; set; }

        // we need the mapping from Region to RegionDto and from Difficulty
        // to DifficultyDto and also the Include in the repository, in able to
        // be able to pass these objects.
        public RegionDto Region { get; set; }
        public DifficultyDto Difficulty { get; set; }
    }
}
