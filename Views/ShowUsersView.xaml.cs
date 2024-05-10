using BookReaderApp.Models;
using BookReaderApp.ViewModels;
using Newtonsoft.Json;
using System.Data;

namespace BookReaderApp.Views;

public partial class ShowUsersView : ContentPage
{
    private readonly LoginViewModel loginController = new LoginViewModel();

    public ShowUsersView()
	{
		InitializeComponent();
        FillTable();
    }

    private async void UsersListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {
        if (e.Item is LoginModel selectedUser)
        {
            await Navigation.PushAsync(new EditUserView(selectedUser));
        }
        ((ListView)sender).SelectedItem = null;
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

        UsersListView.ItemsSource = userViewModels;
    }
}