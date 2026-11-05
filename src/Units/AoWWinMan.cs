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
using Aow.Units.AoWWinControl;

namespace Aow.Units.AoWWinMan;

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
                PlaySample("Sfx\\Int\\Btn1.wav");
                break;
            case 3:
                PlaySample("Sfx\\Int\\Btn3.wav");
                break;
            case 1:
                PlaySample("Sfx\\Int\\Btn5.wav");
                break;
            case 2:
                PlaySample("Sfx\\Int\\Btn6.wav");
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
                        catch
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
                        catch
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
        var map = MWindow.MapWindow;
        if (map.Alignment.AlignWidth == TAOWAlignWidth.awBoth && Window.Alignment.AlignWidth == TAOWAlignWidth.awBoth)
        {
            map.Alignment.BottomOffset = TBWindow.TaskWindow.WinHeight + Window.WinHeight;
            map.FAligned = false;
            if (map.Visible)
            {
                Window.Alignment.RightOffset = map.Alignment.RightOffset;
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
        var map = MWindow.MapWindow;
        if (map.Alignment.AlignWidth == TAOWAlignWidth.awBoth && Window.Alignment.AlignWidth == TAOWAlignWidth.awBoth && (Window == ChatWindow.ChatWin || !ChatWindow.ChatWin.Visible || ChatWindow.ChatWin.Alignment.AlignWidth != TAOWAlignWidth.awBoth) && (Window == MagicWin.Dlg || !MagicWin.Dlg.Visible || MagicWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth) && (Window == RealmWin.Dlg || !RealmWin.Dlg.Visible || RealmWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth))
        {
            map.Alignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
            map.FAligned = false;
            Window.Alignment.RightOffset = map.Alignment.RightOffset;

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
        var scanner = AoWSWindow.AoWSWindow.SWindow.ScannerWindow;
        var scannerAlignment = scanner.Alignment;
        scannerAlignment.TopOffset = 0;
        scannerAlignment.RightOffset = 0;

        if (AoWWinMan.General.Manager.WinHeight > 700)
        {
            scanner.WinHeight = (AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight) / 3;
        }
        else
        {
            scanner.WinHeight = (AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight) / 2;
        }

        scanner.WinWidth = scanner.WinHeight * 200 / 216;
        if (AoWHSMap != null)
        {
            scanner.Visible = AoWHSMap.FCombatMap == null && MWindow.MapWindow.Visible;
        }

        var playerInfo = AoWPInfoWin.AoWPInfoWin.PInfoW.PInfoWin;
        var playerInfoAlignment = playerInfo.Alignment;
        playerInfoAlignment.BottomOffset = 0;
        playerInfoAlignment.RightOffset = 0;

        playerInfo.WinWidth = scanner.WinWidth;
        playerInfo.WinHeight = scanner.WinHeight;
        if (AoWHSMap != null)
        {
            playerInfo.Visible = AoWHSMap.FCombatMap == null && MWindow.MapWindow.Visible;
        }

        var controls = AOWControlWin.AOWControlWin.ControlWin.ControlWindow;
        var controlsAlignment = controls.Alignment;
        controlsAlignment.TopOffset = scanner.WinHeight;
        controlsAlignment.RightOffset = 0;

        controls.WinWidth = scanner.WinWidth;
        controls.WinHeight = scanner.WinHeight;
        controls.Visible = false;

        var info = AoWInfoWin.AoWInfoWin.IWindow.InfoWin;
        var infoAlignment = info.Alignment;
        infoAlignment.TopOffset = scanner.WinHeight;
        infoAlignment.RightOffset = 0;

        info.WinWidth = scanner.WinWidth;
        info.WinHeight = scanner.WinHeight;
        info.Visible = false;

        var targetSelection = AoWWorldMap.AoWWorldMap.TargetSelectionDlg.Dlg;
        var targetSelectionAlignment = targetSelection.Alignment;
        targetSelectionAlignment.TopOffset = scanner.WinHeight;
        targetSelectionAlignment.RightOffset = 0;

        targetSelection.WinWidth = scanner.WinWidth;
        targetSelection.WinHeight = scanner.WinHeight;

        var realm = RealmWin.Dlg;
        var realmAlignment = realm.Alignment;
        realmAlignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
        realmAlignment.LeftOffset = 0;
        realmAlignment.RightOffset = scanner.WinWidth;
        realmAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        realmAlignment.AlignHeight = TAOWAlignHeight.ahBottom;

        realm.WinWidth = realm.MinWidth;
        realm.WinHeight = realm.MinHeight;
        realm.Visible = false;

        var magic = MagicWin.Dlg;
        var magicAlignment = magic.Alignment;
        magicAlignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
        magicAlignment.LeftOffset = 0;
        magicAlignment.RightOffset = scanner.WinWidth;
        magicAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        magicAlignment.AlignHeight = TAOWAlignHeight.ahBottom;

        magic.WinWidth = magic.MinWidth;
        magic.WinHeight = magic.MinHeight;
        magic.Visible = false;

        var unitWindow = AoWUnitWindow.AoWUnitWindow.UnitWindow.UnitWin;
        var unitWindowAlignment = unitWindow.Alignment;
        unitWindowAlignment.AlignWidth = TAOWAlignWidth.awCenter;
        unitWindowAlignment.AlignHeight = TAOWAlignHeight.ahCenter;

        unitWindow.WinWidth = unitWindow.MinWidth;
        unitWindow.WinHeight = unitWindow.MinHeight;
        unitWindow.Visible = false;

        var chat = ChatWindow.ChatWin;
        var chatAlignment = chat.Alignment;
        chatAlignment.BottomOffset = TBWindow.TaskWindow.WinHeight;
        chatAlignment.LeftOffset = 0;
        chatAlignment.RightOffset = scanner.WinWidth;
        chatAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        chatAlignment.AlignHeight = TAOWAlignHeight.ahBottom;

        chat.WinWidth = chat.MinWidth;
        chat.WinHeight = chat.MinHeight;
        chat.Visible = false;

        var tcScanner = TCScanner.TCScannerWin;
        var tcScannerAlignment = tcScanner.Alignment;
        tcScannerAlignment.TopOffset = 0;
        tcScannerAlignment.RightOffset = 0;

        tcScanner.WinHeight = (AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight) * 170 / 432;
        tcScanner.WinWidth = tcScanner.WinHeight * 150 / 170;
        if (AoWHSMap != null)
        {
            tcScanner.Visible = AoWHSMap.FCombatMap != null && MWindow.MapWindow.Visible;
        }

        var tcUnit = AoWTCUnit.AoWTCUnit.TCUnit.TCUnitWin;
        var tcUnitAlignment = tcUnit.Alignment;
        tcUnitAlignment.BottomOffset = 0;
        tcUnitAlignment.RightOffset = 0;

        tcUnit.WinHeight = AoWWinMan.General.Manager.WinHeight - TBWindow.TaskWindow.WinHeight - tcScanner.WinHeight;
        tcUnit.WinWidth = tcScanner.WinWidth;
        tcUnit.Visible = false;

        var mapAlignment = MWindow.MapWindow.Alignment;
        mapAlignment.AlignHeight = TAOWAlignHeight.ahBoth;
        mapAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        mapAlignment.TopOffset = 0;
        mapAlignment.LeftOffset = 0;

        MWindow.FWorldAlignHeight = TAOWAlignHeight.ahBoth;
        MWindow.FWorldAlignWidth = TAOWAlignWidth.awBoth;
        var rect = Rect(0, 0, scanner.WinWidth, TBWindow.TaskWindow.WinHeight);
        MWindow.FWorldRect = rect;
        MWindow.FMapAlignHeight = TAOWAlignHeight.ahBoth;
        MWindow.FMapAlignWidth = TAOWAlignWidth.awBoth;
        var rect2 = Rect(0, 0, tcScanner.WinWidth, TBWindow.TaskWindow.WinHeight);
        MWindow.FMapRect = rect2;
        if (AoWHSMap != null)
        {
            if (AoWHSMap.FCombatMap == null)
            {
                MWindow.MapWindow.Alignment.RightOffset = scanner.WinWidth;
            }
            else
            {
                MWindow.MapWindow.Alignment.RightOffset = tcScanner.WinWidth;
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
    public void ManagerDebugMessage(string Text)
    {
        DialogWin.AddDebugMessage(Text);
    }

    // AoWWinMan.pas:351
    public void ManagerKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        TPlayer Player;
        string S;
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
                S = "Player " + IntToStr(Key - 48) + " set to: " + playerType;
                AoWHSMap.ShowMessage(S);
                AoWHSMap.FPlayers.Update();
                AoWHSMap.FRequestCombatType = AoWHSMap.FPlayers.FLocalHumanPlayers.Count > 1;
            }

            switch (Key)
            {
                case 122:
                    S = ExtractFileDir(Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    AoWWinMan.LoadGame(this, "Crap.hsm");
                    AoWWinMan.StartGame(this);
                    break;
                case 123:
                    S = ExtractFileDir(Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    AoWWinMan.LoadGame(this, "demomap.hsm");
                    AoWWinMan.StartGame(this);
                    break;
            }
        }
    }

    // AoWWinMan.pas:453 reads the inherited modal field on menu siblings too.
    public void HideTransientWindows()
    {
        for (int i = 0; i < Manager.FChildren.Count; i++)
            if (Manager.FChildren[i] is TAOWWindowControl window &&
                window != AOWMWindow.AOWMWindow.MWindow.MapWindow && window.FModal && window.Visible)
                window.Visible = false;
    }
}

public static partial class AoWWinMan
{
    public static TGeneral General;
    // AoWWinMan.pas:514
    public static void LoadGame(TGeneral Controller, string Name)
    {
        if (AoWHSMap != null)
        {
            AoWEngine.CloseHSM(null);
        }

        var e1Loading = TranslateRStr(E1LoadingRStr);
        var fileName = ExtractFileName(Name);
        ProgressDialog.Title.Text = e1Loading + " " + fileName + "...";
        ProgressDialog.StatusLbl.Text = "";
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
    public static void LoadCampaign(TGeneral Controller, string Name)
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
                ProgressDialog.Title.Text = e1Loading + " " + fileName + "...";
                ProgressDialog.StatusLbl.Text = "";
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
