using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCellarMobile.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int OutletId { get; set; }
        public int AuditId { get; set; }
        public int AreaId { get; set; }
        public int LocationId { get; set; }
        public double Measure { get; set; }
        public int Weight { get; set; }
        public double CalculatedMeasure { get; set; }
        public DateTime Time { get; set; }

        public Stock()
        {
 
        }

        public override string ToString()
        {
            //string stockString = "stockString";
            string stockString = String.Format("{0} - {1}", Product.ToString(), Measure);

            //if (Measure > 0 & Weight == 0)
            //    stockString = String.Format("{0} - {1}", Product.ToString(), Measure);

            //else if (Measure == 0 & Weight > 0)
            //    stockString = String.Format("{0} - {1}", Product.ToString(), Weight);

            return stockString;
        }
    }
}
