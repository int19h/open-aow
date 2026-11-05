// Translated from AoWHeroUpgradeDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;
using Aow.Units.AoWButton;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWHeroUpgradeDlg
{
    // close callback, referenced hero, list sort orders, live hero, editable copy
    public partial class THeroUpgradeDlg : TDataModule
    {
        public THeroUpgradeDlg(TComponent owner) : base(owner)
        { }

        public TAOWWindow Dlg;
        public TAOWButton CancelBtn;
        public TAoWFrame BCeltL;
        public TAOWButton Done;
        public TAOWLabel Title;
        public TAOWButton Close;
        public TAOWImage TitleIcon;
        public TAOWImage CeltM;
        public TAOWImage CeltL;
        public TAOWImage CeltR;
        public TAoWFrame TCeltL;
        public TAOWImage TCelt;
        public TAOWPanel UpgradePnl;
        public TAOWLabel HeaderLbl;
        public TAOWLabel SkillPointsLbl;
        public TAOWListBox AvailableAbilities;
        public TAOWVScrollBar AvailableAbilitiesSB;
        public TAOWListBox SelectedAbilities;
        public TAOWVScrollBar SelectedAbilitiesSB;
        public TAOWButton SelectedAbilitiesSort;
        public TAOWButton AvailableAbilityCostSort;
        public TAOWListBox AvailableAbilitiesCost;
        public TAOWButton AvailableAbilitiesSort;
        public TAOWButton AddAbilityBtn;
        public TAOWButton RemoveBtn;
        public TAOWImage FaceFrame;
        public TAOWEdit UnitName;
        public TAOWEdit Alignment;
        public TAOWPanel TopPnl;
        public TAOWPanel StatisicsPnl;
        public TAOWLabel AttValue;
        public TAOWLabel AttLbl;
        public TAOWPanel AttPnl;
        public TAOWLabel DamValue;
        public TAOWLabel DamLbl;
        public TAOWPanel DamPnl;
        public TAOWLabel DefValue;
        public TAOWLabel DefLbl;
        public TAOWPanel DefPnl;
        public TAOWLabel ResValue;
        public TAOWLabel ResLbl;
        public TAOWPanel ResPnl;
        public TAOWLabel HitValue;
        public TAOWLabel HitLbl;
        public TAOWPanel HitPnl;
        public TAOWLabel MovValue;
        public TAOWLabel MovLbl;
        public TAOWPanel MovPnl;
        public TAOWLabel AttI;
        public TAOWLabel DamI;
        public TAOWLabel DefI;
        public TAOWLabel ResI;
        public TAOWLabel HitI;
        public TAOWLabel MovI;
        public TAOWLabel AttA;
        public TAOWLabel DamA;
        public TAOWLabel DefA;
        public TAOWLabel ResA;
        public TAOWLabel HitA;
        public TAOWLabel MovA;
        public TAOWLabel AttP;
        public TAOWLabel DamP;
        public TAOWLabel DefP;
        public TAOWLabel ResP;
        public TAOWLabel HitP;
        public TAOWLabel MovP;
        public TAOWLabel AttB;
        public TAOWLabel DamB;
        public TAOWLabel DefB;
        public TAOWLabel ResB;
        public TAOWLabel HitB;
        public TAOWLabel MovB;
        public TAOWButton AttUp;
        public TAOWButton AttDn;
        public TAOWButton DamUp;
        public TAOWButton DamDn;
        public TAOWButton DefUp;
        public TAOWButton DefDn;
        public TAOWButton ResUp;
        public TAOWButton ResDn;
        public TAOWButton HitUp;
        public TAOWButton HitDn;
        public TAOWButton MovUp;
        public TAOWButton MovDn;
        public TAOWImage AttIc;
        public TAOWImage DamIcn;
        public TAOWImage DefIc;
        public TAOWImage ResIc;
        public TAOWImage HitIc;
        public TAOWImage MovIc;
        public TAOWButton SelectedAbilitiesCostSort;
        public TAOWListBox SelectedAbilitiesCost;
        public TAOWPanel InfoPnl;
        public TAOWLabel LevelLbl;
        public TAOWLabel SkillPoints;
        public TIvTranslator Translator;
        public TExecuteDoneEvent FOnClose;
        public THeroUpgradeEventLog FReferencedHero;
        public int FSelectedSort;
        public int FAvailableSort;
        public THero FHero;
        public THero FEditedHero;
        // AoWHeroUpgradeDlg.pas:287
        public void ClearHero()
        {
            TExecuteDoneEvent OnClose = default;
            THeroUpgradeEventLog Hero = default;
            OnClose = FOnClose;
            Hero = FReferencedHero;
            FOnClose = null;
            FReferencedHero = null;
            if (OnClose != null)
            {
                OnClose(Hero);
            }

            if (Hero != null)
            {
                Hero.Release();
            }

            FEditedHero.Free();
            FEditedHero = null;
        }

        // AoWHeroUpgradeDlg.pas:280
        public bool ValidateHero()
        {
            bool Result = default;
            var call__1 = AoWHSMap.FUnitControl.FindUnit(FReferencedHero.FUnitID);
            FHero = (THero)call__1;
            Result = FHero != null;
            if (!Result)
            {
                Dlg.Visible = false;
            }

            return Result;
        }

        // AoWHeroUpgradeDlg.pas:257
        public void UpdateStatistics()
        {
            var call__2 = FHero.GetInherentAttack();
            AttB.Text = IntToStr(call__2);
            var call__3 = FHero.GetInherentDefense();
            DefB.Text = IntToStr(call__3);
            var call__4 = FHero.GetInherentDamage();
            DamB.Text = IntToStr(call__4);
            var call__5 = FHero.GetInherentResistance();
            ResB.Text = IntToStr(call__5);
            var call__6 = FHero.GetHits();
            HitB.Text = IntToStr(call__6);
            var call__7 = FHero.GetMoves();
            MovB.Text = IntToStr(call__7);
            AttA.Text = IntToStr(FEditedHero.FUnitAttack - FHero.FUnitAttack);
            DefA.Text = IntToStr(FEditedHero.FUnitDefense - FHero.FUnitDefense);
            DamA.Text = IntToStr(FEditedHero.FUnitDamage - FHero.FUnitDamage);
            ResA.Text = IntToStr(FEditedHero.FUnitResistance - FHero.FUnitResistance);
            HitA.Text = IntToStr(FEditedHero.FUnitHits - FHero.FUnitHits);
            MovA.Text = IntToStr(FEditedHero.FUnitMoves - FHero.FUnitMoves);
            var call__8 = FEditedHero.GetInherentAttack();
            AttValue.Text = IntToStr(call__8);
            var call__9 = FEditedHero.GetInherentDefense();
            DefValue.Text = IntToStr(call__9);
            var call__10 = FEditedHero.GetInherentDamage();
            DamValue.Text = IntToStr(call__10);
            var call__11 = FEditedHero.GetInherentResistance();
            ResValue.Text = IntToStr(call__11);
            var call__12 = FEditedHero.GetHits();
            HitValue.Text = IntToStr(call__12);
            var call__13 = FEditedHero.GetMoves();
            MovValue.Text = IntToStr(call__13);
            var call__14 = FEditedHero.GetSkillPoints();
            var call__15 = FHero.GetSkillPoints();
            SkillPoints.Text = IntToStr(call__14) + AnsiString.FromBytes(47) + IntToStr(call__15);
        }

        // AoWHeroUpgradeDlg.pas:252
        public void RefreshStatistics()
        {
            UpdateStatistics();
        }

        // AoWHeroUpgradeDlg.pas:242
        public void SortSelected()
        {
            switch (FSelectedSort)
            {
                case 0:
                    SortStringsAscending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                    break;
                case 1:
                    SortStringsDescending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                    break;
                case 2:
                    SortStringsNumericalAscending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                    break;
                case 3:
                    SortStringsNumericalDescending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                    break;
            }
        }

        // AoWHeroUpgradeDlg.pas:232
        public void SortAvailable()
        {
            switch (FAvailableSort)
            {
                case 0:
                    SortStringsAscending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                    break;
                case 1:
                    SortStringsDescending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                    break;
                case 2:
                    SortStringsNumericalAscending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                    break;
                case 3:
                    SortStringsNumericalDescending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                    break;
            }
        }

        // AoWHeroUpgradeDlg.pas:170
        public void RefreshAbilities()
        {
            AnsiString Name = default;
            AnsiString Cost = default;
            TAbility Ability = default;
            int I = default;
            AvailableAbilities.Items.Clear();
            AvailableAbilitiesCost.Items.Clear();
            {
                I = 1;
                int end__16 = AoWHSSet.FAbilityControl.GetCount() - 1;
                for (; I <= end__16; I += 1)
                {
                    var call__17 = AoWHSSet.FAbilityControl.GetAbility(I);
                    Ability = call__17;
                    bool bool__18 = Ability != null;
                    if (bool__18)
                    {
                        var call__19 = Ability.CanExpand(FEditedHero);
                        bool__18 = call__19;
                    }

                    if (bool__18 && Ability.FSelectionTypes.Contains(TAbilitySelectionType.astHeroUpgrade))
                    {
                        var call__20 = Ability.ExpandName(FEditedHero);
                        AvailableAbilities.Items.AddObject(call__20, IntToObject(I));
                        var call__21 = Ability.ExpandCost(FEditedHero);
                        AvailableAbilitiesCost.Items.Add(IntToStr(call__21));
                    }
                }
            }

            SelectedAbilities.Items.Clear();
            SelectedAbilitiesCost.Items.Clear();
            {
                I = 1;
                int end__22 = FEditedHero.GetAbCount() - 1;
                for (; I <= end__22; I += 1)
                {
                    var call__23 = FEditedHero.GetInherentAbility(I);
                    if (call__23)
                    {
                        var call__24 = AoWHSSet.FAbilityControl.GetAbility(I);
                        Ability = call__24;
                        var call__25 = Ability.GetName(FEditedHero);
                        Name = call__25;
                        var call__26 = FHero.GetAbEnabled(I);
                        bool bool__27 = call__26;
                        if (bool__27)
                        {
                            var call__28 = FHero.GetInherentAbilityLevel(I);
                            var call__29 = FEditedHero.GetInherentAbilityLevel(I);
                            bool__27 = call__28 == call__29;
                        }

                        if (bool__27)
                        {
                            Name = Name + (AnsiString)" (" + TranslateRStr(FixedRStr) + AnsiString.FromBytes(41);
                            Cost = AnsiString.FromBytes(45);
                        }
                        else
                        {
                            var call__30 = Ability.GetSkillPoints(FEditedHero);
                            var call__31 = Ability.GetSkillPoints(FHero);
                            Cost = IntToStr(call__30 - call__31);
                        }

                        SelectedAbilitiesCost.Items.Add(Cost);
                        SelectedAbilities.Items.AddObject(Name, IntToObject(I));
                    }
                }
            }

            var call__32 = AvailableAbilities.Items.Count;
            AvailableAbilitiesSB.Max = call__32 - 1;
            var call__33 = SelectedAbilities.Items.Count;
            SelectedAbilitiesSB.Max = call__33 - 1;
            SortAvailable();
            SortSelected();
        }

        // AoWHeroUpgradeDlg.pas:226
        public void SetAvailableIndex(int Index)
        {
            AvailableAbilities.Selected = Index;
            AvailableAbilitiesCost.Selected = Index;
        }

        // AoWHeroUpgradeDlg.pas:220
        public void SetSelectedIndex(int Index)
        {
            SelectedAbilities.Selected = Index;
            SelectedAbilitiesCost.Selected = Index;
        }

        // AoWHeroUpgradeDlg.pas:212
        public bool CanAddAbility(int AbilityID)
        {
            bool Result = default;
            var call__34 = AoWHSSet.FAbilityControl.GetAbility(AbilityID);
            var call__35 = call__34.CanExpand(FEditedHero);
            bool bool__36 = !call__35;
            if (!bool__36)
            {
                var call__37 = AoWHSSet.FAbilityControl.GetAbility(AbilityID);
                var call__38 = call__37.ExpandCost(FEditedHero);
                var call__39 = FEditedHero.GetSkillPoints();
                bool__36 = call__38 > call__39;
            }

            if (bool__36)
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // AoWHeroUpgradeDlg.pas:302
        public void CloseClick()
        {
            General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWHeroUpgradeDlg.pas:308
        public void FaceFramePreDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
        {
            TRect R = default;
            if (ValidateHero())
            {
                var image = (TAOWImage)Sender;
                var call__41 = Rect(TopPnl.WinLeft + image.WinLeft + 2, TopPnl.WinTop + image.WinTop, TopPnl.WinLeft + image.WinLeft + image.WinWidth - 2, TopPnl.WinTop + image.WinTop + image.WinHeight - 2);
                R = call__41;

                var call__42 = FHero.GetFace();
                call__42.ShowStretched(R, SurfDesc);
            }
        }

        // AoWHeroUpgradeDlg.pas:323
        public void AttUpClick()
        {
            General.PlayButtonSound(0);
            FEditedHero.SetUnitAttack((sbyte)(FEditedHero.FUnitAttack + 1));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:330
        public void AttDnClick()
        {
            General.PlayButtonSound(0);
            if (FEditedHero.FUnitAttack != FHero.FUnitAttack)
            {
                FEditedHero.SetUnitAttack((sbyte)(FEditedHero.FUnitAttack - 1));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:338
        public void DefUpClick()
        {
            General.PlayButtonSound(0);
            FEditedHero.SetUnitDefense((sbyte)(FEditedHero.FUnitDefense + 1));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:345
        public void DefDnClick()
        {
            General.PlayButtonSound(0);
            if (FEditedHero.FUnitDefense != FHero.FUnitDefense)
            {
                FEditedHero.SetUnitDefense((sbyte)(FEditedHero.FUnitDefense - 1));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:353
        public void DamUpClick()
        {
            General.PlayButtonSound(0);
            FEditedHero.SetUnitDamage((sbyte)(FEditedHero.FUnitDamage + 1));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:360
        public void DamDnClick()
        {
            General.PlayButtonSound(0);
            if (FEditedHero.FUnitDamage != FHero.FUnitDamage)
            {
                FEditedHero.SetUnitDamage((sbyte)(FEditedHero.FUnitDamage - 1));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:368
        public void ResUpClick()
        {
            General.PlayButtonSound(0);
            FEditedHero.SetUnitResistance((sbyte)(FEditedHero.FUnitResistance + 1));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:375
        public void ResDnClick()
        {
            General.PlayButtonSound(0);
            if (FEditedHero.FUnitResistance != FHero.FUnitResistance)
            {
                FEditedHero.SetUnitResistance((sbyte)(FEditedHero.FUnitResistance - 1));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:383
        public void HitUpClick()
        {
            General.PlayButtonSound(0);
            FEditedHero.SetUnitHits((sbyte)(FEditedHero.FUnitHits + 1));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:390
        public void HitDnClick()
        {
            General.PlayButtonSound(0);
            if (FEditedHero.FUnitHits != FHero.FUnitHits)
            {
                FEditedHero.SetUnitHits((sbyte)(FEditedHero.FUnitHits - 1));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:398
        public void MovUpClick()
        {
            General.PlayButtonSound(0);
            FEditedHero.SetUnitMoves((sbyte)(FEditedHero.FUnitMoves + 1));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:405
        public void MovDnClick()
        {
            General.PlayButtonSound(0);
            if (FEditedHero.FUnitMoves != FHero.FUnitMoves)
            {
                FEditedHero.SetUnitMoves((sbyte)(FEditedHero.FUnitMoves - 1));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:413
        public void SelectedAbilitiesChange()
        {
            SetSelectedIndex(SelectedAbilities.Selected);
        }

        // AoWHeroUpgradeDlg.pas:418
        public void SelectedAbilitiesCostChange()
        {
            SetSelectedIndex(SelectedAbilitiesCost.Selected);
        }

        // AoWHeroUpgradeDlg.pas:423
        public void SelectedAbilitiesSBChange()
        {
            SelectedAbilities.ListOffset = SelectedAbilitiesSB.Position;
            SelectedAbilitiesCost.ListOffset = SelectedAbilitiesSB.Position;
        }

        // AoWHeroUpgradeDlg.pas:429
        public void AvailableAbilitiesChange()
        {
            SetAvailableIndex(AvailableAbilities.Selected);
        }

        // AoWHeroUpgradeDlg.pas:434
        public void AvailableAbilitiesCostChange()
        {
            SetAvailableIndex(AvailableAbilitiesCost.Selected);
        }

        // AoWHeroUpgradeDlg.pas:439
        public void AvailableAbilitiesSBChange()
        {
            AvailableAbilities.ListOffset = AvailableAbilitiesSB.Position;
            AvailableAbilitiesCost.ListOffset = AvailableAbilitiesSB.Position;
        }

        // AoWHeroUpgradeDlg.pas:445
        public void AddAbilityBtnClick()
        {
            TAOWListBox List = default;
            int Index = default;
            int AbilityID = default;
            General.PlayButtonSound(0);
            List = AvailableAbilities;
            Index = List.Selected;
            if (Index != -1)
            {
                var call__43 = List.Items.Objects[Index];
                AbilityID = ObjectToInt(call__43);
                if (CanAddAbility(AbilityID))
                {
                    FEditedHero.ExpandAbility(AbilityID);
                    RefreshAbilities();
                    RefreshStatistics();
                }
            }
        }

        // AoWHeroUpgradeDlg.pas:465
        public void RemoveBtnClick()
        {
            TAOWListBox List = default;
            int Index = default;
            int AbilityID = default;
            General.PlayButtonSound(0);
            List = SelectedAbilities;
            Index = List.Selected;
            if (Index != -1)
            {
                var call__44 = List.Items.Objects[Index];
                AbilityID = ObjectToInt(call__44);
                var call__45 = FHero.GetInherentAbility(AbilityID);
                if (call__45)
                {
                    var call__46 = FHero.GetInherentAbilityLevel(AbilityID);
                    var call__47 = FEditedHero.GetInherentAbilityLevel(AbilityID);
                    if (call__46 == call__47)
                    {
                        return;
                    }
                }

                FEditedHero.RemoveAbility(AbilityID);
                RefreshAbilities();
                RefreshStatistics();
            }
        }

        // AoWHeroUpgradeDlg.pas:485
        public void SelectedAbilitiesSortClick()
        {
            General.PlayButtonSound(0);
            if (FSelectedSort == 0)
            {
                FSelectedSort = 1;
            }
            else
            {
                FSelectedSort = 0;
            }

            SortSelected();
        }

        // AoWHeroUpgradeDlg.pas:493
        public void SelectedAbilitiesCostSortClick()
        {
            General.PlayButtonSound(0);
            if (FSelectedSort == 2)
            {
                FSelectedSort = 3;
            }
            else
            {
                FSelectedSort = 2;
            }

            SortSelected();
        }

        // AoWHeroUpgradeDlg.pas:501
        public void AvailableAbilitiesSortClick()
        {
            General.PlayButtonSound(0);
            if (FAvailableSort == 0)
            {
                FAvailableSort = 1;
            }
            else
            {
                FAvailableSort = 0;
            }

            SortAvailable();
        }

        // AoWHeroUpgradeDlg.pas:509
        public void AvailableAbilityCostSortClick()
        {
            General.PlayButtonSound(0);
            if (FAvailableSort == 2)
            {
                FAvailableSort = 3;
            }
            else
            {
                FAvailableSort = 2;
            }

            SortAvailable();
        }

        // AoWHeroUpgradeDlg.pas:517
        public void DoneClick()
        {
            THeroUpgradeTE Event = default;
            if (ValidateHero())
            {
                var call__48 = AoWHSMap.FPlayers.GetPlayers(FHero.FPlayer);
                var call__49 = call__48.GetBusy();
                if (!call__49)
                {
                    Event = new THeroUpgradeTE(null);
                    Event.Setup(FHero);
                    Event.FAttack = (sbyte)(FEditedHero.FUnitAttack - FHero.FUnitAttack);
                    Event.FDefense = (sbyte)(FEditedHero.FUnitDefense - FHero.FUnitDefense);
                    Event.FDamage = (sbyte)(FEditedHero.FUnitDamage - FHero.FUnitDamage);
                    Event.FResistance = (sbyte)(FEditedHero.FUnitResistance - FHero.FUnitResistance);
                    Event.FHits = (sbyte)(FEditedHero.FUnitHits - FHero.FUnitHits);
                    Event.FMoves = (sbyte)(FEditedHero.FUnitMoves - FHero.FUnitMoves);
                    Event.FAbilities.AssignAb(FEditedHero);
                    var call__50 = AoWHSMap.FTokenManager.GetTokenControl(FHero.FPlayer);
                    call__50.AddEvent(Event);
                    Event.Release();
                    Dlg.Visible = false;
                }
            }
        }

        // AoWHeroUpgradeDlg.pas:539
        public void DlgHide()
        {
            ClearHero();
        }

        // AoWHeroUpgradeDlg.pas:544
        public void AvailableAbilitiesMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility Ability = default;
            if (Button == TMouseButton.mbRight && AvailableAbilities.Selected != -1)
            {
                var call__51 = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
                var call__52 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__51));
                Ability = call__52;
                if (Ability != null)
                {
                    var call__53 = Ability.GetName(null);
                    SetInfoBanner(InfoBanner, call__53, Ability.FDescription);
                }
            }
        }

        // AoWHeroUpgradeDlg.pas:556
        public void SelectedAbilitiesMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility Ability = default;
            if (Button == TMouseButton.mbRight && SelectedAbilities.Selected != -1)
            {
                var call__54 = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
                var call__55 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__54));
                Ability = call__55;
                if (Ability != null)
                {
                    var call__56 = Ability.GetName(null);
                    SetInfoBanner(InfoBanner, call__56, Ability.FDescription);
                }
            }
        }

        // AoWHeroUpgradeDlg.pas:568
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWHeroUpgradeDlg
    {
    }
}
