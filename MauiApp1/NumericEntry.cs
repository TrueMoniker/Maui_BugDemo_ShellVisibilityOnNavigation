namespace MauiApp1;

/// <summary>
/// An entry to bind a double value as the Text.
/// </summary>
// When binding a double to a numeric entry, an Entry control with Keyboard=Numeric uses a string, but then binds to a double. 
// This causes values like "-0.0" to convert to "0", so users are unable to type -0.01.
// So I am manually tracking the bound value separate from the displayed value.
// 
// The base.Text is the string value that the entry is displaying visually. 
// The this.Text value is hiding base.Text and is the bound double that the model is actually tracking.
// The idea is to update the bound value when the text changes, 
// and only update the text when the bound value changes to something that the text is not equivalent to. 
// So for instance if the entry says -0.0 and the bound value is 0, the text will stay -0.0 rather than update to 0.

// IMPORTANT: my entries allow a nullable value. If you want to use a default instead of null, you'll need to implement that.

// Had to use a control rather than a handler, b/c I couldn't find a way to intercept the text changing other than the TextChanged listener.
// When I changed the value back, it caused a race condition that sometimes overwrote values. 
// For example if you type -0.00012 quickly, it would convert -0.000 to 0, then my code would change it back, but after you typed the 1 and before you typed the 2, so the final value would be -0.0002.
public partial class NumericEntry : Entry
{
    /// <summary>
    /// <inheritdoc cref="NumericEntry"/>
    /// </summary>
	public NumericEntry()
	{
        this.Keyboard = Keyboard.Numeric;
        base.TextChanged += SetText;
    }

    private void SetText(object? _s, EventArgs? _e)
    {
        if (string.IsNullOrEmpty(base.Text))
        {
            this.Text = null;
        }
        //If the base value and the bound value aren't matching, when need to update the bound value.
        else if (!IsEqualMathematically(base.Text, this.Text, out double? castText))
        {
           this.Text = castText;
        }
    }

    /// <inheritdoc/>
    protected override void OnBindingContextChanged()
    {
        UpdateViewWhenReused();
        base.OnBindingContextChanged();
    }

    /// <summary>
    /// When this control is used in a collection, it can be recycled. We have to update the text so it has the bound value instead of previous value.
    /// </summary>
    private void UpdateViewWhenReused()
    {
        //If the base value and the bound value aren't matching when the binding context changed, when need to update the base value.
        if (!double.TryParse(base.Text, out double result) || result != this.Text)
        {
            SetBaseText(this.Text.ToString());
        }
    }

    /// <summary>
    /// The text to display.
    /// </summary>
    public static new readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(double?), typeof(Entry), null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: OnTextChanged);

    /// <summary>
    /// The text to bind to. We're hiding the base Entry Text property so we can force it to be a number.
    /// </summary>
    public new double? Text
    {
        get => (double?)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// Override the EntryTextChanged so we have a TextChanged value to invoke only when the bound value of the double changes. That way "TextChanged" events still work with most logic. 
    /// For example changing 0.0 to 0.01 will cause it to trigger, but changing 0.0 to 0.00 won't cause the change.
    /// 
    /// If you need to access the base TextChanged, cast your instance to an Entry and access its TextChanged event.
    /// </summary>
    public new event EventHandler<TextChangedEventArgs> TextChanged;

    //This is called when the bound Text value changes, ie the bound double.
    //We need to update the displayed text to match the bound double, if the value is not equivalent
    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is NumericEntry entry)
        {
            if (newValue is null)
            {
                entry.SetBaseText(null);
            }
            else if (newValue is double newDouble)
            {
                //If the values match mathematically, we don't need to update the text. Example if the text is -0.0 and the double is 0, we can leave the text as -0.0
                if (!IsEqualMathematically((entry as Entry).Text, newDouble, out double? castText))
                {
                    //Casting double to decimal on purpose, otherwise the double.ToString will go to scientific notation if it is too long.
                    entry.SetBaseText(((decimal)newDouble).ToString());
                }
            }
        }
    }

    //Checks the mathematical value of the existingText string. against the newValue double. 
    //Example "-.0" is equivalent to 0
    private static bool IsEqualMathematically(string? text, double? toCompare, out double? mydouble)
    {
        if (text is null && toCompare is null)
        {
            mydouble = null;
            return true;
        }
        else
        {
            if (text is not null && toCompare is not null)
            {
                var isEqual = double.TryParse(text, out double castText) && castText == toCompare;
                mydouble = castText;
                return isEqual;
            }
            else
            {
                mydouble = null;
                return false;
            }
        }
    }

    private void SetBaseText(string? value)
    {
        var oldValue = base.Text;
        base.Text = value;
        TextChanged?.Invoke(this, new TextChangedEventArgs(oldValue, value));
    }
}
