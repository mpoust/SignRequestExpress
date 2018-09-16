////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: TemplateEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified:
 * Description: This is an entity that models an object from the Template table from the database.
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

namespace SignRequestExpressAPI.Entities
{
    public class TemplateEntity
    {
        public Guid Id { get; set; }

        public Guid BrandFK { get; set; }

        public bool IsVertical { get; set; }

        public string Element { get; set; }

        public string ImageURI { get; set; }

        public DateTime ModifiedDT { get; set; }
    }
}
