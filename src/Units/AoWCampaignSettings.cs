// Translated from AoWCampaignSettings.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWComboBox;
using static Aow.Units.AoWComboBox.AoWComboBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;

namespace Aow.Units.AoWCampaignSettings
{
    // { TCampaignSettingsDlg (instance size 132, VMT 0041B1B0) }
    public partial class TCampaignSettingsDlg : Aow.Rtl.TDataModule
    {
        public TCampaignSettingsDlg(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Rtl.TIvTranslator Translator;
        // { +0044 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel DifficultyLevelLbl;
        // { +0060 }
        public Aow.Units.AoWComboBox.TAOWComboBox DifficultyLevelCB;
        // { +0064 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar CombatResolveModeSB;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel TurnModeLbl;
        // { +006C }
        public Aow.Units.AoWComboBox.TAOWComboBox TurnModeCB;
        // { +0070 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AOWVScrollBar1;
        // { +0074 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +007C }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // AoWCampaignSettings.pas:45
        // { +0080 }
        public void DlgShow(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TCampaignDifficultyLevel Level = default;
            DifficultyLevelCB.Items.Clear();
            {
                int first__4 = (int)((Aow.Units.AoWE.TCampaignDifficultyLevel)0);
                int end__1 = (int)((Aow.Units.AoWE.TCampaignDifficultyLevel)2);
                for (int ordinal__3 = first__4; ordinal__3 <= end__1; ordinal__3++)
                {
                    Level = (Aow.Units.AoWE.TCampaignDifficultyLevel)ordinal__3;
                    var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TCampaignDifficultyLevelRStr[(int)Level]);
                    DifficultyLevelCB.Items.AddObject(call__2, Aow.Rtl.Rtl.IntToObject((int)Level));
                }
            }

            var call__5 = DifficultyLevelCB.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FDifficultyLevel));
            DifficultyLevelCB.Selected = call__5;
            TurnModeCB.Items.Clear();
            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayModeRStr[(int)Aow.Units.AoWE.TPlayMode.pmTurn]);
            TurnModeCB.Items.AddObject(call__6, Aow.Rtl.Rtl.IntToObject((int)Aow.Units.AoWE.TPlayMode.pmTurn));
            var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayModeRStr[(int)Aow.Units.AoWE.TPlayMode.pmSimultaneous]);
            TurnModeCB.Items.AddObject(call__7, Aow.Rtl.Rtl.IntToObject((int)Aow.Units.AoWE.TPlayMode.pmSimultaneous));
            var call__8 = TurnModeCB.Items.IndexOfObject(Aow.Rtl.Rtl.IntToObject((int)Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FPlayMode));
            TurnModeCB.Selected = call__8;
        }

        // AoWCampaignSettings.pas:59
        public void OKBtnClick()
        {
            var call__9 = DifficultyLevelCB.Items.Objects[DifficultyLevelCB.Selected];
            Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FDifficultyLevel = (Aow.Units.AoWE.TCampaignDifficultyLevel)Aow.Rtl.Rtl.ObjectToInt(call__9);
            var call__10 = TurnModeCB.Items.Objects[TurnModeCB.Selected];
            Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FPlayMode = (Aow.Units.AoWE.TPlayMode)Aow.Rtl.Rtl.ObjectToInt(call__10);
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
