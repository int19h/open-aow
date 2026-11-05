// Translated from AoWVideoDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWVideoDlg;

// full screen, change flag, display size
public partial class TVideoDialog : TDataModule
{
    public TVideoDialog(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWButton CloseBut;
    public TAoWFrame BCeltL;
    public TAOWLabel Title;
    public TAOWButton Close;
    public TAOWImage TCeltLR;
    public TAoWFrame TopLine;
    public TAOWLabel VideoLbl;
    public TAOWImage TCeltPattern;
    public TAOWImage TitleIcon;
    public TAOWImage TCeltLL;
    public TAOWListBox DisplayModesLB;
    public TAOWCheckBox ResCB;
    public TAOWLabel ResSwLbl;
    public TAOWButton ApplyBtn;
    public TAOWPanel T3LBPnl;
    public TAOWVScrollBar VideoSB;
    public TIvTranslator Translator;
    public TDisplayModeList FDisplayModes;
    public TScreenMode FFullScreen;
    public bool FModeChanged;
    public int FSceneWidth;
    public int FSceneHeight;
    public int FDisplayWidth;
    public int FDisplayHeight;
    // AoWVideoDlg.pas:366
    public void SetSceneSize(int W, int H)
    {
        FSceneWidth = W;
        FSceneHeight = H;
        if (FFullScreen == TScreenMode.scmTitle)
        {
            FModeChanged = true;
        }
    }

    // AoWVideoDlg.pas:416
    public void ShowModes()
    {
        Dlg.Visible = true;
        UpdateModes();
    }

    // AoWVideoDlg.pas:172
    public void UpdateModes()
    {
        TDisplayModeItem Mode;
        string S;
        DisplayModesLB.Items.Clear();
        var desktopWindow = TranslateRStr(DesktopWindowRStr);
        DisplayModesLB.Items.AddObject(desktopWindow, IntToObject(-1));
        for (int I = 0, last = FDisplayModes.Count - 1; I <= last; I++)
        {
            Mode = FDisplayModes.Items[I];
            if (Mode.Width >= 640 && Mode.Height >= 480)
            {
                var fullScreen = TranslateRStr(FullScreenRStr);
                DisplayModesLB.Items.AddObject(fullScreen + ":" + IntToStr(Mode.Width) + "x" + IntToStr(Mode.Height), IntToObject(I));
            }
        }

        var currentMode = CurrentMode();
        DisplayModesLB.Selected = currentMode;
        if (DisplayModesLB.Selected != -1)
        {
            S = DisplayModesLB.Items[DisplayModesLB.Selected];
            S = S + " (" + LowerCase(TranslateRStr(CurrentRStr)) + ")";
            DisplayModesLB.Items[DisplayModesLB.Selected] = S;
        }
    }

    // AoWVideoDlg.pas:195
    public void VideoDlgWindowCreate()
    {
        FDisplayModes = new TDisplayModeList();
        ListDisplayModes(FDisplayModes, 0, 0, 16);
    }

    // AoWVideoDlg.pas:201
    public void VideoDlgWindowDestroy()
    {
        FDisplayModes.Free();
    }

    // AoWVideoDlg.pas:206
    public void CloseButClick()
    {
        General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWVideoDlg.pas:212
    public void ApplyBtnClick()
    {
        int I;
        General.PlayButtonSound(0);
        if (DisplayModesLB.Selected != -1)
        {
            I = ObjectToInt(DisplayModesLB.Items.Objects[DisplayModesLB.Selected]);
            if (I != -1)
            {
                FDisplayWidth = FDisplayModes.Items[I].Width;
                FDisplayHeight = FDisplayModes.Items[I].Height;
                ChangeResolution(FDisplayWidth, FDisplayHeight);
                FFullScreen = TScreenMode.scmGame;
            }
            else
            {
                FFullScreen = TScreenMode.scmWindowed;
                ChangeResolution(0, 0);
            }
        }
    }

    // AoWVideoDlg.pas:233
    public void ResCBChange(TObject Sender)
    {
        if (((TAOWCheckBox)Sender).Checked)
        {
            General.PlayButtonSound(1);
        }
        else
        {
            General.PlayButtonSound(2);
        }

        AoWEngine.FRegistry.SetAllowResolutionSwitches(ResCB.Checked);
    }

    // AoWVideoDlg.pas:239
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        General.Manager.FAligned = false;
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    // AoWVideoDlg.pas:245
    public void DlgShow()
    {
        var allowResolutionSwitches = AoWEngine.FRegistry.GetAllowResolutionSwitches();
        AoWVideoDlg.VideoDialog.ResCB.Checked = allowResolutionSwitches;
    }
}

public enum TScreenMode : byte
{
    scmWindowed = 0,
    scmGame = 1,
    scmTitle = 2
}

// image library, image index, heading, text
public partial class TWMCampaignScenarioPage : TEObject
{
    // AoWVideoDlg.pas:264
    public new const int ClassID = 2248706;

    public string FImageLibrary = "";
    public int FImageIndex;
    public string FHeading = "";
    public TStringList FText;
    // AoWVideoDlg.pas:257
    public override void Destroy()
    {
        FText.Free();
        base.Destroy();
    }

    // AoWVideoDlg.pas:268
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwStrings(FText, 10);
        Stream.rwString(ref FHeading, 11);
        if (!Stream.StorageMode().Contains(TStorageModeElement.smTextOnly))
        {
            Stream.rwString(ref FImageLibrary, 12);
        }

        Stream.rwInteger(ref FImageIndex, 13);
    }

    // AoWVideoDlg.pas:250
    public TWMCampaignScenarioPage(TECustomNode owner) : base(owner)
    {
        FText = new TStringList();
    }

}

public partial class TWMCampaignScenarioPageList : TENode
{
    public TWMCampaignScenarioPageList(TECustomNode owner) : base(owner)
    { }

}

// image library, path text, description, flag, position, paths, pages, map kind
public partial class TWMCampaignScenario : TAoWCampaignScenario
{
    // AoWVideoDlg.pas:304
    public new const int ClassID = 2248705;

