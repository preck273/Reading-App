using BookReaderApp.ViewModels;
using BookReaderApp.Views;

namespace BookReaderApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new LoginView());
        }
    }
}
