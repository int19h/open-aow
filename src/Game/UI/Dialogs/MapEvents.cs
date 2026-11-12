// Translated from aowMapEvents.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Session;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// map control
public partial class MapEvents : DataModule
{
    public MapEvents(Component owner) : base(owner)
    { }

    public MapControl TheMap;

    // Renderers of the DoubleClick keys of the structures (Structure.Views.cs). ClassRegistry adds
    // the original renderers.
    public static Catalog<StructureView, Action<MapEvents, Structure>> StructureScreens { get; } = new("map structure screen");
    // Renderers of the event log views (EventLog.Views.cs), called with the screen, the event, the
    // immediate mode and the done callback. The original renderers ignore the mode (the three that
    // take it name it Unused), it is there for a renderer that wants it. ClassRegistry adds the
    // original renderers.
    public static Catalog<EventView, Action<MapEvents, EventLog, bool, ExecuteDoneEvent>> EventRenderers { get; } = new("map event view");

    // aowMapEvents.pas:93
    public void TheMapActivateAbility(AbilityActivation request)
    {
        if (request.AbilityId == 52)
        {
            DialogHosts.SpellBook.ShowCasting(request);
        }
        else
        {
            if (request.AbilityId == 125)
            {
                DialogHosts.ConstructDialog.Execute(request);
            }
            else
            {
                if (request is UnitSelectionActivation)
                {
                    DialogHosts.UnitSelectionDialog.Execute((UnitSelectionActivation)request);
                }
            }
        }
    }

    // aowMapEvents.pas:100
    public void TheMapArmyChanged(Army army)
    {
        if (army != null)
        {
            if (army == DialogHosts.General.UnitList)
            {
                TheMapArmySelected(army);
                if (DialogHosts.UnitWindow.UnitWin.Visible)
                {
                    DialogHosts.UnitWindow.ShowUnit();
                }

                if (DialogHosts.UnitBanner.BannerPopup.Visible)
                {
                    DialogHosts.UnitBanner.BannerPopupShow();
                }
            }

            if (AoWHSMap.Current.SeatedPlayer == army.Player && DialogHosts.RealmWindow.Dlg.Visible && DialogHosts.RealmWindow.HeaderTab.Selected == 3)
            {
                DialogHosts.RealmWindow.UpdateOverview();
            }
        }
        else
        {
            if (DialogHosts.RealmWindow.Dlg.Visible && DialogHosts.RealmWindow.HeaderTab.Selected == 3)
            {
                DialogHosts.RealmWindow.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:117
    public void TheMapArmyDeselected()
    {
        DialogHosts.General.Abilities.Clear();
        DialogHosts.General.UnitList = null;
        DialogHosts.ControlWindow.MovesLbl.Text = "";
        DialogHosts.ControlWindow.MoveIcon.Visible = false;
        DialogHosts.ControlWindow.Emp1Fr.Visible = true;
        DialogHosts.ControlWindow.Emp1Chk.Visible = true;
        DialogHosts.ControlWindow.Emp2Fr.Visible = true;
        DialogHosts.ControlWindow.Emp2Chk.Visible = true;
        DialogHosts.ControlWindow.Emp3Fr.Visible = true;
        DialogHosts.ControlWindow.Emp3Chk.Visible = true;
        DialogHosts.ControlWindow.Emp4Fr.Visible = true;
        DialogHosts.ControlWindow.Emp4Chk.Visible = true;
        DialogHosts.ControlWindow.Emp5Fr.Visible = true;
        DialogHosts.ControlWindow.Emp5Chk.Visible = true;
        DialogHosts.ControlWindow.Emp6Fr.Visible = true;
        DialogHosts.ControlWindow.Emp6Chk.Visible = true;
        DialogHosts.ControlWindow.Emp7Fr.Visible = true;
        DialogHosts.ControlWindow.Emp7Chk.Visible = true;
        DialogHosts.ControlWindow.Emp8Fr.Visible = true;
        DialogHosts.ControlWindow.Emp8Chk.Visible = true;
        DialogHosts.ControlWindow.MainWindow.Visible = false;
        DialogHosts.TaskBar.ConButton.Visible = false;
        if (DialogHosts.UnitWindow.UnitWin.Visible)
        {
            DialogHosts.UnitWindow.UnitWin.Visible = false;
        }
    }

    // aowMapEvents.pas:144
    public void TheMapArmyDestroyed(Army army)
    {
        if (army != null && army == DialogHosts.General.UnitList)
        {
            TheMapArmyDeselected();
        }

        if (DialogHosts.RealmWindow.Dlg.Visible && DialogHosts.RealmWindow.HeaderTab.Selected == 3)
        {
            DialogHosts.RealmWindow.UpdateOverview();
        }
    }

    // aowMapEvents.pas:150
    public void TheMapArmySelected(Army army)
    {
        int count;
        count = 0;
        for (int I = 0; I <= 7; I++)
        {
            DialogHosts.General.SelectedUnits[I] = -1;
        }

        for (int I = 0, last = army.GetCount() - 1; I <= last; I++)
        {
            if (!army.GetUnitConcealedForPlayer(I, AoWHSMap.Current.SeatedPlayer))
            {
                DialogHosts.General.SelectedUnits[count] = I;
                count += 1;
            }
        }

        DialogHosts.General.UnitList = army;
        DialogHosts.ControlWindow.UpdateArmy();
        if (AoWHSMap.Current.SeatedPlayer == army.Player)
        {
            AoWHSMap.Current.TutorialControl.ShowMessage(63, true, true);
        }
    }

    // aowMapEvents.pas:167
    public void TheMapDayChanged(int day)
    {
        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            DialogHosts.TaskBar.TurnButton.LastPlayer = -1;
        }

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_dayFormat), [day]);
        DialogHosts.PlayerInfoWindow.Turn.Text = format;
        DialogHosts.MapEvents.TheMapGemsChanged(AoWHSMap.Current.SeatedPlayer);
        DialogHosts.MapEvents.TheMapIncomeChanged(AoWHSMap.Current.SeatedPlayer);
        if (DialogHosts.RealmWindow.Dlg.Visible)
        {
            DialogHosts.RealmWindow.UpdateOverview();
        }
    }

