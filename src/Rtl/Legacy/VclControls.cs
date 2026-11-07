namespace Aow.Rtl;

public class IUnknown : TObject { }

public class TCanvas : TObject { }

public class TPopupMenu : TObject { }

public class Text : TObject { }

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

public class TDataModule : TComponent
{
    public TDataModule(TComponent? owner) : base(owner) { }
}

public class TVideoPlayer : TComponent
{
    public TVideoPlayer(TComponent? owner = null) : base(owner) { }
}

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

public class THandleStream : TStream
{
    // TFastLoadFileStream supplies its own managed file stream and skips handle setup.
    public int Handle { get; protected set; }

    public THandleStream(int handle) => Handle = handle;
    public override int Read(Span<byte> buffer) => throw new NotImplementedException("HandleStream.Read");
    public override int Write(ReadOnlySpan<byte> buffer) => throw new NotImplementedException("HandleStream.Write");
    public override int Seek(int offset, int origin) => throw new NotImplementedException("HandleStream.Seek");
}
