using System.Collections.Generic;

namespace AppTaxationCard.Models
{
    public class TypeErrosion
    {
        public int Id { get; set; }
        public string NameTypeErrosion { get; set; }

        public List<Videl> Videls { get; set; }
    }
}