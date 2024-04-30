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
    public class AuditsViewModel : ObservableObject, IQueryAttributable
    {
        //~~~~ Properties ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ObservableCollection<Audit> AllAudits { get; set; }
        public Outlet CurrentOutlet { get; set; }
        public Audit SelectedAudit { get; set; }
        public bool IsAuditSelected { get; set; }


        //~~~~ Constructors ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public AuditsViewModel()
        {
            AllAudits = new ObservableCollection<Audit>();

            AuditSelectedCommand = new Command<Audit>(AuditSelected);
            SelectCommand = new Command(Select);
            NewAuditCommand = new Command(NewAudit);
        }

        //~~~~ Commands ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public ICommand AuditSelectedCommand { get; private set; }
        public ICommand SelectCommand { get; private set; }
        public ICommand NewAuditCommand { get; private set; }

        //~~~~ Methods ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        private void AuditSelected(Audit audit)
        {
            if (audit != null)
            {
                SelectedAudit = audit;
                IsAuditSelected = true;
                OnPropertyChanged(nameof(IsAuditSelected));
            }
            else
            {
                IsAuditSelected = false;
                OnPropertyChanged(nameof(IsAuditSelected));
            }
        }

        private void Select()
        {
            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "SelectedAudit", SelectedAudit }
            };

            Shell.Current.GoToAsync($"..", navigationParameter);
        }

        private void NewAudit()
        {
            int allAuditsCount = AllAudits.Count;
            Audit CreateAudit = new Audit
            { 
                OutletId = CurrentOutlet.Id,
                AuditNo = allAuditsCount += 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now,
            };

            var navigationParameter = new ShellNavigationQueryParameters
            {
                { "CreateAudit", CreateAudit }
            };
            Shell.Current.GoToAsync($"{nameof(Views.EditAudit)}", navigationParameter);
        }

        private void UpdateAllAudits()
        {
            AllAudits = DataAccess.GetAudits(CurrentOutlet.Id);
            OnPropertyChanged(nameof(AllAudits));
        }

        //~~~~ Query Attributes ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("CreatedAudit"))
            {
                Audit audit = query["CreatedAudit"] as Audit;
                Audit matchedAudit = AllAudits.Where((o) => o.Id == audit.Id).FirstOrDefault();

                if (matchedAudit != null)
                {
                    // Product already exists
                }
                else
                    AllAudits.Insert(0, audit);
            }

            if (query.ContainsKey("CurrentOutlet"))
            {
                CurrentOutlet = query["CurrentOutlet"] as Outlet;
                UpdateAllAudits();
            }
        }
    }
}
