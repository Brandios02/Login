using Firebase.Auth;
using PIA.Views;
namespace PIA;

public partial class App : Application
{
    public App(FirebaseAuthClient firebaseAuthClient)
    {
        InitializeComponent();

        MainPage = new NavigationPage(new InterfazUs());
    }
}