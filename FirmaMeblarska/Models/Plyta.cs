using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Plyta
    {
        [Key]
        public int PlytaId { get; set; }
        [Required(ErrorMessage = "Wymagane")]
        [Column(TypeName = "nvarchar(6)")]
        public string Kod { get; set; }
        [DisplayName("Ilość")]
        public int Ilosc { get; set; }

        public IList<ZamowieniePlyta> ZamowieniePlyta { get; set; }
        //public virtual ICollection<ZamowieniePlyta> ZamowieniePlyta { get; set; }
    }
}
