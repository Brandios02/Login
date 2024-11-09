using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.DependencyInjection;

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
        // Configuración de FirebaseAuthClient para la autenticación con correo electrónico y contraseña
        builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = "AIzaSyC6ZJBkRRBAk5xm99vHToHlLLmSsxMxles",  // Tu API Key de Firebase
            AuthDomain = "prueba-726d0.firebaseapp.com",  // Tu dominio de autenticación de Firebase
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()  // Proveedor de autenticación por correo electrónico
            }
        }));
#endif

        return builder.Build();
    }
}
