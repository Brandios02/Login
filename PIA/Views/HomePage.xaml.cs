
namespace PIA.Views;

using Firebase.Auth;
using Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }
    private async void btn_RegEmp(object sender, EventArgs e)
    {
        //SemanticScreenReader.Announce(btn_RegEmp);
        await Navigation.PushModalAsync(new Views.RegistroPage());
    }

    private async void btn_login(object sender, EventArgs e)
    {
        //SemanticScreenReader.Announce(btn_RegEmp);
        await Navigation.PushModalAsync(new Views.NewPage1());
    }
}