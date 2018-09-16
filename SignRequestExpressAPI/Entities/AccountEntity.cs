////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: AccountEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified:
 * Description: This is an entity that models an object from the Account table from the database. Account here is the bars/taverns/distributors
 *  that Sales sells the beer to. UserAccount is the model for usernames/logins/etc.
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Entities
{
    public class AccountEntity
    {
        //[Required]
        public Guid Id { get; set; }

        public string AccountName { get; set; }

        public DateTime AddedDate { get; set; }

        public string LogoURI { get; set; }

        public string WebsiteURL { get; set; }

        public Guid AssociateFK { get; set; }

        public int AccountContactFK { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}
