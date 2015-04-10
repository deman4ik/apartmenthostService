using appartmenthostService.DataObjects;
using appartmenthostService.Models;
using AutoMapper;

namespace appartmenthostService.Helpers
{
    public class DTOMapper
    {
        public static void CreateMapping(IConfiguration cfg)
        {

            cfg.CreateMap<Models.Profile, UserDTO>();            
            cfg.CreateMap<Apartment, ApartmentDTO>();

            cfg.CreateMap<Advert, AdvertDTO>()
                .ForMember(advertDTO => advertDTO.User, map => map.MapFrom(advert => advert.User))
                .ForMember(advertDTO => advertDTO.Apartment, map => map.MapFrom(advert => advert.Apartment));

            cfg.CreateMap<AdvertDTO, Advert>()
                .ForMember(advert => advert.User, map => map.Ignore())
                .ForMember(advert => advert.Apartment, map => map.Ignore());
        }
    }
}