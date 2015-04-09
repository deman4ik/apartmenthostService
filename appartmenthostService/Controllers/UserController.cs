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
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class UserController : TableController<Profile>
    {
        appartmenthostContext context = new appartmenthostContext();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            DomainManager = new EntityDomainManager<Profile>(context, Request, Services);
        }

        // GET tables/User
        public IQueryable<UserDTO> GetAllUser()
        {
            return Query().Project().To<UserDTO>();

        }

        // GET tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<UserDTO> GetUser(string id)
        {
            var result = Lookup(id).Queryable.Project().To<UserDTO>();



            //var result = this.Lookup(id).Queryable.Select(x => new UserDTO()
            //{
            //    Id = x.Id,
            //    FirstName = x.FirstName,
            //    LastName = x.LastName,
            //    Gender = x.Gender,
            //    Birthday = x.Birthday,
            //    Phone = x.Phone,
            //    ContactEmail = x.ContactEmail,
            //    ContactKind = x.ContactKind,
            //    Description = x.Description,
            //    PictureId = x.PictureId
            //} );
            return SingleResult<UserDTO>.Create(result);
        }

        // PATCH tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task<Profile> PatchProfile(string id, Delta<Profile> patch)
        //{
        //     return UpdateAsync(id, patch);
        //}

        //// POST tables/User
        //public async Task<IHttpActionResult> PostProfile(Profile item)
        //{
        //    Profile current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}

        //// DELETE tables/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //public Task DeleteProfile(string id)
        //{
        //     return DeleteAsync(id);
        //}

    }
}