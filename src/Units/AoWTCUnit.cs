// Translated from AoWTCUnit.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AOWGauge;
using Aow.Units.AoWButton;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.aowUserDrawBox;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.E1Resources.E1Resources;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;

namespace Aow.Units.AoWTCUnit
{
    // components
    public partial class TTCUnit : TDataModule
    {
        public TTCUnit(TComponent owner) : base(owner)
        { }

        public TAOWWindow TCUnitWin;
        public TAOWImage SIcon;
        public TAOWButton Min;
        public TAOWButton Close;
        public TAOWLabel IWTitle;
        public TAOWImage UAttackIcon;
        public TAOWImage UDefenseIcon;
        public TAOWImage UMovesIcon;
        public TAOWImage UDamageIcon;
        public TAOWImage UResistanceIcon;
        public TAOWImage UHitsIcon;
        public TAOWLabel UAttack;
        public TAOWLabel UDefense;
        public TAOWLabel UMoves;
        public TAOWLabel UDamage;
        public TAOWLabel UResistance;
        public TAOWLabel UHits;
        public TAOWMemo UnitName;
        public TAOWPanel UStatPnl;
        public TAOWListBox UAbilLB;
        public TAOWLabel AbilNameLbl;
        public TAOWPanel TCUnitPnl;
        public TAOWImage AOWImage1;
        public TAOWMemo AUnitName;
        public TAOWListBox USelAbilLb;
        public TAOWPanel TCAbilPnl;
        public TAOWImage AbilAtt;
        public TAOWLabel AbilAttLbl;
        public TAOWPanel AbilStatBox;
        public TAOWListBox AbilStat;
        public TAOWImage AbilDam;
        public TAOWLabel AbilDamLbl;
        public TAOWLabel TargetLbl;
        public TAOWLabel TargetULbl;
        public TAOWPanel TargetSelPnl;
        public TAOWPanel NoTargetPnl;
        public TAOWMemo TargetMessage;
        public TAoWGauge UnitHits;
        public TAoWGauge UnitMana;
        public TAOWImage CelticCross;
        public TAOWPanel UStatP1;
        public TAOWPanel UStatP2;
        public TAOWImage AAttackIcon;
        public TAOWImage ADefenseIcon;
        public TAOWImage AMovesIcon;
        public TAOWImage ADamageIcon;
        public TAOWImage AResistanceIcon;
        public TAOWImage AHitsIcons;
        public TAOWLabel AAttack;
        public TAOWLabel ADefense;
        public TAOWLabel AMoves;
        public TAOWLabel ADamage;
        public TAOWLabel AResistance;
        public TAOWLabel AHits;
        public TAOWPanel AStatPnl;
        public TAOWPanel AStatP1;
        public TAOWPanel AStatP2;
        public TAOWButton Center;
        public TAOWVScrollBar AbilSB;
        public TAOWVScrollBar UAbilSB;
        public TAOWVScrollBar AOWVScrollBar1;
        public TAOWPanel AbilFr;
        public TAOWPanel ANameFr;
        public TAOWPanel TargerFr;
        public TAOWPanel TNamePnl;
        public TAOWPanel UnitFr;
        public TaowUserDrawBox UnitBox;
        public TIvTranslator Translator;
        // AoWTCUnit.pas:106
        public void TCUnitWinResize()
        {
            this.TCUnitWin.Alignment.RightOffset = this.TCUnitWin.WinManager.WinWidth - this.TCUnitWin.WinLeft - this.TCUnitWin.WinWidth;
            this.TCUnitWin.Alignment.BottomOffset = this.TCUnitWin.WinManager.WinHeight - this.TCUnitWin.WinTop - this.TCUnitWin.WinHeight;
        }

        // AoWTCUnit.pas:116
        public void MinClick()
        {
            General.PlayButtonSound(0);
            TCUnitWin.Visible = false;
        }

        // AoWTCUnit.pas:123
        public void UAbilLBPreDrawItem(TObject Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SD)
        {
            if (General.FCombatUnit != null)
            {
                var call__2 = General.FCombatUnit.FUnit.GetPlayer();
                bool bool__3 = call__2 == AoWCombatMap.FSeatedPlayer && General.FCombatUnit.FUnit.FMovePoints > 0 && DrawItem < General.FCombatUnit.FCombatAbilityCount;
                if (bool__3)
                {
                    var call__4 = General.FCombatUnit.FAbilities[DrawItem];
                    var call__5 = AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)call__4).Ability);
                    var call__6 = call__5.GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                    bool bool__7 = !TSet<TCombatMode>.Of().WithRange(TCombatMode.cmShortRange, TCombatMode.cmLongRange).Contains(call__6);
                    if (bool__7)
                    {
                        var call__8 = General.FCombatUnit.FAbilities[DrawItem];
                        bool__7 = ((TTCAbility)call__8).Ability != 52;
                    }

