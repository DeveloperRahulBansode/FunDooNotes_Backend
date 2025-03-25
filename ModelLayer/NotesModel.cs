using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class NotesModel
    {
        public string Title { get; set; }

        public string Description { get; set; } = String.Empty;

        public string Color { get; set; } = String.Empty;

        [DefaultValue(false)]
        public bool IsTrash { get; set; }

        [DefaultValue(false)]
        public bool IsArchive { get; set; }
    }
}
