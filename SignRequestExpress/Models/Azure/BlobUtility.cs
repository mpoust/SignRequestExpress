// COMMENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace SignRequestExpress.Models.Azure
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

        // TODO: create Azure WebJob to resize images as they are uploaded.

        // TODO: create UploadBlod

        public async Task<List<IListBlobItem>> GetBlobs(string blobContainer, string blobDirectory)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(blobContainer);
            await container.CreateIfNotExistsAsync();

            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            CloudBlobDirectory directory = container.GetDirectoryReference(blobDirectory);

            BlobContinuationToken continuationToken = null;
            List<IListBlobItem> blobItems = new List<IListBlobItem>();

            do
            {
                var response = await directory.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                blobItems.AddRange(response.Results);
            }
            while (continuationToken != null);

            return blobItems;
        }
    }
}
