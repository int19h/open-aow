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

    public Window Dlg;
    public ComboBox DifficultyLevelCB;
    public ComboBox TurnModeCB;

    // AoWCampaignSettings.pas:45
    public void DlgShow()
    {
        CampaignDifficultyLevel level;
        DifficultyLevelCB.Items.Clear();
        for (int levelOrdinal = (int)(CampaignDifficultyLevel)0; levelOrdinal <= (int)(CampaignDifficultyLevel)2; levelOrdinal++)
        {
            level = (CampaignDifficultyLevel)levelOrdinal;
            var campaignDifficultyLevel = AbilityRules.TranslateResourceString(s_campaignDifficultyLevelTexts[(int)level]);
            DifficultyLevelCB.Items.AddObject(campaignDifficultyLevel, IntToObject((int)level));
        }

        var indexOfObject = DifficultyLevelCB.Items.IndexOfObject(IntToObject((int)GameTables.GameEngine.Campaign.DifficultyLevel));
        DifficultyLevelCB.Selected = indexOfObject;
        TurnModeCB.Items.Clear();
        var playMode = AbilityRules.TranslateResourceString(GameText.s_playModeTexts[(int)PlayMode.Turn]);
        TurnModeCB.Items.AddObject(playMode, IntToObject((int)PlayMode.Turn));
        var playModeText = AbilityRules.TranslateResourceString(GameText.s_playModeTexts[(int)PlayMode.Simultaneous]);
        TurnModeCB.Items.AddObject(playModeText, IntToObject((int)PlayMode.Simultaneous));
        var itemsIndexOfObject = TurnModeCB.Items.IndexOfObject(IntToObject((int)GameTables.GameEngine.Campaign.PlayMode));
        TurnModeCB.Selected = itemsIndexOfObject;
    }

    // AoWCampaignSettings.pas:59
    public void OKBtnClick()
    {
        var objectValue = DifficultyLevelCB.Items.Objects[DifficultyLevelCB.Selected];
        GameTables.GameEngine.Campaign.DifficultyLevel = (CampaignDifficultyLevel)ObjectToInt(objectValue);
        var objectValue2 = TurnModeCB.Items.Objects[TurnModeCB.Selected];
        GameTables.GameEngine.Campaign.PlayMode = (PlayMode)ObjectToInt(objectValue2);
        Dlg.Visible = false;
    }

    // AoWCampaignSettings.pas:66
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    private static StringArray3 s_campaignDifficultyLevelTexts = StringArray3.From(["Easy", "Normal", "Hard"]);
}
