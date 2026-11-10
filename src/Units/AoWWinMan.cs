// Translated from AoWWinMan.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine.Buffers;
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
    public General(global::Aow.Rtl.Component owner) : base(owner)
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
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseCampaign();
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetMap(null);
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetMap(null);
        Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory);
        Aow.Game.Ui.Dialogs.DialogHosts.General.HideWindows();
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.ResetScene();
        Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, true);
    }

    // AoWWinMan.pas:415
    public void HideWindows()
    {
        if (!FWindowsHidden)
        {
            FWindowsHidden = true;
            FInfoVisible = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible;
            FRealmVisible = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible;
            FMagicVisible = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible;
            FUnitVisible = Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible;
            FTargetVisible = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible;
            FScannerVisible = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible;
            FControlVisible = Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Visible;
            FChatVisible = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible;
            FMapVisible = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
            for (int I = 0, last = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren.Count - 1; I <= last; I++)
            {
                if ((AoWComponent)Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I] != Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow)
                {
                    ((AoWComponent)Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I]).Visible = false;
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
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible = FInfoVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible = FRealmVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = FMagicVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible = FScannerVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Visible = FControlVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible = FMapVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible = FUnitVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible = FTargetVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible = FChatVisible;
        }
    }

    // AoWWinMan.pas:464
    public void SaveLayout()
    {
        // { Read the saved rectangle through the window pointer cell. }
        var rect = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Alignment.BottomOffset, Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(0, rect);
        var rect2 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(1, rect2);
        if (Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Alignment.AlignWidth != AOWAlignWidth.Center)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }

        var rect4 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(6, rect4);
        var rect5 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(7, rect5);
        if (Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.BottomOffset, Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }

        var rect6 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(12, rect6);
        var rect7 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Alignment.BottomOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(10, rect7);
        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Left, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Top, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
    }

    // AoWWinMan.pas:79
    public void PlayButtonSound(byte Kind)
    {
        switch (Kind)
        {
            case 0:
                Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\Btn1.wav");
                break;
            case 3:
                Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\Btn3.wav");
                break;
            case 1:
                Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\Btn5.wav");
                break;
            case 2:
                Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\Btn6.wav");
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
        var map = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth == AOWAlignWidth.Both && Window.Alignment.AlignWidth == AOWAlignWidth.Both)
        {
            map.Alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight + Window.WinHeight;
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
        var map = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth == AOWAlignWidth.Both && Window.Alignment.AlignWidth == AOWAlignWidth.Both && (Window == Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin || !Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible || Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.AlignWidth != AOWAlignWidth.Both) && (Window == Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg || !Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible || Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both) && (Window == Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg || !Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible || Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.AlignWidth != AOWAlignWidth.Both))
        {
            map.Alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
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
        TCAbility A;
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
                A = new TCAbility();
                A.Ability = I;
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.FPlayer == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer && (Aow.Game.Engine.AoWHSSet.Current.FAbilityControl.GetAbility(I).GetControlType(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit) & Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action)) != Set<AbilityControlTypeElement>.Of())
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
        var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow;
        var scannerAlignment = scanner.Alignment;
        scannerAlignment.TopOffset = 0;
        scannerAlignment.RightOffset = 0;

        if (Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight > 700)
        {
            scanner.WinHeight = (Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight - Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight) / 3;
        }
        else
        {
            scanner.WinHeight = (Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight - Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight) / 2;
        }

        scanner.WinWidth = scanner.WinHeight * 200 / 216;
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
        {
            scanner.Visible = Aow.Game.World.Map.AoWHSMap.Current.FCombatMap == null && Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
        }

        var playerInfo = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin;
        var playerInfoAlignment = playerInfo.Alignment;
        playerInfoAlignment.BottomOffset = 0;
        playerInfoAlignment.RightOffset = 0;

        playerInfo.WinWidth = scanner.WinWidth;
        playerInfo.WinHeight = scanner.WinHeight;
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
        {
            playerInfo.Visible = Aow.Game.World.Map.AoWHSMap.Current.FCombatMap == null && Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
        }

        var controls = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow;
        var controlsAlignment = controls.Alignment;
        controlsAlignment.TopOffset = scanner.WinHeight;
        controlsAlignment.RightOffset = 0;

        controls.WinWidth = scanner.WinWidth;
        controls.WinHeight = scanner.WinHeight;
        controls.Visible = false;

        var info = Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin;
        var infoAlignment = info.Alignment;
        infoAlignment.TopOffset = scanner.WinHeight;
        infoAlignment.RightOffset = 0;

        info.WinWidth = scanner.WinWidth;
        info.WinHeight = scanner.WinHeight;
        info.Visible = false;

        var targetSelection = Aow.Game.Ui.Dialogs.DialogHosts.TargetSelectionDlg.Dlg;
        var targetSelectionAlignment = targetSelection.Alignment;
        targetSelectionAlignment.TopOffset = scanner.WinHeight;
        targetSelectionAlignment.RightOffset = 0;

        targetSelection.WinWidth = scanner.WinWidth;
        targetSelection.WinHeight = scanner.WinHeight;

        var realm = Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg;
        var realmAlignment = realm.Alignment;
        realmAlignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
        realmAlignment.LeftOffset = 0;
        realmAlignment.RightOffset = scanner.WinWidth;
        realmAlignment.AlignWidth = AOWAlignWidth.Both;
        realmAlignment.AlignHeight = AOWAlignHeight.Bottom;

        realm.WinWidth = realm.MinWidth;
        realm.WinHeight = realm.MinHeight;
        realm.Visible = false;

        var magic = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg;
        var magicAlignment = magic.Alignment;
        magicAlignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
        magicAlignment.LeftOffset = 0;
        magicAlignment.RightOffset = scanner.WinWidth;
        magicAlignment.AlignWidth = AOWAlignWidth.Both;
        magicAlignment.AlignHeight = AOWAlignHeight.Bottom;

        magic.WinWidth = magic.MinWidth;
        magic.WinHeight = magic.MinHeight;
        magic.Visible = false;

        var unitWindow = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin;
        var unitWindowAlignment = unitWindow.Alignment;
        unitWindowAlignment.AlignWidth = AOWAlignWidth.Center;
        unitWindowAlignment.AlignHeight = AOWAlignHeight.Center;

        unitWindow.WinWidth = unitWindow.MinWidth;
        unitWindow.WinHeight = unitWindow.MinHeight;
        unitWindow.Visible = false;

        var chat = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin;
        var chatAlignment = chat.Alignment;
        chatAlignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
        chatAlignment.LeftOffset = 0;
        chatAlignment.RightOffset = scanner.WinWidth;
        chatAlignment.AlignWidth = AOWAlignWidth.Both;
        chatAlignment.AlignHeight = AOWAlignHeight.Bottom;

        chat.WinWidth = chat.MinWidth;
        chat.WinHeight = chat.MinHeight;
        chat.Visible = false;

        var tcScanner = Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin;
        var tcScannerAlignment = tcScanner.Alignment;
        tcScannerAlignment.TopOffset = 0;
        tcScannerAlignment.RightOffset = 0;

        tcScanner.WinHeight = (Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight - Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight) * 170 / 432;
        tcScanner.WinWidth = tcScanner.WinHeight * 150 / 170;
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
        {
            tcScanner.Visible = Aow.Game.World.Map.AoWHSMap.Current.FCombatMap != null && Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
        }

        var tcUnit = Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin;
        var tcUnitAlignment = tcUnit.Alignment;
        tcUnitAlignment.BottomOffset = 0;
        tcUnitAlignment.RightOffset = 0;

        tcUnit.WinHeight = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight - Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight - tcScanner.WinHeight;
        tcUnit.WinWidth = tcScanner.WinWidth;
        tcUnit.Visible = false;

        var mapAlignment = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment;
        mapAlignment.AlignHeight = AOWAlignHeight.Both;
        mapAlignment.AlignWidth = AOWAlignWidth.Both;
        mapAlignment.TopOffset = 0;
        mapAlignment.LeftOffset = 0;

        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignHeight = AOWAlignHeight.Both;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignWidth = AOWAlignWidth.Both;
        var rect = global::Aow.Rtl.RtlUnit.Rect(0, 0, scanner.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight);
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect = rect;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignHeight = AOWAlignHeight.Both;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth = AOWAlignWidth.Both;
        var rect2 = global::Aow.Rtl.RtlUnit.Rect(0, 0, tcScanner.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight);
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect = rect2;
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
        {
            if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap == null)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.RightOffset = scanner.WinWidth;
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.RightOffset = tcScanner.WinWidth;
            }
        }

        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
        if (Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible)
        {
            RegisterDialog(Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin);
        }

        Manager.FAligned = false;
    }

    // AoWWinMan.pas:336
    public void ManagerDebugMessage(string Text)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.AddDebugMessage(Text);
    }

    // AoWWinMan.pas:351
    public void ManagerKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        Player Player;
        string S;
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDebug)
        {
            if (Aow.Game.World.Map.AoWHSMap.Current != null && (Aow.Game.World.Map.AoWHSMap.Current.FPlayMode != PlayMode.Simultaneous || Aow.Game.World.Map.AoWHSMap.Current.FPlayers.FLocalPlayers.Count == 1) && Set<byte>.WithBounds(0, 255).WithRange(49, 57).Contains((byte)Key) && Aow.Game.Combat.AoWCombatMap.Current == null && Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetCount() > Key - 48 && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDebugMode())
            {
                Player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Key - 48);
                if (Player.FPlayerType == PlayerType.CPU5)
                {
                    Player.FPlayerType = PlayerType.Human;
                }
                else
                {
                    Player.FPlayerType = (PlayerType)((int)Player.FPlayerType + 1);
                }

                var playerType = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TPlayerTypeRStr[(int)Player.FPlayerType]);
                S = "Player " + IntToStr(Key - 48) + " set to: " + playerType;
                Aow.Game.World.Map.AoWHSMap.Current.ShowMessage(S);
                Aow.Game.World.Map.AoWHSMap.Current.FPlayers.Update();
                Aow.Game.World.Map.AoWHSMap.Current.FRequestCombatType = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.FLocalHumanPlayers.Count > 1;
            }

            switch (Key)
            {
                case 122:
                    S = ExtractFileDir(global::Aow.Rtl.RtlUnit.Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    Aow.Game.Ui.Dialogs.General.LoadGame(this, FileBoundary.ReadName("Crap.hsm"));
                    Aow.Game.Ui.Dialogs.General.StartGame(this);
                    break;
                case 123:
                    S = ExtractFileDir(global::Aow.Rtl.RtlUnit.Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    Aow.Game.Ui.Dialogs.General.LoadGame(this, FileBoundary.ReadName("demomap.hsm"));
                    Aow.Game.Ui.Dialogs.General.StartGame(this);
                    break;
            }
        }
    }

    // AoWWinMan.pas:453 reads the inherited modal field on menu siblings too.
    public void HideTransientWindows()
    {
        for (int i = 0; i < Manager.FChildren.Count; i++)
            if (Manager.FChildren[i] is AOWWindowControl window &&
                window != Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow && window.FModal && window.Visible)
                window.Visible = false;
    }

    internal static void HideGameWindows(General Controller)
    {
        if (!Controller.FWindowsHidden)
        {
            Controller.FWindowsHidden = true;
            Controller.FInfoVisible = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible;
            Controller.FRealmVisible = Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible;
            Controller.FMagicVisible = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible;
            Controller.FUnitVisible = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible;
            Controller.FTargetVisible = Aow.Game.Ui.Dialogs.DialogHosts.TargetSelectionDlg.Dlg.Visible;
            Controller.FScannerVisible = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible;
            Controller.FControlVisible = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible;
            Controller.FChatVisible = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible;
            Controller.FMapVisible = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
            for (int I = 0, last = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren.Count - 1; I <= last; I++)
            {
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I] != Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow)
                {
                    ((Aow.Game.Ui.Controls.AoWComponent)Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I]).Visible = false;
                }
            }
        }
    }

    internal static void RestoreGameWindows(General Controller)
    {
        if (Controller.FWindowsHidden)
        {
            Controller.FWindowsHidden = false;
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Visible = Controller.FInfoVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible = Controller.FRealmVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible = Controller.FMagicVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible = Controller.FScannerVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible = Controller.FControlVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible = Controller.FMapVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = Controller.FUnitVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.TargetSelectionDlg.Dlg.Visible = Controller.FTargetVisible;
            Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible = Controller.FChatVisible;
        }
    }

    internal static void ExitToTitle(General Controller)
    {
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseCampaign();
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetMap(null);
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetMap(null);
        Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory);
        Aow.Game.Ui.Dialogs.General.HideGameWindows(Aow.Game.Ui.Dialogs.DialogHosts.General);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.ResetScene();
        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.ShowScene(true);
    }

    internal static void SaveWindowLayout(General Controller)
    {
        var playerInfo = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin;
        var rect = global::Aow.Rtl.RtlUnit.Rect(playerInfo.Alignment.RightOffset, playerInfo.Alignment.BottomOffset, playerInfo.WinWidth, playerInfo.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(0, rect);

        var controls = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow;
        var rect2 = global::Aow.Rtl.RtlUnit.Rect(controls.Alignment.RightOffset, controls.Alignment.TopOffset, controls.WinWidth, controls.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(1, rect2);

        var unitWindow = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin;
        if (unitWindow.Alignment.AlignWidth != AOWAlignWidth.Center)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(unitWindow.Alignment.LeftOffset, unitWindow.Alignment.TopOffset, unitWindow.WinWidth, unitWindow.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }

        var realm = Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg;
        if (realm.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(realm.Alignment.LeftOffset, realm.Alignment.TopOffset, realm.WinWidth, realm.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }

        var magic = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg;
        if (magic.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(magic.Alignment.LeftOffset, magic.Alignment.TopOffset, magic.WinWidth, magic.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }

        var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow;
        var rect4 = global::Aow.Rtl.RtlUnit.Rect(scanner.Alignment.RightOffset, scanner.Alignment.TopOffset, scanner.WinWidth, scanner.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(6, rect4);

        var info = Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin;
        var rect5 = global::Aow.Rtl.RtlUnit.Rect(info.Alignment.RightOffset, info.Alignment.TopOffset, info.WinWidth, info.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(7, rect5);

        var chat = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin;
        if (chat.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(chat.Alignment.LeftOffset, chat.Alignment.BottomOffset, chat.WinWidth, chat.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }

        var map = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(map.Alignment.LeftOffset, map.Alignment.TopOffset, map.WinWidth, map.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }

        var tcUnit = Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin;
        var rect6 = global::Aow.Rtl.RtlUnit.Rect(tcUnit.Alignment.RightOffset, tcUnit.Alignment.TopOffset, tcUnit.WinWidth, tcUnit.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(12, rect6);

        var tcScanner = Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin;
        var rect7 = global::Aow.Rtl.RtlUnit.Rect(tcScanner.Alignment.RightOffset, tcScanner.Alignment.BottomOffset, tcScanner.WinWidth, tcScanner.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(10, rect7);

        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth != AOWAlignWidth.Both)
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Left, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Top, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
        else
        {
            var rect3 = global::Aow.Rtl.RtlUnit.Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
    }

    internal static bool HasModalWindow(General controller)
    {
        for (int i = controller.Manager.FChildren.Count - 1; i >= 0; i--)
            if (controller.Manager.FChildren[i] is AOWWindowControl window &&
                window != Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow && window.FModal && window.Visible)
                return true;
        return false;
    }

    internal static void LoadGame(General Controller, string Name)
    {
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
        {
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
        }

        var e1Loading = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1LoadingRStr);
        var fileName = ExtractFileName(Name);
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Title.Text = e1Loading + " " + fileName + "...";
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.StatusLbl.Text = "";
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Bar.Position = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.Bar.Max = 100;
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible = true;
        if (!Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.Window.Visible)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 5000;
        }

        Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        Aow.Game.Audio.AudioPlayback.FadeMusicVolume(0, 1);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.LoadHSM(Name, false, (_, Message, Progression) => Controller.MapProgress(Message, Progression), Aow.Formats.GameProfile.WorldMap);
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo && Aow.Game.World.Map.AoWHSMap.Current != null && !Aow.Game.World.Map.AoWHSMap.Current.FShowAllPlayers)
        {
            Controller.ReturnToTitle();
        }

        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible = false;
    }

    internal static void StartGame(General controller)
    {
        controller.FWindowsHidden = false;
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        if (Aow.Game.World.Map.AoWHSMap.Current.FGameSongs.Songs.Count != 0) Aow.Game.Audio.AudioHost.SongPlayer.Songs.AddStrings(Aow.Game.World.Map.AoWHSMap.Current.FGameSongs.Songs);
        else if (Aow.Game.World.Map.AoWHSMap.Current.FStartupSongs.Songs.Count != 0)
        {
            for (int i = 0; i < Aow.Game.World.Map.AoWHSMap.Current.FStartupSongs.Songs.Count; i++)
                if (RtlUnit.CompareText(Aow.Game.World.Map.AoWHSMap.Current.FStartupSongs.SongNames[i], Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1FileNotFoundRStr)) != 0)
                    Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add(Aow.Game.World.Map.AoWHSMap.Current.FStartupSongs.Songs[i]);
        }
        else Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
        Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        Aow.Game.Audio.AudioPlayback.SetMusicVolume(Aow.Game.Ui.Dialogs.DialogHosts.SoundDlg.MusicVolume.Position);
        var menu = Aow.Game.Ui.Dialogs.DialogHosts.PopupWindows.OverViewsMenu.Items;
        menu.Clear();
        menu.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1BackgroundRStr));
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FCampaign != null) menu.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1CampaignRStr));
        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.Window.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.HSMap = Aow.Game.World.Map.AoWHSMap.Current;
        var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner;
        scanner.SetMap(Aow.Game.World.Map.AoWHSMap.Current);
        scanner.SetLevel(Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(0));
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FButtonChecked = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ChatBtn.Visible = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.Connected;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FAligned = false;
        scanner.FPreviousWidth = 0;
        scanner.FIncremental = false;
        scanner.FSized = false;
        var tabs = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab;
        tabs.Items.Clear();
        tabs.Items.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1SurfaceRStr));
        if (Aow.Game.World.Map.AoWHSMap.Current.MapContainer.LevelCount > 1) tabs.Items.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1CavernsRStr));
        if (Aow.Game.World.Map.AoWHSMap.Current.MapContainer.LevelCount > 2) tabs.Items.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1DepthsRStr));
        tabs.Selected = 0;
        tabs.FSized = false;
        scanner.SetTWidth(0);
        scanner.SetTHeight(0);
        scanner.SetTLeft(-1);
        scanner.SetTTop(-1);
        scanner.FUpdateStep = -1;
        scanner.SetMap(Aow.Game.World.Map.AoWHSMap.Current);
        scanner.SetLevel(Aow.Game.World.Map.AoWHSMap.Current.MapContainer.GetMapLevel(0));
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.HSMap = Aow.Game.World.Map.AoWHSMap.Current;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth - 24;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight - 24;
        Aow.Game.World.Map.AoWHSMap.Current.Activate();
        Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapArmyDeselected();
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapStructureDeselected();
        // The original call at targets the indicator sizing routine
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.UpdatePlayerIndicator();
        Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdatePlayer();
        if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Activated))
        {
            var turn = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton;
            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer != -1)
            {
                turn.FLastPlayer = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer).FPlayerRace;
                if (turn.FLastPlayer == -1) turn.FLastPlayer = 12;
                turn.FCurPlayer = turn.FLastPlayer;
                turn.FAnimationFrame = turn.FCurPlayer * 7;
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.CurP.Visible = Aow.Game.World.Map.AoWHSMap.Current.FPlayMode == PlayMode.Turn;
            }
            else
            {
                turn.FCurPlayer = 12;
                turn.FLastPlayer = turn.FCurPlayer;
                turn.FAnimationFrame = turn.FCurPlayer * 7;
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.CurP.Visible = false;
            }
        }
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.FWorldClockDelay = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ChatBtn.ILIndexUp = 14;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.FChatFlash = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.SetSceneSize(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.FDisplayWidth, Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.FDisplayHeight);
    }

}
