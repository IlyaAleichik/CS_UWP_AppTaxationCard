using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTaxationCard.Models
{
    public class Poroda
    {
        public int Id { get; set; }
        public string NamePoroda { get; set; }

        public List<MaketThen> MaketThens { get; set; }
    }
}
