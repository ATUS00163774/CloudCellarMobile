using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCellarMobile.Models;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace CloudCellarMobile.ViewModels
{
    public class AuditTrailViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ObservableCollection<Stock> AllStocks { get; private set; }

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public AuditTrailViewModel()
        {
            AllStocks = new ObservableCollection<Stock>();
            BackCommand = new AsyncRelayCommand(Back);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand BackCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public async Task Back()
        {
            await Shell.Current.GoToAsync($"..");
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("Stocks"))
            {
                ObservableCollection<Stock> stocks = query["Stocks"] as ObservableCollection<Stock>;
                AllStocks = stocks;
                OnPropertyChanged(nameof(AllStocks));
            }
        }
    }
}
