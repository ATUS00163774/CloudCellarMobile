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
    [QueryProperty(nameof(Models.Location), "CreateLocation")]
    public class EditLocationViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public Models.Location _location;
        public int Id
        {
            get => _location.Id;
            set
            {
                if (_location.Id != value)
                {
                    _location.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public int AreaId
        {
            get => _location.AreaId;
            set
            {
                if (_location.AreaId != value)
                {
                    _location.AreaId = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Name
        {
            get => _location.Name;
            set
            {
                if (_location.Name != value)
                {
                    _location.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EditLocationViewModel()
        {
            _location = new Models.Location();

            SubmitCommand = new Command(Submit);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand SubmitCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Submit()
        {
            int id = DataAccess.InsertLocation(_location);
            _location.Id = id;

            var navigationParameter = new ShellNavigationQueryParameters
        {
            { "CreatedLocation", _location }
        };
            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("CreateLocation"))
            {
                _location = query["CreateLocation"] as Models.Location;
            }
        }
    }
}
