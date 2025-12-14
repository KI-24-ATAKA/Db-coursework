using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Models
{
    public class DrugStock
    {
        public string BatchId { get; set; }
        public int WarehouseId { get; set; }
        public int DrugId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal Price { get; set; }
        public int PackageCount { get; set; }
        public int UnitCount { get; set; }
    }
}
