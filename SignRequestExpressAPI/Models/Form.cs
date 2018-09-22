////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Form.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/22/2018
 * Last Modified: 
 * Description: This helps serialize form data when sent to the client. 
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
    public class Form : Collection<FormField>
    {
        public const string Relation = "form";
        public const string EditRelation = "edit-form";
        public const string CreateRelation = "create-form";
        public const string QueryRelation = "query-form";
    }
}
