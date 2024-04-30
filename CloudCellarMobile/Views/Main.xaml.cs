namespace CloudCellarMobile.Views;

public partial class Main : ContentPage
{
	public Main()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        productsCollection.SelectedItem = null;
    }
}