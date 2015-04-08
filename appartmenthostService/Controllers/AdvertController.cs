using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using appartmenthostService.Attributes;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.Models;
using appartmenthostService.Models;
using appartmenthostService.Helpers;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    
    public class AdvertController : TableController<Advert>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            appartmenthostContext context = new appartmenthostContext();
            DomainManager = new EntityDomainManager<Advert>(context, Request, Services, enableSoftDelete: true);
        }

        // GET tables/Advert
       // [QueryableExpand("Apartments")]
        public IQueryable<Advert> GetAllAdverts()
        {
            return Query();
        }

        // GET tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
       // [QueryableExpand("Apartments")]
        public SingleResult<Advert> GetAdvert(string id)
        {
            return Lookup(id);
        }

    }
}