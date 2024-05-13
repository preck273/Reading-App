using BookReaderApp.HelperClass;
using BookReaderApp.ViewModels;
using BookReaderApp.Views;

namespace BookReaderApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			// Register the converter
			Resources.Add("ByteToImageHelper", new ByteToImageHelper());

			//MainPage = new AppShell();
			MainPage = new NavigationPage(new LoginView());
        }
    }
}
