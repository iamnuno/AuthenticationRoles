using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;

namespace YetAnotherDemo.Models
{
    public class BlobStorageModels
    {
        public FileDetails File { get; set; }
        public Files Files { get; set; }
        public SearchTerm SearchTerm { get; set; }
        public FileTableEntity FileTableEntity { get; set; }
        public ListFileTableEntity ListFileTableEntity { get; set; }
        public FileDelete FileDelete { get; set; }

        public BlobStorageModels()
        {
            File = new FileDetails();
            Files = new Files();
            SearchTerm = new SearchTerm();
            FileTableEntity = new FileTableEntity();
            ListFileTableEntity = new ListFileTableEntity();
            FileDelete = new FileDelete();
        }
    }

    public class FileDetails
    {
        public string Owner { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string DataFormat { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Author { get; set; }
        public IFormFile File { get; set; }

    }

    public class Files
    {
        public List<FileDetails> FilesList { get; set; }
    }

    public class SearchTerm
    {
        public string Search { get; set; }
    }

    public class FileTableEntity : TableEntity
    {

        private static int counter = 0;
        public string Owner { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string DataFormat { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Author { get; set; }

        public FileTableEntity()
        {
            RowKey = System.Threading.Interlocked.Increment(ref counter).ToString();
        }

    }

    public class ListFileTableEntity
    {
        public List<FileTableEntity> FileTableEntitiesList { get; set; }
    }

    public class FileDelete
    {
        public string ID { get; set; }
    }
}
