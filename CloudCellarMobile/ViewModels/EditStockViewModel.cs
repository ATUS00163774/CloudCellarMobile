using CloudCellarMobile.Data;
using CloudCellarMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CloudCellarMobile.ViewModels
{
    [QueryProperty(nameof(Stock), "Stock")]
    public class EditStockViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private Models.Stock _stock;
        public int ID => _stock.Id;
        public int ProductId => _stock.ProductId;
        public Product Product => _stock.Product;
        public int OutletId => _stock.OutletId;
        public int AuditId => _stock.AuditId;
        public int AreaId => _stock.AreaId;
        public int LocationId => _stock.LocationId;
        public double Measure
        {
            get => _stock.Measure;
            set
            {
                if (_stock.Measure != value)
                {
                    _stock.Measure = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Weight
        {
            get => _stock.Weight;
            set
            {
                if (_stock.Weight != value)
                {
                    _stock.Weight = value;
                    OnPropertyChanged();
                }
            }
        }
        public double CalculatedMeasure => _stock.CalculatedMeasure;
        public DateTime Time => _stock.Time;

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EditStockViewModel()
        {
            _stock = new Stock();

            SubmitCommand = new Command(Submit);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand SubmitCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void Submit()
        {
            int id = DataAccess.InsertStock(_stock);
            _stock.Id = id;

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CreatedStock", _stock }
            };
            Shell.Current.GoToAsync($"..", navigationParameter);
        }


        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Stock"))
            {
                Stock stock = query["Stock"] as Stock;
                _stock = stock;

                OnPropertyChanged(nameof(Product));
            }
        }
    }
}
