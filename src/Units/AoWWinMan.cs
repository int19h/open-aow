// Translated from AoWWinMan.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Sound.Sound;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using Aow.Units.AoWWindow;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.City;
using Aow.Units.PrdPlace;
using Aow.Units.Sign;
using Aow.Units.AOWToolTip;
using Aow.Units.AOWWinManager;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.AOWMWindow.AOWMWindow;
using static Aow.Units.AoWRealm.AoWRealm;
using static Aow.Units.E1Resources.E1Resources;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.E1GameFlow.E1GameFlow;

namespace Aow.Units.AoWWinMan
{
    // preview settings. abilities. active ability count. selected indices. unit list. selected
    // unit. combat sprite
    public partial class TGeneral : TDataModule
    {
        public TGeneral(TComponent owner) : base(owner)
        { }

        public TAOWWinManager Manager;
        public TAOWToolTip ToolTip;
        public bool FWindowsHidden;
        public bool FInfoVisible;
        public bool FRealmVisible;
        public bool FMagicVisible;
        public bool FUnitVisible;
        public bool FScannerVisible;
        public bool FControlVisible;
        public bool FChatVisible;
        public bool FMapVisible;
        public bool FTargetVisible;
        public TP2UnitShowInfo FUnitShowInfo;
        // are part of FUnitShowInfo
        public TList FAbilities;
        public int FActiveAbilityCount;
        public FixedArray_int_8 FSelectedUnits;
        public TArmy FUnitList;
        public TStructure FStructure;
        public TStructure FPreviousStructure;
        public TCity FCity;
        public TProductionPlace FProductionPlace;
        public TProductionControl FProductionControl;
        public TSign FSign;
        public TAbstractUnit FSelectedUnit;
        public TTacticalCombatUnitHS FCombatUnit;
        // AoWWinMan.pas:503
        public void ReturnToTitle()
        {
            AoWEngine.CloseHSM(null);
            AoWEngine.CloseCampaign();
            AoWSWindow.AoWSWindow.SWindow.Scanner.SetMap(null);
            TCScanner.Scanner.SetMap(null);
            SongPlayer.Stop(false);
            SongPlayer.Songs.Clear();
            SetCurrentDir(AoWEngine.StartupDirectory);
            AoWWinMan.General.HideWindows();
            TBWindow.TaskWindow.Visible = false;
            TitleWindow.TitleWin.Visible = false;
            TitleScene.ResetScene();
            E2GameFlow.E2GameFlow.ShowTitleScene(TitleScene, true);
        }

