namespace Aow.Rtl;

public enum EditCharCase : byte { Normal, UpperCase, LowerCase }

public enum MouseButton : byte { Left, Right, Middle }

public enum GridDrawState : byte { Selected, Focused, Fixed }

public enum ShiftState : byte { Shift, Alt, Ctrl, Left, Right, Middle, Double }

public enum PropertyAttribute : byte { ValueList, SubProperties, Dialog, MultiSelect, AutoUpdate, SortList, ReadOnly, Revertable }

public delegate void DrawCellEvent<in TSender>(TSender sender, int col, int row, Rect rect, Set<GridDrawState> state);
public delegate void SelectCellEvent<in TSender>(TSender sender, int col, int row, ref bool select);
public delegate void MouseEvent<in TSender>(TSender sender, MouseButton button, Set<ShiftState> shift, int x, int y);
public delegate void MouseMoveEvent<in TSender>(TSender sender, Set<ShiftState> shift, int x, int y);
public delegate void KeyPressEvent<in TSender>(TSender sender, ref byte key);
public delegate void KeyEvent<in TSender>(TSender sender, ref ushort key, Set<ShiftState> shift);

public enum BltMode : byte { None, Stretch, StretchToFit, Center }

public delegate void StreamProc(Stream stream);

public enum ConnectionType : byte { None, Unknown, TCP, IPX, Modem, Serial }

public enum StreamSoundMode : byte { Stop, Play, Stream }

public enum WindowState : byte { Normal, Minimized, Maximized }
