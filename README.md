# Maui_BugDemo_ShellVisibilityOnNavigation
[maui/issues/33351](https://github.com/dotnet/maui/issues/33351)

While navigating back multiple pages, ie Shell.Current.GoToAsync("../.."), if I change the Shell Tab Bar visiblity, the tab bar is not visible.

# Demo
1. Run app on iOS or Android
2. Notice the tab bar
3. Select "Open Example" button
4. Notice the tab bar disappeared
5. Select Page1's "Open Example" button
6. Select "Working - Go Back" button
7. Notice the tab bar appeared
8. Repeat steps 3-5.
9. Select "Broken - Go Back" button
10. Notice the tab bar did not appear.

# Work Around
Navigate by a single page only
```C#
await Shell.Current.GoToAsync(".."); 
await Shell.Current.GoToAsync("..");
```

Instead of 
```C#
await Shell.Current.GoToAsync("../.."); 
```
