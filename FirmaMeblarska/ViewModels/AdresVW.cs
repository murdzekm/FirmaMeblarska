using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.ViewModels
{
    public class AdresVW
    {         
        public int PracownikId { get; set; }
        [Required(ErrorMessage = "Wymagane")]        
        public string Imie { get; set; }

        [Required(ErrorMessage = "Wymagane")]        
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        public int Telefon { get; set; }

        public int AdresId { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        [DisplayName("Miejscowość")]
        public string Miejscowosc { get; set; }
        
        public string Ulica { get; set; }
        [DisplayName("Numer domu")]
        [Required(ErrorMessage = "Wymagane")]
        public string NrDomu { get; set; }
        [DisplayName("Numer lokalu")]
        public string NrLokalu { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        [DisplayName("Kod pocztowy")]
        public string KodPocztowy { get; set; }
    }
}
