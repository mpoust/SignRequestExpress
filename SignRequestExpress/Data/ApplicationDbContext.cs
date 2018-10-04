using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SignRequestExpress.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // No DbSets are declared since IdentityDbContext already provides the Users set and
        //  we don't want to expose any other database data outside of authentication - API
        //   will provide the data
    }
}
