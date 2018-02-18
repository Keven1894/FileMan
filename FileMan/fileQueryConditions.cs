using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMan
{
    class FileQueryConditions
    {
        public string FileName { get; set; }
        public string[] FileNameNotContain { get; set; }
        public string ParentFolder { get; set; }
        public string FM { get; set; }
        public string SR { get; set; }
        public string StudyType { get; set; }
        public string Description { get; set; }
        public DateTime DateCreatedTimeFrom { get; set; }
        public DateTime DateCreatedTimeTo { get; set; }
        public DateTime DateLastAccessedTimeFrom { get; set; }
        public DateTime DateLastAccessedTimeTo { get; set; }
        public DateTime DateLastModifiedTimeFrom { get; set; }
        public DateTime DateLastModifiedTimeTo { get; set; }
        public string SizeLowerBound { get; set; }
        public string SizeUpperBound { get; set; }
        public string Type { get; set; }
        public string Suffix { get; set; }
        public string Owner { get; set; }
    }
}
