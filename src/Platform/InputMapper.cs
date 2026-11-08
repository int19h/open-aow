using Aow.Rtl;

namespace Aow.Platform;

/// <summary>SDL keycodes and USB scancodes translated to the VCL event vocabulary.</summary>
public sealed class InputMapper
{
    private int modifiers;
    private uint buttons;
    private bool inside;
    private (byte Button, ulong Time, float X, float Y)? firstPress;
    private byte doubleButton;
    private float wheelY;

    public void Reset()
    {
        modifiers = 0;
        buttons = 0;
        firstPress = null;
        doubleButton = 0;
        inside = false;
        wheelY = 0;
    }

    public void SetModifiers(int value) => modifiers = value;

    private TSet<TShiftState> Shift(bool twice = false)
    {
        TSet<TShiftState> state = default;
        if ((modifiers & 3) != 0) state = state.With(TShiftState.ssShift);
        if ((modifiers & 192) != 0) state = state.With(TShiftState.ssCtrl);
        if ((modifiers & 768) != 0) state = state.With(TShiftState.ssAlt);
        if ((buttons & 1) != 0) state = state.With(TShiftState.ssLeft);
        if ((buttons & 4) != 0) state = state.With(TShiftState.ssRight);
        return twice ? state.With(TShiftState.ssDouble) : state;
    }

    public void Motion(float x, float y, uint state, DisplayViewport viewport, IDisplayEvents sink)
    {
        buttons = state & 5;
        var point = viewport.Map(x, y);
        if (point.Inside) sink.MouseMove(Shift(), point.X, point.Y);
        else if (inside) sink.MouseMove(default, -1, -1);
        inside = point.Inside;
    }

    public void Leave(IDisplayEvents sink)
    {
        if (inside) sink.MouseMove(default, -1, -1);
        inside = false;
    }

    // direction 1 is SDL_MOUSEWHEEL_FLIPPED. Positive y, after that correction, is away from the user.
    // Fractional y accumulates until it makes a whole step. A roll whose sign differs from the
    // remainder clears the remainder first. A point outside the output is ignored and does not add
    // to the remainder.
    public void Wheel(float y, int direction, float x, float yPosition, DisplayViewport viewport, IDisplayEvents sink)
    {
        if (direction == 1) y = -y;
        var point = viewport.Map(x, yPosition);
        if (!point.Inside) return;
        if (y != 0 && wheelY != 0 && Math.Sign(y) != Math.Sign(wheelY)) wheelY = 0;
        wheelY += y;
        int steps = (int)Math.Truncate(wheelY);
        wheelY -= steps;
        if (steps != 0) sink.MouseWheel(steps, Shift(), point.X, point.Y);
    }

    public void Button(byte button, bool down, ulong milliseconds, float x, float y, DisplayViewport viewport, IDisplayEvents sink)
    {
        if (button is not (1 or 3)) return;
        uint mask = 1u << (button - 1);
        buttons = down ? buttons | mask : buttons & ~mask;
        var point = viewport.Map(x, y);
        var mapped = button == 1 ? TMouseButton.mbLeft : TMouseButton.mbRight;
        if (!down)
        {
            sink.MouseUp(mapped, Shift(doubleButton == button), point.X, point.Y);
            if (doubleButton == button) doubleButton = 0;
            return;
        }
        bool twice = point.Inside && firstPress is { } first && first.Button == button && milliseconds >= first.Time && milliseconds - first.Time <= 500
            && Math.Abs(x - first.X) <= 4 && Math.Abs(y - first.Y) <= 4;
        firstPress = point.Inside && !twice ? (button, milliseconds, x, y) : null;
        doubleButton = twice ? button : (byte)0;
        if (twice) sink.DoubleClick();
        sink.MouseDown(mapped, Shift(twice), point.X, point.Y);
    }

    public void Key(int keycode, int scancode, int keyModifiers, bool down, IDisplayEvents sink)
    {
        modifiers = keyModifiers;
        ushort key = VirtualKey(keycode, scancode);
        if (key == 0) return;
        if (!down) { sink.KeyUp(key, Shift()); return; }
        sink.KeyDown(key, Shift());
        byte character = key switch { 13 => 13, 8 => 8, 27 => 27, 9 => 9, _ => 0 };
        if (character != 0) sink.KeyPress(character);
    }

    public static void Text(string text, IDisplayEvents sink)
    {
        // Only a char with a code page 1252 byte reaches the game, as that byte (strings.md 2.3).
        foreach (char c in text)
            if (Cp1252.IsMapped(c) && Cp1252.Byte(c) >= 32) sink.KeyPress(Cp1252.Byte(c));
    }

    public static ushort VirtualKey(int keycode, int scancode)
    {
        if (keycode is >= 'a' and <= 'z') return (ushort)(keycode - 32);
        if (keycode is >= 'A' and <= 'Z' or >= '0' and <= '9') return (ushort)keycode;
        // SDL scancodes use the USB HID keyboard usage page, independent of layout.
        if (scancode is >= 58 and <= 69) return (ushort)(0x70 + scancode - 58);
        if (scancode is >= 104 and <= 115) return (ushort)(0x7C + scancode - 104);
        if (scancode is >= 89 and <= 97) return (ushort)(0x61 + scancode - 89);
        return scancode switch
        {
            40 or 88 => 0x0D,
            41 => 0x1B,
            42 => 0x08,
            43 => 0x09,
            44 => 0x20,
            45 => 0xBD,
            46 => 0xBB,
            47 => 0xDB,
            48 => 0xDD,
            49 or 50 => 0xDC,
            51 => 0xBA,
            52 => 0xDE,
            53 => 0xC0,
            54 => 0xBC,
            55 => 0xBE,
            56 => 0xBF,
            57 => 0x14,
            70 => 0x2C,
            71 => 0x91,
            72 => 0x13,
            73 => 0x2D,
            74 => 0x24,
            75 => 0x21,
            76 => 0x2E,
            77 => 0x23,
            78 => 0x22,
            79 => 0x27,
            80 => 0x25,
            81 => 0x28,
            82 => 0x26,
            83 => 0x90,
            84 => 0x6F,
            85 => 0x6A,
            86 => 0x6D,
            87 => 0x6B,
            98 => 0x60,
            99 => 0x6E,
            100 or 135 => 0xE2,
            101 or 118 => 0x5D,
            103 or 134 => 0x92,
            116 => 0x2B,
            117 => 0x2F,
            119 => 0x29,
            127 => 0xAD,
            128 => 0xAF,
            129 => 0xAE,
            133 or 159 => 0x6C,
            136 or 144 or 146 => 0x15,
            137 => 0xDC,
            138 => 0x1C,
            139 => 0x1D,
            145 => 0x19,
            147 or 148 => 0xF0,
            153 => 0xF9,
            154 => 0x2C,
            155 => 0x03,
            156 or 162 or 216 => 0x0C,
            157 => 0x21,
            158 => 0x0D,
            163 => 0xF7,
            164 => 0xF8,
            186 => 0x09,
            187 => 0x08,
            224 or 228 => 0x11,
            225 or 229 => 0x10,
            226 or 230 => 0x12,
            227 => 0x5B,
            231 => 0x5C,
            258 => 0x5F,
            262 or 263 or 271 => 0xB3,
            267 => 0xB0,
            268 => 0xB1,
            269 => 0xB2,
            272 => 0xB5,
            280 => 0xAA,
            281 => 0xAC,
            282 => 0xA6,
            283 => 0xA7,
            284 => 0xA9,
            285 => 0xA8,
            286 => 0xAB,
            _ => 0
        };
    }
}
