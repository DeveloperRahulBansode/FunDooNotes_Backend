using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class Notes
    {
        [Key]
        public int NotesId { get; set; }
        [Required]
        [Column("Title", TypeName = "varchar(100)")]
        public string Title { get; set; }

        [Column("Description", TypeName = "text")]
        public string Description { get; set; }

        [Column("Color", TypeName = "varchar(100)")]
        public string Color { get; set; } 

        [Column("IsTrash")]
        public bool IsTrash { get; set; }

        [Column("IsArchive")]
        public bool IsArchive { get; set; }


        [Required]
        public int UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public virtual User User { get; set; }

        public virtual ICollection<NoteLabel> NoteLabels { get; set; } = new List<NoteLabel>();

    }
}
