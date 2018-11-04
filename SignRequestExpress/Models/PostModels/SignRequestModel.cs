////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: SignRequestForm.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/26/2018
 * Last Modified: 
 * Description: Form to connect input on Sign Request Page
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
    public class SignRequestModel
    {
        /* // This didn't do a thing
        public SignRequestModel()
        {
            NeededDate = DateTime.Now.AddDays(14);
        }
        */

        [Required(ErrorMessage = "Please select an Account.")]
        [Display(Name = "Account Name:")]
        public string AccountName { get; set; }

        [Display(Name = "Reason:")]
        public string Reason { get; set; }

        // TODO: Implement logic, hardcoded as 0 in API controller
        //public int UserNumber { get; set; }

        //[Required] // Not required because if empty a 2 week default will be applied
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{dd-MM-yyyy}")]
        [Display(Name = "Date Needed:")]
        public string NeededDate { get; set; }

        [Display(Name = "Proof Before Print?")]
        public bool IsProofNeeded { get; set; }

        [Required(ErrorMessage = "Media Type is required")]
        [Display(Name = "Media Tpe:")]
        public byte MediaFK { get; set; }

        [Display(Name = "Quantity:")]
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")]
        public byte Quantity { get; set; }

        [Required(ErrorMessage = "Please enter the Height.")]
        [Display(Name = "Height (in inches):")]
        public short HeightInch { get; set; }

        [Required(ErrorMessage = "Please enter the Width.")]
        [Display(Name = "Width (in inches):")]
        public short WidthInch { get; set; }

        [Display(Name = "Template:")]
        public Guid Template { get; set; }

        [Display(Name = "Sign Details:")]
        public string Information { get; set; }

        [Display(Name = "Attach File:")]
        public string DataFileUri { get; set; } // formatting change?

        [Display(Name = "Account Logo / Imagery:")]
        public string ImageUri { get; set; } // formatting change?

        // Only used for user error display
        [Required(ErrorMessage = "Please select a Brand.")]
        public string Brand { get; set; }
    }
}
