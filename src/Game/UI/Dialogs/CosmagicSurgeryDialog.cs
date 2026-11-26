#nullable disable warnings
// Translated from AoWDeformFaceDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// cosmagic-surgery caster
public sealed partial class CosmagicSurgeryDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label UnitNameLabel { get; private set; }

    private CosmagicSurgerySpellCaster? _caster;

    // AoWDeformFaceDlg.pas:127
    public void Execute(CosmagicSurgerySpellCaster caster)
    {
        _caster = caster;
        _ = _caster.AddRef();
        Troop? u = UI.Engine.StrategicMap.UnitControl.FindUnit(_caster.TargetUnitId);
        if (u is not null)
        {
            string name = u.GetName();
            UnitNameLabel.Text = name;
            Window.Visible = true;
        }
        else
        {
            Finish();
        }
    }

    // AoWDeformFaceDlg.pas:121
    public void Finish()
    {
        _ = _caster.Release();
        _caster = null;
    }

    // AoWDeformFaceDlg.pas:55
    public void CancelBtnClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWDeformFaceDlg.pas:61
    public void CastBtnClick()
    {
        string reason = "";
        UI.General.PlayButtonSound(0);
        if (_caster.CanCast(ref reason))
        {
            _ = _caster.Cast();
            Window.Visible = false;
        }
        else
        {
            UI.Engine.StrategicMap.ShowMessage(reason);
        }
    }

    // AoWDeformFaceDlg.pas:73
    public void DrawFace(Frame sender, LockedSurface lockedSurface, int offset)
    {
        int face = _caster.FaceIndex;
        while (offset < 0)
        {
            offset += 1;
            face = UI.Engine.GamePack.FaceResources.PreviousFace(face);
        }

        while (offset > 0)
        {
            offset -= 1;
            face = UI.Engine.GamePack.FaceResources.NextFace(face);
        }

        Rect r = new(Window.WinLeft + sender.WinLeft + 2, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth - 2, Window.WinTop + sender.WinTop + sender.WinHeight - 2);
        if (face != -1)
        {
            FaceResource? resource = UI.Engine.GamePack.FaceResources.GetFaceResource(face);
            _ = resource.AddRef();
            resource.FaceImage.ShowStretched(r, lockedSurface);
            _ = resource.Release();
        }
    }

    // AoWDeformFaceDlg.pas:104
    public void PrevFaceBtnClick()
    {
        UI.General.PlayButtonSound(0);
        int previousFace = UI.Engine.GamePack.FaceResources.PreviousFace(_caster.FaceIndex);
        _caster.FaceIndex = previousFace;
        Window.IsDrawn = false;
    }

    // AoWDeformFaceDlg.pas:111
    public void NextFaceBtnClick()
    {
        UI.General.PlayButtonSound(0);
        int nextFace = UI.Engine.GamePack.FaceResources.NextFace(_caster.FaceIndex);
        _caster.FaceIndex = nextFace;
        Window.IsDrawn = false;
    }

    // AoWDeformFaceDlg.pas:118
    public void DlgHide() => Finish();

    // AoWDeformFaceDlg.pas:142
    public void DlgKeyDown(in int key, ShiftStates shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }
}
