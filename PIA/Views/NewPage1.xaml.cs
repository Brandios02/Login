using Firebase.Auth;
using Firebase.Database;
using PIA.Models;
using Microsoft.Maui.Controls;
using System;

namespace PIA.Views
{
    public partial class NewPage1 : ContentPage
    {
        private readonly FirebaseAuthClient _clientAuth;
        private readonly FirebaseClient _firebaseClient;

        // Constructor que recibe FirebaseAuthClient y FirebaseClient
        public NewPage1(FirebaseAuthClient firebaseAuthClient, FirebaseClient firebaseClient)
        {
            InitializeComponent();
            _clientAuth = firebaseAuthClient;
            _firebaseClient = firebaseClient;
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
