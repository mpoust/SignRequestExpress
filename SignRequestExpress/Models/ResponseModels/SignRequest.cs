﻿////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: SignRequest.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 11/07/2018
 * Last Modified: 
 * Description: Model for Sign Requests collected to populate Admin and Sign Shop views
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
    public class SignRequest : Resource
    {
        public SignRequest()
        {

        }

        public SignRequest(Dictionary<string,object> dict)
        {
            RequestNumber = (string)dict["requestNumber"];
            Reason = (string)dict["reason"];
            Status = Convert.ToByte(dict["status"]);
            RequestedDate = Convert.ToDateTime(dict["requestedDate"]);
            NeededDate = Convert.ToDateTime(dict["neededDate"]);
            // Not including ApprovalFK at this time - handle as a string?
            IsProofNeeded = (bool)dict["isProofNeeded"];
            MediaFK = Convert.ToByte(dict["mediaFK"]);
            Quantity = Convert.ToByte(dict["quantity"]);
            HeightInch = Convert.ToDecimal(dict["heightInch"]);
            WidthInch = Convert.ToDecimal(dict["widthInch"]);
            TemplateFK = (string)dict["templateFK"];
        }

        public string RequestNumber { get; set; }

        public string Reason { get; set; }

        public byte Status { get; set; }

        public DateTime RequestedDate { get; set; }

        public DateTime NeededDate { get; set; }

        public string ApprovalFK { get; set; }

        public bool IsProofNeeded { get; set; }

        public byte MediaFK { get; set; }

        public byte Quantity { get; set; }

        public decimal HeightInch { get; set; }

        public decimal WidthInch { get; set; }

        public string TemplateFK { get; set; }

        public string Information { get; set; }

        public string DataFileURI { get; set; }

        public string ImageURI { get; set; }

        public string RequestImageURI { get; set; }
    }
}