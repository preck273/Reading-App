using BookReaderApp.ViewModels;
using BookReaderApp.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace BookReaderApp.Views
{
    public partial class ShowPitchesView : ContentPage
    {
        private readonly LoginViewModel loginController = new LoginViewModel();
        private readonly SignupViewModel signUpController = new SignupViewModel();
        private readonly PitchesViewModel pitchController = new PitchesViewModel();
        private LoginModel selectedUser = new LoginModel();
        private SemaphoreSlim semaphore = new SemaphoreSlim(2, 2); 

        public ShowPitchesView()
        {
            InitializeComponent();
            FillTable();
        }

        private async void PitchesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var selectedPitch = (PitchModel)e.Item;
            getUser(selectedPitch.Userid);
            if (User.Level == userLevel.EDITOR)
            {
                if (selectedPitch.IsPublished == false)
                {
                    bool grantPrivilege = await DisplayAlert("Grant Writer Privilege", $"Grant user {selectedPitch.Userid} Writer privilege?", "Yes", "No");
                    if (!grantPrivilege)
                    {
                        SendEmail(grantPrivilege);
                        Updatedb(grantPrivilege, selectedPitch);

                        ValidationField.Text = "Pitch has been denied!";
                        ShowPitchesView previousPage = new ShowPitchesView();
                        await Navigation.PushAsync(previousPage);
                    }
                    else
                    {
                        await UpdateUserAndSendEmail(grantPrivilege,selectedPitch);

                        ShowPitchesView previousPage = new ShowPitchesView();
                        await Navigation.PushAsync(previousPage);
                    }
                }
                else
                {
                    ValidationField.Text = "Pitch has already been accepted";
                }
            }
            else if (User.Level == userLevel.WRITER)
            {
                if (selectedPitch.Published == false)
                {
                    AddBookView previousPage = new AddBookView(selectedPitch);
                    await Navigation.PushAsync(previousPage);
                }
                else
                {
                    await revertWriter(selectedPitch);
                }
                
            }
        }

        private void SendEmail(bool accepted)
        {
            if (!accepted)
            {
                pitchController.SendEmail(User.Email, selectedUser.Email, "Pitch Denied", "Your pitch has been denied.");
            }
            else
            {
                pitchController.SendEmail(User.Email, selectedUser.Email, "Pitch Accepted", "Your pitch has been accepted.");
            }
        }

        private void Updatedb(bool accepted, PitchModel selectedPitch)
        {
            if (!accepted)
            {
                PitchesViewModel.isReviewed(selectedPitch.Pid);
            }
            else
            {
                PitchesViewModel.isReviewed(selectedPitch.Pid);
                PitchesViewModel.canPublish(selectedPitch.Pid);
            }
        }


        public async Task revertWriter(PitchModel selectedPitch)
        {
            LoginModel loginModel = new LoginModel
            {
                Id = selectedUser.Id,
                Username = selectedUser.Username,
                Password = selectedUser.Password,
                Level = "reader",
                Image = selectedUser.Image,
                Email = selectedUser.Email,
            };

            string postData = JsonConvert.SerializeObject(loginModel);
            string response = await signUpController.Put(selectedUser.Id, postData);

            ValidationField.Text = response;
        }
        private async Task UpdateUserAndSendEmail(bool accepted, PitchModel selectedPitch)
        {
            LoginModel loginModel = new LoginModel
            {
                Id = selectedUser.Id,
                Username = selectedUser.Username,
                Password = selectedUser.Password,
                Level = "writer",
                Image = selectedUser.Image,
                Email = selectedUser.Email,
            };

            string postData = JsonConvert.SerializeObject(loginModel);
            string response = await signUpController.Put(selectedUser.Id, postData);

            ValidationField.Text = response;
            if (response == "User updated successfully!")
            {
                Debug.WriteLine("Sending {0} to {1}", "Pitch Accepted", selectedUser.Email);
                await semaphore.WaitAsync();
                try
                {
                    Debug.WriteLine("{0} to {1} in semaphore", "Pitch Accepted", selectedUser.Email);
                    SendEmail(accepted);
                    Updatedb(accepted, selectedPitch);
                   
                }
                finally
                {
                    Debug.WriteLine("Releasing {0} to {1} from semaphore", "Pitch Accepted", selectedUser.Email);

                    semaphore.Release();
                }

/*
                ShowPitchesView previousPage = new ShowPitchesView();
                await Navigation.PushAsync(previousPage);*/
            }
        }

        private async void HomeClicked(object sender, EventArgs e)
        {
            HomePageView previousPage = new HomePageView();
            await Navigation.PushAsync(previousPage);
        }



        public async void FillTable()
        {
            if (User.Level == userLevel.EDITOR)
            {
                List<PitchModel> pitches = PitchesViewModel.GetAllPitches();
                PitchesListView.ItemsSource = pitches;
            }
            else
            {
                List<PitchModel> pitches = PitchesViewModel.GetUserPitches(User.UserId);
                PitchesListView.ItemsSource = pitches;
            }
        }

        public async void getUser(string id)
        {
            var json = await loginController.GetById(id, "json");
            var user = JsonConvert.DeserializeObject<LoginModel>(json);

            selectedUser = user;
        }
    }
}
