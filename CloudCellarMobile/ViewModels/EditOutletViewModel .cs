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
    public class EditOutletViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public Outlet _outlet;
        public int Id
        {
            get => _outlet.Id;
            set
            {
                if (_outlet.Id != value)
                {
                    _outlet.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Name
        {
            get => _outlet.Name;
            set
            {
                if (_outlet.Name != value)
                {
                    _outlet.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        public string ContactEmail
        {
            get => _outlet.ContactEmail;
            set
            {
                if (_outlet.ContactEmail != value)
                {
                    _outlet.ContactEmail = value;
                    OnPropertyChanged();
                }
            }
        }

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EditOutletViewModel()
        {
            _outlet = new Outlet();

            SubmitCommand = new Command(Submit);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand SubmitCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Submit()
        {
            int id = DataAccess.InsertOutlet(_outlet);
            _outlet.Id = id;

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CreatedOutlet", _outlet }
            };
            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("create"))
            {
                _outlet = new Outlet();
            }
        }
    }
}
