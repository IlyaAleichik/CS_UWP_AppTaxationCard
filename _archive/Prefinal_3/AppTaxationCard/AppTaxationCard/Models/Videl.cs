using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace AppTaxationCard.Models
{













    public class Videl : IComparable
    {
        [Required]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [Range(1, 10000)]
        [DataMember]
        public int NumVidel { get; set; }

        [Required]
        [Range(1, 100)]
        [DataMember]
        public int Area { get; set; }

        [Required]
        [Range(0, 90)]
        [DataMember]
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


        [DataMember]
        public DateTime CreateDateVidel { get; set; }

        [DataMember]
        public int KvartalId { get; set; }
        public Kvartal Kvartal { get; set; }

        [DataMember]
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
