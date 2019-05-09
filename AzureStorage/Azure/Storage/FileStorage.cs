using System;
using System.Collections.Generic;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace Azure.Storage
{
    public class FileStorage
    {
        #region Fields

        private readonly string _conectionString = CloudConfigurationManager.GetSetting("StorageAccountConnection");

        #endregion Fields

        /// <summary>
        /// Get files from azure file storage
        /// </summary>
        /// <param name="fileService">Unit name or File service</param>
        /// <param name="directory">File share folder (string.empty if not has folder)</param>
        /// <param name="fileName">File names with extension (i.e. file_name.extension)</param>
        /// <param name="downloadDataType">Download return data</param>
        /// <returns>File information in a specific format</returns>
        public T GetFileInformation<T>(string fileService, string directory, string fileName, DownloadDataType downloadDataType)
        {
            try
            {
                var context = FileReference(fileService);

                if (!context.Exists())
                {
                    throw new Exception("Error trying to retreive file data.");
                }

                var dir = GetDirectory(context, directory);

                if (!dir.Exists())
                {
                    throw new Exception("Directory entry does not exist");
                }

                var file = dir.GetFileReference(fileName);

                if (!file.Exists())
                {
                    throw new Exception("File searched does not exist");
                }

                return (T)GetData(file, downloadDataType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Upload file to azure file storage
        /// </summary>
        /// <param name="dataToUpload">file information to be upload</param>
        /// <param name="fileService">Unit name or File service</param>
        /// <param name="directory">File share folder (string.empty if not has folder)</param>
        /// <param name="fileName">File names with extension (i.e. file_name.extension)</param>
        /// <returns>file information in a specific format</returns>
        public void UploadFile(byte[] dataToUpload, string fileService, string directory, string fileName)
        {
            try
            {
                var context = FileReference(fileService);

                if (context.Exists())
                {
                    var dir = GetDirectory(context, directory);

                    if (!dir.Exists())
                    {
                        throw new Exception("Directory entry does not exist");
                    }

                    var file = dir.GetFileReference(fileName);

                    if (file.Exists())
                    {
                        file.DeleteIfExists();
                    }

                    file.UploadFromByteArray(dataToUpload, 0, dataToUpload.Length);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Delete a file from azure file storage
        /// </summary>
        /// <param name="fileService">Unit name or File service</param>
        /// <param name="directory">File share folder (string.empty if not has folder)</param>
        /// <param name="fileName">File names with extension (i.e. file_name.extension)</param>
        public void DeleteFile(string fileService, string directory, string fileName)
        {
            try
            {
                var context = FileReference(fileService);

                if (context.Exists())
                {
                    var dir = GetDirectory(context, directory);

                    if (!dir.Exists())
                    {
                        throw new Exception("Directory entry does not exist");
                    }

                    var file = dir.GetFileReference(fileName);

                    if (file.Exists())
                    {
                        file.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get directories entry
        /// </summary>
        /// <param name="fileService">Unit name or File service</param>
        /// <param name="directory">File share folder (string.empty if not has folder)</param>
        /// <returns>List directories</returns>
        public IEnumerable<IListFileItem> GetDirectoryList(string fileService, string directory)
        {
            try
            {
                var context = FileReference(fileService);

                if (context.Exists())
                {
                    var dir = GetDirectory(context, directory);

                    if (!dir.Exists())
                    {
                        throw new Exception("Directory entry does not exist");
                    }

                    return dir.ListFilesAndDirectories();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            throw new Exception("Error trying to retreive directories.");
        } 

        #region Private Methods

        /// <summary>
        /// Get current path of file service storage
        /// </summary>
        /// <param name="context">Unit name or File service</param>
        /// <param name="directory">File share folder</param>
        /// <returns>Specified directory</returns>
        private CloudFileDirectory GetDirectory(CloudFileShare context, string directory)
        {
            var rootDirectory = context.GetRootDirectoryReference();

            if (string.IsNullOrEmpty(directory))
            {
                return rootDirectory;
            }

            return rootDirectory.GetDirectoryReference(directory);
        }

        private object GetData(CloudFile file, DownloadDataType downloadDataType)
        {
            switch (downloadDataType)
            {
                case DownloadDataType.Text:
                    return file.DownloadText();

                case DownloadDataType.ByteArray:
                    var document = new byte[file.Properties.Length];

                    file.DownloadToByteArray(document, 0);

                    return document;

                default:
                    throw new ArgumentOutOfRangeException("downloadDataType");
            }
        }

        /// <summary>
        /// Get File storage reference
        /// </summary>
        /// <param name="fileService">Unit name or directory</param>
        /// <returns>Files directory</returns>
        private CloudFileShare FileReference(string fileService)
        {
            var storageAccount = CloudStorageAccount.Parse(_conectionString);
            var fileClient = storageAccount.CreateCloudFileClient();
            return fileClient.GetShareReference(fileService);
        }

        #endregion Private Methods

    }
}