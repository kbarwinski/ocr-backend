using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignUpUser
{
    [RequiresRole("Admin")]
    public record SignUpUserCommand(string Username, string Password) : IRequest<SignUpUserResponse> { }
}
