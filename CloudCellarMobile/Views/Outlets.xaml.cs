namespace CloudCellarMobile.Views;

public partial class Outlets : ContentPage
{
	public Outlets()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        outletsCollection.SelectedItem = null;
    }
}