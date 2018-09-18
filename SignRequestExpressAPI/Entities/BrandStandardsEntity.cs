////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: BrandStandardsEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified:
 * Description: This is an entity that models an object from the BrandStandards table from the database. 
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
    public class BrandStandardsEntity
    {
        [Required]
        public int Id { get; set; }

        public Guid BrandFK { get; set; }

        public string StandardURI { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}
