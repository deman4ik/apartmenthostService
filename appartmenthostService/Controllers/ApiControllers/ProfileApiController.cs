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
                var result = _context.Profile.Where(p => p.Id == userId).Select(x => new UserDTO
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
                    },
                    Cards = x.User.Cards.Select(c => new CardDTO
                    {
                        Name = c.Name,
                        UserId = c.UserId,
                        Description = c.Description,
                        ApartmentId = c.ApartmentId,
                        PriceDay = c.Genders.Min(ge => ge.Price),
                        PriceGender = c.Genders.FirstOrDefault(gn => gn.Price == c.Genders.Min(ge => ge.Price)).Name,
                        Cohabitation = c.Cohabitation,
                        Lang = c.Lang,
                        Dates = c.Dates.Select(d => new DatesDTO
                        {
                            DateFrom = d.DateFrom,
                            DateTo = d.DateTo
                        }
                            ).ToList(),
                        Apartment = new ApartmentDTO
                        {
                            Id = c.Apartment.Id,
                            Name = c.Apartment.Name,
                            Type = c.Apartment.Type,
                            Options = c.Apartment.Options,
                            UserId = c.Apartment.UserId,
                            Adress = c.Apartment.Adress,
                            FormattedAdress = c.Apartment.FormattedAdress,
                            Latitude = c.Apartment.Latitude,
                            Longitude = c.Apartment.Longitude,
                            PlaceId = c.Apartment.PlaceId,
                            Pictures = c.Apartment.Pictures.Select(apic => new PictureDTO
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
                                Default = apic.Default,
                                CreatedAt = apic.CreatedAt
                            }).ToList()
                        }
                    }).ToList(),
                    Reviews = x.User.InReviews.Select(owrev => new ReviewDTO
                    {
                        Id = owrev.Id,
                        FromUserId = owrev.FromUserId,
                        ToUserId = owrev.ToUserId,
                        ReservationId = owrev.ReservationId,
                        Rating = owrev.Rating,
                        Text = owrev.Text,
                        Type =
                            _context.Reservations.FirstOrDefault(res => res.Id == owrev.ReservationId).UserId == userId
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
                    }).OrderByDescending(r => r.CreatedAt).ToList()
                });
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
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

                var user = _context.Users.SingleOrDefault(u => u.Id == account.UserId);
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


                // Check FirstName is not NULL
                //resp = CheckHelper.IsNull(profile.FirstName, "FirstName", RespH.SRV_USER_REQUIRED);
                //if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

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

                _context.MarkAsModified(profileCurrent);
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
                    _context.MarkAsModified(profile);
                }
            }


            _context.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}