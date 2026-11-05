// Translated from AoWLeaderSetup.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWButton;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.aowSetupEvents.aowSetupEvents;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.E2Globals.E2Globals;

namespace Aow.Units.AoWLeaderSetup
{
    // setup control, phase, fixed spheres, working leader, selected sort, available sort
    public partial class TLeaderSetupWin : TDataModule
    {
        public TLeaderSetupWin(TComponent owner) : base(owner)
        { }

        public TAOWListBox SelectedAbilities;
        public TAOWPanel Phase2Pnl;
        public TAOWPanel LMainPnl;
        public TAOWButton NextBtn;
        public TAOWButton DefaultBtn;
        public TAOWButton ClearBtn;
        public TAOWButton PrevBtn;
        public TAOWPanel EditStatisticsPnl;
        public TAOWListBox AvailableAbilities;
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
        public TAOWLabel SkillPointsLbl;
        public TAOWImage UAttIcn;
        public TAOWImage UDefIcn;
        public TAOWImage UMovIcn;
        public TAOWImage UDamIcn;
        public TAOWImage UResIcn;
        public TAOWImage UHitIcn;
        public TAOWLabel UAttack;
        public TAOWLabel UDefense;
        public TAOWLabel UMoves;
        public TAOWLabel UDamage;
        public TAOWLabel UResistance;
        public TAOWLabel UHits;
        public TAOWPanel StatPnl;
        public TAOWListBox UAbilities;
        public TAOWPanel AbilityPnl;
        public TAOWPanel UStatP1;
        public TAOWPanel UStatP2;
        public TAOWLabel LeaderName;
        public TAOWImage ShieldL;
        public TAOWImage ShieldR;
        public TAOWPanel ButtonPnl;
        public TAOWPanel LeftPnl;
        public TAOWPanel RightPnl;
        public TAOWLabel LeaderStatsLbl;
        public TAOWLabel LeaderAbilLbl;
        public TAOWLabel FullAbilLbl;
        public TAOWButton RemoveAbility;
        public TAOWButton AddAbility;
        public TAOWImage MainFace;
        public TAOWPanel Phase1Pnl;
        public TAOWImage Face4;
        public TAOWImage Face5;
        public TAOWImage Face6;
        public TAOWImage Face7;
        public TAOWImage Face3;
        public TAOWImage Face2;
        public TAOWImage Face1;
        public TAOWEdit NameEdit;
        public TAOWLabel NameLbl;
        public TAOWLabel FaceLbl;
        public TAOWLabel RaceLbl;
        public TAOWLabel Phase1Header;
        public TAOWVScrollBar UAbilSB;
        public TAOWVScrollBar AvailableAbilitiesLB;
        public TAOWVScrollBar SelectedAbilitiesLB;
        public TAOWPanel Phase3Pnl;
        public TAOWLabel SpheresLbl;
        public TAOWButton SelDeath;
        public TAOWPanel SpherePnl;
        public TAOWButton SelLife;
        public TAOWButton SelFire;
        public TAOWButton SelEarth;
        public TAOWButton SelAir;
        public TAOWButton SelWater;
        public TAOWLabel LifeLbl;
        public TAOWButton Sel4;
        public TAOWPanel SphereSelectionPnl;
        public TAOWButton Sel3;
        public TAOWButton Sel2;
        public TAOWButton Sel1;
        public TAOWButton Sel5;
        public TAOWButton Sel6;
        public TAOWButton Sel7;
        public TAOWLabel DeathLbl;
        public TAOWLabel EarthLbl;
        public TAOWLabel WaterLbl;
        public TAOWLabel FireLbl;
        public TAOWLabel AirLbl;
        public TAOWLabel SelectedSpheresLbl;
        public TAOWButton FaceNextBtn;
        public TAOWButton FacePrevBtn;
        public TAOWLabel LeaderLbl;
        public TAOWLabel SideNameLbl;
        public TAOWListBox SelectedAbilitiesCost;
        public TAOWListBox AvailableAbilitiesCost;
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
        public TAOWButton SBase;
        public TAOWButton SExtra;
        public TAOWButton SFinal;
        public TAOWButton SelectedAbilitiesSort;
        public TAOWButton SelectedAbilitiesCostSort;
        public TAOWButton AvailableAbilitiesSort;
        public TAOWButton AvailableAbilitiesCostSort;
        public TAoWFrame MainFaceFr;
        public TAoWFrame FFr1;
        public TAoWFrame FFr7;
        public TAoWFrame FFr2;
        public TAoWFrame FFr6;
        public TAoWFrame FFr3;
        public TAoWFrame FFr5;
        public TAoWFrame FFr4;
        public TAOWLabel Phase2Header;
        public TAOWLabel Phase3Header;
        public TAOWPanel LeaderPnl;
        public TAOWComboBox RaceCB;
        public TIvTranslator Translator;
        public TLeaderSetupControl FSetup;
        public int FPhase;
        public int FFixedSpheres;
        public TLeader FLeader;
        public int FSelectedSort;
        public int FAvailableSort;
        // AoWLeaderSetup.pas:1050
        public void Execute(TLeaderSetupControl Control)
        {
            int I = default;
            Reset();
            if (FSetup != null)
            {
                FSetup.Release();
            }

            FSetup = Control;
            FSetup.AddRef();
            FSelectedSort = 0;
            FAvailableSort = 0;
            LeaderPnl.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LeaderPnl.ILIndexTexture = 4;
            RaceCB.Visible = FSetup.FItems.Count != 0 && FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign);
            RaceLbl.Visible = RaceCB.Visible;
            if (RaceCB.Visible)
            {
                RaceCB.Items.Clear();
                {
                    I = 0;
                    int end__1 = FSetup.FItems.Count - 1;
                    for (; I <= end__1; I += 1)
                    {
                        var call__2 = FSetup.FItems[I];
                        var call__3 = TranslateRStr(RaceNamesRStr[call__2]);
                        var call__4 = FSetup.FItems[I];
                        RaceCB.Items.AddObject(call__3, IntToObject(call__4));
                    }
                }

                var call__5 = FSetup.FItems.IndexOf(FSetup.FOriginalLeader.FRace);
                RaceCB.Selected = call__5;
            }

