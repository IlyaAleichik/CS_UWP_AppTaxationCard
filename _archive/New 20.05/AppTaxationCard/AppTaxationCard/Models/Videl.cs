using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaxationCard.Models
{


 

 

  






    public class Videl : IComparable
    {
        public int Id { get; set; }
        public int NumVidel { get; set; }
        public int Area { get; set; }
        public int Krut { get; set; }

        public int TypeErrosionId { get; set; }
        public TypeErrosion TypeErrosion { get; set; }

        public int DegreeErrosionId { get; set; }
        public DegreeErrosion DegreeErrosion { get; set; }

        public int ExpositionSlopeId { get; set; }
        public ExpositionSlope ExpositionSlope { get; set; }

        public int TypeOrlId { get; set; }
        public TypeOrl TypeOrl { get; set; }

        public int TypeEarthId { get; set; }
        public TypeEarth TypeEarth { get; set; }

        public int ForestryId { get; set; }
        public Forestry Forestry { get; set; }

   

        public DateTime CreateDateVidel { get; set; }


        public int KvartalId { get; set; }
        public Kvartal Kvartal { get; set; }

        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int CompareTo(object o)
        {
            Videl p = o as Videl;
            if (p != null)
                return this.TypeEarthId.CompareTo(p.TypeEarthId);
            else
                throw new Exception("Невозможно сравнить два объекта");
        }

    }

  
}
