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

public partial class TTCMapEvents : TDataModule
{
    public TTCMapEvents(TComponent owner) : base(owner)
    { }

    public TAoWCombatMapControl TCMap;
    public int FChatHeight;

    // aowTCMapEvents.pas:105
    public void TCMapDeActivateMap()
    {
        Aow.Game.Ui.Dialogs.TTCMapEvents.StopSong();
        Aow.Game.Audio.AudioHost.SongPlayer.Resume();
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapViewer.HSMap = Aow.Game.World.Map.TAoWHSMap.AoWHSMap;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.SetMap(Aow.Game.World.Map.TAoWHSMap.AoWHSMap);
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FPreviousWidth = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FIncremental = false;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FSized = false;
        Aow.Game.Ui.Dialogs.DialogHosts.SWindow.Scanner.FUpdateStep = -1;

        if (FChatHeight > 0)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.ChatWin.WinHeight = FChatHeight;
        }

        var chatWindow = Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow;
        chatWindow.ChatWin.Alignment.AlignHeight = TAOWAlignHeight.ahBottom;
        chatWindow.ChatWin.Visible = false;

        Aow.Game.Ui.Dialogs.DialogHosts.ChatWindow.FScrollPending = true;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignWidth;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignHeight;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Left = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.LeftOffset;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Top = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.TopOffset;
        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignWidth == TAOWAlignWidth.awBoth)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.RightOffset;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Right = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth;
        }

        if (Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapAlignHeight == TAOWAlignHeight.ahBoth)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.BottomOffset;
        }
        else
        {
            Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FMapRect.Bottom = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.LeftOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Left;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.TopOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Top;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.RightOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Right;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.BottomOffset = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Bottom;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Right;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.WinHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldRect.Bottom;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignWidth = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignWidth;
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Alignment.AlignHeight = Aow.Game.Ui.Dialogs.DialogHosts.MWindow.FWorldAlignHeight;
        Aow.Game.Ui.Dialogs.DialogHosts.General.ShowWindows();
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.FAligned = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TBWorld.Visible = true;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TBCombat.Visible = false;
        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TurnButton.FButtonChecked = player.FSingleProcessCycle;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Visible = false;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.SetMap(null);
        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 0;
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapGemsChanged(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapIncomeChanged(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapPlayerMagicChanged(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer);
    }

    // aowTCMapEvents.pas:157
    public void TCMapDeselected(THexagonSprite HS)
    {
        if (HS is TTacticalCombatUnitHS)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit = null;
            Aow.Game.Ui.Dialogs.DialogHosts.General.FSelectedUnit = null;
            Aow.Game.Ui.Dialogs.DialogHosts.UnitWindow.UnitWin.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCUnitBtn.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Visible = false;
        }
    }

    // aowTCMapEvents.pas:169
    public void TCMapMapChanged(in TMapChangedInfo R)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.Scanner.DrawMap(R.Bounds.Left - 8, R.Bounds.Top - 6, R.Bounds.Right - R.Bounds.Left, R.Bounds.Bottom - R.Bounds.Top);
    }

    // aowTCMapEvents.pas:174
    public void TCMapMouseOverTarget(THexagonSprite HS)
    {
        TAbstractUnit U;
        if (Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Visible)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TargetSelPnl.Visible = HS != null;
            Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.NoTargetPnl.Visible = HS == null;
            if (HS != null)
            {
                if (HS is TTacticalCombatUnitHS)
                {
                    U = ((TTacticalCombatUnitHS)HS).FUnit.FUnit;
                    var name = U.GetName();
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TargetULbl.Text = name;
                    var hitPoints = U.GetHitPoints();
                    var hits = U.GetHits();
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.AHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                    var movePoints = U.GetMovePoints();
                    var moves = U.GetMoves();
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.AMoves.Text = IntToStr(movePoints) + "/" + IntToStr(moves);
                    var attack = U.GetAttack();
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.AAttack.Text = IntToStr(attack);
                    var damage = U.GetDamage();
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.ADamage.Text = IntToStr(damage);
                    var defense = U.GetDefense();
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.ADefense.Text = IntToStr(defense);
                    var resistance = U.GetResistance();
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.AResistance.Text = IntToStr(resistance);
                    var moveTypes = U.GetMoveTypes();
                    var mTtoInt = Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(moveTypes);
                    Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.AMovesIcon.SetCurrent(mTtoInt);
                    U.ListAbilities(Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.USelAbilLb.Items);
                }
            }
        }
    }

    // aowTCMapEvents.pas:199
    public void TCMapSeatedPlayerChanged(sbyte Player)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.MWindow.MapWindow.Visible = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds;
        Aow.Game.Ui.Dialogs.DialogHosts.TCScanner.TCScannerWin.Visible = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlaySounds;
        if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedCombatPlayer == -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.AutoBtn.Checked = false;
        }
        else
        {
            var player = Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FSeatedCombatPlayer);
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.AutoBtn.Checked = player.FAuto;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapGemsChanged(Player);
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapIncomeChanged(Player);
        Aow.Game.Ui.Dialogs.DialogHosts.MapEvents.TheMapPlayerMagicChanged(Player);
    }

    // aowTCMapEvents.pas:210
    public void TCMapSelected(THexagonSprite HS)
    {
        if (HS is TTacticalCombatUnitHS)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit = (TTacticalCombatUnitHS)HS;
            Aow.Game.Ui.Dialogs.TTCMapEvents.RefreshCombatUnit(Aow.Game.Ui.Dialogs.DialogHosts.TCUnit);
        }
    }

    // aowTCMapEvents.pas:219
    public void TCMapShowMessage(in TShowMessageInfo Info)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCMsg.TextColor = Info.Color;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCMsg.Text = Info.Text;
    }

    // aowTCMapEvents.pas:225
    public void TCMapUnitChanged(TTacticalCombatUnitHS HS)
    {
        if (HS == Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit && HS != null)
        {
            Aow.Game.Ui.Dialogs.TTCMapEvents.RefreshCombatUnit(Aow.Game.Ui.Dialogs.DialogHosts.TCUnit);
        }
    }

    // aowTCMapEvents.pas:230
    public void TCMapCastSpell(TCombatSpellCaster Caster)
    {
        var selectSpell = Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.SelectSpell(Caster.FCombatCasterSpellID);
        Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.UAbilLB.Selected = selectSpell;
    }

    // aowTCMapEvents.pas:235
    public void TCMapActivateAbility(TActivateAbilityInfo Info)
    {
        if (Info.FAbilityID == 52)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.SpellBook.ShowCombatCasting(Info);
        }
    }

    // aowTCMapEvents.pas:240
    public void TCMapTurnChanged()
    {
        var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.FCurrentPlayer);
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCTurnButton.FCurPlayer = player.FPlayerRace;
        if (Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCTurnButton.FCurPlayer == -1)
        {
            Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCTurnButton.FCurPlayer = 12;
        }

        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCTurnButton.FLastPlayer = -1;

        Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.OverlayPriority = 49;
        Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCPInd.FDrawn = false;
    }

    private static readonly string ColdRStr = "Cold";
    private static readonly string HolyRStr = "Holy";
    private static readonly string LightningRStr = "Lightning";
    private static readonly string MagicRStr = "Magic";
    private static readonly string PhysicalRStr = "Physical";
    private static readonly string PoisonRStr = "Poison";
    private static readonly string WallRStr = "Wall";
    private static readonly string E1DamageRStr = "Damage";

    private static void RefreshCombatUnit(global::Aow.Game.Ui.Dialogs.TTCUnit Window)
    {
        global::Aow.Game.Magic.Abilities.TAbilityCombatInfo Info = default(global::Aow.Game.Magic.Abilities.TAbilityCombatInfo);
        global::Aow.Game.Magic.Spells.TSpellCombatInfo info = default(global::Aow.Game.Magic.Spells.TSpellCombatInfo);
        if (global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit != null)
        {
            global::Aow.Game.Magic.Abilities.TAbstractUnit fUnit = global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit;
            global::Aow.Game.Magic.Abilities.THero tHero = ((!(fUnit is global::Aow.Game.Magic.Abilities.THero)) ? null : ((global::Aow.Game.Magic.Abilities.THero)fUnit));
            if (!Window.TCUnitWin.Visible)
            {
                Window.UnitMana.Visible = false;
                global::Aow.Game.Ui.Dialogs.DialogHosts.TBWindow.TCUnitBtn.Visible = true;
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
                global::Aow.Game.Ui.Windows.TAOWPanel unitFr = Window.UnitFr;
                unitFr.Alignment.TopOffset = 7;
                unitFr.WinHeight = 54;
            }
            else
            {
                global::Aow.Game.Ui.Windows.TAOWPanel unitFr2 = Window.UnitFr;
                unitFr2.Alignment.TopOffset = 0;
                unitFr2.WinHeight = 61;
            }
            Window.UnitFr.FAligned = false;
            if (fUnit != null)
            {
                if (global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FSelectedAbility == -1)
                {
                    Window.TCUnitPnl.Visible = true;
                    Window.TCAbilPnl.Visible = false;
                    Window.TargetSelPnl.Visible = false;
                    sbyte hitPoints = fUnit.GetHitPoints();
                    sbyte hits = fUnit.GetHits();
                    Window.UHits.Text = RtlUnit.IntToStr((int)hitPoints) + "/" + RtlUnit.IntToStr((int)hits);
                    sbyte moves = fUnit.GetMoves();
                    Window.UMoves.Text = RtlUnit.IntToStr(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FMovePoints) + "/" + RtlUnit.IntToStr((int)moves);
                    sbyte attack = fUnit.GetAttack();
                    Window.UAttack.Text = RtlUnit.IntToStr((int)attack);
                    sbyte damage = fUnit.GetDamage();
                    Window.UDamage.Text = RtlUnit.IntToStr((int)damage);
                    sbyte defense = fUnit.GetDefense();
                    Window.UDefense.Text = RtlUnit.IntToStr((int)defense);
                    sbyte resistance = fUnit.GetResistance();
                    Window.UResistance.Text = RtlUnit.IntToStr((int)resistance);
                    TSet<global::Aow.Game.World.Movement.TMoveType> moveTypes = fUnit.GetMoveTypes();
                    int current = global::Aow.Game.Ui.Dialogs.DialogHosts.General.MTtoInt(moveTypes);
                    Window.UMovesIcon.SetCurrent(current);
                    global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.ListAbilities((TStrings)(object)Window.UAbilLB.Items, null, null);
                    Window.UAbilLB.Selected = -1;
                    Window.UAbilLB.F188 = -1;
                    Window.UAbilLB.ListOffset = 0;
                }
                else if (global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FSelectedAbility == 52 && global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FSelectedSpell > -1)
                {
                    Window.TCUnitPnl.Visible = false;
                    Window.TCAbilPnl.Visible = true;
                    Window.TargetSelPnl.Visible = false;
                    global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FSelectedSpell).GetCombatInfo(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit, ref info);
                    global::Aow.Game.Magic.Spells.TSpell spell = global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FSelectedSpell);
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
                    ((TStrings)Window.AbilStat.Items).Clear();
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtFire))
                    {
                        string text = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameTables.FireRStr);
                        string text2 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text + " " + text2);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtCold))
                    {
                        string text3 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ColdRStr);
                        string text4 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text3 + " " + text4);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtLightning))
                    {
                        string text5 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LightningRStr);
                        string text6 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text5 + " " + text6);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtMagic))
                    {
                        string text7 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MagicRStr);
                        string text8 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text7 + " " + text8);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtPoison))
                    {
                        string text9 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(PoisonRStr);
                        string text10 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text9 + " " + text10);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtDeath))
                    {
                        string text11 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameTables.DeathRStr);
                        string text12 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text11 + " " + text12);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtHoly))
                    {
                        string text13 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(HolyRStr);
                        string text14 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text13 + " " + text14);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtPhysical))
                    {
                        string text15 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(PhysicalRStr);
                        string text16 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text15 + " " + text16);
                    }
                    if (info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtWall))
                    {
                        string text17 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(WallRStr);
                        string text18 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text17 + " " + text18);
                    }
                }
                else
                {
                    Window.TCUnitPnl.Visible = false;
                    Window.TCAbilPnl.Visible = true;
                    Window.TargetSelPnl.Visible = false;
                    global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FSelectedAbility).GetCombatInfo(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit, ref Info);
                    string name = global::Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FSelectedAbility).GetName(global::Aow.Game.Ui.Dialogs.DialogHosts.General.FCombatUnit.FUnit.FUnit);
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
                    ((TStrings)Window.AbilStat.Items).Clear();
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtFire))
                    {
                        string text19 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameTables.FireRStr);
                        string text20 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text19 + " " + text20);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtCold))
                    {
                        string text21 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ColdRStr);
                        string text22 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text21 + " " + text22);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtLightning))
                    {
                        string text23 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LightningRStr);
                        string text24 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text23 + " " + text24);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtMagic))
                    {
                        string text25 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(MagicRStr);
                        string text26 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text25 + " " + text26);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtPoison))
                    {
                        string text27 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(PoisonRStr);
                        string text28 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text27 + " " + text28);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtDeath))
                    {
                        string text29 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(global::Aow.Game.Magic.Abilities.GameTables.DeathRStr);
                        string text30 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text29 + " " + text30);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtHoly))
                    {
                        string text31 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(HolyRStr);
                        string text32 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text31 + " " + text32);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtPhysical))
                    {
                        string text33 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(PhysicalRStr);
                        string text34 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text33 + " " + text34);
                    }
                    if (Info.DamageTypes.Contains(global::Aow.Game.Combat.TDamageType.dtWall))
                    {
                        string text35 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(WallRStr);
                        string text36 = global::Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(E1DamageRStr);
                        ((TStrings)Window.AbilStat.Items).Add(text35 + " " + text36);
                    }
                }
                ((TStrings)Window.UnitName.Items).Clear();
                string name2 = fUnit.GetName();
                ((TStrings)Window.UnitName.Items).Add(name2);
                sbyte hits2 = fUnit.GetHits();
                Window.UnitHits.Max = hits2;
                sbyte hitPoints2 = fUnit.GetHitPoints();
                Window.UnitHits.Position = hitPoints2;
            }
        }
        global::Aow.Game.Ui.Dialogs.DialogHosts.TCUnit.TCUnitWin.Visible = true;
    }

    private static bool StopSong()
    {
        if (global::Aow.Game.Audio.AudioHost.SoundSystem != null)
        {
            return global::Aow.Game.Audio.AudioHost.SoundSystem.StopSong();
        }
        return false;
    }
}
