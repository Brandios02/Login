using Firebase.Database;
using Firebase.Database.Query;
using PIA.Models;

namespace PIA.Views;

public partial class RegistroPage : ContentPage
{
    FirebaseClient client = new FirebaseClient("https://prueba-726d0-default-rtdb.firebaseio.com/");
    public RegistroPage()
    {
        InitializeComponent();
    }

    private async void btnRegistrar(object sender, EventArgs e)
    {
        if (int.TryParse(idEmpEntry.Text, out int id))
        {
            var nombre = nameEntry.Text;
            var telefono = phoneEntry.Text;
            var email = emailEntry.Text;
            var password = PasswordEntry.Text;

            // Verifica que no haya campos vacíos
            if (string.IsNullOrWhiteSpace(nombre) ||
                string.IsNullOrWhiteSpace(telefono) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Por favor, completa todos los campos.", "OK");
                return;
            }

            // Recupera la lista de empresas existentes
            var empresas = await client.Child("Empresa").OnceAsync<Empresa>();

            // Verifica si ya existe una empresa con el mismo ID o nombre
            if (empresas.Any(e => e.Object.Id == id || e.Object.Name.Equals(nombre, StringComparison.OrdinalIgnoreCase)))
            {
                await DisplayAlert("Error", "Ya existe una empresa con el mismo ID o nombre.", "OK");
                return;
            }

            // Crea una nueva instancia de Empresa
            var nuevaEmpresa = new Empresa
            {
                Id = id,
                Name = nombre,
                Phone = telefono,
                Email = email,
                Password = password
            };

            // Publica la nueva empresa
            try
            {
                await client.Child("Empresa").PostAsync(nuevaEmpresa);
                await DisplayAlert("Éxito", "Empresa registrada exitosamente.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo registrar la empresa: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Error", "El ID debe ser un número entero.", "OK");
        }
    }


}