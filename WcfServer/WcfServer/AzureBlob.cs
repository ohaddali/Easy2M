using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WcfServer
{
    public class AzureBlob
    {
        private CloudBlobContainer container;

        public AzureBlob()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container.
            container = blobClient.GetContainerReference("reports");

            // Create the container if it doesn't already exist.
            container.CreateIfNotExists();
        }

        public async Task<string> uploadFileAsync(string filePath , string fileName)
        {
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(fileName);

            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

            var blobUrl = blockBlob.Uri.AbsoluteUri;

            return blobUrl;
        }

    }
}