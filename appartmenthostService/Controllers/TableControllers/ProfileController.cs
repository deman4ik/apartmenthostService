using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ProfileController : TableController<Profile>
    {
        private apartmenthostContext _contex;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            _contex = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Profile>(_contex, Request, Services);
        }

        // GET tables/Profile
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<UserDTO> GetAllProfile()
        {
            return Query().Select(x=> new UserDTO()
            {
                Id = x.Id,
                Email = x.User.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                Birthday = x.Birthday,
                Phone = x.Phone,
                ContactEmail = x.ContactEmail,
                ContactKind = x.ContactKind,
                Description = x.Description,
                Rating = x.Rating,
                RatingCount = x.RatingCount,
                Score = x.Score,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt

            });
        }

        // GET tables/Profile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [AuthorizeLevel(AuthorizationLevel.User)]
        public SingleResult<UserDTO> GetProfile(string id)
        {
            var result = Lookup(id).Queryable.Select(x=> new UserDTO()
            {
                Id = x.Id,
                Email = x.User.Email,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Gender = x.Gender,
                Birthday = x.Birthday,
                Phone = x.Phone,
                ContactEmail = x.ContactEmail,
                ContactKind = x.ContactKind,
                Description = x.Description,
                Rating = x.Rating,
                RatingCount = x.RatingCount,
                Score = x.Score,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt

            });
            return SingleResult<UserDTO>.Create(result);
        }
    }
}