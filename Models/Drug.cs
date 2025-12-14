using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Drug
    {
        public int DrugId { get; set; }
        public string DrugName { get; set; }
        public decimal Dosage { get; set; }
        public string Form { get; set; }
        public string Manufacturer { get; set; }
        public bool PrescriptionRequired { get; set; }
    }
}
