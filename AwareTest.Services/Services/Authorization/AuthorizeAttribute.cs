using AwareTest.Model.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Authentication;
using System.Web.Mvc;
using System.Web;

namespace AwareTest.Services.Services.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var account = (UserModel)context.HttpContext.Items["User"];
            if (account == null)
            {
                //// not logged in or role not authorized
                //JsonResult jsonResult = new JsonResult();
                //jsonResult.Data = new { Success = false, Message = "Unauthorized" };

                //context.Result = (IActionResult)jsonResult;
                throw new AuthenticationException("Unauthorized");
            }
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var token = httpContext.Request.RequestContext.HttpContext.Request.Headers["X-XSRF-TOKEN"];
            var sessionToken = httpContext.Request.RequestContext.HttpContext.Session["XSRF-TOKEN"];

            if (string.IsNullOrEmpty(token) || sessionToken == null || sessionToken.ToString() != token)
            {
                return false;
            }

            return true;
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ValidateAngularAntiForgeryTokenAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var token = httpContext.Request.RequestContext.HttpContext.Request.Headers["X-XSRF-TOKEN"];
            var sessionToken = httpContext.Request.RequestContext.HttpContext.Session["XSRF-TOKEN"];

            if (string.IsNullOrEmpty(token) || sessionToken == null || sessionToken.ToString() != token)
            {
                return false;
            }

            return true;
        }
    }
}
