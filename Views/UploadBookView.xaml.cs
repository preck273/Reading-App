using BookReaderApp.Models;
using BookReaderApp.MongoDb;
using MongoDB.Driver;

namespace BookReaderApp.Views;

public partial class UploadBookView : ContentPage
{
	private byte[] imageData;
	private byte[] pdfData;
	private IMongoDatabase database;

	public UploadBookView()
	{
		InitializeComponent();
	}

	private async void UploadImageButton_Clicked(object sender, EventArgs e)
	{
		var result = await FilePicker.PickAsync(new PickOptions
		{
			FileTypes = FilePickerFileType.Images,
			PickerTitle = "Select Image"
		});

		if (result != null)
		{
			using (var stream = await result.OpenReadAsync())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					await stream.CopyToAsync(ms);
					imageData = ms.ToArray();
				}
			}
		}
	}

	private async void UploadPdfButton_Clicked(object sender, EventArgs e)
	{
		var result = await FilePicker.PickAsync(new PickOptions
		{
			FileTypes = FilePickerFileType.Pdf,
			PickerTitle = "Select PDF"
		});

		if (result != null)
		{
			using (var stream = await result.OpenReadAsync())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					await stream.CopyToAsync(ms);
					pdfData = ms.ToArray();
				}
			}
		}
	}

	private async void UploadButtonClicked(object sender, EventArgs e)
	{
		// Create a new Book object with the uploaded data
		var book = new BookModel
		{
			Title = Title.Text,
			Author = Author.Text,
			BookImage = imageData,
			PdfFile = pdfData,
			Category = Category.Text,
			PublishedDate = datePicker.Date,
			Description = Description.Text
		};

		BindingContext = book;
		// Save the book to the database
		
		SaveToDatabaseAsync(book);
		await DisplayAlert("Success", "Book uploaded successfully!", "OK");

		await Navigation.PopAsync();
	}

	private async Task SaveToDatabaseAsync(BookModel book)
	{
		
		try
		{

			var client = new MongoClient("mongodb://localhost:27017/");
			var database = client.GetDatabase("ReadingApp");
			var collection = database.GetCollection<BookModel>("Book");

			collection.InsertOne(book);
		}
		catch (Exception ex)
		{
			// Handle the exception
			Console.WriteLine($"An error occurred: {ex.Message}");

			await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
		}
	}
	
}