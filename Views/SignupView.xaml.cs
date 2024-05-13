using BookReaderApp.Models;
using BookReaderApp.ViewModels;
using Newtonsoft.Json;

namespace BookReaderApp.Views;

public partial class SignupView : ContentPage
{
    private readonly SignupViewModel signupController = new SignupViewModel();
    private readonly LoginViewModel loginController = new LoginViewModel();

    public SignupView()
	{
		InitializeComponent();
	}

    private async void SubmitClicked(object sender, EventArgs e)
    {
        if (UsernameEntry.Text == "" || PasswordEntry.Text == "" || EmailEntry.Text == "")
        {

        }
        else
        {
            await Signup();
        }
       
    } 
    
    private async void CancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    //Move to signUp page
    public async Task Signup()
    {

        bool exists = false;
        string username = UsernameEntry.Text;
        string password = PasswordEntry.Text;
        string level = "reader";
        string image = ImageEntry.Text;
        string email = EmailEntry.Text;

        var json = await loginController.GetAllJson();
        var users = JsonConvert.DeserializeObject<List<LoginModel>>(json);

        foreach (var user in users)
        {
            if (username == user.Username)
            {
                exists = true;
                ValidationField.Text = "Username already exists";
            }
        }

        if (exists == false)
        {
            try
            {
                await signupController.Post(username, LoginView.HashString(password), level, image, email);
                ValidationField.Text = "Successfully added!";

                LoginView previousPage = new LoginView();

                await Navigation.PushAsync(previousPage);
            }
            catch (Exception ex)
            {
                ValidationField.Text = $"Failed to add user: {ex.Message}";
            }

        }
    }
}