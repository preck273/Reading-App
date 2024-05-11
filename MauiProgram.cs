using BookReaderApp.MongoDb;
using Microsoft.Extensions.Logging;

namespace BookReaderApp
{
    public static class MauiProgram
    {
		

		public static MauiApp CreateMauiApp()
        {
			CreateDb createDb = new CreateDb("ReadingApp");

			var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();

            
        }
    }
}
