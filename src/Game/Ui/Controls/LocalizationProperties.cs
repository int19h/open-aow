using System.Runtime.CompilerServices;

using Aow.Game.Ui.Windows;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Controls;

internal static class LocalizationProperties
{
    [ModuleInitializer]
    internal static void Register()
    {
        TranslatorProperties.Register<AOWLabel>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.Register<AOWButton>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.Register<AOWEdit>("Text", c => c.Text, (c, value) => c.Text = value);
        TranslatorProperties.RegisterStrings<AOWControl>("ToolTipText", c => c.ToolTipText);
        TranslatorProperties.RegisterStrings<AOWComboBox>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<TaowHTMLMemo>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<AOWListBox>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<AOWMemo>("Items", c => c.Items);
        TranslatorProperties.RegisterStrings<AOWTabPanel>("Items", c => c.Items);
    }
}
