using System.Runtime.InteropServices;

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

    public static implicit operator TGUID(string value)
    {
        var bytes = Guid.Parse(value).ToByteArray();
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
    public TControl(TComponent? owner = null) : base(owner) { }

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

public class TCustomPanel : TWinControl
{
    public TCustomPanel(TComponent? owner = null) : base(owner) { }
}

public class TWinControl : TControl
{
    public TWinControl(TComponent? owner = null) : base(owner) { }

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
    public TTabControl(TComponent? owner = null) : base(owner) { }

    protected internal virtual void Change() { }
}

public class TRegistry : TObject
{
    private readonly ISettingsStore values = RtlUnit.Settings ?? new MemorySettingsStore();
    private string key = "";

    public bool OpenKey(string name, bool create) { key = name; return true; }

    public void CloseKey() => key = "";
    public bool ValueExists(string name) => values.Read(key + "\\" + name) is not null;
    public int ReadInteger(string name) => (int)values.Read(key + "\\" + name)!;
    public bool ReadBool(string name) => (bool)values.Read(key + "\\" + name)!;
    // REGISTRY.PAS TRegistry.ReadString returns '' for a missing value (strings.md 2.2).
    public string ReadString(string name) => (string?)values.Read(key + "\\" + name) ?? "";
    public void WriteInteger(string name, int value) => values.Write(key + "\\" + name, value);
    public void WriteBool(string name, bool value) => values.Write(key + "\\" + name, value);
    public void WriteString(string name, string value) => values.Write(key + "\\" + name, value);

    public int ReadBinaryData(string name, Span<byte> buffer, int size)
    {
        var bytes = (byte[])values.Read(key + "\\" + name)!;
        int count = Math.Min(size, bytes.Length);
        bytes.AsSpan(0, count).CopyTo(buffer);
        return count;
    }

    public void WriteBinaryData(string name, ReadOnlySpan<byte> buffer, int size) => values.Write(key + "\\" + name, buffer[..size].ToArray());
}

public class TIvTranslator : TIvCustomTranslator
{
    public TIvTranslator(TComponent? owner = null) : base(owner) { }
}

public partial class TIvDictionary : TComponent
{
    public string FFileName = "";
    public int FLanguage;

    public void SetLanguage(int language) => SelectLanguage(language);
    public void SetDictionaryName(string name) => NameDictionary(name);
}

public class TIvBinaryDictionary : TIvDictionary { }

public class Text : TObject { }

public class TDataModule : TComponent
{
    public TDataModule(TComponent? owner) : base(owner) { }
}

public class TMainForm : TForm { }

public class TVideoPlayer : TComponent
{
    public TVideoPlayer(TComponent? owner = null) : base(owner) { }
}

// The offline DirectPlay component (aowDPlayComp.pas). It lists the TCP/IP provider that
// DirectPlay lists on any current Windows, but it never connects and never joins a session, so
// the original's guards on Connected and InSession take their offline branch.
public class TGDPlay : TComponent
{
    public TGDPlay(TComponent? owner = null) : base(owner) { }

    // DPlay.pas: the TCP/IP service provider.
    public static readonly TGUID DPSPGUID_TCPIP = "36E95EE0-8577-11CF-960C-0080C7534E82";
    public const int E_FAIL = unchecked((int)0x80004005);

    public TGUID AppGUID { get; set; }
    public bool Connected => false;
    public bool InSession => false;
    public TConnectionType ConnectionType => TConnectionType.conNone;
    public object? DPlay => null;
    public int PlayerID => 0;
    public string SessionName { get; set; } = "";
    // SetPlayerName renames the session player only when connected and in a session.
    public string PlayerName { get; set; } = "";
    public string TCPPort { get; set; } = "";
    // The service providers found by GetConnections: their names and their TGDConnection items.
    public TStringList Connections { get; } = new();
    public TList ConnectionItems { get; } = new();

    public void CheckMessages() { }

    // The original's EnumConnections callback adds the name and the item of each provider.
    public int GetConnections()
    {
        Connections.Clear();
        ConnectionItems.Clear();
        Connections.Add("Internet TCP/IP Connection For DirectPlay");
        ConnectionItems.Add(new TGDConnection { FGuid = DPSPGUID_TCPIP, FType = TConnectionType.conTCP });
        return 0;
    }

    // The offline component has no DirectPlay object to initialize the provider with.
    public int Connect(int index) => E_FAIL;
    // The original fails without a connection.
    public int CreateSession() => E_FAIL;

    // The original sets its close flag only in a session.
    public void CloseSession() { }

    // The original sets its disconnect flag only when connected.
    public void DisConnect() { }
}

// aowDPlayComp.pas: a service provider found by EnumConnections.
public class TGDConnection : TObject
{
    public TGUID FGuid;
    public TConnectionType FType;
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
    public TCustomGrid(TComponent? owner = null) : base(owner) { }

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
    public TComboBox(TComponent? owner = null) : base(owner) { }

    protected internal virtual void Change() { }

    protected internal virtual void DropDown() { }
}

public class TListBox : TWinControl
{
    public TListBox(TComponent? owner = null) : base(owner) { }

}

public class TOpenDialog : TComponent
{
    public TOpenDialog(TComponent owner) : base(owner) { }

    public string Filter = "", FileName = "", DefaultExt = "";

    public virtual bool Execute() => throw new NotImplementedException("VCL.OpenDialog");
}

public class TStringProperty : TObject
{
    public virtual void Edit() => throw new NotImplementedException("VCL.PropertyEditor");
    public virtual TSet<TPropertyAttribute> GetAttributes() => default;
    public virtual string GetValue() => throw new NotImplementedException("VCL.PropertyEditor");
}

public class TIniFile : TObject { }

public class TFiler : TObject { }

public class TIToolServices : TObject
{
    public virtual string GetProjectName() => throw new NotImplementedException("Delphi IDE project service");
}

public class THandleStream : TStream
{
    // TFastLoadFileStream supplies its own managed file stream and skips handle setup.
    public int Handle { get; protected set; }

    public THandleStream(int handle) => Handle = handle;
    public override int Read(Span<byte> buffer) => throw new NotImplementedException("HandleStream.Read");
    public override int Write(ReadOnlySpan<byte> buffer) => throw new NotImplementedException("HandleStream.Write");
    public override int Seek(int offset, int origin) => throw new NotImplementedException("HandleStream.Seek");
}

public static partial class RtlUnit
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

    public static string ExtractRelativePath(string baseName, string destination)
        => Files.ExtractRelativePath(baseName, destination);

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
