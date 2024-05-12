using BookReaderApp.Models;
using BookReaderApp.MongoDb;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System.Drawing;
using Microsoft.Maui;

namespace BookReaderApp.Views;

public partial class UploadBookView : ContentPage
{
	private byte[] pdfBytes; 
	private byte[] imageBytes;

	private SemaphoreSlim semaphore = new SemaphoreSlim(1);

	public UploadBookView()
	{
		InitializeComponent();
	}

	private async void UploadPdfButton_Clicked(object sender, EventArgs e)
	{
		var file = await FilePicker.PickAsync(new PickOptions
		{
			FileTypes = FilePickerFileType.Pdf,
			PickerTitle = "Select PDF"
		});

		if (file != null)
		{
			using (var stream = await file.OpenReadAsync())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					await stream.CopyToAsync(ms);
					pdfBytes = ms.ToArray();
				}
			}
			pdfLabel.Text = file.FileName;
		}
	}

	private async void UploadImageButton_Clicked(object sender, EventArgs e)
	{
		// Replace "ImagePicker" with your actual method for picking an image file
		var imageFile = await FilePicker.PickAsync(new PickOptions
		{
			FileTypes = FilePickerFileType.Images,
			PickerTitle = "Select Image"
		});

		if (imageFile != null)
		{
			using (var stream = await imageFile.OpenReadAsync())
			{
				using (MemoryStream ms = new MemoryStream())
				{
					await stream.CopyToAsync(ms);
					imageBytes = ms.ToArray();
				}
			}
			//bookImage.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
			pdfImageLabel.Text = imageFile.FileName;
		}
	}

	private async void UploadButtonClicked(object sender, EventArgs e)
	{
		try
		{
			await semaphore.WaitAsync(); // Wait for semaphore to become available

			if (pdfBytes != null && imageBytes != null)
			{
				var book = new BookModel
				{
					Title = Title.Text,
					Author = User.UserName,
					Category = Category.Text,
					PublishedDate = datePicker.Date,
					Description = Description.Text
				};

				await SaveToDatabaseAsync(pdfBytes, imageBytes, book);

				await DisplayAlert("Success", "Book uploaded successfully!", "OK");

				await Navigation.PopAsync();
			}
			else
			{
				await DisplayAlert("Error", "Please upload both a PDF file and an image.", "OK");
			}
		}
		finally
		{
			semaphore.Release(); // Release the semaphore to allow other threads to enter
		}
	}

	private async Task SaveToDatabaseAsync(byte[] pdfBytes, byte[] imageBytes, BookModel book)
	{
		var client = new MongoClient("mongodb://localhost:27017");
		var database = client.GetDatabase("ReadingBook");
		var bucket = new GridFSBucket(database);

		using (var pdfStream = new MemoryStream(pdfBytes))
		using (var imageStream = new MemoryStream(imageBytes))
		{
			var options = new GridFSUploadOptions
			{
				Metadata = new BsonDocument("BookDetails", book.ToBsonDocument())
			};

			var pdfFileId = await bucket.UploadFromStreamAsync(book.Title + ".pd", pdfStream, options);
			var imageFileId = await bucket.UploadFromStreamAsync(book.Title  + ".jpg", imageStream, options);

			book.PdfFile = pdfFileId.ToString();
			book.PdfImage = imageFileId.ToString();

			var booksCollection = database.GetCollection<BookModel>("Book");
			await booksCollection.InsertOneAsync(book);
		}
	}
}

	