﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Web.Http;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Messages;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ProfileApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        public ProfileApiController()
        {
        }

        public ProfileApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        // GET api/Profile/{id}
        [Route("api/Profile/{userId}")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpGet]
        public HttpResponseMessage GetProfile(string userId)
        {
            try
            {
                var profile = _context.Profile.AsNoTracking().Where(p => p.Id == userId).Select(x => new UserDTO
                {
                    Id = x.Id,
                    Email = x.User.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Gender = x.Gender,
                    Birthday = x.Birthday,
                    Phone = x.Phone,
                    PhoneStatus = x.User.PhoneStatus,
                    Description = x.Description,
                    Rating = x.Rating,
                    RatingCount = x.RatingCount,
                    Score = x.Score,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,
                    EmailNewsletter = x.User.EmailNewsletter,
                    EmailNotifications = x.User.EmailNotifications,
                    Picture = _context.Pictures.Where(pic => pic.Id == x.PictureId).Select( p => new PictureDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Url = p.Url,
                        Xsmall = p.Xsmall,
                        Small = p.Small,
                        Mid = p.Mid,
                        Large = p.Large,
                        Xlarge = p.Xlarge,
                        Default = p.Default,
                        CreatedAt = p.CreatedAt
                    }).FirstOrDefault()
                }).FirstOrDefault();
                if (profile != null)
                {
                    profile.CardCount = _context.Cards.AsNoTracking().Count(c => c.UserId == profile.Id);
                    if (profile.CardCount > 0)
                    {
                        profile.Cards =
                            _context.Cards.Include("Genders")
                                .Include("Dates")
                                .Include("Apartment")
                                .AsNoTracking()
                                .Select(c => new CardDTO
                                {
                                    Name = c.Name,
                                    UserId = c.UserId,
                                    Description = c.Description,
                                    ApartmentId = c.ApartmentId,
                                    PriceDay = c.Genders.Min(ge => ge.Price),
                                    PriceGender =
                                        c.Genders.FirstOrDefault(gn => gn.Price == c.Genders.Min(ge => ge.Price)).Name,
                                    Cohabitation = c.Cohabitation,
                                    Lang = c.Lang,
                                    Dates = _context.Dates.Where(date => date.CardId == c.Id).Select(d => new DatesDTO
                                    {
                                        DateFrom = d.DateFrom,
                                        DateTo = d.DateTo
                                    }
                                        ).ToList(),
                                    Apartment = _context.Apartments.Where(a => a.Id == c.ApartmentId).Select(ap => new ApartmentDTO
                                    {
                                        Id = ap.Id,
                                        Name = ap.Name,
                                        Type = ap.Type,
                                        Options = ap.Options,
                                        UserId = ap.UserId,
                                        Adress = ap.Adress,
                                        FormattedAdress = ap.FormattedAdress,
                                        Latitude = ap.Latitude,
                                        Longitude = ap.Longitude,
                                        PlaceId = ap.PlaceId,
                                        Pictures = _context.Pictures.Where(pic => pic.Apartments.Contains(c.Apartment)).Select(apic => new PictureDTO
                                        {
                                            Id = apic.Id,
                                            Name = apic.Name,
                                            Description = apic.Description,
                                            Url = apic.Url,
                                            Xsmall = apic.Xsmall,
                                            Small = apic.Small,
                                            Mid = apic.Mid,
                                            Large = apic.Large,
                                            Xlarge = apic.Xlarge,
                                            Default = apic.Default
                                        }).ToList()
                                    }).FirstOrDefault()
                                }).ToList();
                    }


                    profile.Reviews =
                        _context.Reviews.AsNoTracking()
                            .Where(r => r.ToUserId == profile.Id)
                            .Select(owrev => new ReviewDTO
                            {
                                Id = owrev.Id,
                                FromUserId = owrev.FromUserId,
                                ToUserId = owrev.ToUserId,
                                ReservationId = owrev.ReservationId,
                                Rating = owrev.Rating,
                                Text = owrev.Text,
                                Type =
                                    _context.Reservations.FirstOrDefault(res => res.Id == owrev.ReservationId)
                                        .UserId ==
                                    userId
                                        ? ConstVals.Renter
                                        : ConstVals.Owner,
                                CreatedAt = owrev.CreatedAt,
                                UpdatedAt = owrev.UpdatedAt,
                                FromUser = new BaseUserDTO
                                {
                                    Id = owrev.FromUser.Profile.Id,
                                    Email = owrev.FromUser.Email,
                                    FirstName = owrev.FromUser.Profile.FirstName,
                                    LastName = owrev.FromUser.Profile.LastName,
                                    Rating = owrev.FromUser.Profile.Rating,
                                    RatingCount = owrev.FromUser.Profile.RatingCount,
                                    Gender = owrev.FromUser.Profile.Gender,
                                    Picture = new PictureDTO
                                    {
                                        Id = owrev.FromUser.Profile.Picture.Id,
                                        Name = owrev.FromUser.Profile.Picture.Name,
                                        Description = owrev.FromUser.Profile.Picture.Description,
                                        Url = owrev.FromUser.Profile.Picture.Url,
                                        Xsmall = owrev.FromUser.Profile.Picture.Xsmall,
                                        Small = owrev.FromUser.Profile.Picture.Small,
                                        Mid = owrev.FromUser.Profile.Picture.Mid,
                                        Large = owrev.FromUser.Profile.Picture.Large,
                                        Xlarge = owrev.FromUser.Profile.Picture.Xlarge,
                                        Default = owrev.FromUser.Profile.Picture.Default,
                                        CreatedAt = owrev.FromUser.Profile.Picture.CreatedAt
                                    }
                                }
                            }).OrderByDescending(r => r.CreatedAt).ToList();
                }
                List<UserDTO> result = new List<UserDTO> {profile};
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

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

                var user = _context.Users.AsNoTracking().SingleOrDefault(u => u.Id == account.UserId);
                if (user == null)
                {
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                // Check Current Profile is not NULL
                var profileCurrent = _context.Profile.SingleOrDefault(a => a.Id == account.UserId);
                if (profileCurrent == null)
                {
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                if (!string.IsNullOrWhiteSpace(profile.Phone))
                {
                    profile.Phone = CheckHelper.CleanPhone(profile.Phone);
                    if (profile.Phone[0] == '7')
                    {
                        if (profile.Phone.Length != 11)
                        {
                            respList.Add(profile.Phone);
                            return Request.CreateResponse(HttpStatusCode.BadRequest,
                                RespH.Create(RespH.SRV_PROFILE_WRONG_PHONE, respList));
                        }
                    }
                    if (profileCurrent.Phone != profile.Phone)
                    {
                        var hasCards = _context.Cards.Any(x => x.UserId == user.Id);
                        if (hasCards)
                        {
                            respList.Add(user.Id);
                            return Request.CreateResponse(HttpStatusCode.BadRequest,
                                RespH.Create(RespH.SRV_PROFILE_ERR_UPDATE_PHONE, respList));
                        }
                        user.PhoneStatus = ConstVals.PUnconf;
                        _context.MarkAsModified(user);
                    }
                }
                else
                {
                    var hasCards = _context.Cards.Any(x => x.UserId == user.Id);
                    if (hasCards)
                    {
                        respList.Add(user.Id);
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_PROFILE_ERR_UPDATE_PHONE, respList));
                    }
                    user.PhoneStatus = ConstVals.PUnconf;
                    _context.MarkAsModified(user);
                }
                // Check FirstName is not NULL
                resp = CheckHelper.IsNull(profile.FirstName, "FirstName", RespH.SRV_USER_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check LastName is not NULL
                //resp = CheckHelper.IsNull(profile.LastName, "LastName", RespH.SRV_USER_REQUIRED);
                //if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Phone is not NULL
                //resp = CheckHelper.IsNull(profile.Phone, "Phone", RespH.SRV_USER_REQUIRED);
                //if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Genderis not NULL
                //resp = CheckHelper.IsNull(profile.Gender, "Gender", RespH.SRV_USER_REQUIRED);
                //if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                profileCurrent.FirstName = profile.FirstName;
                profileCurrent.LastName = profile.LastName;
                profileCurrent.Gender = profile.Gender;
                profileCurrent.Birthday = profile.Birthday;
                profileCurrent.Phone = profile.Phone;
                profileCurrent.Description = profile.Description;
                user.EmailNotifications = profile.EmailNotifications;
                user.EmailNewsletter = profile.EmailNewsletter;
                _context.MarkAsModified(user);
                _context.MarkAsModified(profileCurrent);
                _context.SaveChanges();

                respList.Add(profileCurrent.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        [Route("api/Profile/Email")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage SetEmail(UserDTO userdata)
        {
            try
            {
                var respList = new List<string>();

                if (string.IsNullOrWhiteSpace(userdata.Email))
                {
                    respList.Add("Email");
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_REQUIRED, respList));
                }

                if (!AuthUtils.IsEmailValid(userdata.Email))
                {
                    respList.Add(userdata.Email);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_INVALID_EMAIL, respList));
                }
                var usersame = _context.Users.AsNoTracking().SingleOrDefault(a => a.Email == userdata.Email);
                if (usersame != null)
                {
                    respList.Add(userdata.Email);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_REG_EXISTS_EMAIL, respList));
                }
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
                var user = _context.Users.SingleOrDefault(x => x.Id == account.UserId);
                var profile = _context.Profile.SingleOrDefault(x => x.Id == account.UserId);
                if (user == null || profile == null)
                {
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                if (!string.IsNullOrWhiteSpace(user.Email))
                {
                    respList.Add(user.Email);
                    return Request.CreateResponse(HttpStatusCode.Unauthorized,
                        RespH.Create(RespH.SRV_USER_EXISTS, respList));
                }

                var salt = AuthUtils.GenerateSalt();
                var confirmCode = AuthUtils.RandomNumString(6);
                user.Email = userdata.Email;
                user.Salt = salt;
                user.SaltedAndHashedEmail = AuthUtils.Hash(confirmCode, salt);
                _context.MarkAsModified(user);
                _context.SaveChanges();
                using (MailSender mailSender = new MailSender())
                {
                    var bem = new BaseEmailMessage
                    {
                        Code = ConstVals.Reg,
                        ToUserId = user.Id,
                        ToUserEmail = user.Email,
                        ToUserName = profile.FirstName,
                        ConfirmCode = confirmCode
                    };
                    mailSender.Create(_context, bem);
                }


                respList.Add(user.Id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
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
                    _context.MarkAsModified(profile);
                }
            }


            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}