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
    [QueryProperty(nameof(Audit), "CreateAudit")]
    public class EditAuditViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public Audit _audit;
        public int Id
        {
            get => _audit.Id;
            set
            {
                if (_audit.Id != value)
                {
                    _audit.Id = value;
                    OnPropertyChanged();
                }
            }
        }
        public int OutletId
        {
            get => _audit.OutletId;
            set
            {
                if (_audit.OutletId != value)
                {
                    _audit.OutletId = value;
                    OnPropertyChanged();
                }
            }
        }
        public int AuditNo
        {
            get => _audit.AuditNo;
            set
            {
                if (_audit.AuditNo != value)
                {
                    _audit.AuditNo = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime StartDate
        {
            get => _audit.StartDate;
            set
            {
                if (_audit.StartDate != value)
                {
                    _audit.StartDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public DateTime EndDate
        {
            get => _audit.EndDate;
            set
            {
                if (_audit.EndDate != value)
                {
                    _audit.EndDate = value;
                    OnPropertyChanged();
                }
            }
        }

        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public EditAuditViewModel()
        {
            _audit = new Audit();

            SubmitCommand = new Command(Submit);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand SubmitCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void Submit()
        {
            int id = DataAccess.InsertAudit(_audit);
            _audit.Id = id;

            var navigationParameter = new ShellNavigationQueryParameters
        {
            { "CreatedAudit", _audit }
        };
            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("CreateAudit"))
            {
                _audit = query["CreateAudit"] as Audit;
                OnPropertyChanged(nameof(AuditNo));
                OnPropertyChanged(nameof(StartDate));
                OnPropertyChanged(nameof(EndDate));
            }
        }
    }
}
