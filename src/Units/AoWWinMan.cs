// Translated from AoWWinMan.pas.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.PrdPlace;
using static Aow.Units.PrdPlace.PrdPlace;
using Aow.Units.ExploreS;
using static Aow.Units.ExploreS.ExploreS;
using Aow.Units.Sign;
using static Aow.Units.Sign.Sign;
using Aow.Units.AOWToolTip;
using static Aow.Units.AOWToolTip.AOWToolTip;
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.AOWMWindow;
using static Aow.Units.AOWMWindow.AOWMWindow;
using Aow.Units.AoWRealm;
using static Aow.Units.AoWRealm.AoWRealm;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using Aow.Units.E1Globals;
using static Aow.Units.E1Globals.E1Globals;
using Aow.Units.E1Resources;
using static Aow.Units.E1Resources.E1Resources;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWUnitBanner;
using static Aow.Units.AoWUnitBanner.AoWUnitBanner;
using Aow.Units.AoWInfoBanner;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;
using Aow.Units.AOWDialogWin;
using static Aow.Units.AOWDialogWin.AOWDialogWin;
using Aow.Units.AoWProgressDialog;
using static Aow.Units.AoWProgressDialog.AoWProgressDialog;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;

namespace Aow.Units.AoWWinMan
{
    // { TGeneral (instance size $CC, VMT 00453FC4).
//     $68 preview settings; $78 abilities; $7C active ability count;
//     $84 selected indices; $A4 unit list; $C0 selected unit; $C4 combat sprite. }
    public partial class TGeneral : Aow.Rtl.TDataModule
    {
        public TGeneral(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AOWWinManager.TAOWWinManager Manager;
        // { +0044 }
        public Aow.Units.AOWToolTip.TAOWToolTip ToolTip;
        // { +0048 }
        public bool FWindowsHidden;
        public Aow.Units.FixedArray_byte_11 FReserved4D;
        public bool FInfoVisible;
        public bool FRealmVisible;
        public bool FMagicVisible;
        public bool FReserved5B;
        public bool FReserved5C;
        public bool FUnitVisible;
        public bool FScannerVisible;
        public bool FControlVisible;
        public bool FChatVisible;
        public bool FMapVisible;
        public bool FReserved62;
        public bool FReserved63;
        public bool FReserved64;
        public bool FTargetVisible;
        public Aow.Units.FixedArray_byte_2 FReserved66;
        // { $4C..$67; 00455A34 }
        // { conflict: FSavedWindowVisibility: array[0..15] of Boolean; }
        // { conflict: FPInfoVisible, FControlVisible, FUnitVisible: Boolean; FReserved5B: array[0..1] of Byte; FRealmVisible, FScannerVisible, FInfoVisible, FChatVisible, FMapVisible: Boolean; FReserved62: array[0..2] of Byte; FMagicVisible: Boolean; }
        public Aow.Units.AoWE.TP2UnitShowInfo FUnitShowInfo;
        // { $68; 0044A1E5 }
        // { $69..$73 are part of FUnitShowInfo. }
        public int FReserved74;
        // { $74 }
        public Aow.Rtl.TList FAbilities;
        // { $78 }
        public int FActiveAbilityCount;
        // { $7C }
        public int F_General_80;
        public Aow.Units.FixedArray_int_8 FSelectedUnits;
        // { $84 }
        public Aow.Units.AoWE.TArmy FUnitList;
        // { $A4 }
        public Aow.Units.AoWE.TStructure FStructure;
        // { $A8; 00442354 }
        public Aow.Units.AoWE.TStructure FPreviousStructure;
        // { $AC }
        public Aow.Units.City.TCity FCity;
        // { $B0 }
        public Aow.Units.PrdPlace.TProductionPlace FProductionPlace;
        // { $B4 }
        public Aow.Units.AoWE.TProductionControl FProductionControl;
        // { $B8; 004437FC }
        public Aow.Units.Sign.TSign FSign;
        // { $BC }
        public Aow.Units.AoWE.TAbstractUnit FSelectedUnit;
        // { $C0; 00449DCD }
        // { conflict: FUnit: TAbstractUnit; }
        public Aow.Units.AoWTC.TTacticalCombatUnitHS FCombatUnit;
        // { $C4 }
        public Aow.Units.FixedArray_byte_4 F_General_C8;
        // AoWWinMan.pas:503
        public void ReturnToTitle()
        {
            Aow.Units.AoWE.AoWE.AoWEngine.CloseHSM(null);
            Aow.Units.AoWE.AoWE.AoWEngine.CloseCampaign();
            Aow.Units.AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(null);
            Aow.Units.AoWExeForms.AoWExeForms.TCScanner.Scanner.SetMap(null);
            Aow.Units.Sound.Sound.SongPlayer.Stop(false);
            Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
            Aow.Rtl.Rtl.SetCurrentDir(Aow.Units.AoWE.AoWE.AoWEngine.StartupDirectory);
            Aow.Units.AoWWinMan.AoWWinMan.General.HideWindows();
            Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.Visible = false;
            Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin.Visible = false;
            Aow.Units.E2Globals.E2Globals.TitleScene.ResetScene();
            Aow.Units.E2GameFlow.E2GameFlow.ShowTitleScene(Aow.Units.E2Globals.E2Globals.TitleScene, true);
        }

        // AoWWinMan.pas:415
        public void HideWindows()
        {
            int I = default;
            if (!FWindowsHidden)
            {
                FWindowsHidden = true;
                FInfoVisible = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible;
                FRealmVisible = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible;
                FMagicVisible = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible;
                FUnitVisible = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible;
                FTargetVisible = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible;
                FScannerVisible = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible;
                FControlVisible = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible;
                FChatVisible = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible;
                FMapVisible = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
                {
                    I = 0;
                    int end__1 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren.Count - 1;
                    for (; I <= end__1; I += 1)
                    {
                        var call__2 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren[I];
                        if (((Aow.Units.AoWComp.TAoWComponent)call__2) != Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow)
                        {
                            var call__3 = Aow.Units.AoWWinMan.AoWWinMan.General.Manager.FChildren[I];
                            ((Aow.Units.AoWComp.TAoWComponent)call__3).Visible = false;
                        }
                    }
                }
            }
        }

        // AoWWinMan.pas:436
        public void ShowWindows()
        {
            if (FWindowsHidden)
            {
                FWindowsHidden = false;
                Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible = FInfoVisible;
                Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = FRealmVisible;
                Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = FMagicVisible;
                Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible = FScannerVisible;
                Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible = FControlVisible;
                Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible = FMapVisible;
                Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible = FUnitVisible;
                Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible = FTargetVisible;
                Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible = FChatVisible;
            }
        }

        // AoWWinMan.pas:464
        public void SaveLayout()
        {
            // { Read the saved rectangle through the window pointer cell. }
            var call__4 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.RightOffset, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.BottomOffset, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.WinWidth, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(0, call__4);
            var call__5 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.RightOffset, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.TopOffset, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.WinWidth, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(1, call__5);
            if (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awCenter)
            {
                var call__6 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.LeftOffset, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.TopOffset, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.WinWidth, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__6);
            }
            else
            {
                var call__7 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__7);
            }