    // aowMapEvents.pas:187
    public void TheMapEndTurn(sbyte player, bool cancelled)
    {
        if (AoWHSMap.Current.SeatedPlayer == player && AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None)
        {
            if (cancelled)
            {
                DialogHosts.TaskBar.TurnButton.Drawn = false;
                DialogHosts.TaskBar.TurnButton.ButtonChecked = false;
            }
            else
            {
                DialogHosts.General.HideTransientWindows();
            }
        }

        DialogHosts.TaskBar.PInd.Drawn = false;
    }

    // aowMapEvents.pas:198
    public void TheMapExecuteEventLog(EventLog eventValue, bool mode, ExecuteDoneEvent done)
    {
        if (!mode)
        {
            DialogHosts.General.HideTransientWindows();
        }

        // None has no renderer: nothing shows and the event is not reported done.
        if (EventRenderers.TryGet(eventValue.View(), out var render))
        {
            render(this, eventValue, mode, done);
        }
    }

    // aowMapEvents.pas:287
    public void TheMapGameOver()
    {
    }

    // aowMapEvents.pas:290
    public void TheMapGemsChanged(sbyte player)
    {
        bool update;
        update = false;
        if (AoWHSMap.Current.SeatedPlayer == player)
        {
            update = true;
        }
        else
        {
            if (AoWHSMap.Current.CombatMap != null)
            {
                if (AoWHSMap.Current.CombatMap.SeatedPlayer == player)
                {
                    update = true;
                }
            }
        }

        if (update)
        {
            if (player == -1)
            {
                update = false;
            }
            else
            {
                if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                {
                    update = false;
                }
            }

            if (!update)
            {
                // { The original clears both income signs when the gold display is cleared. }
                DialogHosts.TaskBar.WGTLbl.Text = "";
                DialogHosts.TaskBar.WGSign.Text = "";
                DialogHosts.TaskBar.WMTSign.Text = "";
            }
            else
            {
                var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
                DialogHosts.TaskBar.WGTLbl.Text = IntToStr(playerLocal.Gems);
                if (DialogHosts.RealmWindow.Dlg.Visible && DialogHosts.RealmWindow.HeaderTab.Selected == 0)
                {
                    DialogHosts.RealmWindow.UpdateOverview();
                }
            }

            DialogHosts.TaskBar.TGTLbl.Text = DialogHosts.TaskBar.WGTLbl.Text;
            DialogHosts.TaskBar.TGSign.Text = DialogHosts.TaskBar.WGSign.Text;
            DialogHosts.TaskBar.TMTSign.Text = DialogHosts.TaskBar.WMTSign.Text;
        }
    }

