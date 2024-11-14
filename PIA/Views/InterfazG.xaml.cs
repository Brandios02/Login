using Microsoft.Maui.Controls;

namespace PIA.Views
{
    public partial class InterfazG : ContentPage
    {
        public InterfazG()
        {
            InitializeComponent();
        }

        private async void IB_VerVisitas(object sender, EventArgs e)
        {
            // Navegar a la p�gina de visitas programadas o mostrar una lista
            await DisplayAlert("Ver Visitas", "Funci�n para ver visitas programadas.", "OK");
        }

        private async void IB_RegistrarVisitante(object sender, EventArgs e)
        {
            // Navegar a la p�gina de registro de visitantes o abrir formulario de registro
            await DisplayAlert("Registrar Visitante", "Funci�n para registrar un visitante.", "OK");
        }

        private async void IB_GenerarReporte(object sender, EventArgs e)
        {
            // Navegar a la p�gina de generaci�n de reportes o mostrar opciones de reporte
            await DisplayAlert("Generar Reporte", "Funci�n para generar reportes.", "OK");
        }
    }
}
