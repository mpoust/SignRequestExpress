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

        private readonly string TemplatesScaled = "templates-scaled";

        public BlobUtility(string accountName, string accountKey)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName="
                + accountName + ";AccountKey=" + accountKey);

            // Gain access to the containers and blobs in Azure storage account
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        // TODO: create Azure WebJob to resize images as they are uploaded.

        // TODO: create UploadBlod

        // TODO: Create return for other blob types

            /*
        public async Task<List<IListBlobItem>> GetBlobs(string blobContainer)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(blobContainer);
            await container.CreateIfNotExistsAsync();

            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            BlobContinuationToken continuationToken = null;
            List<IListBlobItem> blobItems = new List<IListBlobItem>();

            do
            {
                var response = await container.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                blobItems.AddRange(response.Results);
            }
            while (continuationToken != null);

            return blobItems;
        }
        */

        // Directory is the value from the dropdown list in the UI - mapped to brandname in blob hierarchy
        public async Task<List<IListBlobItem>> GetTemplateBlobsByBrand(string blobDirectory)
        {
            CloudBlobContainer container = _blobClient.GetContainerReference(TemplatesScaled);
            await container.CreateIfNotExistsAsync();

            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            // TODO: add a switch here for matching the UI List heading to the actual blob directory
            blobDirectory = blobDirectory.ToLower();

            CloudBlobDirectory directory = container.GetDirectoryReference(blobDirectory);

            BlobContinuationToken continuationToken = null;
            List<IListBlobItem> blobItems = new List<IListBlobItem>();

            do
            {
                // TODO: could filter blobs here? Only return the list with the flavor??
                var response = await directory.ListBlobsSegmentedAsync(continuationToken);
                continuationToken = response.ContinuationToken;
                blobItems.AddRange(response.Results);
            }
            while (continuationToken != null);

            return blobItems;
        }
    }
}
