using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class LabelModel
    {
        [Required]
        public int LabelId { get; set; }

        [Required]
        public string LabelName { get; set; }

        [Required]
        public int UserID { get; set; } 

    }
}
