#nullable disable warnings
// Translated from AoWCampaignSettings.pas.
using System.Collections.Immutable;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

public sealed partial class CampaignSettingsDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public ComboBox DifficultyLevelComboBox { get; private set; }
    public ComboBox TurnModeComboBox { get; private set; }

    // AoWCampaignSettings.pas:45
    public void DlgShow()
    {
        DifficultyLevelComboBox.Items.Clear();
        for (int levelOrdinal = (int)CampaignDifficultyLevel.Easy; levelOrdinal <= (int)CampaignDifficultyLevel.Hard; levelOrdinal++)
        {
            CampaignDifficultyLevel level = (CampaignDifficultyLevel)levelOrdinal;
            string campaignDifficultyLevel = UI.Engine.TranslateResourceString(s_campaignDifficultyLevelTexts[(int)level]);
            _ = DifficultyLevelComboBox.Items.AddObject(campaignDifficultyLevel, item: IntToObject((int)level));
        }

        int indexOfObject = DifficultyLevelComboBox.Items.IndexOfObject(IntToObject((int)UI.Engine.Campaign.DifficultyLevel));
        DifficultyLevelComboBox.Selected = indexOfObject;
        TurnModeComboBox.Items.Clear();
        string playMode = UI.Engine.TranslateResourceString(GameText.PlayModeTexts[(int)PlayMode.Turn]);
        _ = TurnModeComboBox.Items.AddObject(playMode, item: IntToObject((int)PlayMode.Turn));
        string playModeText = UI.Engine.TranslateResourceString(GameText.PlayModeTexts[(int)PlayMode.Simultaneous]);
        _ = TurnModeComboBox.Items.AddObject(playModeText, item: IntToObject((int)PlayMode.Simultaneous));
        int itemsIndexOfObject = TurnModeComboBox.Items.IndexOfObject(IntToObject((int)UI.Engine.Campaign.PlayMode));
        TurnModeComboBox.Selected = itemsIndexOfObject;
    }

    // AoWCampaignSettings.pas:59
    public void OKBtnClick()
    {
        object? objectValue = DifficultyLevelComboBox.Items.Objects[DifficultyLevelComboBox.Selected];
        UI.Engine.Campaign.DifficultyLevel = (CampaignDifficultyLevel)ObjectToInt(objectValue);
        object? objectValue2 = TurnModeComboBox.Items.Objects[TurnModeComboBox.Selected];
        UI.Engine.Campaign.PlayMode = (PlayMode)ObjectToInt(objectValue2);
        Window.Visible = false;
    }

    // AoWCampaignSettings.pas:66
    public void CancelBtnClick() => Window.Visible = false;

    private static readonly ImmutableArray<string> s_campaignDifficultyLevelTexts = ["Easy", "Normal", "Hard"];
}
