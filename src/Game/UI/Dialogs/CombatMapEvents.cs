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

public partial class CombatMapEvents : DataModule
{
    public CombatMapEvents(Component owner) : base(owner)
    { }

    private int _chatHeight;

    // aowTCMapEvents.pas:105
    public void CombatMapDeactivateMap()
    {
        StopSong();
        AudioHost.SongPlayer.Resume();
        DialogHosts.MapWindow.MapViewer.HexSpriteMap = StrategicMap.Current;
        DialogHosts.ScannerWindow.Scanner.SetMap(StrategicMap.Current);
        DialogHosts.ScannerWindow.Scanner.PreviousWidth = 0;
        DialogHosts.ScannerWindow.Scanner.Incremental = false;
        DialogHosts.ScannerWindow.Scanner.Sized = false;
        DialogHosts.ScannerWindow.Scanner.UpdateStep = -1;

        if (_chatHeight > 0)
        {
            DialogHosts.ChatWindow.MainWindow.WinHeight = _chatHeight;
        }

        var chatWindow = DialogHosts.ChatWindow;
        chatWindow.MainWindow.Alignment.AlignHeight = AlignHeight.Bottom;
        chatWindow.MainWindow.Visible = false;

        DialogHosts.ChatWindow.ScrollPending = true;
        DialogHosts.MapWindow.MapAlignWidth = DialogHosts.MapWindow.MainWindow.Alignment.AlignWidth;
        DialogHosts.MapWindow.MapAlignHeight = DialogHosts.MapWindow.MainWindow.Alignment.AlignHeight;
        DialogHosts.MapWindow.MapRectangle.Left = DialogHosts.MapWindow.MainWindow.Alignment.LeftOffset;
        DialogHosts.MapWindow.MapRectangle.Top = DialogHosts.MapWindow.MainWindow.Alignment.TopOffset;
        if (DialogHosts.MapWindow.MapAlignWidth == AlignWidth.Both)
        {
            DialogHosts.MapWindow.MapRectangle.Right = DialogHosts.MapWindow.MainWindow.Alignment.RightOffset;
        }
        else
        {
            DialogHosts.MapWindow.MapRectangle.Right = DialogHosts.MapWindow.MainWindow.WinWidth;
        }

        if (DialogHosts.MapWindow.MapAlignHeight == AlignHeight.Both)
        {
            DialogHosts.MapWindow.MapRectangle.Bottom = DialogHosts.MapWindow.MainWindow.Alignment.BottomOffset;
        }
        else
        {
            DialogHosts.MapWindow.MapRectangle.Bottom = DialogHosts.MapWindow.MainWindow.WinHeight;
        }

        DialogHosts.MapWindow.MainWindow.Alignment.LeftOffset = DialogHosts.MapWindow.WorldRectangle.Left;
        DialogHosts.MapWindow.MainWindow.Alignment.TopOffset = DialogHosts.MapWindow.WorldRectangle.Top;
        DialogHosts.MapWindow.MainWindow.Alignment.RightOffset = DialogHosts.MapWindow.WorldRectangle.Right;
        DialogHosts.MapWindow.MainWindow.Alignment.BottomOffset = DialogHosts.MapWindow.WorldRectangle.Bottom;
        DialogHosts.MapWindow.MainWindow.WinWidth = DialogHosts.MapWindow.WorldRectangle.Right;
        DialogHosts.MapWindow.MainWindow.WinHeight = DialogHosts.MapWindow.WorldRectangle.Bottom;
        DialogHosts.MapWindow.MainWindow.Alignment.AlignWidth = DialogHosts.MapWindow.WorldAlignWidth;
        DialogHosts.MapWindow.MainWindow.Alignment.AlignHeight = DialogHosts.MapWindow.WorldAlignHeight;
        DialogHosts.General.ShowWindows();
        DialogHosts.General.Manager.Aligned = false;
        DialogHosts.TaskBar.WorldPanel.Visible = true;
        DialogHosts.TaskBar.CombatPanel.Visible = false;
        var player = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer);
        DialogHosts.TaskBar.WorldTurnButton.ButtonChecked = player.SingleProcessCycle;
        DialogHosts.CombatScanner.CombatScannerWindow.Visible = false;
        DialogHosts.CombatUnitInfo.MainWindow.Visible = false;
        DialogHosts.CombatScanner.Scanner.SetMap(null);
        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.MapEvents.TheMapGemsChanged(StrategicMap.Current.SeatedPlayer);
        DialogHosts.MapEvents.TheMapIncomeChanged(StrategicMap.Current.SeatedPlayer);
        DialogHosts.MapEvents.TheMapPlayerMagicChanged(StrategicMap.Current.SeatedPlayer);
    }

    // aowTCMapEvents.pas:157
    public void TCMapDeselected(HexSprite hexSprite)
    {
        if (hexSprite is TacticalBattleUnitHexSprite)
        {
            DialogHosts.General.CombatUnit = null;
            DialogHosts.General.SelectedUnit = null;
            DialogHosts.UnitWindow.MainWindow.Visible = false;
            DialogHosts.TaskBar.CombatUnitButton.Visible = false;
            DialogHosts.CombatUnitInfo.MainWindow.Visible = false;
        }
    }

    // aowTCMapEvents.pas:169
    public void CombatMapMapChanged(in MapChangedInfo R)
    {
        DialogHosts.CombatScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);
    }

    // aowTCMapEvents.pas:174
    public void CombatMapMouseOverTarget(HexSprite hexSprite)
    {
        Troop U;
        if (DialogHosts.CombatUnitInfo.MainWindow.Visible)
        {
            DialogHosts.CombatUnitInfo.TargetSelectPanel.Visible = hexSprite != null;
            DialogHosts.CombatUnitInfo.NoTargetPanel.Visible = hexSprite == null;
            if (hexSprite != null)
            {
                if (hexSprite is TacticalBattleUnitHexSprite)
                {
                    U = ((TacticalBattleUnitHexSprite)hexSprite).Unit.Troop;
                    var name = U.GetName();
                    DialogHosts.CombatUnitInfo.TargetUnitLabel.Text = name;
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    DialogHosts.CombatUnitInfo.TargetHitsLabel.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    DialogHosts.CombatUnitInfo.TargetMovesLabel.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                    var attack = U.GetAttack();
                    DialogHosts.CombatUnitInfo.TargetAttackLabel.Text = IntToStr(attack);
                    var damage = U.GetDamage();
                    DialogHosts.CombatUnitInfo.TargetDamageLabel.Text = IntToStr(damage);
                    var defense = U.GetDefense();
                    DialogHosts.CombatUnitInfo.TargetDefenseLabel.Text = IntToStr(defense);
                    var resistance = U.GetResistance();
                    DialogHosts.CombatUnitInfo.TargetResistanceLabel.Text = IntToStr(resistance);
                    var moveTypes = U.GetMoveTypes();
                    var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
                    DialogHosts.CombatUnitInfo.TargetMovesIcon.SetCurrent(moveTypesToInt32);
                    U.ListAbilities(DialogHosts.CombatUnitInfo.TargetAbilityList.Items);
                }
            }
        }
    }

    // aowTCMapEvents.pas:199
    public void CombatMapSeatedPlayerChanged(sbyte player)
    {
        DialogHosts.MapWindow.MainWindow.Visible = CombatMap.Current.PlaySounds;
        DialogHosts.CombatScanner.CombatScannerWindow.Visible = CombatMap.Current.PlaySounds;
        if (CombatMap.Current.SeatedCombatPlayer == -1)
        {
            DialogHosts.TaskBar.AutoCombatCheckBox.Checked = false;
        }
        else
        {
            var playerLocal = CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer);
            DialogHosts.TaskBar.AutoCombatCheckBox.Checked = playerLocal.Auto;
        }

        DialogHosts.MapEvents.TheMapGemsChanged(player);
        DialogHosts.MapEvents.TheMapIncomeChanged(player);
        DialogHosts.MapEvents.TheMapPlayerMagicChanged(player);
    }

    // aowTCMapEvents.pas:210
    public void TCMapSelected(HexSprite hexSprite)
    {
        if (hexSprite is TacticalBattleUnitHexSprite)
        {
            DialogHosts.General.CombatUnit = (TacticalBattleUnitHexSprite)hexSprite;
            RefreshCombatUnit(DialogHosts.CombatUnitInfo);
        }
    }

    // aowTCMapEvents.pas:219
    public void CombatMapShowMessage(in ShowMessageInfo info)
    {
        DialogHosts.TaskBar.CombatMessageEdit.TextColor = info.Color;
        DialogHosts.TaskBar.CombatMessageEdit.Text = info.Text;
    }

    // aowTCMapEvents.pas:225
    public void CombatMapUnitChanged(TacticalBattleUnitHexSprite hexSprite)
    {
        if (hexSprite == DialogHosts.General.CombatUnit && hexSprite != null)
        {
            RefreshCombatUnit(DialogHosts.CombatUnitInfo);
        }
    }

    // aowTCMapEvents.pas:230
    public void CombatMapCastSpell(CombatSpellCaster caster)
    {
        var selectSpell = DialogHosts.General.CombatUnit.SelectSpell(caster.CombatCasterSpellId);
        DialogHosts.CombatUnitInfo.UnitAbilityList.Selected = selectSpell;
    }

    // aowTCMapEvents.pas:235
    public void CombatMapActivateAbility(AbilityActivation info)
    {
        if (info.AbilityId == 52)
        {
            DialogHosts.SpellBook.ShowCombatCasting(info);
        }
    }

    // aowTCMapEvents.pas:240
    public void CombatMapTurnChanged()
    {
        var player = StrategicMap.Current.Players.GetPlayers(CombatMap.Current.CurrentPlayer);
        DialogHosts.TaskBar.CombatTurnButton.CurrentPlayer = player.PlayerRace;
        if (DialogHosts.TaskBar.CombatTurnButton.CurrentPlayer == -1)
        {
            DialogHosts.TaskBar.CombatTurnButton.CurrentPlayer = 12;
        }

        DialogHosts.TaskBar.CombatTurnButton.LastPlayer = -1;

        DialogHosts.General.Manager.OverlayPriority = 49;
        DialogHosts.TaskBar.CombatPlayerIndicatorFrame.Drawn = false;
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
        if (DialogHosts.General.CombatUnit != null)
        {
            Troop fTroop = DialogHosts.General.CombatUnit.Unit.Troop;
            Hero hero = ((!(fTroop is Hero)) ? null : ((Hero)fTroop));
            if (!window.MainWindow.Visible)
            {
                window.UnitManaGauge.Visible = false;
                DialogHosts.TaskBar.CombatUnitButton.Visible = true;
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
                if (DialogHosts.General.CombatUnit.SelectedAbility == -1)
                {
                    window.CombatUnitPanel.Visible = true;
                    window.CombatAbilityPanel.Visible = false;
                    window.TargetSelectPanel.Visible = false;
                    sbyte hitPoints = fTroop.GetHitPoints();
                    sbyte hits = fTroop.GetHits();
                    window.UnitHitsLabel.Text = RtlUnit.IntToStr((int)hitPoints) + "/" + RtlUnit.IntToStr((int)hits);
                    sbyte moves = fTroop.GetMoves();
                    window.UnitMovesLabel.Text = RtlUnit.IntToStr(DialogHosts.General.CombatUnit.Unit.MovePoints) + "/" + RtlUnit.IntToStr((int)moves);
                    sbyte attack = fTroop.GetAttack();
                    window.UnitAttackLabel.Text = RtlUnit.IntToStr((int)attack);
                    sbyte damage = fTroop.GetDamage();
                    window.UnitDamageLabel.Text = RtlUnit.IntToStr((int)damage);
                    sbyte defense = fTroop.GetDefense();
                    window.UnitDefenseLabel.Text = RtlUnit.IntToStr((int)defense);
                    sbyte resistance = fTroop.GetResistance();
                    window.UnitResistanceLabel.Text = RtlUnit.IntToStr((int)resistance);
                    Set<MoveType> moveTypes = fTroop.GetMoveTypes();
                    int current = DialogHosts.General.MoveTypesToInt32(moveTypes);
                    window.UnitMovesIcon.SetCurrent(current);
                    DialogHosts.General.CombatUnit.ListAbilities((StringCollection)(object)window.UnitAbilityList.Items, null, null);
                    window.UnitAbilityList.Selected = -1;
                    window.UnitAbilityList.HoverItem = -1;
                    window.UnitAbilityList.ListOffset = 0;
                }
                else if (DialogHosts.General.CombatUnit.SelectedAbility == 52 && DialogHosts.General.CombatUnit.SelectedSpell > -1)
                {
                    window.CombatUnitPanel.Visible = false;
                    window.CombatAbilityPanel.Visible = true;
                    window.TargetSelectPanel.Visible = false;
                    GamePack.Current.SpellControl.GetSpell(DialogHosts.General.CombatUnit.SelectedSpell).GetCombatInfo(DialogHosts.General.CombatUnit.Unit.Troop, ref info);
                    Spell spell = GamePack.Current.SpellControl.GetSpell(DialogHosts.General.CombatUnit.SelectedSpell);
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
                        string text = AbilityRules.TranslateResourceString(GameTables.FireText);
                        string text2 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text + " " + text2);
                    }
                    if (info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text3 = AbilityRules.TranslateResourceString(s_coldText);
                        string text4 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text3 + " " + text4);
                    }
                    if (info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text5 = AbilityRules.TranslateResourceString(s_lightningText);
                        string text6 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text5 + " " + text6);
                    }
                    if (info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text7 = AbilityRules.TranslateResourceString(s_magicText);
                        string text8 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text7 + " " + text8);
                    }
                    if (info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text9 = AbilityRules.TranslateResourceString(s_poisonText);
                        string text10 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text9 + " " + text10);
                    }
                    if (info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text11 = AbilityRules.TranslateResourceString(GameTables.DeathText);
                        string text12 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text11 + " " + text12);
                    }
                    if (info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text13 = AbilityRules.TranslateResourceString(s_holyText);
                        string text14 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text13 + " " + text14);
                    }
                    if (info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text15 = AbilityRules.TranslateResourceString(s_physicalText);
                        string text16 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text15 + " " + text16);
                    }
                    if (info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text17 = AbilityRules.TranslateResourceString(s_wallText);
                        string text18 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text17 + " " + text18);
                    }
                }
                else
                {
                    window.CombatUnitPanel.Visible = false;
                    window.CombatAbilityPanel.Visible = true;
                    window.TargetSelectPanel.Visible = false;
                    GamePack.Current.AbilityControl.GetAbility(DialogHosts.General.CombatUnit.SelectedAbility).GetCombatInfo(DialogHosts.General.CombatUnit.Unit.Troop, ref Info);
                    string name = GamePack.Current.AbilityControl.GetAbility(DialogHosts.General.CombatUnit.SelectedAbility).GetName(DialogHosts.General.CombatUnit.Unit.Troop);
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
                        string text19 = AbilityRules.TranslateResourceString(GameTables.FireText);
                        string text20 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text19 + " " + text20);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text21 = AbilityRules.TranslateResourceString(s_coldText);
                        string text22 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text21 + " " + text22);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text23 = AbilityRules.TranslateResourceString(s_lightningText);
                        string text24 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text23 + " " + text24);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text25 = AbilityRules.TranslateResourceString(s_magicText);
                        string text26 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text25 + " " + text26);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text27 = AbilityRules.TranslateResourceString(s_poisonText);
                        string text28 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text27 + " " + text28);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text29 = AbilityRules.TranslateResourceString(GameTables.DeathText);
                        string text30 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text29 + " " + text30);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text31 = AbilityRules.TranslateResourceString(s_holyText);
                        string text32 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text31 + " " + text32);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text33 = AbilityRules.TranslateResourceString(s_physicalText);
                        string text34 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilityStatisticsList.Items).Add(text33 + " " + text34);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text35 = AbilityRules.TranslateResourceString(s_wallText);
                        string text36 = AbilityRules.TranslateResourceString(s_damageText);
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
        DialogHosts.CombatUnitInfo.MainWindow.Visible = true;
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
