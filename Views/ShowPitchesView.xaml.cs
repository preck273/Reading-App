using BookReaderApp.ViewModels;
using BookReaderApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;
namespace BookReaderApp.Views;

public partial class ShowPitchesView : ContentPage
{
    private readonly LoginViewModel loginController = new LoginViewModel();
    private readonly SignupViewModel signUpController = new SignupViewModel();
    private LoginModel selectedUser = new LoginModel();
    public ShowPitchesView()
	{
		InitializeComponent();
        FillTable();


    }

    private async void PitchesListView_ItemTapped(object sender, ItemTappedEventArgs e)
    {

        var selectedPitch = (PitchModel)e.Item;
        getUser(selectedPitch.Userid);

        bool grantPrivilege = await DisplayAlert("Grant Writer Privilege", $"Grant user {selectedPitch.Userid} Writer privilege?", "Yes", "No");

        if (!grantPrivilege)
        {
            signUpController.Delete(selectedPitch.Userid);

            ValidationField.Text = "Pitch has been deleted from the database";
        }
        else
        {
            LoginModel loginModel = new LoginModel
            {
                Id = selectedUser.Id,
                Username = selectedUser.Username,
                Password = selectedUser.Password,
                Level ="writer",
                Image = selectedUser.Image,
            };

            string postData = JsonConvert.SerializeObject(loginModel);

            string response = await signUpController.Put(selectedUser.Id, postData);

            ValidationField.Text = response;
            if (response == "User updated successfully!")
            {
                ShowUsersView previousPage = new ShowUsersView();

                await Navigation.PushAsync(previousPage);
            }
        }
    }


    private async void HomeClicked(object sender, EventArgs e)
    {
        HomePageView previousPage = new HomePageView();

        await Navigation.PushAsync(previousPage);
    }

    public async void FillTable()
    {
        List<PitchModel> pitches = PitchesViewModel.GetAllPitches();
           
        PitchesListView.ItemsSource = pitches;
    }
    
    public async void getUser(string id)
    {
        var json = await loginController.GetById(id, "json");
        var user = JsonConvert.DeserializeObject<LoginModel>(json);

        // Populate userList with users from JSON
        selectedUser = user;
    }
}