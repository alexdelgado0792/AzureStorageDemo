using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure.TableEntities
{
    public class Merchant : TableEntity
    {
        public Guid MerchantId { get; set; }
        public string Domain { get; set; }

        public Merchant(string identifier, string category)
        {
            PartitionKey = identifier;
            RowKey = category;
        }

        public Merchant()
        {
            
        }
    }
}