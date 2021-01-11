using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.ViewModels
{
    public class AdresKlientVW
    {
        public int KlientId { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        
        public string Imie { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        
        public string Nazwisko { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wymagane")]       
        public string Telefon { get; set; }      

        public int AdresId { get; set; }
        public string Miejscowosc { get; set; }
        public string Ulica { get; set; }
        public string NrDomu { get; set; }
        public string NrLokalu { get; set; }
        public string KodPocztowy { get; set; }
    }
}
