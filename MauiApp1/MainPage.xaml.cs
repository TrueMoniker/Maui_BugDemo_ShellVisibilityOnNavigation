using System.Threading.Tasks;

namespace MauiApp1;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

    private async void OpenBtn_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("Page1");
    }
}

