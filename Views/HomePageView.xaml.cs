using BookReaderApp.Models;
using BookReaderApp.ViewModels;

namespace BookReaderApp.Views;

public partial class HomePageView : ContentPage
{
	public HomePageView()
	{
		InitializeComponent();
        CheckUserRole();

		var bookCollection = new BookCollection();

		// Fetch book details from the database
		var bookDetails = bookCollection.FetchBookDetailsFromDatabase();

		if (bookDetails != null)
		{
			BindingContext = new UploadBookViewModel
			{
				BookName = bookDetails.Title,
				//BookImage = ImageSource
			};
		}
		else
		{

			DisplayAlert("Error", "Failed to fetch book details from the database", "OK");
		}

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
            UploadBtn.IsVisible = false;   
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

	private async void UploadBtnClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new UploadBookView());
	}
}