// Translated from AoWCampaignSettings.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.AoWCampaignSettings;

public partial class TCampaignSettingsDlg : TDataModule
{
    public TCampaignSettingsDlg(TComponent owner) : base(owner)
    { }

    public TIvTranslator Translator;
    public TAOWWindow Dlg;
    public TAOWButton OKBtn;
    public TAoWFrame BCeltL;
    public TAOWLabel Title;
    public TAOWButton Close;
    public TAOWImage TitleIcon;
    public TAOWLabel DifficultyLevelLbl;
    public TAOWComboBox DifficultyLevelCB;
    public TAOWVScrollBar CombatResolveModeSB;
    public TAOWLabel TurnModeLbl;
    public TAOWComboBox TurnModeCB;
    public TAOWVScrollBar AOWVScrollBar1;
    public TAoWFrame AoWFrame1;
    public TAOWButton CancelBtn;
    public TAOWImage TCeltLineL;

    // AoWCampaignSettings.pas:45
    public void DlgShow()
    {
        TCampaignDifficultyLevel Level;
        DifficultyLevelCB.Items.Clear();
        for (int levelOrdinal = (int)(TCampaignDifficultyLevel)0; levelOrdinal <= (int)(TCampaignDifficultyLevel)2; levelOrdinal++)
        {
            Level = (TCampaignDifficultyLevel)levelOrdinal;
            var campaignDifficultyLevel = TranslateRStr(TCampaignDifficultyLevelRStr[(int)Level]);
            DifficultyLevelCB.Items.AddObject(campaignDifficultyLevel, IntToObject((int)Level));
        }

        var indexOfObject = DifficultyLevelCB.Items.IndexOfObject(IntToObject((int)AoWEngine.FCampaign.FDifficultyLevel));
        DifficultyLevelCB.Selected = indexOfObject;
        TurnModeCB.Items.Clear();
        var playMode = TranslateRStr(TPlayModeRStr[(int)TPlayMode.pmTurn]);
        TurnModeCB.Items.AddObject(playMode, IntToObject((int)TPlayMode.pmTurn));
        var playModeText = TranslateRStr(TPlayModeRStr[(int)TPlayMode.pmSimultaneous]);
        TurnModeCB.Items.AddObject(playModeText, IntToObject((int)TPlayMode.pmSimultaneous));
        var itemsIndexOfObject = TurnModeCB.Items.IndexOfObject(IntToObject((int)AoWEngine.FCampaign.FPlayMode));
        TurnModeCB.Selected = itemsIndexOfObject;
    }

    // AoWCampaignSettings.pas:59
    public void OKBtnClick()
    {
        var objectValue = DifficultyLevelCB.Items.Objects[DifficultyLevelCB.Selected];
        AoWEngine.FCampaign.FDifficultyLevel = (TCampaignDifficultyLevel)ObjectToInt(objectValue);
        var objectValue2 = TurnModeCB.Items.Objects[TurnModeCB.Selected];
        AoWEngine.FCampaign.FPlayMode = (TPlayMode)ObjectToInt(objectValue2);
        Dlg.Visible = false;
    }

    // AoWCampaignSettings.pas:66
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }
}

public static partial class AoWCampaignSettingsUnit
{
}
