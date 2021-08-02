using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaxationCard.Models
{
  public class MaketThen
    {
        [Required]
        public int Id { get; set; }

        public double N = 10;
        [Required]
        [Range(1, 10000)]
        public double Yarus { get; set; }
        [Required]
        [Range(0, 10)]
        public double Koeficent { get; set; }
        [Required]
        [Range(0, 160)]
        public int Vozrast { get; set; }
        [Required]
        [Range(0, 40)]
        public int Visota { get; set; }
        [Required]
        [Range(0, 10000)]
        public int Diametr { get; set; }
        [Required]
        [Range(0, 1)]
        public double Polnota { get; set; }

        public int PorodaId { get; set; }
        public Poroda Poroda { get; set; }

        public int CurrentAccountID { get; set; }

        public int CurrentVidelID { get; set; }

        public int CurrentKvartalID { get; set; }


    }
}
