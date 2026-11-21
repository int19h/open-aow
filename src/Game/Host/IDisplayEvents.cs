
using Aow.Rtl;

namespace Aow.Game.Host;

public interface IDisplayEvents
{
    void MouseDown(MouseButton button, ShiftStates shift, int x, int y) { }

    void MouseUp(MouseButton button, ShiftStates shift, int x, int y) { }

    void MouseMove(ShiftStates shift, int x, int y) { }

    // Positive steps are a roll away from the user. A port addition the original does not have.
    void MouseWheel(int steps, ShiftStates shift, int x, int y) { }

    void DoubleClick() { }

    void KeyDown(ushort key, ShiftStates shift) { }

    void KeyUp(ushort key, ShiftStates shift) { }

    void KeyPress(byte character) { }

    void Resize(int width, int height) { }

    void FocusChanged(bool active) { }

    bool CloseRequested() => true;
}
