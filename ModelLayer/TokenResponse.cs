using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer
{
    public class TokenResponse
    {
                        
        public int UserID { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpiry { get; set; }
    }
}
