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

    }
}