    // aowMapEvents.pas:320
    public void TheMapGlobalMagicChanged()
    {
        GlobalMagicChanged(DialogHosts.MagicWindow);
    }

    // aowMapEvents.pas:323
    public void TheMapIncomeChanged(sbyte player)
    {
        bool update;
        int netIncome;
        update = false;
        if (AoWHSMap.Current.SeatedPlayer == player)
        {
            update = true;
        }
        else
        {
            if (AoWHSMap.Current.CombatMap != null)
            {
                if (AoWHSMap.Current.CombatMap.SeatedPlayer == player)
                {
                    update = true;
                }
            }
        }

        if (update)
        {
            if (player == -1)
            {
                update = false;
            }
            else
            {
                if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                {
                    update = false;
                }
            }

            if (!update)
            {
                DialogHosts.TaskBar.WGILbl.Text = "";
            }
            else
            {
                var upkeep = AoWHSMap.Current.Players.GetPlayers(player).GetUpkeep();
                netIncome = AoWHSMap.Current.Players.GetPlayers(player).Income - upkeep;
                if (netIncome >= 0)
                {
                    DialogHosts.TaskBar.WGSign.Text = "+";
                    DialogHosts.TaskBar.WGSign.TextColor = 7864183;
                    DialogHosts.TaskBar.WGILbl.TextColor = 7864183;
                }
                else
                {
                    DialogHosts.TaskBar.WGSign.Text = "-";
                    DialogHosts.TaskBar.WGSign.TextColor = 119;
                    DialogHosts.TaskBar.WGILbl.TextColor = 119;
                }

                DialogHosts.TaskBar.WGILbl.Text = IntToStr(Math.Abs(netIncome));
                if (DialogHosts.RealmWindow.Dlg.Visible && DialogHosts.RealmWindow.HeaderTab.Selected == 0)
                {
                    DialogHosts.RealmWindow.UpdateOverview();
                }
            }

            DialogHosts.TaskBar.TGILbl.Text = DialogHosts.TaskBar.WGILbl.Text;
            DialogHosts.TaskBar.TGSign.Text = DialogHosts.TaskBar.WGSign.Text;
            DialogHosts.TaskBar.TGSign.TextColor = DialogHosts.TaskBar.WGSign.TextColor;
            DialogHosts.TaskBar.TGILbl.TextColor = DialogHosts.TaskBar.WGILbl.TextColor;
        }
    }

    // aowMapEvents.pas:362
    public void TheMapMapChanged(ref MapChangedInfo info)
    {
        if (info.MapLevel == DialogHosts.ScannerWindow.Scanner.Level)
        {
            DialogHosts.ScannerWindow.Scanner.DrawMap(info.Bounds.Left - 6, info.Bounds.Top - 6, info.Bounds.Right - info.Bounds.Left, info.Bounds.Bottom - info.Bounds.Top);
        }
    }

    // aowMapEvents.pas:370
    public void TheMapScannerIconsChanged(ref MapChangedInfo info)
    {
        if (info.MapLevel == DialogHosts.ScannerWindow.Scanner.Level)
        {
            DialogHosts.ScannerWindow.Scanner.SetObDrawn(false);
        }
    }

