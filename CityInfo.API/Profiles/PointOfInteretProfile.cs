using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInteretProfile : Profile
    {
        public PointOfInteretProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>();
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
            CreateMap<Models.PointOfInterestForUpdateDto, Entities.PointOfInterest>();
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestForUpdateDto>();

        }
    }
}
