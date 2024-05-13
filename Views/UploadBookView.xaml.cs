using BookReaderApp.Models;
using BookReaderApp.NewFolder;


namespace BookReaderApp.Views;

public partial class UploadBookView : ContentPage
{

	private BookModel _book = new BookModel();
	private BookImageModel _bookImage = new BookImageModel();
	private byte[] _coverImage;
	private byte[] _bookImg;

	public UploadBookView()
	{
		InitializeComponent();
		BindingContext = _book;
	}

	private async void UploadCoverImageButtonClicked(object sender, EventArgs e)
	{
		var fileResult = await MediaPicker.PickPhotoAsync();
		if (fileResult != null)
		{
			using (var stream = await fileResult.OpenReadAsync())
			{
				_coverImage = new byte[stream.Length];
				await stream.ReadAsync(_coverImage, 0, _coverImage.Length);
			}

			_book.CoverImage = _coverImage;
			coverImageLabel.Text = "Image selected";
		}
	}

	private async void UploadBookImageButtonClicked(object sender, EventArgs e)
	{
		var fileResult = await MediaPicker.PickPhotoAsync();
		if (fileResult != null)
		{
			using (var stream = await fileResult.OpenReadAsync())
			{
				_bookImg = new byte[stream.Length];
				await stream.ReadAsync(_bookImg, 0, _bookImg.Length);
			}

			_bookImage.BookImage = _bookImg;
			bookImageLabel.Text = "Image selected";
		}
	}

	private async void UploadButtonClicked(object sender, EventArgs e)
	{
		if (_book.Title != null && _coverImage != null && _bookImg != null && _book.Description != null)
		{
			var bookUploadHelper = new BookUploadHelper();
			_book.Author = User.UserName;
			bookUploadHelper.UploadBook(_book, _bookImage);

			await DisplayAlert("Book uploaded successfully", "", "OK");

			await Navigation.PopAsync();
		}
		else
		{
			await DisplayAlert("Please fill in all fields", "", "OK");
		}
	}

}
