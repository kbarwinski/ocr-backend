using MediatR;
using Microsoft.AspNetCore.Http;
using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Common.Behaviors
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class RequiresRoleAttribute : Attribute
    {
        public string RoleName { get; }

        public RequiresRoleAttribute(string roleName)
        {
            RoleName = roleName;
        }
    }

    public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var attribute = request.GetType().GetCustomAttribute<RequiresRoleAttribute>();

            var user = (IUser)_httpContextAccessor.HttpContext.Items["User"];
            
            if (attribute != null)
            {
                if(user == null || !user.Roles.Contains(attribute.RoleName))
                    throw new UnauthorizedAccessException($"{attribute.RoleName} role is required.");
            }

            return await next();
        }
    }
}
