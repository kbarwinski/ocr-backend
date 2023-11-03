using MediatR;
using OcrInvoiceBackend.Application.Common.Behaviors;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignOutUser
{
    [RequiresRole("User")]
    public class SignOutUserCommand : IRequest
    {
    }
}
