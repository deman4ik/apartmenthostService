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
using apartmenthostService.Messages;
using apartmenthostService.Models;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class FeedbackApiController : ApiController
    {
        private readonly IApartmenthostContext _context = new ApartmenthostContext();
        public ApiServices Services { get; set; }

        public FeedbackApiController()
        {
        }

        public FeedbackApiController(IApartmenthostContext context)
        {
            _context = context;
        }

        // POST api/Feedback/
        [Route("api/Feedback/")]
        [AuthorizeLevel(AuthorizationLevel.Anonymous)]
        [HttpPost]
        public HttpResponseMessage PostFeedback(FeedbackDTO feedback)
        {
            try
            {
                var respList = new List<string>();
                if (feedback == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest, RespH.Create(RespH.SRV_FEEDBACK_NULL));


                if (feedback.Text == null)
                {
                    respList.Add("Text");
                }

                //TODO: Проверка Типа, Пользователя на которого пожаловались, отправка сообщения для жалобы.
                if (feedback.Type == null)
                {
                    respList.Add("Type");
                }

                if (feedback.Username == null)
                {
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
                        return Request.CreateResponse(HttpStatusCode.Unauthorized,
                            RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                    }
                    feedback.UserId = account.UserId;
                    feedback.Username = profile.FirstName + " " + profile.LastName;
                    feedback.Email = user.Email;
                }

                if (feedback.AnswerByEmail && feedback.Email == null)
                {
                    respList.Add("Email");
                }

                if (respList.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_FEEDBACK_REQUIRED, respList));


                var feedbackGuid = SequentialGuid.NewGuid().ToString();
                _context.Feedbacks.Add(
                    new Feedback
                    {
                        Id = feedbackGuid,
                        UserId = feedback.UserId,
                        UserName = feedback.Username,
                        Email = feedback.Email,
                        Text = feedback.Text,
                        AnswerByEmail = feedback.AnswerByEmail
                    });

                _context.SaveChanges();

                using (MailSender mailSender = new MailSender())
                {
                    var bem = new BaseEmailMessage
                    {
                        Code = ConstVals.Feedback,
                        ToUserEmail = Environment.GetEnvironmentVariable("FEEDBACK_EMAIL"),
                        ToUserName = "Команда Petforaweek",
                        FromUserEmail = feedback.Email,
                        FromUserName = feedback.Username,
                        Text = feedback.Text,
                        AnswerByEmail = feedback.AnswerByEmail
                    };
                    mailSender.Create(_context, bem);
                }
                respList.Add(feedbackGuid);
                return Request.CreateResponse(HttpStatusCode.OK, RespH.Create(RespH.SRV_CREATED, respList));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return Request.CreateResponse(HttpStatusCode.BadRequest,
                    RespH.Create(RespH.SRV_EXCEPTION, new List<string> {ex.ToString()}));
            }
        }
    }
}