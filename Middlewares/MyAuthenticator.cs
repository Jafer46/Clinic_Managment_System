using CMS.Models;

namespace CMS.Middlewares
{
    public class MyAuthenticator
    {
        private readonly RequestDelegate _next;
        public MyAuthenticator(RequestDelegate next)
        {
            _next = next;
        }
        public Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.RouteValues["controller"] != null)
            {
                var controllerName = httpContext.Request.RouteValues["controller"].ToString();
                var actionName = httpContext.Request.RouteValues["action"].ToString();
                var areaName = (httpContext.Request.RouteValues["area"] == null ? string.Empty : httpContext.Request.RouteValues["area"].ToString());

                //is the current user authorized
                if (httpContext.Session.GetString(SessionVariable.SessionKeyUserId) == null || httpContext.Session.GetString(SessionVariable.SessionKeyUserRoleId) == null)
                {
                    httpContext.Session.SetString(SessionVariable.SessionKeyMessageType, "error");
                    httpContext.Session.SetString(SessionVariable.SessionKeyMessage, "Session Expired! Please login again!");

                    if (!areaName.Equals(string.Empty))
                        httpContext.Response.Redirect("/Home/Login");
                }
                else if (httpContext.Session.GetString(SessionVariable.SessionKeyUserId) != null && httpContext.Session.GetString(SessionVariable.SessionKeyUserRoleId) != null)
                {
                    int userRoleId = Convert.ToInt32(httpContext.Session.GetString(SessionVariable.SessionKeyUserRoleId));

                    bool isAuthorized = Common.isAuthorized(userRoleId, areaName, controllerName, actionName);
                    if (!isAuthorized)
                    {
                        httpContext.Session.SetString(SessionVariable.SessionKeyMessageType, "error");
                        httpContext.Session.SetString(SessionVariable.SessionKeyMessage, "Sorry! You are NOT Authorized to access this page!");
                       //httpContext.Response.Redirect("/Dashboard/Home/NotAuthorized");
                    }
                }
            }

            return _next(httpContext);
        }
    }
    public static class EmpAuthenticatorExtensions
    {
        public static IApplicationBuilder UseEmpAuthenticator(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MyAuthenticator>();
        }
    }
}
