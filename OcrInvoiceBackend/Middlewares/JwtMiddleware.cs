using OcrInvoiceBackend.Application.Services.Identity;

namespace OcrInvoiceBackend.API.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IIdentityService identityService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            var userInfo = await identityService.ValidateJwtTokenAsync(token);

            if (userInfo != null)
            {
                context.Items["User"] = userInfo;
            }

            await _next(context);
        }
    }
}
