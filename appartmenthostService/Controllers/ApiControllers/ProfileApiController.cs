using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ProfileApiController : ApiController
    {
        private readonly apartmenthostContext _context = new apartmenthostContext();
        public ApiServices Services { get; set; }
        //PUT api/Profile/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/Profile")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutCurrentUser(UserDTO profile)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Profile is not NULL 
                if (profile == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NULL));

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check Profile User
                if (profile.Id != account.UserId)
                {
                    respList.Add(profile.Id);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_USER, respList));
                }

                // Check Current Profile is not NULL
                var profileCurrent = _context.Profile.SingleOrDefault(a => a.Id == account.UserId);
                if (profileCurrent == null)
                {
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check FirstName is not NULL
                resp = CheckHelper.IsNull(profile.FirstName, "FirstName", RespH.SRV_USER_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check LastName is not NULL
                resp = CheckHelper.IsNull(profile.LastName, "LastName", RespH.SRV_USER_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Phone is not NULL
                resp = CheckHelper.IsNull(profile.Phone, "Phone", RespH.SRV_USER_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Genderis not NULL
                resp = CheckHelper.IsNull(profile.Gender, "Gender", RespH.SRV_USER_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                profileCurrent.FirstName = profile.FirstName;
                profileCurrent.LastName = profile.LastName;
                profileCurrent.Gender = profile.Gender;
                profileCurrent.Birthday = profile.Birthday;
                profileCurrent.Phone = profile.Phone;
                profileCurrent.ContactEmail = profile.ContactEmail;
                profileCurrent.ContactKind = profile.ContactKind;
                profileCurrent.Description = profile.Description;

                _context.SaveChanges();

                respList.Add(profileCurrent.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        //PUT api/UpdateRating/
        [Route("api/UpdateRating")]
        [HttpPost]
        public HttpResponseMessage UpdateRating()
        {
            var profiles = _context.Profile.ToList();
            foreach (var profile in profiles)
            {
                var reviews = _context.Reviews.Where(rev => rev.ToUserId == profile.Id && rev.Rating > 0);
                var count = reviews.Count();
                if (count > 0)
                {
                    profile.RatingCount = count;
                    profile.Rating = reviews.Average(x => x.Rating);
                    profile.Score = reviews.Sum(x => x.Rating);
                }
            }


            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}