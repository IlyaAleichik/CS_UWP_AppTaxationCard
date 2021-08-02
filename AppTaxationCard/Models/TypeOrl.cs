using System.Collections.Generic;

namespace AppTaxationCard.Models
{

    public class TypeOrl
    {
        public int Id { get; set; }
        public string NameTypeOrl { get; set; }

        public List<Videl> Videls { get; set; }
    }

}