                    bool bool__9 = bool__7;
                    if (!bool__9)
                    {
                        var call__10 = General.FCombatUnit.FUnit.FUnit.GetMoves();
                        bool__9 = General.FCombatUnit.FUnit.FMovePoints == call__10;
                    }

                    bool__3 = bool__9;
                }

                if (bool__3)
                {
                    UAbilLB.TextColor = 16777215;
                }
                else
                {
                    UAbilLB.TextColor = 12632256;
                }
            }
        }

        // AoWTCUnit.pas:138
        public void TCUnitPnlDrawBack(TObject Sender, TSurfaceDesc SD)
        {
            if (General.FCombatUnit != null)
            {
                General.FCombatUnit.FUnit.FUnit.ShowEx(TCUnitPnl.WinLeft - 10, TCUnitPnl.WinTop - 2, SD, General.FUnitShowInfo);
            }
        }

        // AoWTCUnit.pas:146
        public void UAbilLBUpdate()
        {
            if (UAbilLB.F188 != UAbilLB.Selected)
            {
                UAbilLB.Selected = UAbilLB.F188;
            }
        }

        // AoWTCUnit.pas:152
        public void UAbilLBMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility Ability = default;
            if (UAbilLB.Selected > -1)
            {
                if (Button == TMouseButton.mbLeft)
                {
                    var call__11 = UAbilLB.Items.Objects[UAbilLB.Selected];
                    var call__12 = General.FCombatUnit.SelectAbility(ObjectToInt(call__11));
                    UAbilLB.Selected = call__12;
                }
                else
                {
                    if (Button == TMouseButton.mbRight && General.FCombatUnit != null)
                    {
                        var call__13 = UAbilLB.Items.Objects[UAbilLB.Selected];
                        var call__14 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__13));
                        Ability = call__14;
                        if (Ability != null)
                        {
                            var call__15 = Ability.GetName(General.FCombatUnit.FUnit.FUnit);
                            SetInfoBanner(InfoBanner, call__15, Ability.FDescription);
                        }
                    }
                }
            }
        }

        // AoWTCUnit.pas:168
        public void UAbilLBMouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            if (General.FCombatUnit != null)
            {
                var call__16 = General.FCombatUnit.FUnit.GetPlayer();
                if (call__16 == AoWCombatMap.FSeatedPlayer && General.FCombatUnit.FUnit.FMovePoints != 0 && UAbilLB.F188 < General.FCombatUnit.FCombatAbilityCount)
                {
                    var call__17 = General.FCombatUnit.FAbilities[UAbilLB.F188];
                    var call__18 = AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)call__17).Ability);
                    var call__19 = call__18.GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                    bool bool__20 = !TSet<TCombatMode>.Of().WithRange(TCombatMode.cmShortRange, TCombatMode.cmLongRange).Contains(call__19);
                    if (bool__20)
                    {
                        var call__21 = General.FCombatUnit.FAbilities[UAbilLB.F188];
                        bool__20 = ((TTCAbility)call__21).Ability != 52;
                    }

                    if (bool__20)
                    {
                        return;
                    }

                    var tacticalCombatUnit = General.FCombatUnit.FUnit;
                    var call__23 = tacticalCombatUnit.FUnit.GetMoves();
                    if (call__23 <= tacticalCombatUnit.FMovePoints)
                    {
                        return;
                    }
                }

                UAbilLB.F188 = -1;
            }
        }

        // AoWTCUnit.pas:187
        public void HeroSMapMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            General.FSelectedUnit = General.FCombatUnit.FUnit.FUnit;
            AoWUnitWindow.AoWUnitWindow.UnitWindow.ShowUnit();
        }

        // AoWTCUnit.pas:194
        public void CenterClick()
        {
            General.PlayButtonSound(0);
            var call__24 = General.FCombatUnit.GetLhx();
            var call__25 = General.FCombatUnit.GetYhx();
            var call__26 = General.FCombatUnit.GetXhx();
            AoWCombatMap.CenterViewTo(call__26, call__25, call__24);
            var call__27 = General.FCombatUnit.GetLhx();
            var call__28 = General.FCombatUnit.GetYhx();
            var call__29 = General.FCombatUnit.GetXhx();
            AoWCombatMap.PlayCenterAnimation(call__29, call__28, call__27);
        }

        // AoWTCUnit.pas:195
        public void CloseClick()
        {
            General.PlayButtonSound(0);
            TCUnitWin.Visible = false;
        }
    }

    public static partial class AoWTCUnit
    {
        public static TTCUnit TCUnit;
        // AoWTCUnit.pas:197
        public static void RefreshCombatUnit(TTCUnit Window)
        {
            TAbilityCombatInfo AbilityInfo = default;
            TSpellCombatInfo SpellInfo = default;
            TAbstractUnit U = default;
            THero Hero = default;
            if (General.FCombatUnit != null)
            {
                U = General.FCombatUnit.FUnit.FUnit;
                if (U is THero)
                {
                    Hero = (THero)U;
                }
                else
                {
                    Hero = null;
                }

                if (!Window.TCUnitWin.Visible)
                {
                    Window.UnitMana.Visible = false;
                    TBWindow.TCUnitBtn.Visible = true;
                }

                if (Hero != null)
                {
                    if (Hero.GetCastingPointsMax() > 0)
                    {
                        Window.UnitMana.Visible = true;
                        var call__30 = Hero.GetCastingPointsMax();
                        Window.UnitMana.Max = call__30;
                        var call__31 = Hero.GetCastingPoints();
                        Window.UnitMana.Position = call__31;
                    }

                    var panel = Window.UnitFr;
                    panel.Alignment.TopOffset = 7;
                    panel.WinHeight = 54;
                }
                else
                {
                    var panel2 = Window.UnitFr;
                    panel2.Alignment.TopOffset = 0;
                    panel2.WinHeight = 61;
                }

                Window.UnitFr.FAligned = false;
                if (U != null)
                {
                    if (General.FCombatUnit.FSelectedAbility == -1)
                    {
                        Window.TCUnitPnl.Visible = true;
                        Window.TCAbilPnl.Visible = false;
                        Window.TargetSelPnl.Visible = false;
                        var call__34 = U.GetHitPoints();
                        var call__35 = U.GetHits();
                        Window.UHits.Text = IntToStr(call__34) + AnsiString.FromBytes(47) + IntToStr(call__35);
                        var call__36 = U.GetMoves();
                        Window.UMoves.Text = IntToStr(General.FCombatUnit.FUnit.FMovePoints) + AnsiString.FromBytes(47) + IntToStr(call__36);
                        var call__37 = U.GetAttack();
                        Window.UAttack.Text = IntToStr(call__37);
                        var call__38 = U.GetDamage();
                        Window.UDamage.Text = IntToStr(call__38);
                        var call__39 = U.GetDefense();
                        Window.UDefense.Text = IntToStr(call__39);
                        var call__40 = U.GetResistance();
                        Window.UResistance.Text = IntToStr(call__40);
                        var call__41 = U.GetMoveTypes();
                        var call__42 = General.MTtoInt(call__41);
                        Window.UMovesIcon.SetCurrent(call__42);
                        General.FCombatUnit.ListAbilities(Window.UAbilLB.Items, null, null);
                        Window.UAbilLB.Selected = -1;
                        Window.UAbilLB.F188 = -1;
                        Window.UAbilLB.ListOffset = 0;
                    }
                    else
                    {
                        if (General.FCombatUnit.FSelectedAbility == 52 && General.FCombatUnit.FSelectedSpell > -1)
                        {
                            Window.TCUnitPnl.Visible = false;
                            Window.TCAbilPnl.Visible = true;
                            Window.TargetSelPnl.Visible = false;
                            var call__43 = AoWHSSet.FSpellControl.GetSpell(General.FCombatUnit.FSelectedSpell);
                            call__43.GetCombatInfo(General.FCombatUnit.FUnit.FUnit, ref SpellInfo);
                            var call__44 = AoWHSSet.FSpellControl.GetSpell(General.FCombatUnit.FSelectedSpell);
                            Window.AbilNameLbl.Text = call__44.FSpellName;
                            if (SpellInfo.Attack > 0)
                            {
                                Window.AbilAttLbl.Text = IntToStr(SpellInfo.Attack);
                            }
                            else
                            {
                                Window.AbilAttLbl.Text = AnsiString.FromBytes(45);
                            }

                            if (SpellInfo.Damage > 0)
                            {
                                Window.AbilDamLbl.Text = IntToStr(SpellInfo.Damage);
                            }
                            else
                            {
                                Window.AbilDamLbl.Text = AnsiString.FromBytes(45);
                            }

                            Window.AbilStat.Items.Clear();
                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtFire))
                            {
                                var call__45 = TranslateRStr(FireRStr);
                                var call__46 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__45 + AnsiString.FromBytes(32) + call__46);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtCold))
                            {
                                var call__47 = TranslateRStr(ColdRStr);
                                var call__48 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__47 + AnsiString.FromBytes(32) + call__48);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtLightning))
                            {
                                var call__49 = TranslateRStr(LightningRStr);
                                var call__50 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__49 + AnsiString.FromBytes(32) + call__50);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtMagic))
                            {
                                var call__51 = TranslateRStr(MagicRStr);
                                var call__52 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__51 + AnsiString.FromBytes(32) + call__52);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtPoison))
                            {
                                var call__53 = TranslateRStr(PoisonRStr);
                                var call__54 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__53 + AnsiString.FromBytes(32) + call__54);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtDeath))
                            {
                                var call__55 = TranslateRStr(DeathRStr);
                                var call__56 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__55 + AnsiString.FromBytes(32) + call__56);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtHoly))
                            {
                                var call__57 = TranslateRStr(HolyRStr);
                                var call__58 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__57 + AnsiString.FromBytes(32) + call__58);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtPhysical))
                            {
                                var call__59 = TranslateRStr(PhysicalRStr);
                                var call__60 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__59 + AnsiString.FromBytes(32) + call__60);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtWall))
                            {
                                var call__61 = TranslateRStr(WallRStr);
                                var call__62 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__61 + AnsiString.FromBytes(32) + call__62);
                            }
                        }
                        else
                        {
                            Window.TCUnitPnl.Visible = false;
                            Window.TCAbilPnl.Visible = true;
                            Window.TargetSelPnl.Visible = false;
                            var call__63 = AoWHSSet.FAbilityControl.GetAbility(General.FCombatUnit.FSelectedAbility);
                            call__63.GetCombatInfo(General.FCombatUnit.FUnit.FUnit, ref AbilityInfo);
                            var call__64 = AoWHSSet.FAbilityControl.GetAbility(General.FCombatUnit.FSelectedAbility);
                            var call__65 = call__64.GetName(General.FCombatUnit.FUnit.FUnit);
                            Window.AbilNameLbl.Text = call__65;
                            if (AbilityInfo.Attack > 0)
                            {
                                Window.AbilAttLbl.Text = IntToStr(AbilityInfo.Attack);
                            }
                            else
                            {
                                Window.AbilAttLbl.Text = AnsiString.FromBytes(45);
                            }

                            if (AbilityInfo.Damage > 0)
                            {
                                Window.AbilDamLbl.Text = IntToStr(AbilityInfo.Damage);
                            }
                            else
                            {
                                Window.AbilDamLbl.Text = AnsiString.FromBytes(45);
                            }

                            Window.AbilStat.Items.Clear();
                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtFire))
                            {
                                var call__66 = TranslateRStr(FireRStr);
                                var call__67 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__66 + AnsiString.FromBytes(32) + call__67);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtCold))
                            {
                                var call__68 = TranslateRStr(ColdRStr);
                                var call__69 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__68 + AnsiString.FromBytes(32) + call__69);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtLightning))
                            {
                                var call__70 = TranslateRStr(LightningRStr);
                                var call__71 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__70 + AnsiString.FromBytes(32) + call__71);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtMagic))
                            {
                                var call__72 = TranslateRStr(MagicRStr);
                                var call__73 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__72 + AnsiString.FromBytes(32) + call__73);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtPoison))
                            {
                                var call__74 = TranslateRStr(PoisonRStr);
                                var call__75 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__74 + AnsiString.FromBytes(32) + call__75);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtDeath))
                            {
                                var call__76 = TranslateRStr(DeathRStr);
                                var call__77 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__76 + AnsiString.FromBytes(32) + call__77);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtHoly))
                            {
                                var call__78 = TranslateRStr(HolyRStr);
                                var call__79 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__78 + AnsiString.FromBytes(32) + call__79);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtPhysical))
                            {
                                var call__80 = TranslateRStr(PhysicalRStr);
                                var call__81 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__80 + AnsiString.FromBytes(32) + call__81);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtWall))
                            {
                                var call__82 = TranslateRStr(WallRStr);
                                var call__83 = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(call__82 + AnsiString.FromBytes(32) + call__83);
                            }
                        }
                    }

                    Window.UnitName.Items.Clear();
                    var call__84 = U.GetName();
                    Window.UnitName.Items.Add(call__84);
                    var call__85 = U.GetHits();
                    Window.UnitHits.Max = call__85;
                    var call__86 = U.GetHitPoints();
                    Window.UnitHits.Position = call__86;
                }
            }

            TCUnit.TCUnitWin.Visible = true;
        }
    }
}
