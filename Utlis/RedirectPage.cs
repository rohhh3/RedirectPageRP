using DeviceDetectorNET;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Threading.Tasks;

namespace Middleware.Utlis
{
    public class RedirectPage
    {
        private readonly RequestDelegate _next;

        public RedirectPage(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string userAgent = context.Request.Headers["User-Agent"].ToString();
            var userAgentParser = new DeviceDetector(userAgent);
            userAgentParser.Parse();

            string browserName = userAgentParser.GetClient().Match.Name;
            if(IsSupportedBrowser(browserName))
            {
                if (browserName == "Microsoft Edge" || browserName == "Edg" || browserName == "IE" || browserName == "Trident" || browserName == "msedge")
                {
                    context.Response.Redirect("https://www.mozilla.org/pl/firefox/new/");
                    return;
                }
                else if (browserName == "Chrome")
                {
                    context.Response.Redirect("https://www.google.com/intl/pl_pl/chrome/");
                    return;
                }
                else if (browserName == "Opera")
                {
                    context.Response.Redirect("https://www.opera.com/pl/download");
                    return;
                }
            }

            await _next(context);
        }

        private bool IsSupportedBrowser(string browserName)
        {
            return browserName.Contains("Edge") || browserName.Contains("Edg") || browserName.Contains("Trident") || browserName.Contains("IE") || browserName.Contains("msedge") || browserName.Contains("Chrome") || browserName.Contains("Opera");
        }
    }
}
