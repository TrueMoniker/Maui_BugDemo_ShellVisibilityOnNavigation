namespace MauiApp1;

public partial class Page1 : ContentPage
{
	public Page1()
	{
		InitializeComponent();
	}

	private async void OpenBtn_Clicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("Page2");
    }
}