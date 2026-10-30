// Translated from AOWDialogWin.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;

namespace Aow.Units.AOWDialogWin
{
    // { TDWin (instance size 176, VMT 00450334) }
    public partial class TDWin : Aow.Rtl.TDataModule
    {
        public TDWin(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWWindow.TAOWWindow Dialog;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel DTitle;
        // { +0048 }
        public Aow.Units.AoWLabel.TAOWLabel DMessage;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton DButton;
        // { +0050 }
        public Aow.Units.AoWWindow.TAOWWindow DebugWin;
        // { +0054 }
        public Aow.Units.AoWLabel.TAOWLabel DebugDTitle;
        // { +0058 }
        public Aow.Units.AoWButton.TAOWButton DebugDOkBtn;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel FrameLbl;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel DebugDfrTitle;
        // { +0064 }
        public Aow.Units.AoWMemo.TAOWMemo DebugMemo;
        // { +0068 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar DMemoScroll;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel okPnl;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel yesPnl;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton yesBtn;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton noBtn;
        // { +007C }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0080 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +0084 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0088 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +008C }
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        // { +0090 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +0094 }
        public Aow.Units.AoWFrame.TAoWFrame SignBLine;
        // { +0098 }
        public Aow.Units.AoWImage.TAOWImage SIcon;
        // { +009C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +00A0 }
        public byte FAction;
        // { $A4; 004506E7 }
        public Aow.Units.FixedArray_byte_8 F_DWin_A5;
        public bool FSkipCombatChoice;
        // { $AD; 00439AEC }
        public Aow.Units.FixedArray_byte_2 F_DWin_AE;
        // AOWDialogWin.pas:121
        public void Confirm(Aow.Rtl.AnsiString Title, Aow.Rtl.AnsiString Text, byte Action)
        {
            FAction = unchecked((byte)Action);
            DTitle.Text = Title;
            DMessage.Text = Text;
            yesPnl.Visible = true;
            okPnl.Visible = false;
            Dialog.Visible = true;
        }

        // AOWDialogWin.pas:113
        public void AddDebugMessage(Aow.Rtl.AnsiString Text)
        {
            DebugMemo.Items.Add(Text);
            var call__1 = DebugMemo.Items.Count;
            if (call__1 > 99)
            {
                DebugMemo.Items.Delete(0);
            }

            DebugMemo.FSized = false;
            DMemoScroll.Position = DMemoScroll.Max;
        }

        // AOWDialogWin.pas:60
        public void DButtonClick(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dialog.Visible = false;
        }

        // AOWDialogWin.pas:67
        public void DebugDOkBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            DebugWin.Visible = false;
        }

        // AOWDialogWin.pas:73
        // { draft }
        public void yesBtnClick(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TStructure Structure = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            switch (FAction)
            {
                case 0:
                case 1:
                    ;
                    break;
                case 2:
                    Aow.Units.AoWWinMan.AoWWinMan.General.FCity.Loot();
                    break;
                case 3:
                    var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.FCity.CanRaze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), ref Reason);
                    if (call__2)
                    {
                        var call__3 = Aow.Units.AoWWinMan.AoWWinMan.General.FCity.Raze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        if (call__3)
                        {
                            Structure = Aow.Units.AoWWinMan.AoWWinMan.General.FStructure;
                            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapStructureDeselected(Aow.Units.AoWWinMan.AoWWinMan.General.FStructure);
                            Aow.Units.aowMapEvents.aowMapEvents.MapEvents.TheMapStructureSelected(Structure);
                        }
                    }
                    else
                    {
                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                    }

                    break;
                case 4:
                    var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                    call__4.Surrender();
                    break;
                case 5:
                    Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap.WorldPnl.Visible = false;
                    Aow.Units.AoWWinMan.AoWWinMan.General.ReturnToTitle();
                    break;
                case 6:
                    if (Aow.Units.AoWTC.AoWTC.AoWCombatMap != null)
                    {
                        Aow.Units.AoWTC.AoWTC.AoWCombatMap.Surrender(unchecked((sbyte)Aow.Units.AoWTC.AoWTC.AoWCombatMap.FSeatedCombatPlayer));
                    }

                    break;
                case 7:
                    if (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit != null)
                    {
                        var call__5 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.CanDisband(ref Reason);
                        if (call__5)
                        {
                            var call__6 = Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.Disband();
                            if (call__6)
                            {
                                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = false;
                            }
                        }
                        else
                        {
                            Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                        }
                    }

                    break;
            }

            Dialog.Visible = false;
        }

        // AOWDialogWin.pas:104
        public void DialogKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            switch (Key)
            {
                case 27:
                    DButtonClick(this);
                    break;
                case 13:
                    if (yesPnl.Visible)
                    {
                        yesBtnClick(this);
                    }
                    else
                    {
                        DButtonClick(this);
                    }

                    break;
            }
        }
    }

    public static partial class AOWDialogWin
    {
    }
}
