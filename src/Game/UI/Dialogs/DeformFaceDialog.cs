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

    public Window Dlg;
    public Frame FaceFrame;
    public Button CancelBtn;
    public Image TitleIcon;
    public Label Title;
    public Button CastBtn;
    public Label SelectNewFaceLbl;
    public Button Close;
    public Frame LeftFace;
    public Frame RightFace;
    public Frame AoWFrame1;
    public Image CeltM;
    public Image CeltL;
    public Image CeltR;
    public Button PrevFaceBtn;
    public Button NextFaceBtn;
    public Label UnitNameLbl;
    public IvTranslator Translator;
    public CosmeticSurgerySpellCaster FCaster;

    // AoWDeformFaceDlg.pas:127
    public void Execute(CosmeticSurgerySpellCaster Caster)
    {
        Troop U;
        FCaster = Caster;
        FCaster.AddRef();
        U = AoWHSMap.Current.FUnitControl.FindUnit(FCaster.FTargetUnitID);
        if (U != null)
        {
            var name = U.GetName();
            UnitNameLbl.Text = name;
            Dlg.Visible = true;
        }
        else
        {
            Finish();
        }
    }

    // AoWDeformFaceDlg.pas:121
    public void Finish()
    {
        FCaster.Release();
        FCaster = null;
    }

    // AoWDeformFaceDlg.pas:55
    public void CancelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWDeformFaceDlg.pas:61
    public void CastBtnClick()
    {
        string Reason = "";
        DialogHosts.General.PlayButtonSound(0);
        if (FCaster.CanCast(ref Reason))
        {
            FCaster.Cast();
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.Current.ShowMessage(Reason);
        }
    }

    // AoWDeformFaceDlg.pas:73
    public void DrawFace(Frame Sender, LockedSurface SD, int Offset)
    {
        Rect R;
        int Face;
        FaceResource Resource;
        Face = FCaster.FFaceIndex;
        while (Offset < 0)
        {
            Offset += 1;
            Face = AoWHSSet.Current.FFaceResources.PrevFace(Face);
        }

        while (Offset > 0)
        {
            Offset -= 1;
            Face = AoWHSSet.Current.FFaceResources.NextFace(Face);
        }

        R = new Rect(Dlg.WinLeft + Sender.WinLeft + 2, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth - 2, Dlg.WinTop + Sender.WinTop + Sender.WinHeight - 2);
        if (Face != -1)
        {
            Resource = AoWHSSet.Current.FFaceResources.GetFaceResource(Face);
            Resource.AddRef();
            Resource.GetFaceImage().ShowStretched(R, SD);
            Resource.Release();
        }
    }

    // AoWDeformFaceDlg.pas:104
    public void PrevFaceBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var prevFace = AoWHSSet.Current.FFaceResources.PrevFace(FCaster.FFaceIndex);
        FCaster.FFaceIndex = prevFace;
        Dlg.FDrawn = false;
    }

    // AoWDeformFaceDlg.pas:111
    public void NextFaceBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var nextFace = AoWHSSet.Current.FFaceResources.NextFace(FCaster.FFaceIndex);
        FCaster.FFaceIndex = nextFace;
        Dlg.FDrawn = false;
    }

    // AoWDeformFaceDlg.pas:118
    public void DlgHide()
    {
        Finish();
    }

    // AoWDeformFaceDlg.pas:142
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}
