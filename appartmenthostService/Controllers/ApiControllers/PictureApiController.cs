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
using CloudinaryDotNet;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Account = CloudinaryDotNet.Account;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class PictureApiController : ApiController
    {
        private readonly apartmenthostContext _context = new apartmenthostContext();
        public ApiServices Services { get; set; }
        // POST api/Picture/Upload/Profile/
        [Route("api/Picture/Upload/Profile/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage UploadProfile(string id, PictureDTO picture)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Picture is not NULL 
                if (picture == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_PICTURE_NULL));

                // Check Picture name is not NULL
                resp = CheckHelper.IsNull(picture.Name, "Name", RespH.SRV_PICTURE_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Picture CloudinaryPublicId is not NULL
                resp = CheckHelper.IsNull(picture.CloudinaryPublicId, "CloudinaryPublicId", RespH.SRV_PICTURE_REQUIRED);
                if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Current Profile is not NULL
                var profile = _context.Profile.SingleOrDefault(a => a.Id == id);
                if (profile == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
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

                // Check Profile User
                if (profile.Id != account.UserId)
                {
                    respList.Add(profile.Id);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_USER, respList));
                }

                var pictureGuid = Guid.NewGuid().ToString();
                profile.Picture = new Picture
                {
                    Id = pictureGuid,
                    Name = picture.Name,
                    Description = picture.Description,
                    Url = CloudinaryHelper.Cloudinary.Api.UrlImgUp.BuildUrl(picture.Name),
                    Small =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(34).Height(34).Crop("thumb")).BuildUrl(picture.Name),
                    Mid =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(62).Height(62).Crop("thumb")).BuildUrl(picture.Name),
                    Large =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(76).Height(76).Crop("thumb")).BuildUrl(picture.Name),
                    Xlarge =
                        CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                            new Transformation().Width(96).Height(96).Crop("thumb")).BuildUrl(picture.Name),
                    Default = true
                };

                _context.SaveChanges();

                respList.Add(pictureGuid);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        // POST api/Picture/Upload/Apartment/
        [Route("api/Picture/Upload/Apartment/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage UploadApartment(string id, List<PictureDTO> pictures)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp;

                // Check Pictures is not NULL 
                if (pictures == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_PICTURE_NULL));


                // Check Apartment is not NULL
                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == id);
                if (apartment == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
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

                // Check Apartemtn User
                if (apartment.UserId != account.UserId)
                {
                    respList.Add(apartment.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_WRONG_USER, respList));
                }

                foreach (var picture in pictures)
                {
                    // Check Picture name is not NULL
                    resp = CheckHelper.IsNull(picture.Name, "Name", RespH.SRV_PICTURE_REQUIRED);
                    if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                    // Check Picture CloudinaryPublicId is not NULL
                    resp = CheckHelper.IsNull(picture.CloudinaryPublicId, "CloudinaryPublicId",
                        RespH.SRV_PICTURE_REQUIRED);
                    if (resp != null) return Request.CreateResponse(HttpStatusCode.BadRequest, resp);
                }

                foreach (var picture in pictures)
                {
                    var pictureGuid = Guid.NewGuid().ToString();
                    var pic = new Picture
                    {
                        Id = pictureGuid,
                        Name = picture.Name,
                        Description = picture.Description,
                        Url = CloudinaryHelper.Cloudinary.Api.UrlImgUp.BuildUrl(picture.Name),
                        Xsmall =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(143).Crop("thumb")).BuildUrl(picture.Name),
                        Small =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(190).Crop("thumb")).BuildUrl(picture.Name),
                        Mid =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Height(225).Width(370).Crop("fill")).BuildUrl(picture.Name),
                        Large =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(552).Crop("limit")).BuildUrl(picture.Name),
                        Xlarge =
                            CloudinaryHelper.Cloudinary.Api.UrlImgUp.Transform(
                                new Transformation().Width(1024).Crop("limit")).BuildUrl(picture.Name),
                        Default = picture.Default ?? false
                    };
                    _context.Set<Picture>().Add(pic);

                    _context.SaveChanges();
                    apartment.Pictures.Add(pic);
                    _context.SaveChanges();
                    respList.Add(pictureGuid);
                }
                var defaultPic = _context.Pictures.SingleOrDefault(x => x.Default && x.Apartments.Any(a => a.Id == apartment.Id));
                if (defaultPic == null)
                {
                    var pic = _context.Pictures.SingleOrDefault(x => x.Id == apartment.Pictures.First().Id);
                    pic.Default = true;
                    _context.SaveChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        //POST api/Picture/Default
        [Route("api/Picture/Default/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
        public HttpResponseMessage SetDefault(string id)
        {
            try
            {
                var respList = new List<string>();
                var pic = _context.Pictures.SingleOrDefault(x => x.Id == id);
                if (pic == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_PICTURE_NOTFOUND, respList));
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
                var pics =
                    _context.Pictures.Where(
                        x => x.Apartments.Any(a => a.UserId == account.UserId && a.Pictures.Any(p => p.Id == id)));
                if (pics.Any())
                {
                    foreach (var other in pics.Where(x => x.Id != id && x.Default))
                    {
                        other.Default = false;
                    }
                    pic.Default = true;
                }
                else
                {
                    if (_context.Profile.Any(x => x.Picture.Id == id && x.Id == account.UserId))
                    {
                        pic.Default = true;
                    }
                }
                if (pic.Default)
                {
                    _context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED));
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_WRONG_USER));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        // POST api/Picture/Delete
        [Route("api/Picture/Delete/Profile/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpDelete]
        public HttpResponseMessage DeleteProfile(string id)
        {
            try
            {
                var respList = new List<string>();
                // Check Current Profile is not NULL
                var profile = _context.Profile.SingleOrDefault(a => a.Id == id);
                if (profile == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
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

                // Check Profile User
                if (profile.Id != account.UserId)
                {
                    respList.Add(profile.Id);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_USER, respList));
                }
                string cloudname;
                string apikey;
                string apisecret;
                if (!(Services.Settings.TryGetValue("CLOUDINARY_CLOUD_NAME", out cloudname) |
                      Services.Settings.TryGetValue("CLOUDINARY_API_KEY", out apikey) |
                      Services.Settings.TryGetValue("CLOUDINARY_API_SECRET", out apisecret)))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_PICTURE_BAD_CLOUDINARY_CRED));
                }

                var clacc = new Account(
                    cloudname,
                    apikey,
                    apisecret);
                var cloudinary = new Cloudinary(clacc);
                cloudinary.DeleteResources(profile.Picture.CloudinaryPublicId);
                var picture = _context.Pictures.SingleOrDefault(x => x.Id == profile.PictureId);
                profile.Picture = null;
                _context.SaveChanges();
                _context.Pictures.Remove(picture);
                _context.SaveChanges();

                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        // POST api/Picture/Delete
        [Route("api/Picture/Delete/Apartment/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpDelete]
        public HttpResponseMessage DeleteApartment(string id, List<string> picIds)
        {
            try
            {
                var respList = new List<string>();
                // Check Current Profile is not NULL
                var apartment = _context.Apartments.SingleOrDefault(a => a.Id == id);
                if (apartment == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_NOTFOUND, respList));
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

                // Check Apartemtn User
                if (apartment.UserId != account.UserId)
                {
                    respList.Add(apartment.UserId);
                    respList.Add(account.UserId);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_APARTMENT_WRONG_USER, respList));
                }
                string cloudname;
                string apikey;
                string apisecret;
                if (!(Services.Settings.TryGetValue("CLOUDINARY_CLOUD_NAME", out cloudname) |
                      Services.Settings.TryGetValue("CLOUDINARY_API_KEY", out apikey) |
                      Services.Settings.TryGetValue("CLOUDINARY_API_SECRET", out apisecret)))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_PICTURE_BAD_CLOUDINARY_CRED));
                }

                var clacc = new Account(
                    cloudname,
                    apikey,
                    apisecret);
                var cloudinary = new Cloudinary(clacc);
                foreach (var picId in picIds)
                {
                    var pic = _context.Pictures.SingleOrDefault(p => p.Id == picId);
                    if (pic != null)
                    {
                        cloudinary.DeleteResources(pic.CloudinaryPublicId);
                        apartment.Pictures.Remove(pic);
                        _context.SaveChanges();
                        _context.Pictures.Remove(pic);
                        _context.SaveChanges();
                    }
                }
                if (apartment.Pictures.Any())
                {
                    var defaultPic = apartment.Pictures.SingleOrDefault(x => x.Default);
                    if (defaultPic == null)
                    {
                        var pic = _context.Pictures.SingleOrDefault(x => x.Id == apartment.Pictures.First().Id);
                        pic.Default = true;
                        _context.SaveChanges();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.InnerException.ToString()}));
            }
        }

        public void SetDefaultPicture(string cardId)
        {
        }
    }
}