            if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__8 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.TopOffset, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.WinWidth, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__8);
            }
            else
            {
                var call__9 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__9);
            }

            if (Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__10 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.TopOffset, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__10);
            }
            else
            {
                var call__11 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__11);
            }

            var call__12 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.RightOffset, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.TopOffset, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(6, call__12);
            var call__13 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.RightOffset, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.TopOffset, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.WinWidth, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(7, call__13);
            if (Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__14 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.BottomOffset, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__14);
            }
            else
            {
                var call__15 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__15);
            }

            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__16 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinWidth, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__16);
            }
            else
            {
                var call__17 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__17);
            }

            var call__18 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.RightOffset, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.TopOffset, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(12, call__18);
            var call__19 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.RightOffset, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.BottomOffset, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.WinWidth, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(10, call__19);
            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__20 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Left, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Top, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Right, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Bottom);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(11, call__20);
            }
            else
            {
                var call__21 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(11, call__21);
            }
        }

        // AoWWinMan.pas:79
        public void PlayButtonSound(byte Kind)
        {
            switch (Kind)
            {
                case 0:
                    Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Btn1.wav");
                    break;
                case 3:
                    Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Btn3.wav");
                    break;
                case 1:
                    Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Btn5.wav");
                    break;
                case 2:
                    Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Btn6.wav");
                    break;
            }
        }

        // AoWWinMan.pas:123
        // { draft; 00455DFC }
        // { conflict: procedure ButtonSound(Index: Byte); }
        public void SortLists(ref Aow.Rtl.TList Lists, byte Order)
        {
            Aow.Units.AoWListBox.TAOWListBox L = default;
            int I = default;
            int J = default;
            int InsertAt = default;
            int ValueI = default;
            int ValueJ = default;
            var call__22 = Lists[0];
            L = (Aow.Units.AoWListBox.TAOWListBox)call__22;
            {
                I = 1;
                int end__23 = L.Items.Count - 1;
                for (; I <= end__23; I += 1)
                {
                    J = I - 1;
                    InsertAt = -1;
                    while (J > -1)
                    {
                        switch (Order)
                        {
                            case 0:
                                var call__24 = L.Items[J];
                                var call__25 = L.Items[I];
                                if (Aow.Rtl.Rtl.CompareStr(Aow.Rtl.Rtl.UpperCase(call__25), Aow.Rtl.Rtl.UpperCase(call__24)) < 0)
                                {
                                    InsertAt = J;
                                }

                                break;
                            case 1:
                                var call__26 = L.Items[J];
                                var call__27 = L.Items[I];
                                if (Aow.Rtl.Rtl.CompareStr(Aow.Rtl.Rtl.UpperCase(call__27), Aow.Rtl.Rtl.UpperCase(call__26)) > 0)
                                {
                                    InsertAt = J;
                                }

                                break;
                            case 2:
                                try
                                {
                                    var call__28 = L.Items[I];
                                    ValueI = Aow.Rtl.Rtl.StrToInt(call__28);
                                    var call__29 = L.Items[J];
                                    ValueJ = Aow.Rtl.Rtl.StrToInt(call__29);
                                    if (ValueI > ValueJ)
                                    {
                                        InsertAt = J;
                                    }
                                }
                                catch (System.Exception)
                                {
                                }

                                break;
                            case 3:
                                try
                                {
                                    var call__30 = L.Items[I];
                                    ValueI = Aow.Rtl.Rtl.StrToInt(call__30);
                                    var call__31 = L.Items[J];
                                    ValueJ = Aow.Rtl.Rtl.StrToInt(call__31);
                                    if (ValueI < ValueJ)
                                    {
                                        InsertAt = J;
                                    }
                                }
                                catch (System.Exception)
                                {
                                }

                                break;
                        }

                        J -= 1;
                    }

                    if (InsertAt > -1)
                    {
                        {
                            J = 0;
                            int end__32 = Lists.Count - 1;
                            for (; J <= end__32; J += 1)
                            {
                                var call__33 = Lists[J];
                                ((Aow.Units.AoWListBox.TAOWListBox)call__33).Items.Move(I, InsertAt);
                            }
                        }
                    }
                }
            }

            {
                I = 0;
                int end__34 = Lists.Count - 1;
                for (; I <= end__34; I += 1)
                {
                    var call__35 = Lists[I];
                    L = (Aow.Units.AoWListBox.TAOWListBox)call__35;
                    L.ListOffset = 0;
                    L.Selected = -1;
                    L.FSized = false;
                }
            }

            Lists.Free();
        }

        // AoWWinMan.pas:152
        // { draft; 00455744 }
        public void RegisterDialog(Aow.Units.AoWWindow.TAOWWindow Window)
        {
            // { Tried direct initial map alignment receiver; no exact match. }
            var window = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
            if ((window.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth) && (Window.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth))
            {
                var window2 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                window2.Alignment.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight + Window.WinHeight;
                window2.FAligned = false;
                if (window2.Visible)
                {
                    Window.Alignment.RightOffset = window2.Alignment.RightOffset;
                }
                else
                {
                    Window.Alignment.RightOffset = 0;
                }

                Window.FAligned = false;
            }
        }

        // AoWWinMan.pas:168
        // { draft; 00456AAC }
        public void UnregisterDialog(Aow.Units.AoWWindow.TAOWWindow Window)
        {
            // { Tried direct initial map alignment receiver; no exact match. }
            var window = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
            if (((((window.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth) && (Window.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth)) && (((Window == Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin) || !Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible) || (Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth))) && (((Window == Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg) || !Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible) || (Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth))) && (((Window == Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg) || !Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible) || (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)))
            {
                var window2 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                window2.Alignment.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                window2.FAligned = false;
                Window.Alignment.RightOffset = window2.Alignment.RightOffset;

                Window.FAligned = false;
            }
        }

        // AoWWinMan.pas:89
        // { draft; 00456B34 }
        public int IndexOfUnit(Aow.Units.AoWE.TAbstractUnit U)
        {
            int Result = default;
            int I = default;
            Result = -1;
            if ((FSelectedUnits[0] > -1) && (FUnitList != null))
            {
                {
                    I = 0;
                    byte end__40 = 7;
                    for (; I <= end__40; I += 1)
                    {
                        bool bool__41 = (FSelectedUnits[I] > -1);
                        if (bool__41)
                        {
                            var call__42 = FUnitList.Count;
                            bool__41 = (I < call__42);
                        }

                        if (bool__41)
                        {
                            var call__43 = FUnitList.GetUnit(FSelectedUnits[I]);
                            if (U == call__43)
                            {
                                Result = I;
                            }
                        }
                    }
                }
            }

            return Result;
        }

        // AoWWinMan.pas:100
        public void BuildAbilities()
        {
            int I = default;
            Aow.Units.AoWTC.TTCAbility A = default;
            FActiveAbilityCount = 0;
            {
                I = 0;
                int end__44 = FAbilities.Count - 1;
                for (; I <= end__44; I += 1)
                {
                    var call__45 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__45).Free();
                }
            }

            FAbilities.Clear();
            {
                I = 0;
                byte end__46 = 169;
                for (; I <= end__46; I += 1)
                {
                    var call__47 = FSelectedUnit.GetAbilityEnabled(I);
                    if (call__47)
                    {
                        A = new Aow.Units.AoWTC.TTCAbility();
                        A.Ability = I;
                        bool bool__48 = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        if (bool__48)
                        {
                            var call__49 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__50 = call__49.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            bool__48 = ((call__50 & TSet<Aow.Units.AoWE.TAbilityControlTypeElement>.Of(Aow.Units.AoWE.TAbilityControlTypeElement.acAction)) != TSet<Aow.Units.AoWE.TAbilityControlTypeElement>.Of());
                        }

                        if (bool__48)
                        {
                            FAbilities.Insert(FActiveAbilityCount, A);
                            FActiveAbilityCount += 1;
                        }
                        else
                        {
                            FAbilities.Add(A);
                        }
                    }
                }
            }
        }

        // AoWWinMan.pas:197
        // { 00455D10 }
        public int MTtoInt(TSet<Aow.Units.AoWE.TMoveType> MoveTypes)
        {
            int Result = default;
            Result = 0;
            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtForestry)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtForestry))
            {
                Result = 2;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtMountaineering)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtMountaineering))
            {
                Result = 4;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming))
            {
                Result = 6;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtWalking, Aow.Units.AoWE.TMoveType.mtSwimming)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtWalking, Aow.Units.AoWE.TMoveType.mtSwimming))
            {
                Result = 8;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming, Aow.Units.AoWE.TMoveType.mtForestry)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming, Aow.Units.AoWE.TMoveType.mtForestry))
            {
                Result = 10;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming, Aow.Units.AoWE.TMoveType.mtMountaineering)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming, Aow.Units.AoWE.TMoveType.mtMountaineering))
            {
                Result = 12;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtForestry, Aow.Units.AoWE.TMoveType.mtMountaineering)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtForestry, Aow.Units.AoWE.TMoveType.mtMountaineering))
            {
                Result = 14;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming, Aow.Units.AoWE.TMoveType.mtForestry, Aow.Units.AoWE.TMoveType.mtMountaineering)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming, Aow.Units.AoWE.TMoveType.mtForestry, Aow.Units.AoWE.TMoveType.mtMountaineering))
            {
                Result = 16;
            }

            if ((MoveTypes & TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtFlying)) == TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtFlying))
            {
                Result = 18;
            }

            return Result;
        }

        // AoWWinMan.pas:185
        public int SphereToImage(Aow.Units.AoWE.TMagicSphere Sphere)
        {
            int Result = default;
            switch (Sphere)
            {
                case Aow.Units.AoWE.TMagicSphere.msLife:
                    Result = 27;
                    break;
                case Aow.Units.AoWE.TMagicSphere.msDeath:
                    Result = 30;
                    break;
                case Aow.Units.AoWE.TMagicSphere.msEarth:
                    Result = 33;
                    break;
                case Aow.Units.AoWE.TMagicSphere.msAir:
                    Result = 36;
                    break;
                case Aow.Units.AoWE.TMagicSphere.msWater:
                    Result = 39;
                    break;
                default:
                    Result = 42;
                    break;
            }

            return Result;
        }

        // AoWWinMan.pas:216
        // { draft }
        public void GeneralCreate()
        {
            FUnitShowInfo.Direction = 3;
            FUnitShowInfo.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
            var call__51 = new Aow.Rtl.TList();
            FAbilities = call__51;
        }

        // AoWWinMan.pas:223
        // { draft }
        public void GeneralDestroy()
        {
            int I = default;
            {
                I = 0;
                int end__52 = FAbilities.Count - 1;
                for (; I <= end__52; I += 1)
                {
                    var call__53 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__53).Free();
                }
            }

            FAbilities.Free();
        }

        // AoWWinMan.pas:231
        // { draft }
        public void ResetLayout()
        {
            // { Tried addressable Self; no exact match. }
            var window = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
            var alignment = window.Alignment;
            alignment.TopOffset = 0;
            alignment.RightOffset = 0;

            if (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight > 700)
            {
                window.WinHeight = (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) / 3;
            }
            else
            {
                window.WinHeight = (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) / 2;
            }

            window.WinWidth = (window.WinHeight * 200) / 216;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                window.Visible = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null) && Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
            }

            var window2 = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
            var alignment2 = window2.Alignment;
            alignment2.BottomOffset = 0;
            alignment2.RightOffset = 0;

            window2.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window2.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                window2.Visible = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null) && Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
            }

            var window3 = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
            var alignment3 = window3.Alignment;
            alignment3.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            alignment3.RightOffset = 0;

            window3.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window3.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            window3.Visible = false;

            var window4 = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
            var alignment4 = window4.Alignment;
            alignment4.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            alignment4.RightOffset = 0;

            window4.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window4.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            window4.Visible = false;

            var window5 = Aow.Units.AoWWorldMap.AoWWorldMap.TargetSelectionDlg.Dlg;
            var alignment5 = window5.Alignment;
            alignment5.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            alignment5.RightOffset = 0;

            window5.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window5.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;

            var window6 = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg;
            var alignment6 = window6.Alignment;
            alignment6.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
            alignment6.LeftOffset = 0;
            alignment6.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            alignment6.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            alignment6.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;

            window6.WinWidth = window6.MinWidth;
            window6.WinHeight = window6.MinHeight;
            window6.Visible = false;

            var window7 = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg;
            var alignment7 = window7.Alignment;
            alignment7.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
            alignment7.LeftOffset = 0;
            alignment7.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            alignment7.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            alignment7.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;

            window7.WinWidth = window7.MinWidth;
            window7.WinHeight = window7.MinHeight;
            window7.Visible = false;

            var window8 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
            var alignment8 = window8.Alignment;
            alignment8.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
            alignment8.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter;

            window8.WinWidth = window8.MinWidth;
            window8.WinHeight = window8.MinHeight;
            window8.Visible = false;

            var window9 = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin;
            var alignment9 = window9.Alignment;
            alignment9.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
            alignment9.LeftOffset = 0;
            alignment9.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            alignment9.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            alignment9.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;

            window9.WinWidth = window9.MinWidth;
            window9.WinHeight = window9.MinHeight;
            window9.Visible = false;

            var window10 = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin;
            var alignment10 = window10.Alignment;
            alignment10.TopOffset = 0;
            alignment10.RightOffset = 0;

            window10.WinHeight = ((Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) * 170) / 432;
            window10.WinWidth = (window10.WinHeight * 150) / 170;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                window10.Visible = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null) && Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
            }

            var window11 = Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
            var alignment11 = window11.Alignment;
            alignment11.BottomOffset = 0;
            alignment11.RightOffset = 0;

            window11.WinHeight = (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) - Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinHeight;
            window11.WinWidth = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinWidth;
            window11.Visible = false;

            var alignment12 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment;
            alignment12.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            alignment12.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            alignment12.TopOffset = 0;
            alignment12.LeftOffset = 0;

            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            var call__77 = Aow.Rtl.Rtl.Rect(0, 0, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight);
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect = call__77;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            var call__78 = Aow.Rtl.Rtl.Rect(0, 0, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight);
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect = call__78;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                if (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null)
                {
                    Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                }
                else
                {
                    Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.RightOffset = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinWidth;
                }
            }

            Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
            if (Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible)
            {
                RegisterDialog(Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin);
            }

            Manager.FAligned = false;
        }

        // AoWWinMan.pas:336
        // { draft }
        public void ManagerDebugMessage(Aow.Rtl.AnsiString Text)
        {
            Aow.Units.E2Globals.E2Globals.DialogWin.AddDebugMessage(Text);
        }

        // AoWWinMan.pas:351
        // { 00454CA7 }
        // { conflict: procedure MapProgress(Sender: TObject; const Text: string; Position: Integer); }
        public void ManagerKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            Aow.Units.AoWE.TPlayer Player = default;
            Aow.Rtl.AnsiString S = default;
            if (Aow.Units.AoWE.AoWE.AoWEngine.FDebug)
            {
                bool bool__79 = (((Aow.Units.AoWE.AoWE.AoWHSMap != null) && ((Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode != Aow.Units.AoWE.TPlayMode.pmSimultaneous) || (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.FLocalPlayers.Count == 1))) && TSet<byte>.WithBounds(0, 255).WithRange((int)49, (int)57).Contains((byte)Key)) && (Aow.Units.AoWTC.AoWTC.AoWCombatMap == null);
                if (bool__79)
                {
                    var call__80 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetCount();
                    bool__79 = (call__80 > (Key - ((int)48)));
                }

                bool bool__81 = bool__79;
                if (bool__81)
                {
                    var call__82 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetDebugMode();
                    bool__81 = call__82;
                }

                if (bool__81)
                {
                    var call__83 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Key - ((int)48));
                    Player = call__83;
                    if (Player.FPlayerType == Aow.Units.AoWE.TPlayerType.ptCPU5)
                    {
                        Player.FPlayerType = Aow.Units.AoWE.TPlayerType.ptHuman;
                    }
                    else
                    {
                        Player.FPlayerType = (Aow.Units.AoWE.TPlayerType)(((int)Player.FPlayerType) + 1);
                    }

                    var call__84 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TPlayerTypeRStr[(int)Player.FPlayerType]);
                    S = ((((AnsiString)"Player ") + Aow.Rtl.Rtl.IntToStr(Key - ((int)48))) + ((AnsiString)" set to: ")) + call__84;
                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(S);
                    Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Update();
                    Aow.Units.AoWE.AoWE.AoWHSMap.FRequestCombatType = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.FLocalHumanPlayers.Count > 1;
                }

                switch (Key)
                {
                    case 122:
                        var call__85 = Aow.Rtl.Rtl.Application.ExeName;
                        var call__86 = Aow.Rtl.Rtl.ExtractFileDir(call__85);
                        S = call__86;
                        S = S + ((AnsiString)"\\Scenario\\Custom");
                        Aow.Rtl.Rtl.SetCurrentDir(S);
                        Aow.Units.AoWWinMan.AoWWinMan.LoadGame(this, (AnsiString)"Crap.hsm");
                        Aow.Units.AoWWinMan.AoWWinMan.StartGame(this);
                        break;
                    case 123:
                        var call__87 = Aow.Rtl.Rtl.Application.ExeName;
                        var call__88 = Aow.Rtl.Rtl.ExtractFileDir(call__87);
                        S = call__88;
                        S = S + ((AnsiString)"\\Scenario\\Custom");
                        Aow.Rtl.Rtl.SetCurrentDir(S);
                        Aow.Units.AoWWinMan.AoWWinMan.LoadGame(this, (AnsiString)"demomap.hsm");
                        Aow.Units.AoWWinMan.AoWWinMan.StartGame(this);
                        break;
                }
            }
        }
    }

    public static partial class AoWWinMan
    {
        // { draft }
        public static Aow.Units.AoWWinMan.TGeneral General;
        // AoWWinMan.pas:514
        public static void LoadGame(Aow.Units.AoWWinMan.TGeneral Controller, Aow.Rtl.AnsiString Name)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.CloseHSM(null);
            }

            var call__89 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1LoadingRStr);
            var call__90 = Aow.Rtl.Rtl.ExtractFileName(Name);
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = ((call__89 + AnsiString.FromBytes(32)) + call__90) + ((AnsiString)"...");
            Aow.Units.E2Globals.E2Globals.ProgressDialog.StatusLbl.Text = (AnsiString)"";
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position = 0;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Max = 100;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = true;
            if (!Aow.Units.E2Globals.E2Globals.TitleScene.Window.Visible)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 5000;
            }

            Aow.Units.Sound.Sound.SongPlayer.Stop(false);
            Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
            Aow.Units.Sound.Sound.FadeMusicVolume(0, 1);
            Aow.Units.AoWE.AoWE.AoWEngine.LoadHSM(Name, false, Controller.MapProgress);
            if ((Aow.Units.AoWE.AoWE.AoWEngine.FDemo && (Aow.Units.AoWE.AoWE.AoWHSMap != null)) && !Aow.Units.AoWE.AoWE.AoWHSMap.FShowAllPlayers)
            {
                Controller.ReturnToTitle();
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = false;
        }

        // AoWWinMan.pas:532
        public static void LoadCampaign(Aow.Units.AoWWinMan.TGeneral Controller, Aow.Rtl.AnsiString Name)
        {
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.CloseHSM(null);
            }

            Aow.Units.AoWE.AoWE.AoWEngine.LoadCampaign(Name);
            if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign != null)
            {
                if (Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FScenarioStarted)
                {
                    var call__91 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1LoadingRStr);
                    var call__92 = Aow.Rtl.Rtl.ExtractFileName(Name);
                    Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = ((call__91 + AnsiString.FromBytes(32)) + call__92) + ((AnsiString)"...");
                    Aow.Units.E2Globals.E2Globals.ProgressDialog.StatusLbl.Text = (AnsiString)"";
                    Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Position = 0;
                    Aow.Units.E2Globals.E2Globals.ProgressDialog.Bar.Max = 100;
                    Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = true;
                    if (!Aow.Units.E2Globals.E2Globals.TitleScene.Window.Visible)
                    {
                        Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 5000;
                    }

                    Aow.Units.Sound.Sound.SongPlayer.Stop(false);
                    Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
                    Aow.Units.Sound.Sound.FadeMusicVolume(0, 1);
                    Aow.Units.AoWE.AoWE.AoWEngine.LoadCampaignMap(Controller.MapProgress);
                    Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
                    Aow.Units.E2Globals.E2Globals.ProgressDialog.ProgDlg.Visible = false;
                }
                else
                {
                    Aow.Units.E2Globals.E2Globals.TitleScene.HideScene();
                    Aow.Units.E1GameFlow.E1GameFlow.ShowCampaignMap(Aow.Units.AoWWorldMap.AoWWorldMap.WorldMap);
                }
            }
        }
    }
}
