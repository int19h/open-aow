// Translated from AoWCampaignSettings.pas.
using System.Collections.Immutable;
using Aow.Engine.Buffers;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class CampaignSettingsDialog : UIModule
{
    public CampaignSettingsDialog(UIContext ui) : base(ui)
    { }

    public Window Window;
    public ComboBox DifficultyLevelComboBox;
    public ComboBox TurnModeComboBox;

    // AoWCampaignSettings.pas:45
    public void DlgShow()
    {
        CampaignDifficultyLevel level;
        DifficultyLevelComboBox.Items.Clear();
        for (int levelOrdinal = (int)(CampaignDifficultyLevel)0; levelOrdinal <= (int)(CampaignDifficultyLevel)2; levelOrdinal++)
        {
            level = (CampaignDifficultyLevel)levelOrdinal;
            var campaignDifficultyLevel = UI.Engine.TranslateResourceString(s_campaignDifficultyLevelTexts[(int)level]);
            DifficultyLevelComboBox.Items.AddObject(campaignDifficultyLevel, IntToObject((int)level));
        }

        var indexOfObject = DifficultyLevelComboBox.Items.IndexOfObject(IntToObject((int)UI.Engine.Campaign.DifficultyLevel));
        DifficultyLevelComboBox.Selected = indexOfObject;
        TurnModeComboBox.Items.Clear();
        var playMode = UI.Engine.TranslateResourceString(GameText.PlayModeTexts[(int)PlayMode.Turn]);
        TurnModeComboBox.Items.AddObject(playMode, IntToObject((int)PlayMode.Turn));
        var playModeText = UI.Engine.TranslateResourceString(GameText.PlayModeTexts[(int)PlayMode.Simultaneous]);
        TurnModeComboBox.Items.AddObject(playModeText, IntToObject((int)PlayMode.Simultaneous));
        var itemsIndexOfObject = TurnModeComboBox.Items.IndexOfObject(IntToObject((int)UI.Engine.Campaign.PlayMode));
        TurnModeComboBox.Selected = itemsIndexOfObject;
    }

    // AoWCampaignSettings.pas:59
    public void OKBtnClick()
    {
        var objectValue = DifficultyLevelComboBox.Items.Objects[DifficultyLevelComboBox.Selected];
        UI.Engine.Campaign.DifficultyLevel = (CampaignDifficultyLevel)ObjectToInt(objectValue);
        var objectValue2 = TurnModeComboBox.Items.Objects[TurnModeComboBox.Selected];
        UI.Engine.Campaign.PlayMode = (PlayMode)ObjectToInt(objectValue2);
        Window.Visible = false;
    }

    // AoWCampaignSettings.pas:66
    public void CancelBtnClick()
    {
        Window.Visible = false;
    }

    private static readonly ImmutableArray<string> s_campaignDifficultyLevelTexts = ["Easy", "Normal", "Hard"];
}
