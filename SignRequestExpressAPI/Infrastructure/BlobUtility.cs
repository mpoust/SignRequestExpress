// COMMENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SignRequestExpressAPI.Infrastructure
{
    public class BlobUtility
    {
        private readonly CloudBlobClient _blobClient;

        public BlobUtility(string accountName, string accountKey)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName="
                + accountName + ";AccountKey=" + accountKey);

            // Gain access to the containers and blobs in Azure storage account
            _blobClient = storageAccount.CreateCloudBlobClient();
        }
    }
}
