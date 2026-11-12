// Translated from AoWWinMan.pas.
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
public partial class General : UIModule
{
    public General(UIContext ui) : base(ui)
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
        UI.Engine.CloseHsm(null);
        UI.Engine.CloseCampaign();
        UI.ScannerWindow.Scanner.SetMap(null);
        UI.CombatScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(UI.Engine.StartupDirectory);
        UI.General.HideWindows();
        UI.TaskBar.TaskWindow.Visible = false;
        UI.TitleWindow.MainWindow.Visible = false;
        UI.TitleScene.ResetScene();
        WindowActions.ShowTitleScene(UI.TitleScene, true);
    }

    // AoWWinMan.pas:415
    public void HideWindows()
    {
        if (!WindowsHidden)
        {
            WindowsHidden = true;
            InfoVisible = UI.PlayerInfoWindow.MainWindow.Visible;
            RealmVisible = UI.ControlWindow.MainWindow.Visible;
            MagicVisible = UI.UnitWindow.MainWindow.Visible;
            UnitVisible = UI.RealmWindow.Window.Visible;
            TargetVisible = UI.MagicWindow.Window.Visible;
            ScannerVisible = UI.ScannerWindow.MainWindow.Visible;
            ControlVisible = UI.InfoWindow.MainWindow.Visible;
            ChatVisible = UI.ChatWindow.MainWindow.Visible;
            MapVisible = UI.MapWindow.MainWindow.Visible;
            for (int I = 0, last = UI.General.Manager.Children.Count - 1; I <= last; I++)
            {
                if (UI.General.Manager.Children[I] != UI.TaskBar.TaskWindow)
                {
                    UI.General.Manager.Children[I].Visible = false;
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
            UI.PlayerInfoWindow.MainWindow.Visible = InfoVisible;
            UI.ControlWindow.MainWindow.Visible = RealmVisible;
            UI.UnitWindow.MainWindow.Visible = MagicVisible;
            UI.ScannerWindow.MainWindow.Visible = ScannerVisible;
            UI.InfoWindow.MainWindow.Visible = ControlVisible;
            UI.MapWindow.MainWindow.Visible = MapVisible;
            UI.RealmWindow.Window.Visible = UnitVisible;
            UI.MagicWindow.Window.Visible = TargetVisible;
            UI.ChatWindow.MainWindow.Visible = ChatVisible;
        }
    }

    // AoWWinMan.pas:464
    public void SaveLayout()
    {
        // { Read the saved rectangle through the window pointer cell. }
        var rectangle = new Rect(UI.PlayerInfoWindow.MainWindow.Alignment.RightOffset, UI.PlayerInfoWindow.MainWindow.Alignment.BottomOffset, UI.PlayerInfoWindow.MainWindow.WinWidth, UI.PlayerInfoWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(0, rectangle);
        var rectangle2 = new Rect(UI.ControlWindow.MainWindow.Alignment.RightOffset, UI.ControlWindow.MainWindow.Alignment.TopOffset, UI.ControlWindow.MainWindow.WinWidth, UI.ControlWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(1, rectangle2);
        if (UI.UnitWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Center)
        {
            var rectangle3 = new Rect(UI.UnitWindow.MainWindow.Alignment.LeftOffset, UI.UnitWindow.MainWindow.Alignment.TopOffset, UI.UnitWindow.MainWindow.WinWidth, UI.UnitWindow.MainWindow.WinHeight);
            UI.Engine.Registry.SetRectangle(2, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(2, rectangle3);
        }

        if (UI.RealmWindow.Window.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(UI.RealmWindow.Window.Alignment.LeftOffset, UI.RealmWindow.Window.Alignment.TopOffset, UI.RealmWindow.Window.WinWidth, UI.RealmWindow.Window.WinHeight);
            UI.Engine.Registry.SetRectangle(5, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(5, rectangle3);
        }

        if (UI.MagicWindow.Window.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(UI.MagicWindow.Window.Alignment.LeftOffset, UI.MagicWindow.Window.Alignment.TopOffset, UI.MagicWindow.Window.WinWidth, UI.MagicWindow.Window.WinHeight);
            UI.Engine.Registry.SetRectangle(13, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(13, rectangle3);
        }

        var rectangle4 = new Rect(UI.ScannerWindow.MainWindow.Alignment.RightOffset, UI.ScannerWindow.MainWindow.Alignment.TopOffset, UI.ScannerWindow.MainWindow.WinWidth, UI.ScannerWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(6, rectangle4);
        var rectangle5 = new Rect(UI.InfoWindow.MainWindow.Alignment.RightOffset, UI.InfoWindow.MainWindow.Alignment.TopOffset, UI.InfoWindow.MainWindow.WinWidth, UI.InfoWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(7, rectangle5);
        if (UI.ChatWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(UI.ChatWindow.MainWindow.Alignment.LeftOffset, UI.ChatWindow.MainWindow.Alignment.BottomOffset, UI.ChatWindow.MainWindow.WinWidth, UI.ChatWindow.MainWindow.WinHeight);
            UI.Engine.Registry.SetRectangle(8, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(8, rectangle3);
        }

        if (UI.MapWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(UI.MapWindow.MainWindow.Alignment.LeftOffset, UI.MapWindow.MainWindow.Alignment.TopOffset, UI.MapWindow.MainWindow.WinWidth, UI.MapWindow.MainWindow.WinHeight);
            UI.Engine.Registry.SetRectangle(9, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(9, rectangle3);
        }

        var rectangle6 = new Rect(UI.CombatScanner.CombatScannerWindow.Alignment.RightOffset, UI.CombatScanner.CombatScannerWindow.Alignment.TopOffset, UI.CombatScanner.CombatScannerWindow.WinWidth, UI.CombatScanner.CombatScannerWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(12, rectangle6);
        var rectangle7 = new Rect(UI.CombatUnitInfo.MainWindow.Alignment.RightOffset, UI.CombatUnitInfo.MainWindow.Alignment.BottomOffset, UI.CombatUnitInfo.MainWindow.WinWidth, UI.CombatUnitInfo.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(10, rectangle7);
        if (UI.MapWindow.MapAlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(UI.MapWindow.MapRectangle.Left, UI.MapWindow.MapRectangle.Top, UI.MapWindow.MapRectangle.Right, UI.MapWindow.MapRectangle.Bottom);
            UI.Engine.Registry.SetRectangle(11, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(11, rectangle3);
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
    public void SortLists(ref DelphiList<ListBox> lists, byte order)
    {
        ListBox L;
        int J;
        int insertAt;
        int valueI;
        int valueJ;
        L = lists[0];
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
                    lists[J].Items.Move(I, insertAt);
                }
            }
        }

        for (int I = 0, last = lists.Count - 1; I <= last; I++)
        {
            L = lists[I];
            L.ListOffset = 0;
            L.Selected = -1;
            L.Sized = false;
        }

        lists?.Dispose();
    }

    // AoWWinMan.pas:152
    public void RegisterDialog(Window window)
    {
        var map = UI.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth == AlignWidth.Both && window.Alignment.AlignWidth == AlignWidth.Both)
        {
            map.Alignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight + window.WinHeight;
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
        var map = UI.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth == AlignWidth.Both && window.Alignment.AlignWidth == AlignWidth.Both && (window == UI.ChatWindow.MainWindow || !UI.ChatWindow.MainWindow.Visible || UI.ChatWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both) && (window == UI.MagicWindow.Window || !UI.MagicWindow.Window.Visible || UI.MagicWindow.Window.Alignment.AlignWidth != AlignWidth.Both) && (window == UI.RealmWindow.Window || !UI.RealmWindow.Window.Visible || UI.RealmWindow.Window.Alignment.AlignWidth != AlignWidth.Both))
        {
            map.Alignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
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
                if (UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer && (GamePack.Current.AbilityControl.GetAbility(I).GetControlType(UI.General.SelectedUnit) & Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action)) != Set<AbilityControlTypeElement>.Of())
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
        var scanner = UI.ScannerWindow.MainWindow;
        var scannerAlignment = scanner.Alignment;
        scannerAlignment.TopOffset = 0;
        scannerAlignment.RightOffset = 0;

        if (UI.General.Manager.WinHeight > 700)
        {
            scanner.WinHeight = (UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight) / 3;
        }
        else
        {
            scanner.WinHeight = (UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight) / 2;
        }

        scanner.WinWidth = scanner.WinHeight * 200 / 216;
        if (StrategicMap.Current != null)
        {
            scanner.Visible = StrategicMap.Current.CombatMap == null && UI.MapWindow.MainWindow.Visible;
        }

        var playerInfo = UI.PlayerInfoWindow.MainWindow;
        var playerInfoAlignment = playerInfo.Alignment;
        playerInfoAlignment.BottomOffset = 0;
        playerInfoAlignment.RightOffset = 0;

        playerInfo.WinWidth = scanner.WinWidth;
        playerInfo.WinHeight = scanner.WinHeight;
        if (StrategicMap.Current != null)
        {
            playerInfo.Visible = StrategicMap.Current.CombatMap == null && UI.MapWindow.MainWindow.Visible;
        }

        var controls = UI.ControlWindow.MainWindow;
        var controlsAlignment = controls.Alignment;
        controlsAlignment.TopOffset = scanner.WinHeight;
        controlsAlignment.RightOffset = 0;

        controls.WinWidth = scanner.WinWidth;
        controls.WinHeight = scanner.WinHeight;
        controls.Visible = false;

        var info = UI.InfoWindow.MainWindow;
        var infoAlignment = info.Alignment;
        infoAlignment.TopOffset = scanner.WinHeight;
        infoAlignment.RightOffset = 0;

        info.WinWidth = scanner.WinWidth;
        info.WinHeight = scanner.WinHeight;
        info.Visible = false;

        var targetSelection = UI.TargetSelectionDialog.Window;
        var targetSelectionAlignment = targetSelection.Alignment;
        targetSelectionAlignment.TopOffset = scanner.WinHeight;
        targetSelectionAlignment.RightOffset = 0;

        targetSelection.WinWidth = scanner.WinWidth;
        targetSelection.WinHeight = scanner.WinHeight;

        var realm = UI.RealmWindow.Window;
        var realmAlignment = realm.Alignment;
        realmAlignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        realmAlignment.LeftOffset = 0;
        realmAlignment.RightOffset = scanner.WinWidth;
        realmAlignment.AlignWidth = AlignWidth.Both;
        realmAlignment.AlignHeight = AlignHeight.Bottom;

        realm.WinWidth = realm.MinWidth;
        realm.WinHeight = realm.MinHeight;
        realm.Visible = false;

        var magic = UI.MagicWindow.Window;
        var magicAlignment = magic.Alignment;
        magicAlignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        magicAlignment.LeftOffset = 0;
        magicAlignment.RightOffset = scanner.WinWidth;
        magicAlignment.AlignWidth = AlignWidth.Both;
        magicAlignment.AlignHeight = AlignHeight.Bottom;

        magic.WinWidth = magic.MinWidth;
        magic.WinHeight = magic.MinHeight;
        magic.Visible = false;

        var unitWindow = UI.UnitWindow.MainWindow;
        var unitWindowAlignment = unitWindow.Alignment;
        unitWindowAlignment.AlignWidth = AlignWidth.Center;
        unitWindowAlignment.AlignHeight = AlignHeight.Center;

        unitWindow.WinWidth = unitWindow.MinWidth;
        unitWindow.WinHeight = unitWindow.MinHeight;
        unitWindow.Visible = false;

        var chat = UI.ChatWindow.MainWindow;
        var chatAlignment = chat.Alignment;
        chatAlignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        chatAlignment.LeftOffset = 0;
        chatAlignment.RightOffset = scanner.WinWidth;
        chatAlignment.AlignWidth = AlignWidth.Both;
        chatAlignment.AlignHeight = AlignHeight.Bottom;

        chat.WinWidth = chat.MinWidth;
        chat.WinHeight = chat.MinHeight;
        chat.Visible = false;

        var tacticalBattleScanner = UI.CombatScanner.CombatScannerWindow;
        var tacticalBattleScannerAlignment = tacticalBattleScanner.Alignment;
        tacticalBattleScannerAlignment.TopOffset = 0;
        tacticalBattleScannerAlignment.RightOffset = 0;

        tacticalBattleScanner.WinHeight = (UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight) * 170 / 432;
        tacticalBattleScanner.WinWidth = tacticalBattleScanner.WinHeight * 150 / 170;
        if (StrategicMap.Current != null)
        {
            tacticalBattleScanner.Visible = StrategicMap.Current.CombatMap != null && UI.MapWindow.MainWindow.Visible;
        }

        var tacticalBattleUnit = UI.CombatUnitInfo.MainWindow;
        var tacticalBattleUnitAlignment = tacticalBattleUnit.Alignment;
        tacticalBattleUnitAlignment.BottomOffset = 0;
        tacticalBattleUnitAlignment.RightOffset = 0;

        tacticalBattleUnit.WinHeight = UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight - tacticalBattleScanner.WinHeight;
        tacticalBattleUnit.WinWidth = tacticalBattleScanner.WinWidth;
        tacticalBattleUnit.Visible = false;

        var mapAlignment = UI.MapWindow.MainWindow.Alignment;
        mapAlignment.AlignHeight = AlignHeight.Both;
        mapAlignment.AlignWidth = AlignWidth.Both;
        mapAlignment.TopOffset = 0;
        mapAlignment.LeftOffset = 0;

        UI.MapWindow.WorldAlignHeight = AlignHeight.Both;
        UI.MapWindow.WorldAlignWidth = AlignWidth.Both;
        var rectangle = new Rect(0, 0, scanner.WinWidth, UI.TaskBar.TaskWindow.WinHeight);
        UI.MapWindow.WorldRectangle = rectangle;
        UI.MapWindow.MapAlignHeight = AlignHeight.Both;
        UI.MapWindow.MapAlignWidth = AlignWidth.Both;
        var rectangle2 = new Rect(0, 0, tacticalBattleScanner.WinWidth, UI.TaskBar.TaskWindow.WinHeight);
        UI.MapWindow.MapRectangle = rectangle2;
        if (StrategicMap.Current != null)
        {
            if (StrategicMap.Current.CombatMap == null)
            {
                UI.MapWindow.MainWindow.Alignment.RightOffset = scanner.WinWidth;
            }
            else
            {
                UI.MapWindow.MainWindow.Alignment.RightOffset = tacticalBattleScanner.WinWidth;
            }
        }

        UI.MapWindow.MainWindow.Alignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        if (UI.ChatWindow.MainWindow.Visible)
        {
            RegisterDialog(UI.ChatWindow.MainWindow);
        }

        Manager.Aligned = false;
    }

    // AoWWinMan.pas:336
    public void ManagerDebugMessage(string text)
    {
        UI.DebugWindow.AddMessage(text);
    }

    // AoWWinMan.pas:351
    public void ManagerKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        Player player;
        string S;
        if (UI.Engine.Debug)
        {
            if (StrategicMap.Current != null && (StrategicMap.Current.PlayMode != PlayMode.Simultaneous || StrategicMap.Current.Players.LocalPlayers.Count == 1) && Set<byte>.WithBounds(0, 255).WithRange(49, 57).Contains((byte)key) && CombatMap.Current == null && StrategicMap.Current.Players.GetCount() > key - 48 && UI.Engine.Registry.GetDebugMode())
            {
                player = StrategicMap.Current.Players.GetPlayers(key - 48);
                if (player.PlayerType == PlayerType.Cpu5)
                {
                    player.PlayerType = PlayerType.Human;
                }
                else
                {
                    player.PlayerType = (PlayerType)((int)player.PlayerType + 1);
                }

                var playerType = UI.Engine.TranslateResourceString(GameTables.PlayerTypeTexts[(int)player.PlayerType]);
                S = "Player " + IntToStr(key - 48) + " set to: " + playerType;
                StrategicMap.Current.ShowMessage(S);
                StrategicMap.Current.Players.Update();
                StrategicMap.Current.RequestCombatType = StrategicMap.Current.Players.LocalHumanPlayers.Count > 1;
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
                window != UI.MapWindow.MainWindow && window._modal && window.Visible)
                window.Visible = false;
    }

    internal static void HideGameWindows(General controller)
    {
        if (!controller.WindowsHidden)
        {
            controller.WindowsHidden = true;
            controller.InfoVisible = controller.UI.PlayerInfoWindow.MainWindow.Visible;
            controller.RealmVisible = controller.UI.RealmWindow.Window.Visible;
            controller.MagicVisible = controller.UI.MagicWindow.Window.Visible;
            controller.UnitVisible = controller.UI.UnitWindow.MainWindow.Visible;
            controller.TargetVisible = controller.UI.TargetSelectionDialog.Window.Visible;
            controller.ScannerVisible = controller.UI.ScannerWindow.MainWindow.Visible;
            controller.ControlVisible = controller.UI.ControlWindow.MainWindow.Visible;
            controller.ChatVisible = controller.UI.ChatWindow.MainWindow.Visible;
            controller.MapVisible = controller.UI.MapWindow.MainWindow.Visible;
            for (int I = 0, last = controller.UI.General.Manager.Children.Count - 1; I <= last; I++)
            {
                if (controller.UI.General.Manager.Children[I] != controller.UI.TaskBar.TaskWindow)
                {
                    controller.UI.General.Manager.Children[I].Visible = false;
                }
            }
        }
    }

    internal static void RestoreGameWindows(General controller)
    {
        if (controller.WindowsHidden)
        {
            controller.WindowsHidden = false;
            controller.UI.PlayerInfoWindow.MainWindow.Visible = controller.InfoVisible;
            controller.UI.RealmWindow.Window.Visible = controller.RealmVisible;
            controller.UI.MagicWindow.Window.Visible = controller.MagicVisible;
            controller.UI.ScannerWindow.MainWindow.Visible = controller.ScannerVisible;
            controller.UI.ControlWindow.MainWindow.Visible = controller.ControlVisible;
            controller.UI.MapWindow.MainWindow.Visible = controller.MapVisible;
            controller.UI.UnitWindow.MainWindow.Visible = controller.UnitVisible;
            controller.UI.TargetSelectionDialog.Window.Visible = controller.TargetVisible;
            controller.UI.ChatWindow.MainWindow.Visible = controller.ChatVisible;
        }
    }

    internal static void ExitToTitle(General controller)
    {
        controller.UI.Engine.CloseHsm(null);
        controller.UI.Engine.CloseCampaign();
        controller.UI.ScannerWindow.Scanner.SetMap(null);
        controller.UI.CombatScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(controller.UI.Engine.StartupDirectory);
        HideGameWindows(controller.UI.General);
        controller.UI.TaskBar.TaskWindow.Visible = false;
        controller.UI.TitleWindow.MainWindow.Visible = false;
        controller.UI.TitleScene.ResetScene();
        controller.UI.TitleScene.ShowScene(true);
    }

    internal static void SaveWindowLayout(General controller)
    {
        var playerInfo = controller.UI.PlayerInfoWindow.MainWindow;
        var rectangle = new Rect(playerInfo.Alignment.RightOffset, playerInfo.Alignment.BottomOffset, playerInfo.WinWidth, playerInfo.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(0, rectangle);

        var controls = controller.UI.ControlWindow.MainWindow;
        var rectangle2 = new Rect(controls.Alignment.RightOffset, controls.Alignment.TopOffset, controls.WinWidth, controls.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(1, rectangle2);

        var unitWindow = controller.UI.UnitWindow.MainWindow;
        if (unitWindow.Alignment.AlignWidth != AlignWidth.Center)
        {
            var rectangle3 = new Rect(unitWindow.Alignment.LeftOffset, unitWindow.Alignment.TopOffset, unitWindow.WinWidth, unitWindow.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(2, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(2, rectangle3);
        }

        var realm = controller.UI.RealmWindow.Window;
        if (realm.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(realm.Alignment.LeftOffset, realm.Alignment.TopOffset, realm.WinWidth, realm.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(5, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(5, rectangle3);
        }

        var magic = controller.UI.MagicWindow.Window;
        if (magic.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(magic.Alignment.LeftOffset, magic.Alignment.TopOffset, magic.WinWidth, magic.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(13, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(13, rectangle3);
        }

        var scanner = controller.UI.ScannerWindow.MainWindow;
        var rectangle4 = new Rect(scanner.Alignment.RightOffset, scanner.Alignment.TopOffset, scanner.WinWidth, scanner.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(6, rectangle4);

        var info = controller.UI.InfoWindow.MainWindow;
        var rectangle5 = new Rect(info.Alignment.RightOffset, info.Alignment.TopOffset, info.WinWidth, info.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(7, rectangle5);

        var chat = controller.UI.ChatWindow.MainWindow;
        if (chat.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(chat.Alignment.LeftOffset, chat.Alignment.BottomOffset, chat.WinWidth, chat.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(8, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(8, rectangle3);
        }

        var map = controller.UI.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(map.Alignment.LeftOffset, map.Alignment.TopOffset, map.WinWidth, map.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(9, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(9, rectangle3);
        }

        var tacticalBattleUnit = controller.UI.CombatUnitInfo.MainWindow;
        var rectangle6 = new Rect(tacticalBattleUnit.Alignment.RightOffset, tacticalBattleUnit.Alignment.TopOffset, tacticalBattleUnit.WinWidth, tacticalBattleUnit.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(12, rectangle6);

        var tacticalBattleScanner = controller.UI.CombatScanner.CombatScannerWindow;
        var rectangle7 = new Rect(tacticalBattleScanner.Alignment.RightOffset, tacticalBattleScanner.Alignment.BottomOffset, tacticalBattleScanner.WinWidth, tacticalBattleScanner.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(10, rectangle7);

        if (controller.UI.MapWindow.MapAlignWidth != AlignWidth.Both)
        {
            var rectangle3 = new Rect(controller.UI.MapWindow.MapRectangle.Left, controller.UI.MapWindow.MapRectangle.Top, controller.UI.MapWindow.MapRectangle.Right, controller.UI.MapWindow.MapRectangle.Bottom);
            controller.UI.Engine.Registry.SetRectangle(11, rectangle3);
        }
        else
        {
            var rectangle3 = new Rect(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(11, rectangle3);
        }
    }

    internal static bool HasModalWindow(General controller)
    {
        for (int i = controller.Manager.Children.Count - 1; i >= 0; i--)
            if (controller.Manager.Children[i] is WindowControl window &&
                window != controller.UI.MapWindow.MainWindow && window._modal && window.Visible)
                return true;
        return false;
    }

    internal static void LoadGame(General controller, string name)
    {
        if (StrategicMap.Current != null)
        {
            controller.UI.Engine.CloseHsm(null);
        }

        var loading = controller.UI.Engine.TranslateResourceString(LayerText.LoadingText);
        var fileName = ExtractFileName(name);
        controller.UI.ProgressDialog.TitleLabel.Text = loading + " " + fileName + "...";
        controller.UI.ProgressDialog.StatusLabel.Text = "";
        controller.UI.ProgressDialog.ProgressBar.Position = 0;
        controller.UI.ProgressDialog.ProgressBar.Max = 100;
        controller.UI.ProgressDialog.Window.Visible = true;
        if (!controller.UI.TitleScene.Window.Visible)
        {
            controller.UI.General.Manager.OverlayPriority = 5000;
        }

        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        AudioPlayback.FadeMusicVolume(0, 1);
        controller.UI.Engine.LoadHsm(name, false, (_, message, progression) => controller.MapProgress(message, progression), GameProfile.WorldMap);
        if (controller.UI.Engine.Demo && StrategicMap.Current != null && !StrategicMap.Current.ShowAllPlayers)
        {
            controller.ReturnToTitle();
        }

        controller.UI.General.Manager.OverlayPriority = 0;
        controller.UI.ProgressDialog.Window.Visible = false;
    }

    internal static void StartGame(General controller)
    {
        controller.WindowsHidden = false;
        AudioHost.SongPlayer.Songs.Clear();
        if (StrategicMap.Current.GameSongs.Songs.Count != 0) AudioHost.SongPlayer.Songs.AddStrings(StrategicMap.Current.GameSongs.Songs);
        else if (StrategicMap.Current.StartupSongs.Songs.Count != 0)
        {
            for (int i = 0; i < StrategicMap.Current.StartupSongs.Songs.Count; i++)
                if (RtlUnit.CompareText(StrategicMap.Current.StartupSongs.SongNames[i], controller.UI.Engine.TranslateResourceString(LayerText.FileNotFoundText)) != 0)
                    AudioHost.SongPlayer.Songs.Add(StrategicMap.Current.StartupSongs.Songs[i]);
        }
        else AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
        AudioHost.SongPlayer.Play(0);
        AudioPlayback.SetMusicVolume(controller.UI.SoundDialog.MusicVolumeSlider.Position);
        var menu = controller.UI.PopupWindows.OverviewsMenu.Items;
        menu.Clear();
        menu.Add(controller.UI.Engine.TranslateResourceString(LayerText.BackgroundText));
        if (controller.UI.Engine.Campaign != null) menu.Add(controller.UI.Engine.TranslateResourceString(LayerText.CampaignText));
        controller.UI.TitleScene.Window.Visible = false;
        controller.UI.TitleWindow.MainWindow.Visible = false;
        controller.UI.TaskBar.TaskWindow.Visible = true;
        controller.UI.MapWindow.MainWindow.Visible = true;
        controller.UI.ScannerWindow.MainWindow.Visible = true;
        controller.UI.MapWindow.MapViewer.HexSpriteMap = StrategicMap.Current;
        var scanner = controller.UI.ScannerWindow.Scanner;
        scanner.SetMap(StrategicMap.Current);
        scanner.SetLevel(StrategicMap.Current.MapContainer.GetMapLevel(0));
        controller.UI.TaskBar.WorldTurnButton.ButtonChecked = false;
        controller.UI.TaskBar.ChatButton.Visible = controller.UI.SetupEvents.DirectPlay.Connected;
        controller.UI.General.Manager.Aligned = false;
        scanner.PreviousWidth = 0;
        scanner.Incremental = false;
        scanner.Sized = false;
        var tabs = controller.UI.ScannerWindow.ScannerTabs;
        tabs.Items.Clear();
        tabs.Items.Add(controller.UI.Engine.TranslateResourceString(LayerText.SurfaceText));
        if (StrategicMap.Current.MapContainer.LevelCount > 1) tabs.Items.Add(controller.UI.Engine.TranslateResourceString(LayerText.CavernsText));
        if (StrategicMap.Current.MapContainer.LevelCount > 2) tabs.Items.Add(controller.UI.Engine.TranslateResourceString(LayerText.DepthsText));
        tabs.Selected = 0;
        tabs.Sized = false;
        scanner.SetTWidth(0);
        scanner.SetTHeight(0);
        scanner.SetTLeft(-1);
        scanner.SetTTop(-1);
        scanner.UpdateStep = -1;
        scanner.SetMap(StrategicMap.Current);
        scanner.SetLevel(StrategicMap.Current.MapContainer.GetMapLevel(0));
        controller.UI.MapWindow.MapViewer.HexSpriteMap = StrategicMap.Current;
        controller.UI.MapWindow.MapViewer.SceneWidth = controller.UI.MapWindow.MainWindow.WinWidth - 24;
        controller.UI.MapWindow.MapViewer.SceneHeight = controller.UI.MapWindow.MainWindow.WinHeight - 24;
        StrategicMap.Current.Activate();
        controller.UI.RealmWindow.Window.Visible = false;
        controller.UI.MapEvents.TheMapArmyDeselected();
        controller.UI.MapEvents.TheMapStructureDeselected();
        // The original call at targets the indicator sizing routine
        controller.UI.TaskBar.UpdatePlayerIndicator();
        controller.UI.PlayerInfoWindow.UpdatePlayer();
        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            var turn = controller.UI.TaskBar.WorldTurnButton;
            if (StrategicMap.Current.SeatedPlayer != -1)
            {
                turn.LastPlayer = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).PlayerRace;
                if (turn.LastPlayer == -1) turn.LastPlayer = 12;
                turn.CurrentPlayer = turn.LastPlayer;
                turn.AnimationFrame = turn.CurrentPlayer * 7;
                controller.UI.TaskBar.CurrentPlayerImage.Visible = StrategicMap.Current.PlayMode == PlayMode.Turn;
            }
            else
            {
                turn.CurrentPlayer = 12;
                turn.LastPlayer = turn.CurrentPlayer;
                turn.AnimationFrame = turn.CurrentPlayer * 7;
                controller.UI.TaskBar.CurrentPlayerImage.Visible = false;
            }
        }
        controller.UI.TaskBar.WorldClockDelay = 0;
        controller.UI.TaskBar.ChatButton.ImageLibraryIndexUp = 14;
        controller.UI.TaskBar.ChatFlash = 0;
        controller.UI.VideoDialog.SetSceneSize(controller.UI.VideoDialog.DisplayWidth, controller.UI.VideoDialog.DisplayHeight);
    }

}
