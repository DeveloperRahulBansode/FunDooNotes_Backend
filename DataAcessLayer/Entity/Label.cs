using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace DataAcessLayer.Entity
{
   public class Label
    {
    

        [Key]
        public int LabelId { get; set; }
        public string LabelName { get; set; }
        [ForeignKey("LabelNote")]
        
        [Required]
        public int UserId { get; set; }  

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

        public virtual ICollection<NoteLabel> NoteLabels { get; set; } = new List<NoteLabel>();

    }
}
