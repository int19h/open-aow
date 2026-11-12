// Translated from AoWWinMan.pas.
using Aow.Engine.Buffers;
using Aow.Formats;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// preview settings. abilities. active ability count. selected indices. unit list. selected
// unit. combat sprite
public partial class General : DataModule
{
    public General(Component owner) : base(owner)
    { }

    public WindowManager Manager;
    public ToolTip ToolTip;

    public bool WindowsHidden { get; set; }
    public bool InfoVisible { get; set; }
    public bool RealmVisible { get; set; }
    public bool MagicVisible { get; set; }
    public bool UnitVisible { get; set; }
    public bool ScannerVisible { get; set; }
    public bool ControlVisible { get; set; }
    public bool ChatVisible { get; set; }
    public bool MapVisible { get; set; }
    public bool TargetVisible { get; set; }

    public UnitShowInfo UnitShowInfo;

    // are part of UnitShowInfo
    public List<CombatAbilityRef> Abilities { get; private set; }
    public int ActiveAbilityCount { get; private set; }

    public int[] SelectedUnits { get; } = new int[8];

    public Army UnitList { get; set; }
    public Structure Structure { get; set; }
    public Structure PreviousStructure { get; set; }
    public City City { get; set; }
    public ProductionPlace ProductionPlace { get; set; }
    public ProductionControl ProductionControl { get; set; }
    public Sign Sign { get; set; }
    public Troop SelectedUnit { get; set; }
    public TacticalBattleUnitHexSprite CombatUnit { get; set; }

    // AoWWinMan.pas:503
    public void ReturnToTitle()
    {
        GameTables.GameEngine.CloseHsm(null);
        GameTables.GameEngine.CloseCampaign();
        DialogHosts.ScannerWindow.Scanner.SetMap(null);
        DialogHosts.CombatScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(GameTables.GameEngine.StartupDirectory);
        DialogHosts.General.HideWindows();
        DialogHosts.TaskBar.TaskWindow.Visible = false;
        DialogHosts.TitleWindow.MainWindow.Visible = false;
        DialogHosts.TitleScene.ResetScene();
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
    }

    // AoWWinMan.pas:415
    public void HideWindows()
    {
        if (!WindowsHidden)
        {
            WindowsHidden = true;
            InfoVisible = DialogHosts.PlayerInfoWindow.MainWindow.Visible;
            RealmVisible = DialogHosts.ControlWindow.MainWindow.Visible;
            MagicVisible = DialogHosts.UnitWindow.MainWindow.Visible;
            UnitVisible = DialogHosts.RealmWindow.Window.Visible;
            TargetVisible = DialogHosts.MagicWindow.Window.Visible;
            ScannerVisible = DialogHosts.ScannerWindow.MainWindow.Visible;
            ControlVisible = DialogHosts.InfoWindow.MainWindow.Visible;
            ChatVisible = DialogHosts.ChatWindow.MainWindow.Visible;
            MapVisible = DialogHosts.MapWindow.MainWindow.Visible;
            for (int I = 0, last = DialogHosts.General.Manager.Children.Count - 1; I <= last; I++)
            {
                if ((UIComponent)DialogHosts.General.Manager.Children[I] != DialogHosts.TaskBar.TaskWindow)
                {
                    ((UIComponent)DialogHosts.General.Manager.Children[I]).Visible = false;
                }
            }
        }
    }

    // AoWWinMan.pas:436
    public void ShowWindows()
    {
        if (WindowsHidden)
        {
            WindowsHidden = false;
            DialogHosts.PlayerInfoWindow.MainWindow.Visible = InfoVisible;
            DialogHosts.ControlWindow.MainWindow.Visible = RealmVisible;
            DialogHosts.UnitWindow.MainWindow.Visible = MagicVisible;
            DialogHosts.ScannerWindow.MainWindow.Visible = ScannerVisible;
            DialogHosts.InfoWindow.MainWindow.Visible = ControlVisible;
            DialogHosts.MapWindow.MainWindow.Visible = MapVisible;
            DialogHosts.RealmWindow.Window.Visible = UnitVisible;
            DialogHosts.MagicWindow.Window.Visible = TargetVisible;
            DialogHosts.ChatWindow.MainWindow.Visible = ChatVisible;
        }
    }

