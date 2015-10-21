using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.DataObjects;
using apartmenthostService.Migrations;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class SeedDbController : ApiController
    {
        private readonly ApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }
        // GET api/SeedDb
        [HttpPost]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        public HttpResponseMessage Post(string method)
        {
            try
            {
                switch (method)
                {
                    case "PopulateArticles":
                        TestDBPopulator.PopulateArticles(_context);
                        break;
                    case "PopulateProfiles":
                        TestDBPopulator.PopulateProfiles(_context);
                        break;
                    case "PopulateProfilePic":
                        TestDBPopulator.PopulateProfilePic(_context);
                        break;
                    case "PopulateApartments":
                        TestDBPopulator.PopulateApartments(_context);
                        break;
                    case "PopulateApartmentPics":
                        TestDBPopulator.PopulateApartmentPics(_context);
                        break;
                    case "PopulateCards":
                        TestDBPopulator.PopulateCards(_context);
                        break;
                    case "PopulateCardDates":
                        TestDBPopulator.PopulateCardDates(_context);
                        break;
                    case "PopulateCardGenders":
                        TestDBPopulator.PopulateCardGenders(_context);
                        break;
                    case "PopulateFavorites":
                        TestDBPopulator.PopulateFavorites(_context);
                        break;
                    case "PopulateReservations":
                        TestDBPopulator.PopulateReservations(_context);
                        break;
                    case "PopulateReviews":
                        TestDBPopulator.PopulateReviews(_context);
                        break;
                    case "PopulateNotifications":
                        TestDBPopulator.PopulateNotifications(_context);
                        break;
                }
                _context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }
    }
}