using MediatR;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignInUser
{
    public record SignInUserCommand(string Username, string Password) : IRequest<SignInUserResponse> { }
}
