// Translated from AoWCampaignSettings.pas.
using Aow.Engine.Buffers;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class CampaignSettingsDialog : DataModule
{
    public CampaignSettingsDialog(Component owner) : base(owner)
    { }

    public IvTranslator Translator;
    public Window Dlg;
    public Button OKBtn;
    public Frame BCeltL;
    public Label Title;
    public Button Close;
    public Image TitleIcon;
    public Label DifficultyLevelLbl;
    public ComboBox DifficultyLevelCB;
    public VerticalScrollBar CombatResolveModeSB;
    public Label TurnModeLbl;
    public ComboBox TurnModeCB;
    public VerticalScrollBar AOWVScrollBar1;
    public Frame AoWFrame1;
    public Button CancelBtn;
    public Image TCeltLineL;

    // AoWCampaignSettings.pas:45
    public void DlgShow()
    {
        Campaign.DifficultyLevel Level;
        DifficultyLevelCB.Items.Clear();
        for (int levelOrdinal = (int)(Campaign.DifficultyLevel)0; levelOrdinal <= (int)(Campaign.DifficultyLevel)2; levelOrdinal++)
        {
            Level = (Campaign.DifficultyLevel)levelOrdinal;
            var campaignDifficultyLevel = AbilityRules.TranslateRStr(TCampaignDifficultyLevelRStr[(int)Level]);
            DifficultyLevelCB.Items.AddObject(campaignDifficultyLevel, IntToObject((int)Level));
        }

        var indexOfObject = DifficultyLevelCB.Items.IndexOfObject(IntToObject((int)GameTables.AoWEngine.FCampaign.FDifficultyLevel));
        DifficultyLevelCB.Selected = indexOfObject;
        TurnModeCB.Items.Clear();
        var playMode = AbilityRules.TranslateRStr(GameText.TPlayModeRStr[(int)PlayMode.Turn]);
        TurnModeCB.Items.AddObject(playMode, IntToObject((int)PlayMode.Turn));
        var playModeText = AbilityRules.TranslateRStr(GameText.TPlayModeRStr[(int)PlayMode.Simultaneous]);
        TurnModeCB.Items.AddObject(playModeText, IntToObject((int)PlayMode.Simultaneous));
        var itemsIndexOfObject = TurnModeCB.Items.IndexOfObject(IntToObject((int)GameTables.AoWEngine.FCampaign.FPlayMode));
        TurnModeCB.Selected = itemsIndexOfObject;
    }

    // AoWCampaignSettings.pas:59
    public void OKBtnClick()
    {
        var objectValue = DifficultyLevelCB.Items.Objects[DifficultyLevelCB.Selected];
        GameTables.AoWEngine.FCampaign.FDifficultyLevel = (Campaign.DifficultyLevel)ObjectToInt(objectValue);
        var objectValue2 = TurnModeCB.Items.Objects[TurnModeCB.Selected];
        GameTables.AoWEngine.FCampaign.FPlayMode = (PlayMode)ObjectToInt(objectValue2);
        Dlg.Visible = false;
    }

    // AoWCampaignSettings.pas:66
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    private static StringArray3 TCampaignDifficultyLevelRStr = StringArray3.From(["Easy", "Normal", "Hard"]);
}
