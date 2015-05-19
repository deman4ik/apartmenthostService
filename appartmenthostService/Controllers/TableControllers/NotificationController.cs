using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using apartmenthostService.Authentication;
using apartmenthostService.DataObjects;
using Microsoft.WindowsAzure.Mobile.Service;
using apartmenthostService.Models;
using AutoMapper.QueryableExtensions;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace apartmenthostService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.Application)]
    public class NotificationController : TableController<Notification>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            apartmenthostContext context = new apartmenthostContext();
            DomainManager = new EntityDomainManager<Notification>(context, Request, Services);
        }

        // GET tables/Notification
        [AuthorizeLevel(AuthorizationLevel.User)]
        public IQueryable<NotificationDTO> GetAllNotification()
        {
            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(currentUser);
            if (account == null) return null;
            return Query().Where(a => a.UserId == account.UserId).Project().To<NotificationDTO>(); ; 
        }

        // GET tables/Notification/48D68C86-6EA6-4C25-AA33-223FC9A27959
        [AuthorizeLevel(AuthorizationLevel.User)]
        public SingleResult<NotificationDTO> GetNotification(string id)
        {
            var currentUser = User as ServiceUser;
            var account = AuthUtils.GetUserAccount(currentUser);
            if (account == null) return null;
            var result = Lookup(id).Queryable.Where(a => a.UserId == account.UserId).Project().To<NotificationDTO>();
            return SingleResult<NotificationDTO>.Create(result);
        }
    }
}