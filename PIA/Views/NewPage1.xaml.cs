using Firebase.Auth;
using PIA.Models;

namespace PIA.Views
{
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
            // Validar que los campos no est�n vac�os
            if (string.IsNullOrWhiteSpace(emailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            try
            {
                // Intentar iniciar sesi�n
                await _clientAuth.SignInWithEmailAndPasswordAsync(emailEntry.Text, PasswordEntry.Text);
                await DisplayAlert("Inicio de sesi�n exitoso", "Bienvenido", "OK");

                // Cerrar la p�gina modal
                await Navigation.PopModalAsync();

                // Navegar a InterfazUs despu�s del inicio de sesi�n
                await Application.Current.MainPage.Navigation.PushAsync(new InterfazUs());
            }
            catch (Exception ex)
            {
                // Mostrar el mensaje de error
                await DisplayAlert("Inicio de sesi�n", ex.Message, "OK");
            }
        }
    }
}
