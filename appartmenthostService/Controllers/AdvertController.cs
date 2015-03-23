using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.DataObjects;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)] 
    public class AdvertController : TableController<Advert>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            appartmenthostContext context = new appartmenthostContext();
            DomainManager = new EntityDomainManager<Advert>(context, Request, Services);
        }

        // GET tables/Advert
        public IQueryable<Advert> GetAllAdverts()
        {
            // Get the logged in user
            var currentUser = User as ServiceUser;

            return Query().Where(advert => advert.UserId == currentUser.Id);
        }

        // GET tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Advert> GetAdvert(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Advert> PatchAdvert(string id, Delta<Advert> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Advert
        public async Task<IHttpActionResult> PostAdvert(Advert item)
        {
            // Get the logged in user
            var currentUser = User as ServiceUser;

            // Set the user ID on the item
            item.UserId = currentUser.Id;
            Advert current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Advert/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteAdvert(string id)
        {
            return DeleteAsync(id);
        }
    }
}