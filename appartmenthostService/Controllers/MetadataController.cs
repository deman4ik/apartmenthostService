using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using appartmenthostService.DataObjects;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace appartmenthostService.Controllers
{
     
     [AuthorizeLevel(AuthorizationLevel.Application)]
    public class MetadataController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/Metadata/Apartment
         [Route("api/Metadata/Apartment")]
        public string GetApartment()
        {
            ApartmentDTO apartment = new ApartmentDTO();
             List<MetadataItem> apartmentItems = apartment.GetType().GetProperties().Select(prop => new MetadataItem()
             {
                 Type = prop.PropertyType.Name,
                 Name = prop.Name
             }).ToList();
             return JsonConvert.SerializeObject(apartmentItems);
        }

         // GET api/Metadata/Advert
         [Route("api/Metadata/Advert")]
         public string GetAdvert()
         {
             AdvertDTO advert = new AdvertDTO();
             List<MetadataItem> advertItems = advert.GetType().GetProperties().Select(prop => new MetadataItem()
             {
                 Type = prop.PropertyType.Name,
                 Name = prop.Name
             }).ToList();
             return JsonConvert.SerializeObject(advertItems);
         }

         // GET api/Metadata/User
         [Route("api/Metadata/User")]
         public string GetUser()
         {
             UserDTO user = new UserDTO();
             List<MetadataItem> userItems = user.GetType().GetProperties().Select(prop => new MetadataItem()
             {
                 Type = prop.PropertyType.Name,
                 Name = prop.Name
             }).ToList();
             return JsonConvert.SerializeObject(userItems);
         }
    }
}
