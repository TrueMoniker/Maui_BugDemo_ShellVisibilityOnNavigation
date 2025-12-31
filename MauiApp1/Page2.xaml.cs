namespace MauiApp1;

public partial class Page2 : ContentPage
{
	public Page2()
	{
		InitializeComponent();
	}

	private async void Broken_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("../..");
    }

	private async void Working_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("..");
		await Shell.Current.GoToAsync("..");
    }
}