            var call__6 = TranslateRStr(RaceNamesPluralRStr[FSetup.FOriginalLeader.FRace]);
            SideNameLbl.Text = call__6;
            ShieldL.CurrentImage = FSetup.FOriginalLeader.FRace + 13;
            ShieldR.CurrentImage = FSetup.FOriginalLeader.FRace + 13;
            ShieldR.Visible = !FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign);
            LoadLeader();
            SaveLeader();
            if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmLook))
            {
                SetPhase(1);
            }
            else
            {
                if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmSettings))
                {
                    SetPhase(2);
                }
                else
                {
                    if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmSpheres))
                    {
                        SetPhase(3);
                    }
                }
            }

            LeaderPnl.Visible = true;
        }

        // AoWLeaderSetup.pas:1083
        public void Finish()
        {
            TLeaderSetupControl Control = default;
            SaveLeader();
            Control = FSetup;
            Control.FStatus = TLeaderSetupStatus.lssDone;
            FSetup = null;
            LeaderPnl.Visible = false;
            SetupEvents.UpdatePlayerName(Control.FOriginalLeader.FHeroName);
            Control.Done();
            Control.Release();
        }

        // AoWLeaderSetup.pas:1097
        public void Cancel()
        {
            TLeaderSetupControl Control = default;
            SaveLeader();
            Control = FSetup;
            Control.FStatus = TLeaderSetupStatus.lssCanceled;
            FSetup = null;
            LeaderPnl.Visible = false;
            Control.Done();
            Control.Release();
        }

        // AoWLeaderSetup.pas:750
        public void LoadLeader()
        {
            FLeader.Assign(FSetup.FOriginalLeader, AoWEngine);
        }

        // AoWLeaderSetup.pas:755
        public void SaveLeader()
        {
            TLeader Leader = default;
            FSetup.FOriginalLeader.Assign(FLeader, AoWEngine);
            Leader = FSetup.FOriginalLeader;
            LeaderName.Text = Leader.FHeroName;
            var call__7 = Leader.GetInherentAttack();
            UAttack.Text = IntToStr(call__7);
            var call__8 = Leader.GetInherentDefense();
            UDefense.Text = IntToStr(call__8);
            var call__9 = Leader.GetInherentDamage();
            UDamage.Text = IntToStr(call__9);
            var call__10 = Leader.GetInherentResistance();
            UResistance.Text = IntToStr(call__10);
            var call__11 = Leader.GetHits();
            UHits.Text = IntToStr(call__11);
            var call__12 = Leader.GetMoves();
            UMoves.Text = IntToStr(call__12);
            UAbilities.Items.Clear();
            Leader.ListAbilities(UAbilities.Items);
            var call__13 = TranslateRStr(RaceNamesPluralRStr[Leader.FRace]);
            SideNameLbl.Text = call__13;
            ShieldL.CurrentImage = Leader.FRace + 13;
            ShieldR.CurrentImage = Leader.FRace + 13;
        }

        // AoWLeaderSetup.pas:775
        public void UpdateSkillPoints()
        {
            var call__14 = FLeader.GetSkillPoints();
            var call__15 = TranslateRStr(SkillPointsRStr);
            var call__16 = FLeader.GetSkillPointsMax();
            SkillPointsLbl.Text = call__15 + AnsiString.FromBytes(32) + IntToStr(call__14) + AnsiString.FromBytes(47) + IntToStr(call__16);
        }

        // AoWLeaderSetup.pas:781
        public void UpdateStatistics()
        {
            THeroResource Res = default;
            Res = FLeader.FHeroResource;
            AttB.Text = IntToStr(Res.FAttack);
            DefB.Text = IntToStr(Res.FDefense);
            DamB.Text = IntToStr(Res.FDamage);
            ResB.Text = IntToStr(Res.FResistance);
            HitB.Text = IntToStr(Res.FHits);
            MovB.Text = IntToStr(Res.FMoves);
            AttA.Text = IntToStr(FLeader.FUnitAttack);
            DefA.Text = IntToStr(FLeader.FUnitDefense);
            DamA.Text = IntToStr(FLeader.FUnitDamage);
            ResA.Text = IntToStr(FLeader.FUnitResistance);
            HitA.Text = IntToStr(FLeader.FUnitHits);
            MovA.Text = IntToStr(FLeader.FUnitMoves);
            var call__17 = FLeader.GetInherentAttack();
            AttValue.Text = IntToStr(call__17);
            var call__18 = FLeader.GetInherentDefense();
            DefValue.Text = IntToStr(call__18);
            var call__19 = FLeader.GetInherentDamage();
            DamValue.Text = IntToStr(call__19);
            var call__20 = FLeader.GetInherentResistance();
            ResValue.Text = IntToStr(call__20);
            var call__21 = FLeader.GetHits();
            HitValue.Text = IntToStr(call__21);
            var call__22 = FLeader.GetMoves();
            MovValue.Text = IntToStr(call__22);
            UpdateSkillPoints();
        }

        // AoWLeaderSetup.pas:807
        public void UpdateLeader()
        {
            NameEdit.Text = FLeader.FHeroName;
            UpdateAbilities();
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:814
        public void SortSelectedAbilities()
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

        // AoWLeaderSetup.pas:830
        public void SortAvailableAbilities()
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

        // AoWLeaderSetup.pas:852
        public void UpdateAbilities()
        {
            int I = default;
            AnsiString Name = default;
            AnsiString Cost = default;
            TAbility Ability = default;
            AvailableAbilities.Items.Clear();
            AvailableAbilitiesCost.Items.Clear();
            {
                I = 0;
                int end__23 = AoWHSSet.FAbilityControl.GetCount() - 1;
                for (; I <= end__23; I += 1)
                {
                    var call__24 = AoWHSSet.FAbilityControl.GetAbility(I);
                    bool bool__25 = call__24 != null;
                    if (bool__25)
                    {
                        var call__26 = AoWHSSet.FAbilityControl.GetAbility(I);
                        bool__25 = (call__26.FSelectionTypes & TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astCustomizeLeader, TAbilitySelectionType.astHeroUpgrade)) != TSet<TAbilitySelectionType>.Of();
                    }

                    if (bool__25)
                    {
                        var call__27 = AoWHSSet.FAbilityControl.GetAbility(I);
                        var call__28 = call__27.CanExpand(FLeader);
                        if (call__28)
                        {
                            var call__29 = AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__30 = call__29.ExpandName(FLeader);
                            AvailableAbilities.Items.AddObject(call__30, IntToObject(I));
                            var call__31 = AoWHSSet.FAbilityControl.GetAbility(I);
                            var call__32 = call__31.ExpandCost(FLeader);
                            AvailableAbilitiesCost.Items.Add(IntToStr(call__32));
                        }
                    }
                }
            }

            SelectedAbilities.Items.Clear();
            SelectedAbilitiesCost.Items.Clear();
            {
                I = 0;
                int end__33 = FLeader.GetAbCount() - 1;
                for (; I <= end__33; I += 1)
                {
                    var call__34 = AoWHSSet.FAbilityControl.GetAbility(I);
                    Ability = call__34;
                    var call__35 = FLeader.GetAbSet(I);
                    bool bool__36 = call__35;
                    if (bool__36)
                    {
                        var call__37 = FLeader.FHeroResource.FAbilities.GetAbSet(I);
                        bool__36 = call__37 || (Ability.FSelectionTypes & TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astCustomizeLeader, TAbilitySelectionType.astHeroUpgrade)) != TSet<TAbilitySelectionType>.Of();
                    }

                    if (bool__36)
                    {
                        var call__38 = AoWHSSet.FAbilityControl.GetAbility(I);
                        var call__39 = call__38.GetName(FLeader);
                        Name = call__39;
                        var call__40 = FLeader.FHeroResource.FAbilities.GetAbSet(I);
                        bool bool__41 = call__40;
                        if (bool__41)
                        {
                            var call__42 = FLeader.FHeroResource.FAbilities.GetAbLevel(I);
                            var call__43 = FLeader.GetAbilityLevel(I);
                            bool__41 = call__42 == call__43;
                        }

                        if (bool__41)
                        {
                            Name = Name + (AnsiString)" (" + TranslateRStr(FixedRStr) + AnsiString.FromBytes(41);
                            Cost = AnsiString.FromBytes(45);
                        }
                        else
                        {
                            var call__44 = Ability.GetSkillPoints(FLeader);
                            var call__45 = Ability.GetSkillPoints(FLeader.FHeroResource.FAbilities);
                            Cost = IntToStr(call__44 - call__45);
                        }

                        SelectedAbilitiesCost.Items.Add(Cost);
                        SelectedAbilities.Items.AddObject(Name, IntToObject(I));
                    }
                }
            }

            var call__46 = AvailableAbilities.Items.Count;
            AvailableAbilitiesLB.Max = call__46 - 1;
            var call__47 = SelectedAbilities.Items.Count;
            SelectedAbilitiesLB.Max = call__47 - 1;
            SortAvailableAbilities();
            SortSelectedAbilities();
        }

        // AoWLeaderSetup.pas:840
        public void SelectAvailableAbility(int Index)
        {
            AvailableAbilities.Selected = Index;
            AvailableAbilitiesCost.Selected = Index;
        }

        // AoWLeaderSetup.pas:824
        public void SelectSelectedAbility(int Index)
        {
            SelectedAbilities.Selected = Index;
            SelectedAbilitiesCost.Selected = Index;
        }

        // AoWLeaderSetup.pas:846
        public bool CanAddAbility(int AbilityID)
        {
            bool Result = default;
            var call__48 = AoWHSSet.FAbilityControl.GetAbility(AbilityID);
            var call__49 = call__48.CanExpand(FLeader);
            bool bool__50 = call__49;
            if (bool__50)
            {
                var call__51 = AoWHSSet.FAbilityControl.GetAbility(AbilityID);
                var call__52 = call__51.ExpandCost(FLeader);
                var call__53 = FLeader.GetSkillPoints();
                bool__50 = call__52 <= call__53;
            }

            Result = bool__50;
            return Result;
        }

        // AoWLeaderSetup.pas:913
        public void SetPhase(int Value)
        {
            FPhase = Value;
            Phase1Pnl.Visible = Value == 1;
            Phase2Pnl.Visible = Value == 2;
            Phase3Pnl.Visible = Value == 3;
            Phase1Header.Visible = Value == 1;
            Phase2Header.Visible = Value == 2;
            Phase3Header.Visible = Value == 3;
            StatPnl.Visible = Value > 2;
            AbilityPnl.Visible = Value > 2;
            LeaderName.Visible = Value > 1;
            LeaderLbl.Visible = !FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign);
            SideNameLbl.Visible = !FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign);
            ShieldL.Visible = !FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign) || Value > 1;
            ShieldR.Visible = !FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign) || Value > 1;
            MainFaceFr.Visible = !FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign) || Value > 1;
            MainFace.Visible = MainFaceFr.Visible;
            if (Value == 3 || (Value == 2 && (FSetup.FMode & TSet<TLeaderSetupModeElement>.Of(TLeaderSetupModeElement.lsmSpheres)) == TSet<TLeaderSetupModeElement>.Of()) || (Value == 1 && (FSetup.FMode & TSet<TLeaderSetupModeElement>.Of(TLeaderSetupModeElement.lsmSettings, TLeaderSetupModeElement.lsmSpheres)) == TSet<TLeaderSetupModeElement>.Of()))
            {
                var call__54 = TranslateRStr(FinishRStr);
                NextBtn.Text = call__54;
            }
            else
            {
                var call__55 = TranslateRStr(NextRStr);
                NextBtn.Text = call__55;
            }

            switch (Value)
            {
                case 1:
                    UpdateLeader();
                    break;
                case 2:
                    UpdateLeader();
                    break;
                case 3:
                    UpdateSpheres();
                    break;
            }
        }

        // AoWLeaderSetup.pas:895
        public void SetFixedSpheres(int Value)
        {
            FFixedSpheres = Value;
            UpdateSpheres();
        }

        // AoWLeaderSetup.pas:901
        public void Reset()
        {
            ClearBtn.Visible = true;
            DefaultBtn.Visible = true;
            PrevBtn.Visible = true;
            NextBtn.Visible = true;
            var call__56 = TranslateRStr(Step1CustomizeLeaderRStr);
            Phase1Header.Text = call__56;
            var call__57 = TranslateRStr(Step2AssignSkillPointsRStr);
            Phase2Header.Text = call__57;
            var call__58 = TranslateRStr(Step3AssignSpheresRStr);
            Phase3Header.Text = call__58;
            SetFixedSpheres(0);
        }

        // AoWLeaderSetup.pas:943
        public void UpdateSpheres()
        {
            int I = default;
            TAOWButton Btn = default;
            {
                I = 0;
                byte end__59 = 6;
                for (; I <= end__59; I += 1)
                {
                    switch (I)
                    {
                        case 0:
                            Btn = Sel1;
                            break;
                        case 1:
                            Btn = Sel2;
                            break;
                        case 2:
                            Btn = Sel3;
                            break;
                        case 3:
                            Btn = Sel4;
                            break;
                        case 4:
                            Btn = Sel5;
                            break;
                        case 5:
                            Btn = Sel6;
                            break;
                        case 6:
                            Btn = Sel7;
                            break;
                        default:
                            Btn = null;
                            break;
                    }

                    var call__60 = FLeader.GetSphereCount();
                    if (I < call__60)
                    {
                        Btn.Visible = true;
                        var call__61 = FLeader.GetSpheres(I);
                        switch ((int)call__61 & 127)
                        {
                            case 1:
                                Btn.ILIndexDown = 28;
                                Btn.ILIndexLit = 29;
                                Btn.ILIndexUp = 27;
                                break;
                            case 5:
                                Btn.ILIndexDown = 43;
                                Btn.ILIndexLit = 44;
                                Btn.ILIndexUp = 42;
                                break;
                            case 3:
                                Btn.ILIndexDown = 34;
                                Btn.ILIndexLit = 35;
                                Btn.ILIndexUp = 33;
                                break;
                            case 4:
                                Btn.ILIndexDown = 37;
                                Btn.ILIndexLit = 38;
                                Btn.ILIndexUp = 36;
                                break;
                            case 6:
                                Btn.ILIndexDown = 40;
                                Btn.ILIndexLit = 41;
                                Btn.ILIndexUp = 39;
                                break;
                            case 2:
                                Btn.ILIndexDown = 31;
                                Btn.ILIndexLit = 32;
                                Btn.ILIndexUp = 30;
                                break;
                        }
                    }
                    else
                    {
                        Btn.Visible = false;
                    }

                    if (I < FFixedSpheres)
                    {
                        Btn.FCaptureInput = false;
                        Btn.ILIndexUp = Btn.ILIndexUp + 1;
                    }
                    else
                    {
                        Btn.FCaptureInput = true;
                    }
                }
            }

            var call__62 = FLeader.GetSphereCount();
            var call__63 = TranslateRStr(SelectedSpheresRStr);
            SelectedSpheresLbl.Text = call__63 + (AnsiString)" (" + IntToStr(call__62) + AnsiString.FromBytes(47) + IntToStr(FLeader.FSpherePicksMax) + AnsiString.FromBytes(41);
            var call__64 = FLeader.CanAddSphere(TMagicSphere.msLife);
            if (call__64)
            {
                SelLife.ILIndexUp = 27;
                SelLife.FCaptureInput = true;
            }
            else
            {
                SelLife.ILIndexUp = 28;
                SelLife.FCaptureInput = false;
            }

            var call__65 = FLeader.CanAddSphere(TMagicSphere.msDeath);
            if (call__65)
            {
                SelDeath.ILIndexUp = 30;
                SelDeath.FCaptureInput = true;
            }
            else
            {
                SelDeath.ILIndexUp = 31;
                SelDeath.FCaptureInput = false;
            }

            var call__66 = FLeader.CanAddSphere(TMagicSphere.msFire);
            if (call__66)
            {
                SelFire.ILIndexUp = 42;
                SelFire.FCaptureInput = true;
            }
            else
            {
                SelFire.ILIndexUp = 43;
                SelFire.FCaptureInput = false;
            }

            var call__67 = FLeader.CanAddSphere(TMagicSphere.msWater);
            if (call__67)
            {
                SelWater.ILIndexUp = 39;
                SelWater.FCaptureInput = true;
            }
            else
            {
                SelWater.ILIndexUp = 40;
                SelWater.FCaptureInput = false;
            }

            var call__68 = FLeader.CanAddSphere(TMagicSphere.msEarth);
            if (call__68)
            {
                SelEarth.ILIndexUp = 33;
                SelEarth.FCaptureInput = true;
            }
            else
            {
                SelEarth.ILIndexUp = 34;
                SelEarth.FCaptureInput = false;
            }

            var call__69 = FLeader.CanAddSphere(TMagicSphere.msAir);
            if (call__69)
            {
                SelAir.ILIndexUp = 36;
                SelAir.FCaptureInput = true;
            }
            else
            {
                SelAir.ILIndexUp = 37;
                SelAir.FCaptureInput = false;
            }

            SelLife.FDrawn = false;
            SelDeath.FDrawn = false;
            SelFire.FDrawn = false;
            SelWater.FDrawn = false;
            SelEarth.FDrawn = false;
            SelAir.FDrawn = false;
        }

        // AoWLeaderSetup.pas:269
        public void LeaderSetupWinCreate()
        {
            var call__70 = new TLeader(null);
            FLeader = call__70;
        }

        // AoWLeaderSetup.pas:274
        public void LeaderSetupWinDestroy()
        {
            FLeader.Free();
        }

        // AoWLeaderSetup.pas:279
        public void AddAbilityClick()
        {
            int AbilityID = default;
            General.PlayButtonSound(0);
            if (AvailableAbilities.Selected != -1)
            {
                var call__71 = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
                AbilityID = ObjectToInt(call__71);
                if (CanAddAbility(AbilityID))
                {
                    FLeader.ExpandAbility(AbilityID);
                    UpdateAbilities();
                    UpdateLeader();
                }
            }
        }

        // AoWLeaderSetup.pas:296
        public void RemoveAbilityClick()
        {
            int AbilityID = default;
            General.PlayButtonSound(0);
            if (SelectedAbilities.Selected != -1)
            {
                var call__72 = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
                AbilityID = ObjectToInt(call__72);
                var call__73 = FLeader.FHeroResource.FAbilities.GetAbSet(AbilityID);
                bool bool__74 = call__73;
                if (bool__74)
                {
                    var call__75 = FLeader.FHeroResource.FAbilities.GetAbLevel(AbilityID);
                    var call__76 = FLeader.GetAbilityLevel(AbilityID);
                    bool__74 = call__75 == call__76;
                }

                if (bool__74)
                {
                    return;
                }

                FLeader.RemoveAbility(AbilityID);
                UpdateAbilities();
                UpdateLeader();
            }
        }

        // AoWLeaderSetup.pas:312
        public void DrawSelectionFace(TObject Sender, TSurfaceDesc SD)
        {
            TLeaderSetupControl Setup = default;
            int Index = default;
            int Offset = default;
            TRect R = default;
            TFaceResource Face = default;
            Setup = FSetup;
            Offset = ((TComponent)Sender).Tag;
            Index = FLeader.FFaceIndex;
            while (Offset < 0)
            {
                Offset += 1;
                var call__77 = AoWHSSet.FFaceResources.PrevFace(Index);
                Index = call__77;
            }

            while (Offset > 0)
            {
                Offset -= 1;
                var call__78 = AoWHSSet.FFaceResources.NextFace(Index);
                Index = call__78;
            }

            var image = (TAOWImage)Sender;
            var call__80 = Rect(Phase1Pnl.WinLeft + image.WinLeft + 2, Phase1Pnl.WinTop + image.WinTop, Phase1Pnl.WinLeft + image.WinLeft + image.WinWidth - 2, Phase1Pnl.WinTop + image.WinTop + image.WinHeight - 2);
            R = call__80;

            var call__81 = Setup.FOriginalLeader.GetFace();
            if (call__81 != null)
            {
                var call__82 = AoWHSSet.FFaceResources.GetFaceResource(Index);
                Face = call__82;
                Face.AddRef();
                var call__83 = Face.GetFaceImage();
                call__83.ShowStretched(R, SD);
                Face.Release();
            }
        }

        // AoWLeaderSetup.pas:345
        public void FacePrevBtnClick()
        {
            General.PlayButtonSound(0);
            var call__84 = AoWHSSet.FFaceResources.PrevFace(FLeader.FFaceIndex);
            FLeader.SetFaceIndex(call__84);
            TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWLeaderSetup.pas:352
        public void FaceNextBtnClick()
        {
            General.PlayButtonSound(0);
            var call__85 = AoWHSSet.FFaceResources.NextFace(FLeader.FFaceIndex);
            FLeader.SetFaceIndex(call__85);
            TitleWindow.TitleWin.FDrawn = false;
        }

        // AoWLeaderSetup.pas:359
        public void PrevBtnClick()
        {
            General.PlayButtonSound(0);
            switch (FPhase)
            {
                case 1:
                    if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign))
                    {
                        Cancel();
                    }

                    break;
                case 2:
                    SaveLeader();
                    if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmLook))
                    {
                        SetPhase(1);
                    }

                    break;
                case 3:
                    SaveLeader();
                    if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmSettings))
                    {
                        SetPhase(2);
                    }
                    else
                    {
                        if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmLook))
                        {
                            SetPhase(1);
                        }
                    }

                    break;
            }
        }

        // AoWLeaderSetup.pas:376
        public void NextBtnClick()
        {
            General.PlayButtonSound(0);
            switch (FPhase)
            {
                case 1:
                    SaveLeader();
                    if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmSettings))
                    {
                        SetPhase(2);
                    }
                    else
                    {
                        if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmSpheres))
                        {
                            SetPhase(3);
                        }
                        else
                        {
                            Finish();
                        }
                    }

                    TitleWindow.TitleWin.FDrawn = false;
                    break;
                case 2:
                    SaveLeader();
                    if (FSetup.FMode.Contains(TLeaderSetupModeElement.lsmSpheres))
                    {
                        SetPhase(3);
                    }
                    else
                    {
                        Finish();
                    }

                    TitleWindow.TitleWin.FDrawn = false;
                    break;
                case 3:
                    var call__86 = FLeader.GetSphereCount();
                    if (call__86 == FLeader.FSpherePicksMax)
                    {
                        Finish();
                    }
                    else
                    {
                        ResetMessageDialog(AoWMessageDlg.AoWMessageDlg.MessageDlg);
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
                        var call__87 = TranslateRStr(PleaseAssignAllSpheresRStr);
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__87);
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
                        AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
                    }

                    break;
            }
        }

        // AoWLeaderSetup.pas:405
        public void MainFaceFrPreDraw(TObject Sender, TSurfaceDesc SD)
        {
            TLeaderSetupControl Setup = default;
            TRect R = default;
            Setup = FSetup;
            if (FPhase > 1)
            {
                var frame = (TAoWFrame)Sender;
                var call__89 = Rect(LMainPnl.WinLeft + frame.WinLeft + 2, LMainPnl.WinTop + frame.WinTop, LMainPnl.WinLeft + frame.WinLeft + frame.WinWidth - 2, LMainPnl.WinTop + frame.WinTop + frame.WinHeight - 2);
                R = call__89;

                var call__90 = Setup.FOriginalLeader.GetFace();
                if (call__90 != null)
                {
                    var call__91 = Setup.FOriginalLeader.GetFace();
                    call__91.ShowStretched(R, SD);
                }
            }
        }

        // AoWLeaderSetup.pas:422
        public void NameEditChange()
        {
            FLeader.FHeroName = NameEdit.Text;
        }

        // AoWLeaderSetup.pas:427
        public void SelectedAbilitiesCostChange()
        {
            SelectSelectedAbility(SelectedAbilitiesCost.Selected);
        }

        // AoWLeaderSetup.pas:432
        public void SelectedAbilitiesChange()
        {
            SelectSelectedAbility(SelectedAbilities.Selected);
        }

        // AoWLeaderSetup.pas:437
        public void AvailableAbilitiesCostChange()
        {
            SelectAvailableAbility(AvailableAbilitiesCost.Selected);
        }

        // AoWLeaderSetup.pas:442
        public void AvailableAbilitiesChange()
        {
            SelectAvailableAbility(AvailableAbilities.Selected);
        }

        // AoWLeaderSetup.pas:447
        public void SelectedAbilitiesLBChange()
        {
            SelectedAbilities.ListOffset = SelectedAbilitiesLB.Position;
            SelectedAbilitiesCost.ListOffset = SelectedAbilitiesLB.Position;
        }

        // AoWLeaderSetup.pas:453
        public void AvailableAbilitiesLBChange()
        {
            AvailableAbilities.ListOffset = AvailableAbilitiesLB.Position;
            AvailableAbilitiesCost.ListOffset = AvailableAbilitiesLB.Position;
        }

        // AoWLeaderSetup.pas:459
        public void AttUpClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitAttack((sbyte)(FLeader.FUnitAttack + 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:466
        public void AttDnClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitAttack((sbyte)(FLeader.FUnitAttack - 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:473
        public void DefUpClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitDefense((sbyte)(FLeader.FUnitDefense + 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:480
        public void DefDnClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitDefense((sbyte)(FLeader.FUnitDefense - 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:487
        public void DamUpClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitDamage((sbyte)(FLeader.FUnitDamage + 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:494
        public void DamDnClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitDamage((sbyte)(FLeader.FUnitDamage - 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:501
        public void ResUpClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitResistance((sbyte)(FLeader.FUnitResistance + 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:508
        public void ResDnClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitResistance((sbyte)(FLeader.FUnitResistance - 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:515
        public void HitUpClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitHits((sbyte)(FLeader.FUnitHits + 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:522
        public void HitDnClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitHits((sbyte)(FLeader.FUnitHits - 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:529
        public void MovUpClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitMoves((sbyte)(FLeader.FUnitMoves + 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:536
        public void MovDnClick()
        {
            General.PlayButtonSound(0);
            FLeader.SetUnitMoves((sbyte)(FLeader.FUnitMoves - 1));
            UpdateStatistics();
        }

        // AoWLeaderSetup.pas:543
        public void ClearBtnClick()
        {
            General.PlayButtonSound(0);
            switch (FPhase)
            {
                case 1:
                {
                    var leader = FLeader;
                    leader.SetNickname((AnsiString)"");
                    leader.FHeroName = (AnsiString)"";
                    UpdateLeader();
                }

                    break;
                case 2:
                {
                    var leader2 = FLeader;
                    leader2.AssignAb(leader2.FHeroResource.FAbilities);
                    leader2.SetUnitAttack(0);
                    leader2.SetUnitDefense(0);
                    leader2.SetUnitDamage(0);
                    leader2.SetUnitHits(0);
                    leader2.SetUnitMoves(0);
                    leader2.SetUnitResistance(0);
                    UpdateLeader();
                }

                    break;
                case 3:
                    FLeader.ClearSpheres();
                    UpdateSpheres();
                    break;
            }
        }

        // AoWLeaderSetup.pas:568
        public void DefaultBtnClick(TObject Sender)
        {
            int I = default;
            General.PlayButtonSound(0);
            TitleWindow.TitleWin.FDrawn = false;
            switch (FPhase)
            {
                case 1:
                {
                    var leader = FLeader;
                    var call__95 = FSetup.FLeader.GetNickname();
                    leader.SetNickname(call__95);
                    leader.FHeroName = FSetup.FLeader.FHeroName;
                    leader.SetFaceIndex(FSetup.FLeader.FFaceIndex);
                    UpdateLeader();
                }

                    break;
                case 2:
                {
                    var leader2 = FLeader;
                    leader2.AssignAb(FSetup.FLeader);
                    leader2.SetUnitAttack(FSetup.FLeader.FUnitAttack);
                    leader2.SetUnitDefense(FSetup.FLeader.FUnitDefense);
                    leader2.SetUnitDamage(FSetup.FLeader.FUnitDamage);
                    leader2.SetUnitHits(FSetup.FLeader.FUnitHits);
                    leader2.SetUnitMoves(FSetup.FLeader.FUnitMoves);
                    leader2.SetUnitResistance(FSetup.FLeader.FUnitResistance);
                    UpdateLeader();
                }

                    break;
                case 3:
                {
                    var leader3 = FLeader;
                    leader3.ClearSpheres();
                    {
                        I = 0;
                        int end__98 = FSetup.FLeader.GetSphereCount() - 1;
                        for (; I <= end__98; I += 1)
                        {
                            var call__99 = FSetup.FLeader.GetSpheres(I);
                            leader3.AddSphere(call__99);
                        }
                    }

                    UpdateLeader();
                    UpdateSpheres();
                }

                    break;
            }
        }

        // AoWLeaderSetup.pas:604
        public void SelLifeClick()
        {
            General.PlayButtonSound(0);
            var call__100 = FLeader.CanAddSphere(TMagicSphere.msLife);
            if (call__100)
            {
                FLeader.AddSphere(TMagicSphere.msLife);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:614
        public void SelFireClick()
        {
            General.PlayButtonSound(0);
            var call__101 = FLeader.CanAddSphere(TMagicSphere.msFire);
            if (call__101)
            {
                FLeader.AddSphere(TMagicSphere.msFire);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:624
        public void SelDeathClick()
        {
            General.PlayButtonSound(0);
            var call__102 = FLeader.CanAddSphere(TMagicSphere.msDeath);
            if (call__102)
            {
                FLeader.AddSphere(TMagicSphere.msDeath);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:634
        public void SelWaterClick()
        {
            General.PlayButtonSound(0);
            var call__103 = FLeader.CanAddSphere(TMagicSphere.msWater);
            if (call__103)
            {
                FLeader.AddSphere(TMagicSphere.msWater);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:644
        public void SelAirClick()
        {
            General.PlayButtonSound(0);
            var call__104 = FLeader.CanAddSphere(TMagicSphere.msAir);
            if (call__104)
            {
                FLeader.AddSphere(TMagicSphere.msAir);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:654
        public void SelEarthClick()
        {
            General.PlayButtonSound(0);
            var call__105 = FLeader.CanAddSphere(TMagicSphere.msEarth);
            if (call__105)
            {
                FLeader.AddSphere(TMagicSphere.msEarth);
                UpdateSpheres();
            }
        }

        // AoWLeaderSetup.pas:664
        public void DeleteSphereClick(TObject Sender)
        {
            General.PlayButtonSound(0);
            var call__106 = FLeader.GetSphereCount();
            if (((TComponent)Sender).Tag < call__106)
            {
                FLeader.DeleteSphereIndex(((TComponent)Sender).Tag);
            }

            UpdateSpheres();
        }

        // AoWLeaderSetup.pas:672
        public void RaceCBChange()
        {
            General.PlayButtonSound(0);
            var call__107 = RaceCB.Items.Objects[RaceCB.Selected];
            FLeader.SetRace((sbyte)ObjectToInt(call__107));
        }

        // AoWLeaderSetup.pas:678
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

            SortSelectedAbilities();
        }

        // AoWLeaderSetup.pas:686
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

            SortSelectedAbilities();
        }

        // AoWLeaderSetup.pas:694
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

            SortAvailableAbilities();
        }

        // AoWLeaderSetup.pas:702
        public void AvailableAbilitiesCostSortClick()
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

            SortAvailableAbilities();
        }

        // AoWLeaderSetup.pas:710
        public void SBaseClick()
        {
            General.PlayButtonSound(0);
        }

        // AoWLeaderSetup.pas:715
        public void SExtraClick()
        {
            General.PlayButtonSound(0);
        }

        // AoWLeaderSetup.pas:720
        public void SFinalClick()
        {
            General.PlayButtonSound(0);
        }

        // AoWLeaderSetup.pas:725
        public void SelectedAbilitiesCostMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility Ability = default;
            if (Button == TMouseButton.mbRight && SelectedAbilities.Selected != -1)
            {
                var call__108 = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
                var call__109 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__108));
                Ability = call__109;
                if (Ability != null)
                {
                    var call__110 = Ability.GetName(null);
                    SetInfoBanner(AoWInfoBanner.AoWInfoBanner.InfoBanner, call__110, Ability.FDescription);
                }
            }
        }

        // AoWLeaderSetup.pas:737
        public void AvailableAbilitiesMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility Ability = default;
            if (Button == TMouseButton.mbRight && AvailableAbilities.Selected != -1)
            {
                var call__111 = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
                var call__112 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__111));
                Ability = call__112;
                if (Ability != null)
                {
                    var call__113 = Ability.GetName(null);
                    SetInfoBanner(AoWInfoBanner.AoWInfoBanner.InfoBanner, call__113, Ability.FDescription);
                }
            }
        }
    }

    public static partial class AoWLeaderSetup
    {
        public static TLeaderSetupWin LeaderSetupWin;
    }
}
