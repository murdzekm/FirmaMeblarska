using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class ZamowieniePlyta
    {
                    
        public int ZamowienieId { get; set; }       
                
       public int PlytaId { get; set; }        
       
        public Zamowienie Zamowienie { get; set; }
        public Plyta Plyta { get; set; }
    }
}
