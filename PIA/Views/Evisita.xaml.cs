using Microsoft.Maui.Controls;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PIA.Models;

namespace PIA.Views
{
    public partial class Evisita : ContentPage
    {
        private IFirebaseClient firebaseClient;
        private const string FirebaseUrl = "https://prueba-726d0-default-rtdb.firebaseio.com/";
        private const string FirebaseAuthSecret = "UvbLQ8Lcr4OGYvzyo2TV8ejRlHxtYkDlbDxPBUNl";  // Cambia con tu authSecret

        public Evisita()
        {
            InitializeComponent();

            // Configuración del cliente Firebase
            var config = new FirebaseConfig
            {
                AuthSecret = FirebaseAuthSecret,
                BasePath = FirebaseUrl
            };
            firebaseClient = new FireSharp.FirebaseClient(config);

            // Cargar visitas al inicializar la página
            LoadVisitas();
        }

        // Cargar las visitas desde Firebase
        private async void LoadVisitas()
        {
            try
            {
                // Obtenemos todas las visitas desde Firebase
                FirebaseResponse response = firebaseClient.Get("visitas");
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Deserializar la respuesta de Firebase en un diccionario
                    var visitasDict = response.ResultAs<Dictionary<string, Visita>>();
                    if (visitasDict != null)
                    {
                        // Convertimos el diccionario en una lista de objetos Visita
                        var visitas = new List<Visita>();
                        foreach (var visita in visitasDict.Values)
                        {
                            visitas.Add(visita);
                        }

                        // Asignar las visitas al ListView
                        visitasListView.ItemsSource = visitas;
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontraron visitas registradas.", "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No se pudo conectar con la base de datos.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Hubo un problema al cargar las visitas: {ex.Message}", "OK");
            }
        }

        // Evento cuando se selecciona una visita
        private void OnVisitSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Verificamos si se ha seleccionado una visita
            var selectedVisit = e.SelectedItem as Visita;
            if (selectedVisit != null)
            {
                // Activar el botón de eliminar si se selecciona una visita
                deleteVisitButton.IsEnabled = true;
            }
            else
            {
                // Desactivar el botón si no se seleccionó ninguna visita
                deleteVisitButton.IsEnabled = false;
            }
        }

        // Evento cuando se hace clic en el botón de eliminar
        private async void OnDeleteVisitClicked(object sender, EventArgs e)
        {
            var selectedVisit = visitasListView.SelectedItem as Visita;
            if (selectedVisit != null)
            {
                bool confirmDelete = await DisplayAlert(
                    "Confirmar Eliminación",
                    $"¿Estás seguro de eliminar la visita de {selectedVisit.VisitorName}?",
                    "Sí", "No");

                if (confirmDelete)
                {
                    // Verificamos que la visita tenga un Id único
                    if (string.IsNullOrEmpty(selectedVisit.Id))
                    {
                        await DisplayAlert("Error", "La visita no tiene un identificador válido.", "OK");
                        return;
                    }

                    // Eliminar la visita seleccionada en Firebase utilizando su Id
                    FirebaseResponse response = firebaseClient.Delete($"visitas/{selectedVisit.Id}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        // Recargar las visitas después de la eliminación
                        await DisplayAlert("Éxito", "Visita eliminada correctamente.", "OK");
                        LoadVisitas(); // Recargar la lista de visitas
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se pudo eliminar la visita.", "OK");
                    }
                }
            }
            else
            {
                await DisplayAlert("Error", "No se ha seleccionado ninguna visita para eliminar.", "OK");
            }
        }
    }
}
