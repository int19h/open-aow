// Translated from AoWWinMan.pas.
using Aow.Engine.Buffers;
using Aow.Formats;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// preview settings. abilities. active ability count. selected indices. unit list. selected
// unit. combat sprite
public partial class General : DataModule
{
    public General(Component owner) : base(owner)
    { }

    public AOWWinManager Manager;
    public AOWToolTip ToolTip;
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
    public P2UnitShowInfo FUnitShowInfo;
    // are part of FUnitShowInfo
    public DelphiList FAbilities;
    public int FActiveAbilityCount;
    public FixedArray_int_8 FSelectedUnits;
    public Army FUnitList;
    public Structure FStructure;
    public Structure FPreviousStructure;
    public City FCity;
    public ProductionPlace FProductionPlace;
    public ProductionControl FProductionControl;
    public Sign FSign;
    public AbstractUnit FSelectedUnit;
    public TacticalCombatUnitHS FCombatUnit;

    // AoWWinMan.pas:503
    public void ReturnToTitle()
    {
        GameTables.AoWEngine.CloseHSM(null);
        GameTables.AoWEngine.CloseCampaign();
        DialogHosts.SWindow.Scanner.SetMap(null);
        DialogHosts.TCScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(GameTables.AoWEngine.StartupDirectory);
        DialogHosts.General.HideWindows();
        DialogHosts.TBWindow.TaskWindow.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = false;
        DialogHosts.TitleScene.ResetScene();
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
    }