    // aowMapEvents.pas:375
    public void TheMapPlayerMagicChanged(sbyte player)
    {
        bool update;
        int netIncome;
        MagicPlayerChanged(DialogHosts.MagicWindow, player);
        DialogHosts.SpellBook.PlayerChanged(player);
        update = false;
        if (AoWHSMap.Current.SeatedPlayer == player)
        {
            update = true;
        }
        else
        {
            if (AoWHSMap.Current.CombatMap != null)
            {
                if (AoWHSMap.Current.CombatMap.SeatedPlayer == player)
                {
                    update = true;
                }
            }
        }

        if (update)
        {
            if (player == -1)
            {
                update = false;
            }
            else
            {
                if (AoWHSMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                {
                    update = false;
                }
            }

            if (!update)
            {
                DialogHosts.TaskBar.WMTLbl.Text = "";
                DialogHosts.TaskBar.WMILbl.Text = "";
            }
            else
            {
                var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
                DialogHosts.TaskBar.WMTLbl.Text = IntToStr(playerLocal.MagicControl.Mana);
                netIncome = AoWHSMap.Current.Players.GetPlayers(player).MagicControl.GetManaIncome() - AoWHSMap.Current.Players.GetPlayers(player).MagicControl.GetManaUpkeep();
                if (netIncome >= 0)
                {
                    DialogHosts.TaskBar.WMTSign.Text = "+";
                    DialogHosts.TaskBar.WMTSign.TextColor = 7864183;
                    DialogHosts.TaskBar.WMILbl.TextColor = 7864183;
                }
                else
                {
                    DialogHosts.TaskBar.WMTSign.Text = "-";
                    DialogHosts.TaskBar.WMTSign.TextColor = 119;
                    DialogHosts.TaskBar.WMILbl.TextColor = 119;
                }

                DialogHosts.TaskBar.WMILbl.Text = IntToStr(Math.Abs(netIncome));
            }

            DialogHosts.TaskBar.TMTLbl.Text = DialogHosts.TaskBar.WMTLbl.Text;
            DialogHosts.TaskBar.TMILbl.Text = DialogHosts.TaskBar.WMILbl.Text;
            DialogHosts.TaskBar.TMTSign.Text = DialogHosts.TaskBar.WMTSign.Text;
            DialogHosts.TaskBar.TMTSign.TextColor = DialogHosts.TaskBar.WMTSign.TextColor;
            DialogHosts.TaskBar.TMILbl.TextColor = DialogHosts.TaskBar.WMILbl.TextColor;
        }
    }

    // aowMapEvents.pas:1021
    public void TheMapLogbookChanged(sbyte player)
    {
        if (player == AoWHSMap.Current.SeatedPlayer)
        {
            DialogHosts.PlayerInfoWindow.UpdateEvents();
        }
    }

    // aowMapEvents.pas:1026
    public void TheMapPlayerTerminated(sbyte player)
    {
        DialogHosts.TaskBar.UpdatePlayerIndicator();
        if (AoWHSMap.Current != null)
            if (AoWHSMap.Current.ConnectionType == PlayerConnectionType.Network)
                if (player == AoWHSMap.Current.SeatedPlayer)
                {
                    /* TODO: Implement aowDPlayComp.DisConnect in the platform or editor service. */
                    throw new NotImplementedException();
                }
    }

    // aowMapEvents.pas:409
    public void TheMapSeatedPlayerChanged(sbyte player)
    {
        if (player == -1)
        {
            DialogHosts.General.HideWindows();
            DialogHosts.General.Manager.OverlayPriority = 0;
            DialogHosts.TaskBar.CurP.Visible = false;
            DialogHosts.TaskBar.WorldClockDelay = 0;
            DialogHosts.TaskBar.TurnButton.CurrentPlayer = 12;
            DialogHosts.TaskBar.TurnButton.LastPlayer = DialogHosts.TaskBar.TurnButton.CurrentPlayer;
            DialogHosts.TaskBar.TurnButton.AnimationFrame = DialogHosts.TaskBar.TurnButton.CurrentPlayer * 7;
        }
        else
        {
            DialogHosts.General.ShowWindows();
            var turnButton = DialogHosts.TaskBar.TurnButton;
            if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated) && !turnButton.Animating)
            {
                if (player > -1)
                {
                    turnButton.CurrentPlayer = AoWHSMap.Current.Players.GetPlayers(player).PlayerRace;
                }
                else
                {
                    turnButton.CurrentPlayer = -2;
                }

                if (turnButton.CurrentPlayer == -1)
                {
                    turnButton.CurrentPlayer = 12;
                }

                turnButton.LastPlayer = -1;
            }

            DialogHosts.TaskBar.CurP.Visible = AoWHSMap.Current.PlayMode == PlayMode.Turn;
            DialogHosts.PlayerInfoWindow.UpdatePlayer();
            if (DialogHosts.RealmWindow.Dlg.Visible)
            {
                DialogHosts.RealmWindow.UpdateOverview();
            }
        }

