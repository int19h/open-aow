// Generated from AoWWinMan.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
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

        // AoWWinMan.pas:453
        public void HideTransientWindows()
        {
            int I = default;
            Aow.Units.AoWWindow.TAOWWindow Window = default;
            {
                I = 0;
                int end__4 = Manager.FChildren.Count - 1;
                for (; I <= end__4; I += 1)
                {
                    var call__5 = Manager.FChildren[I];
                    Window = (Aow.Units.AoWWindow.TAOWWindow)call__5;
                    if (((Window != Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow) && Window.Modal) && Window.Visible)
                    {
                        Window.Visible = false;
                    }
                }
            }
        }

        // AoWWinMan.pas:464
        public void SaveLayout()
        {
            // { Read the saved rectangle through the window pointer cell. }
            var call__6 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.RightOffset, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.BottomOffset, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.WinWidth, Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(0, call__6);
            var call__7 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.RightOffset, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.TopOffset, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.WinWidth, Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(1, call__7);
            if (Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awCenter)
            {
                var call__8 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.LeftOffset, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.TopOffset, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.WinWidth, Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__8);
            }
            else
            {
                var call__9 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(2, call__9);
            }

            if (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__10 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.TopOffset, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.WinWidth, Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__10);
            }
            else
            {
                var call__11 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(5, call__11);
            }

            if (Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__12 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.TopOffset, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__12);
            }
            else
            {
                var call__13 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(13, call__13);
            }

            var call__14 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.RightOffset, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.TopOffset, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(6, call__14);
            var call__15 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.RightOffset, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.TopOffset, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.WinWidth, Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(7, call__15);
            if (Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__16 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.LeftOffset, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.BottomOffset, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__16);
            }
            else
            {
                var call__17 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(8, call__17);
            }

            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__18 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.LeftOffset, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment.TopOffset, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinWidth, Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.WinHeight);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__18);
            }
            else
            {
                var call__19 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(9, call__19);
            }

            var call__20 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.RightOffset, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.Alignment.TopOffset, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(12, call__20);
            var call__21 = Aow.Rtl.Rtl.Rect(Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.RightOffset, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.BottomOffset, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.WinWidth, Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.WinHeight);
            Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(10, call__21);
            if (Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)
            {
                var call__22 = Aow.Rtl.Rtl.Rect(Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Left, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Top, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Right, Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect.Bottom);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(11, call__22);
            }
            else
            {
                var call__23 = Aow.Rtl.Rtl.Rect(0, 0, 0, 0);
                Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.SetRectangle(11, call__23);
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
            var call__24 = Lists[0];
            L = (Aow.Units.AoWListBox.TAOWListBox)call__24;
            {
                I = 1;
                int end__25 = L.Items.Count - 1;
                for (; I <= end__25; I += 1)
                {
                    J = I - 1;
                    InsertAt = -1;
                    while (J > -1)
                    {
                        switch (Order)
                        {
                            case 0:
                                var call__26 = L.Items[J];
                                var call__27 = L.Items[I];
                                if (Aow.Rtl.Rtl.CompareStr(Aow.Rtl.Rtl.UpperCase(call__27), Aow.Rtl.Rtl.UpperCase(call__26)) < 0)
                                {
                                    InsertAt = J;
                                }

                                break;
                            case 1:
                                var call__28 = L.Items[J];
                                var call__29 = L.Items[I];
                                if (Aow.Rtl.Rtl.CompareStr(Aow.Rtl.Rtl.UpperCase(call__29), Aow.Rtl.Rtl.UpperCase(call__28)) > 0)
                                {
                                    InsertAt = J;
                                }

                                break;
                            case 2:
                                try
                                {
                                    var call__30 = L.Items[I];
                                    ValueI = Aow.Rtl.Rtl.StrToInt(call__30);
                                    var call__31 = L.Items[J];
                                    ValueJ = Aow.Rtl.Rtl.StrToInt(call__31);
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
                                    var call__32 = L.Items[I];
                                    ValueI = Aow.Rtl.Rtl.StrToInt(call__32);
                                    var call__33 = L.Items[J];
                                    ValueJ = Aow.Rtl.Rtl.StrToInt(call__33);
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
                            int end__34 = Lists.Count - 1;
                            for (; J <= end__34; J += 1)
                            {
                                var call__35 = Lists[J];
                                ((Aow.Units.AoWListBox.TAOWListBox)call__35).Items.Move(I, InsertAt);
                            }
                        }
                    }
                }
            }

            {
                I = 0;
                int end__36 = Lists.Count - 1;
                for (; I <= end__36; I += 1)
                {
                    var call__37 = Lists[I];
                    L = (Aow.Units.AoWListBox.TAOWListBox)call__37;
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
            {
                var w__38 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                if ((w__38.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth) && (Window.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth))
                {
                    {
                        var w__39 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                        w__39.Alignment.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight + Window.WinHeight;
                        w__39.FAligned = false;
                        if (w__39.Visible)
                        {
                            Window.Alignment.RightOffset = w__39.Alignment.RightOffset;
                        }
                        else
                        {
                            Window.Alignment.RightOffset = 0;
                        }
                    }

                    Window.FAligned = false;
                }
            }
        }

        // AoWWinMan.pas:168
        // { draft; 00456AAC }
        public void UnregisterDialog(Aow.Units.AoWWindow.TAOWWindow Window)
        {
            // { Tried direct initial map alignment receiver; no exact match. }
            {
                var w__40 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                if (((((w__40.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth) && (Window.Alignment.AlignWidth == Aow.Units.AoWComp.TAOWAlignWidth.awBoth)) && (((Window == Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin) || !Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Visible) || (Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth))) && (((Window == Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg) || !Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Visible) || (Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth))) && (((Window == Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg) || !Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Visible) || (Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg.Alignment.AlignWidth != Aow.Units.AoWComp.TAOWAlignWidth.awBoth)))
                {
                    {
                        var w__41 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow;
                        w__41.Alignment.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                        w__41.FAligned = false;
                        Window.Alignment.RightOffset = w__41.Alignment.RightOffset;
                    }

                    Window.FAligned = false;
                }
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
                    byte end__42 = 7;
                    for (; I <= end__42; I += 1)
                    {
                        bool bool__43 = (FSelectedUnits[I] > -1);
                        if (bool__43)
                        {
                            var call__44 = FUnitList.Count;
                            bool__43 = (I < call__44);
                        }

                        if (bool__43)
                        {
                            var call__45 = FUnitList.GetUnit(FSelectedUnits[I]);
                            if (U == call__45)
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
                int end__46 = FAbilities.Count - 1;
                for (; I <= end__46; I += 1)
                {
                    var call__47 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__47).Free();
                }
            }

            FAbilities.Clear();
            {
                I = 0;
                byte end__48 = 169;
                for (; I <= end__48; I += 1)
                {
                    var call__49 = FSelectedUnit.GetAbilityEnabled(I);
                    if (call__49)
                    {
                        A = Aow.Units.AoWTC.TTCAbility.Create();
                        A.Ability = I;
                        bool bool__50 = (Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                        if (bool__50)
                        {
                            var call__51 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__52 = call__51.GetControlType(Aow.Units.AoWWinMan.AoWWinMan.General.FSelectedUnit);
                            bool__50 = ((call__52 & TSet<Aow.Units.AoWE.TAbilityControlTypeElement>.Of(Aow.Units.AoWE.TAbilityControlTypeElement.acAction)) != TSet<Aow.Units.AoWE.TAbilityControlTypeElement>.Of());
                        }

                        if (bool__50)
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

        // AoWWinMan.pas:211
        public void ManagerChangeCursor()
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.MainForm */
            throw new System.NotImplementedException();
        }

        // AoWWinMan.pas:216
        // { draft }
        public void GeneralCreate()
        {
            FUnitShowInfo.Direction = 3;
            FUnitShowInfo.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of(Aow.Units.AoWE.TUnitOverlayModesElement.uoRank);
            var call__53 = new Aow.Rtl.TList();
            FAbilities = call__53;
        }

        // AoWWinMan.pas:223
        // { draft }
        public void GeneralDestroy()
        {
            int I = default;
            {
                I = 0;
                int end__54 = FAbilities.Count - 1;
                for (; I <= end__54; I += 1)
                {
                    var call__55 = FAbilities[I];
                    ((Aow.Rtl.TObject)call__55).Free();
                }
            }

            FAbilities.Free();
        }

        // AoWWinMan.pas:231
        // { draft }
        public void ResetLayout()
        {
            // { Tried addressable Self; no exact match. }
            {
                var w__56 = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
                {
                    var w__57 = w__56.Alignment;
                    w__57.TopOffset = 0;
                    w__57.RightOffset = 0;
                }

                if (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight > 700)
                {
                    w__56.WinHeight = (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) / 3;
                }
                else
                {
                    w__56.WinHeight = (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) / 2;
                }

                w__56.WinWidth = (w__56.WinHeight * 200) / 216;
                if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                {
                    w__56.Visible = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null) && Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
                }
            }

            {
                var w__58 = Aow.Units.AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
                {
                    var w__59 = w__58.Alignment;
                    w__59.BottomOffset = 0;
                    w__59.RightOffset = 0;
                }

                w__58.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                w__58.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                {
                    w__58.Visible = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap == null) && Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
                }
            }

            {
                var w__60 = Aow.Units.AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
                {
                    var w__61 = w__60.Alignment;
                    w__61.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                    w__61.RightOffset = 0;
                }

                w__60.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                w__60.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                w__60.Visible = false;
            }

            {
                var w__62 = Aow.Units.AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
                {
                    var w__63 = w__62.Alignment;
                    w__63.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                    w__63.RightOffset = 0;
                }

                w__62.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                w__62.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                w__62.Visible = false;
            }

            {
                var w__64 = Aow.Units.AoWWorldMap.AoWWorldMap.TargetSelectionDlg.Dlg;
                {
                    var w__65 = w__64.Alignment;
                    w__65.TopOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
                    w__65.RightOffset = 0;
                }

                w__64.WinWidth = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                w__64.WinHeight = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            }

            {
                var w__66 = Aow.Units.AoWRealm.AoWRealm.RealmWin.Dlg;
                {
                    var w__67 = w__66.Alignment;
                    w__67.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                    w__67.LeftOffset = 0;
                    w__67.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                    w__67.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                    w__67.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
                }

                w__66.WinWidth = w__66.MinWidth;
                w__66.WinHeight = w__66.MinHeight;
                w__66.Visible = false;
            }

            {
                var w__68 = Aow.Units.AoWExeForms.AoWExeForms.MagicWin.Dlg;
                {
                    var w__69 = w__68.Alignment;
                    w__69.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                    w__69.LeftOffset = 0;
                    w__69.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                    w__69.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                    w__69.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
                }

                w__68.WinWidth = w__68.MinWidth;
                w__68.WinHeight = w__68.MinHeight;
                w__68.Visible = false;
            }

            {
                var w__70 = Aow.Units.AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
                {
                    var w__71 = w__70.Alignment;
                    w__71.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter;
                    w__71.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter;
                }

                w__70.WinWidth = w__70.MinWidth;
                w__70.WinHeight = w__70.MinHeight;
                w__70.Visible = false;
            }

            {
                var w__72 = Aow.Units.AoWExeForms.AoWExeForms.ChatWindow.ChatWin;
                {
                    var w__73 = w__72.Alignment;
                    w__73.BottomOffset = Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight;
                    w__73.LeftOffset = 0;
                    w__73.RightOffset = Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                    w__73.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                    w__73.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom;
                }

                w__72.WinWidth = w__72.MinWidth;
                w__72.WinHeight = w__72.MinHeight;
                w__72.Visible = false;
            }

            {
                var w__74 = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin;
                {
                    var w__75 = w__74.Alignment;
                    w__75.TopOffset = 0;
                    w__75.RightOffset = 0;
                }

                w__74.WinHeight = ((Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) * 170) / 432;
                w__74.WinWidth = (w__74.WinHeight * 150) / 170;
                if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
                {
                    w__74.Visible = (Aow.Units.AoWE.AoWE.AoWHSMap.FCombatMap != null) && Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Visible;
                }
            }

            {
                var w__76 = Aow.Units.AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
                {
                    var w__77 = w__76.Alignment;
                    w__77.BottomOffset = 0;
                    w__77.RightOffset = 0;
                }

                w__76.WinHeight = (Aow.Units.AoWWinMan.AoWWinMan.General.Manager.WinHeight - Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight) - Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinHeight;
                w__76.WinWidth = Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinWidth;
                w__76.Visible = false;
            }

            {
                var w__78 = Aow.Units.AOWMWindow.AOWMWindow.MWindow.MapWindow.Alignment;
                w__78.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
                w__78.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
                w__78.TopOffset = 0;
                w__78.LeftOffset = 0;
            }

            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldAlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            var call__79 = Aow.Rtl.Rtl.Rect(0, 0, Aow.Units.AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight);
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FWorldRect = call__79;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapAlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            var call__80 = Aow.Rtl.Rtl.Rect(0, 0, Aow.Units.AoWExeForms.AoWExeForms.TCScanner.TCScannerWin.WinWidth, Aow.Units.AoWExeForms.AoWExeForms.TBWindow.TaskWindow.WinHeight);
            Aow.Units.AOWMWindow.AOWMWindow.MWindow.FMapRect = call__80;
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

        // AoWWinMan.pas:341
        public void MapProgress(Aow.Units.Engine.TEObject Sender, Aow.Rtl.AnsiString Message, int Progression)
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.Display */
            throw new System.NotImplementedException();
        }

        // AoWWinMan.pas:351
        // { 00454CA7 }
        // { conflict: procedure MapProgress(Sender: TObject; const Text: string; Position: Integer); }
        public void ManagerKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
        }

        // AoWWinMan.pas:392
        public void ManagerUpdate()
        {
            /* TODO(T-shim) Platform or editor boundary AoWUnit.FCursorChanged */
            throw new System.NotImplementedException();
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

            var call__81 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1LoadingRStr);
            var call__82 = Aow.Rtl.Rtl.ExtractFileName(Name);
            Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = ((call__81 + AnsiString.FromBytes(32)) + call__82) + ((AnsiString)"...");
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
                    var call__83 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E1Resources.E1Resources.E1LoadingRStr);
                    var call__84 = Aow.Rtl.Rtl.ExtractFileName(Name);
                    Aow.Units.E2Globals.E2Globals.ProgressDialog.Title.Text = ((call__83 + AnsiString.FromBytes(32)) + call__84) + ((AnsiString)"...");
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

        // AoWWinMan.pas:559
        public static void StartGame(Aow.Units.AoWWinMan.TGeneral Controller)
        {
            /* TODO(T-shim) Platform or editor boundary aowDPlayComp.Connected */
            throw new System.NotImplementedException();
        }
    }
}
