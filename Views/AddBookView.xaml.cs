using BookReaderApp.Models;
using BookReaderApp.ViewModels;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading;

namespace BookReaderApp.Views;

public partial class AddBookView : ContentPage
{
    private byte[] imageBytes;

    private List<byte[]> imageBytesList = new List<byte[]>();

    private readonly PitchModel pitch = new PitchModel();

    public AddBookView(PitchModel selectedPitch)
    {
        InitializeComponent();
        pitch = selectedPitch;

    }

    private async void UploadImageButton_Clicked(object sender, EventArgs e)
    {

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
            pdfImageLabel.Text = imageFile.FileName;
        }
    }

   

    private async void SubmitBookImagesButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var images = await FilePicker.PickMultipleAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Select Images"
            });

            if (images != null && images.Any())
            {
                imageBytesList.Clear();

                foreach (var image in images)
                {
                    using (var stream = await image.OpenReadAsync())
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            await stream.CopyToAsync(ms);
                            imageBytesList.Add(ms.ToArray());
                        }
                    }
                }
                pdfImageLabel.Text = $"Images selected";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to select images: {ex.Message}", "OK");
        }
    }


    private async void UploadButtonClicked(object sender, EventArgs e)
    {
        if (imageBytes != null && imageBytesList != null)
        {
            var book = new BookModel
            {
                Bid = Int32.Parse(pitch.Pid),
                Image = imageBytes,
                Title = Title.Text,
                Content = Content.Text
            };
            BookViewModel.AddBook(book);


            foreach (var image in imageBytesList)
            {
                var images = new BookImageModel
                {
                    Bid = Int32.Parse(pitch.Pid),
                    Image = image,
                };

                BookImageViewModel.AddImage(images);
            }

            
            PitchesViewModel.published(pitch.Pid);
            await DisplayAlert("Success", "Book uploaded successfully!", "OK");

            ShowPitchesView previousPage = new ShowPitchesView();

            await Navigation.PushAsync(previousPage);
        }

        else
        {
            await DisplayAlert("Error", "Please upload ball files.", "OK");
        }

    }
}