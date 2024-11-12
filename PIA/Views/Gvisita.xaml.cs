using Firebase.Database;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Microsoft.Maui.Controls;
using PIA.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace PIA.Views
{
    public partial class Gvisita : ContentPage
    {
        private IFirebaseClient firebaseClient;
        private FirebaseAuthProvider _firebaseAuthProvider; // Para autenticación
        private const string FirebaseUrl = "https://prueba-726d0-default-rtdb.firebaseio.com/"; // Tu URL de Firebase
        private const string ApiUrl = "http://192.168.0.3:7280/api/qr/generate-qr"; // URL de tu API (cambiar si usas HTTPS)

        public Gvisita()
        {
            InitializeComponent();

            // Configuración del cliente de Firebase
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

                // Aquí es donde puedes colocar el código para mostrar el QR en un ImageView
                // Si tienes un control Image en la UI para mostrar la imagen, por ejemplo, 'qrImageView'
                qrImageView.Source = ImageSource.FromStream(() => new MemoryStream(qrImage));

                // Ahora registrar la visita en Firebase, ya que el QR se generó correctamente
                bool visitRegistered = await RegisterVisit(visitorName, visitType, expirationDate);

                // Si el registro en Firebase fue exitoso, mostrar la alerta de éxito
                if (visitRegistered)
                {
                    await DisplayAlert("Éxito", "Visita registrada y QR generado.", "OK");
                }
                else
                {
                    // Si no se pudo registrar la visita en Firebase
                    await DisplayAlert("Error", "No se pudo registrar la visita en Firebase.", "OK");
                }
            }
            else
            {
                // Si la imagen QR no se genera correctamente, mostrar solo un mensaje de error
                Console.WriteLine("Error: No se pudo generar el QR desde la API.");
                await DisplayAlert("Error", "No se pudo generar el código QR.", "OK");
            }
        }

        // Llamada a la API para generar el código QR
        private async Task<byte[]> GenerateQrFromApi(string content)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // Crear la URL de la solicitud para verificar
                    var requestUrl = $"{ApiUrl}?content={Uri.EscapeDataString(content)}";
                    Console.WriteLine($"Realizando solicitud a: {requestUrl}"); // Depuración de la URL

                    // Realizamos la solicitud GET a la API para generar el QR
                    var response = await client.GetAsync(requestUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        // Leemos la respuesta como un arreglo de bytes (la imagen en formato PNG)
                        var qrBytes = await response.Content.ReadAsByteArrayAsync();

                        // Verificamos que el arreglo de bytes contiene datos
                        if (qrBytes.Length > 0)
                        {
                            Console.WriteLine($"QR generado correctamente, tamaño de la imagen: {qrBytes.Length} bytes.");
                            return qrBytes;
                        }
                        else
                        {
                            // Si la respuesta de la API no contiene datos de imagen
                            Console.WriteLine("La respuesta de la API no contiene una imagen válida.");
                            return null;
                        }
                    }
                    else
                    {
                        // Si la respuesta no es exitosa, mostramos el código de estado y el contenido de la respuesta
                        var errorContent = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error en la respuesta de la API. Código de estado: {response.StatusCode}. Contenido: {errorContent}");
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejo de excepciones en caso de error en la llamada a la API
                Console.WriteLine($"Error al generar el QR: {ex.Message}");
                return null;
            }
        }

        // Registrar la visita en Firebase y devolver un valor booleano de éxito
        private async Task<bool> RegisterVisit(string visitorName, string visitType, DateTime expirationDate)
        {

            var visita = new Visita
            {
                Id = Guid.NewGuid().ToString(),
                VisitorName = visitorName,
                VisitType = visitType,
                ExpirationDate = expirationDate.ToString("yyyy-MM-dd HH:mm:ss"),
                DateCreated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                AccesoAutorizado = true
            };


            try
            {
                // Guardar la visita en Firebase
                SetResponse response = firebaseClient.Set($"visitas/{visita.Id}", visita);

                // Si la respuesta es OK, devolvemos true, indicando que el registro fue exitoso
                return response != null && response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                // En caso de error al registrar la visita, lo registramos en el log
                Console.WriteLine($"Error al registrar la visita en Firebase: {ex.Message}");
                return false;
            }
        }
    }
}
