using System.Collections.Generic;

namespace AppTaxationCard.Models
{
    public class Forestry
    {
        public int Id { get; set; }
        public string NameForestry { get; set; }

        public List<Videl> Videls { get; set; }
    }
}