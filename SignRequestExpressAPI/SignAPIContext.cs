////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: SignAPIContext.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 9/20/2018
 * Description: DBContext class for the SignRequestExpress database. This will house all of the DB sets, or tables, that we will track
 *  on this context. This will be achieved using Entity Framework Core.
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SignRequestExpressAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI
{
    public class SignAPIContext : IdentityDbContext<UserEntity, UserRoleEntity, Guid>
    {
        public SignAPIContext(DbContextOptions options)
            : base(options) { }

        // All of the DBSets (tables) that will be tracked on the context
        public DbSet<AccountEntity> Account { get; set; }

        public DbSet<AccountContactEntity> AccountContact { get; set; }

        public DbSet<TemplateEntity> Template { get; set; }

        //public DbSet<UserEntity> User { get; set; } // Provided from IdentityDbContext

        public DbSet<BrandEntity> Brand { get; set; }

        public DbSet<BrandStandardsEntity> BrandStandards { get; set; }

        public DbSet<RequestEntity> Request { get; set; }
    }
}
