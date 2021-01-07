using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Adres
    {
        [Key]
        public int AdresId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(50)")]
        [DisplayName("Miejscowość")]
        public string Miejscowosc { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(50)")]
        public string Ulica { get; set; }
        [DisplayName("Nr domu")]
        public string NrDomu { get; set; }
        [DisplayName("Nr lokalu")]
        public string NrLokalu { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(6)")]
        [DisplayName("Kod pocztowy")]
        public string KodPocztowy { get; set; }

        public ICollection<Klient> Klient { get; set; }
        public ICollection<Zamowienie> Zamowienie { get; set; }
        public ICollection<Pracownik> Pracownik { get; set; }
    }
}
