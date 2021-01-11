using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.Models
{
    public class PracownikMaszyna
    {
        public int PracownikId { get; set; }
        public int MaszynaId { get; set; }

        public Maszyna Maszynas { get; set; }
        public Pracownik Pracowniks { get; set; }
    }
}
