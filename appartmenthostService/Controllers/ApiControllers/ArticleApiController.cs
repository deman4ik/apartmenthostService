using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apartmenthostService.DataObjects;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using LinqKit;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;
using Newtonsoft.Json;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ArticleApiController : ApiController
    {
        public ApiServices Services { get; set; }
        readonly apartmenthostContext _context = new apartmenthostContext();

        /// <summary>
        /// GET api/Articles/
        /// </summary>
        [Route("api/Articles/")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpGet]
        public HttpResponseMessage GetArticles([FromUri] string filter = null)
        {
            try
            {
                // Создаем предикат
                var pre = PredicateBuilder.True<Article>();
                pre = pre.And(x => x.Deleted == false);
                 // Получаем объект из строки запроса
                if (!string.IsNullOrWhiteSpace(filter))
                {
                    ArticleDTO artRequest = JsonConvert.DeserializeObject<ArticleDTO>(filter);
                    if (artRequest == null)
                        return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_ARTICLE_INVALID_FILTER));

                    if (artRequest.Id != null)
                    {
                        pre = pre.And(x => x.Id == artRequest.Id);
                    }

                    if (artRequest.Name != null)
                    {
                        pre = pre.And(x => x.Name == artRequest.Name);
                    }

                    if (artRequest.Title != null)
                    {
                        pre = pre.And(x => x.Title == artRequest.Title);
                    }

                    if (artRequest.Text != null)
                    {
                        pre = pre.And(x => x.Text.Contains(artRequest.Text));
                    }

                    if (artRequest.Lang != null)
                    {
                        pre = pre.And(x => x.Lang == artRequest.Lang);
                    }
                }

                var result = _context.Article.Where(pre).Select(art => new ArticleDTO()
                {
                    Id = art.Id,
                    Name = art.Name,
                    Title = art.Title,
                    Text = art.Text,
                    Lang = art.Lang,
                    CreatedAt = art.CreatedAt,
                    UpdatedAt = art.UpdatedAt,
                    Picture = new PictureDTO()
                    {
                        Id = art.Picture.Id,
                        Name = art.Picture.Name,
                        Description = art.Picture.Description,
                        Url = art.Picture.Url,
                        Xsmall = art.Picture.Xsmall,
                        Small = art.Picture.Small,
                        Mid = art.Picture.Mid,
                        Large = art.Picture.Large,
                        Xlarge = art.Picture.Xlarge,
                        CloudinaryPublicId = art.Picture.CloudinaryPublicId,
                        Default = art.Picture.Default,
                        CreatedAt = art.Picture.CreatedAt
                    }
                });
                return this.Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

        /// <summary>
        /// POST api/Article/
        /// </summary>
        [Route("api/Article/")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpPost]
        public HttpResponseMessage PostArticle(ArticleDTO article)
        {
            try
            {
                var respList = new List<string>();
                ResponseDTO resp = null;
                if (article == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ARTICLE_NULL));

                if (article.Name == null)

                    resp = CheckHelper.isNull(article.Name, "Name", RespH.SRV_ARTICLE_REQUIRED);
                    if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

                if (article.Title == null && article.Text == null && article.Tag == null)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ARTICLE_REQUIRED));
                }
                string articleGuid = Guid.NewGuid().ToString();
                _context.Set<Article>().Add(
                    new Article()
                    {
                        Id = articleGuid,
                        Name = article.Name,
                        Title = article.Title,
                        Text = article.Text,
                        Lang = article.Lang,
                        Tag = article.Tag
                    });

                _context.SaveChanges();

                respList.Add(articleGuid);
                return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.InnerException);
                return this.Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
            }
        }

        ///// <summary>
        ///// PUT api/Article/
        ///// </summary>
        //[Route("api/Article/")]
        //[AuthorizeLevel(AuthorizationLevel.Anonymous)]
        //[HttpPost]
        //public HttpResponseMessage PutArticle(ArticleDTO article)
        //{
        //    try
        //    {
        //        var respList = new List<string>();
        //        ResponseDTO resp = null;
        //        if (article == null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ARTICLE_NULL));

        //        if (article.Name == null)

        //            resp = CheckHelper.isNull(article.Name, "Name", RespH.SRV_ARTICLE_REQUIRED);
        //        if (resp != null) return this.Request.CreateResponse(HttpStatusCode.BadRequest, resp);

        //        if (article.Title == null && article.Text == null && article.Tag == null)
        //        {
        //            return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ARTICLE_REQUIRED));
        //        }

        //        var currentArticle = _context.Article.SingleOrDefault(x => x.Id == article.Id);
        //        if (currentArticle == null)
        //            return this.Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ARTICLE_REQUIRED));
        //        string articleGuid = Guid.NewGuid().ToString();
        //        _context.Set<Article>().Add(
        //            new Article()
        //            {
        //                Id = articleGuid,
        //                Name = article.Name,
        //                Title = article.Title,
        //                Text = article.Text,
        //                Tag = article.Tag
        //            });

        //        _context.SaveChanges();

        //        respList.Add(articleGuid);
        //        return this.Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
        //    }
        //    catch (Exception ex)
        //    {

        //        Debug.WriteLine(ex.InnerException);
        //        return this.Request.CreateResponse(HttpStatusCode.BadRequest,
        //            RespH.Create(RespH.SRV_EXCEPTION, new List<string>() { ex.InnerException.ToString() }));
        //    }
        //}
    }
}
