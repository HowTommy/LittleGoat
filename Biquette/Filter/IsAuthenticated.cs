namespace Biquette.Filter
{
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class IsAuthenticated : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string playerId = HttpContext.Current.Request.Cookies["playerId"]?.Value;
            if (string.IsNullOrWhiteSpace(playerId))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "action", "login" },
                        { "controller", "persons" },
                        { "someQuerystring", "someValue" }
                });
            }
        }
    }
}