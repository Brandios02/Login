using Microsoft.Maui.ApplicationModel;  // Importa la clase Launcher
using Microsoft.Maui.Controls;
using System;

namespace PIA.Views
{
    public partial class Cguardia : ContentPage
    {
        // El número de teléfono del guardia (reemplaza con el número real)
        private const string GuardPhoneNumber = "+528131976406";  // Cambia esto con el número real
        private const string GuardWhatsappNumber = "+528131976406";  // Cambia esto con el número real

        public Cguardia()
        {
            InitializeComponent();
        }

        // Método para hacer la llamada telefónica
        private async void CallGuardButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var phoneUri = new Uri($"tel:{GuardPhoneNumber}");
                await Launcher.OpenAsync(phoneUri);  // Usamos Launcher para abrir la aplicación de teléfono
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo realizar la llamada: {ex.Message}", "OK");
            }
        }

        // Método para abrir WhatsApp con un mensaje predefinido
        private async void ContactGuardOnWhatsApp_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Formatear el número de teléfono y mensaje para WhatsApp
                string message = Uri.EscapeDataString("Hola, necesito contactar con el guardia.");
                var whatsappUri = new Uri($"https://wa.me/{GuardWhatsappNumber}?text={message}");

                await Launcher.OpenAsync(whatsappUri);  // Usamos Launcher para abrir WhatsApp
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo abrir WhatsApp: {ex.Message}", "OK");
            }
        }
    }
}
