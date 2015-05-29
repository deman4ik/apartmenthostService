using System.Web.Http;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    public class ReviewApiController : ApiController
    {
        public ApiServices Services { get; set; }
        readonly apartmenthostContext _context = new apartmenthostContext();

        // GET api/Review/AsLessor
        [Route("api/Review/AsLessor")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public string GetAsLessorReview()
        {
            return "lessor";
        }

        // GET api/Review/AsRenter
        [Route("api/Review/AsRenter")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public string GetAsRenterReview()
        {
            return "renter";
        }

        // POST api/Review/{revId}
        [Route("api/Review/{revId}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public string PostReview(string revId)
        {
            return revId;
        }
    }
}
