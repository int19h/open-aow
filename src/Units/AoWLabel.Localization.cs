using System.Runtime.CompilerServices;
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWControl;
using Aow.Units.AoWEdit;
using Aow.Units.AoWHTMLMemo;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWTabPanel;

namespace Aow.Units.AoWLabel;

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
