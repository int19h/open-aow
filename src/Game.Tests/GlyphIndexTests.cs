using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Graphics;

namespace Aow.Game.Tests;

public sealed class GlyphIndexTests
{
    // strings.md section 4: for every byte b, TextWidth and the glyph ShowText draws for the one-char
    // string of b equal glyph b, for every installed font.
    [InstalledFontsFact]
    public void EveryByteDrawsAndMeasuresItsOwnGlyphInEveryInstalledFont()
    {
        string[] files = [.. Directory.EnumerateFiles(InstalledFontsFactAttribute.Fonts, searchPattern: "*.ILB").Order(StringComparer.OrdinalIgnoreCase)];
        Assert.Equal(13, files.Length);
        List<LibraryImageClass> previousClasses = ImageLibraries.LibraryImageClasses;
        string previousBase = ImageLibraries.BaseImageLibraryPath;
        // Shadowed fonts draw through the blend tables that GFXE.Initialize builds at game start.
        (LookupBlendTableCollection100 AlphaBlendTable, LookupBlendTableCollection100 IntensityBlendTable, LookupBlendTableCollection100 BrightenBlendTable, LookupBlendTableCollection100 ShadowBlendTable, LookupBlendTableCollection100 LinearAlphaBlendTable) previousTables = (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable);
        (GraphicsState.AlphaBlendTable, GraphicsState.IntensityBlendTable, GraphicsState.BrightenBlendTable, GraphicsState.ShadowBlendTable, GraphicsState.LinearAlphaBlendTable) =
            (new(), new(), new(), new(), new());
        GraphicsState.InitializeBlendTables(5, count: 31);
        ImageLibraries.LibraryImageClasses = [];
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
            foreach (string file in files)
            {
                glyphs += CheckFont(file);
            }

            Assert.True(glyphs > 13 * 90, userMessage: $"only {glyphs} glyphs drawn");
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
        ImageLibraryFont font = new(null!);
        int drawn = 0;
        try
        {
            font.LoadFromFile(file);
            Assert.Equal(file, font.LoadedFileName);
            string name = Path.GetFileName(file);
            int width = 16, height = 16;
            for (int code = 0; code < 256; code++)
            {
                if (font.GetSafe(code) is { } image)
                {
                    width = Math.Max(width, image.Width);
                    height = Math.Max(height, image.Height);
                }
            }

            width += 64;
            height += 64;
            for (int code = 0; code < 256; code++)
            {
                string text = Cp1252.Text((byte)code);
                Assert.True(text.Length == 1 && Cp1252.Byte(text[0]) == code, userMessage: $"{name}: the one-char string of byte {code}");
                LibraryImage? glyph = font.GetSafe(code);
                Assert.True((glyph?.Width ?? 5) + font.CharacterDistance == font.TextWidth(text), userMessage: $"{name}: TextWidth of byte {code}");
                byte[] shown = Draw(width, height, lockedSurface => font.ShowText(24, 24, text, lockedSurface));
                byte[] expected = Draw(width, height, lockedSurface => glyph?.Show(24, 24, lockedSurface));
                Assert.True(expected.AsSpan().SequenceEqual(shown), userMessage: $"{name}: ShowText of byte {code} draws another glyph");
                if (glyph is not null && expected.Any(value => value != 0))
                {
                    drawn++;
                }
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
        Dib16Surface surface = new(width, height, pixelFormatId: PixelFormat.Rgb555);
        _ = surface.Clear(Color.Black);
        LockedSurface lockedSurface = default;
        _ = surface.Lock(ref lockedSurface);
        try
        {
            draw(lockedSurface);
            return lockedSurface.Data.AsSpan(lockedSurface.DataIndex, length: Math.Abs(lockedSurface.Pitch) * height).ToArray();
        }
        finally
        {
            _ = surface.Unlock();
        }
    }
}
