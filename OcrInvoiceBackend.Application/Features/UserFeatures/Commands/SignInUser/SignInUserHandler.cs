using MediatR;
using OcrInvoiceBackend.Application.Common.Exceptions;
using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignInUser
{
    public class SignInUserHandler : IRequestHandler<SignInUserCommand, SignInUserResponse>
    {
        private readonly IIdentityService _identityService;

        public SignInUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<SignInUserResponse> Handle(SignInUserCommand request, CancellationToken cancellationToken)
        {
            var signInResult = await _identityService.SignInAsync(request.Username, request.Password);
            if (signInResult != OperationResult.Success)
            {
                throw new BadRequestException(signInResult);
            }

            var jwtToken = await _identityService.GenerateJwtTokenAsync(request.Username, request.Password);
            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new NullReferenceException("Failed to generate JWT token.");
            }

            var userInfo = await _identityService.ValidateJwtTokenAsync(jwtToken);
            if (userInfo == null)
            {
                throw new NullReferenceException("Failed to validate generated JWT token.");
            }

            return new SignInUserResponse(jwtToken, userInfo);
        }
    }
}
