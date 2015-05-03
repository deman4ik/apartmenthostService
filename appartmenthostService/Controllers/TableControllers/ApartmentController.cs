using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using Microsoft.WindowsAzure.Mobile.Service;
using apartmenthostService.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ApartmentController : TableController<Apartment>
    {
       private apartmenthostContext context;
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            this.context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Apartment>(context, Request, Services);
        }

        // GET tables/Apartment
       // [QueryableExpand("User")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<ApartmentDTO> GetAllApartment()
        {
            var currentUser = User as ServiceUser;
            if (currentUser == null) return null;
            var account = AuthUtils.GetUserAccount(currentUser);
            return Query().Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Lang = x.Lang,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                PropsVals = x.PropVals
                        .Select(pdto => new PropValDTO()
                          {
                              Id = pdto.Id,
                              PropId = pdto.PropId,
                            Name = pdto.Prop.Name,
                            Type = pdto.Prop.DataType,
                            StrValue = pdto.StrValue,
                            NumValue = pdto.NumValue,
                            DateValue = pdto.DateValue,
                            BoolValue = pdto.BoolValue,
                            DictionaryItemId = pdto.DictionaryItemId,
                            DictionaryItem = new DictionaryItemDTO()
                            {
                                StrValue = pdto.DictionaryItem.StrValue,
                                NumValue = pdto.DictionaryItem.NumValue,
                                DateValue = pdto.DictionaryItem.DateValue,
                                BoolValue = pdto.DictionaryItem.BoolValue
                            }  
                          }).ToList()

            });
           
        }



        // GET tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        // [QueryableExpand("User")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public SingleResult<ApartmentDTO> GetApartment(string id)
        {
            var currentUser = User as ServiceUser;
            if (currentUser == null) return null;
            var account = AuthUtils.GetUserAccount(currentUser);
            var result = Lookup(id).Queryable.Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                UserId = x.UserId,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Lang = x.Lang,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                PropsVals = x.PropVals
                        .Select(pdto => new PropValDTO()
                        {
                            Id = pdto.Id,
                            PropId = pdto.PropId,
                            Name = pdto.Prop.Name,
                            Type = pdto.Prop.DataType,
                            StrValue = pdto.StrValue,
                            NumValue = pdto.NumValue,
                            DateValue = pdto.DateValue,
                            BoolValue = pdto.BoolValue,
                            DictionaryItemId = pdto.DictionaryItemId,
                            DictionaryItem = new DictionaryItemDTO()
                            {
                                StrValue = pdto.DictionaryItem.StrValue,
                                NumValue = pdto.DictionaryItem.NumValue,
                                DateValue = pdto.DictionaryItem.DateValue,
                                BoolValue = pdto.DictionaryItem.BoolValue
                            }
                        }).ToList()

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