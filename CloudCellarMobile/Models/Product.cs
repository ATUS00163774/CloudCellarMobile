using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudCellarMobile.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Barcode { get; set; }
        public int FullWeight { get; set; }
        public int EmptyWeight { get; set; }

        public Product()
        {

        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}ml", Brand, Name, Size); ;
        }

        public static ObservableCollection<string> Categories()
        {
            return new ObservableCollection<string>()
            {
               "Draught",
               "Bottled Beers",
               "Spirits",
               "Minerals",
               "Large Minerals",
               "Wine",
               "Champagne"
            };
        }
    }
}
