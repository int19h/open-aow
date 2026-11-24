namespace Aow.Rtl;

public enum EditCharCase : byte { Normal, UpperCase, LowerCase }

public enum MouseButton : byte { Left, Right, Middle }

[Flags]
public enum ShiftStates : byte
{
    None = 0,
    Shift = 1 << 0,
    Alt = 1 << 1,
    Ctrl = 1 << 2,
    Left = 1 << 3,
    Right = 1 << 4,
    Middle = 1 << 5,
    Double = 1 << 6,
}

public delegate void MouseEvent<in TSender>(TSender sender, MouseButton button, ShiftStates shift, int x, int y);
public delegate void MouseMoveEvent<in TSender>(TSender sender, ShiftStates shift, int x, int y);
public delegate void KeyPressEvent<in TSender>(TSender sender, ref byte key);
public delegate void KeyEvent<in TSender>(TSender sender, ref ushort key, ShiftStates shift);

public enum ConnectionType : byte { None, Unknown, TCP, IPX, Modem, Serial }

public enum WindowState : byte { Normal, Minimized, Maximized }
