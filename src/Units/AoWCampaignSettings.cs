// Translated from AoWCampaignSettings.pas.
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWCampaignSettings.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
            var campaignDifficultyLevel = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.TCampaignSettingsDlg.TCampaignDifficultyLevelRStr[(int)Level]);
            DifficultyLevelCB.Items.AddObject(campaignDifficultyLevel, IntToObject((int)Level));
        }

        var indexOfObject = DifficultyLevelCB.Items.IndexOfObject(IntToObject((int)Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FDifficultyLevel));
        DifficultyLevelCB.Selected = indexOfObject;
        TurnModeCB.Items.Clear();
        var playMode = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TPlayModeRStr[(int)TPlayMode.pmTurn]);
        TurnModeCB.Items.AddObject(playMode, IntToObject((int)TPlayMode.pmTurn));
        var playModeText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.TPlayModeRStr[(int)TPlayMode.pmSimultaneous]);
        TurnModeCB.Items.AddObject(playModeText, IntToObject((int)TPlayMode.pmSimultaneous));
        var itemsIndexOfObject = TurnModeCB.Items.IndexOfObject(IntToObject((int)Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FPlayMode));
        TurnModeCB.Selected = itemsIndexOfObject;
    }

    // AoWCampaignSettings.pas:59
    public void OKBtnClick()
    {
        var objectValue = DifficultyLevelCB.Items.Objects[DifficultyLevelCB.Selected];
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FDifficultyLevel = (TCampaignDifficultyLevel)ObjectToInt(objectValue);
        var objectValue2 = TurnModeCB.Items.Objects[TurnModeCB.Selected];
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign.FPlayMode = (TPlayMode)ObjectToInt(objectValue2);
        Dlg.Visible = false;
    }

    // AoWCampaignSettings.pas:66
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    private static global::Aow.Game.Engine.Buffers.FixedArray_string_3 TCampaignDifficultyLevelRStr = global::Aow.Game.Engine.Buffers.FixedArray_string_3.From(["Easy", "Normal", "Hard"]);
}
