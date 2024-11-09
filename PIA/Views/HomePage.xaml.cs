using Firebase.Auth;
using Firebase.Auth.Providers;
using PIA.Views;

namespace PIA.Views
{
    public partial class HomePage : ContentPage
    {
        private FirebaseAuthClient _firebaseAuthClient;

        public HomePage()
        {
            InitializeComponent();

            // Configura FirebaseAuthClient con FirebaseAuthConfig
            var firebaseAuthConfig = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyC6ZJBkRRBAk5xm99vHToHlLLmSsxMxles",
                AuthDomain = "prueba-726d0.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider() // Asegúrate de incluir el EmailProvider
                }
            };

            _firebaseAuthClient = new FirebaseAuthClient(firebaseAuthConfig);
        }

        private async void btn_RegEmp(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Views.RegistroPage(_firebaseAuthClient));
        }

        private async void btn_login(object sender, EventArgs e)
        {
            // Crea una instancia de NewPage1 y pasa el FirebaseAuthClient
            var newPage = new NewPage1(_firebaseAuthClient);
            await Navigation.PushModalAsync(newPage);
        }
    }
}
