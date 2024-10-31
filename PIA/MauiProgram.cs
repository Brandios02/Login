using Firebase.Database;
using Firebase.Database.Query;
using PIA.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Firebase.Auth.Providers;
using Firebase.Auth;

namespace PIA;

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
                fonts.AddFont("Lato-Bold.ttf", "bold");
                fonts.AddFont("Lato-Regular.ttf", "medium");
                fonts.AddFont("FontAwesome", "recursos");
            });

#if DEBUG

        builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyC6ZJBkRRBAk5xm99vHToHlLLmSsxMxles\r\n",
            AuthDomain = "prueba-726d0.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        }));
#endif
        return builder.Build();
    }
}
