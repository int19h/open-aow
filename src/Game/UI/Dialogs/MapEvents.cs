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
                if (DialogHosts.UnitWindow.MainWindow.Visible)
                {
                    DialogHosts.UnitWindow.ShowUnit();
                }

                if (DialogHosts.UnitBanner.BannerMenu.Visible)
                {
                    DialogHosts.UnitBanner.BannerPopupShow();
                }
            }

            if (AoWHSMap.Current.SeatedPlayer == army.Player && DialogHosts.RealmWindow.Window.Visible && DialogHosts.RealmWindow.HeaderTabs.Selected == 3)
            {
                DialogHosts.RealmWindow.UpdateOverview();
            }
        }
        else
        {
            if (DialogHosts.RealmWindow.Window.Visible && DialogHosts.RealmWindow.HeaderTabs.Selected == 3)
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
        DialogHosts.ControlWindow.MovesLabel.Text = "";
        DialogHosts.ControlWindow.MoveIcon.Visible = false;
        DialogHosts.ControlWindow.EmptySlot1Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck1Frame.Visible = true;
        DialogHosts.ControlWindow.EmptySlot2Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck2Frame.Visible = true;
        DialogHosts.ControlWindow.EmptySlot3Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck3Frame.Visible = true;
        DialogHosts.ControlWindow.EmptySlot4Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck4Frame.Visible = true;
        DialogHosts.ControlWindow.EmptySlot5Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck5Frame.Visible = true;
        DialogHosts.ControlWindow.EmptySlot6Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck6Frame.Visible = true;
        DialogHosts.ControlWindow.EmptySlot7Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck7Frame.Visible = true;
        DialogHosts.ControlWindow.EmptySlot8Frame.Visible = true;
        DialogHosts.ControlWindow.EmptyCheck8Frame.Visible = true;
        DialogHosts.ControlWindow.MainWindow.Visible = false;
        DialogHosts.TaskBar.PartyButton.Visible = false;
        if (DialogHosts.UnitWindow.MainWindow.Visible)
        {
            DialogHosts.UnitWindow.MainWindow.Visible = false;
        }
    }

    // aowMapEvents.pas:144
    public void TheMapArmyDestroyed(Army army)
    {
        if (army != null && army == DialogHosts.General.UnitList)
        {
            TheMapArmyDeselected();
        }

        if (DialogHosts.RealmWindow.Window.Visible && DialogHosts.RealmWindow.HeaderTabs.Selected == 3)
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
            DialogHosts.TaskBar.WorldTurnButton.LastPlayer = -1;
        }

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_dayFormat), [day]);
        DialogHosts.PlayerInfoWindow.TurnLabel.Text = format;
        DialogHosts.MapEvents.TheMapGemsChanged(AoWHSMap.Current.SeatedPlayer);
        DialogHosts.MapEvents.TheMapIncomeChanged(AoWHSMap.Current.SeatedPlayer);
        if (DialogHosts.RealmWindow.Window.Visible)
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
                DialogHosts.TaskBar.WorldTurnButton.Drawn = false;
                DialogHosts.TaskBar.WorldTurnButton.ButtonChecked = false;
            }
            else
            {
                DialogHosts.General.HideTransientWindows();
            }
        }

        DialogHosts.TaskBar.PlayerIndicatorFrame.Drawn = false;
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
                DialogHosts.TaskBar.WorldGoldTotalLabel.Text = "";
                DialogHosts.TaskBar.WorldGoldSignLabel.Text = "";
                DialogHosts.TaskBar.WorldManaSignLabel.Text = "";
            }
            else
            {
                var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
                DialogHosts.TaskBar.WorldGoldTotalLabel.Text = IntToStr(playerLocal.Gems);
                if (DialogHosts.RealmWindow.Window.Visible && DialogHosts.RealmWindow.HeaderTabs.Selected == 0)
                {
                    DialogHosts.RealmWindow.UpdateOverview();
                }
            }

            DialogHosts.TaskBar.CombatGoldTotalLabel.Text = DialogHosts.TaskBar.WorldGoldTotalLabel.Text;
            DialogHosts.TaskBar.CombatGoldSignLabel.Text = DialogHosts.TaskBar.WorldGoldSignLabel.Text;
            DialogHosts.TaskBar.CombatManaSignLabel.Text = DialogHosts.TaskBar.WorldManaSignLabel.Text;
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
                DialogHosts.TaskBar.WorldGoldIncomeLabel.Text = "";
            }
            else
            {
                var upkeep = AoWHSMap.Current.Players.GetPlayers(player).GetUpkeep();
                netIncome = AoWHSMap.Current.Players.GetPlayers(player).Income - upkeep;
                if (netIncome >= 0)
                {
                    DialogHosts.TaskBar.WorldGoldSignLabel.Text = "+";
                    DialogHosts.TaskBar.WorldGoldSignLabel.TextColor = 7864183;
                    DialogHosts.TaskBar.WorldGoldIncomeLabel.TextColor = 7864183;
                }
                else
                {
                    DialogHosts.TaskBar.WorldGoldSignLabel.Text = "-";
                    DialogHosts.TaskBar.WorldGoldSignLabel.TextColor = 119;
                    DialogHosts.TaskBar.WorldGoldIncomeLabel.TextColor = 119;
                }

                DialogHosts.TaskBar.WorldGoldIncomeLabel.Text = IntToStr(Math.Abs(netIncome));
                if (DialogHosts.RealmWindow.Window.Visible && DialogHosts.RealmWindow.HeaderTabs.Selected == 0)
                {
                    DialogHosts.RealmWindow.UpdateOverview();
                }
            }

            DialogHosts.TaskBar.CombatGoldIncomeLabel.Text = DialogHosts.TaskBar.WorldGoldIncomeLabel.Text;
            DialogHosts.TaskBar.CombatGoldSignLabel.Text = DialogHosts.TaskBar.WorldGoldSignLabel.Text;
            DialogHosts.TaskBar.CombatGoldSignLabel.TextColor = DialogHosts.TaskBar.WorldGoldSignLabel.TextColor;
            DialogHosts.TaskBar.CombatGoldIncomeLabel.TextColor = DialogHosts.TaskBar.WorldGoldIncomeLabel.TextColor;
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
                DialogHosts.TaskBar.WorldManaTotalLabel.Text = "";
                DialogHosts.TaskBar.WorldManaIncomeLabel.Text = "";
            }
            else
            {
                var playerLocal = AoWHSMap.Current.Players.GetPlayers(player);
                DialogHosts.TaskBar.WorldManaTotalLabel.Text = IntToStr(playerLocal.MagicControl.Mana);
                netIncome = AoWHSMap.Current.Players.GetPlayers(player).MagicControl.GetManaIncome() - AoWHSMap.Current.Players.GetPlayers(player).MagicControl.GetManaUpkeep();
                if (netIncome >= 0)
                {
                    DialogHosts.TaskBar.WorldManaSignLabel.Text = "+";
                    DialogHosts.TaskBar.WorldManaSignLabel.TextColor = 7864183;
                    DialogHosts.TaskBar.WorldManaIncomeLabel.TextColor = 7864183;
                }
                else
                {
                    DialogHosts.TaskBar.WorldManaSignLabel.Text = "-";
                    DialogHosts.TaskBar.WorldManaSignLabel.TextColor = 119;
                    DialogHosts.TaskBar.WorldManaIncomeLabel.TextColor = 119;
                }

                DialogHosts.TaskBar.WorldManaIncomeLabel.Text = IntToStr(Math.Abs(netIncome));
            }

            DialogHosts.TaskBar.CombatManaTotalLabel.Text = DialogHosts.TaskBar.WorldManaTotalLabel.Text;
            DialogHosts.TaskBar.CombatManaIncomeLabel.Text = DialogHosts.TaskBar.WorldManaIncomeLabel.Text;
            DialogHosts.TaskBar.CombatManaSignLabel.Text = DialogHosts.TaskBar.WorldManaSignLabel.Text;
            DialogHosts.TaskBar.CombatManaSignLabel.TextColor = DialogHosts.TaskBar.WorldManaSignLabel.TextColor;
            DialogHosts.TaskBar.CombatManaIncomeLabel.TextColor = DialogHosts.TaskBar.WorldManaIncomeLabel.TextColor;
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
            DialogHosts.TaskBar.CurrentPlayerImage.Visible = false;
            DialogHosts.TaskBar.WorldClockDelay = 0;
            DialogHosts.TaskBar.WorldTurnButton.CurrentPlayer = 12;
            DialogHosts.TaskBar.WorldTurnButton.LastPlayer = DialogHosts.TaskBar.WorldTurnButton.CurrentPlayer;
            DialogHosts.TaskBar.WorldTurnButton.AnimationFrame = DialogHosts.TaskBar.WorldTurnButton.CurrentPlayer * 7;
        }
        else
        {
            DialogHosts.General.ShowWindows();
            var turnButton = DialogHosts.TaskBar.WorldTurnButton;
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

            DialogHosts.TaskBar.CurrentPlayerImage.Visible = AoWHSMap.Current.PlayMode == PlayMode.Turn;
            DialogHosts.PlayerInfoWindow.UpdatePlayer();
            if (DialogHosts.RealmWindow.Window.Visible)
            {
                DialogHosts.RealmWindow.UpdateOverview();
            }
        }

        TheMapGemsChanged(player);
        TheMapIncomeChanged(player);
        TheMapPlayerMagicChanged(player);
        DialogHosts.TaskBar.PlayerIndicatorFrame.Drawn = false;
        DialogHosts.PlayerInfoWindow.UpdateEvents();
    }

    // aowMapEvents.pas:440
    public void TheMapShowMessage(ref ShowMessageInfo info)
    {
        DialogHosts.TaskBar.WorldMessageEdit.TextColor = info.Color;
        DialogHosts.TaskBar.WorldMessageEdit.Text = info.Text;
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

        if (DialogHosts.RealmWindow.Window.Visible && DialogHosts.RealmWindow.HeaderTabs.Selected < 3)
        {
            DialogHosts.RealmWindow.UpdateOverview();
        }
    }

    // aowMapEvents.pas:457
    public void TheMapStructureDeselected()
    {
        if (DialogHosts.General.Sign != null)
        {
            DialogHosts.General.Sign.ChangeText(AoWHSMap.Current.SeatedPlayer, DialogHosts.InfoWindow.SignTextMemo.Items);
        }

        DialogHosts.General.Structure = null;
        DialogHosts.General.City = null;
        DialogHosts.General.ProductionPlace = null;
        DialogHosts.General.ProductionControl = null;
        DialogHosts.General.Sign = null;
        DialogHosts.InfoWindow.StructureNameLabel.Text = "";
        DialogHosts.InfoWindow.StructureLine.Visible = false;
        DialogHosts.InfoWindow.MainWindow.Visible = false;
        DialogHosts.TaskBar.StructureButton.Visible = false;
    }

    // aowMapEvents.pas:467
    public void TheMapStructureDestroyed(Structure structure)
    {
        if (structure != null && structure == DialogHosts.General.Structure)
        {
            TheMapStructureDeselected();
        }

        if (DialogHosts.RealmWindow.Window.Visible)
        {
            if (DialogHosts.RealmWindow.HeaderTabs.Selected == 2 || DialogHosts.RealmWindow.HeaderTabs.Selected == 1)
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
            if (DialogHosts.UnitWindow.MainWindow.Visible)
            {
                DialogHosts.UnitWindow.ShowUnit();
            }

            if (DialogHosts.UnitBanner.BannerMenu.Visible)
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
    public void TheMapHexSpriteChanged(HexSprite hexSprite)
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
    public void TheMapStructureDoubleClick(Structure structure)
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
    public void TheMapDeactivateHexSpriteMap()
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
        if (!DialogHosts.TaskBar.StructureButton.Visible)
        {
            DialogHosts.InfoWindow.MainWindow.Visible = true;
        }
        DialogHosts.TaskBar.StructureButton.Visible = true;
    }

    private static void UpdateStructureInfo(InfoWindow dialog, Structure structure)
    {
        if (!dialog.MainWindow.Visible)
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
        dialog.TitleLabel.Text = text;
        dialog.ItemBag = items;
        dialog.ItemPanel.Visible = true;
        dialog.MainWindow.Visible = true;
        dialog.StructureNameLabel.Text = "";
        LibraryImage imageTop = DialogHosts.UIGraphics.MiscArt[35];
        dialog.StructureLine.ImageTop = imageTop;
        LibraryImage imageMiddle = DialogHosts.UIGraphics.MiscArt[36];
        dialog.StructureLine.ImageMiddle = imageMiddle;
        LibraryImage imageBottom = DialogHosts.UIGraphics.MiscArt[37];
        dialog.StructureLine.ImageBottom = imageBottom;
        ArmyHexSprite armyHexSprite = ((AoWMapField)items.Owner).GetArmyHexSprite();
        if (armyHexSprite != null && armyHexSprite.Army.Player == AoWHSMap.Current.SeatedPlayer)
        {
            dialog.ItemBagMemo.Visible = false;
            Hero hero = armyHexSprite.Army.Hero(255, RosterTables.AllHeroTypes);
            dialog.MainButton.Visible = hero != null;
            string text2 = AbilityRules.TranslateResourceString(s_pickupText);
            dialog.MainButton.Text = text2;
            dialog.ItemBagList.Visible = true;
            ((StringCollection)dialog.ItemBagList.Items).Clear();
            dialog.ItemBagScrollBar.Visible = true;
            int i = 0;
            for (int number = items.Items.Count - 1; i <= number; i++)
            {
                ((StringCollection)dialog.ItemBagList.Items).Add("");
            }
            dialog.ItemsInBagLabel.Visible = true;
        }
        else
        {
            dialog.ItemBagMemo.Visible = true;
            dialog.ItemBagList.Visible = false;
            dialog.ItemsInBagLabel.Visible = false;
            dialog.ItemBagScrollBar.Visible = false;
            dialog.MainButton.Visible = false;
        }
    }

    private static void UpdateItemSelection(InfoWindow dialog, ItemHexSprite items)
    {
        if (dialog.MainWindow.Visible && items == dialog.ItemBag)
        {
            ShowItemSelection(dialog, dialog.ItemBag);
        }
    }

    internal static void ResetStructureInfo(InfoWindow dialog)
    {
        dialog.StructureLine.Visible = true;
        dialog.StructureLine.Sized = false;
        dialog.MainWindow.Aligned = false;
        dialog.CityPanel.Visible = false;
        dialog.CityBorderPanel.Visible = false;
        dialog.ProductionPlacePanel.Visible = false;
        dialog.SignPanel.Visible = false;
        dialog.NotYoursPanel.Visible = false;
        dialog.SingleButtonPanel.Visible = false;
        dialog.MainButton.Visible = true;
        dialog.RazeButton.Visible = true;
        dialog.ExplorationSitePanel.Visible = false;
        dialog.AltarPanel.Visible = false;
        dialog.IncomePanel.Visible = false;
        dialog.ItemPanel.Visible = false;
        dialog.ConstructionPanel.Visible = false;
        dialog.ProductionOrb.Visible = false;
        dialog.ProductionOrbTimeImage.Visible = false;
        dialog.ProductionOrbTurnsLabel.Visible = false;
        dialog.RazePanel.Visible = false;
        dialog.MainButton.Visible = false;
        ((StringCollection)dialog.SingleButtonMemo.Items).Clear();
        dialog.PopulationRaceLabel.Visible = false;
        dialog.PopulationLabel.Visible = false;
        string text = AbilityRules.TranslateResourceString(GameText.StructureText);
        dialog.TitleLabel.Text = text;
    }

    private static void ShowTargetSelection(TargetSelectionDialog dialog, TargetSelectionControl control, TargetSelectionChange change)
    {
        switch (change)
        {
            case TargetSelectionChange.Activated:
                {
                    dialog.HeaderLabel.Text = control.Title;
                    dialog.MainButton.Text = control.CancelText;
                    ((Persistent)dialog.DefaultMemo.Items).Assign((Persistent)(object)control.Text);
                    dialog.Window.Visible = true;
                    if (!control.TargetMode.Contains(TargetSelectionModeElement.ValidateDistance))
                    {
                        string text = AbilityRules.TranslateResourceString(UnlimitedText);
                        dialog.TargetLabel.Text = RtlUnit.LowerCase(text);
                    }
                    else
                    {
                        dialog.TargetLabel.Text = RtlUnit.IntToStr(control.Range);
                    }
                    dialog.BorderLine.ImageTop = control.SourceImage;
                    dialog.BorderLine.ImageMiddle = control.ValidTargetImage;
                    dialog.BorderLine.ImageBottom = control.InvalidTargetImage;
                    DialogHosts.General.Manager.OverlayPriority = 49;
                    Window mapWindow = DialogHosts.MapWindow.MainWindow;
                    mapWindow._modal = true;
                    mapWindow.Priority = 500;
                    DialogHosts.ScannerWindow.MainWindow.Priority = dialog.Window.Priority;
                    break;
                }
            case TargetSelectionChange.Deactivated:
                dialog.Window.Visible = false;
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
            dialog.CasterNameLabel.Text = name;
            sbyte castingPoints = hero.GetCastingPoints();
            string text = AbilityRules.TranslateResourceString(GameText.CastingPointsText);
            sbyte castingPointsMax = hero.GetCastingPointsMax();
            dialog.CastingPointsLabel.Text = string.Concat(new string[5]
            {
                text,
                ": ",
                RtlUnit.IntToStr((int)castingPoints),
                "/",
                RtlUnit.IntToStr((int)castingPointsMax)
            });
        }
        Spell spell = AoWHSSet.Current.SpellControl.GetSpell(dialog.Caster.CasterSpellId);
        dialog.SpellNameLabel.Text = spell.SpellName;
        int number = hero.CastingMana(spell);
        string text2 = AbilityRules.TranslateResourceString(GameText.CastingCostText);
        string text3 = AbilityRules.TranslateResourceString(GameText.ManaText);
        dialog.SpellManaLabel.Text = string.Concat(new string[5]
        {
            text2,
            ": ",
            RtlUnit.IntToStr(number),
            " ",
            text3
        });
        string text4 = AbilityRules.TranslateResourceString(s_upkeepText);
        string text5 = AbilityRules.TranslateResourceString(GameText.ManaText);
        dialog.SpellUpkeepLabel.Text = string.Concat(new string[5]
        {
            text4,
            ": ",
            RtlUnit.IntToStr(spell.ManaUpkeep),
            " ",
            text5
        });
        Player players = AoWHSMap.Current.Players.GetPlayers(hero.Player);
        string text6 = AbilityRules.TranslateResourceString(GameText.ManaText);
        dialog.CastingManaLabel.Text = text6 + ": " + RtlUnit.IntToStr(players.MagicControl.Mana);
        dialog.Unit1CheckBox.Checked = false;
        dialog.Unit2CheckBox.Checked = false;
        dialog.Unit3CheckBox.Checked = false;
        dialog.Unit4CheckBox.Checked = false;
        dialog.Unit5CheckBox.Checked = false;
        dialog.Unit6CheckBox.Checked = false;
        dialog.Unit7CheckBox.Checked = false;
        dialog.Unit8CheckBox.Checked = false;
        dialog.SelectionInfoLabel.Text = "";
        dialog.Window.Visible = true;
    }

    private static void RefreshRelations(RelationWindow dialog)
    {
        if (!dialog.MainWindow.Visible)
        {
            return;
        }
        dialog.FillPlayers();
        dialog.RelationPanel.Aligned = false;
        dialog.MainWindow.Drawn = false;
        if (dialog.SelectedPlayer != -1)
        {
            dialog.HighlightPlayer(dialog.SelectedPlayer);
        }
        if (dialog.InfoPlayer != -1)
        {
            if (dialog.NewActionPanel.Visible)
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
        window.MainWindow.Visible = false;
        window.ItemBag = null;
    }

    private static void MagicPlayerChanged(MagicWindow window, sbyte player)
    {
        if (window.Window.Visible && player == AoWHSMap.Current.SeatedPlayer)
        {
            MagicWindow.RefreshMagicWindow(window);
        }
    }

    private static void GlobalMagicChanged(MagicWindow window)
    {
        if (window.Window.Visible)
        {
            MagicWindow.RefreshGlobalMagic(window);
        }
    }
}
