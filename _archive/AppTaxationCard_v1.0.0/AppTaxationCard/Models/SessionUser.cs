using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaxationCard.Models
{
    public class SessionUser
    {
        public int Id { get; set; }
        public int SId { get; set; }
        public string Username { get; set; }
        public DateTime SessionTime { get; set; }
    }
}
