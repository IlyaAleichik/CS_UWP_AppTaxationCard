using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AppTaxationCard.Models
{
  public class Account
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        [RegularExpression(@"^[А-Яа-яA-Za-z0-9]+$", ErrorMessage = "Логин должен содрежать только буквы анг и рус афавита и цифры")]
        public string Username { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$", ErrorMessage = "Имя долно содержать только буквы анг и рус афавита")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        [RegularExpression(@"^[А-Яа-яA-Za-z]+$", ErrorMessage = "Фамилия должна содержать только буквы анг и рус афавита")]
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 4)]
        public string Password { get; set; }
        public DateTime CreateTime { get; set; }
        public string ProfileImage { get; set; }

        public List<Videl> Videls { get; set; }

    }
}
