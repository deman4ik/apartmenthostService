using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Helpers;
using apartmenthostService.Migrations;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    /* TODO: DEPRECATE TEST ONLY */

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
                TestDbPopulator testDbPopulator = new TestDbPopulator(_context);
                switch (method)
                {
                    case "PopulateArticles":
                        testDbPopulator.PopulateArticles();
                        break;
                    case "PopulateProfiles":
                        testDbPopulator.PopulateProfiles();
                        break;
                    case "PopulateProfilePic":
                        testDbPopulator.PopulateProfilePic();
                        break;
                    case "PopulateApartments":
                        testDbPopulator.PopulateApartments();
                        break;
                    case "PopulateApartmentPics":
                        testDbPopulator.PopulateApartmentPics();
                        break;
                    case "PopulateCards":
                        testDbPopulator.PopulateCards();
                        break;
                    case "PopulateCardDates":
                        testDbPopulator.PopulateCardDates();
                        break;
                    case "PopulateCardGenders":
                        testDbPopulator.PopulateCardGenders();
                        break;
                    case "PopulateFavorites":
                        testDbPopulator.PopulateFavorites();
                        break;
                    case "PopulateReservations":
                        testDbPopulator.PopulateReservations();
                        break;
                    case "PopulateReviews":
                        testDbPopulator.PopulateReviews();
                        break;
                    case "PopulateNotifications":
                        testDbPopulator.PopulateNotifications();
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