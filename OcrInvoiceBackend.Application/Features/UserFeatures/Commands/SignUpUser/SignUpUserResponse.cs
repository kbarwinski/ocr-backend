using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Application.Features.UserFeatures.Commands.SignUpUser
{
    public record SignUpUserResponse(string JwtToken, IUser UserInfo) { }
}
