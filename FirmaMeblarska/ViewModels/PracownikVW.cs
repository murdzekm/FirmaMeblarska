using FirmaMeblarska.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.ViewModels
{
    public class PracownikVW
    {
        public IEnumerable<Zespol> Zespol { get; set; }
        public IEnumerable<Pracownik> Pracownik { get; set; }
    }
}
