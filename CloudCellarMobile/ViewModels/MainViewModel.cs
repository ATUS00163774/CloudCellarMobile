using CloudCellarMobile.Data;
using CloudCellarMobile.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;

namespace CloudCellarMobile.ViewModels
{
    // Objects the class can receive through queries
    [QueryProperty(nameof(Product), "CreatedProduct"), QueryProperty(nameof(Stock), "CreatedStock"), QueryProperty(nameof(Outlet), "SelectedOutlet"), QueryProperty(nameof(Audit), "SelectedAudit"), QueryProperty(nameof(AreaLocation), "SelectedAreaLocation")]
    public class MainViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ObservableCollection<Product> AllProducts { get; set; }   
        public ObservableCollection<Stock> AllStocks { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }

        public Outlet CurrentOutlet { get; set; }

        public Audit CurrentAudit { get; set; }
        public string CurrentAuditPrettyString
        {
            get => CurrentAudit.ToPrettyString();
        }
        public string CurrentAuditShortString
        {
            get => CurrentAudit.ToShortString();
        }

        public Area CurrentArea { get; set; }
        public Models.Location CurrentLocation { get; set; }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged();
                    Filter();
                }
            }
        }
        private string _searchText;
        public ObservableCollection<string> Categories { get; set; }
        public string SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    OnPropertyChanged();
                    Filter();
                }
            }
        }
        private string _selectedCategory;
        public ObservableCollection<string> SubCategories { get; set; }     
        public string SelectedSubCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedSubCategory != value)
                {
                    _selectedSubCategory = value;
                    OnPropertyChanged();
                    Filter();
                }
            }
        }
        private string _selectedSubCategory;

        public Stock RecentStock { get; set; }

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public MainViewModel()
        {
            AllProducts = DataAccess.GetProducts();
            FilteredProducts = AllProducts;

            AllStocks = new ObservableCollection<Stock>();

            Categories = Product.Categories();

            CurrentAudit = new Audit();
            RecentStock = new Stock();

            NewProductCommand = new Command(NewProduct);
            NewStockCommand = new Command<Product>(NewStock);
            ViewStocksCommand = new Command(ViewStocks);
            ScanBarcodeCommand = new Command(ScanBarcode);
            SelectOutletCommand = new Command(SelectOutlet);
            SelectAuditCommand = new Command(SelectAudit);
            SelectAreaLocationCommand = new Command(SelectAreaLocation);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand NewProductCommand { get; private set; }
        public ICommand NewStockCommand { get; private set; }
        public ICommand ViewStocksCommand { get; private set; }
        public ICommand ScanBarcodeCommand { get; private set; }
        public ICommand SelectOutletCommand { get; private set; }
        public ICommand SelectAuditCommand { get; private set; }
        public ICommand SelectAreaLocationCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void NewProduct()
        {
            string create = "create";
            Shell.Current.GoToAsync($"{nameof(Views.EditProduct)}?create={create}");
            Shell.Current.FlyoutIsPresented = false;
        }

        private void NewStock(Product product)
        {
            if (product != null)
            {

                Stock newStock = new Stock()
                {
                    ProductId = product.Id,
                    Product = product,
                    OutletId = CurrentOutlet.Id,
                    AuditId = CurrentAudit.Id,
                    AreaId = CurrentArea.Id,
                    LocationId = CurrentLocation.Id,
                    Time = DateTime.Now
                };

                var navigationParameter = new ShellNavigationQueryParameters
                {
                    { "Stock", newStock }
                };
                Shell.Current.GoToAsync($"{nameof(Views.EditStock)}", navigationParameter);
            }
        }

        private void ViewStocks()
        {
            var navigationParameter = new ShellNavigationQueryParameters
                {
                    { "Stocks", AllStocks }
                };

            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.GoToAsync($"{nameof(Views.AuditTrail)}", navigationParameter);
        }

        private void ScanBarcode()
        {
            // Shell.Current.GoToAsync($"{nameof(Views.Barcode)}");
        }

        private void SelectOutlet()
        {
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.GoToAsync($"{nameof(Views.Outlets)}");
        }

        private void SelectAudit()
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CurrentOutlet", CurrentOutlet }
            };
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.GoToAsync($"{nameof(Views.Audits)}", navigationParameter);
        }

        private void SelectAreaLocation()
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CurrentOutlet", CurrentOutlet }
            };
            Shell.Current.FlyoutIsPresented = false;
            Shell.Current.GoToAsync($"{nameof(Views.AreaLocations)}", navigationParameter);
        }

        private void Filter()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProducts = AllProducts;
            }
            else
            {
                FilteredProducts = new ObservableCollection<Product>(
                    AllProducts.Where(p => p.ToString().ToLower().Contains(SearchText.ToLower())));
            }

            OnPropertyChanged(nameof(FilteredProducts));
        }

        private async Task UpdateAllStocks()
        {
            if (CurrentOutlet == null)
                await Shell.Current.GoToAsync($"{nameof(Views.Outlets)}");

            if (CurrentAudit == null)
            {
                var navigationParameter = new ShellNavigationQueryParameters
                {
                    { "CurrentOutlet", CurrentOutlet }
                };

                await Shell.Current.GoToAsync($"{nameof(Views.Audits)}", navigationParameter);
            }

            AllStocks = DataAccess.GetStocks(CurrentOutlet.Id, CurrentAudit.Id);
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("CreatedProduct"))
            {
                Product product = query["CreatedProduct"] as Product;
                Product matchedProduct = AllProducts.Where((p) => p.Id == product.Id).FirstOrDefault();

                if (matchedProduct != null)
                {
                    // Product already exists
                }
                else
                    AllProducts.Insert(0, product);
            }

            if (query.ContainsKey("CreatedStock"))
            {
                Stock stock = query["CreatedStock"] as Stock;
                Stock matchedStock = AllStocks.Where((s) => s.Id == stock.Id).FirstOrDefault();

                if (matchedStock != null)
                {
                    // Stock already exists
                }
                else
                {
                    AllStocks.Insert(0, stock);
                    RecentStock = stock;
                    OnPropertyChanged(nameof(RecentStock));
                }
            }

            if (query.ContainsKey("SelectedOutlet"))
            {
                CurrentOutlet = query["SelectedOutlet"] as Outlet;
                OnPropertyChanged(nameof(CurrentOutlet));

                //SelectAudit();
            }

            if (query.ContainsKey("SelectedAudit"))
            {
                CurrentAudit = query["SelectedAudit"] as Audit;
                OnPropertyChanged(nameof(CurrentAudit));
                OnPropertyChanged(nameof(CurrentAuditPrettyString));
                OnPropertyChanged(nameof(CurrentAuditShortString));

                //SelectAreaLocation();
            }

            if (query.ContainsKey("SelectedAreaLocation"))
            {
                AreaLocation selectedAreaLocation = query["SelectedAreaLocation"] as AreaLocation;

                CurrentArea = selectedAreaLocation.Area;
                OnPropertyChanged(nameof(CurrentArea));

                CurrentLocation = selectedAreaLocation.Location;
                OnPropertyChanged(nameof(CurrentLocation));
            }

            if (query.ContainsKey("DetectedBarcode"))
            {
                string barcode = query["DetectedBarcode"].ToString();

                Product matchedProduct = AllProducts.Where((p) => p.Barcode == barcode).FirstOrDefault();

                if (matchedProduct != null)
                {
                    Console.WriteLine("Returned from barcode");
                    NewStock(matchedProduct);
                }
                else
                {
                    // New Product
                }
            }
        }
    }
}
