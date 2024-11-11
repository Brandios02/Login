using Firebase.Database;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.Maui.Controls;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PIA.Views
{
    public partial class Gvisita : ContentPage
    {
        private IFirebaseClient firebaseClient;
        private const string FirebaseUrl = "https://prueba-726d0-default-rtdb.firebaseio.com/"; // Tu URL de Firebase
        private const string ApiUrl = "http://192.168.0.3:7280/api/qr/generate-qr"; // URL de tu API (cambiar si usas HTTPS)

        public Gvisita()
        {
            InitializeComponent();
            var config = new FirebaseConfig
            {
                AuthSecret = "UvbLQ8Lcr4OGYvzyo2TV8ejRlHxtYkDlbDxPBUNl", // Tu clave de autenticación
                BasePath = FirebaseUrl
            };
            firebaseClient = new FireSharp.FirebaseClient(config);
        }

        private async void BtnGenQR(object sender, EventArgs e)
        {
            string visitorName = nameEntry.Text;  // Nombre del visitante
            string visitType = rolePicker.SelectedItem.ToString();  // Tipo de visita seleccionada

            // Establecer la fecha de expiración dependiendo del tipo de visita
            DateTime expirationDate = visitType == "Frecuente" ? DateTime.MaxValue : DateTime.Now.AddHours(24);
            string qrContent = $"Nombre: {visitorName}\nTipo de Visita: {visitType}\nExpiración: {expirationDate}";

            // Llamar a la API para generar el código QR
            var qrImage = await GenerateQrFromApi(qrContent);

            if (qrImage != null)
            {
                // Si la imagen QR se genera correctamente
                Console.WriteLine("QR generado correctamente, mostrando la imagen.");

                // Actualizar la UI con el QR recibido
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    qrImageView.Source = ImageSource.FromStream(() => new MemoryStream(qrImage));
                });
            }
            else
            {
                // Si la imagen QR no se genera correctamente
                Console.WriteLine("Error: No se pudo generar el QR desde la API.");
                await DisplayAlert("Error", "No se pudo generar el código QR.", "OK");
            }

            // Registra la visita en la base de datos Firebase
            await RegisterVisit(visitorName, visitType, expirationDate);
        }

        // Llamada a la API para generar el código QR
        private async Task<byte[]> GenerateQrFromApi(string content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Preparar la URL de la API y agregar el parámetro 'content' a la solicitud
                    var response = await client.GetAsync($"{ApiUrl}?content={Uri.EscapeDataString(content)}");

                    Console.WriteLine($"Estado de la respuesta de la API: {response.StatusCode}");

                    if (response.IsSuccessStatusCode)
                    {
                        // Leer el contenido de la respuesta (la imagen QR en formato PNG)
                        var qrBytes = await response.Content.ReadAsByteArrayAsync();
                        Console.WriteLine($"Tamaño de la imagen QR recibida: {qrBytes.Length} bytes");

                        // Verificar si la imagen tiene contenido
                        if (qrBytes.Length > 0)
                        {
                            Console.WriteLine("Imagen QR generada correctamente");
                            return qrBytes;
                        }
                        else
                        {
                            Console.WriteLine("La respuesta de la API no contiene imagen válida.");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine("La API devolvió un error al generar el QR.");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar errores de conexión o de la API
                Console.WriteLine($"Error al generar el QR: {ex.Message}");
                return null;
            }
        }

        // Registrar la visita en Firebase
        private async Task RegisterVisit(string visitorName, string visitType, DateTime expirationDate)
        {
            var visita = new
            {
                VisitorName = visitorName,
                VisitType = visitType,
                ExpirationDate = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            var visitasRef = firebaseClient.Get("visitas");
            string visitId = Guid.NewGuid().ToString();

            // Guardar la visita en Firebase
            SetResponse response = firebaseClient.Set($"visitas/{visitId}", visita);

            // Mostrar un mensaje según si la operación fue exitosa o no
            if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                await DisplayAlert("Éxito", "Visita registrada y QR generado.", "OK");
            }
            else
            {
                await DisplayAlert("Error", "No se pudo registrar la visita.", "OK");
            }
        }
    }
}
