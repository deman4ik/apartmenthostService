using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using AutoMapper;

namespace apartmenthostService.Helpers
{
    public class DTOMapper
    {
        public static void CreateMapping(IConfiguration cfg)
        {
            cfg.CreateMap<Notification, NotificationDTO>();
            cfg.CreateMap<Apartment, ApartmentDTO>();
           cfg.CreateMap<Models.Profile, UserDTO>()
               .ForMember(userDTO => userDTO.Email, map => map.MapFrom(profile => profile.User.Email));

            cfg.CreateMap<Advert, AdvertDTO>()
                .ForMember(advertDTO => advertDTO.User, map => map.MapFrom(advert => advert.User))
                .ForMember(advertDTO => advertDTO.Apartment, map => map.MapFrom(advert => advert.Apartment));

            cfg.CreateMap<AdvertDTO, Advert>()
                .ForMember(advert => advert.User, map => map.Ignore())
                .ForMember(advert => advert.Apartment, map => map.Ignore());

        }
    }
}
