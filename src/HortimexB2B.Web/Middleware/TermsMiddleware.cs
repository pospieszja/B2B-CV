using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HortimexB2B.Infrastructure.Identity;
using HortimexB2B.Infrastructure.Repositories;
using HortimexB2B.Infrastructure.Repositories.Interfaces;
using HortimexB2B.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace HortimexB2B.Web.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class TermsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TermsMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _next = next;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task Invoke(HttpContext httpContext, IUserService userService)
        {
            var isAuthenticated = httpContext.User.Identity.IsAuthenticated;

            if (isAuthenticated && !httpContext.Request.Path.ToString().StartsWith("/User/Terms"))
            {
                var name = httpContext.User.Identity.Name;

                if (!await userService.IsConsentWasGivenAsync(name))
                {
                    httpContext.Response.Redirect("/User/Terms");
                }
            }

            await _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseTermsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TermsMiddleware>();
        }
    }
}
