using Aow.Game.Draw;
using Aow.Game.Draw.Images;

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
        var previousClasses = ImageLibraries.LibraryImageClasses;
        var previousBase = ImageLibraries.BaseImageLibraryPath;
        // Shadowed fonts draw through the blend tables that GFXE.Initialize builds at game start.
        var previousTables = (GfxState.AlphaBlendTable, GfxState.IntensityBlendTable, GfxState.BrightenBlendTable, GfxState.ShadowBlendTable, GfxState.LinearAlphaBlendTable);
        (GfxState.AlphaBlendTable, GfxState.IntensityBlendTable, GfxState.BrightenBlendTable, GfxState.ShadowBlendTable, GfxState.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        Aow.Game.Draw.GfxState.InitBlendTables(5, 31);
        ImageLibraries.LibraryImageClasses = new DelphiList();
        // The game keeps library names relative to the install, as FontModule does with Int\Fonts.
        ImageLibraries.BaseImageLibraryPath = InstalledFontsFactAttribute.Root + Path.DirectorySeparatorChar;
        try
        {
            Aow.Game.Draw.Images.ImageLibraries.RegisterLibraryImageClasses([
                LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RLESprite08>(),
                LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(),
                LibraryImageClass.Of<RLESprite16>(), LibraryImageClass.Of<TransparentRLESprite16>(),
                LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
            int glyphs = 0;
            foreach (string file in files) glyphs += CheckFont(file);
            Assert.True(glyphs > 13 * 90, $"only {glyphs} glyphs drawn");
        }
        finally
        {
            Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses.Dispose();
            ImageLibraries.LibraryImageClasses = previousClasses;
            ImageLibraries.BaseImageLibraryPath = previousBase;
            GfxState.AlphaBlendTable?.Dispose();
            GfxState.IntensityBlendTable?.Dispose();
            GfxState.BrightenBlendTable?.Dispose();
            GfxState.ShadowBlendTable?.Dispose();
            GfxState.LinearAlphaBlendTable?.Dispose();
            (GfxState.AlphaBlendTable, GfxState.IntensityBlendTable, GfxState.BrightenBlendTable, GfxState.ShadowBlendTable, GfxState.LinearAlphaBlendTable) = previousTables;
        }
    }

    private static int CheckFont(string file)
    {
        var font = new ImageLibraryFont(null!);
        int drawn = 0;
        try
        {
            font.LoadFromFile(file);
            Assert.Equal(file, font.LoadedFileName);
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
                string text = Cp1252.Text((byte)code);
                Assert.True(text.Length == 1 && Cp1252.Byte(text[0]) == code, $"{name}: the one-char string of byte {code}");
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
            font.Dispose();
        }
        return drawn;
    }

    private static byte[] Draw(int width, int height, Action<SurfaceDesc> draw)
    {
        using var surface = new DIB16Surface(width, height, PixelFormats.pfRGB555);
        surface.Cls(0);
        SurfaceDesc sd = default;
        surface.Lock(ref sd);
        try
        {
            draw(sd);
            return sd.Data.AsSpan(sd.DataIndex, Math.Abs(sd.Pitch) * height).ToArray();
        }
        finally { surface.Unlock(); }
    }
}
