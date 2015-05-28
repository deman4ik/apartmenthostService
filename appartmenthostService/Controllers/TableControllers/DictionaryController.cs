using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class DictionaryController : TableController<Dictionary>
    {
        private apartmenthostContext _context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Dictionary>(_context, Request, Services);
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