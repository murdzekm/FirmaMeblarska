using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.ViewModels
{
    public class AdresVW
    {
        public int PracownikId { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Email { get; set; }
        public int Telefon { get; set; }       


        public int AdresId { get; set; }
        public string Miejscowosc { get; set; }
        public string Ulica { get; set; }
        public string NrDomu { get; set; }
        public string NrLokalu { get; set; }
        public string KodPocztowy { get; set; }
    }
}
