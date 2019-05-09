using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure.TableEntities
{
    public class Reseller : TableEntity
    {
        public Guid ResellerId { get; set; }

        public Reseller(string identifier, string category)
        {
            PartitionKey = identifier;
            RowKey = category;
        }

        public Reseller()
        {
            
        }
    }
}
