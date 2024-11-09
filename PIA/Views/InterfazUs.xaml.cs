namespace PIA.Views;

public partial class InterfazUs : ContentPage
{
	public InterfazUs()
	{
		InitializeComponent();
	}
    private async void IB_Gvisita(object sender, EventArgs e)
    {
        // Navega a la página correspondiente cuando se haga clic en el primer ImageButton
        await Navigation.PushAsync(new Gvisita()); // Sustituye VisitaPage por la página correcta
    }
}