﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class UserApiController : ApiController
    {
        private readonly IApartmenthostContext _context;
        public ApiServices Services { get; set; }

        public UserApiController()
        {
            _context = new ApartmenthostContext();
        }

        public UserApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        // GET api/User
        [Route("api/User")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage GetCurrentUser()
        {
            try
            {
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NULL));
                var account = _context.Accounts.AsNoTracking().SingleOrDefault(a => a.AccountId == currentUser.Id);
                if (account == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND));
                var result = _context.Profile.AsNoTracking().Where(p => p.Id == account.UserId).Select(x => new UserDTO
                {
                    Id = x.Id,
                    Email = x.User.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Gender = x.Gender,
                    Birthday = x.Birthday,
                    Phone = x.Phone,
                    Description = x.Description,
                    Rating = x.Rating,
                    RatingCount = x.RatingCount,
                    Score = x.Score,
                    EmailConfirmed = x.User.EmailConfirmed,
                    PhoneStatus = x.User.PhoneStatus,
                    CardCount = _context.Cards.Count(c => c.UserId == x.Id),
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    EmailNotifications = x.User.EmailNotifications,
                    EmailNewsletter = x.User.EmailNewsletter,
                    Picture = new PictureDTO
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
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        [Route("api/User/GetUserInfo")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpGet]
        public HttpResponseMessage GetUserInfo()
        {
            var currentUser = User as ServiceUser;
            var creds = currentUser.GetIdentitiesAsync();
            return Request.CreateResponse(HttpStatusCode.OK, creds);
        }

        [Route("api/DelUser/{email}")]
        [HttpPost]
        [AuthorizeLevel(AuthorizationLevel.Admin)]
        public string DeleteUser(string email)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);
            if (user != null)
            {
                var profile = _context.Profile.SingleOrDefault(x => x.Id == user.Id);
                var accounts = _context.Accounts.Where(x => x.UserId == user.Id);

                if (profile != null)
                    _context.Profile.Remove(profile);

                if (accounts.Any())
                    _context.Accounts.RemoveRange(accounts);
                _context.Users.Remove(user);
                _context.SaveChanges();
                return "Пользователь " + email + " удален";
            }

            return "Нечего удалять";
        }
    }
}