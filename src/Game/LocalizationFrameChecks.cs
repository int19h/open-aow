using System.Text.Json;
using Aow.Layout;
using Aow.Rtl;
using Aow.Rtl.Localize;
using Aow.Units.ImageLib;
using Aow.Units.GFXE;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using Aow.Units.AoWSWindow;
using Aow.Units.AoWFileDialog;

namespace Aow.Game;

internal static class LocalizationFrameChecks
{
    public static void Run(string root, string output, LayoutSession layouts)
    {
        int language = LocalizationRuntime.ReadLanguage(RtlUnit.Settings);
        var germanBytes = new SortedSet<byte>();
        foreach (string file in new[] { "AoW", "ResStr", "CustMaps", "Release" })
        {
            var dictionary = new MldDictionary();
            dictionary.Open(Path.Combine(root, "Dict", file + ".mld"));
            foreach (var entry in dictionary.Entries)
                for (int index = 1; index <= entry.Translations[1].Length; index++)
                {
                    byte code = Cp1252.Byte(entry.Translations[1][index - 1]);
                    if (code >= 128) germanBytes.Add(code);
                }
            dictionary.Close();
        }
        var coverage = new List<object>();
        var fonts = new[] { FontModule.Age8, FontModule.Age10, FontModule.Age16s,
            FontModule.Age10s, FontModule.Age8s, FontModule.Sessis14G, FontModule.AoW15WhiteGrey,
            FontModule.AoW15BlackBrown, FontModule.Age10LB, FontModule.Age10b, FontModule.Age10bs };
        foreach (var font in fonts)
        {
            byte[] missing = germanBytes.Where(code => !font.ImageExist(code) || font[code].Width <= 0 || font[code].Height <= 0).ToArray();
            coverage.Add(new { font = font.Name, missing = missing.Select(b => (int)b).ToArray() });
        }
        string undead = TranslateRStr("Undead(plural)");
        bool undeadEqual = language != 1 || undead == "Undead";
        string mapCaption = AoWSWindowUnit.SWindow.STitleLabel.Text;
        string expectedMapCaption = language == 2 ? "Weltkarte" : "World Map";
        if (mapCaption != expectedMapCaption) throw new InvalidOperationException("The startup form translation was not applied");
        // The file dialog title holds the one German text with a byte from 128 to 159 (ResStr.mld,
        // "Spiel speichern unter" and the ellipsis 0x85), shown by the languages-de-save fixture.
        var dialogTitle = AoWFileDialogUnit.FileDlgWindow?.Title?.Text ?? "";
        var required = new SortedSet<string>(StringComparer.Ordinal);
        // A byte that no installed font draws, such as the ellipsis 0x85, leaves the original's
        // 5 pixel gap. The frame comparison checks that gap, so it is recorded, not a missing glyph.
        var absentEverywhere = Enumerable.Range(128, 128)
            .Where(code => fonts.All(font => !font.ImageExist(code) || font[code].Width <= 0 || font[code].Height <= 0)).ToHashSet();
        var absent = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var module in layouts.Modules.Values)
            for (int index = 0; index < module.ComponentCount; index++)
            {
                if (module.Components[index] is not Aow.Units.AoWControl.TAOWControl control ||
                    !control.Visible || control.FAOWWindow is not { Visible: true } || control.FAOWFont is not { } font) continue;
                var texts = new List<string> { control.FText };
                if (control.GetType().GetProperty("Items")?.GetValue(control) is TStrings items)
                    for (int item = 0; item < items.Count; item++) texts.Add(items[item]);
                foreach (var text in texts)
                {
                    string cased = font.CharCase switch
                    {
                        TEditCharCase.ecUpperCase => RtlUnit.UpperCase(text),
                        TEditCharCase.ecLowerCase => RtlUnit.LowerCase(text),
                        _ => text
                    };
                    for (int character = 1; character <= cased.Length; character++)
                    {
                        byte code = Cp1252.Byte(cased[character - 1]);
                        if (code < 128) continue;
                        bool missing = !font.ImageExist(code) || font[code].Width <= 0 || font[code].Height <= 0;
                        if (missing && absentEverywhere.Contains(code))
                        {
                            absent.Add($"{font.Name}:{code}");
                            continue;
                        }
                        required.Add($"{font.Name}:{code}");
                        if (missing)
                            throw new InvalidOperationException($"Missing visible fixture glyph {code} in {font.Name}, component {control.Name}");
                    }
                }
            }
        File.WriteAllText(Path.Combine(output, "localization.json"), JsonSerializer.Serialize(new
        {
            language, undead, undead_equal = undeadEqual, map_caption = mapCaption,
            file_dialog_title = dialogTitle, file_dialog_title_bytes = Cp1252.Encode(dialogTitle).ToArray().Select(b => (int)b).ToArray(),
            fixture_glyphs_equal = true, required_glyphs = required, absent_glyphs = absent, german_bytes = germanBytes, coverage
        }, new JsonSerializerOptions { WriteIndented = true }) + "\n");
        if (!undeadEqual) throw new InvalidOperationException("English Undead(plural) did not translate to Undead");
        if (language == 1)
        {
            var left = new TDIB16Surface(120, 40, GFXEUnit.pfRGB555);
            var right = new TDIB16Surface(120, 40, GFXEUnit.pfRGB555);
            try
            {
                left.Cls(0);
                right.Cls(0);
                Aow.Units.GFXE.TSurfaceDesc l = default, r = default;
                left.Lock(ref l);
                right.Lock(ref r);
                try
                {
                    FontModule.Age8.SetText(0, 0, undead, l);
                    FontModule.Age8.SetText(0, 0, "Undead", r);
                    var pixels = RtlUnit.BytePointer(l.Data).AsSpan(l.Pitch * 40);
                    if (!pixels.SequenceEqual(RtlUnit.BytePointer(r.Data).AsSpan(r.Pitch * 40)) || !pixels.ToArray().Any(b => b != 0))
                        throw new InvalidOperationException("English Undead rendering differs");
                }
                finally { left.Unlock(); right.Unlock(); }
            }
            finally { left.Free(); right.Free(); }
        }
        Console.WriteLine($"Localization frames: language {language}, Undead(plural) = {undead}, {germanBytes.Count} German extended bytes scanned across {fonts.Length} fonts");
    }
}
