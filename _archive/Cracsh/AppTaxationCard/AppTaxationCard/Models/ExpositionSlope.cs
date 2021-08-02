using System.Collections.Generic;

namespace AppTaxationCard.Models
{
    public class ExpositionSlope
    {
        public int Id { get; set; }
        public string NameExpositionSlope { get; set; }

        public List<Videl> Videls { get; set; }
    }
}