namespace Aow.Rtl;

public class IUnknown
{
}

public class Canvas
{
}

public class PopupMenu
{
}

public class Text
{
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
    protected internal virtual void WindowProc(ref Message message) => throw new NotImplementedException("VCL.WndProc");
}

public class WinControl : Control
{
    public WinControl(Component? owner = null) : base(owner) { }

    protected internal virtual void CreateParams(ref CreateParams parameters) => throw new NotImplementedException("VCL.CreateParams");
    protected internal virtual void CreateWindow() => throw new NotImplementedException("VCL.CreateWnd");
    protected internal virtual void DestroyWnd() => throw new NotImplementedException("VCL.DestroyWnd");

    protected internal virtual void MouseDown(MouseButton button, Set<ShiftState> shift, int x, int y) { }

    protected internal virtual void MouseUp(MouseButton button, Set<ShiftState> shift, int x, int y) { }

    protected internal virtual void MouseMove(Set<ShiftState> shift, int x, int y) { }

    protected internal virtual void KeyPress(ref byte key) { }

    protected internal virtual void DoubleClick() { }
}

public class Form : WinControl { }

public class DataModule : Component
{
    public DataModule(Component? owner) : base(owner) { }
}

public class VideoPlayer : Component
{
    public VideoPlayer(Component? owner = null) : base(owner) { }
}
