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
    /* TODO: ПРОВЕРКА ПРАВ ДОСТУПА */

    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class ArticleApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        public ArticleApiController()
        {
        }

        public ArticleApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     GET api/Articles/
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
                if (!string.IsNullOrWhiteSpace(filter) && filter != "{}" && filter != "{filter}")
                {
                    var artRequest = JsonConvert.DeserializeObject<ArticleDTO>(filter);
                    if (artRequest == null)
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
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

                    if (artRequest.Type != null)
                    {
                        pre = pre.And(x => x.Type == artRequest.Type);
                    }

                    if (artRequest.Lang != null)
                    {
                        pre = pre.And(x => x.Lang == artRequest.Lang);
                    }
                }

                var result = _context.Article.AsExpandable().Where(pre).Select(art => new ArticleDTO
                {
                    Id = art.Id,
                    Name = art.Name,
                    Title = art.Title,
                    Text = art.Text,
                    Type = art.Type,
                    Lang = art.Lang,
                    CreatedAt = art.CreatedAt,
                    UpdatedAt = art.UpdatedAt
                }).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        /// <summary>
        ///     POST api/Article/
        /// </summary>
        [Route("api/Article/")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpPost]
        public HttpResponseMessage PostArticle(ArticleDTO article)
        {
            try
            {
                var respList = new List<string>();
                if (article == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ARTICLE_NULL));


                if (article.Name == null)
                {
                    respList.Add("Name");
                }
                if (article.Title == null)
                {
                    respList.Add("Title");
                }
                if (article.Text == null)
                {
                    respList.Add("Text");
                }
                if (article.Type == null)
                {
                    respList.Add("Type");
                }
                if (article.Lang == null)
                {
                    respList.Add("Lang");
                }

                if (respList.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ARTICLE_REQUIRED, respList));

                var articleGuid = SequentialGuid.NewGuid().ToString();
                _context.Article.Add(
                    new Article
                    {
                        Id = articleGuid,
                        Name = article.Name,
                        Title = article.Title,
                        Text = article.Text,
                        Lang = article.Lang,
                        Type = article.Type
                    });

                _context.SaveChanges();

                respList.Add(articleGuid);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        /// <summary>
        ///     PUT api/Article/
        /// </summary>
        [Route("api/Article/{id}")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpPut]
        public HttpResponseMessage PutArticle(string id, ArticleDTO article)
        {
            try
            {
                var respList = new List<string>();
                if (article == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_ARTICLE_NULL));

                var articleCurrent = _context.Article.SingleOrDefault(a => a.Id == id);
                if (articleCurrent == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ARTICLE_NOTFOUND, respList));
                }

                if (article.Name == null)
                {
                    respList.Add("Name");
                }
                if (article.Title == null)
                {
                    respList.Add("Title");
                }
                if (article.Text == null)
                {
                    respList.Add("Text");
                }
                if (article.Type == null)
                {
                    respList.Add("Type");
                }
                if (article.Lang == null)
                {
                    respList.Add("Lang");
                }
                if (respList.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ARTICLE_REQUIRED, respList));


                articleCurrent.Name = article.Name;
                articleCurrent.Title = article.Title;
                articleCurrent.Text = article.Text;
                articleCurrent.Type = article.Type;
                articleCurrent.Lang = article.Lang;

                _context.MarkAsModified(articleCurrent);
                _context.SaveChanges();

                respList.Add(id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_UPDATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }

        /// <summary>
        ///     DELETE api/Article/
        /// </summary>
        [Route("api/Article/{id}")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpDelete]
        public HttpResponseMessage DeleteArticle(string id)
        {
            try
            {
                var respList = new List<string>();

                var article = _context.Article.SingleOrDefault(a => a.Id == id);
                if (article == null)
                {
                    respList.Add(id);
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_ARTICLE_NOTFOUND, respList));
                }

                _context.Article.Remove(article);


                _context.SaveChanges();

                respList.Add(id);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_DELETED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.InnerException);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }
    }
}