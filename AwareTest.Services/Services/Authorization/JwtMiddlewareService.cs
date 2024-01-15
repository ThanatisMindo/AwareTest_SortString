using AwareTest.Services.IService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace AwareTest.Services.Services.Authorization
{
    public class JwtMiddlewareService
    {
        private readonly RequestDelegate _next;

        public JwtMiddlewareService(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IJwtUtilsService jwtUtils)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if(authHeader != null)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault().Split(' ').Last();

                var userId = jwtUtils.ValidateJwtToken(token);
                if (userId != null)
                {
                    // attach user to context on successful jwt validation
                    context.Items["User"] = userId;
                }
            }
            await _next(context);
        }
    }
}
