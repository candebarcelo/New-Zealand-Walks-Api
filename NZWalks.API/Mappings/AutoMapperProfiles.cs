using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Mappings
{
    // autoMapper automatically converts from domain model to DTO and vice versa.
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            // map from a source to a destination
            // ReverseMap makes it so that it's mapped in both ways, i.e. from R to RDTO and from RDTO to R
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<WalkDto, Walk>().ReverseMap();
            CreateMap<DifficultyDto, Difficulty>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();

            // if the property names weren't the same, for example FullName in the source and Name in the
            // destination, it would be this after the CreateMap and b4 the ReverseMap:
            // .ForMember(x => x.Name, options => options.MapFrom(x => x.FullName))
        }
    }
}
