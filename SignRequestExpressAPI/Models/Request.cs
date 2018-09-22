﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: Request.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/20/2018
 * Last Modified: 
 * Description: Resource the API will return to the client - corresponding with RequestEntity.cs
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
    public class Request : Resource
    {
        public string RequestNumber { get; set; }

        public byte Status { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime NeededDate { get; set; }

        public Guid ApprovalFK { get; set; }

        public bool IsProofNeeded { get; set; }

        public byte MediaFK { get; set; }

        public byte Quantity { get; set; }

        public bool IsVertical { get; set; }

        public short HeightInch { get; set; }

        public short WidthInch { get; set; }

        public Guid? TemplateFK { get; set; }

        public string Information { get; set; }

        public string DataFileURI { get; set; }

        public string ImageURI { get; set; }

        public string RequestImageURI { get; set; }
    }
}