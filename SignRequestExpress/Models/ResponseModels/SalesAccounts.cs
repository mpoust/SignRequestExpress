////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: UserInfo.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/23/2018
 * Last Modified: 
 * Description: Model for Sales Accounts collected to populate account dropdown
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

namespace SignRequestExpress.Models.ResponseModels
{
    public class SalesAccounts : Resource
    {
        public string AccountName { get; set; }

        public DateTime AddedDate { get; set; }

        public string LogoURI { get; set; }

        public string WebsiteURL { get; set; }

        public Guid AssociateFK { get; set; }

        public int AccountContactFK { get; set; }
    }
}
