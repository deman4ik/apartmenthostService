using System;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace apartmenthostService.Attributes
{
    public class QueryableExpandAttribute : ActionFilterAttribute
    {
        private const string ODataExpandOption = "$expand=";

        public QueryableExpandAttribute(string expand)
        {
            AlwaysExpand = expand;
        }

        public string AlwaysExpand { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var request = actionContext.Request;
            var query = request.RequestUri.Query.Substring(1);
            var parts = query.Split('&').ToList();
            var foundExpand = false;
            for (var i = 0; i < parts.Count; i++)
            {
                var segment = parts[i];
                if (segment.StartsWith(ODataExpandOption, StringComparison.Ordinal))
                {
                    foundExpand = true;
                    parts[i] += "," + AlwaysExpand;
                    break;
                }
            }

            if (!foundExpand)
            {
                parts.Add(ODataExpandOption + AlwaysExpand);
            }

            var modifiedRequestUri = new UriBuilder(request.RequestUri);
            modifiedRequestUri.Query = string.Join("&",
                parts.Where(p => p.Length > 0));
            request.RequestUri = modifiedRequestUri.Uri;
            base.OnActionExecuting(actionContext);
        }
    }
}