        TheMapGemsChanged(player);
        TheMapIncomeChanged(player);
        TheMapPlayerMagicChanged(player);
        DialogHosts.TaskBar.PInd.Drawn = false;
        DialogHosts.PlayerInfoWindow.UpdateEvents();
    }

    // aowMapEvents.pas:440
    public void TheMapShowMessage(ref ShowMessageInfo info)
    {
        DialogHosts.TaskBar.TBMessEdit.TextColor = info.Color;
        DialogHosts.TaskBar.TBMessEdit.Text = info.Text;
    }

    // aowMapEvents.pas:446
    public void TheMapStructureChanged(Structure structure)
    {
        UpdateStructureInfo(DialogHosts.InfoWindow, structure);
        if (structure != null)
        {
            DialogHosts.CityScreen.CityChanged(structure);
            DialogHosts.ProductionScreen.PlaceChanged(structure);
        }

        if (DialogHosts.RealmWindow.Dlg.Visible && DialogHosts.RealmWindow.HeaderTab.Selected < 3)
        {
            DialogHosts.RealmWindow.UpdateOverview();
        }
    }

    // aowMapEvents.pas:457
    public void TheMapStructureDeselected()
    {
        if (DialogHosts.General.Sign != null)
        {
            DialogHosts.General.Sign.ChangeText(AoWHSMap.Current.SeatedPlayer, DialogHosts.InfoWindow.SignText.Items);
        }

        DialogHosts.General.Structure = null;
        DialogHosts.General.City = null;
        DialogHosts.General.ProductionPlace = null;
        DialogHosts.General.ProductionControl = null;
        DialogHosts.General.Sign = null;
        DialogHosts.InfoWindow.StructNameLbl.Text = "";
        DialogHosts.InfoWindow.StructLine.Visible = false;
        DialogHosts.InfoWindow.InfoWin.Visible = false;
        DialogHosts.TaskBar.StructButton.Visible = false;
    }

    // aowMapEvents.pas:467
    public void TheMapStructureDestroyed(Structure structure)
    {
        if (structure != null && structure == DialogHosts.General.Structure)
        {
            TheMapStructureDeselected();
        }

        if (DialogHosts.RealmWindow.Dlg.Visible)
        {
            if (DialogHosts.RealmWindow.HeaderTab.Selected == 2 || DialogHosts.RealmWindow.HeaderTab.Selected == 1)
            {
                DialogHosts.RealmWindow.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:473
    public void TheMapStructureSelected(Structure structure)
    {
        DialogHosts.General.PreviousStructure = DialogHosts.General.Structure;
        DialogHosts.General.Structure = structure;
        DialogHosts.General.ProductionControl = null;
        ShowStructureInfo(DialogHosts.InfoWindow, structure);
    }

    // aowMapEvents.pas:481
    public void TheMapItemsDeselected()
    {
        HideItemSelection(DialogHosts.InfoWindow);
    }

    // aowMapEvents.pas:484
    public void TheMapItemsSelected(ItemHexSprite items)
    {
        ShowItemSelection(DialogHosts.InfoWindow, items);
    }

    // aowMapEvents.pas:487
    public void TheMapTargetSelectionChange(TargetSelectionControl control, TargetSelectionChange change)
    {
        ShowTargetSelection(DialogHosts.TargetSelectionDialog, control, change);
    }

    // aowMapEvents.pas:490
    public void TheMapUnitChanged(Troop U)
    {
        if (U == DialogHosts.General.SelectedUnit)
        {
            if (DialogHosts.UnitWindow.UnitWin.Visible)
            {
                DialogHosts.UnitWindow.ShowUnit();
            }

            if (DialogHosts.UnitBanner.BannerPopup.Visible)
            {
                DialogHosts.UnitBanner.BannerPopupShow();
            }
        }

        DialogHosts.SpellBook.UnitChanged(U);
    }

    // aowMapEvents.pas:500
    public void TheMapCastSpell(SpellCaster caster)
    {
        if (caster is UnitSpellCaster)
        {
            ShowUnitSpellCaster(DialogHosts.CastUnitSpellDialog, (UnitSpellCaster)caster);
        }
        else
        {
            if (caster is DisjunctionSpellCaster)
            {
                DialogHosts.DisjunctionDialog.Execute((DisjunctionSpellCaster)caster);
            }
            else
            {
                if (caster is CosmeticSurgerySpellCaster)
                {
                    DialogHosts.DeformFaceDialog.Execute((CosmeticSurgerySpellCaster)caster);
                }
            }
        }
    }

    // aowMapEvents.pas:507
    public void TheMapHSChanged(HexSprite hexSprite)
    {
        if (hexSprite is ItemHexSprite)
        {
            UpdateItemSelection(DialogHosts.InfoWindow, (ItemHexSprite)hexSprite);
        }
    }

    // aowMapEvents.pas:512
    public void TheMapRelationsChanged()
    {
        RefreshRelations(DialogHosts.RelationWindow);
    }

    // aowMapEvents.pas:515
    public void TheMapDiplomaticChange()
    {
        RefreshRelations(DialogHosts.RelationWindow);
    }

    // aowMapEvents.pas:518
    public void TheMapStructureDblClick(Structure structure)
    {
        if (structure == DialogHosts.General.Structure && structure != null)
        {
            if (StructureScreens.TryGet(structure.DoubleClick(AoWHSMap.Current.SeatedPlayer), out var open))
            {
                open(this, structure);
            }
        }
    }

    // aowMapEvents.pas:176. The offline transport has no connection to release.
    public void TheMapDeactivateHSM()
    {
        try
        {
            DialogHosts.General.SaveLayout();
            DialogHosts.General.HideWindows();
            DialogHosts.MapWindow.MapViewer.HexSpriteMap = null;
            if (DialogHosts.SetupEvents.LobbyGame)
                GameHost.MainForm.CloseRequested();
        }
        catch
        {
            ShowMessage("Error during close");
        }
    }

    private static readonly string s_dayFormat = "Day %d";
    private static readonly string s_pickupText = "Pickup";

    internal static string UnlimitedText { get; } = "Unlimited";

    private static readonly string s_upkeepText = "Upkeep";
    private static readonly string s_itemBagText = "Item Bag";

    private static void ShowStructureInfo(InfoWindow dialog, Structure structure)
    {
        InfoWindow.StructurePanels[structure.InfoPanel(AoWHSMap.Current.SeatedPlayer)].Invoke(dialog, structure);
        if (!DialogHosts.TaskBar.StructButton.Visible)
        {
            DialogHosts.InfoWindow.InfoWin.Visible = true;
        }
        DialogHosts.TaskBar.StructButton.Visible = true;
    }

    private static void UpdateStructureInfo(InfoWindow dialog, Structure structure)
    {
        if (!dialog.InfoWin.Visible)
        {
            return;
        }
        if (structure == null)
        {
            structure = DialogHosts.General.Structure;
            if (structure != null && !structure.Selected)
            {
                structure = null;
            }
        }
        if (structure != null && structure.Selected)
        {
            ShowStructureInfo(dialog, structure);
        }
    }

    private static void ShowItemSelection(InfoWindow dialog, ItemHexSprite items)
    {
        ResetStructureInfo(dialog);
        string text = AbilityRules.TranslateResourceString(s_itemBagText);
        dialog.Title.Text = text;
        dialog.ItemBag = items;
        dialog.ItemPnl.Visible = true;
        dialog.InfoWin.Visible = true;
        dialog.StructNameLbl.Text = "";
        LibraryImage imageTop = DialogHosts.UIGraphics.MiscArt[35];
        dialog.StructLine.ImageTop = imageTop;
        LibraryImage imageMiddle = DialogHosts.UIGraphics.MiscArt[36];
        dialog.StructLine.ImageMiddle = imageMiddle;
        LibraryImage imageBottom = DialogHosts.UIGraphics.MiscArt[37];
        dialog.StructLine.ImageBottom = imageBottom;
        ArmyHexSprite armyHexSprite = ((AoWMapField)items.Owner).GetArmyHexSprite();
        if (armyHexSprite != null && armyHexSprite.Army.Player == AoWHSMap.Current.SeatedPlayer)
        {
            dialog.ItemBagMemo.Visible = false;
            Hero hero = armyHexSprite.Army.Hero(255, RosterTables.AllHeroTypes);
            dialog.MainBtn.Visible = hero != null;
            string text2 = AbilityRules.TranslateResourceString(s_pickupText);
            dialog.MainBtn.Text = text2;
            dialog.ItembagLB.Visible = true;
            ((StringCollection)dialog.ItembagLB.Items).Clear();
            dialog.ItembagSB.Visible = true;
            int i = 0;
            for (int number = items.Items.Count - 1; i <= number; i++)
            {
                ((StringCollection)dialog.ItembagLB.Items).Add("");
            }
            dialog.ItemsInBagLbl.Visible = true;
        }
        else
        {
            dialog.ItemBagMemo.Visible = true;
            dialog.ItembagLB.Visible = false;
            dialog.ItemsInBagLbl.Visible = false;
            dialog.ItembagSB.Visible = false;
            dialog.MainBtn.Visible = false;
        }
    }

    private static void UpdateItemSelection(InfoWindow dialog, ItemHexSprite items)
    {
        if (dialog.InfoWin.Visible && items == dialog.ItemBag)
        {
            ShowItemSelection(dialog, dialog.ItemBag);
        }
    }

    internal static void ResetStructureInfo(InfoWindow dialog)
    {
        dialog.StructLine.Visible = true;
        dialog.StructLine.Sized = false;
        dialog.InfoWin.Aligned = false;
        dialog.CityPanel.Visible = false;
        dialog.CityBorderPnl.Visible = false;
        dialog.ProductionPlacePnl.Visible = false;
        dialog.SignPnl.Visible = false;
        dialog.NotYoursPnl.Visible = false;
        dialog.SingleButPnl.Visible = false;
        dialog.MainBtn.Visible = true;
        dialog.RazeBtn.Visible = true;
        dialog.ExplorationSitePnl.Visible = false;
        dialog.AltarPnl.Visible = false;
        dialog.IncomePnl.Visible = false;
        dialog.ItemPnl.Visible = false;
        dialog.ConstructionPnl.Visible = false;
        dialog.ProdOrb.Visible = false;
        dialog.ProdOrbTime.Visible = false;
        dialog.ProdOrbTurnsLbl.Visible = false;
        dialog.RazePnl.Visible = false;
        dialog.MainBtn.Visible = false;
        ((StringCollection)dialog.SBmemo.Items).Clear();
        dialog.PopulationRaceLbl.Visible = false;
        dialog.PopulationLbl.Visible = false;
        string text = AbilityRules.TranslateResourceString(GameText.StructureText);
        dialog.Title.Text = text;
    }

    private static void ShowTargetSelection(TargetSelectionDialog dialog, TargetSelectionControl control, TargetSelectionChange change)
    {
        switch (change)
        {
            case TargetSelectionChange.Activated:
                {
                    dialog.HeaderLbl.Text = control.Title;
                    dialog.MainBtn.Text = control.CancelText;
                    ((Persistent)dialog.DefaultMemo.Items).Assign((Persistent)(object)control.Text);
                    dialog.Dlg.Visible = true;
                    if (!control.TargetMode.Contains(TargetSelectionModeElement.ValidateDistance))
                    {
                        string text = AbilityRules.TranslateResourceString(UnlimitedText);
                        dialog.TargetLbl.Text = RtlUnit.LowerCase(text);
                    }
                    else
                    {
                        dialog.TargetLbl.Text = RtlUnit.IntToStr(control.Range);
                    }
                    dialog.BorderImage.ImageTop = control.SourceImage;
                    dialog.BorderImage.ImageMiddle = control.ValidTargetImage;
                    dialog.BorderImage.ImageBottom = control.InvalidTargetImage;
                    DialogHosts.General.Manager.OverlayPriority = 49;
                    Window mapWindow = DialogHosts.MapWindow.MainWindow;
                    mapWindow._modal = true;
                    mapWindow.Priority = 500;
                    DialogHosts.ScannerWindow.MainWindow.Priority = dialog.Dlg.Priority;
                    break;
                }
            case TargetSelectionChange.Deactivated:
                dialog.Dlg.Visible = false;
                DialogHosts.MapWindow.MainWindow.Priority = 0;
                DialogHosts.MapWindow.MainWindow._modal = false;
                DialogHosts.ScannerWindow.MainWindow.Priority = 10;
                DialogHosts.General.Manager.OverlayPriority = 0;
                break;
        }
    }

    private static void ShowUnitSpellCaster(CastUnitSpellDialog dialog, UnitSpellCaster caster)
    {
        if (dialog.Caster != null)
        {
            dialog.Caster.Release();
        }
        dialog.Caster = caster;
        dialog.Caster.AddRef();
        Hero hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(dialog.Caster.CasterSourceId);
        if (hero != null)
        {
            string name = hero.GetName();
            dialog.CasterNameLbl.Text = name;
            sbyte castingPoints = hero.GetCastingPoints();
            string text = AbilityRules.TranslateResourceString(GameText.CastingPointsText);
            sbyte castingPointsMax = hero.GetCastingPointsMax();
            dialog.CastingPointsLbl.Text = string.Concat(new string[5]
            {
                text,
                ": ",
                RtlUnit.IntToStr((int)castingPoints),
                "/",
                RtlUnit.IntToStr((int)castingPointsMax)
            });
        }
        Spell spell = AoWHSSet.Current.SpellControl.GetSpell(dialog.Caster.CasterSpellId);
        dialog.SpellNameLbl.Text = spell.SpellName;
        int number = hero.CastingMana(spell);
        string text2 = AbilityRules.TranslateResourceString(GameText.CastingCostText);
        string text3 = AbilityRules.TranslateResourceString(GameText.ManaText);
        dialog.SpellManaLbl.Text = string.Concat(new string[5]
        {
            text2,
            ": ",
            RtlUnit.IntToStr(number),
            " ",
            text3
        });
        string text4 = AbilityRules.TranslateResourceString(s_upkeepText);
        string text5 = AbilityRules.TranslateResourceString(GameText.ManaText);
        dialog.SpellUpkeepLbl.Text = string.Concat(new string[5]
        {
            text4,
            ": ",
            RtlUnit.IntToStr(spell.ManaUpkeep),
            " ",
            text5
        });
        Player players = AoWHSMap.Current.Players.GetPlayers(hero.Player);
        string text6 = AbilityRules.TranslateResourceString(GameText.ManaText);
        dialog.CastingManaLbl.Text = text6 + ": " + RtlUnit.IntToStr(players.MagicControl.Mana);
        dialog.U1.Checked = false;
        dialog.U2.Checked = false;
        dialog.U3.Checked = false;
        dialog.U4.Checked = false;
        dialog.U5.Checked = false;
        dialog.U6.Checked = false;
        dialog.U7.Checked = false;
        dialog.U8.Checked = false;
        dialog.SelectionInfoLbl.Text = "";
        dialog.Dlg.Visible = true;
    }

    private static void RefreshRelations(RelationWindow dialog)
    {
        if (!dialog.RelWin.Visible)
        {
            return;
        }
        dialog.FillPlayers();
        dialog.RelPnl.Aligned = false;
        dialog.RelWin.Drawn = false;
        if (dialog.SelectedPlayer != -1)
        {
            dialog.HighlightPlayer(dialog.SelectedPlayer);
        }
        if (dialog.InfoPlayer != -1)
        {
            if (dialog.RNewActionPnl.Visible)
            {
                dialog.ShowPlayerActions(dialog.InfoPlayer);
            }
            else
            {
                dialog.ShowPlayerInformation(dialog.InfoPlayer);
            }
        }
        else if (dialog.SelectedRace != -1)
        {
            dialog.HighlightRace(dialog.SelectedRace);
        }
        if (dialog.InfoRace != -1)
        {
            dialog.ShowRaceInformation((byte)dialog.InfoRace);
        }
    }

    private static void HideItemSelection(InfoWindow window)
    {
        window.InfoWin.Visible = false;
        window.ItemBag = null;
    }

    private static void MagicPlayerChanged(MagicWindow window, sbyte player)
    {
        if (window.Dlg.Visible && player == AoWHSMap.Current.SeatedPlayer)
        {
            MagicWindow.RefreshMagicWindow(window);
        }
    }

    private static void GlobalMagicChanged(MagicWindow window)
    {
        if (window.Dlg.Visible)
        {
            MagicWindow.RefreshGlobalMagic(window);
        }
    }
}
