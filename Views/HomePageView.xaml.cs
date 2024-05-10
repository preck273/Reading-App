using BookReaderApp.Models;

namespace BookReaderApp.Views;

public partial class HomePageView : ContentPage
{
	public HomePageView()
	{
		InitializeComponent();
        CheckUserRole();

    }
    private async void ShowUsersClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShowUsersView());
    }

    private void CheckUserRole()
    {
        if (User.Level == userLevel.EDITOR)
        {
            ShowUserBtn.IsVisible = true; 
            PitchBtn.IsVisible = true;
            AddPitchBtn.IsVisible = false;
        }
        else
        {
            ShowUserBtn.IsVisible = false;
            PitchBtn.IsVisible = false;
            AddPitchBtn.IsVisible = true;
        }
    }

    private async void PitchBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShowPitchesView());
    }

    private async void AddPitchBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AddPitchView());
    }
}