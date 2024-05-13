using BookReaderApp.Models;
using BookReaderApp.ViewModels;
using System.Diagnostics;

namespace BookReaderApp.Views;

public partial class ShowBookView : ContentPage
{
    private readonly BookModel book = new BookModel();
    private  List<BookImageModel> images = new List<BookImageModel>();
    private StackLayout bookLayout = new StackLayout();

    public ShowBookView(BookModel selectedBook)
    {
        InitializeComponent();
        book = selectedBook;

        images = BookImageViewModel.GetAllImages(book.Bid);

        DisplayBook();
    }

    private void DisplayBook()
    {
        bookLayout = new StackLayout
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
            Padding = 20,
            VerticalOptions = LayoutOptions.Start
        };

        var content = new Label
        {
            Text = book.Content,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Start
        };

        bookLayout.Children.Add(image);
        bookLayout.Children.Add(title);


        int totalCount = images.Count;
        CountdownEvent countdown = new CountdownEvent(totalCount);

        foreach (var imag in images)
        {
            ThreadPool.QueueUserWorkItem(LoadImage, new object[] { imag, countdown });
            Thread.Sleep(3000);
        }

        bookLayout.Children.Add(content);

        BooksLayout.Children.Add(bookLayout);
    }
    private void LoadImage(object state)
    {
        object[] array = state as object[];

        BookImageModel image = (BookImageModel)array[0];

        CountdownEvent countdown = (CountdownEvent)array[1];
        try
        {
            var pic = new Image
            {
                Source = ImageSource.FromStream(() => new System.IO.MemoryStream(image.Image)),
                HeightRequest = 5000,
                Aspect = Aspect.AspectFit
            };
            bookLayout.Children.Add(pic);
        }
        finally
        {
            countdown.Signal();
        }
        Debug.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} is exiting.");

    }
    private async void HomeClicked(object sender, EventArgs e)
    {
        HomePageView previousPage = new HomePageView();

        await Navigation.PushAsync(previousPage);
    }

}


