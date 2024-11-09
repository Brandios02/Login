#if ANDROID
using Android.Graphics;
#endif

using Firebase.Database;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using ZXing; // Para generar el QR
using ZXing.Mobile;
using System.IO; // Para MemoryStream

namespace PIA.Views
{
    public partial class Gvisita : ContentPage
    {
        private IFirebaseClient firebaseClient;
        private const string FirebaseUrl = "https://prueba-726d0-default-rtdb.firebaseio.com/";

        public Gvisita()
        {
            InitializeComponent();
            var config = new FirebaseConfig
            {
                AuthSecret = "UvbLQ8Lcr4OGYvzyo2TV8ejRlHxtYkDlbDxPBUNl",
                BasePath = FirebaseUrl
            };
            firebaseClient = new FireSharp.FirebaseClient(config);
        }

        private async void btnGenQR(object sender, EventArgs e)
        {
            string visitorName = nameEntry.Text;
            string visitType = rolePicker.SelectedItem.ToString();

            DateTime expirationDate = visitType == "Frecuente" ? DateTime.MaxValue : DateTime.Now.AddHours(24);
            string qrContent = $"Nombre: {visitorName}\nTipo de Visita: {visitType}\nExpiración: {expirationDate}";

#if ANDROID
            // Crear BarcodeWriter con tipo de salida Bitmap
            var barcodeWriter = new BarcodeWriter<Bitmap>
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300
                }
            };

            // Genera el QR como un objeto Bitmap
            var qrBitmap = barcodeWriter.Write(qrContent);

            // Convierte el Bitmap en un arreglo de bytes
            using (var stream = new MemoryStream())
            {
                qrBitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Seek(0, SeekOrigin.Begin); // Reinicia la posición del stream

                // Asigna el ImageSource desde el stream
                qrImageView.Source = ImageSource.FromStream(() => new MemoryStream(stream.ToArray()));
            }
#endif

            await RegisterVisit(visitorName, visitType, expirationDate);
        }

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

            SetResponse response = firebaseClient.Set($"visitas/{visitId}", visita);

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
