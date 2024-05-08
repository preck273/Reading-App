namespace BookReaderApp.Views;

public partial class HomePageView : ContentPage
{
	public HomePageView()
	{
		InitializeComponent();
	}
    private async void SignupClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SignupView());
    }
}