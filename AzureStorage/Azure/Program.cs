using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Azure.Queues;
using Azure.Storage;
using Azure.TableEntities;

namespace Azure
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var storage = new QueueStorage();

            //var ttt = storage.GetQueueMessagesAsObject<User>("synchronization");

            var t = new List<string>();

            t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));
            //t.Add(string.Format("MerchantId: {0}, TenantId: {1}", Guid.NewGuid(), Guid.NewGuid()));

            //storage.AddQueueToStorage("synchronization", t);
            //var td = storage.GetQueueMessageAsText("synchronization");
            //storage.GetQueueMessageText<User>("synchronization");
        }
    }
}
