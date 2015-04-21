using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using appartmenthostService.DataObjects;
using appartmenthostService.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace appartmenthostService.Controllers
{

    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class UserApiController : ApiController
    {

        public ApiServices Services { get; set; }

        appartmenthostContext context = new appartmenthostContext();

        // GET api/User
        [Route("api/User")]
        [HttpGet]
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<UserDTO> GetCurrentUser()
        {
            var currentUser = User as ServiceUser;
            var account = context.Accounts.SingleOrDefault(a => a.AccountId == currentUser.Id);
            var result = context.Profile.Where(p => p.Id == account.UserId).Project().To<UserDTO>();
            return result;
        }

    }
}
