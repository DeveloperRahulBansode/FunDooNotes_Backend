using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAcessLayer.Entity
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        [Column("First_Name", TypeName = "varchar(100)")]
        public string FirstName { get; set; }
        [Column("Last_Name", TypeName = "varchar(100)")]
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        [Column("Email", TypeName = "varchar(100)")]
        public string Email { get; set; }
        [Required]
        [Column("Password", TypeName = "varchar(100)")]
        public string Password { get; set; }


        public virtual ICollection<Notes> Notes { get; set; } = new List<Notes>();
    }
}

