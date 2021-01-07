using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Narzedzie
    {
        [Key]
        public int NarzedzieId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(30)")]
        public string Nazwa { get; set; }

        [DisplayName("Numer Seryjny")]
        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(20)")]
        public string NrSeryjny { get; set; }
    }
}
