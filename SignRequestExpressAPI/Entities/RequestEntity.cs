﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RequestEntity.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 
 * Description: This entity represents the Request that Sales user will submit. 
 * 
 * References:
 *   
 * (c) Michael Poust, 2018
 */
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using SignRequestExpressAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Entities
{
    public class RequestEntity
    {
        public Guid Id { get; set; }

        public string RequestNumber { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime NeededDate { get; set; }

        public Guid ApprovalFK { get; set; }

        public bool IsProofNeeded { get; set; }

        public UInt16 MediaFK { get; set; }

        public UInt16 Quantity { get; set; }

        public bool IsVertical { get; set; }

        public UInt32 HeightInch { get; set; }

        public UInt32 WidthInch { get; set; }

        public Guid TemplateFK { get; set; }

        public string Information { get; set; }

        public string DataURI { get; set; }

        public string ImageURI { get; set; }

        public string RequestImageURI { get; set; }

        public DateTime ModifiedDateTime { get; set; }
    }
}
