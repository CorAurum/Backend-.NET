using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TuPenca.Infrastructure.Middleware
{
    public class SitioResolverMiddleware
    {
        private readonly RequestDelegate _next;

        public SitioResolverMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            var host = context.Request.Headers["X-Sitio"].FirstOrDefault() ?? context.Request.Host.Host;

            var sitio = await db.Sitios
                .FirstOrDefaultAsync(t => t.UrlPropia == host);

            if (sitio != null)
            {
                context.Items["Sitio"] = sitio;
            }

            await _next(context);
        }
    }
}
