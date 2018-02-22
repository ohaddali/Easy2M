using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WcfServer
{
    public class AzureQueue
    {
        private CloudQueue queue;

        public AzureQueue()
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            queue = queueClient.GetQueueReference("reportsqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();
        }

        public void sendMessage(String text)
        {
            CloudQueueMessage message = new CloudQueueMessage(text);
            queue.AddMessage(message);
        }

        public string peekMessage()
        {
            CloudQueueMessage peekedMessage = queue.PeekMessage();
            return peekedMessage.AsString;
        }

        public string deleteMessage()
        {
            CloudQueueMessage retrievedMessage = queue.GetMessage();

            if (retrievedMessage == null)
                return null;

            queue.DeleteMessage(retrievedMessage);

            return retrievedMessage.AsString;
        }

        public async Task<string> deleteMessageAsync()
        {
            CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();

            if (retrievedMessage == null)
                return null;

            queue.DeleteMessage(retrievedMessage);

            return retrievedMessage.AsString;
        }
    }


}