namespace LittleGoat.Filter
{
    using LittleGoat.DataAccess;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class IsAuthenticated : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string playerId = HttpContext.Current.Request.Cookies["playerId"]?.Value;
            if (string.IsNullOrEmpty(playerId))
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "action", "ChooseName" },
                        { "controller", "Account" },
                        { "returnUrl", HttpContext.Current.Request.RawUrl }
                });
            }

            using (LittleGoatEntities entities = new LittleGoatEntities())
            {
                var existingPlayer = entities.Player.FirstOrDefault(p => p.Id == playerId);
                if (existingPlayer == null)
                {
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary {
                                            { "action", "ChooseName" },
                                            { "controller", "Account" },
                                            { "returnUrl", HttpContext.Current.Request.RawUrl }
                    });
                }
            }
        }
    }
}