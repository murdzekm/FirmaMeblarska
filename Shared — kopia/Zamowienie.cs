using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class Zamowienie
    {
        [Key]
        public int ZamowienieId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [DisplayName("Data złożenia")]
        [DataType(DataType.Date)]
        public DateTime DataZlozenia { get; set; }
        public int Cena { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Klient")]
        public int KlientId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Adres")]
        public int AdresId { get; set; }

        [Required(ErrorMessage = "Wymagane")]
        [Display(Name = "Zespół")]
        public int ZespolId { get; set; }

        

        [NotMapped]
        public string Statuss { get; set; }
        [NotMapped]
        public string Klients { get; set; }
        [NotMapped]
        [Display(Name = "Zespół")]
        public string Zespols { get; set; }
        [NotMapped]
        public string Adress { get; set; }
        public Zamowienie()
        {
            DataZlozenia = DateTime.Now;
        }

        
        
        public Status Status { get; set; }
        public Klient Klient { get; set; }

        public Adres Adres { get; set; }
        public Zespol Zespol { get; set; }
        public IList<ZamowieniePlyta> ZamowieniePlyta { get; set; }
        //public virtual ICollection<ZamowieniePlyta> ZamowieniePlyta { get; set; }
    }
}
