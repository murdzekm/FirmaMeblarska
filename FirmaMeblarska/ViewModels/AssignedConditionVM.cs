using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirmaMeblarska.ViewModels
{
    public class AssignedConditionVM
    {
        public int PracownikID { get; set; }
        public string PracownikNazwa { get; set; }
        public bool Assigned { get; set; }
    }
}
