using BookReaderApp.Models;
using BookReaderApp.ViewModels;
using MongoDB.Bson;
using MongoDB.Driver.GridFS;
using MongoDB.Driver;
using System.Collections.ObjectModel;


namespace BookReaderApp.Views;

public partial class HomePageView : ContentPage
{
    private readonly BookCollection bookCollection;

	public ObservableCollection<BookModel> Books { get; set; }

	public HomePageView()
    {
        InitializeComponent();
        CheckUserRole();

		Books = new ObservableCollection<BookModel>();
		bookCollection = new BookCollection();
		BookListView.ItemsSource = Books;

	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		var bookList = await bookCollection.GetBooksAsync();

		Books.Clear();

		foreach (var book in bookList)
		{
			Books.Add(book);
		}
	}

	private async void FetchFilesAndDisplay(BookModel book)
	{
		var client = new MongoClient("mongodb://localhost:27017");
		var database = client.GetDatabase("ReadingBook");
		var bucket = new GridFSBucket(database);

		// Fetch the PDF file
		var pdfFileId = ObjectId.Parse(book.PdfFile);
		var pdfStream = await bucket.OpenDownloadStreamAsync(pdfFileId);
		byte[] pdfBytes;
		using (var ms = new MemoryStream())
		{
			await pdfStream.CopyToAsync(ms);
			pdfBytes = ms.ToArray();
		}

		// Fetch the image file
		var imageFileId = ObjectId.Parse(book.PdfImage);
		var imageStream = await bucket.OpenDownloadStreamAsync(imageFileId);
		byte[] imageBytes;
		using (var ms = new MemoryStream())
		{
			await imageStream.CopyToAsync(ms);
			imageBytes = ms.ToArray();
		}

		book.PdfImage = Convert.ToBase64String(imageBytes); 
		book.PdfImage = Convert.ToBase64String(imageBytes);

		book.PdfFile = Convert.ToBase64String(pdfBytes);

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
            UploadBtn.IsVisible = false;   
        }
        else
        {
            ShowUserBtn.IsVisible = false;
            PitchBtn.IsVisible = false;
            AddPitchBtn.IsVisible = true;
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

	private async void UploadBtnClicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new UploadBookView());
	}
}