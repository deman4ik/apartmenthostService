using System.Linq;
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

        

    }
}
