using MauiAppPrevisaoDoTempo; 
using MauiAppPrevisaoDoTempo.Helpers;
using Microsoft.Maui.Storage;
using System.IO; 

namespace MauiAppPrevisaoDoTempo
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<SQLiteDatabaseHelpers>(s =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "PrevisaoTempo.db3");
                return new SQLiteDatabaseHelpers(dbPath);
            });

            return builder.Build();
        }
    }
}