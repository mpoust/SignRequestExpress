////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Template.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/15/2018
 * Last Modified: 9/17/2018
 * Description: Resource the API will return to the client - corresponding with TemplateEntity.cs
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
    public class Template : Resource
    {
        public Guid BrandFK { get; set; }

        public bool IsVertical { get; set; }

        public string Element { get; set; }

        public string ImageURI { get; set; }
    }
}
