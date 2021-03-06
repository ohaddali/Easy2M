﻿using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using System;
using System.Collections.Generic;
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
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the queue client.
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            queue = queueClient.GetQueueReference("reports queue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();
        }

        public void sendMessage(String text)
        {
            CloudQueueMessage message = new CloudQueueMessage(text);
            queue.AddMessage(message);
        }

        public String peekMessage()
        {
            CloudQueueMessage peekedMessage = queue.PeekMessage();
            return peekedMessage.AsString;
        }

        public String deleteMessage()
        {
            CloudQueueMessage retrievedMessage = queue.GetMessage();

            queue.DeleteMessage(retrievedMessage);

            return retrievedMessage.AsString;
        }

        public async Task<string> deleteMessageAsync()
        {
            CloudQueueMessage retrievedMessage = await queue.GetMessageAsync();

            queue.DeleteMessage(retrievedMessage);

            return retrievedMessage.AsString;
        }
    }


}