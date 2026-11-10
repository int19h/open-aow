namespace Aow.Rtl;

[LegacyObject]
public class IUnknown
{
    public IUnknown()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class Canvas
{
    public Canvas()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class PopupMenu
{
    public PopupMenu()
    {
        CaptureAddress.Assign(this);
    }
}

[LegacyObject]
public class Text
{
    public Text()
    {
        CaptureAddress.Assign(this);
    }
}

// These VCL controls retain component identity. Native drawing awaits the platform layer.
public class Control : Component
{
    public Control(Component? owner = null) : base(owner) { }

    public int Width, Height, Left, Top, Color;

    public int Cursor { get; set; }

    protected internal void SetCursor(int value) => Cursor = value;
    public void Hide() => throw new NotImplementedException("VCL.Hide");
    public void Show() => throw new NotImplementedException("VCL.Show");

    public Control? Parent;

    public virtual void Invalidate() => throw new NotImplementedException("VCL.Invalidate");
    protected internal virtual void Paint() => throw new NotImplementedException("VCL.Paint");

    protected internal virtual void KeyDown(ref ushort key, Set<ShiftState> shift) { }

    protected internal virtual void KeyUp(ref ushort key, Set<ShiftState> shift) { }

    protected internal virtual void Click() { }

    protected internal virtual int GetPalette() => 0;
    protected internal virtual void WndProc(ref Message message) => throw new NotImplementedException("VCL.WndProc");
}

public class CustomPanel : WinControl
{
    public CustomPanel(Component? owner = null) : base(owner) { }
}

public class WinControl : Control
{
    public WinControl(Component? owner = null) : base(owner) { }

    protected internal virtual void CreateParams(ref CreateParams parameters) => throw new NotImplementedException("VCL.CreateParams");
    protected internal virtual void CreateWnd() => throw new NotImplementedException("VCL.CreateWnd");
    protected internal virtual void DestroyWnd() => throw new NotImplementedException("VCL.DestroyWnd");

    protected internal virtual void MouseDown(MouseButton button, Set<ShiftState> shift, int x, int y) { }

    protected internal virtual void MouseUp(MouseButton button, Set<ShiftState> shift, int x, int y) { }

    protected internal virtual void MouseMove(Set<ShiftState> shift, int x, int y) { }

    protected internal virtual void KeyPress(ref byte key) { }

    protected internal virtual void DblClick() { }
}

public class Form : WinControl { }

public class Button : WinControl { }

public class Panel : CustomPanel { }

public class Memo : WinControl { }

public class Image : Control { }

public class Label : Control { }

public class Bevel : Control { }

public class TabControl : WinControl
{
    public TabControl(Component? owner = null) : base(owner) { }

    protected internal virtual void Change() { }
}

public class DataModule : Component
{
    public DataModule(Component? owner) : base(owner) { }
}

public class VideoPlayer : Component
{
    public VideoPlayer(Component? owner = null) : base(owner) { }
}

public class CustomGrid : WinControl
{
    public CustomGrid(Component? owner = null) : base(owner) { }

    public int ColCount, RowCount;
    public int Col, Row, DefaultColWidth, DefaultRowHeight;
    public ScrollStyle ScrollBars;

    protected internal void SetColCount(int value) => ColCount = value;
    protected internal void SetRowCount(int value) => RowCount = value;
    protected internal virtual void DrawCell(int col, int row, Rect rect, Set<GridDrawState> state) => throw new NotImplementedException("VCL.DrawCell");
    protected internal virtual bool SelectCell(int col, int row) => true;

    public enum ScrollStyle : byte { None, Horizontal, Vertical, Both }
}

public class ComboBox : WinControl
{
    public ComboBox(Component? owner = null) : base(owner) { }

    protected internal virtual void Change() { }

    protected internal virtual void DropDown() { }
}

public class ListBox : WinControl
{
    public ListBox(Component? owner = null) : base(owner) { }

}

public class OpenDialog : Component
{
    public OpenDialog(Component owner) : base(owner) { }

    public string Filter = "", FileName = "", DefaultExt = "";

    public virtual bool Execute() => throw new NotImplementedException("VCL.OpenDialog");
}

[LegacyObject]
public class StringProperty
{
    public virtual void Edit() => throw new NotImplementedException("VCL.PropertyEditor");
    public virtual Set<PropertyAttribute> GetAttributes() => default;
    public virtual string GetValue() => throw new NotImplementedException("VCL.PropertyEditor");

    public StringProperty()
    {
        CaptureAddress.Assign(this);
    }
}
