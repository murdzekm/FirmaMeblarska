using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Pracownik
    {
        [Key]
        public int PracownikId { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(25)")]
        public string Imie { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(25)")]
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        public int Telefon { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Adres")]
        public int AdresId { get; set; }

        [NotMapped]
        public string Adres { get; set; }

        public Adres adres { get; set; }
        public ICollection<ZespolPracownik> ZespolPracownik { get; set; }
    }
}
