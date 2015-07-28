using System;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Simplify.Mail;

namespace apartmenthostService.Controllers
{
    public class MailApiController : ApiController
    {
        public ApiServices Services { get; set; }
        // POST api/MailApi
        public string SendMail()
        {
            try
            {
                var mailSender = new MailSender("appSettings");
                mailSender.Send("apartmenthost@inbox.ru", "deman4ik@gmail.com", "It's working!",
                    "Mail message, can be full HTML page");
                return "OK";
            }
            catch (Exception e)
            {
                return e.ToString();
            }
        }
    }
}