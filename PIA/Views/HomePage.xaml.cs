using Firebase.Auth;
using Firebase.Database;
using Firebase.Auth.Providers;
using PIA.Views;

namespace PIA.Views
{
    public partial class HomePage : ContentPage
    {
        private FirebaseAuthClient _firebaseAuthClient;
        private FirebaseClient _firebaseClient;

        public HomePage()
        {
            InitializeComponent();

            // Configura FirebaseAuthClient con FirebaseAuthConfig
            var firebaseAuthConfig = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyC6ZJBkRRBAk5xm99vHToHlLLmSsxMxles",  // Tu API key de Firebase
                AuthDomain = "prueba-726d0.firebaseapp.com",      // Tu dominio de autenticación
                Providers = new FirebaseAuthProvider[]  // Proveedores que usarás para la autenticación
                {
                    new EmailProvider()  // Proveedor para Email
                }
            };

            // Crear una instancia de FirebaseAuthClient
            _firebaseAuthClient = new FirebaseAuthClient(firebaseAuthConfig);

            // Configura FirebaseClient con la URL de la base de datos
            _firebaseClient = new FirebaseClient("https://prueba-726d0-default-rtdb.firebaseio.com/");
        }

        private async void btn_RegEmp(object sender, EventArgs e)
        {
            // Navegar a la página de registro y pasar el FirebaseAuthClient
            await Navigation.PushModalAsync(new RegistroPage(_firebaseAuthClient));
        }

        private async void btn_login(object sender, EventArgs e)
        {
            // Crear una nueva página de login y pasarle el FirebaseAuthClient (y FirebaseClient)
            var newPage = new NewPage1(_firebaseAuthClient, _firebaseClient);  // Pasamos ambos parámetros
            await Navigation.PushModalAsync(newPage);
        }
    }
}
