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
    public class DictionaryItemController : TableController<DictionaryItem>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            appartmenthostContext context = new appartmenthostContext();
            DomainManager = new EntityDomainManager<DictionaryItem>(context, Request, Services);
        }

        // GET tables/DictionaryItem
        public IQueryable<DictionaryItem> GetAllDictionaryItem()
        {
            return Query(); 
        }

        // GET tables/DictionaryItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<DictionaryItem> GetDictionaryItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/DictionaryItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<DictionaryItem> PatchDictionaryItem(string id, Delta<DictionaryItem> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/DictionaryItem
        public async Task<IHttpActionResult> PostDictionaryItem(DictionaryItem item)
        {
            DictionaryItem current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/DictionaryItem/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteDictionaryItem(string id)
        {
             return DeleteAsync(id);
        }

    }
}