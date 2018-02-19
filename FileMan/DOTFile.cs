using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileMan
{
    class DOTFile
    {
        public string FilePathAndName { get; set; }
        public string ParentFolder { get; set; }
        public string Name { get; set; }
        public string DateCreated { get; set; }
        public string DateLastAccessed { get; set; }
        public string DateLastModified { get; set; }
        public string Size { get; set; }
        public string Type { get; set; }
        public string Suffix { get; set; }
        public string Owner { get; set; }
        public string SectionNumber { get; set; }
        public string SR { get; set; }
        public string StudyType { get; set; }
        public string Location { get; set; }
        public string BeginningMilepost { get; set; }
        public string EndingMilepost { get; set; }
        public string FM { get; set; }
        public string Author { get; set; }
        public string KeyWords { get; set; }
        public string Comments { get; set; }
    }
}
