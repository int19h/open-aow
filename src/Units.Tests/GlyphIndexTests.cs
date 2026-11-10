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
        var previousClasses = Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses;
        var previousBase = Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath;
        // Shadowed fonts draw through the blend tables that GFXE.Initialize builds at game start.
        var previousTables = (Aow.Game.Draw.GfxState.AlphaBlendTable, Aow.Game.Draw.GfxState.IntensityBlendTable, Aow.Game.Draw.GfxState.BrightenBlendTable, Aow.Game.Draw.GfxState.ShadowBlendTable, Aow.Game.Draw.GfxState.LinearAlphaBlendTable);
        (Aow.Game.Draw.GfxState.AlphaBlendTable, Aow.Game.Draw.GfxState.IntensityBlendTable, Aow.Game.Draw.GfxState.BrightenBlendTable, Aow.Game.Draw.GfxState.ShadowBlendTable, Aow.Game.Draw.GfxState.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        Aow.Game.Draw.GfxState.InitBlendTables(5, 31);
        Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses = new TList();
        // The game keeps library names relative to the install, as FontModule does with Int\Fonts.
        Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = InstalledFontsFactAttribute.Root + Path.DirectorySeparatorChar;
        try
        {
            Aow.Game.Draw.Images.ImageLibraries.RegisterLibraryImageClasses([
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
            Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses.Dispose();
            Aow.Game.Draw.Images.ImageLibraries.LibraryImageClasses = previousClasses;
            Aow.Game.Draw.Images.ImageLibraries.BaseImageLibraryPath = previousBase;
            Aow.Game.Draw.GfxState.AlphaBlendTable?.Dispose();
            Aow.Game.Draw.GfxState.IntensityBlendTable?.Dispose();
            Aow.Game.Draw.GfxState.BrightenBlendTable?.Dispose();
            Aow.Game.Draw.GfxState.ShadowBlendTable?.Dispose();
            Aow.Game.Draw.GfxState.LinearAlphaBlendTable?.Dispose();
            (Aow.Game.Draw.GfxState.AlphaBlendTable, Aow.Game.Draw.GfxState.IntensityBlendTable, Aow.Game.Draw.GfxState.BrightenBlendTable, Aow.Game.Draw.GfxState.ShadowBlendTable, Aow.Game.Draw.GfxState.LinearAlphaBlendTable) = previousTables;
        }
    }

    private static int CheckFont(string file)
    {
        var font = new TImageLibraryFont(null!);
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

    private static byte[] Draw(int width, int height, Action<TSurfaceDesc> draw)
    {
        using var surface = new TDIB16Surface(width, height, Aow.Game.Draw.PixelFormats.pfRGB555);
        surface.Cls(0);
        TSurfaceDesc sd = default;
        surface.Lock(ref sd);
        try
        {
            draw(sd);
            return sd.Data.AsSpan(sd.DataIndex, Math.Abs(sd.Pitch) * height).ToArray();
        }
        finally { surface.Unlock(); }
    }
}
