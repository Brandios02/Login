using Firebase.Auth;
using Firebase.Database;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using PIA.Models;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using Firebase.Auth.Providers;
using Microsoft.Maui.Storage;

namespace PIA.Views
{
    public partial class RegistroPage : ContentPage
    {
        // Configuración de FireSharp (Realtime Database)
        private IFirebaseClient firebaseClient;
        private const string FirebaseUrl = "https://prueba-726d0-default-rtdb.firebaseio.com/";  // URL de tu base de datos Firebase

        // Variable para FirebaseAuthClient
        private readonly FirebaseAuthClient _firebaseAuthClient;

        // Constructor con inyección de dependencias
        public RegistroPage(FirebaseAuthClient firebaseAuthClient)
        {
            InitializeComponent();
            _firebaseAuthClient = firebaseAuthClient;  // Guardamos la instancia de FirebaseAuthClient

            // Configura FireSharp con la URL y tu Firebase Database Secret
            var config = new FirebaseConfig
            {
                AuthSecret = "UvbLQ8Lcr4OGYvzyo2TV8ejRlHxtYkDlbDxPBUNl",  // Firebase Database Secret
                BasePath = FirebaseUrl
            };
            firebaseClient = new FireSharp.FirebaseClient(config);  // Crear la instancia de FireSharp
        }

        private async void btnRegistrar(object sender, EventArgs e)
        {
            var nombre = nameEntry.Text;
            var telefono = phoneEntry.Text;
            var email = emailEntry.Text;
            var password = PasswordEntry.Text;
            var tipoUsuario = tipousEntry.SelectedItem?.ToString();  // Puede ser "Guardia" o "Residente"

            // Verifica que no haya campos vacíos
            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(telefono) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(tipoUsuario))
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            try
            {
                // Registrar al usuario en Firebase Authentication
                var authResponse = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(email, password);

                if (authResponse != null)
                {
                    // Crear un nuevo objeto de usuario para guardar en la base de datos
                    var nuevaEmpresa = new Usuario
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = nombre,
                        Phone = telefono,
                        Email = email,
                        Role = tipoUsuario
                    };

                    // Modificar el email para que sea seguro en la ruta (sin caracteres no permitidos)
                    var emailPathSafe = email.Replace(".", "_").Replace("@", "_");

                    // Guarda la nueva empresa en Firebase Realtime Database
                    SetResponse response = firebaseClient.Set("Usuarios/" + emailPathSafe, nuevaEmpresa);

                    // Verificar la respuesta de Firebase
                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        await DisplayAlert("Éxito", "Usuario registrado exitosamente.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo registrar usuario.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo registrar el usuario en Firebase Authentication.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
            }
        }
    }
}
