using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaxationCard.Models
{
    public class Kvartal
    {
        public int Id { get; set; }
        public int NumKvartal { get; set; }
        public int CreateAcc { get; set; }
        public DateTime CreateDateKvartal { get; set; }
        public List<Videl> Videls { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

      
    }
}
