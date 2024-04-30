namespace CloudCellarMobile
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.EditProduct), typeof(Views.EditProduct));
            Routing.RegisterRoute(nameof(Views.EditStock), typeof(Views.EditStock));

            Routing.RegisterRoute(nameof(Views.Outlets), typeof(Views.Outlets));
            Routing.RegisterRoute(nameof(Views.EditOutlet), typeof(Views.EditOutlet));

            Routing.RegisterRoute(nameof(Views.Audits), typeof(Views.Audits));
            Routing.RegisterRoute(nameof(Views.EditAudit), typeof(Views.EditAudit));

            Routing.RegisterRoute(nameof(Views.AreaLocations), typeof(Views.AreaLocations));
            Routing.RegisterRoute(nameof(Views.EditArea), typeof(Views.EditArea));
            Routing.RegisterRoute(nameof(Views.EditLocation), typeof(Views.EditLocation));

            Routing.RegisterRoute(nameof(Views.AuditTrail), typeof(Views.AuditTrail));
        }
    }
}
