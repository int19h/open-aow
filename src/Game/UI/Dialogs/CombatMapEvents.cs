#nullable disable warnings
// Translated from aowTCMapEvents.pas.
using System.Globalization;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;

using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class CombatMapEvents(UIContext ui) : UIModule(ui)
{
    private int _chatHeight;

    // aowTCMapEvents.pas:105
    public void CombatMapDeactivateMap()
    {
        _ = StopSong();
        _ = AudioHost.SongPlayer.Resume();
        UI.MapWindow.MapViewer.HexSpriteMap = StrategicMap.Current;
        UI.ScannerWindow.Scanner.SetMap(StrategicMap.Current);
        UI.ScannerWindow.Scanner.PreviousWidth = 0;
        UI.ScannerWindow.Scanner.Incremental = false;
        UI.ScannerWindow.Scanner.Sized = false;
        UI.ScannerWindow.Scanner.UpdateStep = -1;

        if (_chatHeight > 0)
        {
            UI.ChatWindow.MainWindow.WinHeight = _chatHeight;
        }

        ChatWindow chatWindow = UI.ChatWindow;
        chatWindow.MainWindow.Alignment.AlignHeight = AlignHeight.Bottom;
        chatWindow.MainWindow.Visible = false;

        UI.ChatWindow.ScrollPending = true;
        UI.MapWindow.MapAlignWidth = UI.MapWindow.MainWindow.Alignment.AlignWidth;
        UI.MapWindow.MapAlignHeight = UI.MapWindow.MainWindow.Alignment.AlignHeight;
        Rect mapRectangle = UI.MapWindow.MapRectangle;
        mapRectangle.Left = UI.MapWindow.MainWindow.Alignment.LeftOffset;
        UI.MapWindow.MapRectangle = mapRectangle;
        Rect mapRectangle2 = UI.MapWindow.MapRectangle;
        mapRectangle2.Top = UI.MapWindow.MainWindow.Alignment.TopOffset;
        UI.MapWindow.MapRectangle = mapRectangle2;
        if (UI.MapWindow.MapAlignWidth == AlignWidth.Both)
        {
            Rect mapRectangle3 = UI.MapWindow.MapRectangle;
            mapRectangle3.Right = UI.MapWindow.MainWindow.Alignment.RightOffset;
            UI.MapWindow.MapRectangle = mapRectangle3;
        }
        else
        {
            Rect mapRectangle4 = UI.MapWindow.MapRectangle;
            mapRectangle4.Right = UI.MapWindow.MainWindow.WinWidth;
            UI.MapWindow.MapRectangle = mapRectangle4;
        }

        if (UI.MapWindow.MapAlignHeight == AlignHeight.Both)
        {
            Rect mapRectangle5 = UI.MapWindow.MapRectangle;
            mapRectangle5.Bottom = UI.MapWindow.MainWindow.Alignment.BottomOffset;
            UI.MapWindow.MapRectangle = mapRectangle5;
        }
        else
        {
            Rect mapRectangle6 = UI.MapWindow.MapRectangle;
            mapRectangle6.Bottom = UI.MapWindow.MainWindow.WinHeight;
            UI.MapWindow.MapRectangle = mapRectangle6;
        }

        UI.MapWindow.MainWindow.Alignment.LeftOffset = UI.MapWindow.WorldRectangle.Left;
        UI.MapWindow.MainWindow.Alignment.TopOffset = UI.MapWindow.WorldRectangle.Top;
        UI.MapWindow.MainWindow.Alignment.RightOffset = UI.MapWindow.WorldRectangle.Right;
        UI.MapWindow.MainWindow.Alignment.BottomOffset = UI.MapWindow.WorldRectangle.Bottom;
        UI.MapWindow.MainWindow.WinWidth = UI.MapWindow.WorldRectangle.Right;
        UI.MapWindow.MainWindow.WinHeight = UI.MapWindow.WorldRectangle.Bottom;
        UI.MapWindow.MainWindow.Alignment.AlignWidth = UI.MapWindow.WorldAlignWidth;
        UI.MapWindow.MainWindow.Alignment.AlignHeight = UI.MapWindow.WorldAlignHeight;
        UI.General.ShowWindows();
        UI.General.Manager.Aligned = false;
        UI.TaskBar.WorldPanel.Visible = true;
        UI.TaskBar.CombatPanel.Visible = false;
        Player? player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        UI.TaskBar.WorldTurnButton.ButtonChecked = player.SingleProcessCycle;
        UI.CombatScanner.CombatScannerWindow.Visible = false;
        UI.CombatUnitInfo.MainWindow.Visible = false;
        UI.CombatScanner.Scanner.SetMap(null);
        UI.General.Manager.OverlayPriority = 0;
        UI.MapEvents.TheMapGemsChanged(StrategicMap.Current.SeatedPlayer);
        UI.MapEvents.TheMapIncomeChanged(StrategicMap.Current.SeatedPlayer);
        UI.MapEvents.TheMapPlayerMagicChanged(StrategicMap.Current.SeatedPlayer);
    }

    // aowTCMapEvents.pas:157
    public void TCMapDeselected(HexSprite? hexSprite)
    {
        if (hexSprite is TacticalBattleUnitHexSprite)
        {
            UI.General.CombatUnit = null;
            UI.General.SelectedUnit = null;
            UI.UnitWindow.MainWindow.Visible = false;
            UI.TaskBar.CombatUnitButton.Visible = false;
            UI.CombatUnitInfo.MainWindow.Visible = false;
        }
    }

    // aowTCMapEvents.pas:169
    public void CombatMapMapChanged(in MapChangedInfo R) => UI.CombatScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);

    // aowTCMapEvents.pas:174
    public void CombatMapMouseOverTarget(HexSprite? hexSprite)
    {
        if (UI.CombatUnitInfo.MainWindow.Visible)
        {
            UI.CombatUnitInfo.TargetSelectPanel.Visible = hexSprite is not null;
            UI.CombatUnitInfo.NoTargetPanel.Visible = hexSprite is null;
            if (hexSprite is not null)
            {
                if (hexSprite is TacticalBattleUnitHexSprite unitHexSprite)
                {
                    Troop? u = unitHexSprite.Unit.Troop;
                    string name = u.GetName();
                    UI.CombatUnitInfo.TargetUnitLabel.Text = name;
                    int hitPoints = u.GetHitPoints();
                    int hits = u.GetHits();
                    UI.CombatUnitInfo.TargetHitsLabel.Text = $"{hitPoints.ToString(CultureInfo.InvariantCulture)}/{hits.ToString(CultureInfo.InvariantCulture)}";
                    int movePoints = u.GetMovePoints();
                    int moves = u.Moves;
                    UI.CombatUnitInfo.TargetMovesLabel.Text = $"{movePoints.ToString(CultureInfo.InvariantCulture)}/{moves.ToString(CultureInfo.InvariantCulture)}";
                    int attack = u.GetAttack();
                    UI.CombatUnitInfo.TargetAttackLabel.Text = attack.ToString(CultureInfo.InvariantCulture);
                    int damage = u.GetDamage();
                    UI.CombatUnitInfo.TargetDamageLabel.Text = damage.ToString(CultureInfo.InvariantCulture);
                    int defense = u.GetDefense();
                    UI.CombatUnitInfo.TargetDefenseLabel.Text = defense.ToString(CultureInfo.InvariantCulture);
                    int resistance = u.GetResistance();
                    UI.CombatUnitInfo.TargetResistanceLabel.Text = resistance.ToString(CultureInfo.InvariantCulture);
                    MoveTypes moveTypes = u.GetMoveTypes();
                    int moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                    UI.CombatUnitInfo.TargetMovesIcon.SetCurrent(moveTypesToInt32);
                    u.ListAbilities(UI.CombatUnitInfo.TargetAbilityList.Items);
                }
            }
        }
    }

    // aowTCMapEvents.pas:199
    public void CombatMapSeatedPlayerChanged(sbyte player)
    {
        UI.MapWindow.MainWindow.Visible = CombatMap.Current.PlaySounds;
        UI.CombatScanner.CombatScannerWindow.Visible = CombatMap.Current.PlaySounds;
        if (CombatMap.Current.SeatedCombatPlayer == -1)
        {
            UI.TaskBar.AutoCombatCheckBox.Checked = false;
        }
        else
        {
            TacticalBattlePlayer playerLocal = CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer);
            UI.TaskBar.AutoCombatCheckBox.Checked = playerLocal.Auto;
        }

        UI.MapEvents.TheMapGemsChanged(player);
        UI.MapEvents.TheMapIncomeChanged(player);
        UI.MapEvents.TheMapPlayerMagicChanged(player);
    }

    // aowTCMapEvents.pas:210
    public void TCMapSelected(HexSprite? hexSprite)
    {
        if (hexSprite is TacticalBattleUnitHexSprite unitHexSprite)
        {
            UI.General.CombatUnit = unitHexSprite;
            RefreshCombatUnit(UI.CombatUnitInfo);
        }
    }

    // aowTCMapEvents.pas:219
    public void CombatMapShowMessage(in ShowMessageInfo info)
    {
        UI.TaskBar.CombatMessageEdit.TextColor = info.Color;
        UI.TaskBar.CombatMessageEdit.Text = info.Text;
    }

    // aowTCMapEvents.pas:225
    public void CombatMapUnitChanged(TacticalBattleUnitHexSprite? hexSprite)
    {
        if (hexSprite == UI.General.CombatUnit && hexSprite is not null)
        {
            RefreshCombatUnit(UI.CombatUnitInfo);
        }
    }

    // aowTCMapEvents.pas:230
    public void CombatMapCastSpell(CombatSpellCaster caster)
    {
        int selectSpell = UI.General.CombatUnit.SelectSpell(caster.CombatCasterSpellId);
        UI.CombatUnitInfo.UnitAbilityList.Selected = selectSpell;
    }

    // aowTCMapEvents.pas:235
    public void CombatMapActivateAbility(AbilityActivation info)
    {
        if (info.AbilityId == 52)
        {
            UI.SpellBook.ShowCombatCasting(info);
        }
    }

    // aowTCMapEvents.pas:240
    public void CombatMapTurnChanged()
    {
        Player? player = StrategicMap.Current.Players.GetPlayers(CombatMap.Current.CurrentPlayer);
        UI.TaskBar.CombatTurnButton.CurrentPlayer = player.PlayerRace;
        if (UI.TaskBar.CombatTurnButton.CurrentPlayer == -1)
        {
            UI.TaskBar.CombatTurnButton.CurrentPlayer = 12;
        }

        UI.TaskBar.CombatTurnButton.LastPlayer = -1;

        UI.General.Manager.OverlayPriority = 49;
        UI.TaskBar.CombatPlayerIndicatorFrame.IsDrawn = false;
    }

    private static readonly string s_coldText = "Cold";
    private static readonly string s_holyText = "Holy";
    private static readonly string s_lightningText = "Lightning";
    private static readonly string s_magicText = "Magic";
    private static readonly string s_physicalText = "Physical";
    private static readonly string s_poisonText = "Poison";
    private static readonly string s_wallText = "Wall";
    private static readonly string s_damageText = "Damage";

    private static void RefreshCombatUnit(CombatUnitInfo window)
    {
        if (window.UI.General.CombatUnit is not null)
        {
            Troop? fTroop = window.UI.General.CombatUnit.Unit.Troop;
            Hero? hero = fTroop is Hero faceTroop ? faceTroop : null;
            if (!window.MainWindow.Visible)
            {
                window.UnitManaGauge.Visible = false;
                window.UI.TaskBar.CombatUnitButton.Visible = true;
            }
            if (hero is not null)
            {
                if (hero.GetCastingPointsMax() > 0)
                {
                    window.UnitManaGauge.Visible = true;
                    int castingPointsMax = hero.GetCastingPointsMax();
                    window.UnitManaGauge.Max = castingPointsMax;
                    int castingPoints = hero.GetCastingPoints();
                    window.UnitManaGauge.Position = castingPoints;
                }
                Panel unitFrame = window.UnitFramePanel;
                unitFrame.Alignment.TopOffset = 7;
                unitFrame.WinHeight = 54;
            }
            else
            {
                Panel unitFrame2 = window.UnitFramePanel;
                unitFrame2.Alignment.TopOffset = 0;
                unitFrame2.WinHeight = 61;
            }
            window.UnitFramePanel.Aligned = false;
            if (fTroop is not null)
            {
                if (window.UI.General.CombatUnit.SelectedAbility == -1)
                {
                    window.CombatUnitPanel.Visible = true;
                    window.CombatAbilityPanel.Visible = false;
                    window.TargetSelectPanel.Visible = false;
                    int hitPoints = fTroop.GetHitPoints();
                    int hits = fTroop.GetHits();
                    window.UnitHitsLabel.Text = $"{hitPoints.ToString(CultureInfo.InvariantCulture)}/{hits.ToString(CultureInfo.InvariantCulture)}";
                    int moves = fTroop.Moves;
                    window.UnitMovesLabel.Text = $"{window.UI.General.CombatUnit.Unit.MovePoints.ToString(CultureInfo.InvariantCulture)}/{moves.ToString(CultureInfo.InvariantCulture)}";
                    int attack = fTroop.GetAttack();
                    window.UnitAttackLabel.Text = attack.ToString(CultureInfo.InvariantCulture);
                    int damage = fTroop.GetDamage();
                    window.UnitDamageLabel.Text = damage.ToString(CultureInfo.InvariantCulture);
                    int defense = fTroop.GetDefense();
                    window.UnitDefenseLabel.Text = defense.ToString(CultureInfo.InvariantCulture);
                    int resistance = fTroop.GetResistance();
                    window.UnitResistanceLabel.Text = resistance.ToString(CultureInfo.InvariantCulture);
                    MoveTypes moveTypes = fTroop.GetMoveTypes();
                    int current = window.UI.General.MoveTypesToInt32(moveTypes);
                    window.UnitMovesIcon.SetCurrent(current);
                    window.UI.General.CombatUnit.ListAbilities((DelphiStringCollection)(object)window.UnitAbilityList.Items, types: null, sources: null);
                    window.UnitAbilityList.Selected = -1;
                    window.UnitAbilityList.HoverItem = -1;
                    window.UnitAbilityList.ListOffset = 0;
                }
                else if (window.UI.General.CombatUnit.SelectedAbility == 52 && window.UI.General.CombatUnit.SelectedSpell > -1)
                {
                    window.CombatUnitPanel.Visible = false;
                    window.CombatAbilityPanel.Visible = true;
                    window.TargetSelectPanel.Visible = false;
                    SpellCombatInfo info = default;
                    GamePack.Current.SpellControl[window.UI.General.CombatUnit.SelectedSpell].GetCombatInfo(window.UI.General.CombatUnit.Unit.Troop, ref info);
                    Spell? spell = GamePack.Current.SpellControl[window.UI.General.CombatUnit.SelectedSpell];
                    window.AbilityNameLabel.Text = spell.SpellName;
                    window.AbilityAttackLabel.Text = info.Attack > 0 ? ((int)info.Attack).ToString(CultureInfo.InvariantCulture) : "-";
                    window.AbilityDamageLabel.Text = info.Damage > 0 ? ((int)info.Damage).ToString(CultureInfo.InvariantCulture) : "-";
                    window.AbilityStatisticsList.Items.Clear();
                    if (info.DamageTypes.Contains(DamageTypes.Fire))
                    {
                        string text = window.UI.Engine.TranslateResourceString(GameTables.FireText);
                        string text2 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text} {text2}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Cold))
                    {
                        string text3 = window.UI.Engine.TranslateResourceString(s_coldText);
                        string text4 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text3} {text4}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Lightning))
                    {
                        string text5 = window.UI.Engine.TranslateResourceString(s_lightningText);
                        string text6 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text5} {text6}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Magic))
                    {
                        string text7 = window.UI.Engine.TranslateResourceString(s_magicText);
                        string text8 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text7} {text8}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Poison))
                    {
                        string text9 = window.UI.Engine.TranslateResourceString(s_poisonText);
                        string text10 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text9} {text10}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Death))
                    {
                        string text11 = window.UI.Engine.TranslateResourceString(GameTables.DeathText);
                        string text12 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text11} {text12}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Holy))
                    {
                        string text13 = window.UI.Engine.TranslateResourceString(s_holyText);
                        string text14 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text13} {text14}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Physical))
                    {
                        string text15 = window.UI.Engine.TranslateResourceString(s_physicalText);
                        string text16 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text15} {text16}");
                    }
                    if (info.DamageTypes.Contains(DamageTypes.Wall))
                    {
                        string text17 = window.UI.Engine.TranslateResourceString(s_wallText);
                        string text18 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text17} {text18}");
                    }
                }
                else
                {
                    window.CombatUnitPanel.Visible = false;
                    window.CombatAbilityPanel.Visible = true;
                    window.TargetSelectPanel.Visible = false;
                    AbilityCombatInfo abilityCombatInfo = default;
                    GamePack.Current.AbilityControl[window.UI.General.CombatUnit.SelectedAbility].GetCombatInfo(window.UI.General.CombatUnit.Unit.Troop, info: ref abilityCombatInfo);
                    string name = GamePack.Current.AbilityControl[window.UI.General.CombatUnit.SelectedAbility].GetName(window.UI.General.CombatUnit.Unit.Troop);
                    window.AbilityNameLabel.Text = name;
                    window.AbilityAttackLabel.Text = abilityCombatInfo.Attack > 0 ? ((int)abilityCombatInfo.Attack).ToString(CultureInfo.InvariantCulture) : "-";
                    window.AbilityDamageLabel.Text = abilityCombatInfo.Damage > 0 ? ((int)abilityCombatInfo.Damage).ToString(CultureInfo.InvariantCulture) : "-";
                    window.AbilityStatisticsList.Items.Clear();
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Fire))
                    {
                        string text19 = window.UI.Engine.TranslateResourceString(GameTables.FireText);
                        string text20 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text19} {text20}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Cold))
                    {
                        string text21 = window.UI.Engine.TranslateResourceString(s_coldText);
                        string text22 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text21} {text22}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Lightning))
                    {
                        string text23 = window.UI.Engine.TranslateResourceString(s_lightningText);
                        string text24 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text23} {text24}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Magic))
                    {
                        string text25 = window.UI.Engine.TranslateResourceString(s_magicText);
                        string text26 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text25} {text26}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Poison))
                    {
                        string text27 = window.UI.Engine.TranslateResourceString(s_poisonText);
                        string text28 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text27} {text28}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Death))
                    {
                        string text29 = window.UI.Engine.TranslateResourceString(GameTables.DeathText);
                        string text30 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text29} {text30}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Holy))
                    {
                        string text31 = window.UI.Engine.TranslateResourceString(s_holyText);
                        string text32 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text31} {text32}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Physical))
                    {
                        string text33 = window.UI.Engine.TranslateResourceString(s_physicalText);
                        string text34 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text33} {text34}");
                    }
                    if (abilityCombatInfo.DamageTypes.Contains(DamageTypes.Wall))
                    {
                        string text35 = window.UI.Engine.TranslateResourceString(s_wallText);
                        string text36 = window.UI.Engine.TranslateResourceString(s_damageText);
                        _ = window.AbilityStatisticsList.Items.Add($"{text35} {text36}");
                    }
                }
                window.UnitNameMemo.Items.Clear();
                string name2 = fTroop.GetName();
                _ = window.UnitNameMemo.Items.Add(name2);
                int hits2 = fTroop.GetHits();
                window.UnitHitsGauge.Max = hits2;
                int hitPoints2 = fTroop.GetHitPoints();
                window.UnitHitsGauge.Position = hitPoints2;
            }
        }
        window.UI.CombatUnitInfo.MainWindow.Visible = true;
    }

    private static bool StopSong() => AudioHost.SoundSystem is not null && AudioHost.SoundSystem.StopSong();
}
