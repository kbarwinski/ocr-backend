using MediatR;
using Microsoft.AspNetCore.Mvc;
using OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignInUser;
using OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignUpUser;

namespace OcrInvoiceBackend.API.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult<SignInUserResponse>> SignIn(SignInUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult<SignUpUserResponse>> SignUp(SignUpUserCommand request, CancellationToken cancellationToken)
        {
            var response = await _mediator.Send(request, cancellationToken);
            return Ok(response);
        }
    }
}
