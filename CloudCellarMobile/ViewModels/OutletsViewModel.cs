using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudCellarMobile.Models;
using CloudCellarMobile.Data;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Collections;

namespace CloudCellarMobile.ViewModels
{
    [QueryProperty(nameof(Outlet), "CreatedOutlet")]
    public class OutletsViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ObservableCollection<Outlet> AllOutlets { get; set; }
        public Outlet SelectedOutlet { get; set; }
        public bool IsOutletSelected { get; set; }


        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public OutletsViewModel()
        {
            AllOutlets = DataAccess.GetOutlets();

            OutletSelectedCommand = new Command<Outlet>(OutletSelected);
            SelectCommand = new Command(Select);
            NewOutletCommand = new Command(NewOutlet);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand OutletSelectedCommand { get; private set; }
        public ICommand SelectCommand { get; private set; }
        public ICommand NewOutletCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void OutletSelected(Outlet outlet)
        {
            if (outlet != null)
            {
                SelectedOutlet = outlet;
                IsOutletSelected = true;
                OnPropertyChanged(nameof(IsOutletSelected));
            }
            else
            {
                IsOutletSelected = false;
                OnPropertyChanged(nameof(IsOutletSelected));
            }
        }

        private void Select()
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "SelectedOutlet", SelectedOutlet }
            };

            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        private void NewOutlet()
        {
            string create = "create";
            Shell.Current.GoToAsync($"{nameof(Views.EditOutlet)}?create={create}");
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("CreatedOutlet"))
            {
                Outlet outlet = query["CreatedOutlet"] as Outlet;
                Outlet matchedOutlet = AllOutlets.Where((o) => o.Id == outlet.Id).FirstOrDefault();

                if (matchedOutlet != null)
                {
                    // Product already exists
                }
                else
                    AllOutlets.Insert(0, outlet);
            }
        }
    }
}
