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
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{

    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class UserApiController : ApiController
    {

        public ApiServices Services { get; set; }

        readonly apartmenthostContext _context = new apartmenthostContext();

        // GET api/User
        [Route("api/User")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public IQueryable<UserDTO> GetCurrentUser()
        {
            var currentUser = User as ServiceUser;
            if (currentUser == null) return null;
            var account = _context.Accounts.SingleOrDefault(a => a.AccountId == currentUser.Id);
            if (account == null) return null;
            var result = _context.Profile.Where(p => p.Id == account.UserId).Project().To<UserDTO>();
            return result;
        }

        //PUT api/User/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [Route("api/User")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPut]
        public HttpResponseMessage PutCurrentUser(UserDTO profile)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Profile is not NULL 
                if (profile == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NULL));

                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check Profile User
                if (profile.Id != account.UserId)
                {
                    respList.Add(profile.Id);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_USER, respList));
                }

                // Check Current Profile is not NULL
                var profileCurrent = _context.Profile.SingleOrDefault(a => a.Id == currentUser.Id);
                if (profileCurrent == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check FirstName is not NULL
                resp = CheckHelper.isNull(profile.FirstName, "FirstName", RespH.SRV_USER_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check LastName is not NULL
                resp = CheckHelper.isNull(profile.LastName, "LastName", RespH.SRV_USER_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Phone is not NULL
                resp = CheckHelper.isNull(profile.Phone, "Phone", RespH.SRV_USER_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Genderis not NULL
                resp = CheckHelper.isNull(profile.Gender, "Gender", RespH.SRV_USER_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Gender Dictionary
                resp = CheckHelper.isValidDicItem(_context, profile.Gender, ConstDictionary.Gender, "Gender", RespH.SRV_USER_INVALID_DICITEM);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

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
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

    }
}
