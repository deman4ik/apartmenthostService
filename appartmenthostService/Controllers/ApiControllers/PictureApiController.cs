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
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class PictureApiController : ApiController
    {
        public ApiServices Services { get; set; }
        readonly apartmenthostContext _context = new apartmenthostContext();

        // POST api/Picture/Upload
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
                if (picture == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_PICTURE_NULL));

                // Check Picture name is not NULL
                resp = CheckHelper.isNull(picture.Name, "Name", RespH.SRV_PICTURE_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Picture CloudinaryPublicId is not NULL
                resp = CheckHelper.isNull(picture.CloudinaryPublicId, "CloudinaryPublicId", RespH.SRV_PICTURE_REQUIRED);
                if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                // Check Current Profile is not NULL
                var profile = _context.Profile.SingleOrDefault(a => a.Id == id);
                if (profile == null)
                {
                    respList.Add(id);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check Profile User
                if (profile.Id != account.UserId)
                {
                    respList.Add(profile.Id);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_USER, respList));
                }
                string cloudname;
                string apikey;
                string apisecret; 
                if (!(Services.Settings.TryGetValue("CLOUDINARY_CLOUD_NAME", out cloudname) |
                Services.Settings.TryGetValue("CLOUDINARY_API_KEY", out apikey) | Services.Settings.TryGetValue("CLOUDINARY_API_SECRET", out apisecret)))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_PICTURE_BAD_CLOUDINARY_CRED));
                }

                CloudinaryDotNet.Account clacc = new CloudinaryDotNet.Account(
                                                                            cloudname,
                                                                            apikey,
                                                                            apisecret);
                Cloudinary cloudinary = new Cloudinary(clacc);

                string pictureGuid = Guid.NewGuid().ToString();
                profile.Picture = new Picture()
                {
                    Id = pictureGuid,
                    Name = picture.Name,
                    Description = picture.Description,
                    Url = cloudinary.Api.UrlImgUp.BuildUrl(picture.Name),
                    Default = true
                };

                _context.SaveChanges();

                respList.Add(pictureGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }

        }


        // POST api/Picture/Delete
        [Route("api/Picture/Delete/Profile/{id}")]
        [AuthorizeLevel(AuthorizationLevel.User)]
        [HttpPost]
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
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }
                // Check Current User
                var currentUser = User as ServiceUser;
                if (currentUser == null)
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_UNAUTH));
                var account = AuthUtils.GetUserAccount(_context, currentUser);
                if (account == null)
                {
                    respList.Add(currentUser.Id);
                    return this.Request.CreateResponse(HttpStatusCode.Unauthorized, RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                }

                // Check Profile User
                if (profile.Id != account.UserId)
                {
                    respList.Add(profile.Id);
                    respList.Add(account.UserId);
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_USER_WRONG_USER, respList));
                }
                string cloudname;
                string apikey;
                string apisecret;
                if (!(Services.Settings.TryGetValue("CLOUDINARY_CLOUD_NAME", out cloudname) |
                Services.Settings.TryGetValue("CLOUDINARY_API_KEY", out apikey) | Services.Settings.TryGetValue("CLOUDINARY_API_SECRET", out apisecret)))
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_PICTURE_BAD_CLOUDINARY_CRED));
                }

                CloudinaryDotNet.Account clacc = new CloudinaryDotNet.Account(
                                                                            cloudname,
                                                                            apikey,
                                                                            apisecret);
                Cloudinary cloudinary = new Cloudinary(clacc);
                cloudinary.DeleteResources(profile.Picture.CloudinaryPublicId);
                var picture = _context.Pictures.SingleOrDefault(x => x.Id == profile.PictureId);
                profile.Picture = null;
                _context.SaveChanges();
                _context.Pictures.Remove(picture);
                _context.SaveChanges();

                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

    }
}
