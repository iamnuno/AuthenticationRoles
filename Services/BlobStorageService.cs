using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherDemo.Models;


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

        public void UploadFile(string containerName, IFormFile file)
        {
            _container = _blobClient.GetContainerReference(containerName);
            CloudBlockBlob blockBlob = _container.GetBlockBlobReference(file.FileName);

            using Stream stream = file.OpenReadStream();
            blockBlob.UploadFromStream(stream);

        }

        /*
        public List<string> ListContainers()
        {
            return _blobClient.ListContainers().Select(x => x.Name).ToList();
        }
        */

        
    }
}
