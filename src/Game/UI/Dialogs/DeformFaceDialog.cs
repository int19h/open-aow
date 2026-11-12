// Translated from AoWDeformFaceDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// cosmetic-surgery caster
public partial class DeformFaceDialog : DataModule
{
    public DeformFaceDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label UnitNameLabel;
    private CosmeticSurgerySpellCaster _caster;

    // AoWDeformFaceDlg.pas:127
    public void Execute(CosmeticSurgerySpellCaster caster)
    {
        Troop U;
        _caster = caster;
        _caster.AddRef();
        U = AoWHSMap.Current.UnitControl.FindUnit(_caster.TargetUnitId);
        if (U != null)
        {
            var name = U.GetName();
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
        _caster.Release();
        _caster = null;
    }

    // AoWDeformFaceDlg.pas:55
    public void CancelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWDeformFaceDlg.pas:61
    public void CastBtnClick()
    {
        string reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (_caster.CanCast(ref reason))
        {
            _caster.Cast();
            Window.Visible = false;
        }
        else
        {
            AoWHSMap.Current.ShowMessage(reason);
        }
    }

    // AoWDeformFaceDlg.pas:73
    public void DrawFace(Frame sender, LockedSurface lockedSurface, int offset)
    {
        Rect R;
        int face;
        FaceResource resource;
        face = _caster.FaceIndex;
        while (offset < 0)
        {
            offset += 1;
            face = AoWHSSet.Current.FaceResources.PreviousFace(face);
        }

        while (offset > 0)
        {
            offset -= 1;
            face = AoWHSSet.Current.FaceResources.NextFace(face);
        }

        R = new Rect(Window.WinLeft + sender.WinLeft + 2, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth - 2, Window.WinTop + sender.WinTop + sender.WinHeight - 2);
        if (face != -1)
        {
            resource = AoWHSSet.Current.FaceResources.GetFaceResource(face);
            resource.AddRef();
            resource.GetFaceImage().ShowStretched(R, lockedSurface);
            resource.Release();
        }
    }

    // AoWDeformFaceDlg.pas:104
    public void PrevFaceBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var previousFace = AoWHSSet.Current.FaceResources.PreviousFace(_caster.FaceIndex);
        _caster.FaceIndex = previousFace;
        Window.Drawn = false;
    }

    // AoWDeformFaceDlg.pas:111
    public void NextFaceBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var nextFace = AoWHSSet.Current.FaceResources.NextFace(_caster.FaceIndex);
        _caster.FaceIndex = nextFace;
        Window.Drawn = false;
    }

    // AoWDeformFaceDlg.pas:118
    public void DlgHide()
    {
        Finish();
    }

    // AoWDeformFaceDlg.pas:142
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }
}
