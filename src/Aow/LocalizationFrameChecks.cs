#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using System.Text.Json;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.UI.Dialogs;
using Aow.Graphics;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow;

internal static class LocalizationFrameChecks
{
    public static void Run(string root, string output, LayoutSession layouts, UIContext ui)
    {
        int language = LocalizationRuntime.ReadLanguage(RtlHost.Settings);
        SortedSet<byte> germanBytes = [];
        foreach (string file in new[] { "AoW", "ResStr", "CustMaps", "Release" })
        {
            MldDictionary dictionary = new();
            dictionary.Open(Path.Combine(root, "Dict", $"{file}.mld"));
            foreach (MldEntry entry in dictionary.Entries)
            {
                for (int index = 1; index <= entry.Translations[1].Length; index++)
                {
                    byte code = Cp1252.Byte(entry.Translations[1][index - 1]);
                    if (code >= 128)
                    {
                        _ = germanBytes.Add(code);
                    }
                }
            }

            dictionary.Close();
        }
        ImageLibraryFont[] fonts = [ ui.Fonts.Age8, ui.Fonts.Age10, ui.Fonts.Age16Shadow,
            ui.Fonts.Age10Shadow, ui.Fonts.Age8Shadow, ui.Fonts.Age14Gold, ui.Fonts.Age15WhiteOnGrey,
            ui.Fonts.Age15BlackOnBrown, ui.Fonts.Age10List, ui.Fonts.Age10Bold, ui.Fonts.Age10BoldShadow ];
        List<object> coverage = [.. fonts.Select(font =>
        {
            byte[] missing = [.. germanBytes.Where(code => !font.ImageExist(code) || font[code].Width <= 0 || font[code].Height <= 0)];
            return (object)new { font = font.Name, missing = missing.Select(static glyph => (int)glyph).ToArray() };
        })];
        string undead = ui.Engine.TranslateResourceString("Undead(plural)");
        bool undeadEqual = language != 1 || undead == "Undead";
        string mapCaption = ui.ScannerWindow.TitleLabel.Text;
        string expectedMapCaption = language == 2 ? "Weltkarte" : "World Map";
        if (mapCaption != expectedMapCaption)
        {
            throw new InvalidOperationException("The startup form translation was not applied");
        }
        // The file dialog title holds the one German text with a byte from 128 to 159 (ResStr.mld,
        // "Spiel speichern unter" and the ellipsis 0x85), shown by the languages-de-save fixture.
        string dialogTitle = ui.FileDialogWindow?.TitleLabel?.Text ?? "";
        SortedSet<string> required = [with(StringComparer.Ordinal)];
        // A byte that no installed font draws, such as the ellipsis 0x85, leaves the original's
        // 5 pixel gap. The frame comparison checks that gap, so it is recorded, not a missing glyph.
        HashSet<int> absentEverywhere = [.. Enumerable.Range(128, count: 128).Where(code => fonts.All(font => !font.ImageExist(code) || font[code].Width <= 0 || font[code].Height <= 0))];
        SortedSet<string> absent = [with(StringComparer.Ordinal)];
        foreach (Component module in layouts.Modules.Values)
        {
            for (int index = 0; index < module.ComponentCount; index++)
            {
                if (module.Components[index] is not Game.UI.Controls.Control control ||
                    !control.Visible || control.Window is not { Visible: true } || control.Font is not { } font)
                {
                    continue;
                }

                List<string> texts = ItemsOf(control) is DelphiStringCollection items
                    ? [control.Text, .. items]
                    : [control.Text];

                foreach (string text in texts)
                {
                    string cased = font.CharCase switch
                    {
                        EditCharCase.UpperCase => DelphiTextCompare.UpperCase(text),
                        EditCharCase.LowerCase => DelphiTextCompare.LowerCase(text),
                        EditCharCase.Normal or _ => text
                    };
                    for (int character = 1; character <= cased.Length; character++)
                    {
                        byte code = Cp1252.Byte(cased[character - 1]);
                        if (code < 128)
                        {
                            continue;
                        }

                        bool missing = !font.ImageExist(code) || font[code].Width <= 0 || font[code].Height <= 0;
                        if (missing && absentEverywhere.Contains(code))
                        {
                            _ = absent.Add($"{font.Name}:{code}");
                            continue;
                        }
                        _ = required.Add($"{font.Name}:{code}");
                        if (missing)
                        {
                            throw new InvalidOperationException($"Missing visible fixture glyph {code} in {font.Name}, component {control.Name}");
                        }
                    }
                }
            }
        }

        File.WriteAllText(Path.Combine(output, "localization.json"), contents: JsonSerializer.Serialize(new
        {
            language,
            undead,
            undead_equal = undeadEqual,
            map_caption = mapCaption,
            file_dialog_title = dialogTitle,
            file_dialog_title_bytes = Cp1252.Encode(dialogTitle).ToArray().Select(static b => (int)b).ToArray(),
            fixture_glyphs_equal = true,
            required_glyphs = required,
            absent_glyphs = absent,
            german_bytes = germanBytes,
            coverage
        }, options: new JsonSerializerOptions { WriteIndented = true }) + "\n");
        if (!undeadEqual)
        {
            throw new InvalidOperationException("English Undead(plural) did not translate to Undead");
        }

        if (language == 1)
        {
            Dib16Surface left = new(120, 40, pixelFormatId: PixelFormat.Rgb555);
            Dib16Surface right = new(120, 40, pixelFormatId: PixelFormat.Rgb555);
            _ = left.Clear(Color.Black);
            _ = right.Clear(Color.Black);
            LockedSurface l = default, r = default;
            _ = left.Lock(ref l);
            _ = right.Lock(ref r);
            try
            {
                ui.Fonts.Age8.SetText(0, 0, text: undead, lockedSurface: l);
                ui.Fonts.Age8.SetText(0, 0, text: "Undead", lockedSurface: r);
                Span<byte> pixels = l.Data.AsSpan(l.DataIndex, length: l.Pitch * 40);
                if (!pixels.SequenceEqual(r.Data.AsSpan(r.DataIndex, length: r.Pitch * 40)) || !pixels.ToArray().Any(b => b != 0))
                {
                    throw new InvalidOperationException("English Undead rendering differs");
                }
            }
            finally
            {
                _ = left.Unlock();
                _ = right.Unlock();
            }
        }
        Console.WriteLine($"Localization frames: language {language}, Undead(plural) = {undead}, {germanBytes.Count} German extended bytes scanned across {fonts.Length} fonts");
    }

    // A typed list box redeclares Items, so the nearest declaration wins.
    private static object? ItemsOf(Game.UI.Controls.Control control)
    {
        for (Type? type = control.GetType(); type is not null; type = type.BaseType)
        {
            if (type.GetProperty("Items", bindingAttr: System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.DeclaredOnly) is { } property)
            {
                return property.GetValue(control);
            }
        }

        return null;
    }
}
