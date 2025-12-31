using System;

namespace MauiApp1;

public class MyTab : ShellContent
{
    protected override void OnAppearing()
    {
        base.OnAppearing();

        Shell.SetTabBarIsVisible(this.Parent, true);

        Shell.Current.Navigating += (arg, arg2) => MyMethod();
    }

    private void MyMethod()
    {
        Shell.SetTabBarIsVisible(this.Parent, false);
    }
}
