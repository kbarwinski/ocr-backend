using FluentMigrator;
using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Persistence.Migrations.Identity
{
    [Migration(202308142248)]
    public class _02_SeedExampleUsers : IdentityBaseMigration
    {
        private readonly IIdentityService _identityService;

        private readonly string _adminEmail;
        private readonly string _adminPassword;

        private readonly string _userEmail;
        private readonly string _userPassword;

        public _02_SeedExampleUsers(IIdentityService identityService)
        {
            _identityService = identityService;

            _adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            _adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            _userEmail = Environment.GetEnvironmentVariable("USER_EMAIL");
            _userPassword = Environment.GetEnvironmentVariable("USER_PASSWORD");
        }

        public override void Up()
        {
            _identityService.CreateUserAsync(_adminEmail, _adminPassword, "Admin").GetAwaiter().GetResult();
            _identityService.CreateUserAsync(_userEmail, _userPassword, "User").GetAwaiter().GetResult();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
