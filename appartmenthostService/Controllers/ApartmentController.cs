using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using appartmenthostService.Attributes;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.Models;
using appartmenthostService.Models;

namespace appartmenthostService.Controllers
{
    public class ApartmentController : TableController<Apartment>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            appartmenthostContext context = new appartmenthostContext();
            DomainManager = new EntityDomainManager<Apartment>(context, Request, Services);
        }

        // GET tables/Apartment
       // [QueryableExpand("User")]
        public IQueryable<Apartment> GetAllApartment()
        {
            return Query(); 
        }

        // GET tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
       // [QueryableExpand("User")]
        public SingleResult<Apartment> GetApartment(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Apartment> PatchApartment(string id, Delta<Apartment> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Apartment
        public async Task<IHttpActionResult> PostApartment(Apartment item)
        {
            Apartment current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteApartment(string id)
        {
             return DeleteAsync(id);
        }

    }
}