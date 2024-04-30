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
    [QueryProperty(nameof(Area), "CreateArea")]
    public class EditAreaViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public Area _area;
        public int Id
        {
            get => _area.Id;
            set
            {
                if (_area.Id != value)
                {
                    _area.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public int OutletId
        {
            get => _area.OutletId;
            set
            {
                if (_area.OutletId != value)
                {
                    _area.OutletId = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Name
        {
            get => _area.Name;
            set
            {
                if (_area.Name != value)
                {
                    _area.Name = value;
                    OnPropertyChanged();
                }
            }
        }

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EditAreaViewModel()
        {
            _area = new Area();

            SubmitCommand = new Command(Submit);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand SubmitCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Submit()
        {
            int id = DataAccess.InsertArea(_area);
            _area.Id = id;

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CreatedArea", _area }
            };
            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("CreateArea"))
            {
                _area = query["CreateArea"] as Area;
            }
        }
    }
}
