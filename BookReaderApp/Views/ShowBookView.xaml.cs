using BookReaderApp.Models;
using BookReaderApp.ViewModels;
using System.Diagnostics;
using System.Threading;

namespace BookReaderApp.Views;

public partial class ShowBookView : ContentPage
{
    private readonly BookModel book = new BookModel();
    private  List<BookImageModel> images = new List<BookImageModel>();
    private StackLayout bookLayout = new StackLayout();
    private static Mutex mut = new Mutex();
    private int  count;

    public ShowBookView(BookModel selectedBook)
    {
        InitializeComponent();
        book = selectedBook;

        images = BookImageViewModel.GetAllImages(book.Bid);

        int count = 0;
        
        //mutex
        Thread newThread = new Thread(new ThreadStart(DisplayBook));
        newThread.Start();
    }

    private void UseResource(BookImageModel imag)
    {
        // Wait until it is safe to enter.
        Debug.WriteLine("Image {0} is requesting the mutex",count);
        mut.WaitOne();

        Debug.WriteLine("Image {0} has entered the protected area",
                          count);

        var pic = new Image
        {
            Source = ImageSource.FromStream(() => new System.IO.MemoryStream(imag.Image)),
            HeightRequest = 5000,
            Aspect = Aspect.AspectFit
        };
        bookLayout.Children.Add(pic);

       
        Debug.WriteLine("Image {0} is leaving the protected area",
            count);

        // Release the Mutex.
        mut.ReleaseMutex();
        Debug.WriteLine("Image {0} has released the mutex",
            count);
        count += 1;
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

        
        foreach (var imag in images)
        {
            //Unsafe
            /*var pic = new Image
            {
                Source = ImageSource.FromStream(() => new System.IO.MemoryStream(imag.Image)),
                HeightRequest = 5000,
                Aspect = Aspect.AspectFit
            };
            bookLayout.Children.Add(pic);*/

            //safe
            UseResource(imag);
        }



        bookLayout.Children.Add(content);

        BooksLayout.Children.Add(bookLayout);
    }
  
    private async void HomeClicked(object sender, EventArgs e)
    {
        HomePageView previousPage = new HomePageView();

        await Navigation.PushAsync(previousPage);
    }

}


