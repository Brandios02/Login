using Firebase.Auth;
using PIA.Models;

namespace PIA.Views;

public partial class NewPage1 : ContentPage
{
    private readonly FirebaseAuthClient _clientAuth;
    public NewPage1(FirebaseAuthClient firebaseAuthClient)
    {
        InitializeComponent();
        _clientAuth = firebaseAuthClient;
    }
    private async void btn_login(object sender, EventArgs e)
    {
        try
        {
            await _clientAuth.SignInWithEmailAndPasswordAsync(emailEntry.Text, PasswordEntry.Text);
            await Application.Current.MainPage.DisplayAlert("Inicio de sesion exitoso", "Bienvenido", "OK");

        }
        catch (Exception)
        {
            await Application.Current.MainPage.DisplayAlert("Inicio de sesion", "Ocurrio un problema", "Ok");
        }
    }


}