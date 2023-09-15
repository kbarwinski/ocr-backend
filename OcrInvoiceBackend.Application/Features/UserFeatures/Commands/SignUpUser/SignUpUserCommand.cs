using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignUpUser
{
    public record SignUpUserCommand(string Username, string Password) : IRequest<SignUpUserResponse> { }
}
