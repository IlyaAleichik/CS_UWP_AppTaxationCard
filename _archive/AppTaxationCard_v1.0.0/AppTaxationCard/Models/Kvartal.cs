using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaxationCard.Models
{
    public class Kvartal
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1, 10000)]
        public int NumKvartal { get; set; }
        public int CreateAcc { get; set; }
        public DateTime CreateDateKvartal { get; set; }
        public List<Videl> Videls { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

      
    }
}
