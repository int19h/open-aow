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
                bool condition = General.FCombatUnit.FUnit.GetPlayer() == AoWCombatMap.FSeatedPlayer && General.FCombatUnit.FUnit.FMovePoints > 0 && DrawItem < General.FCombatUnit.FCombatAbilityCount;
                if (condition)
                {
                    var ability = General.FCombatUnit.FAbilities[DrawItem];
                    var combatMode = AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability).Ability).GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                    bool test = !TSet<TCombatMode>.Of().WithRange(TCombatMode.cmShortRange, TCombatMode.cmLongRange).Contains(combatMode) && ((TTCAbility)General.FCombatUnit.FAbilities[DrawItem]).Ability != 52;
                    if (!test)
                    {
                        var moves = General.FCombatUnit.FUnit.FUnit.GetMoves();
                        test = General.FCombatUnit.FUnit.FMovePoints == moves;
                    }

                    condition = test;
                }

                if (condition)
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
            TAbility Ability;
            if (UAbilLB.Selected > -1)
            {
                if (Button == TMouseButton.mbLeft)
                {
                    var objectValue = UAbilLB.Items.Objects[UAbilLB.Selected];
                    var selectAbility = General.FCombatUnit.SelectAbility(ObjectToInt(objectValue));
                    UAbilLB.Selected = selectAbility;
                }
                else
                {
                    if (Button == TMouseButton.mbRight && General.FCombatUnit != null)
                    {
                        var objectValue = UAbilLB.Items.Objects[UAbilLB.Selected];
                        Ability = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
                        if (Ability != null)
                        {
                            var name = Ability.GetName(General.FCombatUnit.FUnit.FUnit);
                            SetInfoBanner(InfoBanner, name, Ability.FDescription);
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
                if (General.FCombatUnit.FUnit.GetPlayer() == AoWCombatMap.FSeatedPlayer && General.FCombatUnit.FUnit.FMovePoints != 0 && UAbilLB.F188 < General.FCombatUnit.FCombatAbilityCount)
                {
                    var ability = General.FCombatUnit.FAbilities[UAbilLB.F188];
                    var combatMode = AoWHSSet.FAbilityControl.GetAbility(((TTCAbility)ability).Ability).GetCombatMode(General.FCombatUnit.FUnit.FUnit);
                    if (!TSet<TCombatMode>.Of().WithRange(TCombatMode.cmShortRange, TCombatMode.cmLongRange).Contains(combatMode) && ((TTCAbility)General.FCombatUnit.FAbilities[UAbilLB.F188]).Ability != 52)
                    {
                        return;
                    }

                    var tacticalCombatUnit = General.FCombatUnit.FUnit;
                    if (tacticalCombatUnit.FUnit.GetMoves() <= tacticalCombatUnit.FMovePoints)
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
            var lhx = General.FCombatUnit.GetLhx();
            var yhx = General.FCombatUnit.GetYhx();
            var xhx = General.FCombatUnit.GetXhx();
            AoWCombatMap.CenterViewTo(xhx, yhx, lhx);
            var fCombatUnitLhx = General.FCombatUnit.GetLhx();
            var fCombatUnitYhx = General.FCombatUnit.GetYhx();
            var fCombatUnitXhx = General.FCombatUnit.GetXhx();
            AoWCombatMap.PlayCenterAnimation(fCombatUnitXhx, fCombatUnitYhx, fCombatUnitLhx);
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
            TAbstractUnit U;
            THero Hero;
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
                        var castingPointsMax = Hero.GetCastingPointsMax();
                        Window.UnitMana.Max = castingPointsMax;
                        var castingPoints = Hero.GetCastingPoints();
                        Window.UnitMana.Position = castingPoints;
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
                        var hitPoints = U.GetHitPoints();
                        var hits = U.GetHits();
                        Window.UHits.Text = IntToStr(hitPoints) + "/" + IntToStr(hits);
                        var moves = U.GetMoves();
                        Window.UMoves.Text = IntToStr(General.FCombatUnit.FUnit.FMovePoints) + "/" + IntToStr(moves);
                        var attack = U.GetAttack();
                        Window.UAttack.Text = IntToStr(attack);
                        var damage = U.GetDamage();
                        Window.UDamage.Text = IntToStr(damage);
                        var defense = U.GetDefense();
                        Window.UDefense.Text = IntToStr(defense);
                        var resistance = U.GetResistance();
                        Window.UResistance.Text = IntToStr(resistance);
                        var moveTypes = U.GetMoveTypes();
                        var mTtoInt = General.MTtoInt(moveTypes);
                        Window.UMovesIcon.SetCurrent(mTtoInt);
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
                            AoWHSSet.FSpellControl.GetSpell(General.FCombatUnit.FSelectedSpell).GetCombatInfo(General.FCombatUnit.FUnit.FUnit, ref SpellInfo);
                            var spell = AoWHSSet.FSpellControl.GetSpell(General.FCombatUnit.FSelectedSpell);
                            Window.AbilNameLbl.Text = spell.FSpellName;
                            if (SpellInfo.Attack > 0)
                            {
                                Window.AbilAttLbl.Text = IntToStr(SpellInfo.Attack);
                            }
                            else
                            {
                                Window.AbilAttLbl.Text = "-";
                            }

                            if (SpellInfo.Damage > 0)
                            {
                                Window.AbilDamLbl.Text = IntToStr(SpellInfo.Damage);
                            }
                            else
                            {
                                Window.AbilDamLbl.Text = "-";
                            }

                            Window.AbilStat.Items.Clear();
                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtFire))
                            {
                                var fire = TranslateRStr(FireRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(fire + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtCold))
                            {
                                var cold = TranslateRStr(ColdRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(cold + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtLightning))
                            {
                                var lightning = TranslateRStr(LightningRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(lightning + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtMagic))
                            {
                                var magic = TranslateRStr(MagicRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(magic + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtPoison))
                            {
                                var poison = TranslateRStr(PoisonRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(poison + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtDeath))
                            {
                                var death = TranslateRStr(DeathRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(death + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtHoly))
                            {
                                var holy = TranslateRStr(HolyRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(holy + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtPhysical))
                            {
                                var physical = TranslateRStr(PhysicalRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(physical + " " + e1Damage);
                            }

                            if (SpellInfo.DamageTypes.Contains(TDamageType.dtWall))
                            {
                                var wall = TranslateRStr(WallRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(wall + " " + e1Damage);
                            }
                        }
                        else
                        {
                            Window.TCUnitPnl.Visible = false;
                            Window.TCAbilPnl.Visible = true;
                            Window.TargetSelPnl.Visible = false;
                            AoWHSSet.FAbilityControl.GetAbility(General.FCombatUnit.FSelectedAbility).GetCombatInfo(General.FCombatUnit.FUnit.FUnit, ref AbilityInfo);
                            var name = AoWHSSet.FAbilityControl.GetAbility(General.FCombatUnit.FSelectedAbility).GetName(General.FCombatUnit.FUnit.FUnit);
                            Window.AbilNameLbl.Text = name;
                            if (AbilityInfo.Attack > 0)
                            {
                                Window.AbilAttLbl.Text = IntToStr(AbilityInfo.Attack);
                            }
                            else
                            {
                                Window.AbilAttLbl.Text = "-";
                            }

                            if (AbilityInfo.Damage > 0)
                            {
                                Window.AbilDamLbl.Text = IntToStr(AbilityInfo.Damage);
                            }
                            else
                            {
                                Window.AbilDamLbl.Text = "-";
                            }

                            Window.AbilStat.Items.Clear();
                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtFire))
                            {
                                var fire = TranslateRStr(FireRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(fire + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtCold))
                            {
                                var cold = TranslateRStr(ColdRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(cold + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtLightning))
                            {
                                var lightning = TranslateRStr(LightningRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(lightning + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtMagic))
                            {
                                var magic = TranslateRStr(MagicRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(magic + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtPoison))
                            {
                                var poison = TranslateRStr(PoisonRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(poison + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtDeath))
                            {
                                var death = TranslateRStr(DeathRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(death + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtHoly))
                            {
                                var holy = TranslateRStr(HolyRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(holy + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtPhysical))
                            {
                                var physical = TranslateRStr(PhysicalRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(physical + " " + e1Damage);
                            }

                            if (AbilityInfo.DamageTypes.Contains(TDamageType.dtWall))
                            {
                                var wall = TranslateRStr(WallRStr);
                                var e1Damage = TranslateRStr(E1DamageRStr);
                                Window.AbilStat.Items.Add(wall + " " + e1Damage);
                            }
                        }
                    }

                    Window.UnitName.Items.Clear();
                    var uName = U.GetName();
                    Window.UnitName.Items.Add(uName);
                    var uHits = U.GetHits();
                    Window.UnitHits.Max = uHits;
                    var uHitPoints = U.GetHitPoints();
                    Window.UnitHits.Position = uHitPoints;
                }
            }

            TCUnit.TCUnitWin.Visible = true;
        }
    }
}
