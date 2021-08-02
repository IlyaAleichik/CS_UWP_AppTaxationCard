using System.Collections.Generic;

namespace AppTaxationCard.Models
{
    public class DegreeErrosion
    {
        public int Id { get; set; }
        public string NameDegreeErrosion { get; set; }

        public List<Videl> Videls { get; set; }
    }

}