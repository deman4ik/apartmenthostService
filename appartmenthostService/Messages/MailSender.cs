using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.Helpers;
using apartmenthostService.Models;
using Alpinely.TownCrier;
using Exceptions;
using SendGrid;

namespace apartmenthostService.Messages
{
    public class MailSender : IDisposable
    {
        private readonly string _username;
        private readonly string _password;

        public MailSender()
        {
            _username = Environment.GetEnvironmentVariable("SENDGRID_USERNAME");
            _password = Environment.GetEnvironmentVariable("SENDGRID_PASSWORD");
        }

        private async Task Send(SendGridMessage message)
        {
            // Create credentials, specifying your user name and password.
            var credentials = new NetworkCredential(_username, _password);

            // Create an Web transport for sending email.
            var transportWeb = new Web(credentials);

            // Send the email, which returns an awaitable task.
            await transportWeb.DeliverAsync(message);
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="basemessage"></param>
        public async void Create(IApartmenthostContext context, BaseEmailMessage basemessage)
        {
            try
            {
                // Сообщение SendGrid
                SendGridMessage message = new SendGridMessage();
                // Шаблонизатор
                var emailFactory = new MergedEmailFactory(new TemplateParser());


                // Считываем глобальные настройки
                var fromAddress = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS");
                var from = Environment.GetEnvironmentVariable("EMAIL_FROM");
                var webDomain = Environment.GetEnvironmentVariable("WEB_DOMAIN");

                // Отправитель
                message.From = new MailAddress(fromAddress, from);

                // Получатель
                message.AddTo(basemessage.ToUserEmail);

                // Глобальный HTML шаблон письма
                StringBuilder htmlTemplate = new StringBuilder();
                var templArt =
                    context.Article.SingleOrDefault(x => x.Name == ConstVals.EmailTemplate && x.Type == ConstVals.Email);
                htmlTemplate.Append(templArt.Text);

                // Приветстиве пользователя
                var greetArt =
                    context.Article.SingleOrDefault(x => x.Name == ConstVals.Greet && x.Type == ConstVals.Email);
                var greetTv = new Dictionary<string, string>
                {
                    {"username", basemessage.ToUserName}
                };
                MailMessage greetMsg = emailFactory
                    .WithTokenValues(greetTv)
                    .WithHtmlBody(greetArt.Text).Create();

                // HTML шаблон основного тела письма
                StringBuilder bodyTemplate = new StringBuilder();
                var bodyTemplateTv = new Dictionary<string, string>();
                var bodyTokenValues = new Dictionary<string, string>();

                // Считывание шаблона
                if (!string.IsNullOrEmpty(basemessage.Code))
                {
                    var article =
                        context.Article.SingleOrDefault(x => x.Name == basemessage.Code && x.Type == ConstVals.Email);
                    if (article != null)
                    {
                        message.Subject = article.Title;
                        bodyTemplate.Append(article.Text);
                    }
                }
                var dateformat = "dd.MM.yyyy";
                switch (basemessage.Code)
                {
                    case RespH.SRV_NOTIF_CARD_FAVORITED:
                        bodyTokenValues.Add("username", basemessage.FromUserName);
                        bodyTokenValues.Add("url", webDomain + "#/posts/" + basemessage.CardId);
                        break;
                    case RespH.SRV_NOTIF_RESERV_NEW:
                        bodyTokenValues.Add("username", basemessage.FromUserName);
                        bodyTokenValues.Add("cardname", basemessage.CardName);
                        bodyTokenValues.Add("cardtype", basemessage.CardType);
                        bodyTokenValues.Add("carddesc", basemessage.CardDescription);
                        bodyTokenValues.Add("datefrom", basemessage.DateFrom.ToString(dateformat));
                        bodyTokenValues.Add("dateto", basemessage.DateTo.ToString(dateformat));
                        bodyTokenValues.Add("url", webDomain + "#/posts/" + basemessage.CardId);
                        break;
                    case RespH.SRV_NOTIF_RESERV_PENDING:
                        bodyTokenValues.Add("ownername", basemessage.FromUserName);
                        bodyTokenValues.Add("cardname", basemessage.CardName);
                        bodyTokenValues.Add("cardtype", basemessage.CardType);
                        bodyTokenValues.Add("carddesc", basemessage.CardDescription);
                        bodyTokenValues.Add("datefrom", basemessage.DateFrom.ToString(dateformat));
                        bodyTokenValues.Add("dateto", basemessage.DateTo.ToString(dateformat));
                        bodyTokenValues.Add("url", webDomain + "#/posts/" + basemessage.CardId);
                        break;
                    case RespH.SRV_NOTIF_RESERV_ACCEPTED:
                        bodyTokenValues.Add("ownername", basemessage.FromUserName);
                        bodyTokenValues.Add("cardname", basemessage.CardName);
                        bodyTokenValues.Add("cardtype", basemessage.CardType);
                        bodyTokenValues.Add("carddesc", basemessage.CardDescription);
                        bodyTokenValues.Add("datefrom", basemessage.DateFrom.ToString(dateformat));
                        bodyTokenValues.Add("dateto", basemessage.DateTo.ToString(dateformat));
                        bodyTokenValues.Add("url", webDomain + "#/posts/" + basemessage.CardId);
                        break;
                    case RespH.SRV_NOTIF_RESERV_DECLINED:
                        bodyTokenValues.Add("ownername", basemessage.FromUserName);
                        bodyTokenValues.Add("cardname", basemessage.CardName);
                        bodyTokenValues.Add("cardtype", basemessage.CardType);
                        bodyTokenValues.Add("carddesc", basemessage.CardDescription);
                        bodyTokenValues.Add("datefrom", basemessage.DateFrom.ToString(dateformat));
                        bodyTokenValues.Add("dateto", basemessage.DateTo.ToString(dateformat));
                        bodyTokenValues.Add("url", webDomain + "#/posts/" + basemessage.CardId);
                        break;
                    case RespH.SRV_NOTIF_REVIEW_ADDED:
                        bodyTokenValues.Add("username", basemessage.FromUserName);
                        bodyTokenValues.Add("reviewtext", basemessage.ReviewText);
                        break;
                    case RespH.SRV_NOTIF_REVIEW_RATING_ADDED:
                        bodyTokenValues.Add("username", basemessage.FromUserName);
                        bodyTokenValues.Add("reviewtext", basemessage.ReviewText);
                        bodyTokenValues.Add("reviewrating",
                            basemessage.ReviewRating.ToString(CultureInfo.InvariantCulture));
                        break;
                    case RespH.SRV_NOTIF_REVIEW_AVAILABLE:
                        bodyTokenValues.Add("cardname", basemessage.CardName);
                        bodyTokenValues.Add("carddesc", basemessage.CardDescription);
                        bodyTokenValues.Add("datefrom", basemessage.DateFrom.ToString(dateformat));
                        bodyTokenValues.Add("dateto", basemessage.DateTo.ToString(dateformat));
                        break;
                    case ConstVals.Reg:
                        bodyTokenValues.Add("code", basemessage.ConfirmCode);
                        bodyTokenValues.Add("url", webDomain + "#/confirm?userId=" + basemessage.ToUserId + "&code=" +
                                                   basemessage.ConfirmCode);
                        break;
                    case ConstVals.Restore:
                        bodyTokenValues.Add("code", basemessage.ConfirmCode);
                        bodyTokenValues.Add("url", webDomain + "#/reset?userId=" + basemessage.ToUserId + "&code=" +
                                                   basemessage.ConfirmCode);
                        break;
                    case ConstVals.Feedback:
                    case ConstVals.Abuse:
                        bodyTokenValues.Add("username", basemessage.FromUserName);
                        bodyTokenValues.Add("text", basemessage.Text);

                        if (!string.IsNullOrWhiteSpace(basemessage.FromUserEmail) && basemessage.AnswerByEmail)
                        {
                            bodyTokenValues.Add("email", basemessage.FromUserEmail);
                        }
                        else
                        {
                            bodyTokenValues.Add("email", "не указан");
                        }
                        break;
                }
                // Формируем основное тело
                emailFactory = new MergedEmailFactory(new TemplateParser());
                var bodyMsg = emailFactory
                    .WithTokenValues(bodyTokenValues)
                    .WithHtmlBody(bodyTemplate.ToString())
                    .Create();
                // Соединяем все вместе
                StringBuilder fullEmail = new StringBuilder();
                fullEmail.Append(greetMsg.Body);
                fullEmail.Append("<br><br>");
                fullEmail.Append(bodyMsg.Body);
                bodyTemplateTv.Add("content", fullEmail.ToString());
                emailFactory = new MergedEmailFactory(new TemplateParser());
                MailMessage fullMailMessage = emailFactory.WithTokenValues(bodyTemplateTv)
                    .WithHtmlBody(htmlTemplate.ToString())
                    .Create();
                message.Html = fullMailMessage.Body;

                // Отслеживание переходов из письма
                message.EnableClickTracking(true);

                await Send(message);
            }
            catch (InvalidApiRequestException e)
            {
                Debug.WriteLine("!!!!!Invalid Api Request Exception!!!!!!");
                foreach (var err in e.Errors)
                {
                    Debug.WriteLine(err);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("!!!!!SEND EMAIL EXCEPTION!!!!!!");
                Debug.WriteLine(e);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}