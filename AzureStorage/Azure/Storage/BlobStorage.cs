using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;

namespace Azure.Storage
{
    public class BlobStorage
    {
        #region Fields

        private readonly string _conectionString = CloudConfigurationManager.GetSetting("StorageAccountConnection");

        #endregion Fields

        /// <summary>
        /// Get files from azure blob storage
        /// </summary>
        /// <param name="container">Azure storage Blob container name</param>
        /// <param name="blobName">blob file name</param>
        /// <param name="downloadDataType">Download return data</param>
        /// <returns>Blob information in a specific format</returns>
        public T GetBlobInformation<T>(string container, string blobName, DownloadDataType downloadDataType)
        {
            try
            {
                var blobContainer = BlobReference(container);

                if (!blobContainer.Exists())
                {
                    throw new Exception("Blob Container does not exist.");
                }

                var blob = blobContainer.GetBlockBlobReference(blobName);

                if (!blob.Exists())
                {
                    throw new Exception("Blob does not exist.");
                }

                return (T)GetData(blob, downloadDataType);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Upload file to azure Blob storage
        /// </summary>
        /// <param name="dataToUpload">file information to be upload</param>
        /// <param name="container">Azure storage Blob container name</param>
        /// <param name="blobName">blob file name</param>
        public void UploadBlob(byte[] dataToUpload, string container, string blobName)
        {
            try
            {
                var blobContainer = BlobReference(container);

                if (!blobContainer.Exists())
                {
                    throw new Exception("Blob Container does not exist.");
                }

                var blob = blobContainer.GetBlockBlobReference(blobName);

                if (blob.Exists())
                {
                    blob.DeleteIfExists();
                }

                blob.UploadFromByteArray(dataToUpload, 0, dataToUpload.Length);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete a file from azure blob storage
        /// </summary>
        /// <param name="container">Azure storage Blob container name</param>
        /// <param name="blobName">blob file name</param>
        public void DeleteBlob(string container, string blobName)
        {
            try
            {
                var blobContainer = BlobReference(container);

                if (!blobContainer.Exists())
                {
                    throw new Exception("Blob Container does not exist.");
                }

                var blob = blobContainer.GetBlockBlobReference(blobName);

                if (blob.Exists())
                {
                    blob.Delete();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// List of Blobs in the current container
        /// </summary>
        /// <param name="container">Azure storage Blob container name</param>
        /// <returns>Returns an enumerable collection of the blobs in the container</returns>
        public IEnumerable<IListBlobItem> GetDirectoryList(string container)
        {
            try
            {
                var blobContainer = BlobReference(container);

                if (!blobContainer.Exists())
                {
                    throw new Exception("Blob Container does not exist.");
                }

                return blobContainer.ListBlobs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Private Methods

        private object GetData(CloudBlockBlob blob, DownloadDataType downloadDataType)
        {
            switch (downloadDataType)
            {
                case DownloadDataType.Text:
                    return blob.DownloadText();

                case DownloadDataType.ByteArray:
                    var document = new byte[blob.Properties.Length];

                    blob.DownloadToByteArray(document, 0);

                    return document;

                default:
                    throw new ArgumentOutOfRangeException("downloadDataType");
            }
        }

        /// <summary>
        /// Get File storage reference
        /// </summary>
        /// <param name="container">Blob Container</param>
        /// <returns>Blob Container</returns>
        private CloudBlobContainer BlobReference(string container)
        {
            var storageAccount = CloudStorageAccount.Parse(_conectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient.GetContainerReference(container);
        }

        #endregion Private Methods
    }
}
