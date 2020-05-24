using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YetAnotherDemo.Models
{
    public class FileUploadModel
    {
        public string Owner { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string DataFormat { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Category { get; set; } // file can be categorized according to user roles

    }
}
