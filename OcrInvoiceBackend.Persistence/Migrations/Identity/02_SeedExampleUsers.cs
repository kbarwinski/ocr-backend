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

        public _02_SeedExampleUsers(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async override void Up()
        {
            await _identityService.CreateUserAsync("test1@wp.pl", "Test123456", "Admin");
            await _identityService.CreateUserAsync("test2@wp.pl", "Test1234567", "User");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
