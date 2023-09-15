using MediatR;
using OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignUpUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignInUser
{
    public record SignInUserCommand(string Username, string Password) : IRequest<SignInUserResponse> { }
}
