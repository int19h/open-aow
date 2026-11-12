// Translated from aowMapEvents.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Session;

using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// map control
public partial class MapEvents : UIModule
{
    public MapEvents(UIContext ui) : base(ui)
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
            UI.SpellBook.ShowCasting(request);
        }
        else
        {
            if (request.AbilityId == 125)
            {
                UI.ConstructDialog.Execute(request);
            }
            else
            {
                if (request is UnitSelectionActivation)
                {
                    UI.UnitSelectionDialog.Execute((UnitSelectionActivation)request);
                }
            }
        }
    }

    // aowMapEvents.pas:100
    public void TheMapArmyChanged(Army army)
    {
        if (army != null)
        {
            if (army == UI.General.UnitList)
            {
                TheMapArmySelected(army);
                if (UI.UnitWindow.MainWindow.Visible)
                {
                    UI.UnitWindow.ShowUnit();
                }

                if (UI.UnitBanner.BannerMenu.Visible)
                {
                    UI.UnitBanner.BannerPopupShow();
                }
            }

            if (StrategicMap.Current.SeatedPlayer == army.Player && UI.RealmWindow.Window.Visible && UI.RealmWindow.HeaderTabs.Selected == 3)
            {
                UI.RealmWindow.UpdateOverview();
            }
        }
        else
        {
            if (UI.RealmWindow.Window.Visible && UI.RealmWindow.HeaderTabs.Selected == 3)
            {
                UI.RealmWindow.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:117
    public void TheMapArmyDeselected()
    {
        UI.General.Abilities.Clear();
        UI.General.UnitList = null;
        UI.ControlWindow.MovesLabel.Text = "";
        UI.ControlWindow.MoveIcon.Visible = false;
        UI.ControlWindow.EmptySlot1Frame.Visible = true;
        UI.ControlWindow.EmptyCheck1Frame.Visible = true;
        UI.ControlWindow.EmptySlot2Frame.Visible = true;
        UI.ControlWindow.EmptyCheck2Frame.Visible = true;
        UI.ControlWindow.EmptySlot3Frame.Visible = true;
        UI.ControlWindow.EmptyCheck3Frame.Visible = true;
        UI.ControlWindow.EmptySlot4Frame.Visible = true;
        UI.ControlWindow.EmptyCheck4Frame.Visible = true;
        UI.ControlWindow.EmptySlot5Frame.Visible = true;
        UI.ControlWindow.EmptyCheck5Frame.Visible = true;
        UI.ControlWindow.EmptySlot6Frame.Visible = true;
        UI.ControlWindow.EmptyCheck6Frame.Visible = true;
        UI.ControlWindow.EmptySlot7Frame.Visible = true;
        UI.ControlWindow.EmptyCheck7Frame.Visible = true;
        UI.ControlWindow.EmptySlot8Frame.Visible = true;
        UI.ControlWindow.EmptyCheck8Frame.Visible = true;
        UI.ControlWindow.MainWindow.Visible = false;
        UI.TaskBar.PartyButton.Visible = false;
        if (UI.UnitWindow.MainWindow.Visible)
        {
            UI.UnitWindow.MainWindow.Visible = false;
        }
    }

    // aowMapEvents.pas:144
    public void TheMapArmyDestroyed(Army army)
    {
        if (army != null && army == UI.General.UnitList)
        {
            TheMapArmyDeselected();
        }

        if (UI.RealmWindow.Window.Visible && UI.RealmWindow.HeaderTabs.Selected == 3)
        {
            UI.RealmWindow.UpdateOverview();
        }
    }

    // aowMapEvents.pas:150
    public void TheMapArmySelected(Army army)
    {
        int count;
        count = 0;
        for (int I = 0; I <= 7; I++)
        {
            UI.General.SelectedUnits[I] = -1;
        }

        for (int I = 0, last = army.GetCount() - 1; I <= last; I++)
        {
            if (!army.GetUnitConcealedForPlayer(I, StrategicMap.Current.SeatedPlayer))
            {
                UI.General.SelectedUnits[count] = I;
                count += 1;
            }
        }

        UI.General.UnitList = army;
        UI.ControlWindow.UpdateArmy();
        if (StrategicMap.Current.SeatedPlayer == army.Player)
        {
            StrategicMap.Current.TutorialControl.ShowMessage(63, true, true);
        }
    }

    // aowMapEvents.pas:167
    public void TheMapDayChanged(int day)
    {
        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            UI.TaskBar.WorldTurnButton.LastPlayer = -1;
        }

        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_dayFormat), [day]);
        UI.PlayerInfoWindow.TurnLabel.Text = format;
        UI.MapEvents.TheMapGemsChanged(StrategicMap.Current.SeatedPlayer);
        UI.MapEvents.TheMapIncomeChanged(StrategicMap.Current.SeatedPlayer);
        if (UI.RealmWindow.Window.Visible)
        {
            UI.RealmWindow.UpdateOverview();
        }
    }

    // aowMapEvents.pas:187
    public void TheMapEndTurn(sbyte player, bool cancelled)
    {
        if (StrategicMap.Current.SeatedPlayer == player && StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).GameOverStatus == GameOverStatus.None)
        {
            if (cancelled)
            {
                UI.TaskBar.WorldTurnButton.Drawn = false;
                UI.TaskBar.WorldTurnButton.ButtonChecked = false;
            }
            else
            {
                UI.General.HideTransientWindows();
            }
        }

        UI.TaskBar.PlayerIndicatorFrame.Drawn = false;
    }

    // aowMapEvents.pas:198
    public void TheMapExecuteEventLog(EventLog eventValue, bool mode, ExecuteDoneEvent done)
    {
        if (!mode)
        {
            UI.General.HideTransientWindows();
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
        if (StrategicMap.Current.SeatedPlayer == player)
        {
            update = true;
        }
        else
        {
            if (StrategicMap.Current.CombatMap != null)
            {
                if (StrategicMap.Current.CombatMap.SeatedPlayer == player)
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
                if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                {
                    update = false;
                }
            }

            if (!update)
            {
                // { The original clears both income signs when the gold display is cleared. }
                UI.TaskBar.WorldGoldTotalLabel.Text = "";
                UI.TaskBar.WorldGoldSignLabel.Text = "";
                UI.TaskBar.WorldManaSignLabel.Text = "";
            }
            else
            {
                var playerLocal = StrategicMap.Current.Players.GetPlayers(player);
                UI.TaskBar.WorldGoldTotalLabel.Text = IntToStr(playerLocal.Gems);
                if (UI.RealmWindow.Window.Visible && UI.RealmWindow.HeaderTabs.Selected == 0)
                {
                    UI.RealmWindow.UpdateOverview();
                }
            }

            UI.TaskBar.CombatGoldTotalLabel.Text = UI.TaskBar.WorldGoldTotalLabel.Text;
            UI.TaskBar.CombatGoldSignLabel.Text = UI.TaskBar.WorldGoldSignLabel.Text;
            UI.TaskBar.CombatManaSignLabel.Text = UI.TaskBar.WorldManaSignLabel.Text;
        }
    }

    // aowMapEvents.pas:320
    public void TheMapGlobalMagicChanged()
    {
        GlobalMagicChanged(UI.MagicWindow);
    }

    // aowMapEvents.pas:323
    public void TheMapIncomeChanged(sbyte player)
    {
        bool update;
        int netIncome;
        update = false;
        if (StrategicMap.Current.SeatedPlayer == player)
        {
            update = true;
        }
        else
        {
            if (StrategicMap.Current.CombatMap != null)
            {
                if (StrategicMap.Current.CombatMap.SeatedPlayer == player)
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
                if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                {
                    update = false;
                }
            }

            if (!update)
            {
                UI.TaskBar.WorldGoldIncomeLabel.Text = "";
            }
            else
            {
                var upkeep = StrategicMap.Current.Players.GetPlayers(player).GetUpkeep();
                netIncome = StrategicMap.Current.Players.GetPlayers(player).Income - upkeep;
                if (netIncome >= 0)
                {
                    UI.TaskBar.WorldGoldSignLabel.Text = "+";
                    UI.TaskBar.WorldGoldSignLabel.TextColor = 7864183;
                    UI.TaskBar.WorldGoldIncomeLabel.TextColor = 7864183;
                }
                else
                {
                    UI.TaskBar.WorldGoldSignLabel.Text = "-";
                    UI.TaskBar.WorldGoldSignLabel.TextColor = 119;
                    UI.TaskBar.WorldGoldIncomeLabel.TextColor = 119;
                }

                UI.TaskBar.WorldGoldIncomeLabel.Text = IntToStr(Math.Abs(netIncome));
                if (UI.RealmWindow.Window.Visible && UI.RealmWindow.HeaderTabs.Selected == 0)
                {
                    UI.RealmWindow.UpdateOverview();
                }
            }

            UI.TaskBar.CombatGoldIncomeLabel.Text = UI.TaskBar.WorldGoldIncomeLabel.Text;
            UI.TaskBar.CombatGoldSignLabel.Text = UI.TaskBar.WorldGoldSignLabel.Text;
            UI.TaskBar.CombatGoldSignLabel.TextColor = UI.TaskBar.WorldGoldSignLabel.TextColor;
            UI.TaskBar.CombatGoldIncomeLabel.TextColor = UI.TaskBar.WorldGoldIncomeLabel.TextColor;
        }
    }

    // aowMapEvents.pas:362
    public void TheMapMapChanged(ref MapChangedInfo info)
    {
        if (info.MapLevel == UI.ScannerWindow.Scanner.Level)
        {
            UI.ScannerWindow.Scanner.DrawMap(info.Bounds.Left - 6, info.Bounds.Top - 6, info.Bounds.Right - info.Bounds.Left, info.Bounds.Bottom - info.Bounds.Top);
        }
    }

    // aowMapEvents.pas:370
    public void TheMapScannerIconsChanged(ref MapChangedInfo info)
    {
        if (info.MapLevel == UI.ScannerWindow.Scanner.Level)
        {
            UI.ScannerWindow.Scanner.SetObDrawn(false);
        }
    }

    // aowMapEvents.pas:375
    public void TheMapPlayerMagicChanged(sbyte player)
    {
        bool update;
        int netIncome;
        MagicPlayerChanged(UI.MagicWindow, player);
        UI.SpellBook.PlayerChanged(player);
        update = false;
        if (StrategicMap.Current.SeatedPlayer == player)
        {
            update = true;
        }
        else
        {
            if (StrategicMap.Current.CombatMap != null)
            {
                if (StrategicMap.Current.CombatMap.SeatedPlayer == player)
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
                if (StrategicMap.Current.Players.GetPlayers(player).PlayerType != PlayerType.Human)
                {
                    update = false;
                }
            }

            if (!update)
            {
                UI.TaskBar.WorldManaTotalLabel.Text = "";
                UI.TaskBar.WorldManaIncomeLabel.Text = "";
            }
            else
            {
                var playerLocal = StrategicMap.Current.Players.GetPlayers(player);
                UI.TaskBar.WorldManaTotalLabel.Text = IntToStr(playerLocal.MagicControl.Mana);
                netIncome = StrategicMap.Current.Players.GetPlayers(player).MagicControl.GetManaIncome() - StrategicMap.Current.Players.GetPlayers(player).MagicControl.GetManaUpkeep();
                if (netIncome >= 0)
                {
                    UI.TaskBar.WorldManaSignLabel.Text = "+";
                    UI.TaskBar.WorldManaSignLabel.TextColor = 7864183;
                    UI.TaskBar.WorldManaIncomeLabel.TextColor = 7864183;
                }
                else
                {
                    UI.TaskBar.WorldManaSignLabel.Text = "-";
                    UI.TaskBar.WorldManaSignLabel.TextColor = 119;
                    UI.TaskBar.WorldManaIncomeLabel.TextColor = 119;
                }

                UI.TaskBar.WorldManaIncomeLabel.Text = IntToStr(Math.Abs(netIncome));
            }

            UI.TaskBar.CombatManaTotalLabel.Text = UI.TaskBar.WorldManaTotalLabel.Text;
            UI.TaskBar.CombatManaIncomeLabel.Text = UI.TaskBar.WorldManaIncomeLabel.Text;
            UI.TaskBar.CombatManaSignLabel.Text = UI.TaskBar.WorldManaSignLabel.Text;
            UI.TaskBar.CombatManaSignLabel.TextColor = UI.TaskBar.WorldManaSignLabel.TextColor;
            UI.TaskBar.CombatManaIncomeLabel.TextColor = UI.TaskBar.WorldManaIncomeLabel.TextColor;
        }
    }

    // aowMapEvents.pas:1021
    public void TheMapLogbookChanged(sbyte player)
    {
        if (player == StrategicMap.Current.SeatedPlayer)
        {
            UI.PlayerInfoWindow.UpdateEvents();
        }
    }

    // aowMapEvents.pas:1026
    public void TheMapPlayerTerminated(sbyte player)
    {
        UI.TaskBar.UpdatePlayerIndicator();
        if (StrategicMap.Current != null)
            if (StrategicMap.Current.ConnectionType == PlayerConnectionType.Network)
                if (player == StrategicMap.Current.SeatedPlayer)
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
            UI.General.HideWindows();
            UI.General.Manager.OverlayPriority = 0;
            UI.TaskBar.CurrentPlayerImage.Visible = false;
            UI.TaskBar.WorldClockDelay = 0;
            UI.TaskBar.WorldTurnButton.CurrentPlayer = 12;
            UI.TaskBar.WorldTurnButton.LastPlayer = UI.TaskBar.WorldTurnButton.CurrentPlayer;
            UI.TaskBar.WorldTurnButton.AnimationFrame = UI.TaskBar.WorldTurnButton.CurrentPlayer * 7;
        }
        else
        {
            UI.General.ShowWindows();
            var turnButton = UI.TaskBar.WorldTurnButton;
            if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated) && !turnButton.Animating)
            {
                if (player > -1)
                {
                    turnButton.CurrentPlayer = StrategicMap.Current.Players.GetPlayers(player).PlayerRace;
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

            UI.TaskBar.CurrentPlayerImage.Visible = StrategicMap.Current.PlayMode == PlayMode.Turn;
            UI.PlayerInfoWindow.UpdatePlayer();
            if (UI.RealmWindow.Window.Visible)
            {
                UI.RealmWindow.UpdateOverview();
            }
        }

        TheMapGemsChanged(player);
        TheMapIncomeChanged(player);
        TheMapPlayerMagicChanged(player);
        UI.TaskBar.PlayerIndicatorFrame.Drawn = false;
        UI.PlayerInfoWindow.UpdateEvents();
    }

    // aowMapEvents.pas:440
    public void TheMapShowMessage(ref ShowMessageInfo info)
    {
        UI.TaskBar.WorldMessageEdit.TextColor = info.Color;
        UI.TaskBar.WorldMessageEdit.Text = info.Text;
    }

    // aowMapEvents.pas:446
    public void TheMapStructureChanged(Structure structure)
    {
        UpdateStructureInfo(UI.InfoWindow, structure);
        if (structure != null)
        {
            UI.CityScreen.CityChanged(structure);
            UI.ProductionScreen.PlaceChanged(structure);
        }

        if (UI.RealmWindow.Window.Visible && UI.RealmWindow.HeaderTabs.Selected < 3)
        {
            UI.RealmWindow.UpdateOverview();
        }
    }

    // aowMapEvents.pas:457
    public void TheMapStructureDeselected()
    {
        if (UI.General.Sign != null)
        {
            UI.General.Sign.ChangeText(StrategicMap.Current.SeatedPlayer, UI.InfoWindow.SignTextMemo.Items);
        }

        UI.General.Structure = null;
        UI.General.City = null;
        UI.General.ProductionPlace = null;
        UI.General.ProductionControl = null;
        UI.General.Sign = null;
        UI.InfoWindow.StructureNameLabel.Text = "";
        UI.InfoWindow.StructureLine.Visible = false;
        UI.InfoWindow.MainWindow.Visible = false;
        UI.TaskBar.StructureButton.Visible = false;
    }

    // aowMapEvents.pas:467
    public void TheMapStructureDestroyed(Structure structure)
    {
        if (structure != null && structure == UI.General.Structure)
        {
            TheMapStructureDeselected();
        }

        if (UI.RealmWindow.Window.Visible)
        {
            if (UI.RealmWindow.HeaderTabs.Selected == 2 || UI.RealmWindow.HeaderTabs.Selected == 1)
            {
                UI.RealmWindow.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:473
    public void TheMapStructureSelected(Structure structure)
    {
        UI.General.PreviousStructure = UI.General.Structure;
        UI.General.Structure = structure;
        UI.General.ProductionControl = null;
        ShowStructureInfo(UI.InfoWindow, structure);
    }

    // aowMapEvents.pas:481
    public void TheMapItemsDeselected()
    {
        HideItemSelection(UI.InfoWindow);
    }

    // aowMapEvents.pas:484
    public void TheMapItemsSelected(ItemHexSprite items)
    {
        ShowItemSelection(UI.InfoWindow, items);
    }

    // aowMapEvents.pas:487
    public void TheMapTargetSelectionChange(TargetSelectionControl control, TargetSelectionChange change)
    {
        ShowTargetSelection(UI.TargetSelectionDialog, control, change);
    }

    // aowMapEvents.pas:490
    public void TheMapUnitChanged(Troop U)
    {
        if (U == UI.General.SelectedUnit)
        {
            if (UI.UnitWindow.MainWindow.Visible)
            {
                UI.UnitWindow.ShowUnit();
            }

            if (UI.UnitBanner.BannerMenu.Visible)
            {
                UI.UnitBanner.BannerPopupShow();
            }
        }

        UI.SpellBook.UnitChanged(U);
    }

    // aowMapEvents.pas:500
    public void TheMapCastSpell(SpellCaster caster)
    {
        if (caster is UnitSpellCaster)
        {
            ShowUnitSpellCaster(UI.CastUnitSpellDialog, (UnitSpellCaster)caster);
        }
        else
        {
            if (caster is DisjunctionSpellCaster)
            {
                UI.DisjunctionDialog.Execute((DisjunctionSpellCaster)caster);
            }
            else
            {
                if (caster is CosmeticSurgerySpellCaster)
                {
                    UI.DeformFaceDialog.Execute((CosmeticSurgerySpellCaster)caster);
                }
            }
        }
    }

    // aowMapEvents.pas:507
    public void TheMapHexSpriteChanged(HexSprite hexSprite)
    {
        if (hexSprite is ItemHexSprite)
        {
            UpdateItemSelection(UI.InfoWindow, (ItemHexSprite)hexSprite);
        }
    }

    // aowMapEvents.pas:512
    public void TheMapRelationsChanged()
    {
        RefreshRelations(UI.RelationWindow);
    }

    // aowMapEvents.pas:515
    public void TheMapDiplomaticChange()
    {
        RefreshRelations(UI.RelationWindow);
    }

    // aowMapEvents.pas:518
    public void TheMapStructureDoubleClick(Structure structure)
    {
        if (structure == UI.General.Structure && structure != null)
        {
            if (StructureScreens.TryGet(structure.DoubleClick(StrategicMap.Current.SeatedPlayer), out var open))
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
            UI.General.SaveLayout();
            UI.General.HideWindows();
            UI.MapWindow.MapViewer.HexSpriteMap = null;
            if (UI.SetupEvents.LobbyGame)
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
        InfoWindow.StructurePanels[structure.InfoPanel(StrategicMap.Current.SeatedPlayer)].Invoke(dialog, structure);
        if (!dialog.UI.TaskBar.StructureButton.Visible)
        {
            dialog.UI.InfoWindow.MainWindow.Visible = true;
        }
        dialog.UI.TaskBar.StructureButton.Visible = true;
    }

    private static void UpdateStructureInfo(InfoWindow dialog, Structure structure)
    {
        if (!dialog.MainWindow.Visible)
        {
            return;
        }
        if (structure == null)
        {
            structure = dialog.UI.General.Structure;
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
        LibraryImage imageTop = dialog.UI.Graphics.MiscArt[35];
        dialog.StructureLine.ImageTop = imageTop;
        LibraryImage imageMiddle = dialog.UI.Graphics.MiscArt[36];
        dialog.StructureLine.ImageMiddle = imageMiddle;
        LibraryImage imageBottom = dialog.UI.Graphics.MiscArt[37];
        dialog.StructureLine.ImageBottom = imageBottom;
        ArmyHexSprite armyHexSprite = items.Owner.GetArmyHexSprite();
        if (armyHexSprite != null && armyHexSprite.Army.Player == StrategicMap.Current.SeatedPlayer)
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
                    dialog.UI.General.Manager.OverlayPriority = 49;
                    Window mapWindow = dialog.UI.MapWindow.MainWindow;
                    mapWindow._modal = true;
                    mapWindow.Priority = 500;
                    dialog.UI.ScannerWindow.MainWindow.Priority = dialog.Window.Priority;
                    break;
                }
            case TargetSelectionChange.Deactivated:
                dialog.Window.Visible = false;
                dialog.UI.MapWindow.MainWindow.Priority = 0;
                dialog.UI.MapWindow.MainWindow._modal = false;
                dialog.UI.ScannerWindow.MainWindow.Priority = 10;
                dialog.UI.General.Manager.OverlayPriority = 0;
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
        Hero hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(dialog.Caster.CasterSourceId);
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
        Spell spell = GamePack.Current.SpellControl.GetSpell(dialog.Caster.CasterSpellId);
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
        Player players = StrategicMap.Current.Players.GetPlayers(hero.Player);
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
        if (window.Window.Visible && player == StrategicMap.Current.SeatedPlayer)
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
