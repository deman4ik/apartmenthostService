using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.DataObjects;
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

        apartmenthostContext context = new apartmenthostContext();

        // GET api/User
        [Route("api/User")]
        [HttpGet]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<UserDTO> GetCurrentUser()
        {
            var currentUser = User as ServiceUser;
            if (currentUser == null) return null;
            var account = context.Accounts.SingleOrDefault(a => a.AccountId == currentUser.Id);
            if (account == null) return null;
            var result = context.Profile.Where(p => p.Id == account.UserId).Project().To<UserDTO>();
            return result;
        }

    }
}
