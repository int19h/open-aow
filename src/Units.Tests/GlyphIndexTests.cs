using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using Images = Aow.Units.ImageLib.ImageLib;
using G = Aow.Units.GFXE.GFXE;

namespace Aow.Units.Tests;

/// <summary>Skips with the resolved path when the installed fonts are absent.</summary>
public sealed class InstalledFontsFactAttribute : FactAttribute
{
    // The gate sets AOW_GAME_ROOT. The German language frames run from a copy of the same install,
    // so its fonts are the same files.
    internal static string Root => Path.GetFullPath(Environment.GetEnvironmentVariable("AOW_GAME_ROOT") is { Length: > 0 } root
        ? root : @"D:\Games\GOG\Age of Wonders");
    internal static string Fonts => Path.Combine(Root, "Int", "Fonts");

    public InstalledFontsFactAttribute()
    {
        if (!Directory.Exists(Fonts) || !Directory.EnumerateFiles(Fonts, "*.ILB").Any())
            Skip = $"Installed fonts are absent: {Fonts}. Set AOW_GAME_ROOT to the game installation.";
    }
}

public sealed class GlyphIndexTests
{
    // strings.md section 4: for every byte b, TextWidth and the glyph ShowText draws for the one-char
    // string of b equal glyph b, for every installed font.
    [InstalledFontsFact]
    public void EveryByteDrawsAndMeasuresItsOwnGlyphInEveryInstalledFont()
    {
        string[] files = Directory.EnumerateFiles(InstalledFontsFactAttribute.Fonts, "*.ILB")
            .Order(StringComparer.OrdinalIgnoreCase).ToArray();
        Assert.Equal(13, files.Length);
        var previousClasses = Images.LibraryImageClasses;
        var previousBase = Images.BaseImageLibraryPath;
        // Shadowed fonts draw through the blend tables that GFXE.Initialize builds at game start.
        var previousTables = (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable);
        (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        G.InitBlendTables(5, 31);
        Images.LibraryImageClasses = new TList();
        // The game keeps library names relative to the install, as FontModule does with Int\Fonts.
        Images.BaseImageLibraryPath = InstalledFontsFactAttribute.Root + Path.DirectorySeparatorChar;
        try
        {
            Images.RegisterLibraryImageClasses([
                LibraryImageClass.Of<TPicture08>(), LibraryImageClass.Of<TRLESprite08>(),
                LibraryImageClass.Of<TPicture16>(), LibraryImageClass.Of<TTransparentPicture16>(),
                LibraryImageClass.Of<TRLESprite16>(), LibraryImageClass.Of<TTransparentRLESprite16>(),
                LibraryImageClass.Of<TShadow>(), LibraryImageClass.Of<TImageNode>(), LibraryImageClass.Of<TSprite16>()]);
            int glyphs = 0;
            foreach (string file in files) glyphs += CheckFont(file);
            Assert.True(glyphs > 13 * 90, $"only {glyphs} glyphs drawn");
        }
        finally
        {
            Images.LibraryImageClasses.Destroy();
            Images.LibraryImageClasses = previousClasses;
            Images.BaseImageLibraryPath = previousBase;
            G.AlphaBlendTable.Free();
            G.IntensityBlendTable.Free();
            G.BrightenBlendTable.Free();
            G.ShadowBlendTable.Free();
            G.LinearAlphaBlendTable.Free();
            (G.AlphaBlendTable, G.IntensityBlendTable, G.BrightenBlendTable, G.ShadowBlendTable, G.LinearAlphaBlendTable) = previousTables;
        }
    }

    private static int CheckFont(string file)
    {
        var font = new TImageLibraryFont(null!);
        int drawn = 0;
        try
        {
            font.LoadFromFile(file);
            Assert.Equal(file, font.LoadedFileName.ToString());
            string name = Path.GetFileName(file);
            int width = 16, height = 16;
            for (int code = 0; code < 256; code++)
                if (font.GetSafe(code) is { } image)
                {
                    width = Math.Max(width, image.Width);
                    height = Math.Max(height, image.Height);
                }
            width += 64;
            height += 64;
            for (int code = 0; code < 256; code++)
            {
                AnsiString text = Cp1252.Text((byte)code);
                Assert.True(text.Length == 1 && text[1] == code, $"{name}: the one-char string of byte {code}");
                var glyph = font.GetSafe(code);
                Assert.True((glyph?.Width ?? 5) + font.CharacterDistance == font.TextWidth(text), $"{name}: TextWidth of byte {code}");
                byte[] shown = Draw(width, height, sd => font.ShowText(24, 24, text, sd));
                byte[] expected = Draw(width, height, sd => glyph?.Show(24, 24, sd));
                Assert.True(expected.AsSpan().SequenceEqual(shown), $"{name}: ShowText of byte {code} draws another glyph");
                if (glyph is not null && expected.Any(value => value != 0)) drawn++;
            }
        }
        finally
        {
            font.Destroy();
        }
        return drawn;
    }

    private static byte[] Draw(int width, int height, Action<TSurfaceDesc> draw)
    {
        var surface = new TDIB16Surface(width, height, G.pfRGB555);
        try
        {
            surface.Cls(0);
            TSurfaceDesc sd = default;
            surface.Lock(ref sd);
            try
            {
                draw(sd);
                return Runtime.BytePointer(sd.Data).AsSpan(Math.Abs(sd.Pitch) * height).ToArray();
            }
            finally { surface.Unlock(); }
        }
        finally { surface.Free(); }
    }
}
