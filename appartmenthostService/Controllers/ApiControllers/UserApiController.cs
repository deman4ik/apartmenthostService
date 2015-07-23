using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.Http;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using System.Net.Http;

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
        public HttpResponseMessage GetCurrentUser()
        {
            try
            {
                var currentUser = User as ServiceUser;
                if (currentUser == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NULL));
                var account = _context.Accounts.SingleOrDefault(a => a.AccountId == currentUser.Id);
                if (account == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND));
                var result = _context.Profile.Where(p => p.Id == account.UserId).Select(x => new UserDTO()
                {
                    Id = x.Id,
                    Email = x.User.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Gender = x.Gender,
                    Birthday = x.Birthday,
                    Phone = x.Phone,
                    ContactEmail = x.ContactEmail,
                    ContactKind = x.ContactKind,
                    Description = x.Description,
                    Rating = x.Rating,
                    RatingCount = x.RatingCount,
                    Score = x.Score,
                    CardCount = _context.Cards.Count(c => c.UserId == x.Id),
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    Picture = new PictureDTO()
                    {
                        Id = x.Picture.Id,
                        Name = x.Picture.Name,
                        Description = x.Picture.Description,
                        Url = x.Picture.Url,
                        Xsmall = x.Picture.Xsmall,
                        Small = x.Picture.Small,
                        Mid = x.Picture.Mid,
                        Large = x.Picture.Large,
                        Xlarge = x.Picture.Xlarge,
                        Default = x.Picture.Default,
                        CreatedAt = x.Picture.CreatedAt
                    }


                });
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.ToString() }));
            }
            
        }

        

    }
}
