using System;
using System.Collections.Generic;
using System.Linq;
using Azure.TableEntities;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Azure.Storage
{
    public class TableStorage
    {
        #region Fields

        private readonly string _conectionString = CloudConfigurationManager.GetSetting("StorageAccountConnection");

        #endregion Fields

        /// <summary>
        /// Create a new table
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void CreateTable(string tableName)
        {
            try
            {
                var table = TableReference(tableName);

                table.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete an existing table
        /// </summary>
        /// <param name="tableName">Table name</param>
        public void DeleteTable(string tableName)
        {
            try
            {
                var table = TableReference(tableName);

                table.DeleteIfExists();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Enumerates all tables from current storage
        /// </summary>
        /// <returns>List of tables</returns>
        public IEnumerable<string> ListTables()
        {
            try
            {
                var client = TableClient();

                var tableNames = client.ListTables();

                return tableNames.Select(item => item.Name).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert or update a table row
        /// </summary>
        /// <param name="tableName">Table name</param>
        /// <param name="tableEntity">Entity to persist</param>
        public void InsertOrUpdateInformation(string tableName, ITableEntity tableEntity)
        {
            try
            {
                var table = TableReference(tableName);

                if (!table.Exists())
                {
                    throw new Exception("Table does not exist.");
                }

                var insertData = TableOperation.InsertOrMerge(tableEntity);

                table.Execute(insertData);
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        /// <summary>
        /// Retreive Entity information
        /// </summary>
        /// <typeparam name="TEntity">Table</typeparam>
        /// <param name="identifier">Entity Partition Key</param>
        /// <param name="rowKey">Entity Row Key</param>
        /// <returns>Entity information</returns>
        public TEntity GetEntity<TEntity>(string identifier, string rowKey) where TEntity : ITableEntity
        {
            try
            {
                var table = TableReference(typeof(TEntity).Name);

                if (!table.Exists())
                {
                    throw new Exception("Table does not exist.");
                }

                var entity = TableOperation.Retrieve<Merchant>(identifier, rowKey);

                var result = table.Execute(entity);

                return (TEntity) result.Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }   
        }

        /// <summary>
        /// Delete a specific data
        /// </summary>
        /// <param name="tableEntity">Entity to persist</param>
        public void DeleteRegister<TEntity>(ITableEntity tableEntity)
        {
            try
            {
                var table = TableReference(typeof(TEntity).Name);

                if (!table.Exists())
                {
                    throw new Exception("Table does not exist.");
                }

                var entity = TableOperation.Delete(tableEntity);

                table.Execute(entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private Methods

        /// <summary>
        /// Get Table storage reference
        /// </summary>
        /// <param name="tableName">Name of the table</param>
        /// <returns>Blob Container</returns>
        private CloudTable TableReference(string tableName)
        {
            var tableClient = TableClient();
            return tableClient.GetTableReference(tableName);
        }


        /// <summary>
        /// Get Table storage client
        /// </summary>
        /// <returns>Azure table client</returns>
        private CloudTableClient TableClient()
        {
            var storageAccount = CloudStorageAccount.Parse(_conectionString);
            return  storageAccount.CreateCloudTableClient();
        }

        #endregion Private Methods

    }
}
