////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: PostSignRequest.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/25/2018
 * Last Modified: 
 * Description: Model for user to POST to API during request creation
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

namespace SignRequestExpress.Models.PostModels
{
    // TODO: Add JSON validation?
    public class PostSignRequest
    {
        public Guid UserId { get; set; }

        public string Reason { get; set; }

        // TODO: Implement logic, hardcoded as 0 in API controller
        //public int UserNumber { get; set; }

        public DateTime NeededDate { get; set; }
        
        public bool IsProofNeeded { get; set; }

        public byte MediaFK { get; set; }

        public byte Quantity { get; set; }

        public bool IsVertical { get; set; }

        public short HeightInch { get; set; }

        public short WidthInch { get; set; }

        public Guid Template { get; set; }

        public string Information { get; set; }

        public string DataFileUri { get; set; }

        public string ImageUri { get; set; }
    }
}
