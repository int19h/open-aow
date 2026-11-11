using System.Runtime.CompilerServices;
using Aow.Game.UI.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Controls;

internal static class LocalizationProperties
{
    [ModuleInitializer]
    internal static void Register()
    {
        TranslatorProperties.Register<Label>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.Register<Button>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.Register<Edit>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.RegisterStrings<Control>("ToolTipText", c => c.ToolTipText);
        TranslatorProperties.RegisterStrings<ComboBox>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<HtmlMemo>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<ListBox>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<Memo>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<TabPanel>("Items", c => c.Items);
    }
}
