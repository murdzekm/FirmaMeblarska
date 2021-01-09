using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Zespol
    {
        [Key]
        public int ZespolId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(20)")]
        public string Nazwa { get; set; }

        public IList<ZespolPracownik> ZespolPracownik { get; set; }

        
        //public ICollection<Zamowienie> Zamowienie { get; set; }

    }
}
