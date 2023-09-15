using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Services.Identity
{
    public enum OperationResult
    {
        Success,
        Failure,
        InvalidCredentials,
        UserLockedOut,
        EmailNotConfirmed,
    }

    public interface IIdentityService
    {
        Task<OperationResult> CreateUserAsync(string email, string password, string roleName);
        Task<string?> GenerateJwtTokenAsync(string email, string password);
        Task<IUser?> ValidateJwtTokenAsync(string token);
        Task<OperationResult> SignInAsync(string email, string password);
        Task SignOutAsync();
        Task<OperationResult> CreateRoleAsync(string roleName);
        Task<bool> RoleExistsAsync(string roleName);
    }
}
