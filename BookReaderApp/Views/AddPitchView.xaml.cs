using BookReaderApp.Models;
using BookReaderApp.ViewModels;

namespace BookReaderApp.Views;

public partial class AddPitchView : ContentPage
{
	public AddPitchView()
	{
		InitializeComponent();
	}
    private async void HomeClicked(object sender, EventArgs e)
    {
        HomePageView previousPage = new HomePageView();

        await Navigation.PushAsync(previousPage);
    }


    private void OnSubmitClicked(object sender, EventArgs e)
    {
        string pitchContent = PitchEntry.Text;


        if (string.IsNullOrWhiteSpace(pitchContent))
        {
            ValidationField.Text = "Please enter pitch content";
            return;
        }

        bool success = PitchesViewModel.AddPitch(User.UserId, pitchContent, false, false,false);

        if (success)
        {
            ValidationField.Text = "Pitch submitted successfully";
        }
        else
        {
            ValidationField.Text = "Failed to submit pitch";
        }

        
        
    }
}