        // AoWWinMan.pas:415
        public void HideWindows()
        {
            if (!FWindowsHidden)
            {
                FWindowsHidden = true;
                FInfoVisible = AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible;
                FRealmVisible = AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible;
                FMagicVisible = AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible;
                FUnitVisible = RealmWin.Dlg.Visible;
                FTargetVisible = MagicWin.Dlg.Visible;
                FScannerVisible = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible;
                FControlVisible = AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible;
                FChatVisible = ChatWindow.ChatWin.Visible;
                FMapVisible = MWindow.MapWindow.Visible;
                for (int I = 0, last = AoWWinMan.General.Manager.FChildren.Count - 1; I <= last; I++)
                {
                    if ((TAoWComponent)AoWWinMan.General.Manager.FChildren[I] != TBWindow.TaskWindow)
                    {
                        ((TAoWComponent)AoWWinMan.General.Manager.FChildren[I]).Visible = false;
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
                AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Visible = FInfoVisible;
                AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Visible = FRealmVisible;
                AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Visible = FMagicVisible;
                AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Visible = FScannerVisible;
                AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Visible = FControlVisible;
                MWindow.MapWindow.Visible = FMapVisible;
                RealmWin.Dlg.Visible = FUnitVisible;
                MagicWin.Dlg.Visible = FTargetVisible;
                ChatWindow.ChatWin.Visible = FChatVisible;
            }
        }

        // AoWWinMan.pas:464
        public void SaveLayout()
        {
            // { Read the saved rectangle through the window pointer cell. }
            var rect = Rect(AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.RightOffset, AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.Alignment.BottomOffset, AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.WinWidth, AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin.WinHeight);
            AoWEngine.FRegistry.SetRectangle(0, rect);
            var rect2 = Rect(AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.RightOffset, AOWControlWin.AOWControlWin.ControlWin.ControlWindow.Alignment.TopOffset, AOWControlWin.AOWControlWin.ControlWin.ControlWindow.WinWidth, AOWControlWin.AOWControlWin.ControlWin.ControlWindow.WinHeight);
            AoWEngine.FRegistry.SetRectangle(1, rect2);
            if (AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.AlignWidth != TAOWAlignWidth.awCenter)
            {
                var rect3 = Rect(AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.LeftOffset, AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.Alignment.TopOffset, AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.WinWidth, AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin.WinHeight);
                AoWEngine.FRegistry.SetRectangle(2, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(2, rect3);
            }

            if (RealmWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(RealmWin.Dlg.Alignment.LeftOffset, RealmWin.Dlg.Alignment.TopOffset, RealmWin.Dlg.WinWidth, RealmWin.Dlg.WinHeight);
                AoWEngine.FRegistry.SetRectangle(5, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(5, rect3);
            }

            if (MagicWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(MagicWin.Dlg.Alignment.LeftOffset, MagicWin.Dlg.Alignment.TopOffset, MagicWin.Dlg.WinWidth, MagicWin.Dlg.WinHeight);
                AoWEngine.FRegistry.SetRectangle(13, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(13, rect3);
            }

            var rect4 = Rect(AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.RightOffset, AoWSWindow.AoWSWindow.SWindow.ScannerWindow.Alignment.TopOffset, AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth, AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight);
            AoWEngine.FRegistry.SetRectangle(6, rect4);
            var rect5 = Rect(AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.RightOffset, AoWInfoWin.AoWInfoWin.IWindow.InfoWin.Alignment.TopOffset, AoWInfoWin.AoWInfoWin.IWindow.InfoWin.WinWidth, AoWInfoWin.AoWInfoWin.IWindow.InfoWin.WinHeight);
            AoWEngine.FRegistry.SetRectangle(7, rect5);
            if (ChatWindow.ChatWin.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(ChatWindow.ChatWin.Alignment.LeftOffset, ChatWindow.ChatWin.Alignment.BottomOffset, ChatWindow.ChatWin.WinWidth, ChatWindow.ChatWin.WinHeight);
                AoWEngine.FRegistry.SetRectangle(8, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(8, rect3);
            }

            if (MWindow.MapWindow.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(MWindow.MapWindow.Alignment.LeftOffset, MWindow.MapWindow.Alignment.TopOffset, MWindow.MapWindow.WinWidth, MWindow.MapWindow.WinHeight);
                AoWEngine.FRegistry.SetRectangle(9, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(9, rect3);
            }

            var rect6 = Rect(TCScanner.TCScannerWin.Alignment.RightOffset, TCScanner.TCScannerWin.Alignment.TopOffset, TCScanner.TCScannerWin.WinWidth, TCScanner.TCScannerWin.WinHeight);
            AoWEngine.FRegistry.SetRectangle(12, rect6);
            var rect7 = Rect(AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.RightOffset, AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.Alignment.BottomOffset, AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.WinWidth, AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin.WinHeight);
            AoWEngine.FRegistry.SetRectangle(10, rect7);
            if (MWindow.FMapAlignWidth != TAOWAlignWidth.awBoth)
            {
                var rect3 = Rect(MWindow.FMapRect.Left, MWindow.FMapRect.Top, MWindow.FMapRect.Right, MWindow.FMapRect.Bottom);
                AoWEngine.FRegistry.SetRectangle(11, rect3);
            }
            else
            {
                var rect3 = Rect(0, 0, 0, 0);
                AoWEngine.FRegistry.SetRectangle(11, rect3);
            }
        }

        // AoWWinMan.pas:79
        public void PlayButtonSound(byte Kind)
        {
            switch (Kind)
            {
                case 0:
                    PlaySample((AnsiString)"Sfx\\Int\\Btn1.wav");
                    break;
                case 3:
                    PlaySample((AnsiString)"Sfx\\Int\\Btn3.wav");
                    break;
                case 1:
                    PlaySample((AnsiString)"Sfx\\Int\\Btn5.wav");
                    break;
                case 2:
                    PlaySample((AnsiString)"Sfx\\Int\\Btn6.wav");
                    break;
            }
        }

        // AoWWinMan.pas:123
        public void SortLists(ref TList Lists, byte Order)
        {
            TAOWListBox L;
            int J;
            int InsertAt;
            int ValueI;
            int ValueJ;
            L = (TAOWListBox)Lists[0];
            for (int I = 1, last = L.Items.Count - 1; I <= last; I++)
            {
                J = I - 1;
                InsertAt = -1;
                while (J > -1)
                {
                    switch (Order)
                    {
                        case 0:
                            var item = L.Items[J];
                            if (CompareStr(UpperCase(L.Items[I]), UpperCase(item)) < 0)
                            {
                                InsertAt = J;
                            }

                            break;
                        case 1:
                            var item2 = L.Items[J];
                            if (CompareStr(UpperCase(L.Items[I]), UpperCase(item2)) > 0)
                            {
                                InsertAt = J;
                            }

                            break;
                        case 2:
                            try
                            {
                                ValueI = StrToInt(L.Items[I]);
                                ValueJ = StrToInt(L.Items[J]);
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
                                ValueI = StrToInt(L.Items[I]);
                                ValueJ = StrToInt(L.Items[J]);
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
                    J = 0;
                    int limit = Lists.Count - 1;
                    for (; J <= limit; J += 1)
                    {
                        ((TAOWListBox)Lists[J]).Items.Move(I, InsertAt);
                    }
                }
            }

            for (int I = 0, last = Lists.Count - 1; I <= last; I++)
            {
                L = (TAOWListBox)Lists[I];
                L.ListOffset = 0;
                L.Selected = -1;
                L.FSized = false;
            }

            Lists.Free();
        }

        // AoWWinMan.pas:152
        public void RegisterDialog(TAOWWindow Window)
        {
            var window = MWindow.MapWindow;
            if (window.Alignment.AlignWidth == TAOWAlignWidth.awBoth && Window.Alignment.AlignWidth == TAOWAlignWidth.awBoth)
            {
                var window2 = MWindow.MapWindow;
                window2.Alignment.BottomOffset = TBWindow.TaskWindow.WinHeight + Window.WinHeight;
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
        public void UnregisterDialog(TAOWWindow Window)
        {
            var window = MWindow.MapWindow;
            if (window.Alignment.AlignWidth == TAOWAlignWidth.awBoth && Window.Alignment.AlignWidth == TAOWAlignWidth.awBoth && (Window == ChatWindow.ChatWin || !ChatWindow.ChatWin.Visible || ChatWindow.ChatWin.Alignment.AlignWidth != TAOWAlignWidth.awBoth) && (Window == MagicWin.Dlg || !MagicWin.Dlg.Visible || MagicWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth) && (Window == RealmWin.Dlg || !RealmWin.Dlg.Visible || RealmWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth))
            {
                var window2 = MWindow.MapWindow;
                window2.Alignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
                window2.FAligned = false;
                Window.Alignment.RightOffset = window2.Alignment.RightOffset;

                Window.FAligned = false;
            }
        }

        // AoWWinMan.pas:89
        public int IndexOfUnit(TAbstractUnit U)
        {
            int Result = -1;
            if (FSelectedUnits[0] > -1 && FUnitList != null)
            {
                for (int I = 0; I <= 7; I++)
                {
                    if (FSelectedUnits[I] > -1 && I < FUnitList.Count)
                    {
                        if (U == FUnitList.GetUnit(FSelectedUnits[I]))
                        {
                            Result = I;
                        }
                    }
                }
            }

            return Result;
        }

        // AoWWinMan.pas:100
        public void BuildAbilities()
        {
            TTCAbility A;
            FActiveAbilityCount = 0;
            for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
            {
                ((TObject)FAbilities[I]).Free();
            }

            FAbilities.Clear();
            for (int I = 0; I <= 169; I++)
            {
                if (FSelectedUnit.GetAbilityEnabled(I))
                {
                    A = new TTCAbility();
                    A.Ability = I;
                    if (AoWWinMan.General.FSelectedUnit.FPlayer == AoWHSMap.FSeatedPlayer && (AoWHSSet.FAbilityControl.GetAbility(I).GetControlType(AoWWinMan.General.FSelectedUnit) & TSet<TAbilityControlTypeElement>.Of(TAbilityControlTypeElement.acAction)) != TSet<TAbilityControlTypeElement>.Of())
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

        // AoWWinMan.pas:197
        public int MTtoInt(TSet<TMoveType> MoveTypes)
        {
            int Result = 0;
            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtForestry)) == TSet<TMoveType>.Of(TMoveType.mtForestry))
            {
                Result = 2;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtMountaineering)) == TSet<TMoveType>.Of(TMoveType.mtMountaineering))
            {
                Result = 4;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtSwimming)) == TSet<TMoveType>.Of(TMoveType.mtSwimming))
            {
                Result = 6;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtWalking, TMoveType.mtSwimming)) == TSet<TMoveType>.Of(TMoveType.mtWalking, TMoveType.mtSwimming))
            {
                Result = 8;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtSwimming, TMoveType.mtForestry)) == TSet<TMoveType>.Of(TMoveType.mtSwimming, TMoveType.mtForestry))
            {
                Result = 10;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtSwimming, TMoveType.mtMountaineering)) == TSet<TMoveType>.Of(TMoveType.mtSwimming, TMoveType.mtMountaineering))
            {
                Result = 12;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtForestry, TMoveType.mtMountaineering)) == TSet<TMoveType>.Of(TMoveType.mtForestry, TMoveType.mtMountaineering))
            {
                Result = 14;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtSwimming, TMoveType.mtForestry, TMoveType.mtMountaineering)) == TSet<TMoveType>.Of(TMoveType.mtSwimming, TMoveType.mtForestry, TMoveType.mtMountaineering))
            {
                Result = 16;
            }

            if ((MoveTypes & TSet<TMoveType>.Of(TMoveType.mtFlying)) == TSet<TMoveType>.Of(TMoveType.mtFlying))
            {
                Result = 18;
            }

            return Result;
        }

        // AoWWinMan.pas:185
        public int SphereToImage(TMagicSphere Sphere)
        {
            int Result;
            switch (Sphere)
            {
                case TMagicSphere.msLife:
                    Result = 27;
                    break;
                case TMagicSphere.msDeath:
                    Result = 30;
                    break;
                case TMagicSphere.msEarth:
                    Result = 33;
                    break;
                case TMagicSphere.msAir:
                    Result = 36;
                    break;
                case TMagicSphere.msWater:
                    Result = 39;
                    break;
                default:
                    Result = 42;
                    break;
            }

            return Result;
        }

        // AoWWinMan.pas:216
        public void GeneralCreate()
        {
            FUnitShowInfo.Direction = 3;
            FUnitShowInfo.Overlays = TSet<TUnitOverlayModesElement>.Of(TUnitOverlayModesElement.uoRank);
            FAbilities = new TList();
        }

        // AoWWinMan.pas:223
        public void GeneralDestroy()
        {
            for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
            {
                ((TObject)FAbilities[I]).Free();
            }

            FAbilities.Free();
        }

        // AoWWinMan.pas:231
        public void ResetLayout()
        {
            var window = AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
            var alignment = window.Alignment;
            alignment.TopOffset = 0;
            alignment.RightOffset = 0;

            if (AoWWinMan.General.Manager.WinHeight > 700)
            {
                window.WinHeight = (AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight) / 3;
            }
            else
            {
                window.WinHeight = (AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight) / 2;
            }

            window.WinWidth = window.WinHeight * 200 / 216;
            if (AoWHSMap != null)
            {
                window.Visible = AoWHSMap.FCombatMap == null && MWindow.MapWindow.Visible;
            }

            var window2 = AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
            var alignment2 = window2.Alignment;
            alignment2.BottomOffset = 0;
            alignment2.RightOffset = 0;

            window2.WinWidth = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window2.WinHeight = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            if (AoWHSMap != null)
            {
                window2.Visible = AoWHSMap.FCombatMap == null && MWindow.MapWindow.Visible;
            }

            var window3 = AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
            var alignment3 = window3.Alignment;
            alignment3.TopOffset = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            alignment3.RightOffset = 0;

            window3.WinWidth = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window3.WinHeight = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            window3.Visible = false;

            var window4 = AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
            var alignment4 = window4.Alignment;
            alignment4.TopOffset = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            alignment4.RightOffset = 0;

            window4.WinWidth = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window4.WinHeight = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            window4.Visible = false;

            var window5 = AoWWorldMap.AoWWorldMap.TargetSelectionDlg.Dlg;
            var alignment5 = window5.Alignment;
            alignment5.TopOffset = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;
            alignment5.RightOffset = 0;

            window5.WinWidth = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            window5.WinHeight = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinHeight;

            var window6 = RealmWin.Dlg;
            var alignment6 = window6.Alignment;
            alignment6.BottomOffset = TBWindow.TaskWindow.WinHeight;
            alignment6.LeftOffset = 0;
            alignment6.RightOffset = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            alignment6.AlignWidth = TAOWAlignWidth.awBoth;
            alignment6.AlignHeight = TAOWAlignHeight.ahBottom;

            window6.WinWidth = window6.MinWidth;
            window6.WinHeight = window6.MinHeight;
            window6.Visible = false;

            var window7 = MagicWin.Dlg;
            var alignment7 = window7.Alignment;
            alignment7.BottomOffset = TBWindow.TaskWindow.WinHeight;
            alignment7.LeftOffset = 0;
            alignment7.RightOffset = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            alignment7.AlignWidth = TAOWAlignWidth.awBoth;
            alignment7.AlignHeight = TAOWAlignHeight.ahBottom;

            window7.WinWidth = window7.MinWidth;
            window7.WinHeight = window7.MinHeight;
            window7.Visible = false;

            var window8 = AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
            var alignment8 = window8.Alignment;
            alignment8.AlignWidth = TAOWAlignWidth.awCenter;
            alignment8.AlignHeight = TAOWAlignHeight.ahCenter;

            window8.WinWidth = window8.MinWidth;
            window8.WinHeight = window8.MinHeight;
            window8.Visible = false;

            var window9 = ChatWindow.ChatWin;
            var alignment9 = window9.Alignment;
            alignment9.BottomOffset = TBWindow.TaskWindow.WinHeight;
            alignment9.LeftOffset = 0;
            alignment9.RightOffset = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
            alignment9.AlignWidth = TAOWAlignWidth.awBoth;
            alignment9.AlignHeight = TAOWAlignHeight.ahBottom;

            window9.WinWidth = window9.MinWidth;
            window9.WinHeight = window9.MinHeight;
            window9.Visible = false;

            var window10 = TCScanner.TCScannerWin;
            var alignment10 = window10.Alignment;
            alignment10.TopOffset = 0;
            alignment10.RightOffset = 0;

            window10.WinHeight = (AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight) * 170 / 432;
            window10.WinWidth = window10.WinHeight * 150 / 170;
            if (AoWHSMap != null)
            {
                window10.Visible = AoWHSMap.FCombatMap != null && MWindow.MapWindow.Visible;
            }

            var window11 = AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
            var alignment11 = window11.Alignment;
            alignment11.BottomOffset = 0;
            alignment11.RightOffset = 0;

            window11.WinHeight = AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight - TCScanner.TCScannerWin.WinHeight;
            window11.WinWidth = TCScanner.TCScannerWin.WinWidth;
            window11.Visible = false;

            var alignment12 = MWindow.MapWindow.Alignment;
            alignment12.AlignHeight = TAOWAlignHeight.ahBoth;
            alignment12.AlignWidth = TAOWAlignWidth.awBoth;
            alignment12.TopOffset = 0;
            alignment12.LeftOffset = 0;

            MWindow.FWorldAlignHeight = TAOWAlignHeight.ahBoth;
            MWindow.FWorldAlignWidth = TAOWAlignWidth.awBoth;
            var rect = Rect(0, 0, AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth, TBWindow.TaskWindow.WinHeight);
            MWindow.FWorldRect = rect;
            MWindow.FMapAlignHeight = TAOWAlignHeight.ahBoth;
            MWindow.FMapAlignWidth = TAOWAlignWidth.awBoth;
            var rect2 = Rect(0, 0, TCScanner.TCScannerWin.WinWidth, TBWindow.TaskWindow.WinHeight);
            MWindow.FMapRect = rect2;
            if (AoWHSMap != null)
            {
                if (AoWHSMap.FCombatMap == null)
                {
                    MWindow.MapWindow.Alignment.RightOffset = AoWSWindow.AoWSWindow.SWindow.ScannerWindow.WinWidth;
                }
                else
                {
                    MWindow.MapWindow.Alignment.RightOffset = TCScanner.TCScannerWin.WinWidth;
                }
            }

            MWindow.MapWindow.Alignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
            if (ChatWindow.ChatWin.Visible)
            {
                RegisterDialog(ChatWindow.ChatWin);
            }

            Manager.FAligned = false;
        }

        // AoWWinMan.pas:336
        public void ManagerDebugMessage(AnsiString Text)
        {
            DialogWin.AddDebugMessage(Text);
        }

        // AoWWinMan.pas:351
        public void ManagerKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            TPlayer Player;
            AnsiString S;
            if (AoWEngine.FDebug)
            {
                if (AoWHSMap != null && (AoWHSMap.FPlayMode != TPlayMode.pmSimultaneous || AoWHSMap.FPlayers.FLocalPlayers.Count == 1) && TSet<byte>.WithBounds(0, 255).WithRange(49, 57).Contains((byte)Key) && AoWCombatMap == null && AoWHSMap.FPlayers.GetCount() > Key - 48 && AoWEngine.FRegistry.GetDebugMode())
                {
                    Player = AoWHSMap.FPlayers.GetPlayers(Key - 48);
                    if (Player.FPlayerType == TPlayerType.ptCPU5)
                    {
                        Player.FPlayerType = TPlayerType.ptHuman;
                    }
                    else
                    {
                        Player.FPlayerType = (TPlayerType)((int)Player.FPlayerType + 1);
                    }

                    var playerType = TranslateRStr(TPlayerTypeRStr[(int)Player.FPlayerType]);
                    S = (AnsiString)"Player " + IntToStr(Key - 48) + (AnsiString)" set to: " + playerType;
                    AoWHSMap.ShowMessage(S);
                    AoWHSMap.FPlayers.Update();
                    AoWHSMap.FRequestCombatType = AoWHSMap.FPlayers.FLocalHumanPlayers.Count > 1;
                }

                switch (Key)
                {
                    case 122:
                        S = ExtractFileDir(Application.ExeName);
                        S = S + (AnsiString)"\\Scenario\\Custom";
                        SetCurrentDir(S);
                        AoWWinMan.LoadGame(this, (AnsiString)"Crap.hsm");
                        AoWWinMan.StartGame(this);
                        break;
                    case 123:
                        S = ExtractFileDir(Application.ExeName);
                        S = S + (AnsiString)"\\Scenario\\Custom";
                        SetCurrentDir(S);
                        AoWWinMan.LoadGame(this, (AnsiString)"demomap.hsm");
                        AoWWinMan.StartGame(this);
                        break;
                }
            }
        }
    }

    public static partial class AoWWinMan
    {
        public static TGeneral General;
        // AoWWinMan.pas:514
        public static void LoadGame(TGeneral Controller, AnsiString Name)
        {
            if (AoWHSMap != null)
            {
                AoWEngine.CloseHSM(null);
            }

            var e1Loading = TranslateRStr(E1LoadingRStr);
            var fileName = ExtractFileName(Name);
            ProgressDialog.Title.Text = e1Loading + AnsiString.FromBytes(32) + fileName + (AnsiString)"...";
            ProgressDialog.StatusLbl.Text = (AnsiString)"";
            ProgressDialog.Bar.Position = 0;
            ProgressDialog.Bar.Max = 100;
            ProgressDialog.ProgDlg.Visible = true;
            if (!TitleScene.Window.Visible)
            {
                General.Manager.OverlayPriority = 5000;
            }

            SongPlayer.Stop(false);
            SongPlayer.Songs.Clear();
            FadeMusicVolume(0, 1);
            AoWEngine.LoadHSM(Name, false, Controller.MapProgress);
            if (AoWEngine.FDemo && AoWHSMap != null && !AoWHSMap.FShowAllPlayers)
            {
                Controller.ReturnToTitle();
            }

            General.Manager.OverlayPriority = 0;
            ProgressDialog.ProgDlg.Visible = false;
        }

        // AoWWinMan.pas:532
        public static void LoadCampaign(TGeneral Controller, AnsiString Name)
        {
            if (AoWHSMap != null)
            {
                AoWEngine.CloseHSM(null);
            }

            AoWEngine.LoadCampaign(Name);
            if (AoWEngine.FCampaign != null)
            {
                if (AoWEngine.FCampaign.FScenarioStarted)
                {
                    var e1Loading = TranslateRStr(E1LoadingRStr);
                    var fileName = ExtractFileName(Name);
                    ProgressDialog.Title.Text = e1Loading + AnsiString.FromBytes(32) + fileName + (AnsiString)"...";
                    ProgressDialog.StatusLbl.Text = (AnsiString)"";
                    ProgressDialog.Bar.Position = 0;
                    ProgressDialog.Bar.Max = 100;
                    ProgressDialog.ProgDlg.Visible = true;
                    if (!TitleScene.Window.Visible)
                    {
                        General.Manager.OverlayPriority = 5000;
                    }

                    SongPlayer.Stop(false);
                    SongPlayer.Songs.Clear();
                    FadeMusicVolume(0, 1);
                    AoWEngine.LoadCampaignMap(Controller.MapProgress);
                    General.Manager.OverlayPriority = 0;
                    ProgressDialog.ProgDlg.Visible = false;
                }
                else
                {
                    TitleScene.HideScene();
                    ShowCampaignMap(AoWWorldMap.AoWWorldMap.WorldMap);
                }
            }
        }
    }
}
