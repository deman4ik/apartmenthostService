using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class PropController : TableController<Prop>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            apartmenthostContext context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Prop>(context, Request, Services);
        }

        // GET tables/Prop
        public IQueryable<Prop> GetAllProp()
        {
            return Query(); 
        }

        // GET tables/Prop/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Prop> GetProp(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Prop/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Prop> PatchProp(string id, Delta<Prop> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Prop
        public async Task<IHttpActionResult> PostProp(Prop item)
        {
            Prop current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Prop/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteProp(string id)
        {
             return DeleteAsync(id);
        }

    }
}