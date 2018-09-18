////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: BrandEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/18/2018
 * Last Modified:
 * Description: This is an entity that models an object from the Brand table from the database. 
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
    public class BrandEntity
    {
        [Required]
        public Guid Id { get; set; }

        public string BrandName { get; set; }

        public string BrandFlavors { get; set; } // comes as an XML object

        public DateTime ModifiedDateTime { get; set; }
    }
}
