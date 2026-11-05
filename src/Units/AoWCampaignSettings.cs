// Translated from AoWCampaignSettings.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;

namespace Aow.Units.AoWCampaignSettings
{
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
        public void DlgShow(TObject Sender)
        {
            TCampaignDifficultyLevel Level = default;
            DifficultyLevelCB.Items.Clear();
            {
                int first__4 = (int)(TCampaignDifficultyLevel)0;
                int end__1 = (int)(TCampaignDifficultyLevel)2;
                for (int ordinal__3 = first__4; ordinal__3 <= end__1; ordinal__3++)
                {
                    Level = (TCampaignDifficultyLevel)ordinal__3;
                    var call__2 = TranslateRStr(TCampaignDifficultyLevelRStr[(int)Level]);
                    DifficultyLevelCB.Items.AddObject(call__2, IntToObject((int)Level));
                }
            }

            var call__5 = DifficultyLevelCB.Items.IndexOfObject(IntToObject((int)AoWEngine.FCampaign.FDifficultyLevel));
            DifficultyLevelCB.Selected = call__5;
            TurnModeCB.Items.Clear();
            var call__6 = TranslateRStr(TPlayModeRStr[(int)TPlayMode.pmTurn]);
            TurnModeCB.Items.AddObject(call__6, IntToObject((int)TPlayMode.pmTurn));
            var call__7 = TranslateRStr(TPlayModeRStr[(int)TPlayMode.pmSimultaneous]);
            TurnModeCB.Items.AddObject(call__7, IntToObject((int)TPlayMode.pmSimultaneous));
            var call__8 = TurnModeCB.Items.IndexOfObject(IntToObject((int)AoWEngine.FCampaign.FPlayMode));
            TurnModeCB.Selected = call__8;
        }

        // AoWCampaignSettings.pas:59
        public void OKBtnClick()
        {
            var call__9 = DifficultyLevelCB.Items.Objects[DifficultyLevelCB.Selected];
            AoWEngine.FCampaign.FDifficultyLevel = (TCampaignDifficultyLevel)ObjectToInt(call__9);
            var call__10 = TurnModeCB.Items.Objects[TurnModeCB.Selected];
            AoWEngine.FCampaign.FPlayMode = (TPlayMode)ObjectToInt(call__10);
            Dlg.Visible = false;
        }

        // AoWCampaignSettings.pas:66
        public void CancelBtnClick()
        {
            Dlg.Visible = false;
        }
    }

    public static partial class AoWCampaignSettings
    {
    }
}
