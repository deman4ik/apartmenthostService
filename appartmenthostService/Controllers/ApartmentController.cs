using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using appartmenthostService.Authentication;
using appartmenthostService.DataObjects;
using appartmenthostService.Helpers;
using Microsoft.WindowsAzure.Mobile.Service;
using appartmenthostService.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{
    
    [AuthorizeLevel(AuthorizationLevel.Application)]
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
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<ApartmentDTO> GetAllApartment()
        {

            var account = AuthUtils.GetUserAccount(User as ServiceUser);

            return Query().Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Price = x.Price,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Rating = x.Rating,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                PropsVals = context.PropVals.Where(
                                                     pv => pv.TableItemId == x.Id
                                                  && pv.Prop.Tables.Any(t => t.Name == ConstTable.ApartmentTable)
                                                                                     ).Select(pdto => new PropValDTO()
                                                                                     {
                                                                                         Name = pdto.Prop.Name,
                                                                                         Type = pdto.Prop.DataType,
                                                                                         StrValue = pdto.StrValue,
                                                                                         NumValue = pdto.NumValue,
                                                                                         DateValue = pdto.DateValue,
                                                                                         BoolValue = pdto.BoolValue,
                                                                                         CreatedAt = x.CreatedAt,
                                                                                         UpdatedAt = x.UpdatedAt

                                                                                     }
                                                                                     
                                                                                     ).ToList()

            });
           
        }



        // GET tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        // [QueryableExpand("User")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public SingleResult<ApartmentDTO> GetApartment(string id)
        {
            var account = AuthUtils.GetUserAccount(User as ServiceUser);
            var result = Lookup(id).Queryable.Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Price = x.Price,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Rating = x.Rating,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                 PropsVals = context.PropVals.Where(
                                                     pv => pv.TableItemId == x.Id
                                                  && pv.Prop.Tables.Any(t => t.Name == ConstTable.ApartmentTable)
                                                                                     ).Select(pdto => new PropValDTO()
                                                                                     {
                                                                                         Id = pdto.Id,
                                                                                         Name = pdto.Prop.Name,
                                                                                         PropId = pdto.PropId,
                                                                                         Type = pdto.Prop.DataType,
                                                                                         StrValue = pdto.StrValue,
                                                                                         NumValue = pdto.NumValue,
                                                                                         DateValue = pdto.DateValue,
                                                                                         BoolValue = pdto.BoolValue,
                                                                                         DictionaryItemId = pdto.DictionaryItemId

                                                                                     }
                                                                                     
                                                                                     ).ToList()
                
            });
            return SingleResult.Create(result);
        }


        // // PATCH tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //[AuthorizeLevel(AuthorizationLevel.User)]
        //public Task<Apartment> PatchApartment(string id, Delta<Apartment> patch)
        //{
        //    return UpdateAsync(id, patch);
        //}

        //// POST tables/Apartment
        //[AuthorizeLevel(AuthorizationLevel.User)]
        //public async Task<IHttpActionResult> PostApartment(Apartment item)
        //{
        //    Apartment current = await InsertAsync(item);
        //    return CreatedAtRoute("Tables", new { id = current.Id }, current);
        //}

        //// DELETE tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        //[AuthorizeLevel(AuthorizationLevel.User)]
        //public Task DeleteApartment(string id)
        //{
        //     return DeleteAsync(id);
        //}

    }
}