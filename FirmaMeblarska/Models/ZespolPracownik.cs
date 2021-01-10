using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class ZespolPracownik
    {
        public int ZespolId { get; set; }
        
        public int PracownikId { get; set; }    

        

        public  Zespol Zespols { get; set; }
        public  Pracownik Pracowniks { get; set; }
    }
}
