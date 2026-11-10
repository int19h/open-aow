// Translated from AoWCampaignSettings.pas.
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWCampaignSettings.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class CampaignSettingsDlg : DataModule
{
    public CampaignSettingsDlg(Component owner) : base(owner)
    { }

    public IvTranslator Translator;
    public AOWWindow Dlg;
    public AOWButton OKBtn;
    public AoWFrame BCeltL;
    public AOWLabel Title;
    public AOWButton Close;
    public AOWImage TitleIcon;
    public AOWLabel DifficultyLevelLbl;
    public AOWComboBox DifficultyLevelCB;
    public AOWVScrollBar CombatResolveModeSB;
    public AOWLabel TurnModeLbl;
    public AOWComboBox TurnModeCB;
    public AOWVScrollBar AOWVScrollBar1;
    public AoWFrame AoWFrame1;
    public AOWButton CancelBtn;
    public AOWImage TCeltLineL;

    // AoWCampaignSettings.pas:45
    public void DlgShow()
    {
        AoWCampaign.CampaignDifficultyLevel Level;
        DifficultyLevelCB.Items.Clear();
        for (int levelOrdinal = (int)(AoWCampaign.CampaignDifficultyLevel)0; levelOrdinal <= (int)(AoWCampaign.CampaignDifficultyLevel)2; levelOrdinal++)
        {
            Level = (AoWCampaign.CampaignDifficultyLevel)levelOrdinal;
            var campaignDifficultyLevel = Magic.Abilities.AbilityRules.TranslateRStr(TCampaignDifficultyLevelRStr[(int)Level]);
            DifficultyLevelCB.Items.AddObject(campaignDifficultyLevel, IntToObject((int)Level));
        }

        var indexOfObject = DifficultyLevelCB.Items.IndexOfObject(IntToObject((int)Magic.Abilities.GameTables.AoWEngine.FCampaign.FDifficultyLevel));
        DifficultyLevelCB.Selected = indexOfObject;
        TurnModeCB.Items.Clear();
        var playMode = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.TPlayModeRStr[(int)PlayMode.Turn]);
        TurnModeCB.Items.AddObject(playMode, IntToObject((int)PlayMode.Turn));
        var playModeText = Magic.Abilities.AbilityRules.TranslateRStr(Magic.Abilities.GameText.TPlayModeRStr[(int)PlayMode.Simultaneous]);
        TurnModeCB.Items.AddObject(playModeText, IntToObject((int)PlayMode.Simultaneous));
        var itemsIndexOfObject = TurnModeCB.Items.IndexOfObject(IntToObject((int)Magic.Abilities.GameTables.AoWEngine.FCampaign.FPlayMode));
        TurnModeCB.Selected = itemsIndexOfObject;
    }

    // AoWCampaignSettings.pas:59
    public void OKBtnClick()
    {
        var objectValue = DifficultyLevelCB.Items.Objects[DifficultyLevelCB.Selected];
        Magic.Abilities.GameTables.AoWEngine.FCampaign.FDifficultyLevel = (AoWCampaign.CampaignDifficultyLevel)ObjectToInt(objectValue);
        var objectValue2 = TurnModeCB.Items.Objects[TurnModeCB.Selected];
        Magic.Abilities.GameTables.AoWEngine.FCampaign.FPlayMode = (PlayMode)ObjectToInt(objectValue2);
        Dlg.Visible = false;
    }

    // AoWCampaignSettings.pas:66
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    private static Engine.Buffers.FixedArray_string_3 TCampaignDifficultyLevelRStr = Engine.Buffers.FixedArray_string_3.From(["Easy", "Normal", "Hard"]);
}
