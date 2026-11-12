// Translated from aowTCMapEvents.pas.
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;

using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class CombatMapEvents : UIModule
{
    public CombatMapEvents(UIContext ui) : base(ui)
    { }

    private int _chatHeight;

    // aowTCMapEvents.pas:105
    public void CombatMapDeactivateMap()
    {
        StopSong();
        AudioHost.SongPlayer.Resume();
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

        var chatWindow = UI.ChatWindow;
        chatWindow.MainWindow.Alignment.AlignHeight = AlignHeight.Bottom;
        chatWindow.MainWindow.Visible = false;

        UI.ChatWindow.ScrollPending = true;
        UI.MapWindow.MapAlignWidth = UI.MapWindow.MainWindow.Alignment.AlignWidth;
        UI.MapWindow.MapAlignHeight = UI.MapWindow.MainWindow.Alignment.AlignHeight;
        UI.MapWindow.MapRectangle.Left = UI.MapWindow.MainWindow.Alignment.LeftOffset;
        UI.MapWindow.MapRectangle.Top = UI.MapWindow.MainWindow.Alignment.TopOffset;
        if (UI.MapWindow.MapAlignWidth == AlignWidth.Both)
        {
            UI.MapWindow.MapRectangle.Right = UI.MapWindow.MainWindow.Alignment.RightOffset;
        }
        else
        {
            UI.MapWindow.MapRectangle.Right = UI.MapWindow.MainWindow.WinWidth;
        }

        if (UI.MapWindow.MapAlignHeight == AlignHeight.Both)
        {
            UI.MapWindow.MapRectangle.Bottom = UI.MapWindow.MainWindow.Alignment.BottomOffset;
        }
        else
        {
            UI.MapWindow.MapRectangle.Bottom = UI.MapWindow.MainWindow.WinHeight;
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
        var player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
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
    public void TCMapDeselected(HexSprite hexSprite)
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
    public void CombatMapMapChanged(in MapChangedInfo R)
    {
        UI.CombatScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);
    }

    // aowTCMapEvents.pas:174
    public void CombatMapMouseOverTarget(HexSprite hexSprite)
    {
        Troop U;
        if (UI.CombatUnitInfo.MainWindow.Visible)
        {
            UI.CombatUnitInfo.TargetSelectPanel.Visible = hexSprite != null;
            UI.CombatUnitInfo.NoTargetPanel.Visible = hexSprite == null;
            if (hexSprite != null)
            {
                if (hexSprite is TacticalBattleUnitHexSprite)
                {
                    U = ((TacticalBattleUnitHexSprite)hexSprite).Unit.Troop;
                    var name = U.GetName();
                    UI.CombatUnitInfo.TargetUnitLabel.Text = name;
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    UI.CombatUnitInfo.TargetHitsLabel.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    UI.CombatUnitInfo.TargetMovesLabel.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                    var attack = U.GetAttack();
                    UI.CombatUnitInfo.TargetAttackLabel.Text = IntToStr(attack);
                    var damage = U.GetDamage();
                    UI.CombatUnitInfo.TargetDamageLabel.Text = IntToStr(damage);
                    var defense = U.GetDefense();
                    UI.CombatUnitInfo.TargetDefenseLabel.Text = IntToStr(defense);
                    var resistance = U.GetResistance();
                    UI.CombatUnitInfo.TargetResistanceLabel.Text = IntToStr(resistance);
                    var moveTypes = U.GetMoveTypes();
                    var moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                    UI.CombatUnitInfo.TargetMovesIcon.SetCurrent(moveTypesToInt32);
                    U.ListAbilities(UI.CombatUnitInfo.TargetAbilityList.Items);
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
            var playerLocal = CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer);
            UI.TaskBar.AutoCombatCheckBox.Checked = playerLocal.Auto;
        }

        UI.MapEvents.TheMapGemsChanged(player);
        UI.MapEvents.TheMapIncomeChanged(player);
        UI.MapEvents.TheMapPlayerMagicChanged(player);
    }

    // aowTCMapEvents.pas:210
    public void TCMapSelected(HexSprite hexSprite)
    {
        if (hexSprite is TacticalBattleUnitHexSprite)
        {
            UI.General.CombatUnit = (TacticalBattleUnitHexSprite)hexSprite;
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
    public void CombatMapUnitChanged(TacticalBattleUnitHexSprite hexSprite)
    {
        if (hexSprite == UI.General.CombatUnit && hexSprite != null)
        {
            RefreshCombatUnit(UI.CombatUnitInfo);
        }
    }

    // aowTCMapEvents.pas:230
    public void CombatMapCastSpell(CombatSpellCaster caster)
    {
        var selectSpell = UI.General.CombatUnit.SelectSpell(caster.CombatCasterSpellId);
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
        var player = StrategicMap.Current.Players.GetPlayers(CombatMap.Current.CurrentPlayer);
        UI.TaskBar.CombatTurnButton.CurrentPlayer = player.PlayerRace;
        if (UI.TaskBar.CombatTurnButton.CurrentPlayer == -1)
        {
            UI.TaskBar.CombatTurnButton.CurrentPlayer = 12;
        }

        UI.TaskBar.CombatTurnButton.LastPlayer = -1;

        UI.General.Manager.OverlayPriority = 49;
        UI.TaskBar.CombatPlayerIndicatorFrame.Drawn = false;
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
        AbilityCombatInfo Info = default(AbilityCombatInfo);
        SpellCombatInfo info = default(SpellCombatInfo);
        if (window.UI.General.CombatUnit != null)
        {
            Troop fTroop = window.UI.General.CombatUnit.Unit.Troop;
            Hero hero = ((!(fTroop is Hero)) ? null : ((Hero)fTroop));
            if (!window.MainWindow.Visible)
            {
                window.UnitManaGauge.Visible = false;
                window.UI.TaskBar.CombatUnitButton.Visible = true;
            }
            if (hero != null)
            {
                if (hero.GetCastingPointsMax() > 0)
                {
                    window.UnitManaGauge.Visible = true;
                    sbyte castingPointsMax = hero.GetCastingPointsMax();
                    window.UnitManaGauge.Max = castingPointsMax;
                    sbyte castingPoints = hero.GetCastingPoints();
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
            if (fTroop != null)
            {
                if (window.UI.General.CombatUnit.SelectedAbility == -1)
                {
                    window.CombatUnitPanel.Visible = true;
                    window.CombatAbilityPanel.Visible = false;
                    window.TargetSelectPanel.Visible = false;
                    sbyte hitPoints = fTroop.GetHitPoints();
                    sbyte hits = fTroop.GetHits();
                    window.UnitHitsLabel.Text = RtlUnit.IntToStr((int)hitPoints) + "/" + RtlUnit.IntToStr((int)hits);
                    sbyte moves = fTroop.GetMoves();
                    window.UnitMovesLabel.Text = RtlUnit.IntToStr(window.UI.General.CombatUnit.Unit.MovePoints) + "/" + RtlUnit.IntToStr((int)moves);
                    sbyte attack = fTroop.GetAttack();
                    window.UnitAttackLabel.Text = RtlUnit.IntToStr((int)attack);
                    sbyte damage = fTroop.GetDamage();
                    window.UnitDamageLabel.Text = RtlUnit.IntToStr((int)damage);
                    sbyte defense = fTroop.GetDefense();
                    window.UnitDefenseLabel.Text = RtlUnit.IntToStr((int)defense);
                    sbyte resistance = fTroop.GetResistance();
                    window.UnitResistanceLabel.Text = RtlUnit.IntToStr((int)resistance);
                    Set<MoveType> moveTypes = fTroop.GetMoveTypes();
                    int current = window.UI.General.MoveTypesToInt32(moveTypes);
                    window.UnitMovesIcon.SetCurrent(current);
                    window.UI.General.CombatUnit.ListAbilities((StringCollection)(object)window.UnitAbilityList.Items, null, null);
                    window.UnitAbilityList.Selected = -1;
                    window.UnitAbilityList.HoverItem = -1;
                    window.UnitAbilityList.ListOffset = 0;
                }
                else if (window.UI.General.CombatUnit.SelectedAbility == 52 && window.UI.General.CombatUnit.SelectedSpell > -1)
                {
                    window.CombatUnitPanel.Visible = false;
                    window.CombatAbilityPanel.Visible = true;
                    window.TargetSelectPanel.Visible = false;
                    GamePack.Current.SpellControl.GetSpell(window.UI.General.CombatUnit.SelectedSpell).GetCombatInfo(window.UI.General.CombatUnit.Unit.Troop, ref info);
                    Spell spell = GamePack.Current.SpellControl.GetSpell(window.UI.General.CombatUnit.SelectedSpell);
                    window.AbilityNameLabel.Text = spell.SpellName;
                    if (info.Attack > 0)
                    {
                        window.AbilityAttackLabel.Text = RtlUnit.IntToStr((int)info.Attack);
                    }
                    else
                    {
                        window.AbilityAttackLabel.Text = "-";
                    }
                    if (info.Damage > 0)
                    {
                        window.AbilityDamageLabel.Text = RtlUnit.IntToStr((int)info.Damage);
                    }
                    else
                    {
                        window.AbilityDamageLabel.Text = "-";
                    }
                    ((StringCollection)window.AbilityStatisticsList.Items).Clear();
                    if (info.DamageTypes.Contains(DamageType.Fire))
                    {
                        string text = window.UI.Engine.TranslateResourceString(GameTables.FireText);
                        string text2 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text + " " + text2);
                    }
                    if (info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text3 = window.UI.Engine.TranslateResourceString(s_coldText);
                        string text4 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text3 + " " + text4);
                    }
                    if (info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text5 = window.UI.Engine.TranslateResourceString(s_lightningText);
                        string text6 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text5 + " " + text6);
                    }
                    if (info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text7 = window.UI.Engine.TranslateResourceString(s_magicText);
                        string text8 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text7 + " " + text8);
                    }
                    if (info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text9 = window.UI.Engine.TranslateResourceString(s_poisonText);
                        string text10 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text9 + " " + text10);
                    }
                    if (info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text11 = window.UI.Engine.TranslateResourceString(GameTables.DeathText);
                        string text12 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text11 + " " + text12);
                    }
                    if (info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text13 = window.UI.Engine.TranslateResourceString(s_holyText);
                        string text14 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text13 + " " + text14);
                    }
                    if (info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text15 = window.UI.Engine.TranslateResourceString(s_physicalText);
                        string text16 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text15 + " " + text16);
                    }
                    if (info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text17 = window.UI.Engine.TranslateResourceString(s_wallText);
                        string text18 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text17 + " " + text18);
                    }
                }
                else
                {
                    window.CombatUnitPanel.Visible = false;
                    window.CombatAbilityPanel.Visible = true;
                    window.TargetSelectPanel.Visible = false;
                    GamePack.Current.AbilityControl.GetAbility(window.UI.General.CombatUnit.SelectedAbility).GetCombatInfo(window.UI.General.CombatUnit.Unit.Troop, ref Info);
                    string name = GamePack.Current.AbilityControl.GetAbility(window.UI.General.CombatUnit.SelectedAbility).GetName(window.UI.General.CombatUnit.Unit.Troop);
                    window.AbilityNameLabel.Text = name;
                    if (Info.Attack > 0)
                    {
                        window.AbilityAttackLabel.Text = RtlUnit.IntToStr((int)Info.Attack);
                    }
                    else
                    {
                        window.AbilityAttackLabel.Text = "-";
                    }
                    if (Info.Damage > 0)
                    {
                        window.AbilityDamageLabel.Text = RtlUnit.IntToStr((int)Info.Damage);
                    }
                    else
                    {
                        window.AbilityDamageLabel.Text = "-";
                    }
                    ((StringCollection)window.AbilityStatisticsList.Items).Clear();
                    if (Info.DamageTypes.Contains(DamageType.Fire))
                    {
                        string text19 = window.UI.Engine.TranslateResourceString(GameTables.FireText);
                        string text20 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text19 + " " + text20);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text21 = window.UI.Engine.TranslateResourceString(s_coldText);
                        string text22 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text21 + " " + text22);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text23 = window.UI.Engine.TranslateResourceString(s_lightningText);
                        string text24 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text23 + " " + text24);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text25 = window.UI.Engine.TranslateResourceString(s_magicText);
                        string text26 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text25 + " " + text26);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text27 = window.UI.Engine.TranslateResourceString(s_poisonText);
                        string text28 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text27 + " " + text28);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text29 = window.UI.Engine.TranslateResourceString(GameTables.DeathText);
                        string text30 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text29 + " " + text30);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text31 = window.UI.Engine.TranslateResourceString(s_holyText);
                        string text32 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text31 + " " + text32);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text33 = window.UI.Engine.TranslateResourceString(s_physicalText);
                        string text34 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text33 + " " + text34);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text35 = window.UI.Engine.TranslateResourceString(s_wallText);
                        string text36 = window.UI.Engine.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text35 + " " + text36);
                    }
                }
                ((StringCollection)window.UnitNameMemo.Items).Clear();
                string name2 = fTroop.GetName();
                ((StringCollection)window.UnitNameMemo.Items).Add(name2);
                sbyte hits2 = fTroop.GetHits();
                window.UnitHitsGauge.Max = hits2;
                sbyte hitPoints2 = fTroop.GetHitPoints();
                window.UnitHitsGauge.Position = hitPoints2;
            }
        }
        window.UI.CombatUnitInfo.MainWindow.Visible = true;
    }

    private static bool StopSong()
    {
        if (AudioHost.SoundSystem != null)
        {
            return AudioHost.SoundSystem.StopSong();
        }
        return false;
    }
}
