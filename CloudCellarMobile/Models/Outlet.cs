using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCellarMobile.Models
{
    public class Outlet
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactEmail { get; set; }

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}
