using MediatR;
using Microsoft.AspNetCore.Http;
using OcrInvoiceBackend.Application.Common.Behaviors;
using OcrInvoiceBackend.Application.Services.Identity;
using System;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignOutUser
{
    public class SignOutUserHandler : IRequestHandler<SignOutUserCommand>
    {
        private readonly IIdentityService _identityService;

        public SignOutUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Handle(SignOutUserCommand request, CancellationToken cancellationToken)
        {
            await _identityService.SignOutAsync();
        }
    }
}
