namespace PIA.Views;

public partial class InterfazUs : ContentPage
{
	public InterfazUs()
	{
		InitializeComponent();
	}
    private async void IB_Gvisita(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new Gvisita()); 
    }
    private async void IB_Evisita(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new Evisita()); 
    }
    private async void IB_Cguardia(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new Cguardia());
    }
}