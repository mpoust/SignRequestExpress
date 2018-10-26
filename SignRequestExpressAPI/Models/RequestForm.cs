////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RequestForm.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/21/2018
 * Last Modified: 10/26/2018
 * Description: This class represents the JSON data that a client needs to POST to the /requests/{requestNumber} route.
 *      Contains all relevant request data.
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

namespace SignRequestExpressAPI.Models
{
    public class RequestForm
    {
        // What properties do I need here? Which once can I auto generate to include in the database
        // does this happen here or in the SPA??

        [Required]
        public Guid UserId { get; set; }

        [Display(Name ="reason", Description ="Reason for request")]
        public string Reason { get; set; }

        /*
        [Required]
        [Display(Name = "status", Description = "Status of request")] // probably dont need status here, just generate it within DefaultRequestService
        [Range(0, 3, ErrorMessage = "Status must be between 0 and 3")]
        public byte? Status { get; set; }
        */


         /*
        [Required]
        [Display(Name = "requestDate", Description = "Date request was submitted")]
        public DateTime? RequestedDate { get; set; }
        */

        [Required]
        [Display(Name = "neededDate", Description = "Date request is needed")]
        public DateTime? NeededDate { get; set; }
       

        /*
        [Required]
        [Display(Name = "approval", Description = "Approval status")]
        [Range(0, 3, ErrorMessage = "Approval must be between 0 and 3")]
        public Guid? Approval { get; set; }
        */

        [Required]
        [Display(Name = "isProofNeeded", Description = "Should a proof be generated first")]
        public bool? IsProofNeeded { get; set; }

        [Required]
        [Display(Name = "mediaFK", Description = "Type of paper")]
        [Range(1, 4, ErrorMessage = "Media type must be between 1 and 4")]
        public byte? MediaFK { get; set; }

        [Required]
        [Display(Name = "quantity", Description = "How many to print")]
        [Range(1, 100, ErrorMessage = "Quantity cannot be 0 or greater than 100")]
        public byte? Quantity { get; set; }

        [Display(Name = "isVertical", Description = "Is the sign vertical")]
        public bool? IsVertical { get; set; }

        [Required]
        [Display(Name = "heightInch", Description = "Height of sign in inches")]
        [Range(1, 384, ErrorMessage = "Height must be greater than 0 and less than 384 inches. Signs cannot be made larger than 32 feet.")]
        public short? HeightInch { get; set; }

        [Required]
        [Display(Name = "widthInch", Description = "Width of sign in inches")]
        [Range(1, 384, ErrorMessage = "Width must be greater than 0 and less than 384 inches. Signs cannot be made larger than 32 feet.")]
        public short? WidthInch { get; set; }

        [Display(Name = "template", Description = "Foreign key of template for the request")]
        public Guid? Template { get; set; }

        [Display(Name = "information", Description = "Information to include on sign")]
        public string Information { get; set; }

        [Display(Name = "dataFileURI", Description = "URI of data file Blob with information to include")]
        public string DataFileURI { get; set; }

        [Display(Name = "imageURI", Description = "URI of additional image Blob to include")]
        public string ImageURI { get; set; }

        [Display(Name = "modifiedDateTime", Description = "Timestamp of most recent modification to request")]
        public DateTime ModifiedDateTime { get; set; }
    }
}