    public string FImageLibrary = "";
    public string FPathDescription = "";
    public string FDescription = "";
    public bool F_WMCampaignScenario_20;
    // rwBool
    public int FMapX;
    public int FMapY;
    public TIntegerList FPaths;
    public TWMCampaignScenarioPageList FIntroPages;
    public TWMCampaignScenarioPageList FOutroPages;
    public byte FMapKind;
    // AoWVideoDlg.pas:295
    public override void Destroy()
    {
        FIntroPages.Free();
        FOutroPages.Free();
        FPaths.Free();
        base.Destroy();
    }

    // AoWVideoDlg.pas:308
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (!Stream.StorageMode().Contains(TStorageModeElement.smTextOnly))
        {
            Stream.rwString(ref FImageLibrary, 12);
        }

        Stream.rwString(ref FPathDescription, 13);
        Stream.rwString(ref FDescription, 14);
        Stream.rwBool(ref F_WMCampaignScenario_20, 15);
        Stream.rwInteger(ref FMapX, 16);
        Stream.rwInteger(ref FMapY, 17);
        Stream.rwEObject(FIntroPages, 19);
        Stream.rwEObject(FOutroPages, 20);
        Stream.rwStreamProc(21, FPaths.LoadFromStream, FPaths.SaveToStream);
        Stream.rwData(Bytes.Of(ref FMapKind, 1), 1, 22);
    }

    // AoWVideoDlg.pas:278
    public TWMCampaignScenario(TECustomNode owner) : base(owner)
    {
        FIntroPages = new TWMCampaignScenarioPageList(null);
        FOutroPages = new TWMCampaignScenarioPageList(null);
        for (int I = 0; I <= 9; I++)
        {
            new TWMCampaignScenarioPage(FIntroPages);
            new TWMCampaignScenarioPage(FOutroPages);
        }

        FPaths = new TIntegerList();
        FPaths.Default = -1;
    }

}

// faction, state, page
public partial class TWMCampaign : TAoWCampaign
{
    // AoWVideoDlg.pas:334
    public new const int ClassID = 2248704;

    public int FFaction;
    public int FState;
    public int FPage;
    // AoWVideoDlg.pas:329
    public override void Destroy()
    {
        base.Destroy();
    }

    // AoWVideoDlg.pas:337
    public override void ReadWrite(TEStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FFaction, 30);
    }

    // AoWVideoDlg.pas:324
    public TWMCampaign(TECustomNode owner) : base(owner)
    {
    }

    // AoWVideoDlg.pas:348
    public override void Clear()
    {
        base.Clear();
    }

    // AoWVideoDlg.pas:343
    public override TAoWCampaignScenario CreateScenario() => new TWMCampaignScenario(null);

    // AoWVideoDlg.pas:353
    public override string GetAutoSaveFileName(TCampaignAutoSaveType Auto)
    {
        string Result;
        if (Auto == TCampaignAutoSaveType.casScenarioFinished)
        {
            Result = AoWHSMap.FMapName;
            if (FFaction == 0)
            {
                Result = Result + "_" + TranslateRStr(WonRStr);
            }
            else
            {
                // { Original uses Won for both factions. }
                Result = Result + "-" + TranslateRStr(WonRStr);
            }

            Result = Result + ".acg";
        }
        else
        {
            Result = base.GetAutoSaveFileName(Auto);
        }

        return Result;
    }

}

public static partial class AoWVideoDlg
{
    public static TVideoDialog VideoDialog;
    // AoWVideoDlg.pas:422
    public static void RegisterWorldMapCampaign(TEngine AEngine)
    {
        AEngine.RegisterEClasses([EClass.Of<TWMCampaignScenarioPage>(), EClass.Of<TWMCampaignScenario>(), EClass.Of<TWMCampaign>()]);
    }
}
