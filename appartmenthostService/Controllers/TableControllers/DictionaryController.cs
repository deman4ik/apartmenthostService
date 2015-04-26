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
    public class DictionaryController : TableController<Dictionary>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            apartmenthostContext context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Dictionary>(context, Request, Services);
        }

        // GET tables/Dictionary
        public IQueryable<Dictionary> GetAllDictionary()
        {
            return Query(); 
        }

        // GET tables/Dictionary/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Dictionary> GetDictionary(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Dictionary/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Dictionary> PatchDictionary(string id, Delta<Dictionary> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Dictionary
        public async Task<IHttpActionResult> PostDictionary(Dictionary item)
        {
            Dictionary current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Dictionary/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDictionary(string id)
        {
             return DeleteAsync(id);
        }

    }
}