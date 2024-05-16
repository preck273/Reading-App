using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;
using BookReaderApp.Models;
using BookReaderApp.ViewModels;



namespace BookReaderApp.Views
{

    public partial class LoginView : ContentPage
    {
        private readonly LoginViewModel loginController = new LoginViewModel();

        private string level;
        private string hashedpassword;


        int count = 0;
        public LoginView()
        {
            InitializeComponent();
            PitchesViewModel pitchController = new PitchesViewModel();

        }

        private async void SignupClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignupView());
        }

        private async void LoginClicked(object sender, EventArgs e)
        {
            if (UsernameEntry.Text == "" || PasswordEntry.Text == "")
            {
                ValidationField.Text = "Enter both a username password";
            }
            else
            {
                //Task Parallel Library
                await Login();
            }
        }

        private async Task Login()
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;


            if (await TryLogIn(username, password))
            {

                switch (level)
                {
                    case "editor":
                        User.Level = userLevel.EDITOR;
                        break;
                    case "writer":
                        User.Level = userLevel.WRITER;

                        break;
                    case "reader":
                        User.Level = userLevel.READER;
                        break;
                    default:
                        User.Level = userLevel.READER;
                        break;
                }

                User.UserName = username;

                User.UserId = await loginController.GetUserId(User.UserName, hashedpassword);
                User.Image = await loginController.GetUserImage(User.UserName, hashedpassword);
                User.Email = await loginController.GetUserEmail(User.UserName, hashedpassword);

                await Navigation.PushAsync(new HomePageView());
            }
            else
            {
                ValidationField.Text = "Invalid username or password.";
            }
        }

        private async Task<bool> TryLogIn(string username, string password)
        {
            try
            {
                //Asynchronous I/O - Api call
                string json = await loginController.GetAllJson();

                var users = JsonConvert.DeserializeObject<List<LoginModel>>(json);

                var user = users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    if (user.Password == HashString(password))
                    {
                        hashedpassword = HashString(password);
                        level = user.Level;
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ValidationField.Text = $"Failed to log in: {ex.Message}";
                return false;
            }
        }

        public static string HashString(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        
    }
}
