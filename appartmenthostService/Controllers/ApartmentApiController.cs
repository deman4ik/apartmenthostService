using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using appartmenthostService.DataObjects;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ApartmentApiController : ApiController
    {
        public ApiServices Services { get; set; }
        appartmenthostContext context = new appartmenthostContext();
        
        // POST api/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Apartment")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost] 
        public HttpResponseMessage PostApartment(ApartmentDTO apartment)
        {

            //Apartment current;
            return null;
        }

    }
}
