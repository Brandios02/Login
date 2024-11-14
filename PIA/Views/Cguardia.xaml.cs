using Microsoft.Maui.ApplicationModel;  // Importa la clase Launcher
using Microsoft.Maui.Controls;
using System;

namespace PIA.Views
{
    public partial class Cguardia : ContentPage
    {
        // El n�mero de tel�fono del guardia (reemplaza con el n�mero real)
        private const string GuardPhoneNumber = "+528131976406";  // Cambia esto con el n�mero real
        private const string GuardWhatsappNumber = "+528131976406";  // Cambia esto con el n�mero real

        public Cguardia()
        {
            InitializeComponent();
        }

        // M�todo para hacer la llamada telef�nica
        private async void CallGuardButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var phoneUri = new Uri($"tel:{GuardPhoneNumber}");
                await Launcher.OpenAsync(phoneUri);  // Usamos Launcher para abrir la aplicaci�n de tel�fono
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"No se pudo realizar la llamada: {ex.Message}", "OK");
            }
        }

        // M�todo para abrir WhatsApp con un mensaje predefinido
        private async void ContactGuardOnWhatsApp_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Formatear el n�mero de tel�fono y mensaje para WhatsApp
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
