using AutoMapper;
using BusinessAvailability.API.Models.Domain;
using BusinessAvailability.API.Models.DTO;

namespace BusinessAvailability.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<BusinessService, BusinessServiceDTO>().ReverseMap();
        }
    }
}
