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
                if ((TAoWComponent)Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I] != Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow)
                {
                    ((TAoWComponent)Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I]).Visible = false;
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
        var rect = Rect(Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.Alignment.BottomOffset, Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(0, rect);
        var rect2 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(1, rect2);
        if (Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Alignment.AlignWidth != TAOWAlignWidth.awCenter)
        {
            var rect3 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }

        var rect4 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(6, rect4);
        var rect5 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(7, rect5);
        if (Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.BottomOffset, Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.LeftOffset, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }

        var rect6 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Alignment.TopOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(12, rect6);
        var rect7 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Alignment.RightOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Alignment.BottomOffset, Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(10, rect7);
        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Left, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Top, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
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

        Lists?.Dispose();
    }

    // AoWWinMan.pas:152
    public void RegisterDialog(TAOWWindow Window)
    {
        var map = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth == TAOWAlignWidth.awBoth && Window.Alignment.AlignWidth == TAOWAlignWidth.awBoth)
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
    public void UnregisterDialog(TAOWWindow Window)
    {
        var map = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth == TAOWAlignWidth.awBoth && Window.Alignment.AlignWidth == TAOWAlignWidth.awBoth && (Window == Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin || !Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Visible || Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.Alignment.AlignWidth != TAOWAlignWidth.awBoth) && (Window == Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg || !Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Visible || Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth) && (Window == Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg || !Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible || Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Alignment.AlignWidth != TAOWAlignWidth.awBoth))
        {
            map.Alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
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
            (FAbilities[I] as IDisposable)?.Dispose();
        }

        FAbilities.Clear();
        for (int I = 0; I <= 169; I++)
        {
            if (FSelectedUnit.GetAbilityEnabled(I))
            {
                A = new TTCAbility();
                A.Ability = I;
                if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit.FPlayer == Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer && (Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(I).GetControlType(Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit) & TSet<TAbilityControlTypeElement>.Of(TAbilityControlTypeElement.acAction)) != TSet<TAbilityControlTypeElement>.Of())
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
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            scanner.Visible = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null && Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
        }

        var playerInfo = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin;
        var playerInfoAlignment = playerInfo.Alignment;
        playerInfoAlignment.BottomOffset = 0;
        playerInfoAlignment.RightOffset = 0;

        playerInfo.WinWidth = scanner.WinWidth;
        playerInfo.WinHeight = scanner.WinHeight;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            playerInfo.Visible = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null && Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
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
        realmAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        realmAlignment.AlignHeight = TAOWAlignHeight.ahBottom;

        realm.WinWidth = realm.MinWidth;
        realm.WinHeight = realm.MinHeight;
        realm.Visible = false;

        var magic = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg;
        var magicAlignment = magic.Alignment;
        magicAlignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
        magicAlignment.LeftOffset = 0;
        magicAlignment.RightOffset = scanner.WinWidth;
        magicAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        magicAlignment.AlignHeight = TAOWAlignHeight.ahBottom;

        magic.WinWidth = magic.MinWidth;
        magic.WinHeight = magic.MinHeight;
        magic.Visible = false;

        var unitWindow = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin;
        var unitWindowAlignment = unitWindow.Alignment;
        unitWindowAlignment.AlignWidth = TAOWAlignWidth.awCenter;
        unitWindowAlignment.AlignHeight = TAOWAlignHeight.ahCenter;

        unitWindow.WinWidth = unitWindow.MinWidth;
        unitWindow.WinHeight = unitWindow.MinHeight;
        unitWindow.Visible = false;

        var chat = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin;
        var chatAlignment = chat.Alignment;
        chatAlignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight;
        chatAlignment.LeftOffset = 0;
        chatAlignment.RightOffset = scanner.WinWidth;
        chatAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        chatAlignment.AlignHeight = TAOWAlignHeight.ahBottom;

        chat.WinWidth = chat.MinWidth;
        chat.WinHeight = chat.MinHeight;
        chat.Visible = false;

        var tcScanner = Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin;
        var tcScannerAlignment = tcScanner.Alignment;
        tcScannerAlignment.TopOffset = 0;
        tcScannerAlignment.RightOffset = 0;

        tcScanner.WinHeight = (Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight - Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight) * 170 / 432;
        tcScanner.WinWidth = tcScanner.WinHeight * 150 / 170;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            tcScanner.Visible = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap != null && Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible;
        }

        var tcUnit = Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin;
        var tcUnitAlignment = tcUnit.Alignment;
        tcUnitAlignment.BottomOffset = 0;
        tcUnitAlignment.RightOffset = 0;

        tcUnit.WinHeight = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.WinHeight - Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight - tcScanner.WinHeight;
        tcUnit.WinWidth = tcScanner.WinWidth;
        tcUnit.Visible = false;

        var mapAlignment = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment;
        mapAlignment.AlignHeight = TAOWAlignHeight.ahBoth;
        mapAlignment.AlignWidth = TAOWAlignWidth.awBoth;
        mapAlignment.TopOffset = 0;
        mapAlignment.LeftOffset = 0;

        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignHeight = TAOWAlignHeight.ahBoth;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignWidth = TAOWAlignWidth.awBoth;
        var rect = Rect(0, 0, scanner.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight);
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect = rect;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignHeight = TAOWAlignHeight.ahBoth;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth = TAOWAlignWidth.awBoth;
        var rect2 = Rect(0, 0, tcScanner.WinWidth, Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.WinHeight);
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect = rect2;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FCombatMap == null)
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
    public void ManagerKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        TPlayer Player;
        string S;
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDebug)
        {
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null && (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayMode != TPlayMode.pmSimultaneous || Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.FLocalPlayers.Count == 1) && TSet<byte>.WithBounds(0, 255).WithRange(49, 57).Contains((byte)Key) && Aow.Game.Combat.TAoWCombatMap.AoWCombatMap == null && Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetCount() > Key - 48 && Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.GetDebugMode())
            {
                Player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Key - 48);
                if (Player.FPlayerType == TPlayerType.ptCPU5)
                {
                    Player.FPlayerType = TPlayerType.ptHuman;
                }
                else
                {
                    Player.FPlayerType = (TPlayerType)((int)Player.FPlayerType + 1);
                }

                var playerType = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TPlayerTypeRStr[(int)Player.FPlayerType]);
                S = "Player " + IntToStr(Key - 48) + " set to: " + playerType;
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.ShowMessage(S);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.Update();
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FRequestCombatType = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.FLocalHumanPlayers.Count > 1;
            }

            switch (Key)
            {
                case 122:
                    S = ExtractFileDir(Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    Aow.Game.Ui.Dialogs.TGeneral.LoadGame(this, FileBoundary.ReadName("Crap.hsm"));
                    Aow.Game.Ui.Dialogs.TGeneral.StartGame(this);
                    break;
                case 123:
                    S = ExtractFileDir(Application.ExeName);
                    S = S + "\\Scenario\\Custom";
                    SetCurrentDir(S);
                    Aow.Game.Ui.Dialogs.TGeneral.LoadGame(this, FileBoundary.ReadName("demomap.hsm"));
                    Aow.Game.Ui.Dialogs.TGeneral.StartGame(this);
                    break;
            }
        }
    }

    // AoWWinMan.pas:453 reads the inherited modal field on menu siblings too.
    public void HideTransientWindows()
    {
        for (int i = 0; i < Manager.FChildren.Count; i++)
            if (Manager.FChildren[i] is TAOWWindowControl window &&
                window != Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow && window.FModal && window.Visible)
                window.Visible = false;
    }

    internal static void HideGameWindows(TGeneral Controller)
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
                    ((Aow.Game.Ui.Controls.TAoWComponent)Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FChildren[I]).Visible = false;
                }
            }
        }
    }

    internal static void RestoreGameWindows(TGeneral Controller)
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

    internal static void ExitToTitle(TGeneral Controller)
    {
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseCampaign();
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetMap(null);
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetMap(null);
        Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(Aow.Game.Magic.Abilities.GameTables.AoWEngine.StartupDirectory);
        Aow.Game.Ui.Dialogs.TGeneral.HideGameWindows(Aow.Game.Ui.Dialogs.DialogHosts.General);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TaskWindow.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleWindow.TitleWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.ResetScene();
        Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.ShowScene(true);
    }

    internal static void SaveWindowLayout(TGeneral Controller)
    {
        var playerInfo = Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.PInfoWin;
        var rect = Rect(playerInfo.Alignment.RightOffset, playerInfo.Alignment.BottomOffset, playerInfo.WinWidth, playerInfo.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(0, rect);

        var controls = Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow;
        var rect2 = Rect(controls.Alignment.RightOffset, controls.Alignment.TopOffset, controls.WinWidth, controls.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(1, rect2);

        var unitWindow = Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin;
        if (unitWindow.Alignment.AlignWidth != TAOWAlignWidth.awCenter)
        {
            var rect3 = Rect(unitWindow.Alignment.LeftOffset, unitWindow.Alignment.TopOffset, unitWindow.WinWidth, unitWindow.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(2, rect3);
        }

        var realm = Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg;
        if (realm.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(realm.Alignment.LeftOffset, realm.Alignment.TopOffset, realm.WinWidth, realm.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(5, rect3);
        }

        var magic = Aow.Game.Ui.Dialogs.DialogHosts.MagicWin.Dlg;
        if (magic.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(magic.Alignment.LeftOffset, magic.Alignment.TopOffset, magic.WinWidth, magic.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(13, rect3);
        }

        var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow;
        var rect4 = Rect(scanner.Alignment.RightOffset, scanner.Alignment.TopOffset, scanner.WinWidth, scanner.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(6, rect4);

        var info = Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin;
        var rect5 = Rect(info.Alignment.RightOffset, info.Alignment.TopOffset, info.WinWidth, info.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(7, rect5);

        var chat = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin;
        if (chat.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(chat.Alignment.LeftOffset, chat.Alignment.BottomOffset, chat.WinWidth, chat.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(8, rect3);
        }

        var map = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
        if (map.Alignment.AlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(map.Alignment.LeftOffset, map.Alignment.TopOffset, map.WinWidth, map.WinHeight);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(9, rect3);
        }

        var tcUnit = Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin;
        var rect6 = Rect(tcUnit.Alignment.RightOffset, tcUnit.Alignment.TopOffset, tcUnit.WinWidth, tcUnit.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(12, rect6);

        var tcScanner = Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin;
        var rect7 = Rect(tcScanner.Alignment.RightOffset, tcScanner.Alignment.BottomOffset, tcScanner.WinWidth, tcScanner.WinHeight);
        Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(10, rect7);

        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth != TAOWAlignWidth.awBoth)
        {
            var rect3 = Rect(Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Left, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Top, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right, Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
        else
        {
            var rect3 = Rect(0, 0, 0, 0);
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.FRegistry.SetRectangle(11, rect3);
        }
    }

    internal static bool HasModalWindow(TGeneral controller)
    {
        for (int i = controller.Manager.FChildren.Count - 1; i >= 0; i--)
            if (controller.Manager.FChildren[i] is TAOWWindowControl window &&
                window != Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow && window.FModal && window.Visible)
                return true;
        return false;
    }

    internal static void LoadGame(TGeneral Controller, string Name)
    {
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null)
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
        if (Aow.Game.Magic.Abilities.GameTables.AoWEngine.FDemo && Aow.Game.World.Map.TAoWHSMap.AoWHSMap != null && !Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FShowAllPlayers)
        {
            Controller.ReturnToTitle();
        }

        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.ProgressDialog.ProgDlg.Visible = false;
    }

    internal static void StartGame(TGeneral controller)
    {
        controller.FWindowsHidden = false;
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGameSongs.Songs.Count != 0) Aow.Game.Audio.AudioHost.SongPlayer.Songs.AddStrings(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGameSongs.Songs);
        else if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.Songs.Count != 0)
        {
            for (int i = 0; i < Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.Songs.Count; i++)
                if (RtlUnit.CompareText(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.SongNames[i], Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1FileNotFoundRStr)) != 0)
                    Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FStartupSongs.Songs[i]);
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
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.HSMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
        var scanner = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner;
        scanner.SetMap(Aow.Game.World.Map.TAoWHSMap.AoWHSMap);
        scanner.SetLevel(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(0));
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FButtonChecked = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ChatBtn.Visible = Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.DPlay.Connected;
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FAligned = false;
        scanner.FPreviousWidth = 0;
        scanner.FIncremental = false;
        scanner.FSized = false;
        var tabs = Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerTab;
        tabs.Items.Clear();
        tabs.Items.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1SurfaceRStr));
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.LevelCount > 1) tabs.Items.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1CavernsRStr));
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.LevelCount > 2) tabs.Items.Add(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.LayerText.E1DepthsRStr));
        tabs.Selected = 0;
        tabs.FSized = false;
        scanner.SetTWidth(0);
        scanner.SetTHeight(0);
        scanner.SetTLeft(-1);
        scanner.SetTTop(-1);
        scanner.FUpdateStep = -1;
        scanner.SetMap(Aow.Game.World.Map.TAoWHSMap.AoWHSMap);
        scanner.SetLevel(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.MapContainer.GetMapLevel(0));
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.HSMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth - 24;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.SceneHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight - 24;
        Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Activate();
        Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapArmyDeselected();
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapStructureDeselected();
        // The original call at targets the indicator sizing routine
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.UpdatePlayerIndicator();
        Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdatePlayer();
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.Status.Contains(THSMapStatusElement.msActivated))
        {
            var turn = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton;
            if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer != -1)
            {
                turn.FLastPlayer = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FPlayerRace;
                if (turn.FLastPlayer == -1) turn.FLastPlayer = 12;
                turn.FCurPlayer = turn.FLastPlayer;
                turn.FAnimationFrame = turn.FCurPlayer * 7;
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.CurP.Visible = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayMode == TPlayMode.pmTurn;
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
