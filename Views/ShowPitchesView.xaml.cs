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

        if(selectedPitch.IsPublished == false)
        {
            bool grantPrivilege = await DisplayAlert("Grant Writer Privilege", $"Grant user {selectedPitch.Userid} Writer privilege?", "Yes", "No");
            if (!grantPrivilege)
            {
                //Possibly send email to user that their pitch has been denied
                PitchesViewModel.DeletePitch(selectedPitch.Pid);

                ValidationField.Text = "Pitch has been denied. It has been deleted from the database";
                ShowPitchesView previousPage = new ShowPitchesView();

                await Navigation.PushAsync(previousPage);
            }
            else
            {
                LoginModel loginModel = new LoginModel
                {
                    Id = selectedUser.Id,
                    Username = selectedUser.Username,
                    Password = selectedUser.Password,
                    Level = "writer",
                    Image = selectedUser.Image,
                };

                string postData = JsonConvert.SerializeObject(loginModel);

                string response = await signUpController.Put(selectedUser.Id, postData);

                ValidationField.Text = response;
                if (response == "User updated successfully!")
                {
                    PitchesViewModel.canPublish(selectedPitch.Pid);
                    ShowUsersView nextPage = new ShowUsersView();

                    await Navigation.PushAsync(nextPage);
                }
            }
        }
        else
        {
            ValidationField.Text = "Pitch has already been accepted";
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