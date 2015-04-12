using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using appartmenthostService.Authentication;
using appartmenthostService.DataObjects;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class ApartmentController : TableController<Apartment>
    {
        appartmenthostContext context = new appartmenthostContext();
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            DomainManager = new EntityDomainManager<Apartment>(context, Request, Services);
        }

        // GET tables/Apartment
       // [QueryableExpand("User")]
        public IQueryable<ApartmentDTO> GetAllApartment()
        {

            var currentUser = User as ServiceUser;
            var account = context.Accounts.SingleOrDefault(a => a.AccountId == currentUser.Id);
            //var props = context.Props.Where(p => p.Tables.Any(t => t.Name == "Apartment")).ToList();

            //List<PropValDTO> propValsList = new List<PropValDTO>();

            //foreach (var prop in props)
            //{
            //    var propVals = context.PropVals.Where(pv => pv.PropId)
            //}
            return Query().Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Price = x.Price,
                Cohabitation = x.Сohabitation,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Rating = x.Rating,
                PropsVals =
            });
           
        }



        // GET tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        // [QueryableExpand("User")]
        public SingleResult<ApartmentDTO> GetApartment(string id)
        {
            var currentUser = User as ServiceUser;
            var account = context.Accounts.SingleOrDefault(a => a.AccountId == currentUser.Id);
            var result = Lookup(id).Queryable.Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Price = x.Price,
                Cohabitation = x.Сohabitation,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Rating = x.Rating
                
            });
            return SingleResult<ApartmentDTO>.Create(result);
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