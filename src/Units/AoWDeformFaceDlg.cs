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
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// cosmetic-surgery caster
public partial class DeformFaceDlg : DataModule
{
    public DeformFaceDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AoWFrame FaceFrame;
    public AOWButton CancelBtn;
    public AOWImage TitleIcon;
    public AOWLabel Title;
    public AOWButton CastBtn;
    public AOWLabel SelectNewFaceLbl;
    public AOWButton Close;
    public AoWFrame LeftFace;
    public AoWFrame RightFace;
    public AoWFrame AoWFrame1;
    public AOWImage CeltM;
    public AOWImage CeltL;
    public AOWImage CeltR;
    public AOWButton PrevFaceBtn;
    public AOWButton NextFaceBtn;
    public AOWLabel UnitNameLbl;
    public IvTranslator Translator;
    public CosmeticSurgerySpellCaster FCaster;

    // AoWDeformFaceDlg.pas:127
    public void Execute(CosmeticSurgerySpellCaster Caster)
    {
        AbstractUnit U;
        FCaster = Caster;
        FCaster.AddRef();
        U = World.Map.AoWHSMap.Current.FUnitControl.FindUnit(FCaster.FTargetUnitID);
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
            World.Map.AoWHSMap.Current.ShowMessage(Reason);
        }
    }

    // AoWDeformFaceDlg.pas:73
    public void DrawFace(AoWFrame Sender, SurfaceDesc SD, int Offset)
    {
        Rect R;
        int Face;
        FaceResource Resource;
        Face = FCaster.FFaceIndex;
        while (Offset < 0)
        {
            Offset += 1;
            Face = Engine.AoWHSSet.Current.FFaceResources.PrevFace(Face);
        }

        while (Offset > 0)
        {
            Offset -= 1;
            Face = Engine.AoWHSSet.Current.FFaceResources.NextFace(Face);
        }

        R = new Rect(Dlg.WinLeft + Sender.WinLeft + 2, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth - 2, Dlg.WinTop + Sender.WinTop + Sender.WinHeight - 2);
        if (Face != -1)
        {
            Resource = Engine.AoWHSSet.Current.FFaceResources.GetFaceResource(Face);
            Resource.AddRef();
            Resource.GetFaceImage().ShowStretched(R, SD);
            Resource.Release();
        }
    }

    // AoWDeformFaceDlg.pas:104
    public void PrevFaceBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var prevFace = Engine.AoWHSSet.Current.FFaceResources.PrevFace(FCaster.FFaceIndex);
        FCaster.FFaceIndex = prevFace;
        Dlg.FDrawn = false;
    }

    // AoWDeformFaceDlg.pas:111
    public void NextFaceBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var nextFace = Engine.AoWHSSet.Current.FFaceResources.NextFace(FCaster.FFaceIndex);
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
