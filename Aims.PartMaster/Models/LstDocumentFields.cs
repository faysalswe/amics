using System;
using System.Collections.Generic;
using System.Text;

namespace Aims.Core.Models
{
    public class LstDocumentFields
    {
        public string Id { get; set; }
        public string docName { get; set; }
        public string ActionFlag { get; set; }
        public string LineNumber { get; set; }
        public string ParentId { get; set; }
        public string PODoc { get; set; }
        public string SODoc { get; set; }
        public string WODoc { get; set; }
    }
}
