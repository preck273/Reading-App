using BookReaderApp.Models;
using BookReaderApp.ViewModels;

namespace BookReaderApp.Views;

public partial class HomePageView : ContentPage
{
	public HomePageView()
	{
		InitializeComponent();
        CheckUserRole();
        DisplayBooks();
    }
  
    private void DisplayBooks()
    {
        var allBooks = BookViewModel.GetAllBooks();
        foreach (var book in allBooks)
        {
            var bookLayout = new StackLayout
            {
                Margin = new Thickness(5),
                VerticalOptions = LayoutOptions.Start
            };

            var image = new Image
            {
                Source = ImageSource.FromStream(() => new System.IO.MemoryStream(book.Image)),
                HeightRequest = 200,
                Aspect = Aspect.AspectFit
            };
            
            var title = new Label
            {
                Text = book.Title,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Start
            };

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += async (s, e) =>
            {
                await Navigation.PushAsync(new ShowBookView(book));
            };
            image.GestureRecognizers.Add(tapGestureRecognizer);


            bookLayout.Children.Add(image);
            bookLayout.Children.Add(title);

            BooksLayout.Children.Add(bookLayout);
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
            ViewPitchBtn.IsVisible = false;
        }
        else
        {
            ShowUserBtn.IsVisible = false;
            PitchBtn.IsVisible = false;
            AddPitchBtn.IsVisible = true;
            ViewPitchBtn.IsVisible = true;
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
    private async void SignoutClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new LoginView());
    }
    private async void ViewPitchBtnClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ShowPitchesView());
    }
}