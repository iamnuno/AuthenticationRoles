using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YetAnotherDemo.Models;


namespace YetAnotherDemo.Services
{
    public class BlobStorageService
    {
        private readonly CloudStorageAccount _cloudStorageAccount;
        private readonly CloudBlobClient _blobClient;
        private readonly CloudBlobContainer _container;
        public BlobStorageService()
        {
            _cloudStorageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            _blobClient = _cloudStorageAccount.CreateCloudBlobClient();
        }

        public List<string> ListContainers()
        {

            return _blobClient.ListContainers().Select(x => x.Name).ToList();


        }

        
    }
}
