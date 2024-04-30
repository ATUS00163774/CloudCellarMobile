namespace CloudCellarMobile.Views;

public partial class Audits : ContentPage
{
	public Audits()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        auditsCollection.SelectedItem = null;
    }
}