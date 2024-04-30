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
    public class EditProductViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public Product _product;
        public int Id
        {
            get => _product.Id;
            set
            {
                if (_product.Id != value)
                {
                    _product.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Brand
        {
            get => _product.Brand;
            set
            {
                if (_product.Brand != value)
                {
                    _product.Brand = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Name
        {
            get => _product.Name;
            set
            {
                if (_product.Name != value)
                {
                    _product.Name = value;
                    OnPropertyChanged();
                }
            }
        }
        public int Size
        {
            get => _product.Size;
            set
            {
                if (_product.Size != value)
                {
                    _product.Size = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Category
        {
            get => _product.Category;
            set
            {
                if (_product.Category != value)
                {
                    _product.Category = value;
                    OnPropertyChanged();
                }
            }
        }
        public string SubCategory
        {
            get => _product.SubCategory;
            set
            {
                if (_product.SubCategory != value)
                {
                    _product.SubCategory = value;
                    OnPropertyChanged();
                }
            }
        }
        public string Barcode
        {
            get => _product.Barcode;
            set
            {
                if (_product.Barcode != value)
                {
                    _product.Barcode = value;
                    OnPropertyChanged();
                }
            }
        }
        public int FullWeight
        {
            get => _product.FullWeight;
            set
            {
                if (_product.FullWeight != value)
                {
                    _product.FullWeight = value;
                    OnPropertyChanged();
                }
            }
        }
        public int EmptyWeight
        {
            get => _product.EmptyWeight;
            set
            {
                if (_product.EmptyWeight != value)
                {
                    _product.EmptyWeight = value;
                    OnPropertyChanged();
                }
            }
        }

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EditProductViewModel()
        {
            _product = new Product();

            SubmitCommand = new Command(Submit);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand SubmitCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Submit()
        {
            int id = DataAccess.InsertProduct(_product);
            _product.Id = id;

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CreatedProduct", _product }
            };
            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("create"))
            {
                _product = new Product();
            }
        }
    }
}
