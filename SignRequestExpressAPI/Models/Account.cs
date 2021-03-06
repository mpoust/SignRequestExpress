﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Account.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified:
 * Description: This the Account resource that maps to the AccountEntity.
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public class Account : Resource
    {
        public string AccountName { get; set; }

        public DateTime AddedDate { get; set; }

        public string LogoURI { get; set; }

        public string WebsiteURL { get; set; }

        public Guid AssociateFK { get; set; }

        public int AccountContactFK { get; set; }
    }
}
