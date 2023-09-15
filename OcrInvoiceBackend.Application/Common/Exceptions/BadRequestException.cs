using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        private static Dictionary<OperationResult, string> OperationMessages = new Dictionary<OperationResult, string>
        {
            [OperationResult.Success] = "Signing successful.",
            [OperationResult.Failure] = "Signing failed.",
            [OperationResult.InvalidCredentials] = "Invalid credentials provided.",
            [OperationResult.UserLockedOut] = "User locked out from signing in.",
            [OperationResult.EmailNotConfirmed] = "User email confirmation required."
        };

        public BadRequestException(string message) : base(message)
        {
        }

        public BadRequestException(OperationResult identityResult) : base(OperationMessages[identityResult])
        {
        }

        public BadRequestException(string[] errors) : base("Multiple errors occurred. See error details.")
        {
            Errors = errors;
        }

        public string[] Errors { get; set; }
    }
}
