using BookReaderApp.ViewModels;
using BookReaderApp.Models;
using Newtonsoft.Json;
namespace BookReaderApp.Views;

public partial class ShowPitchesView : ContentPage
{
    private readonly LoginViewModel loginController = new LoginViewModel();
    public ShowPitchesView()
	{
		InitializeComponent();
        FillTable();

    }

    private async void PitchesListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
       
    }

    private async void HomeClicked(object sender, EventArgs e)
    {
        HomePageView previousPage = new HomePageView();

        await Navigation.PushAsync(previousPage);
    }

    public async void FillTable()
    {
        var json = await loginController.GetAllJson();
        var users = JsonConvert.DeserializeObject<List<LoginModel>>(json);

        var userViewModels = new List<LoginModel>();
        foreach (var user in users)
        {
            var userViewModel = new LoginModel
            {
                Id = user.Id,
                Username = user.Username,
                Password = user.Password,
                Level = user.Level,
                Image = user.Image
            };
            userViewModels.Add(userViewModel);
        }

        PitchesListView.ItemsSource = userViewModels;
    }
}