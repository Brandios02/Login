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
            // Validar que los campos no estén vacíos
            if (string.IsNullOrWhiteSpace(emailEntry.Text) || string.IsNullOrWhiteSpace(PasswordEntry.Text))
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            try
            {
                // Intentar iniciar sesión
                await _clientAuth.SignInWithEmailAndPasswordAsync(emailEntry.Text, PasswordEntry.Text);
                await DisplayAlert("Inicio de sesión exitoso", "Bienvenido", "OK");

                // Cerrar la página modal
                await Navigation.PopModalAsync();

                // Navegar a InterfazUs después del inicio de sesión
                await Application.Current.MainPage.Navigation.PushAsync(new InterfazUs());
            }
            catch (Exception ex)
            {
                // Mostrar el mensaje de error
                await DisplayAlert("Inicio de sesión", ex.Message, "OK");
            }
        }
    }
}
