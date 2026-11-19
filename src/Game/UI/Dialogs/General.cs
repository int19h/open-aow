#nullable disable
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
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.UI.Dialogs;

// preview settings. abilities. active ability count. selected indices. unit list. selected
// unit. combat sprite
public partial class General(UIContext ui) : UIModule(ui)
{
    public WindowManager Manager { get; set; }
    public ToolTip ToolTip { get; private set; }

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

    public UnitShowInfo UnitShowInfo { get; private set; }

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
        _ = AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        _ = SetCurrentDir(UI.Engine.StartupDirectory);
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
            for (int i = 0, last = UI.General.Manager.Children.Count - 1; i <= last; i++)
            {
                if (UI.General.Manager.Children[i] != UI.TaskBar.TaskWindow)
                {
                    UI.General.Manager.Children[i].Visible = false;
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
        Rect rectangle = new(UI.PlayerInfoWindow.MainWindow.Alignment.RightOffset, UI.PlayerInfoWindow.MainWindow.Alignment.BottomOffset, UI.PlayerInfoWindow.MainWindow.WinWidth, UI.PlayerInfoWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(0, rectangle);
        Rect rectangle2 = new(UI.ControlWindow.MainWindow.Alignment.RightOffset, UI.ControlWindow.MainWindow.Alignment.TopOffset, UI.ControlWindow.MainWindow.WinWidth, UI.ControlWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(1, rectangle2);
        if (UI.UnitWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Center)
        {
            Rect rectangle3 = new(UI.UnitWindow.MainWindow.Alignment.LeftOffset, UI.UnitWindow.MainWindow.Alignment.TopOffset, UI.UnitWindow.MainWindow.WinWidth, UI.UnitWindow.MainWindow.WinHeight);
            UI.Engine.Registry.SetRectangle(2, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(2, rectangle3);
        }

        if (UI.RealmWindow.Window.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(UI.RealmWindow.Window.Alignment.LeftOffset, UI.RealmWindow.Window.Alignment.TopOffset, UI.RealmWindow.Window.WinWidth, UI.RealmWindow.Window.WinHeight);
            UI.Engine.Registry.SetRectangle(5, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(5, rectangle3);
        }

        if (UI.MagicWindow.Window.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(UI.MagicWindow.Window.Alignment.LeftOffset, UI.MagicWindow.Window.Alignment.TopOffset, UI.MagicWindow.Window.WinWidth, UI.MagicWindow.Window.WinHeight);
            UI.Engine.Registry.SetRectangle(13, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(13, rectangle3);
        }

        Rect rectangle4 = new(UI.ScannerWindow.MainWindow.Alignment.RightOffset, UI.ScannerWindow.MainWindow.Alignment.TopOffset, UI.ScannerWindow.MainWindow.WinWidth, UI.ScannerWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(6, rectangle4);
        Rect rectangle5 = new(UI.InfoWindow.MainWindow.Alignment.RightOffset, UI.InfoWindow.MainWindow.Alignment.TopOffset, UI.InfoWindow.MainWindow.WinWidth, UI.InfoWindow.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(7, rectangle5);
        if (UI.ChatWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(UI.ChatWindow.MainWindow.Alignment.LeftOffset, UI.ChatWindow.MainWindow.Alignment.BottomOffset, UI.ChatWindow.MainWindow.WinWidth, UI.ChatWindow.MainWindow.WinHeight);
            UI.Engine.Registry.SetRectangle(8, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(8, rectangle3);
        }

        if (UI.MapWindow.MainWindow.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(UI.MapWindow.MainWindow.Alignment.LeftOffset, UI.MapWindow.MainWindow.Alignment.TopOffset, UI.MapWindow.MainWindow.WinWidth, UI.MapWindow.MainWindow.WinHeight);
            UI.Engine.Registry.SetRectangle(9, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(9, rectangle3);
        }

        Rect rectangle6 = new(UI.CombatScanner.CombatScannerWindow.Alignment.RightOffset, UI.CombatScanner.CombatScannerWindow.Alignment.TopOffset, UI.CombatScanner.CombatScannerWindow.WinWidth, UI.CombatScanner.CombatScannerWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(12, rectangle6);
        Rect rectangle7 = new(UI.CombatUnitInfo.MainWindow.Alignment.RightOffset, UI.CombatUnitInfo.MainWindow.Alignment.BottomOffset, UI.CombatUnitInfo.MainWindow.WinWidth, UI.CombatUnitInfo.MainWindow.WinHeight);
        UI.Engine.Registry.SetRectangle(10, rectangle7);
        if (UI.MapWindow.MapAlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(UI.MapWindow.MapRectangle.Left, UI.MapWindow.MapRectangle.Top, UI.MapWindow.MapRectangle.Right, UI.MapWindow.MapRectangle.Bottom);
            UI.Engine.Registry.SetRectangle(11, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            UI.Engine.Registry.SetRectangle(11, rectangle3);
        }
    }

    // AoWWinMan.pas:79
    public void PlayButtonSound(byte kind)
    {
        switch (kind)
        {
            case 0:
                _ = AudioPlayback.PlaySample("Sfx\\Int\\Btn1.wav");
                break;
            case 3:
                _ = AudioPlayback.PlaySample("Sfx\\Int\\Btn3.wav");
                break;
            case 1:
                _ = AudioPlayback.PlaySample("Sfx\\Int\\Btn5.wav");
                break;
            case 2:
                _ = AudioPlayback.PlaySample("Sfx\\Int\\Btn6.wav");
                break;
            default:
                break;
        }
    }

    // AoWWinMan.pas:123
    public void SortLists(DelphiList<ListBox> lists, byte order)
    {
        ListBox l;
        int j;
        int insertAt;
        int valueI;
        int valueJ;
        l = lists[0];
        for (int i = 1, last = l.Items.Count - 1; i <= last; i++)
        {
            j = i - 1;
            insertAt = -1;
            while (j > -1)
            {
                switch (order)
                {
                    case 0:
                        string item = l.Items[j];
                        if (CompareStr(UpperCase(l.Items[i]), UpperCase(item)) < 0)
                        {
                            insertAt = j;
                        }

                        break;
                    case 1:
                        string item2 = l.Items[j];
                        if (CompareStr(UpperCase(l.Items[i]), UpperCase(item2)) > 0)
                        {
                            insertAt = j;
                        }

                        break;
                    case 2:
                        try
                        {
                            valueI = StrToInt(l.Items[i]);
                            valueJ = StrToInt(l.Items[j]);
                            if (valueI > valueJ)
                            {
                                insertAt = j;
                            }
                        }
                        catch
                        {
                        }

                        break;
                    case 3:
                        try
                        {
                            valueI = StrToInt(l.Items[i]);
                            valueJ = StrToInt(l.Items[j]);
                            if (valueI < valueJ)
                            {
                                insertAt = j;
                            }
                        }
                        catch
                        {
                        }

                        break;
                    default:
                        break;
                }

                j -= 1;
            }

            if (insertAt > -1)
            {
                j = 0;
                int limit = lists.Count - 1;
                for (; j <= limit; j += 1)
                {
                    lists[j].Items.Move(i, insertAt);
                }
            }
        }

        for (int i = 0, last = lists.Count - 1; i <= last; i++)
        {
            l = lists[i];
            l.ListOffset = 0;
            l.Selected = -1;
            l.Sized = false;
        }

        lists?.Dispose();
    }

    // AoWWinMan.pas:152
    public void RegisterDialog(Window window)
    {
        Window map = UI.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth == AlignWidth.Both && window.Alignment.AlignWidth == AlignWidth.Both)
        {
            map.Alignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight + window.WinHeight;
            map.Aligned = false;
            window.Alignment.RightOffset = map.Visible ? map.Alignment.RightOffset : 0;

            window.Aligned = false;
        }
    }

    // AoWWinMan.pas:168
    public void UnregisterDialog(Window window)
    {
        Window map = UI.MapWindow.MainWindow;
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
            for (int i = 0; i <= 7; i++)
            {
                if (SelectedUnits[i] > -1 && i < UnitList.Units.Count)
                {
                    if (U == UnitList.Units.GetUnit(SelectedUnits[i]))
                    {
                        result = i;
                    }
                }
            }
        }

        return result;
    }

    // AoWWinMan.pas:100
    public void BuildAbilities()
    {
        CombatAbilityRef a;
        ActiveAbilityCount = 0;
        for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
        {
            (Abilities[i] as IDisposable)?.Dispose();
        }

        Abilities.Clear();
        for (int i = 0; i <= 169; i++)
        {
            if (SelectedUnit.GetAbilityEnabled(i))
            {
                a = new CombatAbilityRef
                {
                    Ability = i
                };
                if (UI.General.SelectedUnit.Player == StrategicMap.Current.SeatedPlayer && (GamePack.Current.AbilityControl.GetAbility(i).GetControlType(UI.General.SelectedUnit) & Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action)) != Set<AbilityControlTypeElement>.Of())
                {
                    Abilities.Insert(ActiveAbilityCount, a);
                    ActiveAbilityCount += 1;
                }
                else
                {
                    Abilities.Add(a);
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
        int result = sphere switch
        {
            MagicSphere.Life => 27,
            MagicSphere.Death => 30,
            MagicSphere.Earth => 33,
            MagicSphere.Air => 36,
            MagicSphere.Water => 39,
            MagicSphere.Cosmos or MagicSphere.Fire or MagicSphere.Secret or MagicSphere.None or _ => 42,
        };
        return result;
    }

    // AoWWinMan.pas:216
    public void GeneralCreate()
    {
        UnitShowInfo unitShowInfo = UnitShowInfo;
        unitShowInfo.Direction = 3;
        UnitShowInfo = unitShowInfo;
        UnitShowInfo unitShowInfo2 = UnitShowInfo;
        unitShowInfo2.Overlays = Set<UnitOverlayModesElement>.Of(UnitOverlayModesElement.Rank);
        UnitShowInfo = unitShowInfo2;
        Abilities = [];
    }

    // AoWWinMan.pas:223
    public void GeneralDestroy()
    {
        for (int i = 0, last = Abilities.Count - 1; i <= last; i++)
        {
            (Abilities[i] as IDisposable)?.Dispose();
        }

        Abilities?.Clear();
    }

    // AoWWinMan.pas:231
    public void ResetLayout()
    {
        Window scanner = UI.ScannerWindow.MainWindow;
        Controls.Alignment scannerAlignment = scanner.Alignment;
        scannerAlignment.TopOffset = 0;
        scannerAlignment.RightOffset = 0;

        scanner.WinHeight = UI.General.Manager.WinHeight > 700
            ? (UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight) / 3
            : (UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight) / 2;

        scanner.WinWidth = scanner.WinHeight * 200 / 216;
        if (StrategicMap.Current != null)
        {
            scanner.Visible = StrategicMap.Current.CombatMap == null && UI.MapWindow.MainWindow.Visible;
        }

        Window playerInfo = UI.PlayerInfoWindow.MainWindow;
        Controls.Alignment playerInfoAlignment = playerInfo.Alignment;
        playerInfoAlignment.BottomOffset = 0;
        playerInfoAlignment.RightOffset = 0;

        playerInfo.WinWidth = scanner.WinWidth;
        playerInfo.WinHeight = scanner.WinHeight;
        if (StrategicMap.Current != null)
        {
            playerInfo.Visible = StrategicMap.Current.CombatMap == null && UI.MapWindow.MainWindow.Visible;
        }

        Window controls = UI.ControlWindow.MainWindow;
        Controls.Alignment controlsAlignment = controls.Alignment;
        controlsAlignment.TopOffset = scanner.WinHeight;
        controlsAlignment.RightOffset = 0;

        controls.WinWidth = scanner.WinWidth;
        controls.WinHeight = scanner.WinHeight;
        controls.Visible = false;

        Window info = UI.InfoWindow.MainWindow;
        Controls.Alignment infoAlignment = info.Alignment;
        infoAlignment.TopOffset = scanner.WinHeight;
        infoAlignment.RightOffset = 0;

        info.WinWidth = scanner.WinWidth;
        info.WinHeight = scanner.WinHeight;
        info.Visible = false;

        Window targetSelection = UI.TargetSelectionDialog.Window;
        Controls.Alignment targetSelectionAlignment = targetSelection.Alignment;
        targetSelectionAlignment.TopOffset = scanner.WinHeight;
        targetSelectionAlignment.RightOffset = 0;

        targetSelection.WinWidth = scanner.WinWidth;
        targetSelection.WinHeight = scanner.WinHeight;

        Window realm = UI.RealmWindow.Window;
        Controls.Alignment realmAlignment = realm.Alignment;
        realmAlignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        realmAlignment.LeftOffset = 0;
        realmAlignment.RightOffset = scanner.WinWidth;
        realmAlignment.AlignWidth = AlignWidth.Both;
        realmAlignment.AlignHeight = AlignHeight.Bottom;

        realm.WinWidth = realm.MinWidth;
        realm.WinHeight = realm.MinHeight;
        realm.Visible = false;

        Window magic = UI.MagicWindow.Window;
        Controls.Alignment magicAlignment = magic.Alignment;
        magicAlignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        magicAlignment.LeftOffset = 0;
        magicAlignment.RightOffset = scanner.WinWidth;
        magicAlignment.AlignWidth = AlignWidth.Both;
        magicAlignment.AlignHeight = AlignHeight.Bottom;

        magic.WinWidth = magic.MinWidth;
        magic.WinHeight = magic.MinHeight;
        magic.Visible = false;

        Window unitWindow = UI.UnitWindow.MainWindow;
        Controls.Alignment unitWindowAlignment = unitWindow.Alignment;
        unitWindowAlignment.AlignWidth = AlignWidth.Center;
        unitWindowAlignment.AlignHeight = AlignHeight.Center;

        unitWindow.WinWidth = unitWindow.MinWidth;
        unitWindow.WinHeight = unitWindow.MinHeight;
        unitWindow.Visible = false;

        Window chat = UI.ChatWindow.MainWindow;
        Controls.Alignment chatAlignment = chat.Alignment;
        chatAlignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        chatAlignment.LeftOffset = 0;
        chatAlignment.RightOffset = scanner.WinWidth;
        chatAlignment.AlignWidth = AlignWidth.Both;
        chatAlignment.AlignHeight = AlignHeight.Bottom;

        chat.WinWidth = chat.MinWidth;
        chat.WinHeight = chat.MinHeight;
        chat.Visible = false;

        Window tacticalBattleScanner = UI.CombatScanner.CombatScannerWindow;
        Controls.Alignment tacticalBattleScannerAlignment = tacticalBattleScanner.Alignment;
        tacticalBattleScannerAlignment.TopOffset = 0;
        tacticalBattleScannerAlignment.RightOffset = 0;

        tacticalBattleScanner.WinHeight = (UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight) * 170 / 432;
        tacticalBattleScanner.WinWidth = tacticalBattleScanner.WinHeight * 150 / 170;
        if (StrategicMap.Current != null)
        {
            tacticalBattleScanner.Visible = StrategicMap.Current.CombatMap != null && UI.MapWindow.MainWindow.Visible;
        }

        Window tacticalBattleUnit = UI.CombatUnitInfo.MainWindow;
        Controls.Alignment tacticalBattleUnitAlignment = tacticalBattleUnit.Alignment;
        tacticalBattleUnitAlignment.BottomOffset = 0;
        tacticalBattleUnitAlignment.RightOffset = 0;

        tacticalBattleUnit.WinHeight = UI.General.Manager.WinHeight - UI.TaskBar.TaskWindow.WinHeight - tacticalBattleScanner.WinHeight;
        tacticalBattleUnit.WinWidth = tacticalBattleScanner.WinWidth;
        tacticalBattleUnit.Visible = false;

        Controls.Alignment mapAlignment = UI.MapWindow.MainWindow.Alignment;
        mapAlignment.AlignHeight = AlignHeight.Both;
        mapAlignment.AlignWidth = AlignWidth.Both;
        mapAlignment.TopOffset = 0;
        mapAlignment.LeftOffset = 0;

        UI.MapWindow.WorldAlignHeight = AlignHeight.Both;
        UI.MapWindow.WorldAlignWidth = AlignWidth.Both;
        Rect rectangle = new(0, 0, scanner.WinWidth, UI.TaskBar.TaskWindow.WinHeight);
        UI.MapWindow.WorldRectangle = rectangle;
        UI.MapWindow.MapAlignHeight = AlignHeight.Both;
        UI.MapWindow.MapAlignWidth = AlignWidth.Both;
        Rect rectangle2 = new(0, 0, tacticalBattleScanner.WinWidth, UI.TaskBar.TaskWindow.WinHeight);
        UI.MapWindow.MapRectangle = rectangle2;
        if (StrategicMap.Current != null)
        {
            UI.MapWindow.MainWindow.Alignment.RightOffset = StrategicMap.Current.CombatMap == null
                ? scanner.WinWidth
                : tacticalBattleScanner.WinWidth;
        }

        UI.MapWindow.MainWindow.Alignment.BottomOffset = UI.TaskBar.TaskWindow.WinHeight;
        if (UI.ChatWindow.MainWindow.Visible)
        {
            RegisterDialog(UI.ChatWindow.MainWindow);
        }

        Manager.Aligned = false;
    }

    // AoWWinMan.pas:336
    public void ManagerDebugMessage(string text) => UI.DebugWindow.AddMessage(text);

    // AoWWinMan.pas:351
    public void ManagerKeyDown(in ushort key, Set<ShiftState> shift)
    {
        Player player;
        string s;
        if (UI.Engine.Debug)
        {
            if (StrategicMap.Current != null && (StrategicMap.Current.PlayMode != PlayMode.Simultaneous || StrategicMap.Current.Players.LocalPlayers.Count == 1) && Set<byte>.WithBounds(0, 255).WithRange(49, 57).Contains((byte)key) && CombatMap.Current == null && StrategicMap.Current.Players.GetCount() > key - 48 && UI.Engine.Registry.GetDebugMode())
            {
                player = StrategicMap.Current.Players.GetPlayers(key - 48);
                player.PlayerType = player.PlayerType == PlayerType.Cpu5 ? PlayerType.Human : (PlayerType)((int)player.PlayerType + 1);

                string playerType = UI.Engine.TranslateResourceString(GameTables.PlayerTypeTexts[(int)player.PlayerType]);
                s = $"Player {IntToStr(key - 48)} set to: {playerType}";
                StrategicMap.Current.ShowMessage(s);
                StrategicMap.Current.Players.Update();
                StrategicMap.Current.RequestCombatType = StrategicMap.Current.Players.LocalHumanPlayers.Count > 1;
            }

            switch (key)
            {
                case 122:
                    s = ExtractFileDir(LegacyApplication.Application.ExeName);
                    s += "\\Scenario\\Custom";
                    _ = SetCurrentDir(s);
                    LoadGame(this, FileBoundary.ReadName("Crap.hsm"));
                    StartGame(this);
                    break;
                case 123:
                    s = ExtractFileDir(LegacyApplication.Application.ExeName);
                    s += "\\Scenario\\Custom";
                    _ = SetCurrentDir(s);
                    LoadGame(this, FileBoundary.ReadName("demomap.hsm"));
                    StartGame(this);
                    break;
                default:
                    break;
            }
        }
    }

    // AoWWinMan.pas:453 reads the inherited modal field on menu siblings too.
    public void HideTransientWindows()
    {
        for (int i = 0; i < Manager.Children.Count; i++)
        {
            if (Manager.Children[i] is WindowControl window &&
                window != UI.MapWindow.MainWindow && window.Modal && window.Visible)
            {
                window.Visible = false;
            }
        }
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
            for (int i = 0, last = controller.UI.General.Manager.Children.Count - 1; i <= last; i++)
            {
                if (controller.UI.General.Manager.Children[i] != controller.UI.TaskBar.TaskWindow)
                {
                    controller.UI.General.Manager.Children[i].Visible = false;
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
        _ = AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        _ = SetCurrentDir(controller.UI.Engine.StartupDirectory);
        HideGameWindows(controller.UI.General);
        controller.UI.TaskBar.TaskWindow.Visible = false;
        controller.UI.TitleWindow.MainWindow.Visible = false;
        controller.UI.TitleScene.ResetScene();
        controller.UI.TitleScene.ShowScene(true);
    }

    internal static void SaveWindowLayout(General controller)
    {
        Window playerInfo = controller.UI.PlayerInfoWindow.MainWindow;
        Rect rectangle = new(playerInfo.Alignment.RightOffset, playerInfo.Alignment.BottomOffset, playerInfo.WinWidth, playerInfo.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(0, rectangle);

        Window controls = controller.UI.ControlWindow.MainWindow;
        Rect rectangle2 = new(controls.Alignment.RightOffset, controls.Alignment.TopOffset, controls.WinWidth, controls.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(1, rectangle2);

        Window unitWindow = controller.UI.UnitWindow.MainWindow;
        if (unitWindow.Alignment.AlignWidth != AlignWidth.Center)
        {
            Rect rectangle3 = new(unitWindow.Alignment.LeftOffset, unitWindow.Alignment.TopOffset, unitWindow.WinWidth, unitWindow.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(2, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(2, rectangle3);
        }

        Window realm = controller.UI.RealmWindow.Window;
        if (realm.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(realm.Alignment.LeftOffset, realm.Alignment.TopOffset, realm.WinWidth, realm.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(5, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(5, rectangle3);
        }

        Window magic = controller.UI.MagicWindow.Window;
        if (magic.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(magic.Alignment.LeftOffset, magic.Alignment.TopOffset, magic.WinWidth, magic.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(13, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(13, rectangle3);
        }

        Window scanner = controller.UI.ScannerWindow.MainWindow;
        Rect rectangle4 = new(scanner.Alignment.RightOffset, scanner.Alignment.TopOffset, scanner.WinWidth, scanner.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(6, rectangle4);

        Window info = controller.UI.InfoWindow.MainWindow;
        Rect rectangle5 = new(info.Alignment.RightOffset, info.Alignment.TopOffset, info.WinWidth, info.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(7, rectangle5);

        Window chat = controller.UI.ChatWindow.MainWindow;
        if (chat.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(chat.Alignment.LeftOffset, chat.Alignment.BottomOffset, chat.WinWidth, chat.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(8, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(8, rectangle3);
        }

        Window map = controller.UI.MapWindow.MainWindow;
        if (map.Alignment.AlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(map.Alignment.LeftOffset, map.Alignment.TopOffset, map.WinWidth, map.WinHeight);
            controller.UI.Engine.Registry.SetRectangle(9, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(9, rectangle3);
        }

        Window tacticalBattleUnit = controller.UI.CombatUnitInfo.MainWindow;
        Rect rectangle6 = new(tacticalBattleUnit.Alignment.RightOffset, tacticalBattleUnit.Alignment.TopOffset, tacticalBattleUnit.WinWidth, tacticalBattleUnit.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(12, rectangle6);

        Window tacticalBattleScanner = controller.UI.CombatScanner.CombatScannerWindow;
        Rect rectangle7 = new(tacticalBattleScanner.Alignment.RightOffset, tacticalBattleScanner.Alignment.BottomOffset, tacticalBattleScanner.WinWidth, tacticalBattleScanner.WinHeight);
        controller.UI.Engine.Registry.SetRectangle(10, rectangle7);

        if (controller.UI.MapWindow.MapAlignWidth != AlignWidth.Both)
        {
            Rect rectangle3 = new(controller.UI.MapWindow.MapRectangle.Left, controller.UI.MapWindow.MapRectangle.Top, controller.UI.MapWindow.MapRectangle.Right, controller.UI.MapWindow.MapRectangle.Bottom);
            controller.UI.Engine.Registry.SetRectangle(11, rectangle3);
        }
        else
        {
            Rect rectangle3 = new(0, 0, 0, 0);
            controller.UI.Engine.Registry.SetRectangle(11, rectangle3);
        }
    }

    internal static bool HasModalWindow(General controller)
    {
        for (int i = controller.Manager.Children.Count - 1; i >= 0; i--)
        {
            if (controller.Manager.Children[i] is WindowControl window &&
                window != controller.UI.MapWindow.MainWindow && window.Modal && window.Visible)
            {
                return true;
            }
        }

        return false;
    }

    internal static void LoadGame(General controller, string name)
    {
        if (StrategicMap.Current != null)
        {
            controller.UI.Engine.CloseHsm(null);
        }

        string loading = controller.UI.Engine.TranslateResourceString(LayerText.LoadingText);
        string fileName = ExtractFileName(name);
        controller.UI.ProgressDialog.TitleLabel.Text = $"{loading} {fileName}...";
        controller.UI.ProgressDialog.StatusLabel.Text = "";
        controller.UI.ProgressDialog.ProgressBar.Position = 0;
        controller.UI.ProgressDialog.ProgressBar.Max = 100;
        controller.UI.ProgressDialog.Window.Visible = true;
        if (!controller.UI.TitleScene.Window.Visible)
        {
            controller.UI.General.Manager.OverlayPriority = 5000;
        }

        _ = AudioHost.SongPlayer.Stop(false);
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
        if (StrategicMap.Current.GameSongs.Songs.Count != 0)
        {
            AudioHost.SongPlayer.Songs.AddStrings(StrategicMap.Current.GameSongs.Songs);
        }
        else if (StrategicMap.Current.StartupSongs.Songs.Count != 0)
        {
            for (int i = 0; i < StrategicMap.Current.StartupSongs.Songs.Count; i++)
            {
                if (CompareText(StrategicMap.Current.StartupSongs.SongNames[i], controller.UI.Engine.TranslateResourceString(LayerText.FileNotFoundText)) != 0)
                {
                    _ = AudioHost.SongPlayer.Songs.Add(StrategicMap.Current.StartupSongs.Songs[i]);
                }
            }
        }
        else
        {
            _ = AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
        }

        _ = AudioHost.SongPlayer.Play(0);
        AudioPlayback.SetMusicVolume(controller.UI.SoundDialog.MusicVolumeSlider.Position);
        DelphiStringList menu = controller.UI.PopupWindows.OverviewsMenu.Items;
        menu.Clear();
        _ = menu.Add(controller.UI.Engine.TranslateResourceString(LayerText.BackgroundText));
        if (controller.UI.Engine.Campaign != null)
        {
            _ = menu.Add(controller.UI.Engine.TranslateResourceString(LayerText.CampaignText));
        }

        controller.UI.TitleScene.Window.Visible = false;
        controller.UI.TitleWindow.MainWindow.Visible = false;
        controller.UI.TaskBar.TaskWindow.Visible = true;
        controller.UI.MapWindow.MainWindow.Visible = true;
        controller.UI.ScannerWindow.MainWindow.Visible = true;
        controller.UI.MapWindow.MapViewer.HexSpriteMap = StrategicMap.Current;
        Scanner scanner = controller.UI.ScannerWindow.Scanner;
        scanner.SetMap(StrategicMap.Current);
        scanner.SetLevel(StrategicMap.Current.MapContainer.GetMapLevel(0));
        controller.UI.TaskBar.WorldTurnButton.ButtonChecked = false;
        controller.UI.TaskBar.ChatButton.Visible = controller.UI.SetupEvents.DirectPlay.Connected;
        controller.UI.General.Manager.Aligned = false;
        scanner.PreviousWidth = 0;
        scanner.Incremental = false;
        scanner.Sized = false;
        TabPanel tabs = controller.UI.ScannerWindow.ScannerTabs;
        tabs.Items.Clear();
        _ = tabs.Items.Add(controller.UI.Engine.TranslateResourceString(LayerText.SurfaceText));
        if (StrategicMap.Current.MapContainer.LevelCount > 1)
        {
            _ = tabs.Items.Add(controller.UI.Engine.TranslateResourceString(LayerText.CavernsText));
        }

        if (StrategicMap.Current.MapContainer.LevelCount > 2)
        {
            _ = tabs.Items.Add(controller.UI.Engine.TranslateResourceString(LayerText.DepthsText));
        }

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
            TurnButton turn = controller.UI.TaskBar.WorldTurnButton;
            if (StrategicMap.Current.SeatedPlayer != -1)
            {
                turn.LastPlayer = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).PlayerRace;
                if (turn.LastPlayer == -1)
                {
                    turn.LastPlayer = 12;
                }

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