    // AoWWinMan.pas:415
    public void HideWindows()
    {
        if (!FWindowsHidden)
        {
            FWindowsHidden = true;
            FInfoVisible = DialogHosts.PInfoW.PInfoWin.Visible;
            FRealmVisible = DialogHosts.ControlWin.ControlWindow.Visible;
            FMagicVisible = DialogHosts.UnitWindow.UnitWin.Visible;
            FUnitVisible = DialogHosts.RealmWin.Dlg.Visible;
            FTargetVisible = DialogHosts.MagicWin.Dlg.Visible;
            FScannerVisible = DialogHosts.SWindow.ScannerWindow.Visible;
            FControlVisible = DialogHosts.IWindow.InfoWin.Visible;
            FChatVisible = DialogHosts.ChatWindow.ChatWin.Visible;
            FMapVisible = DialogHosts.MWindow.MapWindow.Visible;
            for (int I = 0, last = DialogHosts.General.Manager.FChildren.Count - 1; I <= last; I++)
            {
                if ((AoWComponent)DialogHosts.General.Manager.FChildren[I] != DialogHosts.TBWindow.TaskWindow)
                {
                    ((AoWComponent)DialogHosts.General.Manager.FChildren[I]).Visible = false;
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
            DialogHosts.PInfoW.PInfoWin.Visible = FInfoVisible;
            DialogHosts.ControlWin.ControlWindow.Visible = FRealmVisible;
            DialogHosts.UnitWindow.UnitWin.Visible = FMagicVisible;
            DialogHosts.SWindow.ScannerWindow.Visible = FScannerVisible;
            DialogHosts.IWindow.InfoWin.Visible = FControlVisible;
            DialogHosts.MWindow.MapWindow.Visible = FMapVisible;
            DialogHosts.RealmWin.Dlg.Visible = FUnitVisible;
            DialogHosts.MagicWin.Dlg.Visible = FTargetVisible;
            DialogHosts.ChatWindow.ChatWin.Visible = FChatVisible;
        }
    }

    // AoWWinMan.pas:464
    public void SaveLayout()
    {
        // { Read the saved rectangle through the window pointer cell. }
        var rect = new Rect(DialogHosts.PInfoW.PInfoWin.Alignment.RightOffset, DialogHosts.PInfoW.PInfoWin.Alignment.BottomOffset, DialogHosts.PInfoW.PInfoWin.WinWidth, DialogHosts.PInfoW.PInfoWin.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(0, rect);
        var rect2 = new Rect(DialogHosts.ControlWin.ControlWindow.Alignment.RightOffset, DialogHosts.ControlWin.ControlWindow.Alignment.TopOffset, DialogHosts.ControlWin.ControlWindow.WinWidth, DialogHosts.ControlWin.ControlWindow.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(1, rect2);
        if (DialogHosts.UnitWindow.UnitWin.Alignment.AlignWidth != AOWAlignWidth.Center)
        {
            var rect3 = new Rect(DialogHosts.UnitWindow.UnitWin.Alignment.LeftOffset, DialogHosts.UnitWindow.UnitWin.Alignment.TopOffset, DialogHosts.UnitWindow.UnitWin.WinWidth, DialogHosts.UnitWindow.UnitWin.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }

        if (DialogHosts.RealmWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(DialogHosts.RealmWin.Dlg.Alignment.LeftOffset, DialogHosts.RealmWin.Dlg.Alignment.TopOffset, DialogHosts.RealmWin.Dlg.WinWidth, DialogHosts.RealmWin.Dlg.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }

        if (DialogHosts.MagicWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(DialogHosts.MagicWin.Dlg.Alignment.LeftOffset, DialogHosts.MagicWin.Dlg.Alignment.TopOffset, DialogHosts.MagicWin.Dlg.WinWidth, DialogHosts.MagicWin.Dlg.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }

        var rect4 = new Rect(DialogHosts.SWindow.ScannerWindow.Alignment.RightOffset, DialogHosts.SWindow.ScannerWindow.Alignment.TopOffset, DialogHosts.SWindow.ScannerWindow.WinWidth, DialogHosts.SWindow.ScannerWindow.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(6, rect4);
        var rect5 = new Rect(DialogHosts.IWindow.InfoWin.Alignment.RightOffset, DialogHosts.IWindow.InfoWin.Alignment.TopOffset, DialogHosts.IWindow.InfoWin.WinWidth, DialogHosts.IWindow.InfoWin.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(7, rect5);
        if (DialogHosts.ChatWindow.ChatWin.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(DialogHosts.ChatWindow.ChatWin.Alignment.LeftOffset, DialogHosts.ChatWindow.ChatWin.Alignment.BottomOffset, DialogHosts.ChatWindow.ChatWin.WinWidth, DialogHosts.ChatWindow.ChatWin.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }

        if (DialogHosts.MWindow.MapWindow.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(DialogHosts.MWindow.MapWindow.Alignment.LeftOffset, DialogHosts.MWindow.MapWindow.Alignment.TopOffset, DialogHosts.MWindow.MapWindow.WinWidth, DialogHosts.MWindow.MapWindow.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }

        var rect6 = new Rect(DialogHosts.TCScanner.TCScannerWin.Alignment.RightOffset, DialogHosts.TCScanner.TCScannerWin.Alignment.TopOffset, DialogHosts.TCScanner.TCScannerWin.WinWidth, DialogHosts.TCScanner.TCScannerWin.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(12, rect6);
        var rect7 = new Rect(DialogHosts.TCUnit.TCUnitWin.Alignment.RightOffset, DialogHosts.TCUnit.TCUnitWin.Alignment.BottomOffset, DialogHosts.TCUnit.TCUnitWin.WinWidth, DialogHosts.TCUnit.TCUnitWin.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(10, rect7);
        if (DialogHosts.MWindow.FMapAlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(DialogHosts.MWindow.FMapRect.Left, DialogHosts.MWindow.FMapRect.Top, DialogHosts.MWindow.FMapRect.Right, DialogHosts.MWindow.FMapRect.Bottom);
            GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
    }

    // AoWWinMan.pas:79
    public void PlayButtonSound(byte Kind)
    {
        switch (Kind)
        {
            case 0:
                AudioPlayback.PlaySample("Sfx\\Int\\Btn1.wav");
                break;
            case 3:
                AudioPlayback.PlaySample("Sfx\\Int\\Btn3.wav");
                break;
            case 1:
                AudioPlayback.PlaySample("Sfx\\Int\\Btn5.wav");
                break;
            case 2:
                AudioPlayback.PlaySample("Sfx\\Int\\Btn6.wav");
                break;
        }
    }

    // AoWWinMan.pas:123
    public void SortLists(ref DelphiList Lists, byte Order)
    {
        AOWListBox L;
        int J;
        int InsertAt;
        int ValueI;
        int ValueJ;
        L = (AOWListBox)Lists[0];
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
                    ((AOWListBox)Lists[J]).Items.Move(I, InsertAt);
                }
            }
        }

        for (int I = 0, last = Lists.Count - 1; I <= last; I++)
        {
            L = (AOWListBox)Lists[I];
            L.ListOffset = 0;
            L.Selected = -1;
            L.FSized = false;
        }

        Lists?.Dispose();
    }

    // AoWWinMan.pas:152
    public void RegisterDialog(AOWWindow Window)
    {
        var map = DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth == AOWAlignWidth.Both && Window.Alignment.AlignWidth == AOWAlignWidth.Both)
        {
            map.Alignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight + Window.WinHeight;
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
    public void UnregisterDialog(AOWWindow Window)
    {
        var map = DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth == AOWAlignWidth.Both && Window.Alignment.AlignWidth == AOWAlignWidth.Both && (Window == DialogHosts.ChatWindow.ChatWin || !DialogHosts.ChatWindow.ChatWin.Visible || DialogHosts.ChatWindow.ChatWin.Alignment.AlignWidth != AOWAlignWidth.Both) && (Window == DialogHosts.MagicWin.Dlg || !DialogHosts.MagicWin.Dlg.Visible || DialogHosts.MagicWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both) && (Window == DialogHosts.RealmWin.Dlg || !DialogHosts.RealmWin.Dlg.Visible || DialogHosts.RealmWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both))
        {
            map.Alignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
            map.FAligned = false;
            Window.Alignment.RightOffset = map.Alignment.RightOffset;

            Window.FAligned = false;
        }
    }

    // AoWWinMan.pas:89
    public int IndexOfUnit(AbstractUnit U)
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
        CombatAbilityRef A;
        FActiveAbilityCount = 0;
        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            (FAbilities[I] as IDisposable)?.Dispose();
        }

        FAbilities.Clear();
        for (int I = 0; I <= 169; I++)
        {
            if (FSelectedUnit.GetAbilityEnabled(I))
            {
                A = new CombatAbilityRef();
                A.Ability = I;
                if (DialogHosts.General.FSelectedUnit.FPlayer == AoWHSMap.Current.FSeatedPlayer && (AoWHSSet.Current.FAbilityControl.GetAbility(I).GetControlType(DialogHosts.General.FSelectedUnit) & Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action)) != Set<AbilityControlTypeElement>.Of())
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
    public int MTtoInt(Set<MoveType> MoveTypes)
    {
        int Result = 0;
        if ((MoveTypes & Set<MoveType>.Of(MoveType.Forestry)) == Set<MoveType>.Of(MoveType.Forestry))
        {
            Result = 2;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Mountaineering))
        {
            Result = 4;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Swimming)) == Set<MoveType>.Of(MoveType.Swimming))
        {
            Result = 6;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Walking, MoveType.Swimming)) == Set<MoveType>.Of(MoveType.Walking, MoveType.Swimming))
        {
            Result = 8;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry)) == Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry))
        {
            Result = 10;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Swimming, MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Swimming, MoveType.Mountaineering))
        {
            Result = 12;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Forestry, MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Forestry, MoveType.Mountaineering))
        {
            Result = 14;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry, MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry, MoveType.Mountaineering))
        {
            Result = 16;
        }

        if ((MoveTypes & Set<MoveType>.Of(MoveType.Flying)) == Set<MoveType>.Of(MoveType.Flying))
        {
            Result = 18;
        }

        return Result;
    }

