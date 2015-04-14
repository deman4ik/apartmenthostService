using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using appartmenthostService.DataObjects;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ProfileController : TableController<Profile>
    {
        appartmenthostContext context = new appartmenthostContext();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            DomainManager = new EntityDomainManager<Profile>(context, Request, Services);
        }

        // GET tables/Profile
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<UserDTO> GetAllProfile()
        {
            return Query().Project().To<UserDTO>();
        }

        // GET tables/Profile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [AuthorizeLevel(AuthorizationLevel.User)]
        public SingleResult<UserDTO> GetProfile(string id)
        {
            var result = Lookup(id).Queryable.Project().To<UserDTO>();
            return SingleResult<UserDTO>.Create(result);
        }

        // PATCH tables/Profile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task<Profile> PatchProfile(string id, Delta<Profile> patch)
        //{
        //     return UpdateAsync(id, patch);
        //}

        //// POST tables/Profile
        //public async Task<IHttpActionResult> PostProfile(Profile item)
        //{
        //    Profile current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}

        //// DELETE tables/Profile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task DeleteProfile(string id)
        //{
        //     return DeleteAsync(id);
        //}

    }
}