// Generated from AoWScenSetup.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWSetup;
using static Aow.Units.AoWSetup.AoWSetup;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWComboBox;
using static Aow.Units.AoWComboBox.AoWComboBox;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.AoWEmailGameSettingsDlg;
using static Aow.Units.AoWEmailGameSettingsDlg.AoWEmailGameSettingsDlg;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWFileDialog;
using static Aow.Units.AoWFileDialog.AoWFileDialog;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;

namespace Aow.Units.AoWScenSetup
{
    // { TScenSetup (instance size $348, VMT 0040FBF8).
//     $338 map-validation delay; $33C setup control; $340 ping delay; $344 chat scroll pending. }
    public partial class TScenSetup : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWMemo.TAOWMemo ChatMemo;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel ChatPnl;
        // { +0048 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar ChatSB;
        // { +004C }
        public Aow.Units.AoWEdit.TAOWEdit ChatEdit;
        // { +0050 }
        public Aow.Units.AoWPanel.TAOWPanel SidesPnl;
        // { +0054 }
        public Aow.Units.AoWPanel.TAOWPanel SettingsPnl;
        // { +0058 }
        public Aow.Units.AoWListBox.TAOWListBox PlayerList;
        // { +005C }
        public Aow.Units.AoWPanel.TAOWPanel R1Pnl;
        // { +0060 }
        public Aow.Units.AoWPanel.TAOWPanel R5Pnl;
        // { +0064 }
        public Aow.Units.AoWPanel.TAOWPanel R9Pnl;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel R2Pnl;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel R6Pnl;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel R10Pnl;
        // { +0074 }
        public Aow.Units.AoWPanel.TAOWPanel R3Pnl;
        // { +0078 }
        public Aow.Units.AoWPanel.TAOWPanel R7Pnl;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel R11Pnl;
        // { +0080 }
        public Aow.Units.AoWPanel.TAOWPanel R4Pnl;
        // { +0084 }
        public Aow.Units.AoWPanel.TAOWPanel R8Pnl;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel R12Pnl;
        // { +008C }
        public Aow.Units.AoWButton.TAOWButton ExitGame;
        // { +0090 }
        public Aow.Units.AoWButton.TAOWButton PickScenarioBtn;
        // { +0094 }
        public Aow.Units.AoWButton.TAOWButton StartBtn;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel MiscLbl;
        // { +009C }
        public Aow.Units.AoWPanel.TAOWPanel ScenSetupPnl;
        // { +00A0 }
        public Aow.Units.AoWImage.TAOWImage R1PlayerIcon;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel R1Lbl;
        // { +00A8 }
        public Aow.Units.AoWComboBox.TAOWComboBox R1PlayerCB;
        // { +00AC }
        public Aow.Units.AoWPanel.TAOWPanel R1CenterPnl;
        // { +00B0 }
        public Aow.Units.AoWLabel.TAOWLabel TurnLbl;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel DurationLbl;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel TurnsLbl;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel TurnTimeLbl;
        // { +00C0 }
        public Aow.Units.AoWHScroll.TAOWHScrollBar TurnTimeSl;
        // { +00C4 }
        public Aow.Units.AoWLabel.TAOWLabel TurnTimeVal;
        // { +00C8 }
        public Aow.Units.AoWFrame.TAoWFrame DurFr;
        // { +00CC }
        public Aow.Units.AoWHScroll.TAOWHScrollBar TurnsSl;
        // { +00D0 }
        public Aow.Units.AoWLabel.TAOWLabel TurnsVal;
        // { +00D4 }
        public Aow.Units.AoWComboBox.TAOWComboBox PlayModeCB;
        // { +00D8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar TurnVSB;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel R1InfoLbl;
        // { +00E0 }
        public Aow.Units.AoWComboBox.TAOWComboBox R1BehaviorCB;
        // { +00E4 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R1BehaviorSB;
        // { +00E8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R1PlayerSB;
        // { +00EC }
        public Aow.Units.AoWPanel.TAOWPanel R3CenterPnl;
        // { +00F0 }
        public Aow.Units.AoWLabel.TAOWLabel R3Lbl;
        // { +00F4 }
        public Aow.Units.AoWComboBox.TAOWComboBox R3BehaviorCB;
        // { +00F8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R3BehaviorSB;
        // { +00FC }
        public Aow.Units.AoWImage.TAOWImage R3PlayerIcon;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel R3PlayerLbl;
        // { +0104 }
        public Aow.Units.AoWComboBox.TAOWComboBox R3PlayerCB;
        // { +0108 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R3PlayerSB;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel R3InfoLbl;
        // { +0110 }
        public Aow.Units.AoWPanel.TAOWPanel R2CenterPnl;
        // { +0114 }
        public Aow.Units.AoWLabel.TAOWLabel R2Lbl;
        // { +0118 }
        public Aow.Units.AoWComboBox.TAOWComboBox R2BehaviorCB;
        // { +011C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R2BehaviorSB;
        // { +0120 }
        public Aow.Units.AoWImage.TAOWImage R2PlayerIcon;
        // { +0124 }
        public Aow.Units.AoWLabel.TAOWLabel R2InfoLbl;
        // { +0128 }
        public Aow.Units.AoWComboBox.TAOWComboBox R2PlayerCB;
        // { +012C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R2PlayerSB;
        // { +0130 }
        public Aow.Units.AoWLabel.TAOWLabel R2PlayerLbl;
        // { +0134 }
        public Aow.Units.AoWLabel.TAOWLabel R4InfoLbl;
        // { +0138 }
        public Aow.Units.AoWLabel.TAOWLabel R4PlayerLbl;
        // { +013C }
        public Aow.Units.AoWPanel.TAOWPanel R4CenterPnl;
        // { +0140 }
        public Aow.Units.AoWLabel.TAOWLabel R4Lbl;
        // { +0144 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R4BehaviorSB;
        // { +0148 }
        public Aow.Units.AoWComboBox.TAOWComboBox R4PlayerCB;
        // { +014C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R4PlayerSB;
        // { +0150 }
        public Aow.Units.AoWImage.TAOWImage R4PlayerIcon;
        // { +0154 }
        public Aow.Units.AoWImage.TAOWImage R5PlayerIcon;
        // { +0158 }
        public Aow.Units.AoWLabel.TAOWLabel R5Lbl;
        // { +015C }
        public Aow.Units.AoWComboBox.TAOWComboBox R5PlayerCB;
        // { +0160 }
        public Aow.Units.AoWPanel.TAOWPanel R5CenterPnl;
        // { +0164 }
        public Aow.Units.AoWComboBox.TAOWComboBox R5BehaviorCB;
        // { +0168 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R5BehaviorSB;
        // { +016C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R5PlayerSB;
        // { +0170 }
        public Aow.Units.AoWLabel.TAOWLabel R5InfoLbl;
        // { +0174 }
        public Aow.Units.AoWLabel.TAOWLabel R5PlayerLbl;
        // { +0178 }
        public Aow.Units.AoWPanel.TAOWPanel R6CenterPnl;
        // { +017C }
        public Aow.Units.AoWLabel.TAOWLabel R6Lbl;
        // { +0180 }
        public Aow.Units.AoWComboBox.TAOWComboBox R6BehaviorCB;
        // { +0184 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R6BehaviorSB;
        // { +0188 }
        public Aow.Units.AoWImage.TAOWImage R6PlayerIcon;
        // { +018C }
        public Aow.Units.AoWLabel.TAOWLabel R6PlayerLbl;
        // { +0190 }
        public Aow.Units.AoWComboBox.TAOWComboBox R6PlayerCB;
        // { +0194 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R6PlayerSB;
        // { +0198 }
        public Aow.Units.AoWLabel.TAOWLabel R6InfoLbl;
        // { +019C }
        public Aow.Units.AoWPanel.TAOWPanel R7CenterPnl;
        // { +01A0 }
        public Aow.Units.AoWLabel.TAOWLabel R7Lbl;
        // { +01A4 }
        public Aow.Units.AoWComboBox.TAOWComboBox R7BehaviorCB;
        // { +01A8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R7BehaviorSB;
        // { +01AC }
        public Aow.Units.AoWImage.TAOWImage R7PlayerIcon;
        // { +01B0 }
        public Aow.Units.AoWLabel.TAOWLabel R7PlayerLbl;
        // { +01B4 }
        public Aow.Units.AoWComboBox.TAOWComboBox R7PlayerCB;
        // { +01B8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R7PlayerSB;
        // { +01BC }
        public Aow.Units.AoWLabel.TAOWLabel R7InfoLbl;
        // { +01C0 }
        public Aow.Units.AoWLabel.TAOWLabel R8InfoLbl;
        // { +01C4 }
        public Aow.Units.AoWLabel.TAOWLabel R8PlayerLbl;
        // { +01C8 }
        public Aow.Units.AoWPanel.TAOWPanel R8CenterPnl;
        // { +01CC }
        public Aow.Units.AoWLabel.TAOWLabel R8Lbl;
        // { +01D0 }
        public Aow.Units.AoWComboBox.TAOWComboBox R8BehaviorCB;
        // { +01D4 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R8BehaviorSB;
        // { +01D8 }
        public Aow.Units.AoWComboBox.TAOWComboBox R8PlayerCB;
        // { +01DC }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R8PlayerSB;
        // { +01E0 }
        public Aow.Units.AoWImage.TAOWImage R8PlayerIcon;
        // { +01E4 }
        public Aow.Units.AoWImage.TAOWImage R9PlayerIcon;
        // { +01E8 }
        public Aow.Units.AoWLabel.TAOWLabel R9Lbl;
        // { +01EC }
        public Aow.Units.AoWComboBox.TAOWComboBox R9PlayerCB;
        // { +01F0 }
        public Aow.Units.AoWPanel.TAOWPanel R9CenterPnl;
        // { +01F4 }
        public Aow.Units.AoWComboBox.TAOWComboBox R9BehaviorCB;
        // { +01F8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R9BehaviorSB;
        // { +01FC }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R9PlayerSB;
        // { +0200 }
        public Aow.Units.AoWLabel.TAOWLabel R9InfoLbl;
        // { +0204 }
        public Aow.Units.AoWLabel.TAOWLabel R9PlayerLbl;
        // { +0208 }
        public Aow.Units.AoWPanel.TAOWPanel R10CenterPnl;
        // { +020C }
        public Aow.Units.AoWLabel.TAOWLabel R10Lbl;
        // { +0210 }
        public Aow.Units.AoWComboBox.TAOWComboBox R10BehaviorCB;
        // { +0214 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R10BehaviorSB;
        // { +0218 }
        public Aow.Units.AoWImage.TAOWImage R10PlayerIcon;
        // { +021C }
        public Aow.Units.AoWLabel.TAOWLabel R10PlayerLbl;
        // { +0220 }
        public Aow.Units.AoWComboBox.TAOWComboBox R10PlayerCB;
        // { +0224 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar s10sb2;
        // { +0228 }
        public Aow.Units.AoWLabel.TAOWLabel R10InfoLbl;
        // { +022C }
        public Aow.Units.AoWPanel.TAOWPanel R11CenterPnl;
        // { +0230 }
        public Aow.Units.AoWLabel.TAOWLabel R11Lbl;
        // { +0234 }
        public Aow.Units.AoWComboBox.TAOWComboBox R11BehaviorCB;
        // { +0238 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R11BehaviorSB;
        // { +023C }
        public Aow.Units.AoWImage.TAOWImage R11PlayerIcon;
        // { +0240 }
        public Aow.Units.AoWLabel.TAOWLabel R11PlayerLbl;
        // { +0244 }
        public Aow.Units.AoWComboBox.TAOWComboBox R11PlayerCB;
        // { +0248 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R11PlayerSB;
        // { +024C }
        public Aow.Units.AoWLabel.TAOWLabel R11InfoLbl;
        // { +0250 }
        public Aow.Units.AoWLabel.TAOWLabel R12InfoLbl;
        // { +0254 }
        public Aow.Units.AoWLabel.TAOWLabel R12PlayerLbl;
        // { +0258 }
        public Aow.Units.AoWPanel.TAOWPanel R12CenterPnl;
        // { +025C }
        public Aow.Units.AoWLabel.TAOWLabel R12Lbl;
        // { +0260 }
        public Aow.Units.AoWComboBox.TAOWComboBox R12BehaviorCB;
        // { +0264 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R12BehaviorSB;
        // { +0268 }
        public Aow.Units.AoWComboBox.TAOWComboBox R12PlayerCB;
        // { +026C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar R12PlayerSB;
        // { +0270 }
        public Aow.Units.AoWImage.TAOWImage R12PlayerIcon;
        // { +0274 }
        public Aow.Units.AoWComboBox.TAOWComboBox PreferredRaceCB;
        // { +0278 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar PreferredRaceSB;
        // { +027C }
        public Aow.Units.AoWLabel.TAOWLabel PreferredRaceLbl;
        // { +0280 }
        public Aow.Units.AoWFrame.TAoWFrame LFr;
        // { +0284 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox CustomizeLeaders;
        // { +0288 }
        public Aow.Units.AoWLabel.TAOWLabel CustomizeLeadersLbl;
        // { +028C }
        public Aow.Units.AoWButton.TAOWButton EjectBtn;
        // { +0290 }
        public Aow.Units.AoWComboBox.TAOWComboBox R4BehaviorCB;
        // { +0294 }
        public Aow.Units.AoWPanel.TAOWPanel R1FogPnl;
        // { +0298 }
        public Aow.Units.AoWLabel.TAOWLabel R1PlayerLbl;
        // { +029C }
        public Aow.Units.AoWPanel.TAOWPanel R2FogPnl;
        // { +02A0 }
        public Aow.Units.AoWPanel.TAOWPanel R3FogPnl;
        // { +02A4 }
        public Aow.Units.AoWPanel.TAOWPanel R4FogPnl;
        // { +02A8 }
        public Aow.Units.AoWPanel.TAOWPanel R8FogPnl;
        // { +02AC }
        public Aow.Units.AoWPanel.TAOWPanel R7FogPnl;
        // { +02B0 }
        public Aow.Units.AoWPanel.TAOWPanel R6FogPnl;
        // { +02B4 }
        public Aow.Units.AoWPanel.TAOWPanel R5FogPnl;
        // { +02B8 }
        public Aow.Units.AoWPanel.TAOWPanel R12FogPnl;
        // { +02BC }
        public Aow.Units.AoWPanel.TAOWPanel R11FogPnl;
        // { +02C0 }
        public Aow.Units.AoWPanel.TAOWPanel R10FogPnl;
        // { +02C4 }
        public Aow.Units.AoWPanel.TAOWPanel R9FogPnl;
        // { +02C8 }
        public Aow.Units.AoWButton.TAOWButton PreferredRaceSort;
        // { +02CC }
        public Aow.Units.AoWButton.TAOWButton PlayerListSort;
        // { +02D0 }
        public Aow.Units.AoWPanel.TAOWPanel ConnectionPnl;
        // { +02D4 }
        public Aow.Units.AoWListBox.TAOWListBox PreferredRaceList;
        // { +02D8 }
        public Aow.Units.AoWButton.TAOWButton AOWButton1;
        // { +02DC }
        public Aow.Units.AoWListBox.TAOWListBox PingList;
        // { +02E0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar PlayerListSB;
        // { +02E4 }
        public Aow.Units.AoWPanel.TAOWPanel SettingsFogPnl;
        // { +02E8 }
        public Aow.Units.AoWLabel.TAOWLabel ExplorationLbl;
        // { +02EC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox Exploration;
        // { +02F0 }
        public Aow.Units.AoWLabel.TAOWLabel ScenarioLbl;
        // { +02F4 }
        public Aow.Units.AoWLabel.TAOWLabel HostNameLbl;
        // { +02F8 }
        public Aow.Units.AoWLabel.TAOWLabel PlayerNameLbl;
        // { +02FC }
        public Aow.Units.AoWLabel.TAOWLabel FileNameLbl;
        // { +0300 }
        public Aow.Units.AoWButton.TAOWButton MakeHostBtn;
        // { +0304 }
        public Aow.Units.AoWPanel.TAOWPanel RestartSettingsFogPnl;
        // { +0308 }
        public Aow.Units.AoWImage.TAOWImage ChatFogPnl;
        // { +030C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LeadersOnMapCB;
        // { +0310 }
        public Aow.Units.AoWLabel.TAOWLabel AOWLabel1;
        // { +0314 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox AlliedVictoryCB;
        // { +0318 }
        public Aow.Units.AoWLabel.TAOWLabel AlliedVictoryLbl;
        // { +031C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0320 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AOWVScrollBar1;
        // { +0324 }
        public Aow.Units.AoWComboBox.TAOWComboBox TacticalCombatCB;
        // { +0328 }
        public Aow.Units.AoWLabel.TAOWLabel TacticalCombatLbl;
        // { +032C }
        public Aow.Units.AoWCheckBox.TAOWCheckBox AlwaysViewTCCB;
        // { +0330 }
        public Aow.Units.AoWLabel.TAOWLabel AlwaysViewTCLbl;
        // { +0334 }
        public int FValidateDelay;
        // { $338 }
        public Aow.Units.AoWSetup.TSetupControl FSetupControl;
        // { $33C }
        public int FPingDelay;
        // { $340 }
        public bool FChatScrollPending;
        // { $344 }
        public Aow.Units.FixedArray_byte_3 F_ScenSetup_345;
        // AoWScenSetup.pas:526
        public void Execute(Aow.Units.AoWSetup.TSetupControl Control)
        {
            byte Availability = default;
            FSetupControl = Control;
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = true;
            ScenSetupPnl.Visible = true;
            ChatMemo.Items.Clear();
            ChatEdit.Text = (AnsiString)"";
            FValidateDelay = 100;
            FPingDelay = 15;
            var call__1 = FSetupControl.GetRemote();
            ChatFogPnl.Visible = !call__1;
            var call__2 = FSetupControl.GetRemote();
            ChatMemo.FCaptureInput = call__2;
            var call__3 = FSetupControl.GetRemote();
            ChatEdit.FCaptureInput = call__3;
            var call__4 = FSetupControl.GetHost();
            if (call__4 && (FSetupControl.FSettings.FMapName == ((AnsiString)"")))
            {
                PickMap();
            }

            var call__5 = FSetupControl.GetRemote();
            if (call__5)
            {
                TacticalCombatCB.Visible = true;
                TacticalCombatLbl.Visible = true;
                TacticalCombatCB.Items.Clear();
                {
                    int first__9 = (int)0;
                    int end__6 = (int)2;
                    for (int ordinal__8 = first__9; ordinal__8 <= end__6; ordinal__8++)
                    {
                        Availability = (byte)ordinal__8;
                        var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TTacticalCombatAvailabilityRStr[Availability]);
                        TacticalCombatCB.Items.AddObject(call__7, Aow.Rtl.Rtl.IntToObject((int)Availability));
                    }
                }
            }
            else
            {
                TacticalCombatCB.Visible = false;
                TacticalCombatLbl.Visible = false;
            }