    // AoWWinMan.pas:185
    public int SphereToImage(MagicSphere Sphere)
    {
        int Result;
        switch (Sphere)
        {
            case MagicSphere.Life:
                Result = 27;
                break;
            case MagicSphere.Death:
                Result = 30;
                break;
            case MagicSphere.Earth:
                Result = 33;
                break;
            case MagicSphere.Air:
                Result = 36;
                break;
            case MagicSphere.Water:
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
        FUnitShowInfo.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
        FAbilities = new DelphiList();
    }

    // AoWWinMan.pas:223
    public void GeneralDestroy()
    {
        for (int I = 0, last = FAbilities.Count - 1; I <= last; I++)
        {
            (FAbilities[I] as IDisposable)?.Dispose();
        }

        FAbilities?.Dispose();
    }

    // AoWWinMan.pas:231
    public void ResetLayout()
    {
        var scanner = DialogHosts.SWindow.ScannerWindow;
        var scannerAlignment = scanner.Alignment;
        scannerAlignment.TopOffset = 0;
        scannerAlignment.RightOffset = 0;

        if (DialogHosts.General.Manager.WinHeight > 700)
        {
            scanner.WinHeight = (DialogHosts.General.Manager.WinHeight - DialogHosts.TBWindow.TaskWindow.WinHeight) / 3;
        }
        else
        {
            scanner.WinHeight = (DialogHosts.General.Manager.WinHeight - DialogHosts.TBWindow.TaskWindow.WinHeight) / 2;
        }

        scanner.WinWidth = scanner.WinHeight * 200 / 216;
        if (AoWHSMap.Current != null)
        {
            scanner.Visible = AoWHSMap.Current.FCombatMap == null && DialogHosts.MWindow.MapWindow.Visible;
        }

        var playerInfo = DialogHosts.PInfoW.PInfoWin;
        var playerInfoAlignment = playerInfo.Alignment;
        playerInfoAlignment.BottomOffset = 0;
        playerInfoAlignment.RightOffset = 0;

        playerInfo.WinWidth = scanner.WinWidth;
        playerInfo.WinHeight = scanner.WinHeight;
        if (AoWHSMap.Current != null)
        {
            playerInfo.Visible = AoWHSMap.Current.FCombatMap == null && DialogHosts.MWindow.MapWindow.Visible;
        }

        var controls = DialogHosts.ControlWin.ControlWindow;
        var controlsAlignment = controls.Alignment;
        controlsAlignment.TopOffset = scanner.WinHeight;
        controlsAlignment.RightOffset = 0;

        controls.WinWidth = scanner.WinWidth;
        controls.WinHeight = scanner.WinHeight;
        controls.Visible = false;

        var info = DialogHosts.IWindow.InfoWin;
        var infoAlignment = info.Alignment;
        infoAlignment.TopOffset = scanner.WinHeight;
        infoAlignment.RightOffset = 0;

        info.WinWidth = scanner.WinWidth;
        info.WinHeight = scanner.WinHeight;
        info.Visible = false;

        var targetSelection = DialogHosts.TargetSelectionDlg.Dlg;
        var targetSelectionAlignment = targetSelection.Alignment;
        targetSelectionAlignment.TopOffset = scanner.WinHeight;
        targetSelectionAlignment.RightOffset = 0;

        targetSelection.WinWidth = scanner.WinWidth;
        targetSelection.WinHeight = scanner.WinHeight;

        var realm = DialogHosts.RealmWin.Dlg;
        var realmAlignment = realm.Alignment;
        realmAlignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
        realmAlignment.LeftOffset = 0;
        realmAlignment.RightOffset = scanner.WinWidth;
        realmAlignment.AlignWidth = AOWAlignWidth.Both;
        realmAlignment.AlignHeight = AOWAlignHeight.Bottom;

        realm.WinWidth = realm.MinWidth;
        realm.WinHeight = realm.MinHeight;
        realm.Visible = false;

        var magic = DialogHosts.MagicWin.Dlg;
        var magicAlignment = magic.Alignment;
        magicAlignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
        magicAlignment.LeftOffset = 0;
        magicAlignment.RightOffset = scanner.WinWidth;
        magicAlignment.AlignWidth = AOWAlignWidth.Both;
        magicAlignment.AlignHeight = AOWAlignHeight.Bottom;

        magic.WinWidth = magic.MinWidth;
        magic.WinHeight = magic.MinHeight;
        magic.Visible = false;

        var unitWindow = DialogHosts.UnitWindow.UnitWin;
        var unitWindowAlignment = unitWindow.Alignment;
        unitWindowAlignment.AlignWidth = AOWAlignWidth.Center;
        unitWindowAlignment.AlignHeight = AOWAlignHeight.Center;

        unitWindow.WinWidth = unitWindow.MinWidth;
        unitWindow.WinHeight = unitWindow.MinHeight;
        unitWindow.Visible = false;

        var chat = DialogHosts.ChatWindow.ChatWin;
        var chatAlignment = chat.Alignment;
        chatAlignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
        chatAlignment.LeftOffset = 0;
        chatAlignment.RightOffset = scanner.WinWidth;
        chatAlignment.AlignWidth = AOWAlignWidth.Both;
        chatAlignment.AlignHeight = AOWAlignHeight.Bottom;

        chat.WinWidth = chat.MinWidth;
        chat.WinHeight = chat.MinHeight;
        chat.Visible = false;

        var tcScanner = DialogHosts.TCScanner.TCScannerWin;
        var tcScannerAlignment = tcScanner.Alignment;
        tcScannerAlignment.TopOffset = 0;
        tcScannerAlignment.RightOffset = 0;

        tcScanner.WinHeight = (DialogHosts.General.Manager.WinHeight - DialogHosts.TBWindow.TaskWindow.WinHeight) * 170 / 432;
        tcScanner.WinWidth = tcScanner.WinHeight * 150 / 170;
        if (AoWHSMap.Current != null)
        {
            tcScanner.Visible = AoWHSMap.Current.FCombatMap != null && DialogHosts.MWindow.MapWindow.Visible;
        }

        var tcUnit = DialogHosts.TCUnit.TCUnitWin;
        var tcUnitAlignment = tcUnit.Alignment;
        tcUnitAlignment.BottomOffset = 0;
        tcUnitAlignment.RightOffset = 0;

        tcUnit.WinHeight = DialogHosts.General.Manager.WinHeight - DialogHosts.TBWindow.TaskWindow.WinHeight - tcScanner.WinHeight;
        tcUnit.WinWidth = tcScanner.WinWidth;
        tcUnit.Visible = false;

        var mapAlignment = DialogHosts.MWindow.MapWindow.Alignment;
        mapAlignment.AlignHeight = AOWAlignHeight.Both;
        mapAlignment.AlignWidth = AOWAlignWidth.Both;
        mapAlignment.TopOffset = 0;
        mapAlignment.LeftOffset = 0;

        DialogHosts.MWindow.FWorldAlignHeight = AOWAlignHeight.Both;
        DialogHosts.MWindow.FWorldAlignWidth = AOWAlignWidth.Both;
        var rect = new Rect(0, 0, scanner.WinWidth, DialogHosts.TBWindow.TaskWindow.WinHeight);
        DialogHosts.MWindow.FWorldRect = rect;
        DialogHosts.MWindow.FMapAlignHeight = AOWAlignHeight.Both;
        DialogHosts.MWindow.FMapAlignWidth = AOWAlignWidth.Both;
        var rect2 = new Rect(0, 0, tcScanner.WinWidth, DialogHosts.TBWindow.TaskWindow.WinHeight);
        DialogHosts.MWindow.FMapRect = rect2;
        if (AoWHSMap.Current != null)
        {
            if (AoWHSMap.Current.FCombatMap == null)
            {
                DialogHosts.MWindow.MapWindow.Alignment.RightOffset = scanner.WinWidth;
            }
            else
            {
                DialogHosts.MWindow.MapWindow.Alignment.RightOffset = tcScanner.WinWidth;
            }
        }

        DialogHosts.MWindow.MapWindow.Alignment.BottomOffset = DialogHosts.TBWindow.TaskWindow.WinHeight;
        if (DialogHosts.ChatWindow.ChatWin.Visible)
        {
            RegisterDialog(DialogHosts.ChatWindow.ChatWin);
        }

        Manager.FAligned = false;
    }

    // AoWWinMan.pas:336
    public void ManagerDebugMessage(string Text)
    {
        DialogHosts.DialogWin.AddDebugMessage(Text);
    }

    // AoWWinMan.pas:351
    public void ManagerKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        Player Player;
        string S;
        if (GameTables.AoWEngine.FDebug)
        {
            if (AoWHSMap.Current != null && (AoWHSMap.Current.FPlayMode != PlayMode.Simultaneous || AoWHSMap.Current.FPlayers.FLocalPlayers.Count == 1) && Set<byte>.WithBounds(0, 255).WithRange(49, 57).Contains((byte)Key) && AoWCombatMap.Current == null && AoWHSMap.Current.FPlayers.GetCount() > Key - 48 && GameTables.AoWEngine.FRegistry.GetDebugMode())
            {
                Player = AoWHSMap.Current.FPlayers.GetPlayers(Key - 48);
                if (Player.FPlayerType == PlayerType.CPU5)
                {
                    Player.FPlayerType = PlayerType.Human;
                }
                else
                {
                    Player.FPlayerType = (PlayerType)((int)Player.FPlayerType + 1);
                }

                var playerType = AbilityRules.TranslateRStr(GameTables.TPlayerTypeRStr[(int)Player.FPlayerType]);
                S = "Player " + IntToStr(Key - 48) + " set to: " + playerType;
                AoWHSMap.Current.ShowMessage(S);
                AoWHSMap.Current.FPlayers.Update();
                AoWHSMap.Current.FRequestCombatType = AoWHSMap.Current.FPlayers.FLocalHumanPlayers.Count > 1;
            }

            switch (Key)
            {
                case 122:
                    S = ExtractFileDir(RtlUnit.Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    LoadGame(this, FileBoundary.ReadName("Crap.hsm"));
                    StartGame(this);
                    break;
                case 123:
                    S = ExtractFileDir(RtlUnit.Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    LoadGame(this, FileBoundary.ReadName("demomap.hsm"));
                    StartGame(this);
                    break;
            }
        }
    }

    // AoWWinMan.pas:453 reads the inherited modal field on menu siblings too.
    public void HideTransientWindows()
    {
        for (int i = 0; i < Manager.FChildren.Count; i++)
            if (Manager.FChildren[i] is AOWWindowControl window &&
                window != DialogHosts.MWindow.MapWindow && window.FModal && window.Visible)
                window.Visible = false;
    }

    internal static void HideGameWindows(General Controller)
    {
        if (!Controller.FWindowsHidden)
        {
            Controller.FWindowsHidden = true;
            Controller.FInfoVisible = DialogHosts.PInfoW.PInfoWin.Visible;
            Controller.FRealmVisible = DialogHosts.RealmWin.Dlg.Visible;
            Controller.FMagicVisible = DialogHosts.MagicWin.Dlg.Visible;
            Controller.FUnitVisible = DialogHosts.UnitWindow.UnitWin.Visible;
            Controller.FTargetVisible = DialogHosts.TargetSelectionDlg.Dlg.Visible;
            Controller.FScannerVisible = DialogHosts.SWindow.ScannerWindow.Visible;
            Controller.FControlVisible = DialogHosts.ControlWin.ControlWindow.Visible;
            Controller.FChatVisible = DialogHosts.ChatWindow.ChatWin.Visible;
            Controller.FMapVisible = DialogHosts.MWindow.MapWindow.Visible;
            for (int I = 0, last = DialogHosts.General.Manager.FChildren.Count - 1; I <= last; I++)
            {
                if (DialogHosts.General.Manager.FChildren[I] != DialogHosts.TBWindow.TaskWindow)
                {
                    ((AoWComponent)DialogHosts.General.Manager.FChildren[I]).Visible = false;
                }
            }
        }
    }

    internal static void RestoreGameWindows(General Controller)
    {
        if (Controller.FWindowsHidden)
        {
            Controller.FWindowsHidden = false;
            DialogHosts.PInfoW.PInfoWin.Visible = Controller.FInfoVisible;
            DialogHosts.RealmWin.Dlg.Visible = Controller.FRealmVisible;
            DialogHosts.MagicWin.Dlg.Visible = Controller.FMagicVisible;
            DialogHosts.SWindow.ScannerWindow.Visible = Controller.FScannerVisible;
            DialogHosts.ControlWin.ControlWindow.Visible = Controller.FControlVisible;
            DialogHosts.MWindow.MapWindow.Visible = Controller.FMapVisible;
            DialogHosts.UnitWindow.UnitWin.Visible = Controller.FUnitVisible;
            DialogHosts.TargetSelectionDlg.Dlg.Visible = Controller.FTargetVisible;
            DialogHosts.ChatWindow.ChatWin.Visible = Controller.FChatVisible;
        }
    }

    internal static void ExitToTitle(General Controller)
    {
        GameTables.AoWEngine.CloseHSM(null);
        GameTables.AoWEngine.CloseCampaign();
        DialogHosts.SWindow.Scanner.SetMap(null);
        DialogHosts.TCScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(GameTables.AoWEngine.StartupDirectory);
        HideGameWindows(DialogHosts.General);
        DialogHosts.TBWindow.TaskWindow.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = false;
        DialogHosts.TitleScene.ResetScene();
        DialogHosts.TitleScene.ShowScene(true);
    }

    internal static void SaveWindowLayout(General Controller)
    {
        var playerInfo = DialogHosts.PInfoW.PInfoWin;
        var rect = new Rect(playerInfo.Alignment.RightOffset, playerInfo.Alignment.BottomOffset, playerInfo.WinWidth, playerInfo.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(0, rect);

        var controls = DialogHosts.ControlWin.ControlWindow;
        var rect2 = new Rect(controls.Alignment.RightOffset, controls.Alignment.TopOffset, controls.WinWidth, controls.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(1, rect2);

        var unitWindow = DialogHosts.UnitWindow.UnitWin;
        if (unitWindow.Alignment.AlignWidth != AOWAlignWidth.Center)
        {
            var rect3 = new Rect(unitWindow.Alignment.LeftOffset, unitWindow.Alignment.TopOffset, unitWindow.WinWidth, unitWindow.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }

        var realm = DialogHosts.RealmWin.Dlg;
        if (realm.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(realm.Alignment.LeftOffset, realm.Alignment.TopOffset, realm.WinWidth, realm.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }

        var magic = DialogHosts.MagicWin.Dlg;
        if (magic.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(magic.Alignment.LeftOffset, magic.Alignment.TopOffset, magic.WinWidth, magic.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }

        var scanner = DialogHosts.SWindow.ScannerWindow;
        var rect4 = new Rect(scanner.Alignment.RightOffset, scanner.Alignment.TopOffset, scanner.WinWidth, scanner.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(6, rect4);

        var info = DialogHosts.IWindow.InfoWin;
        var rect5 = new Rect(info.Alignment.RightOffset, info.Alignment.TopOffset, info.WinWidth, info.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(7, rect5);

        var chat = DialogHosts.ChatWindow.ChatWin;
        if (chat.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(chat.Alignment.LeftOffset, chat.Alignment.BottomOffset, chat.WinWidth, chat.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }

        var map = DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(map.Alignment.LeftOffset, map.Alignment.TopOffset, map.WinWidth, map.WinHeight);
            GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }

        var tcUnit = DialogHosts.TCUnit.TCUnitWin;
        var rect6 = new Rect(tcUnit.Alignment.RightOffset, tcUnit.Alignment.TopOffset, tcUnit.WinWidth, tcUnit.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(12, rect6);

        var tcScanner = DialogHosts.TCScanner.TCScannerWin;
        var rect7 = new Rect(tcScanner.Alignment.RightOffset, tcScanner.Alignment.BottomOffset, tcScanner.WinWidth, tcScanner.WinHeight);
        GameTables.AoWEngine.FRegistry.SetRectangle(10, rect7);

        if (DialogHosts.MWindow.FMapAlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = new Rect(DialogHosts.MWindow.FMapRect.Left, DialogHosts.MWindow.FMapRect.Top, DialogHosts.MWindow.FMapRect.Right, DialogHosts.MWindow.FMapRect.Bottom);
            GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
        else
        {
            var rect3 = new Rect(0, 0, 0, 0);
            GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
    }

    internal static bool HasModalWindow(General controller)
    {
        for (int i = controller.Manager.FChildren.Count - 1; i >= 0; i--)
            if (controller.Manager.FChildren[i] is AOWWindowControl window &&
                window != DialogHosts.MWindow.MapWindow && window.FModal && window.Visible)
                return true;
        return false;
    }

    internal static void LoadGame(General Controller, string Name)
    {
        if (AoWHSMap.Current != null)
        {
            GameTables.AoWEngine.CloseHSM(null);
        }

        var e1Loading = AbilityRules.TranslateRStr(LayerText.E1LoadingRStr);
        var fileName = ExtractFileName(Name);
        DialogHosts.ProgressDialog.Title.Text = e1Loading + " " + fileName + "...";
        DialogHosts.ProgressDialog.StatusLbl.Text = "";
        DialogHosts.ProgressDialog.Bar.Position = 0;
        DialogHosts.ProgressDialog.Bar.Max = 100;
        DialogHosts.ProgressDialog.ProgDlg.Visible = true;
        if (!DialogHosts.TitleScene.Window.Visible)
        {
            DialogHosts.General.Manager.OverlayPriority = 5000;
        }

        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        AudioPlayback.FadeMusicVolume(0, 1);
        GameTables.AoWEngine.LoadHSM(Name, false, (_, Message, Progression) => Controller.MapProgress(Message, Progression), GameProfile.WorldMap);
        if (GameTables.AoWEngine.FDemo && AoWHSMap.Current != null && !AoWHSMap.Current.FShowAllPlayers)
        {
            Controller.ReturnToTitle();
        }

        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.ProgressDialog.ProgDlg.Visible = false;
    }

    internal static void StartGame(General controller)
    {
        controller.FWindowsHidden = false;
        AudioHost.SongPlayer.Songs.Clear();
        if (AoWHSMap.Current.FGameSongs.Songs.Count != 0) AudioHost.SongPlayer.Songs.AddStrings(AoWHSMap.Current.FGameSongs.Songs);
        else if (AoWHSMap.Current.FStartupSongs.Songs.Count != 0)
        {
            for (int i = 0; i < AoWHSMap.Current.FStartupSongs.Songs.Count; i++)
                if (RtlUnit.CompareText(AoWHSMap.Current.FStartupSongs.SongNames[i], AbilityRules.TranslateRStr(LayerText.E1FileNotFoundRStr)) != 0)
                    AudioHost.SongPlayer.Songs.Add(AoWHSMap.Current.FStartupSongs.Songs[i]);
        }
        else AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
        AudioHost.SongPlayer.Play(0);
        AudioPlayback.SetMusicVolume(DialogHosts.SoundDlg.MusicVolume.Position);
        var menu = DialogHosts.PopupWindows.OverViewsMenu.Items;
        menu.Clear();
        menu.Add(AbilityRules.TranslateRStr(LayerText.E1BackgroundRStr));
        if (GameTables.AoWEngine.FCampaign != null) menu.Add(AbilityRules.TranslateRStr(LayerText.E1CampaignRStr));
        DialogHosts.TitleScene.Window.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = false;
        DialogHosts.TBWindow.TaskWindow.Visible = true;
        DialogHosts.MWindow.MapWindow.Visible = true;
        DialogHosts.SWindow.ScannerWindow.Visible = true;
        DialogHosts.MWindow.MapViewer.HSMap = AoWHSMap.Current;
        var scanner = DialogHosts.SWindow.Scanner;
        scanner.SetMap(AoWHSMap.Current);
        scanner.SetLevel(AoWHSMap.Current.MapContainer.GetMapLevel(0));
        DialogHosts.TBWindow.TurnButton.FButtonChecked = false;
        DialogHosts.TBWindow.ChatBtn.Visible = DialogHosts.SetupEvents.DPlay.Connected;
        DialogHosts.General.Manager.FAligned = false;
        scanner.FPreviousWidth = 0;
        scanner.FIncremental = false;
        scanner.FSized = false;
        var tabs = DialogHosts.SWindow.ScannerTab;
        tabs.Items.Clear();
        tabs.Items.Add(AbilityRules.TranslateRStr(LayerText.E1SurfaceRStr));
        if (AoWHSMap.Current.MapContainer.LevelCount > 1) tabs.Items.Add(AbilityRules.TranslateRStr(LayerText.E1CavernsRStr));
        if (AoWHSMap.Current.MapContainer.LevelCount > 2) tabs.Items.Add(AbilityRules.TranslateRStr(LayerText.E1DepthsRStr));
        tabs.Selected = 0;
        tabs.FSized = false;
        scanner.SetTWidth(0);
        scanner.SetTHeight(0);
        scanner.SetTLeft(-1);
        scanner.SetTTop(-1);
        scanner.FUpdateStep = -1;
        scanner.SetMap(AoWHSMap.Current);
        scanner.SetLevel(AoWHSMap.Current.MapContainer.GetMapLevel(0));
        DialogHosts.MWindow.MapViewer.HSMap = AoWHSMap.Current;
        DialogHosts.MWindow.MapViewer.SceneWidth = DialogHosts.MWindow.MapWindow.WinWidth - 24;
        DialogHosts.MWindow.MapViewer.SceneHeight = DialogHosts.MWindow.MapWindow.WinHeight - 24;
        AoWHSMap.Current.Activate();
        DialogHosts.RealmWin.Dlg.Visible = false;
        DialogHosts.MapEvents.TheMapArmyDeselected();
        DialogHosts.MapEvents.TheMapStructureDeselected();
        // The original call at targets the indicator sizing routine
        DialogHosts.TBWindow.UpdatePlayerIndicator();
        DialogHosts.PInfoW.UpdatePlayer();
        if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Activated))
        {
            var turn = DialogHosts.TBWindow.TurnButton;
            if (AoWHSMap.Current.FSeatedPlayer != -1)
            {
                turn.FLastPlayer = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FPlayerRace;
                if (turn.FLastPlayer == -1) turn.FLastPlayer = 12;
                turn.FCurPlayer = turn.FLastPlayer;
                turn.FAnimationFrame = turn.FCurPlayer * 7;
                DialogHosts.TBWindow.CurP.Visible = AoWHSMap.Current.FPlayMode == PlayMode.Turn;
            }
            else
            {
                turn.FCurPlayer = 12;
                turn.FLastPlayer = turn.FCurPlayer;
                turn.FAnimationFrame = turn.FCurPlayer * 7;
                DialogHosts.TBWindow.CurP.Visible = false;
            }
        }
        DialogHosts.TBWindow.FWorldClockDelay = 0;
        DialogHosts.TBWindow.ChatBtn.ILIndexUp = 14;
        DialogHosts.TBWindow.FChatFlash = 0;
        DialogHosts.VideoDialog.SetSceneSize(DialogHosts.VideoDialog.FDisplayWidth, DialogHosts.VideoDialog.FDisplayHeight);
    }

}
