using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MiliNeu.DataAccess.Data;

namespace UserTracking
{
    public class UserTrackingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory; // Use a factory
        private readonly IConfiguration _configuration; // Use a factory

        public UserTrackingMiddleware(RequestDelegate next, IConfiguration configuration, IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _next = next;
            _configuration = configuration;
            _contextFactory = contextFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var dbContext = _contextFactory.CreateDbContext()) // Create a new context for each request
            {
                // Check if the user has a unique identifier cookie
                var userIdCookie = context.Request.Cookies["UserId"];
                string userId;
                VisitorLog visitorLog = new VisitorLog();

                if (string.IsNullOrEmpty(userIdCookie))
                {
                    userId = Guid.NewGuid().ToString();
                    context.Response.Cookies.Append("UserId", userId, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddYears(1)
                    });
                }
                else
                {
                    userId = userIdCookie;
                    visitorLog = await dbContext.VisitorLogs.SingleOrDefaultAsync(c => c.UserIdentifier == userId);


                }
                if (visitorLog == null)
                {

                    // Log the current visit details
                    if (context.Request.Path.HasValue && !context.Request.Path.Value.Contains("/api") && !context.Request.Path.Value.Contains("."))
                    {
                        visitorLog = new VisitorLog();

                        var pageVisited = context.Request.Path;
                        var referrer = context.Request.Headers["Referer"].ToString();



                        visitorLog.UserIdentifier = userId;
                        visitorLog.Referrer = referrer;
                    }
                    visitorLog = updateVisitorLog(visitorLog, context, userId);
                    dbContext.VisitorLogs.Add(visitorLog);

                }
                else
                {
                    // Log the current visit details
                    if (context.Request.Path.HasValue && !context.Request.Path.Value.Contains("/api") && !context.Request.Path.Value.Contains("."))
                    {
                        var referrer = context.Request.Headers["Referer"].ToString();

                        visitorLog.Referrer = referrer;

                        visitorLog.UserIdentifier = userId;
                        visitorLog.Referrer = referrer;
                        visitorLog = updateVisitorLog(visitorLog, context, userId);

                        dbContext.VisitorLogs.Update(visitorLog);
                    }
                }
                await dbContext.SaveChangesAsync(); // Save changes here


                await _next(context); // Call the next middleware in the pipeline
            }
        }
        public VisitorLog updateVisitorLog(VisitorLog visitor, HttpContext httpContent, string userId)
        {
            var pageVisited = httpContent.Request.Path;

            switch (pageVisited)
            {
                case var page when page == _configuration["UserTrackingUrls:ProductsPage"]:
                    // Handle ProductsPage case here
                    visitor.ProductsPageVisits += 1;
                    break;

                case var page when page == _configuration["UserTrackingUrls:CollectionsPage"]:
                    // Handle CollectionsPage case here
                    visitor.CollectionsPageVisits += 1;

                    break;
                default:
                    // Optional: Handle any other case that doesn't match the above
                    break;
            }

            return visitor;

        }
    }


}
