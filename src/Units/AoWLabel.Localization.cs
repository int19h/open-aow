using System.Runtime.CompilerServices;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Controls;

internal static class LocalizationProperties
{
    [ModuleInitializer]
    internal static void Register()
    {
        TranslatorProperties.Register<TAOWLabel>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.Register<TAOWButton>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.Register<TAOWEdit>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.RegisterStrings<TAOWControl>("ToolTipText", c => c.ToolTipText);
        TranslatorProperties.RegisterStrings<TAOWComboBox>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<TaowHTMLMemo>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<TAOWListBox>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<TAOWMemo>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<TAOWTabPanel>("Items", c => c.Items);
    }
}
