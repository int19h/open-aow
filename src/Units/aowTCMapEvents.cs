// Translated from aowTCMapEvents.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class CombatMapEvents : DataModule
{
    public CombatMapEvents(Component owner) : base(owner)
    { }

    public AoWCombatMapControl TCMap;
    public int FChatHeight;

    // aowTCMapEvents.pas:105
    public void TCMapDeActivateMap()
    {
        StopSong();
        Audio.AudioHost.SongPlayer.Resume();
        DialogHosts.MWindow.MapViewer.HSMap = AoWHSMap.Current;
        DialogHosts.SWindow.Scanner.SetMap(AoWHSMap.Current);
        DialogHosts.SWindow.Scanner.FPreviousWidth = 0;
        DialogHosts.SWindow.Scanner.FIncremental = false;
        DialogHosts.SWindow.Scanner.FSized = false;
        DialogHosts.SWindow.Scanner.FUpdateStep = -1;

        if (FChatHeight > 0)
        {
            DialogHosts.ChatWindow.ChatWin.WinHeight = FChatHeight;
        }

        var chatWindow = DialogHosts.ChatWindow;
        chatWindow.ChatWin.Alignment.AlignHeight = AOWAlignHeight.Bottom;
        chatWindow.ChatWin.Visible = false;

        DialogHosts.ChatWindow.FScrollPending = true;
        DialogHosts.MWindow.FMapAlignWidth = DialogHosts.MWindow.MapWindow.Alignment.AlignWidth;
        DialogHosts.MWindow.FMapAlignHeight = DialogHosts.MWindow.MapWindow.Alignment.AlignHeight;
        DialogHosts.MWindow.FMapRect.Left = DialogHosts.MWindow.MapWindow.Alignment.LeftOffset;
        DialogHosts.MWindow.FMapRect.Top = DialogHosts.MWindow.MapWindow.Alignment.TopOffset;
        if (DialogHosts.MWindow.FMapAlignWidth == AOWAlignWidth.Both)
        {
            DialogHosts.MWindow.FMapRect.Right = DialogHosts.MWindow.MapWindow.Alignment.RightOffset;
        }
        else
        {
            DialogHosts.MWindow.FMapRect.Right = DialogHosts.MWindow.MapWindow.WinWidth;
        }

        if (DialogHosts.MWindow.FMapAlignHeight == AOWAlignHeight.Both)
        {
            DialogHosts.MWindow.FMapRect.Bottom = DialogHosts.MWindow.MapWindow.Alignment.BottomOffset;
        }
        else
        {
            DialogHosts.MWindow.FMapRect.Bottom = DialogHosts.MWindow.MapWindow.WinHeight;
        }

        DialogHosts.MWindow.MapWindow.Alignment.LeftOffset = DialogHosts.MWindow.FWorldRect.Left;
        DialogHosts.MWindow.MapWindow.Alignment.TopOffset = DialogHosts.MWindow.FWorldRect.Top;
        DialogHosts.MWindow.MapWindow.Alignment.RightOffset = DialogHosts.MWindow.FWorldRect.Right;
        DialogHosts.MWindow.MapWindow.Alignment.BottomOffset = DialogHosts.MWindow.FWorldRect.Bottom;
        DialogHosts.MWindow.MapWindow.WinWidth = DialogHosts.MWindow.FWorldRect.Right;
        DialogHosts.MWindow.MapWindow.WinHeight = DialogHosts.MWindow.FWorldRect.Bottom;
        DialogHosts.MWindow.MapWindow.Alignment.AlignWidth = DialogHosts.MWindow.FWorldAlignWidth;
        DialogHosts.MWindow.MapWindow.Alignment.AlignHeight = DialogHosts.MWindow.FWorldAlignHeight;
        DialogHosts.General.ShowWindows();
        DialogHosts.General.Manager.FAligned = false;
        DialogHosts.TBWindow.TBWorld.Visible = true;
        DialogHosts.TBWindow.TBCombat.Visible = false;
        var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer);
        DialogHosts.TBWindow.TurnButton.FButtonChecked = player.FSingleProcessCycle;
        DialogHosts.TCScanner.TCScannerWin.Visible = false;
        DialogHosts.TCUnit.TCUnitWin.Visible = false;
        DialogHosts.TCScanner.Scanner.SetMap(null);
        DialogHosts.General.Manager.OverlayPriority = 0;
        DialogHosts.MapEvents.TheMapGemsChanged(AoWHSMap.Current.FSeatedPlayer);
        DialogHosts.MapEvents.TheMapIncomeChanged(AoWHSMap.Current.FSeatedPlayer);
        DialogHosts.MapEvents.TheMapPlayerMagicChanged(AoWHSMap.Current.FSeatedPlayer);
    }

    // aowTCMapEvents.pas:157
    public void TCMapDeselected(HexagonSprite HS)
    {
        if (HS is TacticalCombatUnitHS)
        {
            DialogHosts.General.FCombatUnit = null;
            DialogHosts.General.FSelectedUnit = null;
            DialogHosts.UnitWindow.UnitWin.Visible = false;
            DialogHosts.TBWindow.TCUnitBtn.Visible = false;
            DialogHosts.TCUnit.TCUnitWin.Visible = false;
        }
    }

    // aowTCMapEvents.pas:169
    public void TCMapMapChanged(in MapChangedInfo R)
    {
        DialogHosts.TCScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);
    }

    // aowTCMapEvents.pas:174
    public void TCMapMouseOverTarget(HexagonSprite HS)
    {
        AbstractUnit U;
        if (DialogHosts.TCUnit.TCUnitWin.Visible)
        {
            DialogHosts.TCUnit.TargetSelPnl.Visible = HS != null;
            DialogHosts.TCUnit.NoTargetPnl.Visible = HS == null;
            if (HS != null)
            {
                if (HS is TacticalCombatUnitHS)
                {
                    U = ((TacticalCombatUnitHS)HS).FUnit.FUnit;
                    var name = U.GetName();
                    DialogHosts.TCUnit.TargetULbl.Text = name;
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    DialogHosts.TCUnit.AHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    DialogHosts.TCUnit.AMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                    var attack = U.GetAttack();
                    DialogHosts.TCUnit.AAttack.Text = IntToStr(attack);
                    var damage = U.GetDamage();
                    DialogHosts.TCUnit.ADamage.Text = IntToStr(damage);
                    var defense = U.GetDefense();
                    DialogHosts.TCUnit.ADefense.Text = IntToStr(defense);
                    var resistance = U.GetResistance();
                    DialogHosts.TCUnit.AResistance.Text = IntToStr(resistance);
                    var moveTypes = U.GetMoveTypes();
                    var mTtoInt = DialogHosts.General.MTtoInt(moveTypes);
                    DialogHosts.TCUnit.AMovesIcon.SetCurrent(mTtoInt);
                    U.ListAbilities(DialogHosts.TCUnit.USelAbilLb.Items);
                }
            }
        }
    }

    // aowTCMapEvents.pas:199
    public void TCMapSeatedPlayerChanged(sbyte Player)
    {
        DialogHosts.MWindow.MapWindow.Visible = AoWCombatMap.Current.FPlaySounds;
        DialogHosts.TCScanner.TCScannerWin.Visible = AoWCombatMap.Current.FPlaySounds;
        if (AoWCombatMap.Current.FSeatedCombatPlayer == -1)
        {
            DialogHosts.TBWindow.AutoBtn.Checked = false;
        }
        else
        {
            var player = AoWCombatMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FSeatedCombatPlayer);
            DialogHosts.TBWindow.AutoBtn.Checked = player.FAuto;
        }

        DialogHosts.MapEvents.TheMapGemsChanged(Player);
        DialogHosts.MapEvents.TheMapIncomeChanged(Player);
        DialogHosts.MapEvents.TheMapPlayerMagicChanged(Player);
    }

    // aowTCMapEvents.pas:210
    public void TCMapSelected(HexagonSprite HS)
    {
        if (HS is TacticalCombatUnitHS)
        {
            DialogHosts.General.FCombatUnit = (TacticalCombatUnitHS)HS;
            RefreshCombatUnit(DialogHosts.TCUnit);
        }
    }

    // aowTCMapEvents.pas:219
    public void TCMapShowMessage(in ShowMessageInfo Info)
    {
        DialogHosts.TBWindow.TCMsg.TextColor = Info.Color;
        DialogHosts.TBWindow.TCMsg.Text = Info.Text;
    }

    // aowTCMapEvents.pas:225
    public void TCMapUnitChanged(TacticalCombatUnitHS HS)
    {
        if (HS == DialogHosts.General.FCombatUnit && HS != null)
        {
            RefreshCombatUnit(DialogHosts.TCUnit);
        }
    }

    // aowTCMapEvents.pas:230
    public void TCMapCastSpell(CombatSpellCaster Caster)
    {
        var selectSpell = DialogHosts.General.FCombatUnit.SelectSpell(Caster.FCombatCasterSpellID);
        DialogHosts.TCUnit.UAbilLB.Selected = selectSpell;
    }

    // aowTCMapEvents.pas:235
    public void TCMapActivateAbility(ActivateAbilityInfo Info)
    {
        if (Info.FAbilityID == 52)
        {
            DialogHosts.SpellBook.ShowCombatCasting(Info);
        }
    }

    // aowTCMapEvents.pas:240
    public void TCMapTurnChanged()
    {
        var player = AoWHSMap.Current.FPlayers.GetPlayers(AoWCombatMap.Current.FCurrentPlayer);
        DialogHosts.TBWindow.TCTurnButton.FCurPlayer = player.FPlayerRace;
        if (DialogHosts.TBWindow.TCTurnButton.FCurPlayer == -1)
        {
            DialogHosts.TBWindow.TCTurnButton.FCurPlayer = 12;
        }

        DialogHosts.TBWindow.TCTurnButton.FLastPlayer = -1;

        DialogHosts.General.Manager.OverlayPriority = 49;
        DialogHosts.TBWindow.TCPInd.FDrawn = false;
    }

    private static readonly string ColdRStr = "Cold";
    private static readonly string HolyRStr = "Holy";
    private static readonly string LightningRStr = "Lightning";
    private static readonly string MagicRStr = "Magic";
    private static readonly string PhysicalRStr = "Physical";
    private static readonly string PoisonRStr = "Poison";
    private static readonly string WallRStr = "Wall";
    private static readonly string E1DamageRStr = "Damage";

    private static void RefreshCombatUnit(CombatUnitInfo Window)
    {
        AbilityCombatInfo Info = default(AbilityCombatInfo);
        Magic.Spells.SpellCombatInfo info = default(Magic.Spells.SpellCombatInfo);
        if (DialogHosts.General.FCombatUnit != null)
        {
            AbstractUnit fUnit = DialogHosts.General.FCombatUnit.FUnit.FUnit;
            Hero tHero = ((!(fUnit is Hero)) ? null : ((Hero)fUnit));
            if (!Window.TCUnitWin.Visible)
            {
                Window.UnitMana.Visible = false;
                DialogHosts.TBWindow.TCUnitBtn.Visible = true;
            }
            if (tHero != null)
            {
                if (tHero.GetCastingPointsMax() > 0)
                {
                    Window.UnitMana.Visible = true;
                    sbyte castingPointsMax = tHero.GetCastingPointsMax();
                    Window.UnitMana.Max = castingPointsMax;
                    sbyte castingPoints = tHero.GetCastingPoints();
                    Window.UnitMana.Position = castingPoints;
                }
                Windows.AOWPanel unitFr = Window.UnitFr;
                unitFr.Alignment.TopOffset = 7;
                unitFr.WinHeight = 54;
            }
            else
            {
                Windows.AOWPanel unitFr2 = Window.UnitFr;
                unitFr2.Alignment.TopOffset = 0;
                unitFr2.WinHeight = 61;
            }
            Window.UnitFr.FAligned = false;
            if (fUnit != null)
            {
                if (DialogHosts.General.FCombatUnit.FSelectedAbility == -1)
                {
                    Window.TCUnitPnl.Visible = true;
                    Window.TCAbilPnl.Visible = false;
                    Window.TargetSelPnl.Visible = false;
                    sbyte hitPoints = fUnit.GetHitPoints();
                    sbyte hits = fUnit.GetHits();
                    Window.UHits.Text = RtlUnit.IntToStr((int)hitPoints) + "/" + RtlUnit.IntToStr((int)hits);
                    sbyte moves = fUnit.GetMoves();
                    Window.UMoves.Text = RtlUnit.IntToStr(DialogHosts.General.FCombatUnit.FUnit.FMovePoints) + "/" + RtlUnit.IntToStr((int)moves);
                    sbyte attack = fUnit.GetAttack();
                    Window.UAttack.Text = RtlUnit.IntToStr((int)attack);
                    sbyte damage = fUnit.GetDamage();
                    Window.UDamage.Text = RtlUnit.IntToStr((int)damage);
                    sbyte defense = fUnit.GetDefense();
                    Window.UDefense.Text = RtlUnit.IntToStr((int)defense);
                    sbyte resistance = fUnit.GetResistance();
                    Window.UResistance.Text = RtlUnit.IntToStr((int)resistance);
                    Set<World.Movement.MoveType> moveTypes = fUnit.GetMoveTypes();
                    int current = DialogHosts.General.MTtoInt(moveTypes);
                    Window.UMovesIcon.SetCurrent(current);
                    DialogHosts.General.FCombatUnit.ListAbilities((StringCollection)(object)Window.UAbilLB.Items, null, null);
                    Window.UAbilLB.Selected = -1;
                    Window.UAbilLB.F188 = -1;
                    Window.UAbilLB.ListOffset = 0;
                }
                else if (DialogHosts.General.FCombatUnit.FSelectedAbility == 52 && DialogHosts.General.FCombatUnit.FSelectedSpell > -1)
                {
                    Window.TCUnitPnl.Visible = false;
                    Window.TCAbilPnl.Visible = true;
                    Window.TargetSelPnl.Visible = false;
                    AoWHSSet.Current.FSpellControl.GetSpell(DialogHosts.General.FCombatUnit.FSelectedSpell).GetCombatInfo(DialogHosts.General.FCombatUnit.FUnit.FUnit, ref info);
                    Magic.Spells.Spell spell = AoWHSSet.Current.FSpellControl.GetSpell(DialogHosts.General.FCombatUnit.FSelectedSpell);
                    Window.AbilNameLbl.Text = spell.FSpellName;
                    if (info.Attack > 0)
                    {
                        Window.AbilAttLbl.Text = RtlUnit.IntToStr((int)info.Attack);
                    }
                    else
                    {
                        Window.AbilAttLbl.Text = "-";
                    }
                    if (info.Damage > 0)
                    {
                        Window.AbilDamLbl.Text = RtlUnit.IntToStr((int)info.Damage);
                    }
                    else
                    {
                        Window.AbilDamLbl.Text = "-";
                    }
                    ((StringCollection)Window.AbilStat.Items).Clear();
                    if (info.DamageTypes.Contains(DamageType.Fire))
                    {
                        string text = AbilityRules.TranslateRStr(GameTables.FireRStr);
                        string text2 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text + " " + text2);
                    }
                    if (info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text3 = AbilityRules.TranslateRStr(ColdRStr);
                        string text4 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text3 + " " + text4);
                    }
                    if (info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text5 = AbilityRules.TranslateRStr(LightningRStr);
                        string text6 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text5 + " " + text6);
                    }
                    if (info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text7 = AbilityRules.TranslateRStr(MagicRStr);
                        string text8 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text7 + " " + text8);
                    }
                    if (info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text9 = AbilityRules.TranslateRStr(PoisonRStr);
                        string text10 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text9 + " " + text10);
                    }
                    if (info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text11 = AbilityRules.TranslateRStr(GameTables.DeathRStr);
                        string text12 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text11 + " " + text12);
                    }
                    if (info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text13 = AbilityRules.TranslateRStr(HolyRStr);
                        string text14 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text13 + " " + text14);
                    }
                    if (info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text15 = AbilityRules.TranslateRStr(PhysicalRStr);
                        string text16 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text15 + " " + text16);
                    }
                    if (info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text17 = AbilityRules.TranslateRStr(WallRStr);
                        string text18 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text17 + " " + text18);
                    }
                }
                else
                {
                    Window.TCUnitPnl.Visible = false;
                    Window.TCAbilPnl.Visible = true;
                    Window.TargetSelPnl.Visible = false;
                    AoWHSSet.Current.FAbilityControl.GetAbility(DialogHosts.General.FCombatUnit.FSelectedAbility).GetCombatInfo(DialogHosts.General.FCombatUnit.FUnit.FUnit, ref Info);
                    string name = AoWHSSet.Current.FAbilityControl.GetAbility(DialogHosts.General.FCombatUnit.FSelectedAbility).GetName(DialogHosts.General.FCombatUnit.FUnit.FUnit);
                    Window.AbilNameLbl.Text = name;
                    if (Info.Attack > 0)
                    {
                        Window.AbilAttLbl.Text = RtlUnit.IntToStr((int)Info.Attack);
                    }
                    else
                    {
                        Window.AbilAttLbl.Text = "-";
                    }
                    if (Info.Damage > 0)
                    {
                        Window.AbilDamLbl.Text = RtlUnit.IntToStr((int)Info.Damage);
                    }
                    else
                    {
                        Window.AbilDamLbl.Text = "-";
                    }
                    ((StringCollection)Window.AbilStat.Items).Clear();
                    if (Info.DamageTypes.Contains(DamageType.Fire))
                    {
                        string text19 = AbilityRules.TranslateRStr(GameTables.FireRStr);
                        string text20 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text19 + " " + text20);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Cold))
                    {
                        string text21 = AbilityRules.TranslateRStr(ColdRStr);
                        string text22 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text21 + " " + text22);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Lightning))
                    {
                        string text23 = AbilityRules.TranslateRStr(LightningRStr);
                        string text24 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text23 + " " + text24);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Magic))
                    {
                        string text25 = AbilityRules.TranslateRStr(MagicRStr);
                        string text26 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text25 + " " + text26);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Poison))
                    {
                        string text27 = AbilityRules.TranslateRStr(PoisonRStr);
                        string text28 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text27 + " " + text28);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Death))
                    {
                        string text29 = AbilityRules.TranslateRStr(GameTables.DeathRStr);
                        string text30 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text29 + " " + text30);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Holy))
                    {
                        string text31 = AbilityRules.TranslateRStr(HolyRStr);
                        string text32 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text31 + " " + text32);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Physical))
                    {
                        string text33 = AbilityRules.TranslateRStr(PhysicalRStr);
                        string text34 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text33 + " " + text34);
                    }
                    if (Info.DamageTypes.Contains(DamageType.Wall))
                    {
                        string text35 = AbilityRules.TranslateRStr(WallRStr);
                        string text36 = AbilityRules.TranslateRStr(E1DamageRStr);
                        ((StringCollection)Window.AbilStat.Items).Add(text35 + " " + text36);
                    }
                }
                ((StringCollection)Window.UnitName.Items).Clear();
                string name2 = fUnit.GetName();
                ((StringCollection)Window.UnitName.Items).Add(name2);
                sbyte hits2 = fUnit.GetHits();
                Window.UnitHits.Max = hits2;
                sbyte hitPoints2 = fUnit.GetHitPoints();
                Window.UnitHits.Position = hitPoints2;
            }
        }
        DialogHosts.TCUnit.TCUnitWin.Visible = true;
    }

    private static bool StopSong()
    {
        if (Audio.AudioHost.SoundSystem != null)
        {
            return Audio.AudioHost.SoundSystem.StopSong();
        }
        return false;
    }
}
