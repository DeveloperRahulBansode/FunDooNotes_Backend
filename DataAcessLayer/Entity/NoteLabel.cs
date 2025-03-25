using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class NoteLabel
    {
        
        [Required]
        public int NotesId { get; set; }

        [Required]
        public int LabelId { get; set; }

        [ForeignKey(nameof(NotesId))]
        public virtual Notes Note { get; set; }

        [ForeignKey(nameof(LabelId))]
        public virtual Label Label { get; set; }
    }
}
