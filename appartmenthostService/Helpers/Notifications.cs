using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using apartmenthostService.DataObjects;
using apartmenthostService.Models;
using HtmlAgilityPack;
using Simplify.Mail;

namespace apartmenthostService.Helpers
{
    public class Email
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
    public static class Notifications
    {
        public static void Create(apartmenthostContext context, string userId, string type, string code, string favoriteId = null, string reservationId = null, string reviewId = null, bool sendMail = false)
        {
            try
            {
                Notification notif = new Notification
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    Type = type,
                    Code = code,
                    Readed = false
                };
                if (!string.IsNullOrEmpty(favoriteId))
                    notif.FavoriteId = favoriteId;
                if (!string.IsNullOrEmpty(reservationId))
                    notif.ReservationId = reservationId;
                if (!string.IsNullOrEmpty(reviewId))
                    notif.ReviewId = reviewId;
                context.Set<Notification>().Add(notif);
                context.SaveChanges();
                if (sendMail)
                {
                    SendEmail(context, userId, type, code, favoriteId, reservationId, reviewId);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("!!!!!CREATING NOTIFICATION EXCEPTION!!!!!!");
                   Debug.WriteLine(e);
                
            }
        }

        public static void SendEmail(apartmenthostContext context, string userId, string type, string code, string favoriteId = null, string reservationId = null, string reviewId = null, string confirmCode = null)
        {
            try
            {

            
            Email email = new Email();
                string body = "{0}";
                StringBuilder templBody = new StringBuilder();
            var greetArt = context.Article.SingleOrDefault(x => x.Name == ConstVals.Greet);
            var templArt = context.Article.SingleOrDefault(x => x.Name == ConstVals.EmailTemplate);
                templBody.Append(templArt.Text);

            if (!string.IsNullOrEmpty(code))
            {
                var article = context.Article.SingleOrDefault(x => x.Name == code);
                if (article != null)
                {
                    email.Title = article.Title;
                    body = article.Text;
                }
            }
            email.From = MailSender.Default.Settings.SmtpUserName;

            var profile = context.Profile.SingleOrDefault(x => x.Id == userId);
            if (!string.IsNullOrEmpty(profile.ContactEmail))
            {
                email.To = profile.ContactEmail;
            }
            else
            {
                var user = context.Users.SingleOrDefault(x => x.Id == userId);
                email.To = user.Email;
            }

            string name = !string.IsNullOrEmpty(profile.FirstName) || !string.IsNullOrEmpty(profile.LastName)
                ? profile.FirstName + " " + profile.LastName
                : email.To;
            var greet = string.Format(greetArt.Text, name);
            switch (code)
            {
                case RespH.SRV_NOTIF_CARD_FAVORITED:
                    var owner = context.Favorites.Where(c => c.Id == favoriteId).Select( s => new { s.User.Profile.FirstName,  s.User.Profile.LastName, s.CardId }).FirstOrDefault();
                    body = String.Format(body, owner.FirstName + " " + owner.LastName, "https://apartmenthost.azurewebsites.net/#/posts/"+owner.CardId);
                    break;
                case RespH.SRV_NOTIF_RESERV_PENDING:
                    var pendreserv =
                        context.Reservations.Where(r => r.Id == reservationId)
                            .Select(res => new {res.Card.Name, res.DateFrom, res.DateTo})
                            .FirstOrDefault();
                    body = String.Format(body, pendreserv.Name, pendreserv.DateFrom, pendreserv.DateTo);
                    break;
                case RespH.SRV_NOTIF_RESERV_ACCEPTED:
                    var accreserv =
                      context.Reservations.Where(r => r.Id == reservationId)
                          .Select(res => new { res.Card.Name, res.DateFrom, res.DateTo, res.Card.User.Profile.FirstName, res.Card.User.Profile.LastName })
                          .FirstOrDefault();
                    body = String.Format(body, accreserv.FirstName + " "+ accreserv.LastName, accreserv.Name, accreserv.DateFrom, accreserv.DateTo);
                    break;
                case RespH.SRV_NOTIF_RESERV_DECLINED:
                    var declreserv =
                      context.Reservations.Where(r => r.Id == reservationId)
                          .Select(res => new { res.Card.Name, res.DateFrom, res.DateTo, res.Card.User.Profile.FirstName, res.Card.User.Profile.LastName })
                          .FirstOrDefault();
                    body = String.Format(body, declreserv.FirstName + " " + declreserv.LastName, declreserv.Name, declreserv.DateFrom, declreserv.DateTo);
                    break;
                case RespH.SRV_NOTIF_REVIEW_ADDED:
                    var addreview =
                        context.Reviews.Where(rev => rev.Id == reviewId)
                            .Select(r => new {r.FromUser.Profile.FirstName, r.FromUser.Profile.LastName, r.Text}).FirstOrDefault();
                    body = String.Format(body, addreview.FirstName + " " + addreview.LastName, addreview.Text);
                    break;
                case RespH.SRV_NOTIF_REVIEW_RATING_ADDED:
                    var ratereview =
                       context.Reviews.Where(rev => rev.Id == reviewId)
                           .Select(r => new { r.FromUser.Profile.FirstName, r.FromUser.Profile.LastName, r.Text, r.Rating }).FirstOrDefault();
                    body = String.Format(body, ratereview.FirstName + " " + ratereview.LastName, ratereview.Text, ratereview.Rating);
                    break;
                case RespH.SRV_NOTIF_REVIEW_AVAILABLE:
                    var reviewav =
                        context.Reservations.Where(res => res.Id == reservationId)
                            .Select(r => new {r.Card.Name, r.DateFrom, r.DateTo}).FirstOrDefault();
                    body = String.Format(body, reviewav.Name, reviewav.DateFrom, reviewav.DateTo);
                    break;
                    case ConstVals.Reg:
                        body = String.Format(body, confirmCode, "https://apartmenthost.azurewebsites.net/#/confirm/"+userId+"/"+confirmCode);
                    break;
                    case ConstVals.Restore:
                        body = String.Format(body, confirmCode, "https://apartmenthost.azurewebsites.net/#/reset/" + userId + "/" + confirmCode);
                    break;

            }
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(templBody.ToString());
                var content = doc.DocumentNode.SelectSingleNode("//*[contains(@class, 'email-content')]");
                content.InnerHtml = greet + "<br><br>" + body;
                email.Body = doc.DocumentNode.WriteContentTo();

            MailSender.Default.Send(email.From, email.To, email.Title, email.Body);
            }
            catch (Exception e)
            {
                Debug.WriteLine("!!!!!SEND EMAIL EXCEPTION!!!!!!");
                Debug.WriteLine(e);
                
            }
        }
            
    }
}
