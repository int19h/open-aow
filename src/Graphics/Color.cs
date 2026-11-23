namespace Aow.Graphics;

// A color: red, green and blue bytes. The packed int is 0x00BBGGRR, the layout of
// layout data, saves and the renderer, and ToPacked and FromPacked convert at that
// boundary and nowhere else. Persisted and streamed data converts through ToPacked
// and never stores the struct. The named value table below has two more copies, in
// the colors rewrite tool and in the forms generator, and all three stay in step.
public readonly record struct Color(byte R, byte G, byte B)
{
    public static Color White { get; } = new(255, 255, 255);

    public static Color Black { get; } = new(0, 0, 0);

    public static Color Red { get; } = new(255, 0, 0);

    public static Color Lime { get; } = new(0, 255, 0);

    public static Color Green { get; } = new(0, 128, 0);

    public static Color Maroon { get; } = new(128, 0, 0);

    public static Color Fuchsia { get; } = new(255, 0, 255);

    public static Color Purple { get; } = new(128, 0, 128);

    public static Color Gray { get; } = new(128, 128, 128);

    public static Color Silver { get; } = new(192, 192, 192);

    public int ToPacked() => R | (G << 8) | (B << 16);

    public static Color FromPacked(int value) => new((byte)value, (byte)(value >> 8), (byte)(value >> 16));
}
