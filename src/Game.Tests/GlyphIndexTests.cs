using Aow.Game.Draw;
using Aow.Game.Draw.Images;

namespace Aow.Game.Tests;

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
        var previousTables = (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable);
        (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        GraphicsState.InitBlendTables(5, 31);
        ImageLibraries.LibraryImageClasses = new List<LibraryImageClass>();
        // The game keeps library names relative to the install, as UIFonts does with Int\Fonts.
        ImageLibraries.BaseImageLibraryPath = InstalledFontsFactAttribute.Root + Path.DirectorySeparatorChar;
        try
        {
            ImageLibraries.RegisterLibraryImageClasses([
                LibraryImageClass.Of<Picture08>(), LibraryImageClass.Of<RleSprite08>(),
                LibraryImageClass.Of<Picture16>(), LibraryImageClass.Of<TransparentPicture16>(),
                LibraryImageClass.Of<RleSprite16>(), LibraryImageClass.Of<TransparentRleSprite16>(),
                LibraryImageClass.Of<Shadow>(), LibraryImageClass.Of<ImageNode>(), LibraryImageClass.Of<Sprite16>()]);
            int glyphs = 0;
            foreach (string file in files) glyphs += CheckFont(file);
            Assert.True(glyphs > 13 * 90, $"only {glyphs} glyphs drawn");
        }
        finally
        {
            ImageLibraries.LibraryImageClasses.Clear();
            ImageLibraries.LibraryImageClasses = previousClasses;
            ImageLibraries.BaseImageLibraryPath = previousBase;
            GraphicsState.AlphaBlendTable?.Dispose();
            GraphicsState.IntensityBlendTable?.Dispose();
            GraphicsState.BrightenBlendTable?.Dispose();
            GraphicsState.ShadowBlendTable?.Dispose();
            GraphicsState.LinearAlphaBlendTable?.Dispose();
            (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable) = previousTables;
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

    private static byte[] Draw(int width, int height, Action<LockedSurface> draw)
    {
        using var surface = new Dib16Surface(width, height, PixelFormats.pfRGB555);
        surface.Cls(0);
        LockedSurface sd = default;
        surface.Lock(ref sd);
        try
        {
            draw(sd);
            return sd.Data.AsSpan(sd.DataIndex, Math.Abs(sd.Pitch) * height).ToArray();
        }
        finally { surface.Unlock(); }
    }
}
