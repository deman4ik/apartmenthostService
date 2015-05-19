﻿using System;
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
            var account = AuthUtils.GetUserAccount(currentUser);
            if (account == null) return null;
            return Query().Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Options = x.Options,
                UserId = x.UserId,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Lang = x.Lang,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            });
           
        }



        // GET tables/Apartment/48D68C86-6EA6-4C25-AA33-223FC9A27959
        // [QueryableExpand("User")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public SingleResult<ApartmentDTO> GetApartment(string id)
        {
            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(currentUser);
            if (account == null) return null;
            var result = Lookup(id).Queryable.Where(a => a.UserId == account.UserId).Select(x => new ApartmentDTO()
            {
                Id = x.Id,
                Name = x.Name,
                Type = x.Type,
                Options = x.Options,
                UserId = x.UserId,
                Adress = x.Adress,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                Lang = x.Lang,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt

            });
            return SingleResult.Create(result);
        }

    }
}