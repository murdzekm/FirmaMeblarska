﻿
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
        public Zamowienie()
        {
            this.DataZlozenia = DateTime.Now;
            this.NrFaktura = DateTime.UtcNow.Date.Year.ToString() +
                DateTime.UtcNow.Date.Month.ToString() +
                DateTime.UtcNow.Date.Day.ToString() + Guid.NewGuid().ToString().Substring(0, 4).ToUpper();
        }

        [Key]
        public int ZamowienieId { get; set; }
        [Display(Name = "Numer faktury")]
        [Required]
        public string NrFaktura { get; set; }

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
        public string CenaNetto
        {
            get
            {
                var wynik = Cena -((Cena * 23) / 100);
                return wynik.ToString();
            }
        }
        public string KwotaVat
        {
            get
            {
                var wynik = (Cena * 23) / 100;
                return wynik.ToString();
            }
        }
        public Status Status { get; set; }
        public Klient Klient { get; set; }
        public Adres Adres { get; set; }
        public Zespol Zespol { get; set; }
        public IList<ZamowieniePlyta> ZamowieniePlyta { get; set; }
        //public virtual ICollection<ZamowieniePlyta> ZamowieniePlyta { get; set; }
    }
}
