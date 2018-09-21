////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: RequestForm.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 9/21/2018
 * Last Modified: 
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
using System.Linq;
using System.Threading.Tasks;

namespace SignRequestExpressAPI.Models
{
    public class RequestForm
    {
           // What properties do I need here? Which once can I auto generate to include in the database
           // does this happen here or in the SPA??

        public string Reason { get; set; }
        
       // public byte 

        //public DateTime 
    }
}
