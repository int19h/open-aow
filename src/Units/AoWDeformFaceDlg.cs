// Translated from AoWDeformFaceDlg.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Rtl;
// Translated from AoWDeformFaceDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Game.Magic.Abilities.AoWEUnit;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// cosmetic-surgery caster
public partial class TDeformFaceDlg : TDataModule
{
    public TDeformFaceDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAoWFrame FaceFrame;
    public TAOWButton CancelBtn;
    public TAOWImage TitleIcon;
    public TAOWLabel Title;
    public TAOWButton CastBtn;
    public TAOWLabel SelectNewFaceLbl;
    public TAOWButton Close;
    public TAoWFrame LeftFace;
    public TAoWFrame RightFace;
    public TAoWFrame AoWFrame1;
    public TAOWImage CeltM;
    public TAOWImage CeltL;
    public TAOWImage CeltR;
    public TAOWButton PrevFaceBtn;
    public TAOWButton NextFaceBtn;
    public TAOWLabel UnitNameLbl;
    public TIvTranslator Translator;
    public TCosmeticSurgerySpellCaster FCaster;

    // AoWDeformFaceDlg.pas:127
    public void Execute(TCosmeticSurgerySpellCaster Caster)
    {
        TAbstractUnit U;
        FCaster = Caster;
        FCaster.AddRef();
        U = AoWHSMap.FUnitControl.FindUnit(FCaster.FTargetUnitID);
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
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWDeformFaceDlg.pas:61
    public void CastBtnClick()
    {
        string Reason = "";
        General.PlayButtonSound(0);
        if (FCaster.CanCast(ref Reason))
        {
            FCaster.Cast();
            Dlg.Visible = false;
        }
        else
        {
            AoWHSMap.ShowMessage(Reason);
        }
    }

    // AoWDeformFaceDlg.pas:73
    public void DrawFace(TAoWFrame Sender, TSurfaceDesc SD, int Offset)
    {
        TRect R;
        int Face;
        TFaceResource Resource;
        Face = FCaster.FFaceIndex;
        while (Offset < 0)
        {
            Offset += 1;
            Face = AoWHSSet.FFaceResources.PrevFace(Face);
        }

        while (Offset > 0)
        {
            Offset -= 1;
            Face = AoWHSSet.FFaceResources.NextFace(Face);
        }

        R = Rect(Dlg.WinLeft + Sender.WinLeft + 2, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth - 2, Dlg.WinTop + Sender.WinTop + Sender.WinHeight - 2);
        if (Face != -1)
        {
            Resource = AoWHSSet.FFaceResources.GetFaceResource(Face);
            Resource.AddRef();
            Resource.GetFaceImage().ShowStretched(R, SD);
            Resource.Release();
        }
    }

    // AoWDeformFaceDlg.pas:104
    public void PrevFaceBtnClick()
    {
        General.PlayButtonSound(0);
        var prevFace = AoWHSSet.FFaceResources.PrevFace(FCaster.FFaceIndex);
        FCaster.FFaceIndex = prevFace;
        Dlg.FDrawn = false;
    }

    // AoWDeformFaceDlg.pas:111
    public void NextFaceBtnClick()
    {
        General.PlayButtonSound(0);
        var nextFace = AoWHSSet.FFaceResources.NextFace(FCaster.FFaceIndex);
        FCaster.FFaceIndex = nextFace;
        Dlg.FDrawn = false;
    }

    // AoWDeformFaceDlg.pas:118
    public void DlgHide()
    {
        Finish();
    }

    // AoWDeformFaceDlg.pas:142
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }
}

public static partial class AoWDeformFaceDlgUnit
{
}