            if (FSetupControl.FSettings.FConnectionType == 2)
            {
                CustomizeLeadersLbl.TextColor = 8421504;
                CustomizeLeaders.FCaptureInput = false;
                PlayModeCB.Items.Clear();
                var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                PlayModeCB.Items.AddObject(call__10, Aow.Rtl.Rtl.IntToObject((int)1));
            }
            else
            {
                CustomizeLeadersLbl.TextColor = AlliedVictoryLbl.TextColor;
                CustomizeLeaders.FCaptureInput = true;
                PlayModeCB.Items.Clear();
                var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SimultaneousRStr);
                PlayModeCB.Items.AddObject(call__11, Aow.Rtl.Rtl.IntToObject((int)2));
                var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ClassicRStr);
                PlayModeCB.Items.AddObject(call__12, Aow.Rtl.Rtl.IntToObject((int)1));
            }

            bool bool__13 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex == -1);
            if (!bool__13)
            {
                var call__14 = Aow.Units.Sound.Sound.SongPlayer.Songs.Count;
                bool__13 = (Aow.Units.Sound.Sound.SongPlayer.CurrentIndex >= call__14);
            }

            bool bool__15 = bool__13;
            if (!bool__15)
            {
                var call__16 = Aow.Units.Sound.Sound.SongPlayer.Songs[Aow.Units.Sound.Sound.SongPlayer.CurrentIndex];
                bool__15 = (call__16 != ((AnsiString)"SONGS\\TITLE.IT"));
            }

            if (bool__15)
            {
                Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
                Aow.Units.Sound.Sound.SongPlayer.Songs.Add((AnsiString)"SONGS\\TITLE.IT");
                Aow.Units.Sound.Sound.SongPlayer.Play(0);
            }

            Aow.Units.E2GameFlow.E2GameFlow.UpdateScenarioSetup(this);
        }

        // AoWScenSetup.pas:579
        public void CloseSetup()
        {
            ScenSetupPnl.Visible = false;
            FSetupControl = null;
        }

        // AoWScenSetup.pas:584
        public void PickMap()
        {
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FMode = 2;
            Aow.Units.AoWFileDialog.AoWFileDialog.FileDlgWindow.FileDialog.Visible = true;
        }

        // AoWScenSetup.pas:257
        public void ExitGameClick(Aow.Rtl.TObject Sender)
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.Connected */
            throw new System.NotImplementedException();
        }

        // AoWScenSetup.pas:281
        // { conflict: procedure ExitGameClick; }
        public void ChatEditReturn()
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.PlayerName */
            throw new System.NotImplementedException();
        }

        // AoWScenSetup.pas:295
        public void PlayerListDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int I = default;
            var call__17 = PlayerList.Items.Count;
            if (DrawItem < call__17)
            {
                var call__18 = PlayerList.Items.Objects[DrawItem];
                var call__19 = FSetupControl.FSettings.FRemotePlayers.IndexOfRemoteID(Aow.Rtl.Rtl.ObjectToInt(call__18));
                I = call__19;
                if (I != -1)
                {
                    var call__20 = FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                    if (call__20.FMapValidation == Aow.Units.AoWSetup.TMapValidationInfo.mviInvalid)
                    {
                        Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.Color = 255;
                        Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, AnsiString.FromBytes(88), SD);
                    }
                    else
                    {
                        var call__21 = FSetupControl.FSettings.FRemotePlayers.GetPlayers(I);
                        if (call__21.FMapValidation == Aow.Units.AoWSetup.TMapValidationInfo.mviInvalidMap)
                        {
                            Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.Color = 32768;
                            Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s.SetText(DrawRect.Right - 8, DrawRect.Top + 4, AnsiString.FromBytes(88), SD);
                        }
                    }
                }
            }
        }

        // AoWScenSetup.pas:316
        public void PingListDrawItem(Aow.Units.AoWComp.TAoWComponent Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, ref Aow.Units.GFXE.TSurfaceDesc SD)
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.GetPlayerIndex */
            throw new System.NotImplementedException();
        }

        // AoWScenSetup.pas:344
        public void ScenSetupPnlUpdate()
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.Connected */
            throw new System.NotImplementedException();
        }

        // AoWScenSetup.pas:358
        public void PickScenarioBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            PickMap();
        }

        // AoWScenSetup.pas:364
        // { draft }
        public void PreferredRaceCBChange()
        {
            if (PreferredRaceCB.Selected >= 0)
            {
                var call__22 = PreferredRaceCB.Items.Objects[PreferredRaceCB.Selected];
                FSetupControl.SetPreferredRace(unchecked((byte)Aow.Rtl.Rtl.ObjectToInt(call__22)));
            }
        }

        // AoWScenSetup.pas:370
        public void StartBtnClick()
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.DeferredSetupAction */
            throw new System.NotImplementedException();
        }

        // AoWScenSetup.pas:378
        public void EjectBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (PlayerList.Selected > -1)
            {
                var call__23 = PlayerList.Items.Objects[PlayerList.Selected];
                FSetupControl.hEjectPlayer(Aow.Rtl.Rtl.ObjectToInt(call__23));
            }
        }

        // AoWScenSetup.pas:385
        public void MakeHostBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (PlayerList.Selected > -1)
            {
                var call__24 = PlayerList.Items.Objects[PlayerList.Selected];
                FSetupControl.hSetHostRemoteID(Aow.Rtl.Rtl.ObjectToInt(call__24));
            }
        }

        // AoWScenSetup.pas:392
        public void PlayerListSBChange()
        {
            PlayerList.ListOffset = PlayerListSB.Position;
            PingList.ListOffset = PlayerListSB.Position;
            PreferredRaceList.ListOffset = PlayerListSB.Position;
        }

        // AoWScenSetup.pas:399
        public void PlayerListChange(Aow.Rtl.TObject Sender)
        {
            SelectPlayer(((Aow.Units.AoWListBox.TAOWListBox)Sender).Selected);
        }

        // AoWScenSetup.pas:404
        public void TurnTimeSlChange()
        {
            FSetupControl.hSetTurnTimeLimitation(TurnTimeSl.Position);
            UpdateTurnTime();
        }

        // AoWScenSetup.pas:409
        public void TurnsCBChange(Aow.Rtl.TObject Sender)
        {
            TurnsSlChange(this);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:415
        public void TurnsSlChange(Aow.Rtl.TObject Sender)
        {
            if (TurnsSl.Position == 0)
            {
                FSetupControl.hSetTurnLimitation(TurnsSl.Position);
            }
            else
            {
                FSetupControl.hSetTurnLimitation(TurnsSl.Position * 10);
            }

            UpdateTurnLimit();
        }

        // AoWScenSetup.pas:422
        public void PlayModeCBChange()
        {
            var call__25 = PlayModeCB.Items.Objects[PlayModeCB.Selected];
            FSetupControl.hSetPlayMode(unchecked((byte)Aow.Rtl.Rtl.ObjectToInt(call__25)));
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:428
        public void RacePlayerCBChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWSetup.TSetupPlayerBehavior Behavior = default;
            Aow.Units.AoWComboBox.TAOWComboBox Combo = default;
            Combo = (Aow.Units.AoWComboBox.TAOWComboBox)Sender;
            var call__26 = Combo.Items[Combo.Selected];
            var call__27 = FSetupControl.PlayerNameToPlayerID(call__26, ref Behavior);
            if (call__27)
            {
                var call__28 = FSetupControl.FSettings.IndexOfRace(unchecked((byte)unchecked((byte)Combo.Tag)));
                FSetupControl.hSetPlayerID(call__28, Behavior, true);
            }

            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:437
        public void RaceBehaviorCBChange(Aow.Units.AoWComboBox.TAOWComboBox Sender)
        {
            Aow.Units.AoWComboBox.TAOWComboBox Control = default;
            Control = Sender;
            var call__29 = FSetupControl.FSettings.IndexOfRace(unchecked((byte)unchecked((byte)Control.Tag)));
            FSetupControl.hSetPlayerBehavior(call__29, unchecked((byte)Control.Selected));
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWScenSetup.pas:444
        public void LeadersOnMapCBChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetLeadersOnMap(LeadersOnMapCB.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:450
        public void CustomizeLeadersChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetCustomizeLeaders(CustomizeLeaders.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:456
        public void AlliedVictoryCBChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetAlliedVictory(AlliedVictoryCB.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:462
        public void ExplorationChange(Aow.Rtl.TObject Sender)
        {
            FSetupControl.hSetExplorationEnabled(Exploration.Checked);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            }
            else
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(2);
            }
        }

        // AoWScenSetup.pas:468
        public void AlwaysViewTCCBChange()
        {
            FSetupControl.hSetAlwaysViewTC(AlwaysViewTCCB.Checked);
        }

        // AoWScenSetup.pas:473
        public void SettingsClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWScenSetup.pas:478
        public void RacePnlDrawBack(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            {
                var w__30 = (Aow.Units.AoWImage.TAOWImage)Sender;
                var call__31 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields[Sender.Tag];
                call__31.Show(w__30.WinLeft + 5, w__30.WinTop + 5, SD);
            }
        }

        // AoWScenSetup.pas:483
        public void ChatSBUpdate()
        {
            if (FChatScrollPending)
            {
                ChatSB.Position = (ChatSB.Max + 1) - ChatSB.NumberVisible;
                FChatScrollPending = false;
            }
        }

        // AoWScenSetup.pas:492
        public void TacticalCombatCBChange()
        {
            var call__32 = TacticalCombatCB.Items.Objects[TacticalCombatCB.Selected];
            FSetupControl.hSetTacticalCombatAvailability(unchecked((byte)Aow.Rtl.Rtl.ObjectToInt(call__32)));
        }

        // AoWScenSetup.pas:498
        public void UpdateTurnTime()
        {
            if (TurnTimeSl.Position == 0)
            {
                var call__33 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoTimerRStr);
                TurnTimeVal.Text = Aow.Rtl.Rtl.LowerCase(call__33);
            }
            else
            {
                var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XMinutesRStr);
                var call__35 = Aow.Units.AoWE.AoWE.Format(call__34, [Aow.Rtl.Rtl.IntToStr(TurnTimeSl.Position)]);
                TurnTimeVal.Text = call__35;
            }
        }

        // AoWScenSetup.pas:504
        public void UpdateTurnLimit()
        {
            if (TurnsSl.Position == 0)
            {
                var call__36 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoLimitRStr);
                TurnsVal.Text = Aow.Rtl.Rtl.LowerCase(call__36);
            }
            else
            {
                var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XTurnsRStr);
                var call__38 = Aow.Units.AoWE.AoWE.Format(Aow.Rtl.Rtl.LowerCase(call__37), [Aow.Rtl.Rtl.IntToStr(TurnsSl.Position * 10)]);
                TurnsVal.Text = call__38;
            }
        }

        // AoWScenSetup.pas:510
        public void SelectPlayer(int Index)
        {
            bool bool__39 = (Index >= 0);
            if (bool__39)
            {
                var call__40 = PlayerList.Items.Count;
                bool__39 = (Index < call__40);
            }

            if (bool__39)
            {
                PlayerList.Selected = Index;
                PingList.Selected = Index;
                PreferredRaceList.Selected = Index;
                var call__41 = FSetupControl.GetHost();
                bool bool__42 = call__41;
                if (bool__42)
                {
                    var call__43 = PlayerList.Items.Objects[PlayerList.Selected];
                    bool__42 = (Aow.Rtl.Rtl.ObjectToInt(call__43) != FSetupControl.FRemotePlayer.FRemoteID);
                }

                EjectBtn.Visible = bool__42;
                var call__44 = PlayerList.Items.Objects[PlayerList.Selected];
                var call__45 = FSetupControl.CanSetHostRemoteID(Aow.Rtl.Rtl.ObjectToInt(call__44));
                MakeHostBtn.Visible = call__45;
            }
            else
            {
                PlayerList.Selected = -1;
                PingList.Selected = -1;
                PreferredRaceList.Selected = -1;
                EjectBtn.Visible = false;
                MakeHostBtn.Visible = false;
            }
        }
    }

    public static partial class AoWScenSetup
    {
    }
}
