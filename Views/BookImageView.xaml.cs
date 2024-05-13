using BookReaderApp.Models;

namespace BookReaderApp.Views;

public partial class BookImageView : ContentPage
{
	public BookImageView(BookModel selectedBook)
	{
		InitializeComponent();
	    SetBookImage(selectedBook);
    }

    private void SetBookImage(BookModel selectedBook)
    {
        // Check if selectedBook has a valid CoverImage
        if (selectedBook != null && selectedBook.CoverImage != null && selectedBook.CoverImage.Length > 0)
        {
            ImageSource imageSource = ImageSource.FromStream(() => new MemoryStream(selectedBook.CoverImage));

            
            bookImage.Source = imageSource;
        }
        else
        {
            bookImage.Source = null;
        }
    }
}