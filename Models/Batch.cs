using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Batch
    {
        public string BatchId { get; set; }
        public int SupplierId { get; set; }
        public DateTime ReceiptDate { get; set; }
    }
}
