using MediatR;
using OcrInvoiceBackend.Application.Common.Exceptions;
using OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignInUser;
using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignUpUser
{
    internal class SignUpUserHandler : IRequestHandler<SignUpUserCommand, SignUpUserResponse>
    {
        private readonly IIdentityService _identityService;

        public SignUpUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<SignUpUserResponse> Handle(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            if (!await _identityService.RoleExistsAsync("User"))
            {
                var roleCreationResult = await _identityService.CreateRoleAsync("User");

                if (roleCreationResult != OperationResult.Success)
                {
                    throw new Exception("Failed to create a default signup user role");
                }
            }

            var createUserResult = await _identityService.CreateUserAsync(request.Username, request.Password, "User");

            if (createUserResult != OperationResult.Success)
            {
                throw new BadRequestException(createUserResult);
            }

            var jwtToken = await _identityService.GenerateJwtTokenAsync(request.Username, request.Password);
            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new NullReferenceException("Failed to generate JWT token after registration");
            }

            var userInfo = await _identityService.ValidateJwtTokenAsync(jwtToken);
            if (userInfo == null)
            {
                throw new NullReferenceException("Failed to validate JWT token after registration");
            }

            return new SignUpUserResponse(jwtToken, userInfo);
        }
    }

}
