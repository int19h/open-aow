// Translated from AoWSelFactionWin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWE.AoWE;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.E2GameFlow.E2GameFlow;

namespace Aow.Units.AoWSelFactionWin;

public partial class TSelFactionWin : TDataModule
{
    public TSelFactionWin(TComponent owner) : base(owner)
    { }

    public TAOWPanel FactionPnl;
    public TAOWLabel TopLbl;
    public TAOWPanel LightPnl;
    public TAOWMemo LText;
    public TAOWButton LStart;
    public TAOWButton LBack;
    public TAOWImage LSphere;
    public TAOWPanel DarkPnl;
    public TAOWButton DStart;
    public TAOWButton DBack;
    public TAOWLabel LName;
    public TAOWMemo DText;
    public TAOWLabel DName;
    public TAOWImage DSphere;
    public TIvTranslator Translator;
    public int FFade;
    public int FFadeImage;
    public int FFadeDirection;
    public FixedArray_byte_4 F_SelFactionWin_8C;
    // AoWSelFactionWin.pas:63
    public void CloseSelection()
    {
        aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
        TitleWindow.TitleWin.Visible = false;
        ShowTitleScene(TitleScene, true);
        FactionPnl.Visible = false;
        FFade = 0;
    }

    // AoWSelFactionWin.pas:72
    public void LStartClick(TObject Sender)
    {
        General.PlayButtonSound(0);
        aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin.ReleaseDiscardableImageData();
        FactionPnl.Visible = false;
        FFade = 0;
        if (Sender == LStart)
        {
            StartCampaignJoin(CampaignJoinDlg, 0);
        }
        else
        {
            StartCampaignJoin(CampaignJoinDlg, 1);
        }
    }

    // AoWSelFactionWin.pas:82
    public void LightPnlMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!DName.Visible && !LName.Visible)
        {
            DName.Visible = true;
            DText.Visible = true;
            DBack.Visible = true;
            DStart.Visible = true;
            DBack.CaptureInput = true;
            DStart.CaptureInput = true;
            TopLbl.Visible = false;
            FFade = 100;
            FactionPnl.ILIndexTexture = 2;
            FFadeImage = 0;
            FFadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:100
    public void DarkPnlMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!LName.Visible && !DName.Visible)
        {
            LName.Visible = true;
            LText.Visible = true;
            LBack.Visible = true;
            LStart.Visible = true;
            LBack.CaptureInput = true;
            LStart.CaptureInput = true;
            TopLbl.Visible = false;
            FFade = 100;
            FactionPnl.ILIndexTexture = 1;
            FFadeImage = 0;
            FFadeDirection = 1;
        }
    }

    // AoWSelFactionWin.pas:118
    public void LBackClick()
    {
        General.PlayButtonSound(0);
        LBack.CaptureInput = false;
        LStart.CaptureInput = false;
        FFade = 100;
        FFadeImage = 0;
        FFadeDirection = 0;
    }

    // AoWSelFactionWin.pas:128
    public void DBackClick()
    {
        General.PlayButtonSound(0);
        DBack.CaptureInput = false;
        DStart.CaptureInput = false;
        FFade = 100;
        FFadeImage = 0;
        FFadeDirection = 0;
    }

    // AoWSelFactionWin.pas:138
    public void FactionPnlMouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!LightPnl.FHandled && !DName.Visible && !DarkPnl.FHandled && !LName.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = false;
            var chooseFaction = TranslateRStr(ChooseFactionRStr);
            TopLbl.Text = chooseFaction;
        }
    }

    // AoWSelFactionWin.pas:148
    public void LightPnlMouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!LName.Visible && !DSphere.Visible)
        {
            DSphere.Visible = true;
            LSphere.Visible = false;
            var cultOfStorms = TranslateRStr(CultOfStormsRStr);
            TopLbl.Text = cultOfStorms;
        }
    }

    // AoWSelFactionWin.pas:158
    public void DarkPnlMouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!DName.Visible && !LSphere.Visible)
        {
            DSphere.Visible = false;
            LSphere.Visible = true;
            var keepers = TranslateRStr(KeepersRStr);
            TopLbl.Text = keepers;
        }
    }

    // AoWSelFactionWin.pas:168
    public void AbortBtnClick()
    {
        General.PlayButtonSound(0);
        CloseSelection();
    }

    // AoWSelFactionWin.pas:174
    public void FactionPnlUpdate()
    {
        TRect R;
        if (AoWSelFactionWin.SelFactionWin.FFade >= 0)
        {
            AoWSelFactionWin.SelFactionWin.FFade -= 6;
            if (FFade < 6)
            {
                if (LStart.Visible && !LStart.CaptureInput)
                {
                    LName.Visible = false;
                    LText.Visible = false;
                    LBack.Visible = false;
                    LStart.Visible = false;
                    LSphere.Visible = false;
                    TopLbl.Visible = true;
                    var chooseFaction = TranslateRStr(ChooseFactionRStr);
                    TopLbl.Text = chooseFaction;
                    FactionPnl.ILIndexTexture = 0;
                }
                else
                {
                    if (DStart.Visible && !DStart.CaptureInput)
                    {
                        DName.Visible = false;
                        DText.Visible = false;
                        DBack.Visible = false;
                        DStart.Visible = false;
                        DSphere.Visible = false;
                        TopLbl.Visible = true;
                        var chooseFaction = TranslateRStr(ChooseFactionRStr);
                        TopLbl.Text = chooseFaction;
                        FactionPnl.ILIndexTexture = 0;
                    }
                }
            }

            R = Rect(this.FactionPnl.WinLeft, this.FactionPnl.WinTop, this.FactionPnl.WinLeft + this.FactionPnl.WinWidth, FactionPnl.WinTop + this.FactionPnl.WinHeight);

            TitleWindow.TitleWin.AddBltRect(ref R, null);
        }
    }

    // AoWSelFactionWin.pas:210
    public void FactionPnlKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        switch (Key)
        {
            case 27:
                CloseSelection();
                break;
        }
    }
}

public static partial class AoWSelFactionWin
{
    public static TSelFactionWin SelFactionWin;
    // AoWSelFactionWin.pas:215
    public static void ShowFactionSelection(TSelFactionWin Dialog)
    {
        TitleWindow.TitleWin.Visible = true;
        SelFactionWin.FactionPnl.Visible = true;
        Dialog.TopLbl.Visible = true;
        var chooseFaction = TranslateRStr(ChooseFactionRStr);
        Dialog.TopLbl.Text = chooseFaction;
        Dialog.LName.Visible = false;
        Dialog.LText.Visible = false;
        Dialog.LBack.Visible = false;
        Dialog.LStart.Visible = false;
        Dialog.LSphere.Visible = false;
        Dialog.DName.Visible = false;
        Dialog.DText.Visible = false;
        Dialog.DBack.Visible = false;
        Dialog.DStart.Visible = false;
        Dialog.DSphere.Visible = false;
        Dialog.FFade = -1;
        Dialog.FactionPnl.ILIndexTexture = 0;
        Dialog.F_SelFactionWin_8C[0] = 0;
    }
}
