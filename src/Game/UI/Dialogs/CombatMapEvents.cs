// Translated from aowTCMapEvents.pas.
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
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
    public void TCMapDeActivateMap()
    {
        StopSong();
        AudioHost.SongPlayer.Resume();
        DialogHosts.MapWindow.MapViewer.HexSpriteMap = AoWHSMap.Current;
        DialogHosts.ScannerWindow.Scanner.SetMap(AoWHSMap.Current);
        DialogHosts.ScannerWindow.Scanner.PreviousWidth = 0;
        DialogHosts.ScannerWindow.Scanner.Incremental = false;
        DialogHosts.ScannerWindow.Scanner.Sized = false;
        DialogHosts.ScannerWindow.Scanner.UpdateStep = -1;

        if (_chatHeight > 0)
        {
            DialogHosts.ChatWindow.ChatWin.WinHeight = _chatHeight;
        }

        var chatWindow = DialogHosts.ChatWindow;
        chatWindow.ChatWin.Alignment.AlignHeight = AlignHeight.Bottom;
        chatWindow.ChatWin.Visible = false;

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
        DialogHosts.TaskBar.TBWorld.Visible = true;
        DialogHosts.TaskBar.TBCombat.Visible = false;
        var player = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer);
        DialogHosts.TaskBar.TurnButton.ButtonChecked = player.SingleProcessCycle;
        DialogHosts.CombatScanner.TCScannerWin.Visible = false;
        DialogHosts.CombatUnitInfo.TCUnitWin.Visible = false;
        DialogHosts.CombatScanner.Scanner.SetMap(null);
        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.MapEvents.TheMapGemsChanged(AoWHSMap.Current.SeatedPlayer);
        DialogHosts.MapEvents.TheMapIncomeChanged(AoWHSMap.Current.SeatedPlayer);
        DialogHosts.MapEvents.TheMapPlayerMagicChanged(AoWHSMap.Current.SeatedPlayer);
    }

    // aowTCMapEvents.pas:157
    public void TCMapDeselected(HexSprite hexSprite)
    {
        if (hexSprite is TacticalBattleUnitHexSprite)
        {
            DialogHosts.General.CombatUnit = null;
            DialogHosts.General.SelectedUnit = null;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
            DialogHosts.TaskBar.TCUnitBtn.Visible = false;
            DialogHosts.CombatUnitInfo.TCUnitWin.Visible = false;
        }
    }

    // aowTCMapEvents.pas:169
    public void TCMapMapChanged(in MapChangedInfo R)
    {
        DialogHosts.CombatScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);
    }

    // aowTCMapEvents.pas:174
    public void TCMapMouseOverTarget(HexSprite hexSprite)
    {
        Troop U;
        if (DialogHosts.CombatUnitInfo.TCUnitWin.Visible)
        {
            DialogHosts.CombatUnitInfo.TargetSelPnl.Visible = hexSprite != null;
            DialogHosts.CombatUnitInfo.NoTargetPnl.Visible = hexSprite == null;
            if (hexSprite != null)
            {
                if (hexSprite is TacticalBattleUnitHexSprite)
                {
                    U = ((TacticalBattleUnitHexSprite)hexSprite).Unit.Troop;
                    var name = U.GetName();
                    DialogHosts.CombatUnitInfo.TargetULbl.Text = name;
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    DialogHosts.CombatUnitInfo.AHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    DialogHosts.CombatUnitInfo.AMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                    var attack = U.GetAttack();
                    DialogHosts.CombatUnitInfo.AAttack.Text = IntToStr(attack);
                    var damage = U.GetDamage();
                    DialogHosts.CombatUnitInfo.ADamage.Text = IntToStr(damage);
                    var defense = U.GetDefense();
                    DialogHosts.CombatUnitInfo.ADefense.Text = IntToStr(defense);
                    var resistance = U.GetResistance();
                    DialogHosts.CombatUnitInfo.AResistance.Text = IntToStr(resistance);
                    var moveTypes = U.GetMoveTypes();
                    var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
                    DialogHosts.CombatUnitInfo.AMovesIcon.SetCurrent(moveTypesToInt32);
                    U.ListAbilities(DialogHosts.CombatUnitInfo.USelAbilLb.Items);
                }
            }
        }
    }

    // aowTCMapEvents.pas:199
    public void TCMapSeatedPlayerChanged(sbyte player)
    {
        DialogHosts.MapWindow.MainWindow.Visible = CombatMap.Current.PlaySounds;
        DialogHosts.CombatScanner.TCScannerWin.Visible = CombatMap.Current.PlaySounds;
        if (CombatMap.Current.SeatedCombatPlayer == -1)
        {
            DialogHosts.TaskBar.AutoBtn.Checked = false;
        }
        else
        {
            var playerLocal = CombatMap.Current.Players.GetPlayers(CombatMap.Current.SeatedCombatPlayer);
            DialogHosts.TaskBar.AutoBtn.Checked = playerLocal.Auto;
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
    public void TCMapShowMessage(in ShowMessageInfo info)
    {
        DialogHosts.TaskBar.TCMsg.TextColor = info.Color;
        DialogHosts.TaskBar.TCMsg.Text = info.Text;
    }

    // aowTCMapEvents.pas:225
    public void TCMapUnitChanged(TacticalBattleUnitHexSprite hexSprite)
    {
        if (hexSprite == DialogHosts.General.CombatUnit && hexSprite != null)
        {
            RefreshCombatUnit(DialogHosts.CombatUnitInfo);
        }
    }

    // aowTCMapEvents.pas:230
    public void TCMapCastSpell(CombatSpellCaster caster)
    {
        var selectSpell = DialogHosts.General.CombatUnit.SelectSpell(caster.CombatCasterSpellId);
        DialogHosts.CombatUnitInfo.UAbilLB.Selected = selectSpell;
    }

    // aowTCMapEvents.pas:235
    public void TCMapActivateAbility(AbilityActivation info)
    {
        if (info.AbilityId == 52)
        {
            DialogHosts.SpellBook.ShowCombatCasting(info);
        }
    }

    // aowTCMapEvents.pas:240
    public void TCMapTurnChanged()
    {
        var player = AoWHSMap.Current.Players.GetPlayers(CombatMap.Current.CurrentPlayer);
        DialogHosts.TaskBar.TCTurnButton.CurrentPlayer = player.PlayerRace;
        if (DialogHosts.TaskBar.TCTurnButton.CurrentPlayer == -1)
        {
            DialogHosts.TaskBar.TCTurnButton.CurrentPlayer = 12;
        }

        DialogHosts.TaskBar.TCTurnButton.LastPlayer = -1;

        DialogHosts.General.Manager.OverlayPriority = 49;
        DialogHosts.TaskBar.TCPInd.Drawn = false;
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
            if (!window.TCUnitWin.Visible)
            {
                window.UnitMana.Visible = false;
                DialogHosts.TaskBar.TCUnitBtn.Visible = true;
            }
            if (hero != null)
            {
                if (hero.GetCastingPointsMax() > 0)
                {
                    window.UnitMana.Visible = true;
                    sbyte castingPointsMax = hero.GetCastingPointsMax();
                    window.UnitMana.Max = castingPointsMax;
                    sbyte castingPoints = hero.GetCastingPoints();
                    window.UnitMana.Position = castingPoints;
                }
                Panel unitFrame = window.UnitFr;
                unitFrame.Alignment.TopOffset = 7;
                unitFrame.WinHeight = 54;
            }
            else
            {
                Panel unitFrame2 = window.UnitFr;
                unitFrame2.Alignment.TopOffset = 0;
                unitFrame2.WinHeight = 61;
            }
            window.UnitFr.Aligned = false;
            if (fTroop != null)
            {
                if (DialogHosts.General.CombatUnit.SelectedAbility == -1)
                {
                    window.TCUnitPnl.Visible = true;
                    window.TCAbilPnl.Visible = false;
                    window.TargetSelPnl.Visible = false;
                    sbyte hitPoints = fTroop.GetHitPoints();
                    sbyte hits = fTroop.GetHits();
                    window.UHits.Text = RtlUnit.IntToStr((int)hitPoints) + "/" + RtlUnit.IntToStr((int)hits);
                    sbyte moves = fTroop.GetMoves();
                    window.UMoves.Text = RtlUnit.IntToStr(DialogHosts.General.CombatUnit.Unit.MovePoints) + "/" + RtlUnit.IntToStr((int)moves);
                    sbyte attack = fTroop.GetAttack();
                    window.UAttack.Text = RtlUnit.IntToStr((int)attack);
                    sbyte damage = fTroop.GetDamage();
                    window.UDamage.Text = RtlUnit.IntToStr((int)damage);
                    sbyte defense = fTroop.GetDefense();
                    window.UDefense.Text = RtlUnit.IntToStr((int)defense);
                    sbyte resistance = fTroop.GetResistance();
                    window.UResistance.Text = RtlUnit.IntToStr((int)resistance);
                    Set<MoveType> moveTypes = fTroop.GetMoveTypes();
                    int current = DialogHosts.General.MoveTypesToInt32(moveTypes);
                    window.UMovesIcon.SetCurrent(current);
                    DialogHosts.General.CombatUnit.ListAbilities((StringCollection)(object)window.UAbilLB.Items, null, null);
                    window.UAbilLB.Selected = -1;
                    window.UAbilLB.HoverItem = -1;
                    window.UAbilLB.ListOffset = 0;
                }
                else if (DialogHosts.General.CombatUnit.SelectedAbility == 52 && DialogHosts.General.CombatUnit.SelectedSpell > -1)
                {
                    window.TCUnitPnl.Visible = false;
                    window.TCAbilPnl.Visible = true;
                    window.TargetSelPnl.Visible = false;
                    AoWHSSet.Current.SpellControl.GetSpell(DialogHosts.General.CombatUnit.SelectedSpell).GetCombatInfo(DialogHosts.General.CombatUnit.Unit.Troop, ref info);
                    Spell spell = AoWHSSet.Current.SpellControl.GetSpell(DialogHosts.General.CombatUnit.SelectedSpell);
                    window.AbilNameLbl.Text = spell.SpellName;
                    if (info.Attack > 0)
                    {
                        window.AbilAttLbl.Text = RtlUnit.IntToStr((int)info.Attack);
                    }
                    else
                    {
                        window.AbilAttLbl.Text = "-";
                    }
                    if (info.Damage > 0)
                    {
                        window.AbilDamLbl.Text = RtlUnit.IntToStr((int)info.Damage);
                    }
                    else
                    {
                        window.AbilDamLbl.Text = "-";
                    }
                    ((StringCollection)window.AbilStat.Items).Clear();
                    if (info.DamageTypes.Contains(DamageType.Fire))
                    {
                        string text = AbilityRules.TranslateResourceString(GameTables.FireText);
                        string text2 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text + " " + text2);
                    }
                    if (info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text3 = AbilityRules.TranslateResourceString(s_coldText);
                        string text4 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text3 + " " + text4);
                    }
                    if (info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text5 = AbilityRules.TranslateResourceString(s_lightningText);
                        string text6 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text5 + " " + text6);
                    }
                    if (info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text7 = AbilityRules.TranslateResourceString(s_magicText);
                        string text8 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text7 + " " + text8);
                    }
                    if (info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text9 = AbilityRules.TranslateResourceString(s_poisonText);
                        string text10 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text9 + " " + text10);
                    }
                    if (info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text11 = AbilityRules.TranslateResourceString(GameTables.DeathText);
                        string text12 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text11 + " " + text12);
                    }
                    if (info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text13 = AbilityRules.TranslateResourceString(s_holyText);
                        string text14 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text13 + " " + text14);
                    }
                    if (info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text15 = AbilityRules.TranslateResourceString(s_physicalText);
                        string text16 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text15 + " " + text16);
                    }
                    if (info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text17 = AbilityRules.TranslateResourceString(s_wallText);
                        string text18 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text17 + " " + text18);
                    }
                }
                else
                {
                    window.TCUnitPnl.Visible = false;
                    window.TCAbilPnl.Visible = true;
                    window.TargetSelPnl.Visible = false;
                    AoWHSSet.Current.AbilityControl.GetAbility(DialogHosts.General.CombatUnit.SelectedAbility).GetCombatInfo(DialogHosts.General.CombatUnit.Unit.Troop, ref Info);
                    string name = AoWHSSet.Current.AbilityControl.GetAbility(DialogHosts.General.CombatUnit.SelectedAbility).GetName(DialogHosts.General.CombatUnit.Unit.Troop);
                    window.AbilNameLbl.Text = name;
                    if (Info.Attack > 0)
                    {
                        window.AbilAttLbl.Text = RtlUnit.IntToStr((int)Info.Attack);
                    }
                    else
                    {
                        window.AbilAttLbl.Text = "-";
                    }
                    if (Info.Damage > 0)
                    {
                        window.AbilDamLbl.Text = RtlUnit.IntToStr((int)Info.Damage);
                    }
                    else
                    {
                        window.AbilDamLbl.Text = "-";
                    }
                    ((StringCollection)window.AbilStat.Items).Clear();
                    if (Info.DamageTypes.Contains(DamageType.Fire))
                    {
                        string text19 = AbilityRules.TranslateResourceString(GameTables.FireText);
                        string text20 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text19 + " " + text20);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text21 = AbilityRules.TranslateResourceString(s_coldText);
                        string text22 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text21 + " " + text22);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text23 = AbilityRules.TranslateResourceString(s_lightningText);
                        string text24 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text23 + " " + text24);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text25 = AbilityRules.TranslateResourceString(s_magicText);
                        string text26 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text25 + " " + text26);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text27 = AbilityRules.TranslateResourceString(s_poisonText);
                        string text28 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text27 + " " + text28);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text29 = AbilityRules.TranslateResourceString(GameTables.DeathText);
                        string text30 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text29 + " " + text30);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text31 = AbilityRules.TranslateResourceString(s_holyText);
                        string text32 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text31 + " " + text32);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text33 = AbilityRules.TranslateResourceString(s_physicalText);
                        string text34 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text33 + " " + text34);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text35 = AbilityRules.TranslateResourceString(s_wallText);
                        string text36 = AbilityRules.TranslateResourceString(s_damageText);
                        ((StringCollection)window.AbilStat.Items).Add(text35 + " " + text36);
                    }
                }
                ((StringCollection)window.UnitName.Items).Clear();
                string name2 = fTroop.GetName();
                ((StringCollection)window.UnitName.Items).Add(name2);
                sbyte hits2 = fTroop.GetHits();
                window.UnitHits.Max = hits2;
                sbyte hitPoints2 = fTroop.GetHitPoints();
                window.UnitHits.Position = hitPoints2;
            }
        }
        DialogHosts.CombatUnitInfo.TCUnitWin.Visible = true;
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
