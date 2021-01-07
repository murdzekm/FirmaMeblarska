using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Magazyn
    {
        [Key]
        public int CzescId { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(80)")]        
        
        public string Nazwa { get; set; }
        [DisplayName("Ilość")]
        public int Ilosc { get; set; }
    }
}
