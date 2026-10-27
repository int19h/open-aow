using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Aow.Generated")]

namespace Aow.Rtl;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TRGBQuad
{
    public byte rgbBlue, rgbGreen, rgbRed, rgbReserved;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TBitmapInfoHeader
{
    public int biSize, biWidth, biHeight;
    public ushort biPlanes, biBitCount;
    public int biCompression, biSizeImage, biXPelsPerMeter, biYPelsPerMeter, biClrUsed, biClrImportant;
}

public struct TBitmapInfo
{
    public TBitmapInfoHeader bmiHeader;
    public TRGBQuad[] bmiColors;
}

public struct TGUID
{
    public int D1;
    public ushort D2, D3;
    public byte[] D4;
    public static implicit operator TGUID(AnsiString value)
    {
        var bytes = Guid.Parse(value.ToString()).ToByteArray();
        return new TGUID { D1 = System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes), D2 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(4)), D3 = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.AsSpan(6)), D4 = bytes[8..] };
    }
}

[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct WordRec
{
    [FieldOffset(0)] public byte Lo;
    [FieldOffset(1)] public byte Hi;
}
[StructLayout(LayoutKind.Explicit, Size = 4)]
public struct LongRec
{
    [FieldOffset(0)] public ushort Lo;
    [FieldOffset(2)] public ushort Hi;
}

public class IUnknown : TObject { }
public struct TMessage { public int Msg, WParam, LParam, Result; }
public struct TWMKey { public int Msg; public ushort CharCode, Unused; public int KeyData, Result; }
public struct TCreateParams { public int Style, ExStyle, X, Y, Width, Height, WndParent, Param; }
public enum TEditCharCase : byte { ecNormal, ecUpperCase, ecLowerCase }
public enum TMouseButton : byte { mbLeft, mbRight, mbMiddle }
public class TSFXManager : TComponent { }
public class TAbstractAoWHSMap : TObject { }
public class TPageTurnControl : TObject { }
public enum TPageTurnMode : byte { ptLeft, ptRight, ptOpen, ptClose }
public enum TGridDrawState : byte { gdSelected, gdFocused, gdFixed }
public enum TShiftState : byte { ssShift, ssAlt, ssCtrl, ssLeft, ssRight, ssMiddle, ssDouble }
public enum TPropertyAttribute : byte { paValueList, paSubProperties, paDialog, paMultiSelect, paAutoUpdate, paSortList, paReadOnly, paRevertable }
public delegate void TDrawCellEvent(TObject sender, int col, int row, TRect rect, TSet<TGridDrawState> state);
public delegate void TSelectCellEvent(TObject sender, int col, int row, ref bool select);
public delegate void TMouseEvent(TObject sender, TMouseButton button, TSet<TShiftState> shift, int x, int y);
public delegate void TMouseMoveEvent(TObject sender, TSet<TShiftState> shift, int x, int y);
public delegate void TKeyPressEvent(TObject sender, ref byte key);
public delegate void TKeyEvent(TObject sender, ref ushort key, TSet<TShiftState> shift);

// These VCL controls retain component identity. Native drawing awaits the platform layer.
public class TControl : TComponent
{
    public int Width, Height, Left, Top, Color;
    public int Cursor { get; set; }
    protected internal void SetCursor(int value) => Cursor = value;
    public void Hide() => throw new NotImplementedException("VCL.Hide");
    public void Show() => throw new NotImplementedException("VCL.Show");
    public TControl? Parent;
    public virtual void Invalidate() => throw new NotImplementedException("VCL.Invalidate");
    protected internal virtual void Paint() => throw new NotImplementedException("VCL.Paint");
    protected internal virtual void KeyDown(ref ushort key, TSet<TShiftState> shift) { }
    protected internal virtual void KeyUp(ref ushort key, TSet<TShiftState> shift) { }
    protected internal virtual void Click() { }
    protected internal virtual int GetPalette() => 0;
    protected internal virtual void WndProc(ref TMessage message) => throw new NotImplementedException("VCL.WndProc");
}
public class TCustomPanel : TWinControl { }
public class TWinControl : TControl
{
    protected internal virtual void CreateParams(ref TCreateParams parameters) => throw new NotImplementedException("VCL.CreateParams");
    protected internal virtual void CreateWnd() => throw new NotImplementedException("VCL.CreateWnd");
    protected internal virtual void DestroyWnd() => throw new NotImplementedException("VCL.DestroyWnd");
    protected internal virtual void MouseDown(TMouseButton button, TSet<TShiftState> shift, int x, int y) { }
    protected internal virtual void MouseUp(TMouseButton button, TSet<TShiftState> shift, int x, int y) { }
    protected internal virtual void MouseMove(TSet<TShiftState> shift, int x, int y) { }
    protected internal virtual void KeyPress(ref byte key) { }
    protected internal virtual void DblClick() { }
}
public class TThread : TObject
{
    protected internal virtual void Execute() => throw new NotImplementedException("VCL.Thread.Execute");
}
public class TForm : TWinControl { }
public class TButton : TWinControl { }
public class TPanel : TCustomPanel { }
public class TMemo : TWinControl { }
public class TImage : TControl { }
public class TLabel : TControl { }
public class TBevel : TControl { }
public class TTabControl : TWinControl
{
    protected internal virtual void Change() { }
}
public class TRegistry : TObject
{
    private readonly ISettingsStore values = Rtl.Settings ?? new MemorySettingsStore();
    private string key = "";
    public bool OpenKey(AnsiString name, bool create) { key = name.ToString(); return true; }
    public void CloseKey() => key = "";
    public bool ValueExists(AnsiString name) => values.Read(key + "\\" + name.ToString()) is not null;
    public int ReadInteger(AnsiString name) => (int)values.Read(key + "\\" + name.ToString())!;
    public bool ReadBool(AnsiString name) => (bool)values.Read(key + "\\" + name.ToString())!;
    public AnsiString ReadString(AnsiString name) => (AnsiString)values.Read(key + "\\" + name.ToString())!;
    public void WriteInteger(AnsiString name, int value) => values.Write(key + "\\" + name.ToString(), value);
    public void WriteBool(AnsiString name, bool value) => values.Write(key + "\\" + name.ToString(), value);
    public void WriteString(AnsiString name, AnsiString value) => values.Write(key + "\\" + name.ToString(), value);
    public int ReadBinaryData(AnsiString name, Span<byte> buffer, int size)
    {
        var bytes = (byte[])values.Read(key + "\\" + name.ToString())!;
        int count = Math.Min(size, bytes.Length);
        bytes.AsSpan(0, count).CopyTo(buffer);
        return count;
    }
    public void WriteBinaryData(AnsiString name, ReadOnlySpan<byte> buffer, int size) => values.Write(key + "\\" + name.ToString(), buffer[..size].ToArray());
}
public class TIvTranslator : TIvCustomTranslator { }
public partial class TIvDictionary : TComponent
{
    public AnsiString FFileName;
    public int FLanguage;
    public void SetLanguage(int language) => SelectLanguage(language);
    public void SetDictionaryName(AnsiString name) => NameDictionary(name);
}
public class TIvBinaryDictionary : TIvDictionary { }
public class Text : TObject { }
public class TDataModule : TComponent { }
public class TMainForm : TForm { }
public class TVideoPlayer : TComponent { }
public class TGDPlay : TComponent
{
    public TGUID AppGUID { get; set; }
    public bool Connected => false;
    public object? DPlay => null;
    public void CheckMessages() { }
    public void GetConnections(TStrings connections) => connections.Clear();
}
public class TGDFile : TObject { }
public class TGDMessage : TObject { }
public enum TConnectionType : byte { conNone, conUnknown, conTCP, conIPX, conModem, conSerial }
public enum TStreamSoundMode : byte { ssmStop, ssmPlay, ssmStream }
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TPaletteEntry { public byte peRed, peGreen, peBlue, peFlags; }
public enum TWindowState : byte { wsNormal, wsMinimized, wsMaximized }
public class TCustomGrid : TWinControl
{
    public int ColCount, RowCount;
    public int Col, Row, DefaultColWidth, DefaultRowHeight;
    public TScrollStyle ScrollBars;
    protected internal void SetColCount(int value) => ColCount = value;
    protected internal void SetRowCount(int value) => RowCount = value;
    protected internal virtual void DrawCell(int col, int row, TRect rect, TSet<TGridDrawState> state) => throw new NotImplementedException("VCL.DrawCell");
    protected internal virtual bool SelectCell(int col, int row) => true;
}
public enum TScrollStyle : byte { ssNone, ssHorizontal, ssVertical, ssBoth }
public class TComboBox : TWinControl
{
    protected internal virtual void Change() { }
    protected internal virtual void DropDown() { }
}
public class TListBox : TWinControl
{
}
public class TOpenDialog : TComponent
{
    public TOpenDialog() { }
    public TOpenDialog(TComponent owner) : base(owner) { }
    public AnsiString Filter, FileName, DefaultExt;
    public virtual bool Execute() => throw new NotImplementedException("VCL.OpenDialog");
}
public class TStringProperty : TObject
{
    public virtual void Edit() => throw new NotImplementedException("VCL.PropertyEditor");
    public virtual TSet<TPropertyAttribute> GetAttributes() => default;
    public virtual AnsiString GetValue() => throw new NotImplementedException("VCL.PropertyEditor");
}
public class TIniFile : TObject { }
public class TFiler : TObject { }
public class TIToolServices : TObject
{
    public virtual AnsiString GetProjectName() => throw new NotImplementedException("Delphi IDE project service");
}

public class THandleStream : TStream
{
    public int Handle { get; protected set; }
    public void Create__init(int handle) => Handle = handle;
    public override int Read(Span<byte> buffer) => throw new NotImplementedException("HandleStream.Read");
    public override int Write(ReadOnlySpan<byte> buffer) => throw new NotImplementedException("HandleStream.Write");
    public override int Seek(int offset, int origin) => throw new NotImplementedException("HandleStream.Seek");
}

public static partial class Rtl
{
    public static TIToolServices? ToolServices => null;
    public static TRect Rect(int left, int top, int right, int bottom) => new() { Left = left, Top = top, Right = right, Bottom = bottom };
    public static Ptr<byte> BytePointer(object? value) => value is null ? default : value is IPointerBox pointer ? pointer.As<byte>() : throw new EInvalidCast("Value is not a pointer");
    public static Ptr<byte> AlignPointer(Ptr<byte> value, int mask) => value + ((value.ByteIndex & mask) - value.ByteIndex);
    public static bool Succeeded(int result) => result >= 0;
    public static bool Failed(int result) => result < 0;
    public static bool QueryPerformanceFrequency(ref long frequency) { frequency = System.Diagnostics.Stopwatch.Frequency; return true; }
    public static void CoUninitialize() { }
    public static void RegisterClasses(Type[] types) { foreach (var type in types) RegisterClass(type); }
    public static AnsiString ExtractRelativePath(AnsiString baseName, AnsiString destination)
        => Files.ExtractRelativePath(baseName.ToString(), destination.ToString());
    public static bool EqualRect(in TRect left, in TRect right) => left.Left == right.Left && left.Top == right.Top && left.Right == right.Right && left.Bottom == right.Bottom;
    public static bool IntersectRect(ref TRect result, in TRect left, in TRect right)
    {
        var intersection = new TRect { Left = Math.Max(left.Left, right.Left), Top = Math.Max(left.Top, right.Top), Right = Math.Min(left.Right, right.Right), Bottom = Math.Min(left.Bottom, right.Bottom) };
        bool nonempty = intersection.Right > intersection.Left && intersection.Bottom > intersection.Top;
        result = nonempty ? intersection : default;
        return nonempty;
    }
    public static bool UnionRect(ref TRect result, in TRect left, in TRect right)
    {
        bool a = left.Right > left.Left && left.Bottom > left.Top, b = right.Right > right.Left && right.Bottom > right.Top;
        result = !a ? b ? right : default : !b ? left : new TRect { Left = Math.Min(left.Left, right.Left), Top = Math.Min(left.Top, right.Top), Right = Math.Max(left.Right, right.Right), Bottom = Math.Max(left.Bottom, right.Bottom) };
        return a || b;
    }
}
