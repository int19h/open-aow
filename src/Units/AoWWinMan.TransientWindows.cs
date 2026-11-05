using Aow.Units.AoWWinControl;

namespace Aow.Units.AoWWinMan;

public partial class TGeneral
{
    // AoWWinMan.pas:453 reads the inherited modal field on menu siblings too.
    public void HideTransientWindows()
    {
        for (int i = 0; i < Manager.FChildren.Count; i++)
            if (Manager.FChildren[i] is TAOWWindowControl window &&
                window != AOWMWindow.AOWMWindow.MWindow.MapWindow && window.FModal && window.Visible)
                window.Visible = false;
    }
}
