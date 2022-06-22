using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Aims.Core.Models
{
    public class LstNotes
    {
        public Guid Id { get; set; }
        public Int16 LineNum { get; set; }
        public Guid ParentId { get; set; }
        public string NotesRef { get; set; }
        public string Notes { get; set; }
        
        [NotMapped]
        public int ActionFlag { get; set; }
    }
}
