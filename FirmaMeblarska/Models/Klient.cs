using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Klient
    {
        [Key]
        public int KlientId { get; set; }
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
        [Column(TypeName = "nvarchar(15)")]
        public string Telefon { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Adres")]
        public int AdresId { get; set; }

       [NotMapped]
        public string Adress { get; set; }

        public string FullName
        {
            get
            {
                return Imie + " " + Nazwisko;
            }
        }
       

        public string Dane
        {
            get
            {
                return Imie + " " + Nazwisko + " " +Email + " " +Telefon ;
            }
        }
        public Adres Adres { get; set; }
        public ICollection<Zamowienie> Zamowienie { get; set; }
    }
}
