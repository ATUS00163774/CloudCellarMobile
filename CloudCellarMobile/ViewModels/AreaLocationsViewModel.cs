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
    [QueryProperty(nameof(Audit), "CreatedAudit"), QueryProperty(nameof(Outlet), "CurrentOutlet")]
    public class AreaLocationsViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ObservableCollection<Area> AllAreas { get; set; }
        public ObservableCollection<Models.Location> AllLocations { get; set; }
        public Outlet CurrentOutlet { get; set; }

        public Area SelectedArea { get; set; }
        public bool IsAreaSelected { get; set; }

        public Models.Location SelectedLocation { get; set; }
        public bool IsLocationSelected { get; set; }


        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public AreaLocationsViewModel()
        {
            AllAreas = new ObservableCollection<Area>();
            AllLocations = new ObservableCollection<Models.Location>();

            AreaSelectedCommand = new Command<Area>(AreaSelected);
            LocationSelectedCommand = new Command<Models.Location>(LocationSelected);
            SelectCommand = new Command(Select);
            NewAreaCommand = new Command(NewArea);
            NewLocationCommand = new Command(NewLocation);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand AreaSelectedCommand { get; private set; }
        public ICommand LocationSelectedCommand { get; private set; }
        public ICommand SelectCommand { get; private set; }
        public ICommand NewAreaCommand { get; private set; }
        public ICommand NewLocationCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void AreaSelected(Area area)
        {
            if (area != null)
            {
                SelectedArea = area;
                IsAreaSelected = true;
                OnPropertyChanged(nameof(IsAreaSelected));
                UpdateAllLocations();
            }
            else
            {
                IsAreaSelected = false;
                OnPropertyChanged(nameof(IsAreaSelected));
            }
        }

        private void LocationSelected(Models.Location location)
        {
            if (location != null)
            {
                SelectedLocation = location;
                IsLocationSelected = true;
                OnPropertyChanged(nameof(IsLocationSelected));
            }
            else
            {
                IsLocationSelected = false;
                OnPropertyChanged(nameof(IsLocationSelected));
            }
        }

        private void Select()
        {
            AreaLocation selectedAreaLocation = new AreaLocation() { Area = SelectedArea, Location = SelectedLocation };
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "SelectedAreaLocation", selectedAreaLocation }
            };

            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        private void NewArea()
        {
            Area CreateArea = new Area
            { 
                OutletId = CurrentOutlet.Id,
            };

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CreateArea", CreateArea }
            };

            Shell.Current.GoToAsync($"{nameof(Views.EditArea)}", navigationParameter);
        }

        private void NewLocation()
        {
            Models.Location CreateLocation = new Models.Location
            {
                AreaId = SelectedArea.Id,
            };

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CreateLocation", CreateLocation }
            };

            Shell.Current.GoToAsync($"{nameof(Views.EditLocation)}", navigationParameter);
        }

        private void UpdateAllAreas()
        {
            AllAreas = DataAccess.GetAreas(CurrentOutlet.Id);
            OnPropertyChanged(nameof(AllAreas));
        }

        private void UpdateAllLocations()
        {
            AllLocations = DataAccess.GetLocations(SelectedArea.Id);
            OnPropertyChanged(nameof(AllLocations));
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("CreatedArea"))
            {
                Area area = query["CreatedArea"] as Area;
                Area matchedArea = AllAreas.Where((o) => o.Id == area.Id).FirstOrDefault();

                if (matchedArea != null)
                {
                    // Product already exists
                }
                else
                    AllAreas.Insert(0, area);
            }

            if (query.ContainsKey("CreatedLocation"))
            {
                Models.Location location = query["CreatedLocation"] as Models.Location;
                Models.Location matchedLocation = AllLocations.Where((o) => o.Id == location.Id).FirstOrDefault();

                if (matchedLocation != null)
                {
                    // Product already exists
                }
                else
                    AllLocations.Insert(0, location);
            }

            if (query.ContainsKey("CurrentOutlet"))
            {
                CurrentOutlet = query["CurrentOutlet"] as Outlet;
                UpdateAllAreas();
            }
        }
    }
}
