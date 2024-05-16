using BookReaderApp.Models;
using BookReaderApp.ViewModels;
using Newtonsoft.Json;

namespace BookReaderApp.Views;

public partial class EditUserView : ContentPage
{
    private readonly SignupViewModel signupController = new SignupViewModel();
    private String newLevel;
    private LoginModel selectedUser = new LoginModel();
    public EditUserView(LoginModel user)
	{
        InitializeComponent();
        selectedUser = user;
        BindingContext = user;
    }
    private async void SubmitClicked(object sender, EventArgs e)
    {
        LoginModel loginModel = new LoginModel
        {
            Id = selectedUser.Id,
            Username = selectedUser.Username,
            Password = selectedUser.Password,
            Level = newLevel ?? selectedUser.Level,
            Image = selectedUser.Image,
            Email = selectedUser.Email,
        };

        string postData = JsonConvert.SerializeObject(loginModel);
        //ValidationField.Text = postData;


        string response = await signupController.Put(selectedUser.Id, postData);

        ValidationField.Text = response;
        if (response == "User updated successfully!")
        {
            ShowUsersView previousPage = new ShowUsersView();

            await Navigation.PushAsync(previousPage);
        }

    }

    private void LevelPicker_SelectedIndexChanged(object sender, System.EventArgs e)
    {

       newLevel = LevelPicker.SelectedItem as string;

    }
}