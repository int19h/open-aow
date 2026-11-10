// Translated from aowMapEvents.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// map control
public partial class MapEvents : DataModule
{
    public MapEvents(global::Aow.Rtl.Component owner) : base(owner)
    { }

    public AoWMapControl TheMap;
    // Renderers of the DoubleClick keys of the structures (Structure.Views.cs). ClassRegistry adds
    // the original renderers.
    public static readonly Catalog<StructureView, Action<MapEvents, Structure>> StructureScreens = new("map structure screen");
    // Renderers of the event log views (EventLog.Views.cs), called with the screen, the event, the
    // immediate mode and the done callback. The original renderers ignore the mode (the three that
    // take it name it Unused), it is there for a renderer that wants it. ClassRegistry adds the
    // original renderers.
    public static readonly Catalog<EventView, Action<MapEvents, EventLog, bool, ExecuteDoneEvent>> EventRenderers = new("map event view");

    // aowMapEvents.pas:93
    public void TheMapActivateAbility(ActivateAbilityInfo Request)
    {
        if (Request.FAbilityID == 52)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SpellBook.ShowCasting(Request);
        }
        else
        {
            if (Request.FAbilityID == 125)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.ConstructDlg.Execute(Request);
            }
            else
            {
                if (Request is UnitSelectorAAI)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitSelectionDlg.Execute((UnitSelectorAAI)Request);
                }
            }
        }
    }

    // aowMapEvents.pas:100
    public void TheMapArmyChanged(Army Army)
    {
        if (Army != null)
        {
            if (Army == Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList)
            {
                TheMapArmySelected(Army);
                if (Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
                }

                if (Aow.Game.Ui.Dialogs.DialogHosts.UnitBanner.BannerPopup.Visible)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.UnitBanner.BannerPopupShow();
                }
            }

            if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Army.FPlayer && Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible && Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected == 3)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
            }
        }
        else
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible && Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected == 3)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:117
    public void TheMapArmyDeselected()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.FAbilities.Clear();
        Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList = null;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.MovesLbl.Text = "";
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.MoveIcon.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp1Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp1Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp2Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp2Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp3Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp3Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp4Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp4Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp5Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp5Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp6Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp6Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp7Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp7Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp8Fr.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.Emp8Chk.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.ControlWindow.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.ConButton.Visible = false;
        if (Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = false;
        }
    }

    // aowMapEvents.pas:144
    public void TheMapArmyDestroyed(Army Army)
    {
        if (Army != null && Army == Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList)
        {
            TheMapArmyDeselected();
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible && Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected == 3)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // aowMapEvents.pas:150
    public void TheMapArmySelected(Army Army)
    {
        int Count;
        Count = 0;
        for (int I = 0; I <= 7; I++)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[I] = -1;
        }

        for (int I = 0, last = Army.GetCount() - 1; I <= last; I++)
        {
            if (!Army.GetUnitConcealedForPlayer(I, Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer))
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnits[Count] = I;
                Count += 1;
            }
        }

        Aow.Game.Ui.Dialogs.DialogHosts.General.FUnitList = Army;
        Aow.Game.Ui.Dialogs.DialogHosts.ControlWin.UpdateArmy();
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Army.FPlayer)
        {
            Aow.Game.World.Map.AoWHSMap.Current.FTutorialControl.ShowMessage(63, true, true);
        }
    }

    // aowMapEvents.pas:167
    public void TheMapDayChanged(int Day)
    {
        if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Activated))
        {
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FLastPlayer = -1;
        }

        var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.MapEvents.DayXRStr), [Day]);
        Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.Turn.Text = format;
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapGemsChanged(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer);
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapIncomeChanged(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer);
        if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // aowMapEvents.pas:187
    public void TheMapEndTurn(sbyte Player, bool Cancelled)
    {
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Player && Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer).FGameOverStatus == GameOverStatus.None)
        {
            if (Cancelled)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FDrawn = false;
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FButtonChecked = false;
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.General.HideTransientWindows();
            }
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.PInd.FDrawn = false;
    }

    // aowMapEvents.pas:198
    public void TheMapExecuteEventLog(EventLog Event, bool Mode, ExecuteDoneEvent Done)
    {
        if (!Mode)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.HideTransientWindows();
        }

        // None has no renderer: nothing shows and the event is not reported done.
        if (EventRenderers.TryGet(Event.View(), out var render))
        {
            render(this, Event, Mode, Done);
        }
    }

    // aowMapEvents.pas:287
    public void TheMapGameOver()
    {
    }

    // aowMapEvents.pas:290
    public void TheMapGemsChanged(sbyte Player)
    {
        bool Update;
        Update = false;
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Update = true;
        }
        else
        {
            if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap != null)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap.FSeatedPlayer == Player)
                {
                    Update = true;
                }
            }
        }

        if (Update)
        {
            if (Player == -1)
            {
                Update = false;
            }
            else
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                // { The original clears both income signs when the gold display is cleared. }
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGTLbl.Text = "";
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.Text = "";
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.Text = "";
            }
            else
            {
                var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGTLbl.Text = IntToStr(player.FGems);
                if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible && Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected == 0)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
                }
            }

            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TGTLbl.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGTLbl.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TGSign.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TMTSign.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.Text;
        }
    }

    // aowMapEvents.pas:320
    public void TheMapGlobalMagicChanged()
    {
        Aow.Game.Ui.Dialogs.MapEvents.GlobalMagicChanged(Aow.Game.Ui.Dialogs.DialogHosts.MagicWin);
    }

    // aowMapEvents.pas:323
    public void TheMapIncomeChanged(sbyte Player)
    {
        bool Update;
        int NetIncome;
        Update = false;
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Update = true;
        }
        else
        {
            if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap != null)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap.FSeatedPlayer == Player)
                {
                    Update = true;
                }
            }
        }

        if (Update)
        {
            if (Player == -1)
            {
                Update = false;
            }
            else
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGILbl.Text = "";
            }
            else
            {
                var upkeep = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).GetUpkeep();
                NetIncome = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FIncome - upkeep;
                if (NetIncome >= 0)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.Text = "+";
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.TextColor = 7864183;
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGILbl.TextColor = 7864183;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.Text = "-";
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.TextColor = 119;
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGILbl.TextColor = 119;
                }

                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGILbl.Text = IntToStr(Math.Abs(NetIncome));
                if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible && Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected == 0)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
                }
            }

            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TGILbl.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGILbl.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TGSign.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TGSign.TextColor = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGSign.TextColor;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TGILbl.TextColor = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WGILbl.TextColor;
        }
    }

    // aowMapEvents.pas:362
    public void TheMapMapChanged(ref MapChangedInfo Info)
    {
        if (Info.MapLevel == Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FLevel)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.DrawMap(Info.Bounds.Left - 6, Info.Bounds.Top - 6, Info.Bounds.Right - Info.Bounds.Left, Info.Bounds.Bottom - Info.Bounds.Top);
        }
    }

    // aowMapEvents.pas:370
    public void TheMapScannerIconsChanged(ref MapChangedInfo Info)
    {
        if (Info.MapLevel == Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FLevel)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetObDrawn(false);
        }
    }

    // aowMapEvents.pas:375
    public void TheMapPlayerMagicChanged(sbyte Player)
    {
        bool Update;
        int NetIncome;
        Aow.Game.Ui.Dialogs.MapEvents.MagicPlayerChanged(Aow.Game.Ui.Dialogs.DialogHosts.MagicWin, Player);
        Aow.Game.Ui.Dialogs.DialogHosts.SpellBook.PlayerChanged(Player);
        Update = false;
        if (Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Update = true;
        }
        else
        {
            if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap != null)
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FCombatMap.FSeatedPlayer == Player)
                {
                    Update = true;
                }
            }
        }

        if (Update)
        {
            if (Player == -1)
            {
                Update = false;
            }
            else
            {
                if (Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTLbl.Text = "";
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMILbl.Text = "";
            }
            else
            {
                var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTLbl.Text = IntToStr(player.FMagicControl.FMana);
                NetIncome = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.GetManaIncome() - Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.GetManaUpkeep();
                if (NetIncome >= 0)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.Text = "+";
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.TextColor = 7864183;
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMILbl.TextColor = 7864183;
                }
                else
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.Text = "-";
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.TextColor = 119;
                    Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMILbl.TextColor = 119;
                }

                Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMILbl.Text = IntToStr(Math.Abs(NetIncome));
            }

            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TMTLbl.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTLbl.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TMILbl.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMILbl.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TMTSign.Text = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.Text;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TMTSign.TextColor = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMTSign.TextColor;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TMILbl.TextColor = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.WMILbl.TextColor;
        }
    }

    // aowMapEvents.pas:1021
    public void TheMapLogbookChanged(sbyte Player)
    {
        if (Player == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdateEvents();
        }
    }

    // aowMapEvents.pas:1026
    public void TheMapPlayerTerminated(sbyte Player)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.UpdatePlayerIndicator();
        if (Aow.Game.World.Map.AoWHSMap.Current != null)
            if (Aow.Game.World.Map.AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network)
                if (Player == Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer)
                {
                    /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
                    throw new NotImplementedException();
                }
    }

    // aowMapEvents.pas:409
    public void TheMapSeatedPlayerChanged(sbyte Player)
    {
        if (Player == -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.HideWindows();
            Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.CurP.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.FWorldClockDelay = 0;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FCurPlayer = 12;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FLastPlayer = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FCurPlayer;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FAnimationFrame = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FCurPlayer * 7;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.ShowWindows();
            var aowTurnButton = Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton;
            if (Aow.Game.World.Map.AoWHSMap.Current.Status.Contains(HSMapStatusElement.Activated) && !aowTurnButton.FAnimating)
            {
                if (Player > -1)
                {
                    aowTurnButton.FCurPlayer = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerRace;
                }
                else
                {
                    aowTurnButton.FCurPlayer = -2;
                }

                if (aowTurnButton.FCurPlayer == -1)
                {
                    aowTurnButton.FCurPlayer = 12;
                }

                aowTurnButton.FLastPlayer = -1;
            }

            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.CurP.Visible = Aow.Game.World.Map.AoWHSMap.Current.FPlayMode == PlayMode.Turn;
            Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdatePlayer();
            if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
            }
        }

        TheMapGemsChanged(Player);
        TheMapIncomeChanged(Player);
        TheMapPlayerMagicChanged(Player);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.PInd.FDrawn = false;
        Aow.Game.Ui.Dialogs.DialogHosts.PInfoW.UpdateEvents();
    }

    // aowMapEvents.pas:440
    public void TheMapShowMessage(ref ShowMessageInfo Info)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TBMessEdit.TextColor = Info.Color;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TBMessEdit.Text = Info.Text;
    }

    // aowMapEvents.pas:446
    public void TheMapStructureChanged(Structure Structure)
    {
        Aow.Game.Ui.Dialogs.MapEvents.UpdateStructureInfo(Aow.Game.Ui.Dialogs.DialogHosts.IWindow, Structure);
        if (Structure != null)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.CityScreen.CityChanged(Structure);
            Aow.Game.Ui.Dialogs.DialogHosts.ProductionScreen.PlaceChanged(Structure);
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible && Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected < 3)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // aowMapEvents.pas:457
    public void TheMapStructureDeselected()
    {
        if (Aow.Game.Ui.Dialogs.DialogHosts.General.FSign != null)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSign.ChangeText(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer, Aow.Game.Ui.Dialogs.DialogHosts.IWindow.SignText.Items);
        }

        Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure = null;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FCity = null;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FProductionPlace = null;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FProductionControl = null;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FSign = null;
        Aow.Game.Ui.Dialogs.DialogHosts.IWindow.StructNameLbl.Text = "";
        Aow.Game.Ui.Dialogs.DialogHosts.IWindow.StructLine.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.StructButton.Visible = false;
    }

    // aowMapEvents.pas:467
    public void TheMapStructureDestroyed(Structure Structure)
    {
        if (Structure != null && Structure == Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure)
        {
            TheMapStructureDeselected();
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.Dlg.Visible)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected == 2 || Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.HeaderTab.Selected == 1)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.RealmWin.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:473
    public void TheMapStructureSelected(Structure Structure)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.FPreviousStructure = Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure = Structure;
        Aow.Game.Ui.Dialogs.DialogHosts.General.FProductionControl = null;
        Aow.Game.Ui.Dialogs.MapEvents.ShowStructureInfo(Aow.Game.Ui.Dialogs.DialogHosts.IWindow, Structure);
    }

    // aowMapEvents.pas:481
    public void TheMapItemsDeselected()
    {
        Aow.Game.Ui.Dialogs.MapEvents.HideItemSelection(Aow.Game.Ui.Dialogs.DialogHosts.IWindow);
    }

    // aowMapEvents.pas:484
    public void TheMapItemsSelected(ItemHS Items)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ShowItemSelection(Aow.Game.Ui.Dialogs.DialogHosts.IWindow, Items);
    }

    // aowMapEvents.pas:487
    public void TheMapTargetSelectionChange(TargetSelectionControl Control, TargetSelectionChange Change)
    {
        Aow.Game.Ui.Dialogs.MapEvents.ShowTargetSelection(Aow.Game.Ui.Dialogs.DialogHosts.TargetSelectionDlg, Control, Change);
    }

    // aowMapEvents.pas:490
    public void TheMapUnitChanged(AbstractUnit U)
    {
        if (U == Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit)
        {
            if (Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.ShowUnit();
            }

            if (Aow.Game.Ui.Dialogs.DialogHosts.UnitBanner.BannerPopup.Visible)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.UnitBanner.BannerPopupShow();
            }
        }

        Aow.Game.Ui.Dialogs.DialogHosts.SpellBook.UnitChanged(U);
    }

    // aowMapEvents.pas:500
    public void TheMapCastSpell(SpellCaster Caster)
    {
        if (Caster is UnitSpellCaster)
        {
            Aow.Game.Ui.Dialogs.MapEvents.ShowUnitSpellCaster(Aow.Game.Ui.Dialogs.DialogHosts.CastUnitSpellDlg, (UnitSpellCaster)Caster);
        }
        else
        {
            if (Caster is DisjunctionSpellCaster)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.DisjunctionDlg.Execute((DisjunctionSpellCaster)Caster);
            }
            else
            {
                if (Caster is CosmeticSurgerySpellCaster)
                {
                    Aow.Game.Ui.Dialogs.DialogHosts.DeformFaceDlg.Execute((CosmeticSurgerySpellCaster)Caster);
                }
            }
        }
    }

    // aowMapEvents.pas:507
    public void TheMapHSChanged(HexagonSprite HS)
    {
        if (HS is ItemHS)
        {
            Aow.Game.Ui.Dialogs.MapEvents.UpdateItemSelection(Aow.Game.Ui.Dialogs.DialogHosts.IWindow, (ItemHS)HS);
        }
    }

    // aowMapEvents.pas:512
    public void TheMapRelationsChanged()
    {
        Aow.Game.Ui.Dialogs.MapEvents.RefreshRelations(Aow.Game.Ui.Dialogs.DialogHosts.RelWindow);
    }

    // aowMapEvents.pas:515
    public void TheMapDiplomaticChange()
    {
        Aow.Game.Ui.Dialogs.MapEvents.RefreshRelations(Aow.Game.Ui.Dialogs.DialogHosts.RelWindow);
    }

    // aowMapEvents.pas:518
    public void TheMapStructureDblClick(Structure Structure)
    {
        if (Structure == Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure && Structure != null)
        {
            if (StructureScreens.TryGet(Structure.DoubleClick(Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer), out var open))
            {
                open(this, Structure);
            }
        }
    }

    // aowMapEvents.pas:176. The offline transport has no connection to release.
    public void TheMapDeactivateHSM()
    {
        try
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.SaveLayout();
            Aow.Game.Ui.Dialogs.DialogHosts.General.HideWindows();
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.HSMap = null;
            if (Aow.Game.Ui.Dialogs.DialogHosts.SetupEvents.FLobbyGame)
                GameHost.MainForm.CloseRequested();
        }
        catch
        {
            ShowMessage("Error during close");
        }
    }

    private static readonly string DayXRStr = "Day %d";
    private static readonly string PickupRStr = "Pickup";
    internal static readonly string UnlimitedRStr = "Unlimited";
    private static readonly string UpkeepRStr = "Upkeep";
    private static readonly string ItemBagRStr = "Item Bag";

    private static void ShowStructureInfo(global::Aow.Game.Ui.Dialogs.IWindow Dialog, global::Aow.Game.World.Structures.Structure Structure)
    {
        global::Aow.Game.Ui.Dialogs.IWindow.StructurePanels[Structure.InfoPanel(global::Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer)].Invoke(Dialog, Structure);
        if (!global::Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.StructButton.Visible)
        {
            global::Aow.Game.Ui.Dialogs.DialogHosts.IWindow.InfoWin.Visible = true;
        }
        global::Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.StructButton.Visible = true;
    }

    private static void UpdateStructureInfo(global::Aow.Game.Ui.Dialogs.IWindow Dialog, global::Aow.Game.World.Structures.Structure Structure)
    {
        if (!Dialog.InfoWin.Visible)
        {
            return;
        }
        if (Structure == null)
        {
            Structure = global::Aow.Game.Ui.Dialogs.DialogHosts.General.FStructure;
            if (Structure != null && !Structure.Selected)
            {
                Structure = null;
            }
        }
        if (Structure != null && Structure.Selected)
        {
            ShowStructureInfo(Dialog, Structure);
        }
    }

    private static void ShowItemSelection(global::Aow.Game.Ui.Dialogs.IWindow Dialog, global::Aow.Game.Draw.ItemHS Items)
    {
        ResetStructureInfo(Dialog);
        string text = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ItemBagRStr);
        Dialog.Title.Text = text;
        Dialog.FItemBag = Items;
        Dialog.ItemPnl.Visible = true;
        Dialog.InfoWin.Visible = true;
        Dialog.StructNameLbl.Text = "";
        global::Aow.Game.Draw.Images.LibraryImage imageTop = global::Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.MiscArt[35];
        Dialog.StructLine.ImageTop = imageTop;
        global::Aow.Game.Draw.Images.LibraryImage imageMiddle = global::Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.MiscArt[36];
        Dialog.StructLine.ImageMiddle = imageMiddle;
        global::Aow.Game.Draw.Images.LibraryImage imageBottom = global::Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.MiscArt[37];
        Dialog.StructLine.ImageBottom = imageBottom;
        global::Aow.Game.Draw.ArmyHS armyHS = ((global::Aow.Game.World.Map.AoWMapField)Items.Owner).GetArmyHS();
        if (armyHS != null && armyHS.FArmy.FPlayer == global::Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer)
        {
            Dialog.ItemBagMemo.Visible = false;
            global::Aow.Game.Magic.Abilities.Hero tHero = armyHS.FArmy.Hero(255, global::Aow.Game.Magic.Abilities.RosterTables.AllHeroTypes);
            Dialog.MainBtn.Visible = tHero != null;
            string text2 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(PickupRStr);
            Dialog.MainBtn.Text = text2;
            Dialog.ItembagLB.Visible = true;
            ((StringCollection)Dialog.ItembagLB.Items).Clear();
            Dialog.ItembagSB.Visible = true;
            int i = 0;
            for (int num = Items.FItems.Count - 1; i <= num; i++)
            {
                ((StringCollection)Dialog.ItembagLB.Items).Add("");
            }
            Dialog.ItemsInBagLbl.Visible = true;
        }
        else
        {
            Dialog.ItemBagMemo.Visible = true;
            Dialog.ItembagLB.Visible = false;
            Dialog.ItemsInBagLbl.Visible = false;
            Dialog.ItembagSB.Visible = false;
            Dialog.MainBtn.Visible = false;
        }
    }

    private static void UpdateItemSelection(global::Aow.Game.Ui.Dialogs.IWindow Dialog, global::Aow.Game.Draw.ItemHS Items)
    {
        if (Dialog.InfoWin.Visible && Items == Dialog.FItemBag)
        {
            ShowItemSelection(Dialog, Dialog.FItemBag);
        }
    }

    internal static void ResetStructureInfo(global::Aow.Game.Ui.Dialogs.IWindow Dialog)
    {
        Dialog.StructLine.Visible = true;
        Dialog.StructLine.FSized = false;
        Dialog.InfoWin.FAligned = false;
        Dialog.CityPanel.Visible = false;
        Dialog.CityBorderPnl.Visible = false;
        Dialog.ProductionPlacePnl.Visible = false;
        Dialog.SignPnl.Visible = false;
        Dialog.NotYoursPnl.Visible = false;
        Dialog.SingleButPnl.Visible = false;
        Dialog.MainBtn.Visible = true;
        Dialog.RazeBtn.Visible = true;
        Dialog.ExplorationSitePnl.Visible = false;
        Dialog.AltarPnl.Visible = false;
        Dialog.IncomePnl.Visible = false;
        Dialog.ItemPnl.Visible = false;
        Dialog.ConstructionPnl.Visible = false;
        Dialog.ProdOrb.Visible = false;
        Dialog.ProdOrbTime.Visible = false;
        Dialog.ProdOrbTurnsLbl.Visible = false;
        Dialog.RazePnl.Visible = false;
        Dialog.MainBtn.Visible = false;
        ((StringCollection)Dialog.SBmemo.Items).Clear();
        Dialog.PopulationRaceLbl.Visible = false;
        Dialog.PopulationLbl.Visible = false;
        string text = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.StructureRStr);
        Dialog.Title.Text = text;
    }

    private static void ShowTargetSelection(global::Aow.Game.Ui.Dialogs.TargetSelectionDlg Dialog, global::Aow.Game.Combat.TargetSelectionControl Control, global::Aow.Game.Combat.TargetSelectionChange Change)
    {
        switch (Change)
        {
            case global::Aow.Game.Combat.TargetSelectionChange.Activated:
                {
                    Dialog.HeaderLbl.Text = Control.FTitle;
                    Dialog.MainBtn.Text = Control.FCancelText;
                    ((Persistent)Dialog.DefaultMemo.Items).Assign((Persistent)(object)Control.FText);
                    Dialog.Dlg.Visible = true;
                    if (!Control.FTargetMode.Contains(global::Aow.Game.Combat.TargetSelectionModeElement.ValidateDistance))
                    {
                        string text = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UnlimitedRStr);
                        Dialog.TargetLbl.Text = RtlUnit.LowerCase(text);
                    }
                    else
                    {
                        Dialog.TargetLbl.Text = RtlUnit.IntToStr(Control.FRange);
                    }
                    Dialog.BorderImage.ImageTop = Control.FSourceImage;
                    Dialog.BorderImage.ImageMiddle = Control.FValidTargetImage;
                    Dialog.BorderImage.ImageBottom = Control.FInvalidTargetImage;
                    global::Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 49;
                    global::Aow.Game.Ui.Windows.AOWWindow mapWindow = global::Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow;
                    mapWindow.FModal = true;
                    mapWindow.Priority = 500;
                    global::Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Priority = Dialog.Dlg.Priority;
                    break;
                }
            case global::Aow.Game.Combat.TargetSelectionChange.Deactivated:
                Dialog.Dlg.Visible = false;
                global::Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Priority = 0;
                global::Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.FModal = false;
                global::Aow.Game.Ui.Dialogs.DialogHosts.SWindow.ScannerWindow.Priority = 10;
                global::Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
                break;
        }
    }

    private static void ShowUnitSpellCaster(global::Aow.Game.Ui.Dialogs.CastUnitSpellDlg Dialog, global::Aow.Game.Units.UnitSpellCaster Caster)
    {
        if (Dialog.FCaster != null)
        {
            Dialog.FCaster.Release();
        }
        Dialog.FCaster = Caster;
        Dialog.FCaster.AddRef();
        global::Aow.Game.Magic.Abilities.Hero tHero = global::Aow.Game.World.Map.AoWHSMap.Current.FUnitControl.FindUnit<global::Aow.Game.Magic.Abilities.Hero>(Dialog.FCaster.FCasterSourceID);
        if (tHero != null)
        {
            string name = tHero.GetName();
            Dialog.CasterNameLbl.Text = name;
            sbyte castingPoints = tHero.GetCastingPoints();
            string text = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.CastingPointsRStr);
            sbyte castingPointsMax = tHero.GetCastingPointsMax();
            Dialog.CastingPointsLbl.Text = string.Concat(new string[5]
            {
                text,
                ": ",
                RtlUnit.IntToStr((int)castingPoints),
                "/",
                RtlUnit.IntToStr((int)castingPointsMax)
            });
        }
        global::Aow.Game.Magic.Spells.Spell spell = global::Aow.Game.Engine.AoWHSSet.Current.FSpellControl.GetSpell(Dialog.FCaster.FCasterSpellID);
        Dialog.SpellNameLbl.Text = spell.FSpellName;
        int num = tHero.CastingMana(spell);
        string text2 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.CastingCostRStr);
        string text3 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.ManaRStr);
        Dialog.SpellManaLbl.Text = string.Concat(new string[5]
        {
            text2,
            ": ",
            RtlUnit.IntToStr(num),
            " ",
            text3
        });
        string text4 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(UpkeepRStr);
        string text5 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.ManaRStr);
        Dialog.SpellUpkeepLbl.Text = string.Concat(new string[5]
        {
            text4,
            ": ",
            RtlUnit.IntToStr(spell.FManaUpkeep),
            " ",
            text5
        });
        global::Aow.Game.Session.Player players = global::Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(tHero.FPlayer);
        string text6 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameText.ManaRStr);
        Dialog.CastingManaLbl.Text = text6 + ": " + RtlUnit.IntToStr(players.FMagicControl.FMana);
        Dialog.U1.Checked = false;
        Dialog.U2.Checked = false;
        Dialog.U3.Checked = false;
        Dialog.U4.Checked = false;
        Dialog.U5.Checked = false;
        Dialog.U6.Checked = false;
        Dialog.U7.Checked = false;
        Dialog.U8.Checked = false;
        Dialog.SelectionInfoLbl.Text = "";
        Dialog.Dlg.Visible = true;
    }

    private static void RefreshRelations(global::Aow.Game.Ui.Dialogs.RelWindow Dialog)
    {
        if (!Dialog.RelWin.Visible)
        {
            return;
        }
        Dialog.FillPlayers();
        Dialog.RelPnl.FAligned = false;
        Dialog.RelWin.FDrawn = false;
        if (Dialog.FSelectedPlayer != -1)
        {
            Dialog.HighlightPlayer(Dialog.FSelectedPlayer);
        }
        if (Dialog.FInfoPlayer != -1)
        {
            if (Dialog.RNewActionPnl.Visible)
            {
                Dialog.ShowPlayerActions(Dialog.FInfoPlayer);
            }
            else
            {
                Dialog.ShowPlayerInformation(Dialog.FInfoPlayer);
            }
        }
        else if (Dialog.FSelectedRace != -1)
        {
            Dialog.HighlightRace(Dialog.FSelectedRace);
        }
        if (Dialog.FInfoRace != -1)
        {
            Dialog.ShowRaceInformation((byte)Dialog.FInfoRace);
        }
    }

    private static void HideItemSelection(global::Aow.Game.Ui.Dialogs.IWindow Window)
    {
        Window.InfoWin.Visible = false;
        Window.FItemBag = null;
    }

    private static void MagicPlayerChanged(global::Aow.Game.Ui.Dialogs.MagicWin Window, sbyte Player)
    {
        if (Window.Dlg.Visible && Player == global::Aow.Game.World.Map.AoWHSMap.Current.FSeatedPlayer)
        {
            global::Aow.Game.Ui.Dialogs.MagicWin.RefreshMagicWindow(Window);
        }
    }

    private static void GlobalMagicChanged(global::Aow.Game.Ui.Dialogs.MagicWin Window)
    {
        if (Window.Dlg.Visible)
        {
            global::Aow.Game.Ui.Dialogs.MagicWin.RefreshGlobalMagic(Window);
        }
    }
}