    // AoWWinMan.pas:464
    public void SaveLayout()
    {
        // { Read the saved rectangle through the window pointer cell. }
        var rectangle = new Rect(DialogHosts.PlayerInfoWindow.MainWindow.Alignment.RightOffset, DialogHosts.PlayerInfoWindow.MainWindow.Alignment.BottomOffset, DialogHosts.PlayerInfoWindow.MainWindow.WinWidth, DialogHosts.PlayerInfoWindow.MainWindow.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(0, rectangle);
        var rectangle2 = new Rect(DialogHosts.ControlWindow.MainWindow.Alignment.RightOffset, DialogHosts.ControlWindow.MainWindow.Alignment.TopOffset, DialogHosts.ControlWindow.MainWindow.WinWidth, DialogHosts.ControlWindow.MainWindow.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(1, rectangle2);
        if (DialogHosts.UnitWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Center)
        {
            var rectangle3 = new Rect(DialogHosts.UnitWindow.MainWindow.Alignment.LeftOffset, DialogHosts.UnitWindow.MainWindow.Alignment.TopOffset, DialogHosts.UnitWindow.MainWindow.WinWidth, DialogHosts.UnitWindow.MainWindow.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(2, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(2, rectangle3);
        }

        if (DialogHosts.RealmWindow.Window.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(DialogHosts.RealmWindow.Window.Alignment.LeftOffset, DialogHosts.RealmWindow.Window.Alignment.TopOffset, DialogHosts.RealmWindow.Window.WinWidth, DialogHosts.RealmWindow.Window.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(5, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(5, rectangle3);
        }

        if (DialogHosts.MagicWindow.Window.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(DialogHosts.MagicWindow.Window.Alignment.LeftOffset, DialogHosts.MagicWindow.Window.Alignment.TopOffset, DialogHosts.MagicWindow.Window.WinWidth, DialogHosts.MagicWindow.Window.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(13, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(13, rectangle3);
        }

        var rectangle4 = new Rect(DialogHosts.ScannerWindow.MainWindow.Alignment.RightOffset, DialogHosts.ScannerWindow.MainWindow.Alignment.TopOffset, DialogHosts.ScannerWindow.MainWindow.WinWidth, DialogHosts.ScannerWindow.MainWindow.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(6, rectangle4);
        var rectangle5 = new Rect(DialogHosts.InfoWindow.MainWindow.Alignment.RightOffset, DialogHosts.InfoWindow.MainWindow.Alignment.TopOffset, DialogHosts.InfoWindow.MainWindow.WinWidth, DialogHosts.InfoWindow.MainWindow.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(7, rectangle5);
        if (DialogHosts.ChatWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(DialogHosts.ChatWindow.MainWindow.Alignment.LeftOffset, DialogHosts.ChatWindow.MainWindow.Alignment.BottomOffset, DialogHosts.ChatWindow.MainWindow.WinWidth, DialogHosts.ChatWindow.MainWindow.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(8, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(8, rectangle3);
        }

        if (DialogHosts.MapWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(DialogHosts.MapWindow.MainWindow.Alignment.LeftOffset, DialogHosts.MapWindow.MainWindow.Alignment.TopOffset, DialogHosts.MapWindow.MainWindow.WinWidth, DialogHosts.MapWindow.MainWindow.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(9, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(9, rectangle3);
        }

        var rectangle6 = new Rect(DialogHosts.CombatScanner.CombatScannerWindow.Alignment.RightOffset, DialogHosts.CombatScanner.CombatScannerWindow.Alignment.TopOffset, DialogHosts.CombatScanner.CombatScannerWindow.WinWidth, DialogHosts.CombatScanner.CombatScannerWindow.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(12, rectangle6);
        var rectangle7 = new Rect(DialogHosts.CombatUnitInfo.MainWindow.Alignment.RightOffset, DialogHosts.CombatUnitInfo.MainWindow.Alignment.BottomOffset, DialogHosts.CombatUnitInfo.MainWindow.WinWidth, DialogHosts.CombatUnitInfo.MainWindow.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(10, rectangle7);
        if (DialogHosts.MapWindow.MapAlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(DialogHosts.MapWindow.MapRectangle.Left, DialogHosts.MapWindow.MapRectangle.Top, DialogHosts.MapWindow.MapRectangle.Right, DialogHosts.MapWindow.MapRectangle.Bottom);
            GameTables.GameEngine.Registry.SetRectangle(11, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(11, rectangle3);
        }
    }

    // AoWWinMan.pas:79
    public void PlayButtonSound(byte kind)
    {
        switch (kind)
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
    public void SortLists(ref DelphiList lists, byte order)
    {
        ListBox L;
        int J;
        int insertAt;
        int valueI;
        int valueJ;
        L = (ListBox)lists[0];
        for (int I = 1, last = L.Items.Count - 1; I <= last; I++)
        {
            J = I - 1;
            insertAt = -1;
            while (J > -1)
            {
                switch (order)
                {
                    case 0:
                        var item = L.Items[J];
                        if (CompareStr(UpperCase(L.Items[I]), UpperCase(item)) < 0)
                        {
                            insertAt = J;
                        }

                        break;
                    case 1:
                        var item2 = L.Items[J];
                        if (CompareStr(UpperCase(L.Items[I]), UpperCase(item2)) > 0)
                        {
                            insertAt = J;
                        }

                        break;
                    case 2:
                        try
                        {
                            valueI = StrToInt(L.Items[I]);
                            valueJ = StrToInt(L.Items[J]);
                            if (valueI > valueJ)
                            {
                                insertAt = J;
                            }
                        }
                        catch
                        {
                        }

                        break;
                    case 3:
                        try
                        {
                            valueI = StrToInt(L.Items[I]);
                            valueJ = StrToInt(L.Items[J]);
                            if (valueI < valueJ)
                            {
                                insertAt = J;
                            }
                        }
                        catch
                        {
                        }

                        break;
                }

                J -= 1;
            }

            if (insertAt > -1)
            {
                J = 0;
                int limit = lists.Count - 1;
                for (; J <= limit; J += 1)
                {
                    ((ListBox)lists[J]).Items.Move(I, insertAt);
                }
            }
        }

        for (int I = 0, last = lists.Count - 1; I <= last; I++)
        {
            L = (ListBox)lists[I];
            L.ListOffset = 0;
            L.Selected = -1;
            L.Sized = false;
        }

        lists?.Dispose();
    }

    // AoWWinMan.pas:152
    public void RegisterDialog(Window window)
    {
        var map = DialogHosts.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth == AlignWidth.Both && window.Alignment.AlignWidth == AlignWidth.Both)
        {
            map.Alignment.BottomOffset = DialogHosts.TaskBar.TaskWindow.WinHeight + window.WinHeight;
            map.Aligned = false;
            if (map.Visible)
            {
                window.Alignment.RightOffset = map.Alignment.RightOffset;
            }
            else
            {
                window.Alignment.RightOffset = 0;
            }

            window.Aligned = false;
        }
    }

    // AoWWinMan.pas:168
    public void UnregisterDialog(Window window)
    {
        var map = DialogHosts.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth == AlignWidth.Both && window.Alignment.AlignWidth == AlignWidth.Both && (window == DialogHosts.ChatWindow.MainWindow || !DialogHosts.ChatWindow.MainWindow.Visible || DialogHosts.ChatWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both) && (window == DialogHosts.MagicWindow.Window || !DialogHosts.MagicWindow.Window.Visible || DialogHosts.MagicWindow.Window.Alignment.AlignWidth != AlignWidth.Both) && (window == DialogHosts.RealmWindow.Window || !DialogHosts.RealmWindow.Window.Visible || DialogHosts.RealmWindow.Window.Alignment.AlignWidth != AlignWidth.Both))
        {
            map.Alignment.BottomOffset = DialogHosts.TaskBar.TaskWindow.WinHeight;
            map.Aligned = false;
            window.Alignment.RightOffset = map.Alignment.RightOffset;

            window.Aligned = false;
        }
    }

    // AoWWinMan.pas:89
    public int IndexOfUnit(Troop U)
    {
        int result = -1;
        if (SelectedUnits[0] > -1 && UnitList != null)
        {
            for (int I = 0; I <= 7; I++)
            {
                if (SelectedUnits[I] > -1 && I < UnitList.Count)
                {
                    if (U == UnitList.GetUnit(SelectedUnits[I]))
                    {
                        result = I;
                    }
                }
            }
        }

        return result;
    }

    // AoWWinMan.pas:100
    public void BuildAbilities()
    {
        CombatAbilityRef A;
        ActiveAbilityCount = 0;
        for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
        {
            (Abilities[I] as IDisposable)?.Dispose();
        }

        Abilities.Clear();
        for (int I = 0; I <= 169; I++)
        {
            if (SelectedUnit.GetAbilityEnabled(I))
            {
                A = new CombatAbilityRef();
                A.Ability = I;
                if (DialogHosts.General.SelectedUnit.Player == AoWHSMap.Current.SeatedPlayer && (AoWHSSet.Current.AbilityControl.GetAbility(I).GetControlType(DialogHosts.General.SelectedUnit) & Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action)) != Set<AbilityControlTypeElement>.Of())
                {
                    Abilities.Insert(ActiveAbilityCount, A);
                    ActiveAbilityCount += 1;
                }
                else
                {
                    Abilities.Add(A);
                }
            }
        }
    }

    // AoWWinMan.pas:197
    public int MoveTypesToInt32(Set<MoveType> moveTypes)
    {
        int result = 0;
        if ((moveTypes & Set<MoveType>.Of(MoveType.Forestry)) == Set<MoveType>.Of(MoveType.Forestry))
        {
            result = 2;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Mountaineering))
        {
            result = 4;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Swimming)) == Set<MoveType>.Of(MoveType.Swimming))
        {
            result = 6;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Walking, MoveType.Swimming)) == Set<MoveType>.Of(MoveType.Walking, MoveType.Swimming))
        {
            result = 8;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry)) == Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry))
        {
            result = 10;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Swimming, MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Swimming, MoveType.Mountaineering))
        {
            result = 12;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Forestry, MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Forestry, MoveType.Mountaineering))
        {
            result = 14;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry, MoveType.Mountaineering)) == Set<MoveType>.Of(MoveType.Swimming, MoveType.Forestry, MoveType.Mountaineering))
        {
            result = 16;
        }

        if ((moveTypes & Set<MoveType>.Of(MoveType.Flying)) == Set<MoveType>.Of(MoveType.Flying))
        {
            result = 18;
        }

        return result;
    }

    // AoWWinMan.pas:185
    public int SphereToImage(MagicSphere sphere)
    {
        int result;
        switch (sphere)
        {
            case MagicSphere.Life:
                result = 27;
                break;
            case MagicSphere.Death:
                result = 30;
                break;
            case MagicSphere.Earth:
                result = 33;
                break;
            case MagicSphere.Air:
                result = 36;
                break;
            case MagicSphere.Water:
                result = 39;
                break;
            default:
                result = 42;
                break;
        }

        return result;
    }

    // AoWWinMan.pas:216
    public void GeneralCreate()
    {
        UnitShowInfo.Direction = 3;
        UnitShowInfo.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
        Abilities = new List<CombatAbilityRef>();
    }

    // AoWWinMan.pas:223
    public void GeneralDestroy()
    {
        for (int I = 0, last = Abilities.Count - 1; I <= last; I++)
        {
            (Abilities[I] as IDisposable)?.Dispose();
        }

        Abilities?.Clear();
    }

    // AoWWinMan.pas:231
    public void ResetLayout()
    {
        var scanner = DialogHosts.ScannerWindow.MainWindow;
        var scannerAlignment = scanner.Alignment;
        scannerAlignment.TopOffset = 0;
        scannerAlignment.RightOffset = 0;

        if (DialogHosts.General.Manager.WinHeight > 700)
        {
            scanner.WinHeight = (DialogHosts.General.Manager.WinHeight - DialogHosts.TaskBar.TaskWindow.WinHeight) / 3;
        }
        else
        {
            scanner.WinHeight = (DialogHosts.General.Manager.WinHeight - DialogHosts.TaskBar.TaskWindow.WinHeight) / 2;
        }

        scanner.WinWidth = scanner.WinHeight * 200 / 216;
        if (AoWHSMap.Current != null)
        {
            scanner.Visible = AoWHSMap.Current.CombatMap == null && DialogHosts.MapWindow.MainWindow.Visible;
        }

        var playerInfo = DialogHosts.PlayerInfoWindow.MainWindow;
        var playerInfoAlignment = playerInfo.Alignment;
        playerInfoAlignment.BottomOffset = 0;
        playerInfoAlignment.RightOffset = 0;

        playerInfo.WinWidth = scanner.WinWidth;
        playerInfo.WinHeight = scanner.WinHeight;
        if (AoWHSMap.Current != null)
        {
            playerInfo.Visible = AoWHSMap.Current.CombatMap == null && DialogHosts.MapWindow.MainWindow.Visible;
        }

        var controls = DialogHosts.ControlWindow.MainWindow;
        var controlsAlignment = controls.Alignment;
        controlsAlignment.TopOffset = scanner.WinHeight;
        controlsAlignment.RightOffset = 0;

        controls.WinWidth = scanner.WinWidth;
        controls.WinHeight = scanner.WinHeight;
        controls.Visible = false;

        var info = DialogHosts.InfoWindow.MainWindow;
        var infoAlignment = info.Alignment;
        infoAlignment.TopOffset = scanner.WinHeight;
        infoAlignment.RightOffset = 0;

        info.WinWidth = scanner.WinWidth;
        info.WinHeight = scanner.WinHeight;
        info.Visible = false;

        var targetSelection = DialogHosts.TargetSelectionDialog.Window;
        var targetSelectionAlignment = targetSelection.Alignment;
        targetSelectionAlignment.TopOffset = scanner.WinHeight;
        targetSelectionAlignment.RightOffset = 0;

        targetSelection.WinWidth = scanner.WinWidth;
        targetSelection.WinHeight = scanner.WinHeight;

        var realm = DialogHosts.RealmWindow.Window;
        var realmAlignment = realm.Alignment;
        realmAlignment.BottomOffset = DialogHosts.TaskBar.TaskWindow.WinHeight;
        realmAlignment.LeftOffset = 0;
        realmAlignment.RightOffset = scanner.WinWidth;
        realmAlignment.AlignWidth = AlignWidth.Both;
        realmAlignment.AlignHeight = AlignHeight.Bottom;

        realm.WinWidth = realm.MinWidth;
        realm.WinHeight = realm.MinHeight;
        realm.Visible = false;

        var magic = DialogHosts.MagicWindow.Window;
        var magicAlignment = magic.Alignment;
        magicAlignment.BottomOffset = DialogHosts.TaskBar.TaskWindow.WinHeight;
        magicAlignment.LeftOffset = 0;
        magicAlignment.RightOffset = scanner.WinWidth;
        magicAlignment.AlignWidth = AlignWidth.Both;
        magicAlignment.AlignHeight = AlignHeight.Bottom;

        magic.WinWidth = magic.MinWidth;
        magic.WinHeight = magic.MinHeight;
        magic.Visible = false;

        var unitWindow = DialogHosts.UnitWindow.MainWindow;
        var unitWindowAlignment = unitWindow.Alignment;
        unitWindowAlignment.AlignWidth = AlignWidth.Center;
        unitWindowAlignment.AlignHeight = AlignHeight.Center;

        unitWindow.WinWidth = unitWindow.MinWidth;
        unitWindow.WinHeight = unitWindow.MinHeight;
        unitWindow.Visible = false;

        var chat = DialogHosts.ChatWindow.MainWindow;
        var chatAlignment = chat.Alignment;
        chatAlignment.BottomOffset = DialogHosts.TaskBar.TaskWindow.WinHeight;
        chatAlignment.LeftOffset = 0;
        chatAlignment.RightOffset = scanner.WinWidth;
        chatAlignment.AlignWidth = AlignWidth.Both;
        chatAlignment.AlignHeight = AlignHeight.Bottom;

        chat.WinWidth = chat.MinWidth;
        chat.WinHeight = chat.MinHeight;
        chat.Visible = false;

        var tacticalBattleScanner = DialogHosts.CombatScanner.CombatScannerWindow;
        var tacticalBattleScannerAlignment = tacticalBattleScanner.Alignment;
        tacticalBattleScannerAlignment.TopOffset = 0;
        tacticalBattleScannerAlignment.RightOffset = 0;

        tacticalBattleScanner.WinHeight = (DialogHosts.General.Manager.WinHeight - DialogHosts.TaskBar.TaskWindow.WinHeight) * 170 / 432;
        tacticalBattleScanner.WinWidth = tacticalBattleScanner.WinHeight * 150 / 170;
        if (AoWHSMap.Current != null)
        {
            tacticalBattleScanner.Visible = AoWHSMap.Current.CombatMap != null && DialogHosts.MapWindow.MainWindow.Visible;
        }

        var tacticalBattleUnit = DialogHosts.CombatUnitInfo.MainWindow;
        var tacticalBattleUnitAlignment = tacticalBattleUnit.Alignment;
        tacticalBattleUnitAlignment.BottomOffset = 0;
        tacticalBattleUnitAlignment.RightOffset = 0;

        tacticalBattleUnit.WinHeight = DialogHosts.General.Manager.WinHeight - DialogHosts.TaskBar.TaskWindow.WinHeight - tacticalBattleScanner.WinHeight;
        tacticalBattleUnit.WinWidth = tacticalBattleScanner.WinWidth;
        tacticalBattleUnit.Visible = false;

        var mapAlignment = DialogHosts.MapWindow.MainWindow.Alignment;
        mapAlignment.AlignHeight = AlignHeight.Both;
        mapAlignment.AlignWidth = AlignWidth.Both;
        mapAlignment.TopOffset = 0;
        mapAlignment.LeftOffset = 0;

        DialogHosts.MapWindow.WorldAlignHeight = AlignHeight.Both;
        DialogHosts.MapWindow.WorldAlignWidth = AlignWidth.Both;
        var rectangle = new Rect(0, 0, scanner.WinWidth, DialogHosts.TaskBar.TaskWindow.WinHeight);
        DialogHosts.MapWindow.WorldRectangle = rectangle;
        DialogHosts.MapWindow.MapAlignHeight = AlignHeight.Both;
        DialogHosts.MapWindow.MapAlignWidth = AlignWidth.Both;
        var rectangle2 = new Rect(0, 0, tacticalBattleScanner.WinWidth, DialogHosts.TaskBar.TaskWindow.WinHeight);
        DialogHosts.MapWindow.MapRectangle = rectangle2;
        if (AoWHSMap.Current != null)
        {
            if (AoWHSMap.Current.CombatMap == null)
            {
                DialogHosts.MapWindow.MainWindow.Alignment.RightOffset = scanner.WinWidth;
            }
            else
            {
                DialogHosts.MapWindow.MainWindow.Alignment.RightOffset = tacticalBattleScanner.WinWidth;
            }
        }

        DialogHosts.MapWindow.MainWindow.Alignment.BottomOffset = DialogHosts.TaskBar.TaskWindow.WinHeight;
        if (DialogHosts.ChatWindow.MainWindow.Visible)
        {
            RegisterDialog(DialogHosts.ChatWindow.MainWindow);
        }

        Manager.Aligned = false;
    }

    // AoWWinMan.pas:336
    public void ManagerDebugMessage(string text)
    {
        DialogHosts.ConfirmationDialog.AddDebugMessage(text);
    }

    // AoWWinMan.pas:351
    public void ManagerKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        Player player;
        string S;
        if (GameTables.GameEngine.Debug)
        {
            if (AoWHSMap.Current != null && (AoWHSMap.Current.PlayMode != PlayMode.Simultaneous || AoWHSMap.Current.Players.LocalPlayers.Count == 1) && Set<byte>.WithBounds(0, 255).WithRange(49, 57).Contains((byte)key) && CombatMap.Current == null && AoWHSMap.Current.Players.GetCount() > key - 48 && GameTables.GameEngine.Registry.GetDebugMode())
            {
                player = AoWHSMap.Current.Players.GetPlayers(key - 48);
                if (player.PlayerType == PlayerType.Cpu5)
                {
                    player.PlayerType = PlayerType.Human;
                }
                else
                {
                    player.PlayerType = (PlayerType)((int)player.PlayerType + 1);
                }

                var playerType = AbilityRules.TranslateResourceString(GameTables.PlayerTypeTexts[(int)player.PlayerType]);
                S = "Player " + IntToStr(key - 48) + " set to: " + playerType;
                AoWHSMap.Current.ShowMessage(S);
                AoWHSMap.Current.Players.Update();
                AoWHSMap.Current.RequestCombatType = AoWHSMap.Current.Players.LocalHumanPlayers.Count > 1;
            }

            switch (key)
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
        for (int i = 0; i < Manager.Children.Count; i++)
            if (Manager.Children[i] is WindowControl window &&
                window != DialogHosts.MapWindow.MainWindow && window._modal && window.Visible)
                window.Visible = false;
    }

    internal static void HideGameWindows(General controller)
    {
        if (!controller.WindowsHidden)
        {
            controller.WindowsHidden = true;
            controller.InfoVisible = DialogHosts.PlayerInfoWindow.MainWindow.Visible;
            controller.RealmVisible = DialogHosts.RealmWindow.Window.Visible;
            controller.MagicVisible = DialogHosts.MagicWindow.Window.Visible;
            controller.UnitVisible = DialogHosts.UnitWindow.MainWindow.Visible;
            controller.TargetVisible = DialogHosts.TargetSelectionDialog.Window.Visible;
            controller.ScannerVisible = DialogHosts.ScannerWindow.MainWindow.Visible;
            controller.ControlVisible = DialogHosts.ControlWindow.MainWindow.Visible;
            controller.ChatVisible = DialogHosts.ChatWindow.MainWindow.Visible;
            controller.MapVisible = DialogHosts.MapWindow.MainWindow.Visible;
            for (int I = 0, last = DialogHosts.General.Manager.Children.Count - 1; I <= last; I++)
            {
                if (DialogHosts.General.Manager.Children[I] != DialogHosts.TaskBar.TaskWindow)
                {
                    ((UIComponent)DialogHosts.General.Manager.Children[I]).Visible = false;
                }
            }
        }
    }

    internal static void RestoreGameWindows(General controller)
    {
        if (controller.WindowsHidden)
        {
            controller.WindowsHidden = false;
            DialogHosts.PlayerInfoWindow.MainWindow.Visible = controller.InfoVisible;
            DialogHosts.RealmWindow.Window.Visible = controller.RealmVisible;
            DialogHosts.MagicWindow.Window.Visible = controller.MagicVisible;
            DialogHosts.ScannerWindow.MainWindow.Visible = controller.ScannerVisible;
            DialogHosts.ControlWindow.MainWindow.Visible = controller.ControlVisible;
            DialogHosts.MapWindow.MainWindow.Visible = controller.MapVisible;
            DialogHosts.UnitWindow.MainWindow.Visible = controller.UnitVisible;
            DialogHosts.TargetSelectionDialog.Window.Visible = controller.TargetVisible;
            DialogHosts.ChatWindow.MainWindow.Visible = controller.ChatVisible;
        }
    }

    internal static void ExitToTitle(General controller)
    {
        GameTables.GameEngine.CloseHsm(null);
        GameTables.GameEngine.CloseCampaign();
        DialogHosts.ScannerWindow.Scanner.SetMap(null);
        DialogHosts.CombatScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(GameTables.GameEngine.StartupDirectory);
        HideGameWindows(DialogHosts.General);
        DialogHosts.TaskBar.TaskWindow.Visible = false;
        DialogHosts.TitleWindow.MainWindow.Visible = false;
        DialogHosts.TitleScene.ResetScene();
        DialogHosts.TitleScene.ShowScene(true);
    }

    internal static void SaveWindowLayout(General controller)
    {
        var playerInfo = DialogHosts.PlayerInfoWindow.MainWindow;
        var rectangle = new Rect(playerInfo.Alignment.RightOffset, playerInfo.Alignment.BottomOffset, playerInfo.WinWidth, playerInfo.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(0, rectangle);

        var controls = DialogHosts.ControlWindow.MainWindow;
        var rectangle2 = new Rect(controls.Alignment.RightOffset, controls.Alignment.TopOffset, controls.WinWidth, controls.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(1, rectangle2);

        var unitWindow = DialogHosts.UnitWindow.MainWindow;
        if (unitWindow.Alignment.AlignWidth != AlignWidth.Center)
        {
            var rectangle3 = new Rect(unitWindow.Alignment.LeftOffset, unitWindow.Alignment.TopOffset, unitWindow.WinWidth, unitWindow.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(2, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(2, rectangle3);
        }

        var realm = DialogHosts.RealmWindow.Window;
        if (realm.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(realm.Alignment.LeftOffset, realm.Alignment.TopOffset, realm.WinWidth, realm.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(5, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(5, rectangle3);
        }

        var magic = DialogHosts.MagicWindow.Window;
        if (magic.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(magic.Alignment.LeftOffset, magic.Alignment.TopOffset, magic.WinWidth, magic.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(13, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(13, rectangle3);
        }

        var scanner = DialogHosts.ScannerWindow.MainWindow;
        var rectangle4 = new Rect(scanner.Alignment.RightOffset, scanner.Alignment.TopOffset, scanner.WinWidth, scanner.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(6, rectangle4);

        var info = DialogHosts.InfoWindow.MainWindow;
        var rectangle5 = new Rect(info.Alignment.RightOffset, info.Alignment.TopOffset, info.WinWidth, info.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(7, rectangle5);

        var chat = DialogHosts.ChatWindow.MainWindow;
        if (chat.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(chat.Alignment.LeftOffset, chat.Alignment.BottomOffset, chat.WinWidth, chat.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(8, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(8, rectangle3);
        }

        var map = DialogHosts.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(map.Alignment.LeftOffset, map.Alignment.TopOffset, map.WinWidth, map.WinHeight);
            GameTables.GameEngine.Registry.SetRectangle(9, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(9, rectangle3);
        }

        var tacticalBattleUnit = DialogHosts.CombatUnitInfo.MainWindow;
        var rectangle6 = new Rect(tacticalBattleUnit.Alignment.RightOffset, tacticalBattleUnit.Alignment.TopOffset, tacticalBattleUnit.WinWidth, tacticalBattleUnit.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(12, rectangle6);

        var tacticalBattleScanner = DialogHosts.CombatScanner.CombatScannerWindow;
        var rectangle7 = new Rect(tacticalBattleScanner.Alignment.RightOffset, tacticalBattleScanner.Alignment.BottomOffset, tacticalBattleScanner.WinWidth, tacticalBattleScanner.WinHeight);
        GameTables.GameEngine.Registry.SetRectangle(10, rectangle7);

        if (DialogHosts.MapWindow.MapAlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(DialogHosts.MapWindow.MapRectangle.Left, DialogHosts.MapWindow.MapRectangle.Top, DialogHosts.MapWindow.MapRectangle.Right, DialogHosts.MapWindow.MapRectangle.Bottom);
            GameTables.GameEngine.Registry.SetRectangle(11, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            GameTables.GameEngine.Registry.SetRectangle(11, rectangle3);
        }
    }

    internal static bool HasModalWindow(General controller)
    {
        for (int i = controller.Manager.Children.Count - 1; i >= 0; i--)
            if (controller.Manager.Children[i] is WindowControl window &&
                window != DialogHosts.MapWindow.MainWindow && window._modal && window.Visible)
                return true;
        return false;
    }

    internal static void LoadGame(General controller, string name)
    {
        if (AoWHSMap.Current != null)
        {
            GameTables.GameEngine.CloseHsm(null);
        }

        var loading = AbilityRules.TranslateResourceString(LayerText.LoadingText);
        var fileName = ExtractFileName(name);
        DialogHosts.ProgressDialog.TitleLabel.Text = loading + " " + fileName + "...";
        DialogHosts.ProgressDialog.StatusLabel.Text = "";
        DialogHosts.ProgressDialog.ProgressBar.Position = 0;
        DialogHosts.ProgressDialog.ProgressBar.Max = 100;
        DialogHosts.ProgressDialog.Window.Visible = true;
        if (!DialogHosts.TitleScene.Window.Visible)
        {
            DialogHosts.General.Manager.OverlayPriority = 5000;
        }

        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        AudioPlayback.FadeMusicVolume(0, 1);
        GameTables.GameEngine.LoadHsm(name, false, (_, message, progression) => controller.MapProgress(message, progression), GameProfile.WorldMap);
        if (GameTables.GameEngine.Demo && AoWHSMap.Current != null && !AoWHSMap.Current.ShowAllPlayers)
        {
            controller.ReturnToTitle();
        }

        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.ProgressDialog.Window.Visible = false;
    }

    internal static void StartGame(General controller)
    {
        controller.WindowsHidden = false;
        AudioHost.SongPlayer.Songs.Clear();
        if (AoWHSMap.Current.GameSongs.Songs.Count != 0) AudioHost.SongPlayer.Songs.AddStrings(AoWHSMap.Current.GameSongs.Songs);
        else if (AoWHSMap.Current.StartupSongs.Songs.Count != 0)
        {
            for (int i = 0; i < AoWHSMap.Current.StartupSongs.Songs.Count; i++)
                if (RtlUnit.CompareText(AoWHSMap.Current.StartupSongs.SongNames[i], AbilityRules.TranslateResourceString(LayerText.FileNotFoundText)) != 0)
                    AudioHost.SongPlayer.Songs.Add(AoWHSMap.Current.StartupSongs.Songs[i]);
        }
        else AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
        AudioHost.SongPlayer.Play(0);
        AudioPlayback.SetMusicVolume(DialogHosts.SoundDialog.MusicVolumeSlider.Position);
        var menu = DialogHosts.PopupWindows.OverviewsMenu.Items;
        menu.Clear();
        menu.Add(AbilityRules.TranslateResourceString(LayerText.BackgroundText));
        if (GameTables.GameEngine.Campaign != null) menu.Add(AbilityRules.TranslateResourceString(LayerText.CampaignText));
        DialogHosts.TitleScene.Window.Visible = false;
        DialogHosts.TitleWindow.MainWindow.Visible = false;
        DialogHosts.TaskBar.TaskWindow.Visible = true;
        DialogHosts.MapWindow.MainWindow.Visible = true;
        DialogHosts.ScannerWindow.MainWindow.Visible = true;
        DialogHosts.MapWindow.MapViewer.HexSpriteMap = AoWHSMap.Current;
        var scanner = DialogHosts.ScannerWindow.Scanner;
        scanner.SetMap(AoWHSMap.Current);
        scanner.SetLevel(AoWHSMap.Current.MapContainer.GetMapLevel(0));
        DialogHosts.TaskBar.WorldTurnButton.ButtonChecked = false;
        DialogHosts.TaskBar.ChatButton.Visible = DialogHosts.SetupEvents.DirectPlay.Connected;
        DialogHosts.General.Manager.Aligned = false;
        scanner.PreviousWidth = 0;
        scanner.Incremental = false;
        scanner.Sized = false;
        var tabs = DialogHosts.ScannerWindow.ScannerTabs;
        tabs.Items.Clear();
        tabs.Items.Add(AbilityRules.TranslateResourceString(LayerText.SurfaceText));
        if (AoWHSMap.Current.MapContainer.LevelCount > 1) tabs.Items.Add(AbilityRules.TranslateResourceString(LayerText.CavernsText));
        if (AoWHSMap.Current.MapContainer.LevelCount > 2) tabs.Items.Add(AbilityRules.TranslateResourceString(LayerText.DepthsText));
        tabs.Selected = 0;
        tabs.Sized = false;
        scanner.SetTWidth(0);
        scanner.SetTHeight(0);
        scanner.SetTLeft(-1);
        scanner.SetTTop(-1);
        scanner.UpdateStep = -1;
        scanner.SetMap(AoWHSMap.Current);
        scanner.SetLevel(AoWHSMap.Current.MapContainer.GetMapLevel(0));
        DialogHosts.MapWindow.MapViewer.HexSpriteMap = AoWHSMap.Current;
        DialogHosts.MapWindow.MapViewer.SceneWidth = DialogHosts.MapWindow.MainWindow.WinWidth - 24;
        DialogHosts.MapWindow.MapViewer.SceneHeight = DialogHosts.MapWindow.MainWindow.WinHeight - 24;
        AoWHSMap.Current.Activate();
        DialogHosts.RealmWindow.Window.Visible = false;
        DialogHosts.MapEvents.TheMapArmyDeselected();
        DialogHosts.MapEvents.TheMapStructureDeselected();
        // The original call at targets the indicator sizing routine
        DialogHosts.TaskBar.UpdatePlayerIndicator();
        DialogHosts.PlayerInfoWindow.UpdatePlayer();
        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            var turn = DialogHosts.TaskBar.WorldTurnButton;
            if (AoWHSMap.Current.SeatedPlayer != -1)
            {
                turn.LastPlayer = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).PlayerRace;
                if (turn.LastPlayer == -1) turn.LastPlayer = 12;
                turn.CurrentPlayer = turn.LastPlayer;
                turn.AnimationFrame = turn.CurrentPlayer * 7;
                DialogHosts.TaskBar.CurrentPlayerImage.Visible = AoWHSMap.Current.PlayMode == PlayMode.Turn;
            }
            else
            {
                turn.CurrentPlayer = 12;
                turn.LastPlayer = turn.CurrentPlayer;
                turn.AnimationFrame = turn.CurrentPlayer * 7;
                DialogHosts.TaskBar.CurrentPlayerImage.Visible = false;
            }
        }
        DialogHosts.TaskBar.WorldClockDelay = 0;
        DialogHosts.TaskBar.ChatButton.ImageLibraryIndexUp = 14;
        DialogHosts.TaskBar.ChatFlash = 0;
        DialogHosts.VideoDialog.SetSceneSize(DialogHosts.VideoDialog.DisplayWidth, DialogHosts.VideoDialog.DisplayHeight);
    }

}
