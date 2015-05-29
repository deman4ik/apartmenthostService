using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    public class ReviewApiController : ApiController
    {
        public ApiServices Services { get; set; }
        readonly apartmenthostContext _context = new apartmenthostContext();

        // GET api/Reviews/{type}
        [Route("api/Reviews/{type}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage GetReviews(string type)
        {
            var respList = new List<string>();
            return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
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
