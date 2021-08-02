using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaxationCard.Models
{
  public class MaketThen
    {
            public int Id { get; set; }

            public double N = 10;

            public double Yarus { get; set; }

            public double Koeficent { get; set; }

            public int Vozrast { get; set; }

            public int Visota { get; set; }

            public int Diametr { get; set; }

            public double Polnota { get; set; }



            public int AccountEditId { get; set; }

            public int CurrentVidel { get; set; }

            public int CurrentKvartal { get; set; }

     


            public int MaketThenPorodaId { get; set; }
            public MaketThenPoroda MaketThenPoroda { get; set; }


    }
}
