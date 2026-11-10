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
    public MapEvents(Component owner) : base(owner)
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
            DialogHosts.SpellBook.ShowCasting(Request);
        }
        else
        {
            if (Request.FAbilityID == 125)
            {
                DialogHosts.ConstructDlg.Execute(Request);
            }
            else
            {
                if (Request is UnitSelectorAAI)
                {
                    DialogHosts.UnitSelectionDlg.Execute((UnitSelectorAAI)Request);
                }
            }
        }
    }

    // aowMapEvents.pas:100
    public void TheMapArmyChanged(Army Army)
    {
        if (Army != null)
        {
            if (Army == DialogHosts.General.FUnitList)
            {
                TheMapArmySelected(Army);
                if (DialogHosts.UnitWindow.UnitWin.Visible)
                {
                    DialogHosts.UnitWindow.ShowUnit();
                }

                if (DialogHosts.UnitBanner.BannerPopup.Visible)
                {
                    DialogHosts.UnitBanner.BannerPopupShow();
                }
            }

            if (AoWHSMap.Current.FSeatedPlayer == Army.FPlayer && DialogHosts.RealmWin.Dlg.Visible && DialogHosts.RealmWin.HeaderTab.Selected == 3)
            {
                DialogHosts.RealmWin.UpdateOverview();
            }
        }
        else
        {
            if (DialogHosts.RealmWin.Dlg.Visible && DialogHosts.RealmWin.HeaderTab.Selected == 3)
            {
                DialogHosts.RealmWin.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:117
    public void TheMapArmyDeselected()
    {
        DialogHosts.General.FAbilities.Clear();
        DialogHosts.General.FUnitList = null;
        DialogHosts.ControlWin.MovesLbl.Text = "";
        DialogHosts.ControlWin.MoveIcon.Visible = false;
        DialogHosts.ControlWin.Emp1Fr.Visible = true;
        DialogHosts.ControlWin.Emp1Chk.Visible = true;
        DialogHosts.ControlWin.Emp2Fr.Visible = true;
        DialogHosts.ControlWin.Emp2Chk.Visible = true;
        DialogHosts.ControlWin.Emp3Fr.Visible = true;
        DialogHosts.ControlWin.Emp3Chk.Visible = true;
        DialogHosts.ControlWin.Emp4Fr.Visible = true;
        DialogHosts.ControlWin.Emp4Chk.Visible = true;
        DialogHosts.ControlWin.Emp5Fr.Visible = true;
        DialogHosts.ControlWin.Emp5Chk.Visible = true;
        DialogHosts.ControlWin.Emp6Fr.Visible = true;
        DialogHosts.ControlWin.Emp6Chk.Visible = true;
        DialogHosts.ControlWin.Emp7Fr.Visible = true;
        DialogHosts.ControlWin.Emp7Chk.Visible = true;
        DialogHosts.ControlWin.Emp8Fr.Visible = true;
        DialogHosts.ControlWin.Emp8Chk.Visible = true;
        DialogHosts.ControlWin.ControlWindow.Visible = false;
        DialogHosts.TBWindow.ConButton.Visible = false;
        if (DialogHosts.UnitWindow.UnitWin.Visible)
        {
            DialogHosts.UnitWindow.UnitWin.Visible = false;
        }
    }

    // aowMapEvents.pas:144
    public void TheMapArmyDestroyed(Army Army)
    {
        if (Army != null && Army == DialogHosts.General.FUnitList)
        {
            TheMapArmyDeselected();
        }

        if (DialogHosts.RealmWin.Dlg.Visible && DialogHosts.RealmWin.HeaderTab.Selected == 3)
        {
            DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // aowMapEvents.pas:150
    public void TheMapArmySelected(Army Army)
    {
        int Count;
        Count = 0;
        for (int I = 0; I <= 7; I++)
        {
            DialogHosts.General.FSelectedUnits[I] = -1;
        }

        for (int I = 0, last = Army.GetCount() - 1; I <= last; I++)
        {
            if (!Army.GetUnitConcealedForPlayer(I, AoWHSMap.Current.FSeatedPlayer))
            {
                DialogHosts.General.FSelectedUnits[Count] = I;
                Count += 1;
            }
        }

        DialogHosts.General.FUnitList = Army;
        DialogHosts.ControlWin.UpdateArmy();
        if (AoWHSMap.Current.FSeatedPlayer == Army.FPlayer)
        {
            AoWHSMap.Current.FTutorialControl.ShowMessage(63, true, true);
        }
    }

    // aowMapEvents.pas:167
    public void TheMapDayChanged(int Day)
    {
        if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Activated))
        {
            DialogHosts.TBWindow.TurnButton.FLastPlayer = -1;
        }

        var format = AbilityRules.Format(AbilityRules.TranslateRStr(DayXRStr), [Day]);
        DialogHosts.PInfoW.Turn.Text = format;
        DialogHosts.MapEvents.TheMapGemsChanged(AoWHSMap.Current.FSeatedPlayer);
        DialogHosts.MapEvents.TheMapIncomeChanged(AoWHSMap.Current.FSeatedPlayer);
        if (DialogHosts.RealmWin.Dlg.Visible)
        {
            DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // aowMapEvents.pas:187
    public void TheMapEndTurn(sbyte Player, bool Cancelled)
    {
        if (AoWHSMap.Current.FSeatedPlayer == Player && AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FGameOverStatus == GameOverStatus.None)
        {
            if (Cancelled)
            {
                DialogHosts.TBWindow.TurnButton.FDrawn = false;
                DialogHosts.TBWindow.TurnButton.FButtonChecked = false;
            }
            else
            {
                DialogHosts.General.HideTransientWindows();
            }
        }

        DialogHosts.TBWindow.PInd.FDrawn = false;
    }

    // aowMapEvents.pas:198
    public void TheMapExecuteEventLog(EventLog Event, bool Mode, ExecuteDoneEvent Done)
    {
        if (!Mode)
        {
            DialogHosts.General.HideTransientWindows();
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
        if (AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Update = true;
        }
        else
        {
            if (AoWHSMap.Current.FCombatMap != null)
            {
                if (AoWHSMap.Current.FCombatMap.FSeatedPlayer == Player)
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
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                // { The original clears both income signs when the gold display is cleared. }
                DialogHosts.TBWindow.WGTLbl.Text = "";
                DialogHosts.TBWindow.WGSign.Text = "";
                DialogHosts.TBWindow.WMTSign.Text = "";
            }
            else
            {
                var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                DialogHosts.TBWindow.WGTLbl.Text = IntToStr(player.FGems);
                if (DialogHosts.RealmWin.Dlg.Visible && DialogHosts.RealmWin.HeaderTab.Selected == 0)
                {
                    DialogHosts.RealmWin.UpdateOverview();
                }
            }

            DialogHosts.TBWindow.TGTLbl.Text = DialogHosts.TBWindow.WGTLbl.Text;
            DialogHosts.TBWindow.TGSign.Text = DialogHosts.TBWindow.WGSign.Text;
            DialogHosts.TBWindow.TMTSign.Text = DialogHosts.TBWindow.WMTSign.Text;
        }
    }

    // aowMapEvents.pas:320
    public void TheMapGlobalMagicChanged()
    {
        GlobalMagicChanged(DialogHosts.MagicWin);
    }

    // aowMapEvents.pas:323
    public void TheMapIncomeChanged(sbyte Player)
    {
        bool Update;
        int NetIncome;
        Update = false;
        if (AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Update = true;
        }
        else
        {
            if (AoWHSMap.Current.FCombatMap != null)
            {
                if (AoWHSMap.Current.FCombatMap.FSeatedPlayer == Player)
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
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                DialogHosts.TBWindow.WGILbl.Text = "";
            }
            else
            {
                var upkeep = AoWHSMap.Current.FPlayers.GetPlayers(Player).GetUpkeep();
                NetIncome = AoWHSMap.Current.FPlayers.GetPlayers(Player).FIncome - upkeep;
                if (NetIncome >= 0)
                {
                    DialogHosts.TBWindow.WGSign.Text = "+";
                    DialogHosts.TBWindow.WGSign.TextColor = 7864183;
                    DialogHosts.TBWindow.WGILbl.TextColor = 7864183;
                }
                else
                {
                    DialogHosts.TBWindow.WGSign.Text = "-";
                    DialogHosts.TBWindow.WGSign.TextColor = 119;
                    DialogHosts.TBWindow.WGILbl.TextColor = 119;
                }

                DialogHosts.TBWindow.WGILbl.Text = IntToStr(Math.Abs(NetIncome));
                if (DialogHosts.RealmWin.Dlg.Visible && DialogHosts.RealmWin.HeaderTab.Selected == 0)
                {
                    DialogHosts.RealmWin.UpdateOverview();
                }
            }

            DialogHosts.TBWindow.TGILbl.Text = DialogHosts.TBWindow.WGILbl.Text;
            DialogHosts.TBWindow.TGSign.Text = DialogHosts.TBWindow.WGSign.Text;
            DialogHosts.TBWindow.TGSign.TextColor = DialogHosts.TBWindow.WGSign.TextColor;
            DialogHosts.TBWindow.TGILbl.TextColor = DialogHosts.TBWindow.WGILbl.TextColor;
        }
    }

    // aowMapEvents.pas:362
    public void TheMapMapChanged(ref MapChangedInfo Info)
    {
        if (Info.MapLevel == DialogHosts.SWindow.Scanner.FLevel)
        {
            DialogHosts.SWindow.Scanner.DrawMap(Info.Bounds.Left - 6, Info.Bounds.Top - 6, Info.Bounds.Right - Info.Bounds.Left, Info.Bounds.Bottom - Info.Bounds.Top);
        }
    }

    // aowMapEvents.pas:370
    public void TheMapScannerIconsChanged(ref MapChangedInfo Info)
    {
        if (Info.MapLevel == DialogHosts.SWindow.Scanner.FLevel)
        {
            DialogHosts.SWindow.Scanner.SetObDrawn(false);
        }
    }

    // aowMapEvents.pas:375
    public void TheMapPlayerMagicChanged(sbyte Player)
    {
        bool Update;
        int NetIncome;
        MagicPlayerChanged(DialogHosts.MagicWin, Player);
        DialogHosts.SpellBook.PlayerChanged(Player);
        Update = false;
        if (AoWHSMap.Current.FSeatedPlayer == Player)
        {
            Update = true;
        }
        else
        {
            if (AoWHSMap.Current.FCombatMap != null)
            {
                if (AoWHSMap.Current.FCombatMap.FSeatedPlayer == Player)
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
                if (AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerType != PlayerType.Human)
                {
                    Update = false;
                }
            }

            if (!Update)
            {
                DialogHosts.TBWindow.WMTLbl.Text = "";
                DialogHosts.TBWindow.WMILbl.Text = "";
            }
            else
            {
                var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                DialogHosts.TBWindow.WMTLbl.Text = IntToStr(player.FMagicControl.FMana);
                NetIncome = AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.GetManaIncome() - AoWHSMap.Current.FPlayers.GetPlayers(Player).FMagicControl.GetManaUpkeep();
                if (NetIncome >= 0)
                {
                    DialogHosts.TBWindow.WMTSign.Text = "+";
                    DialogHosts.TBWindow.WMTSign.TextColor = 7864183;
                    DialogHosts.TBWindow.WMILbl.TextColor = 7864183;
                }
                else
                {
                    DialogHosts.TBWindow.WMTSign.Text = "-";
                    DialogHosts.TBWindow.WMTSign.TextColor = 119;
                    DialogHosts.TBWindow.WMILbl.TextColor = 119;
                }

                DialogHosts.TBWindow.WMILbl.Text = IntToStr(Math.Abs(NetIncome));
            }

            DialogHosts.TBWindow.TMTLbl.Text = DialogHosts.TBWindow.WMTLbl.Text;
            DialogHosts.TBWindow.TMILbl.Text = DialogHosts.TBWindow.WMILbl.Text;
            DialogHosts.TBWindow.TMTSign.Text = DialogHosts.TBWindow.WMTSign.Text;
            DialogHosts.TBWindow.TMTSign.TextColor = DialogHosts.TBWindow.WMTSign.TextColor;
            DialogHosts.TBWindow.TMILbl.TextColor = DialogHosts.TBWindow.WMILbl.TextColor;
        }
    }

    // aowMapEvents.pas:1021
    public void TheMapLogbookChanged(sbyte Player)
    {
        if (Player == AoWHSMap.Current.FSeatedPlayer)
        {
            DialogHosts.PInfoW.UpdateEvents();
        }
    }

    // aowMapEvents.pas:1026
    public void TheMapPlayerTerminated(sbyte Player)
    {
        DialogHosts.TBWindow.UpdatePlayerIndicator();
        if (AoWHSMap.Current != null)
            if (AoWHSMap.Current.FConnectionType == PlayerConnectionType.Network)
                if (Player == AoWHSMap.Current.FSeatedPlayer)
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
            DialogHosts.General.HideWindows();
            DialogHosts.General.Manager.OverlayPriority = 0;
            DialogHosts.TBWindow.CurP.Visible = false;
            DialogHosts.TBWindow.FWorldClockDelay = 0;
            DialogHosts.TBWindow.TurnButton.FCurPlayer = 12;
            DialogHosts.TBWindow.TurnButton.FLastPlayer = DialogHosts.TBWindow.TurnButton.FCurPlayer;
            DialogHosts.TBWindow.TurnButton.FAnimationFrame = DialogHosts.TBWindow.TurnButton.FCurPlayer * 7;
        }
        else
        {
            DialogHosts.General.ShowWindows();
            var aowTurnButton = DialogHosts.TBWindow.TurnButton;
            if (AoWHSMap.Current.Status.Contains(HSMapStatusElement.Activated) && !aowTurnButton.FAnimating)
            {
                if (Player > -1)
                {
                    aowTurnButton.FCurPlayer = AoWHSMap.Current.FPlayers.GetPlayers(Player).FPlayerRace;
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

            DialogHosts.TBWindow.CurP.Visible = AoWHSMap.Current.FPlayMode == PlayMode.Turn;
            DialogHosts.PInfoW.UpdatePlayer();
            if (DialogHosts.RealmWin.Dlg.Visible)
            {
                DialogHosts.RealmWin.UpdateOverview();
            }
        }

        TheMapGemsChanged(Player);
        TheMapIncomeChanged(Player);
        TheMapPlayerMagicChanged(Player);
        DialogHosts.TBWindow.PInd.FDrawn = false;
        DialogHosts.PInfoW.UpdateEvents();
    }

    // aowMapEvents.pas:440
    public void TheMapShowMessage(ref ShowMessageInfo Info)
    {
        DialogHosts.TBWindow.TBMessEdit.TextColor = Info.Color;
        DialogHosts.TBWindow.TBMessEdit.Text = Info.Text;
    }

    // aowMapEvents.pas:446
    public void TheMapStructureChanged(Structure Structure)
    {
        UpdateStructureInfo(DialogHosts.IWindow, Structure);
        if (Structure != null)
        {
            DialogHosts.CityScreen.CityChanged(Structure);
            DialogHosts.ProductionScreen.PlaceChanged(Structure);
        }

        if (DialogHosts.RealmWin.Dlg.Visible && DialogHosts.RealmWin.HeaderTab.Selected < 3)
        {
            DialogHosts.RealmWin.UpdateOverview();
        }
    }

    // aowMapEvents.pas:457
    public void TheMapStructureDeselected()
    {
        if (DialogHosts.General.FSign != null)
        {
            DialogHosts.General.FSign.ChangeText(AoWHSMap.Current.FSeatedPlayer, DialogHosts.IWindow.SignText.Items);
        }

        DialogHosts.General.FStructure = null;
        DialogHosts.General.FCity = null;
        DialogHosts.General.FProductionPlace = null;
        DialogHosts.General.FProductionControl = null;
        DialogHosts.General.FSign = null;
        DialogHosts.IWindow.StructNameLbl.Text = "";
        DialogHosts.IWindow.StructLine.Visible = false;
        DialogHosts.IWindow.InfoWin.Visible = false;
        DialogHosts.TBWindow.StructButton.Visible = false;
    }

    // aowMapEvents.pas:467
    public void TheMapStructureDestroyed(Structure Structure)
    {
        if (Structure != null && Structure == DialogHosts.General.FStructure)
        {
            TheMapStructureDeselected();
        }

        if (DialogHosts.RealmWin.Dlg.Visible)
        {
            if (DialogHosts.RealmWin.HeaderTab.Selected == 2 || DialogHosts.RealmWin.HeaderTab.Selected == 1)
            {
                DialogHosts.RealmWin.UpdateOverview();
            }
        }
    }

    // aowMapEvents.pas:473
    public void TheMapStructureSelected(Structure Structure)
    {
        DialogHosts.General.FPreviousStructure = DialogHosts.General.FStructure;
        DialogHosts.General.FStructure = Structure;
        DialogHosts.General.FProductionControl = null;
        ShowStructureInfo(DialogHosts.IWindow, Structure);
    }

    // aowMapEvents.pas:481
    public void TheMapItemsDeselected()
    {
        HideItemSelection(DialogHosts.IWindow);
    }

    // aowMapEvents.pas:484
    public void TheMapItemsSelected(ItemHS Items)
    {
        ShowItemSelection(DialogHosts.IWindow, Items);
    }

    // aowMapEvents.pas:487
    public void TheMapTargetSelectionChange(TargetSelectionControl Control, TargetSelectionChange Change)
    {
        ShowTargetSelection(DialogHosts.TargetSelectionDlg, Control, Change);
    }

    // aowMapEvents.pas:490
    public void TheMapUnitChanged(AbstractUnit U)
    {
        if (U == DialogHosts.General.FSelectedUnit)
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
    public void TheMapCastSpell(SpellCaster Caster)
    {
        if (Caster is UnitSpellCaster)
        {
            ShowUnitSpellCaster(DialogHosts.CastUnitSpellDlg, (UnitSpellCaster)Caster);
        }
        else
        {
            if (Caster is DisjunctionSpellCaster)
            {
                DialogHosts.DisjunctionDlg.Execute((DisjunctionSpellCaster)Caster);
            }
            else
            {
                if (Caster is CosmeticSurgerySpellCaster)
                {
                    DialogHosts.DeformFaceDlg.Execute((CosmeticSurgerySpellCaster)Caster);
                }
            }
        }
    }

    // aowMapEvents.pas:507
    public void TheMapHSChanged(HexagonSprite HS)
    {
        if (HS is ItemHS)
        {
            UpdateItemSelection(DialogHosts.IWindow, (ItemHS)HS);
        }
    }

    // aowMapEvents.pas:512
    public void TheMapRelationsChanged()
    {
        RefreshRelations(DialogHosts.RelWindow);
    }

    // aowMapEvents.pas:515
    public void TheMapDiplomaticChange()
    {
        RefreshRelations(DialogHosts.RelWindow);
    }

    // aowMapEvents.pas:518
    public void TheMapStructureDblClick(Structure Structure)
    {
        if (Structure == DialogHosts.General.FStructure && Structure != null)
        {
            if (StructureScreens.TryGet(Structure.DoubleClick(AoWHSMap.Current.FSeatedPlayer), out var open))
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
            DialogHosts.General.SaveLayout();
            DialogHosts.General.HideWindows();
            DialogHosts.MWindow.MapViewer.HSMap = null;
            if (DialogHosts.SetupEvents.FLobbyGame)
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

    private static void ShowStructureInfo(IWindow Dialog, Structure Structure)
    {
        IWindow.StructurePanels[Structure.InfoPanel(AoWHSMap.Current.FSeatedPlayer)].Invoke(Dialog, Structure);
        if (!DialogHosts.TBWindow.StructButton.Visible)
        {
            DialogHosts.IWindow.InfoWin.Visible = true;
        }
        DialogHosts.TBWindow.StructButton.Visible = true;
    }

    private static void UpdateStructureInfo(IWindow Dialog, Structure Structure)
    {
        if (!Dialog.InfoWin.Visible)
        {
            return;
        }
        if (Structure == null)
        {
            Structure = DialogHosts.General.FStructure;
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

    private static void ShowItemSelection(IWindow Dialog, ItemHS Items)
    {
        ResetStructureInfo(Dialog);
        string text = AbilityRules.TranslateRStr(ItemBagRStr);
        Dialog.Title.Text = text;
        Dialog.FItemBag = Items;
        Dialog.ItemPnl.Visible = true;
        Dialog.InfoWin.Visible = true;
        Dialog.StructNameLbl.Text = "";
        LibraryImage imageTop = DialogHosts.IntGfxMod.MiscArt[35];
        Dialog.StructLine.ImageTop = imageTop;
        LibraryImage imageMiddle = DialogHosts.IntGfxMod.MiscArt[36];
        Dialog.StructLine.ImageMiddle = imageMiddle;
        LibraryImage imageBottom = DialogHosts.IntGfxMod.MiscArt[37];
        Dialog.StructLine.ImageBottom = imageBottom;
        ArmyHS armyHS = ((AoWMapField)Items.Owner).GetArmyHS();
        if (armyHS != null && armyHS.FArmy.FPlayer == AoWHSMap.Current.FSeatedPlayer)
        {
            Dialog.ItemBagMemo.Visible = false;
            Hero tHero = armyHS.FArmy.Hero(255, RosterTables.AllHeroTypes);
            Dialog.MainBtn.Visible = tHero != null;
            string text2 = AbilityRules.TranslateRStr(PickupRStr);
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

    private static void UpdateItemSelection(IWindow Dialog, ItemHS Items)
    {
        if (Dialog.InfoWin.Visible && Items == Dialog.FItemBag)
        {
            ShowItemSelection(Dialog, Dialog.FItemBag);
        }
    }

    internal static void ResetStructureInfo(IWindow Dialog)
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
        string text = AbilityRules.TranslateRStr(GameText.StructureRStr);
        Dialog.Title.Text = text;
    }

    private static void ShowTargetSelection(TargetSelectionDlg Dialog, TargetSelectionControl Control, TargetSelectionChange Change)
    {
        switch (Change)
        {
            case TargetSelectionChange.Activated:
                {
                    Dialog.HeaderLbl.Text = Control.FTitle;
                    Dialog.MainBtn.Text = Control.FCancelText;
                    ((Persistent)Dialog.DefaultMemo.Items).Assign((Persistent)(object)Control.FText);
                    Dialog.Dlg.Visible = true;
                    if (!Control.FTargetMode.Contains(TargetSelectionModeElement.ValidateDistance))
                    {
                        string text = AbilityRules.TranslateRStr(UnlimitedRStr);
                        Dialog.TargetLbl.Text = RtlUnit.LowerCase(text);
                    }
                    else
                    {
                        Dialog.TargetLbl.Text = RtlUnit.IntToStr(Control.FRange);
                    }
                    Dialog.BorderImage.ImageTop = Control.FSourceImage;
                    Dialog.BorderImage.ImageMiddle = Control.FValidTargetImage;
                    Dialog.BorderImage.ImageBottom = Control.FInvalidTargetImage;
                    DialogHosts.General.Manager.OverlayPriority = 49;
                    AOWWindow mapWindow = DialogHosts.MWindow.MapWindow;
                    mapWindow.FModal = true;
                    mapWindow.Priority = 500;
                    DialogHosts.SWindow.ScannerWindow.Priority = Dialog.Dlg.Priority;
                    break;
                }
            case TargetSelectionChange.Deactivated:
                Dialog.Dlg.Visible = false;
                DialogHosts.MWindow.MapWindow.Priority = 0;
                DialogHosts.MWindow.MapWindow.FModal = false;
                DialogHosts.SWindow.ScannerWindow.Priority = 10;
                DialogHosts.General.Manager.OverlayPriority = 0;
                break;
        }
    }

    private static void ShowUnitSpellCaster(CastUnitSpellDlg Dialog, UnitSpellCaster Caster)
    {
        if (Dialog.FCaster != null)
        {
            Dialog.FCaster.Release();
        }
        Dialog.FCaster = Caster;
        Dialog.FCaster.AddRef();
        Hero tHero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(Dialog.FCaster.FCasterSourceID);
        if (tHero != null)
        {
            string name = tHero.GetName();
            Dialog.CasterNameLbl.Text = name;
            sbyte castingPoints = tHero.GetCastingPoints();
            string text = AbilityRules.TranslateRStr(GameText.CastingPointsRStr);
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
        Spell spell = AoWHSSet.Current.FSpellControl.GetSpell(Dialog.FCaster.FCasterSpellID);
        Dialog.SpellNameLbl.Text = spell.FSpellName;
        int num = tHero.CastingMana(spell);
        string text2 = AbilityRules.TranslateRStr(GameText.CastingCostRStr);
        string text3 = AbilityRules.TranslateRStr(GameText.ManaRStr);
        Dialog.SpellManaLbl.Text = string.Concat(new string[5]
        {
            text2,
            ": ",
            RtlUnit.IntToStr(num),
            " ",
            text3
        });
        string text4 = AbilityRules.TranslateRStr(UpkeepRStr);
        string text5 = AbilityRules.TranslateRStr(GameText.ManaRStr);
        Dialog.SpellUpkeepLbl.Text = string.Concat(new string[5]
        {
            text4,
            ": ",
            RtlUnit.IntToStr(spell.FManaUpkeep),
            " ",
            text5
        });
        Player players = AoWHSMap.Current.FPlayers.GetPlayers(tHero.FPlayer);
        string text6 = AbilityRules.TranslateRStr(GameText.ManaRStr);
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

    private static void RefreshRelations(RelWindow Dialog)
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

    private static void HideItemSelection(IWindow Window)
    {
        Window.InfoWin.Visible = false;
        Window.FItemBag = null;
    }

    private static void MagicPlayerChanged(MagicWin Window, sbyte Player)
    {
        if (Window.Dlg.Visible && Player == AoWHSMap.Current.FSeatedPlayer)
        {
            MagicWin.RefreshMagicWindow(Window);
        }
    }

    private static void GlobalMagicChanged(MagicWin Window)
    {
        if (Window.Dlg.Visible)
        {
            MagicWin.RefreshGlobalMagic(Window);
        }
    }
}
