using System;
using System.Collections.Generic;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;

namespace Azure.Storage
{
    public class QueueStorage
    {
        #region Fields

        private readonly string _conectionString = CloudConfigurationManager.GetSetting("StorageAccountConnection");

        #endregion Fields

        /// <summary>
        /// Add new message to queue service
        /// </summary>
        /// <param name="queueService">Queue service</param>
        /// <param name="messages">Message to add</param>
        /// <param name="queueTimeLife">Queue message life time(Default 7 days)</param>
        public void AddQueueToStorage(string queueService, IEnumerable<string> messages, int queueTimeLife = 7)
        {
            try
            {
                var queue = QueueReference(queueService);

                if (!queue.Exists())
                {
                    throw new Exception("Queue storage does not exist.");
                }

                foreach (var message in messages)
                {
                    queue.AddMessage(new CloudQueueMessage(message), TimeSpan.FromDays(queueTimeLife));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get one message from the queue
        /// </summary>
        /// <param name="queueService">Queue service</param>
        public string GetQueueMessageAsText(string queueService)
        {
            try
            {
                var queue = QueueReference(queueService);

                if (!queue.Exists())
                {
                    throw new Exception("Queue storage does not exist.");
                }

                return queue.PeekMessage().AsString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get one message from the queue
        /// </summary>
        /// <param name="queueService">Queue service</param>
        public CloudQueueMessage GetQueueMessageAsQueue(string queueService)
        {
            try
            {
                var queue = QueueReference(queueService);

                if (!queue.Exists())
                {
                    throw new Exception("Queue storage does not exist.");
                }
                
                return queue.PeekMessage();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get one message from the queue (text in json format) as an specific object
        /// </summary>
        /// <param name="queueService">Queue service</param>
        public T GetQueueMessageAsObject<T>(string queueService)
        {
            try
            {
                var queue = QueueReference(queueService);

                if (!queue.Exists())
                {
                    throw new Exception("Queue storage does not exist.");
                }

                var msg = queue.PeekMessage().AsString;

                return (T)JsonConvert.DeserializeObject(msg, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// List of messages from the queue as text
        /// </summary>
        /// <param name="queueService">Queue service</param>
        public IEnumerable<string> GetQueueMessagesAsText(string queueService)
        {
            try
            {
                var queue = QueueReference(queueService);

                if (!queue.Exists())
                {
                    throw new Exception("Queue storage does not exist.");
                }

                var list = new List<string>();
                foreach (var item in queue.GetMessages(10, TimeSpan.FromSeconds(100)))
                {
                    list.Add(item.AsString);

                    queue.DeleteMessage(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// List of messages from the queue as Queue
        /// </summary>
        /// <param name="queueService">Queue service</param>
        public IEnumerable<CloudQueueMessage> GetQueueMessagesAsQueue(string queueService)
        {
            try
            {
                var queue = QueueReference(queueService);

                if (!queue.Exists())
                {
                    throw new Exception("Queue storage does not exist.");
                }

                var list = new List<CloudQueueMessage>();
                foreach (var item in queue.GetMessages(10, TimeSpan.FromSeconds(100)))
                {
                    list.Add(item);

                    queue.DeleteMessage(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// List of messages from the queue as Object
        /// </summary>
        /// <typeparam name="T">Class to be convert</typeparam>
        /// <param name="queueService">Queue service</param>
        public IEnumerable<T> GetQueueMessagesAsObject<T>(string queueService)
        {
            try
            {
                var queue = QueueReference(queueService);

                if (!queue.Exists())
                {
                    throw new Exception("Queue storage does not exist.");
                }

                var list = new List<T>();
                foreach (var item in queue.GetMessages(10, TimeSpan.FromSeconds(100)))
                {
                    list.Add((T)JsonConvert.DeserializeObject(item.AsString, typeof(T)));

                    queue.DeleteMessage(item);
                }

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private Methods

        /// <summary>
        /// Get queue storage reference
        /// </summary>
        /// <param name="queueService">Unit name or directory</param>
        /// <returns>queue reference</returns>
        private CloudQueue QueueReference(string queueService)
        {
            var storageAccount = CloudStorageAccount.Parse(_conectionString);
            var queueClient = storageAccount.CreateCloudQueueClient();
            return queueClient.GetQueueReference(queueService);
        }

        #endregion Private Methods
    }
}