namespace BookReaderApp.Views;

public partial class SignupView : ContentPage
{
	public SignupView()
	{
		InitializeComponent();
	}

    private void SubmitClicked(object sender, EventArgs e)
    {
        new NavigationPage(new SignupView());
    } 
    
    private void CancelClicked(object sender, EventArgs e)
    {
        new NavigationPage(new SignupView());
    }
}