using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
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

                if (feedback.Type == null)
                {
                    respList.Add("Type");
                }


                if (feedback.UserId != null)
                {
                    var user = _context.Users.AsNoTracking().SingleOrDefault(x => x.Id == feedback.UserId);
                    var profile = _context.Profile.AsNoTracking().SingleOrDefault(x => x.Id == feedback.UserId);
                    if (user == null || profile == null)
                    {
                        respList.Add(feedback.UserId);
                        return Request.CreateResponse(HttpStatusCode.Unauthorized,
                            RespH.Create(RespH.SRV_USER_NOTFOUND, respList));
                    }
                    feedback.UserName = profile.FirstName + " " + profile.LastName;
                    feedback.Email = user.Email;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(feedback.UserName))
                    {
                        respList.Add("Username");
                    }
                }


                if (feedback.AnswerByEmail && feedback.Email == null)
                {
                    respList.Add("Email");
                }

                if (respList.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.BadRequest,
                        RespH.Create(RespH.SRV_FEEDBACK_REQUIRED, respList));
                var bem = new BaseEmailMessage();
                if (feedback.Type == ConstVals.Abuse)
                {
                    var abuser = _context.Users.SingleOrDefault(x => x.Id == feedback.AbuserId);
                    var abuserProfile = _context.Profile.SingleOrDefault(x => x.Id == feedback.AbuserId);

                    if (abuser == null || abuserProfile == null)
                    {
                        respList.Add(feedback.AbuserId);
                        return Request.CreateResponse(HttpStatusCode.BadRequest,
                            RespH.Create(RespH.SRV_FEEDBACK_ABUSER_NOTFOUND, respList));
                    }
                    StringBuilder addText = new StringBuilder();
                    addText.Append("Жалоба на пользователя: <br>");
                    addText.Append(abuserProfile.FirstName + " " + abuserProfile.LastName + "<br>");
                    addText.Append("Email: " + abuser.Email + "<br>");
                    addText.Append("Id: " + abuser.Id + "<br>");
                    addText.Append("Текст жалобы: <br>");
                    addText.Append(feedback.Text);
                    feedback.Text = addText.ToString();
                    bem.Code = ConstVals.Abuse;
                }
                else
                {
                    bem.Code = ConstVals.Feedback;
                }
                var feedbackGuid = SequentialGuid.NewGuid().ToString();
                _context.Feedbacks.Add(
                    new Feedback
                    {
                        Id = feedbackGuid,
                        AbuserId = feedback.AbuserId,
                        UserId = feedback.UserId,
                        UserName = feedback.UserName,
                        Type = feedback.Type,
                        Email = feedback.Email,
                        Text = feedback.Text,
                        AnswerByEmail = feedback.AnswerByEmail
                    });

                _context.SaveChanges();

                using (MailSender mailSender = new MailSender())
                {
                    bem.ToUserEmail =
                        Environment.GetEnvironmentVariable(feedback.Type == ConstVals.Abuse
                            ? "ABUSE_EMAIL"
                            : "FEEDBACK_EMAIL");

                    bem.ToUserName = "Команда Petforaweek";
                    bem.FromUserEmail = feedback.Email;
                    bem.FromUserName = feedback.UserName;
                    bem.Text = feedback.Text;
                    bem.AnswerByEmail = feedback.Type == ConstVals.Abuse || feedback.AnswerByEmail;
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