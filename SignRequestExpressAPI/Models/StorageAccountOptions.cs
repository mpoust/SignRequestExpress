////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/*
 * CIT498 - Senior Project - Fall 2018
 * 
 * FileName: StorageAccountOptions.cs
 * Author: Michael Poust
		   mbp3@pct.edu
 * Created On: 10/28/2018
 * Last Modified: 
 * Description: Options for storage account - Azure BLOB Storage 
 * 
 * References: https://blogs.msdn.microsoft.com/premier_developer/2017/03/14/building-a-simple-photo-album-using-azure-blob-storage-with-net-core/
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
    public class StorageAccountOptions
    {
        public string StorageAccountNameOption { get; set; }

        public string StorageAccountKeyOption { get; set; }

        public string FullTemplatesContainerNameOption { get; set; }

        public string ScaledTemplatesContainerNameOption { get; set; }

        public string FullAccountLogosContainerNameOption { get; set; }

        public string ScaledAccountLogosContainerNameOption { get; set; }
    }
}
