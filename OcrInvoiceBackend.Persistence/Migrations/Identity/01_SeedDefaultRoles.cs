﻿using FluentMigrator;
using OcrInvoiceBackend.Application.Services.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OcrInvoiceBackend.Persistence.Migrations.Identity
{
    [Migration(202308142247)]
    public class _01_SeedDefaultRoles : IdentityBaseMigration
    {
        private readonly IIdentityService _identityService;

        public _01_SeedDefaultRoles(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public override void Up()
        {
            _identityService.CreateRoleAsync("Admin").GetAwaiter().GetResult();
            _identityService.CreateRoleAsync("User").GetAwaiter().GetResult();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
