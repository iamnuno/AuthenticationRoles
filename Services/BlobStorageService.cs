
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YetAnotherDemo.Models;
using CloudStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount;

namespace YetAnotherDemo.Services
{
    public class BlobStorageService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudBlobClient _blobClient;
        private CloudBlobContainer _container;

        public BlobStorageService()
        {
            _cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            _blobClient = _cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task UploadFile(string containerName, BlobStorageModels model)
        {
            // upload blob
            _container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(model.File.File.FileName);

            using Stream stream = model.File.File.OpenReadStream();
            blockBlob.UploadFromStream(stream);
            
            // update table storage
            Microsoft.Azure.Cosmos.Table.CloudStorageAccount cloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference("BlobDetails");
            await table.CreateIfNotExistsAsync();

            FileTableEntity newFile = new FileTableEntity
            {   
                PartitionKey = containerName,
                Owner = model.File.Owner,
                Url = blockBlob.Uri.AbsoluteUri,
                Type = model.File.Type,
                DataFormat = model.File.DataFormat,
                Date = model.File.Date,
                Location = model.File.Location,
                Author = model.File.Author
            };

            TableOperation insert = TableOperation.InsertOrMerge(newFile);
            TableResult result = await table.ExecuteAsync(insert);
            
        }

        public List<FileTableEntity> SearchFiles(string containerName, string searchTerm)
        {
            Microsoft.Azure.Cosmos.Table.CloudStorageAccount cloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference("BlobDetails");

            var files = table.ExecuteQuery(new TableQuery<FileTableEntity>()).ToArray();
            var filesFilteredByPartition = new List<FileTableEntity>();

            // only files from the correct "role" (container) can be displayed
            foreach (FileTableEntity f in files)
            {
                if (f.PartitionKey == containerName)
                {
                    filesFilteredByPartition.Add(f);
                }
            }

            // if no search term return all results
            if (searchTerm == null)
            {
                return filesFilteredByPartition;
            }

            var filesFilteredAfterSearchTerm = new List<FileTableEntity>();

            foreach (FileTableEntity f in filesFilteredByPartition)
            {
                if (f.Author == searchTerm
                    || f.Location == searchTerm
                    || f.Owner == searchTerm
                    || f.Type == searchTerm
                    || f.DataFormat == searchTerm)
                {
                    filesFilteredAfterSearchTerm.Add(f);
                }
            }

            return filesFilteredAfterSearchTerm;
        }

        public void DeleteFile(string container, string id, string user)
        {
            Microsoft.Azure.Cosmos.Table.CloudStorageAccount cloudStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.DevelopmentStorageAccount;
            CloudTableClient tableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference("BlobDetails");

            var files = table.ExecuteQuery(new TableQuery<FileTableEntity>()).ToArray();
            var filesFilteredByPartition = new List<FileTableEntity>();

            // make sure only files from the correct "role" are filetered
            foreach (FileTableEntity f in files)
            {
                if (f.PartitionKey == container)
                {
                    filesFilteredByPartition.Add(f);
                }
            }

            foreach (FileTableEntity f in filesFilteredByPartition)
            {
                if (f.RowKey == id && (f.Owner == user || user == "Admin")) // only file owner or admin can delete the file
                {   

                    // delete from storage table
                    TableOperation delete = TableOperation.Delete(f);
                    table.Execute(delete);

                    _container = _blobClient.GetContainerReference(container);
                    var blobs = _container.ListBlobs(null, false);

                    foreach (var item in blobs)
                    {
                        if (item.Uri.ToString() == f.Url)
                        {   
                            // delete from blob container
                            CloudBlockBlob blob = (CloudBlockBlob) item;
                            blob.DeleteIfExists();
                        }
                    }
                }
            }
        }
    }
}
