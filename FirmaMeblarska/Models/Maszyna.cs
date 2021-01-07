using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Maszyna
    {
        [Key]
        public int MaszynaId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(40)")]
        public string Nazwa { get; set; }

        [DisplayName("Numer seryjny")]
        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(20)")]
        public string NrSeryjny { get; set; }

        [DisplayName("Data przegladu")]
        [Required(ErrorMessage = "Wymagane")]
        public DateTime DataPrzegladu { get; set; }
    }
}
