using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCellarMobile.Models
{
    public class Audit
    {
        public int Id { get; set; }
        public int OutletId { get; set; }
        public int AuditNo { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string ToString()
        {
            return String.Format("{0}: {1} - {2}", AuditNo, StartDate.ToString("dd/MM/yy"), EndDate.ToString("dd/MM/yy"));
        }

        public string ToPrettyString()
        {
            return String.Format("{0} - {1}", StartDate.ToString("MMMM dd, yyyy"), EndDate.ToString("MMMM dd, yyyy"));
        }

        public string ToShortString()
        {
            return String.Format("{0}", EndDate.ToString("MMMM dd, yyyy"));
        }
    }
}
