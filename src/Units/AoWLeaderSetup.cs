// Translated from AoWLeaderSetup.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComboBox;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWInfoBanner;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMessageDlg;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.aowSetupEvents.aowSetupEventsUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E2Globals.E2GlobalsUnit;
using static Aow.Units.Engine.EngineUnit;

namespace Aow.Units.AoWLeaderSetup;

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
        Reset();
        if (FSetup != null)
        {
            FSetup.Release();
        }

        FSetup = Control;
        FSetup.AddRef();
        FSelectedSort = 0;
        FAvailableSort = 0;
        LeaderPnl.ILTexture = aowIntGfxModUnit.IntGfxMod.TitleScreen;
        LeaderPnl.ILIndexTexture = 4;
        RaceCB.Visible = FSetup.FItems.Count != 0 && FSetup.FMode.Contains(TLeaderSetupModeElement.lsmCampaign);
        RaceLbl.Visible = RaceCB.Visible;
        if (RaceCB.Visible)
        {
            RaceCB.Items.Clear();
            for (int I = 0, last = FSetup.FItems.Count - 1; I <= last; I++)
            {
                var item = FSetup.FItems[I];
                var raceNames = TranslateRStr(RaceNamesRStr[item]);
                var item2 = FSetup.FItems[I];
                RaceCB.Items.AddObject(raceNames, IntToObject(item2));
            }

            var indexOf = FSetup.FItems.IndexOf(FSetup.FOriginalLeader.FRace);
            RaceCB.Selected = indexOf;
        }

        var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[FSetup.FOriginalLeader.FRace]);
        SideNameLbl.Text = raceNamesPlural;
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
        TLeaderSetupControl Control;
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
        TLeaderSetupControl Control;
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
        TLeader Leader;
        FSetup.FOriginalLeader.Assign(FLeader, AoWEngine);
        Leader = FSetup.FOriginalLeader;
        LeaderName.Text = Leader.FHeroName;
        var inherentAttack = Leader.GetInherentAttack();
        UAttack.Text = IntToStr(inherentAttack);
        var inherentDefense = Leader.GetInherentDefense();
        UDefense.Text = IntToStr(inherentDefense);
        var inherentDamage = Leader.GetInherentDamage();
        UDamage.Text = IntToStr(inherentDamage);
        var inherentResistance = Leader.GetInherentResistance();
        UResistance.Text = IntToStr(inherentResistance);
        var hits = Leader.GetHits();
        UHits.Text = IntToStr(hits);
        var moves = Leader.GetMoves();
        UMoves.Text = IntToStr(moves);
        UAbilities.Items.Clear();
        Leader.ListAbilities(UAbilities.Items);
        var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[Leader.FRace]);
        SideNameLbl.Text = raceNamesPlural;
        ShieldL.CurrentImage = Leader.FRace + 13;
        ShieldR.CurrentImage = Leader.FRace + 13;
    }

    // AoWLeaderSetup.pas:775
    public void UpdateSkillPoints()
    {
        var skillPoints = FLeader.GetSkillPoints();
        var skillPointsText = TranslateRStr(SkillPointsRStr);
        var skillPointsMax = FLeader.GetSkillPointsMax();
        SkillPointsLbl.Text = skillPointsText + " " + IntToStr(skillPoints) + "/" + IntToStr(skillPointsMax);
    }

    // AoWLeaderSetup.pas:781
    public void UpdateStatistics()
    {
        THeroResource Res;
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
        var inherentAttack = FLeader.GetInherentAttack();
        AttValue.Text = IntToStr(inherentAttack);
        var inherentDefense = FLeader.GetInherentDefense();
        DefValue.Text = IntToStr(inherentDefense);
        var inherentDamage = FLeader.GetInherentDamage();
        DamValue.Text = IntToStr(inherentDamage);
        var inherentResistance = FLeader.GetInherentResistance();
        ResValue.Text = IntToStr(inherentResistance);
        var hits = FLeader.GetHits();
        HitValue.Text = IntToStr(hits);
        var moves = FLeader.GetMoves();
        MovValue.Text = IntToStr(moves);
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
        string Name;
        string Cost;
        TAbility Ability;
        AvailableAbilities.Items.Clear();
        AvailableAbilitiesCost.Items.Clear();
        for (int I = 0, last = AoWHSSet.FAbilityControl.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSSet.FAbilityControl.GetAbility(I) != null && (AoWHSSet.FAbilityControl.GetAbility(I).FSelectionTypes & TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astCustomizeLeader, TAbilitySelectionType.astHeroUpgrade)) != TSet<TAbilitySelectionType>.Of())
            {
                if (AoWHSSet.FAbilityControl.GetAbility(I).CanExpand(FLeader))
                {
                    var expandName = AoWHSSet.FAbilityControl.GetAbility(I).ExpandName(FLeader);
                    AvailableAbilities.Items.AddObject(expandName, IntToObject(I));
                    var expandCost = AoWHSSet.FAbilityControl.GetAbility(I).ExpandCost(FLeader);
                    AvailableAbilitiesCost.Items.Add(IntToStr(expandCost));
                }
            }
        }

        SelectedAbilities.Items.Clear();
        SelectedAbilitiesCost.Items.Clear();
        for (int I = 0, last = FLeader.GetAbCount() - 1; I <= last; I++)
        {
            Ability = AoWHSSet.FAbilityControl.GetAbility(I);
            if (FLeader.GetAbSet(I) && (FLeader.FHeroResource.FAbilities.GetAbSet(I) || (Ability.FSelectionTypes & TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astCustomizeLeader, TAbilitySelectionType.astHeroUpgrade)) != TSet<TAbilitySelectionType>.Of()))
            {
                Name = AoWHSSet.FAbilityControl.GetAbility(I).GetName(FLeader);
                if (FLeader.FHeroResource.FAbilities.GetAbSet(I) && FLeader.FHeroResource.FAbilities.GetAbLevel(I) == FLeader.GetAbilityLevel(I))
                {
                    Name = Name + " (" + TranslateRStr(FixedRStr) + ")";
                    Cost = "-";
                }
                else
                {
                    Cost = IntToStr(Ability.GetSkillPoints(FLeader) - Ability.GetSkillPoints(FLeader.FHeroResource.FAbilities));
                }

                SelectedAbilitiesCost.Items.Add(Cost);
                SelectedAbilities.Items.AddObject(Name, IntToObject(I));
            }
        }

        var count = AvailableAbilities.Items.Count;
        AvailableAbilitiesLB.Max = count - 1;
        var itemsCount = SelectedAbilities.Items.Count;
        SelectedAbilitiesLB.Max = itemsCount - 1;
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
    public bool CanAddAbility(int AbilityID) => AoWHSSet.FAbilityControl.GetAbility(AbilityID).CanExpand(FLeader) && AoWHSSet.FAbilityControl.GetAbility(AbilityID).ExpandCost(FLeader) <= FLeader.GetSkillPoints();

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
            var finish = TranslateRStr(FinishRStr);
            NextBtn.Text = finish;
        }
        else
        {
            var next = TranslateRStr(NextRStr);
            NextBtn.Text = next;
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
        var step1CustomizeLeader = TranslateRStr(Step1CustomizeLeaderRStr);
        Phase1Header.Text = step1CustomizeLeader;
        var step2AssignSkillPoints = TranslateRStr(Step2AssignSkillPointsRStr);
        Phase2Header.Text = step2AssignSkillPoints;
        var step3AssignSpheres = TranslateRStr(Step3AssignSpheresRStr);
        Phase3Header.Text = step3AssignSpheres;
        SetFixedSpheres(0);
    }

    // AoWLeaderSetup.pas:943
    public void UpdateSpheres()
    {
        TAOWButton Btn;
        for (int I = 0; I <= 6; I++)
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

            if (I < FLeader.GetSphereCount())
            {
                Btn.Visible = true;
                switch ((int)FLeader.GetSpheres(I) & 127)
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

        var sphereCount = FLeader.GetSphereCount();
        var selectedSpheres = TranslateRStr(SelectedSpheresRStr);
        SelectedSpheresLbl.Text = selectedSpheres + " (" + IntToStr(sphereCount) + "/" + IntToStr(FLeader.FSpherePicksMax) + ")";
        if (FLeader.CanAddSphere(TMagicSphere.msLife))
        {
            SelLife.ILIndexUp = 27;
            SelLife.FCaptureInput = true;
        }
        else
        {
            SelLife.ILIndexUp = 28;
            SelLife.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(TMagicSphere.msDeath))
        {
            SelDeath.ILIndexUp = 30;
            SelDeath.FCaptureInput = true;
        }
        else
        {
            SelDeath.ILIndexUp = 31;
            SelDeath.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(TMagicSphere.msFire))
        {
            SelFire.ILIndexUp = 42;
            SelFire.FCaptureInput = true;
        }
        else
        {
            SelFire.ILIndexUp = 43;
            SelFire.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(TMagicSphere.msWater))
        {
            SelWater.ILIndexUp = 39;
            SelWater.FCaptureInput = true;
        }
        else
        {
            SelWater.ILIndexUp = 40;
            SelWater.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(TMagicSphere.msEarth))
        {
            SelEarth.ILIndexUp = 33;
            SelEarth.FCaptureInput = true;
        }
        else
        {
            SelEarth.ILIndexUp = 34;
            SelEarth.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(TMagicSphere.msAir))
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
        FLeader = new TLeader(null);
    }

    // AoWLeaderSetup.pas:274
    public void LeaderSetupWinDestroy()
    {
        FLeader.Free();
    }

    // AoWLeaderSetup.pas:279
    public void AddAbilityClick()
    {
        int AbilityID;
        General.PlayButtonSound(0);
        if (AvailableAbilities.Selected != -1)
        {
            AbilityID = ObjectToInt(AvailableAbilities.Items.Objects[AvailableAbilities.Selected]);
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
        int AbilityID;
        General.PlayButtonSound(0);
        if (SelectedAbilities.Selected != -1)
        {
            AbilityID = ObjectToInt(SelectedAbilities.Items.Objects[SelectedAbilities.Selected]);
            if (FLeader.FHeroResource.FAbilities.GetAbSet(AbilityID) && FLeader.FHeroResource.FAbilities.GetAbLevel(AbilityID) == FLeader.GetAbilityLevel(AbilityID))
            {
                return;
            }

            FLeader.RemoveAbility(AbilityID);
            UpdateAbilities();
            UpdateLeader();
        }
    }

    // AoWLeaderSetup.pas:312
    public void DrawSelectionFace(TAoWFrame Sender, TSurfaceDesc SD, int Offset)
    {
        TLeaderSetupControl Setup;
        int Index;
        TRect R;
        TFaceResource Face;
        Setup = FSetup;
        Index = FLeader.FFaceIndex;
        while (Offset < 0)
        {
            Offset += 1;
            Index = AoWHSSet.FFaceResources.PrevFace(Index);
        }

        while (Offset > 0)
        {
            Offset -= 1;
            Index = AoWHSSet.FFaceResources.NextFace(Index);
        }

        var image = Sender;
        R = Rect(Phase1Pnl.WinLeft + image.WinLeft + 2, Phase1Pnl.WinTop + image.WinTop, Phase1Pnl.WinLeft + image.WinLeft + image.WinWidth - 2, Phase1Pnl.WinTop + image.WinTop + image.WinHeight - 2);

        if (Setup.FOriginalLeader.GetFace() != null)
        {
            Face = AoWHSSet.FFaceResources.GetFaceResource(Index);
            Face.AddRef();
            Face.GetFaceImage().ShowStretched(R, SD);
            Face.Release();
        }
    }

    // AoWLeaderSetup.pas:345
    public void FacePrevBtnClick()
    {
        General.PlayButtonSound(0);
        var prevFace = AoWHSSet.FFaceResources.PrevFace(FLeader.FFaceIndex);
        FLeader.SetFaceIndex(prevFace);
        TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWLeaderSetup.pas:352
    public void FaceNextBtnClick()
    {
        General.PlayButtonSound(0);
        var nextFace = AoWHSSet.FFaceResources.NextFace(FLeader.FFaceIndex);
        FLeader.SetFaceIndex(nextFace);
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
                if (FLeader.GetSphereCount() == FLeader.FSpherePicksMax)
                {
                    Finish();
                }
                else
                {
                    ResetMessageDialog(AoWMessageDlgUnit.MessageDlg);
                    AoWMessageDlgUnit.MessageDlg.MessageMemo.Items.Add("");
                    var pleaseAssignAllSpheres = TranslateRStr(PleaseAssignAllSpheresRStr);
                    AoWMessageDlgUnit.MessageDlg.MessageMemo.Items.Add(pleaseAssignAllSpheres);
                    AoWMessageDlgUnit.MessageDlg.OKPnl.Visible = true;
                    AoWMessageDlgUnit.MessageDlg.Dlg.Visible = true;
                }

                break;
        }
    }

    // AoWLeaderSetup.pas:405
    public void MainFaceFrPreDraw(TAoWFrame Sender, TSurfaceDesc SD)
    {
        TLeaderSetupControl Setup;
        TRect R;
        Setup = FSetup;
        if (FPhase > 1)
        {
            var frame = Sender;
            R = Rect(LMainPnl.WinLeft + frame.WinLeft + 2, LMainPnl.WinTop + frame.WinTop, LMainPnl.WinLeft + frame.WinLeft + frame.WinWidth - 2, LMainPnl.WinTop + frame.WinTop + frame.WinHeight - 2);

            if (Setup.FOriginalLeader.GetFace() != null)
            {
                Setup.FOriginalLeader.GetFace().ShowStretched(R, SD);
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
                    leader.SetNickname("");
                    leader.FHeroName = "";
                    UpdateLeader();
                }

                break;
            case 2:
                {
                    var leader = FLeader;
                    leader.AssignAb(leader.FHeroResource.FAbilities);
                    leader.SetUnitAttack(0);
                    leader.SetUnitDefense(0);
                    leader.SetUnitDamage(0);
                    leader.SetUnitHits(0);
                    leader.SetUnitMoves(0);
                    leader.SetUnitResistance(0);
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
    public void DefaultBtnClick()
    {
        General.PlayButtonSound(0);
        TitleWindow.TitleWin.FDrawn = false;
        switch (FPhase)
        {
            case 1:
                {
                    var leader = FLeader;
                    leader.SetNickname(FSetup.FLeader.GetNickname());
                    leader.FHeroName = FSetup.FLeader.FHeroName;
                    leader.SetFaceIndex(FSetup.FLeader.FFaceIndex);
                    UpdateLeader();
                }

                break;
            case 2:
                {
                    var leader = FLeader;
                    leader.AssignAb(FSetup.FLeader);
                    leader.SetUnitAttack(FSetup.FLeader.FUnitAttack);
                    leader.SetUnitDefense(FSetup.FLeader.FUnitDefense);
                    leader.SetUnitDamage(FSetup.FLeader.FUnitDamage);
                    leader.SetUnitHits(FSetup.FLeader.FUnitHits);
                    leader.SetUnitMoves(FSetup.FLeader.FUnitMoves);
                    leader.SetUnitResistance(FSetup.FLeader.FUnitResistance);
                    UpdateLeader();
                }

                break;
            case 3:
                {
                    var leader = FLeader;
                    leader.ClearSpheres();
                    for (int I = 0, last = FSetup.FLeader.GetSphereCount() - 1; I <= last; I++)
                    {
                        leader.AddSphere(FSetup.FLeader.GetSpheres(I));
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
        if (FLeader.CanAddSphere(TMagicSphere.msLife))
        {
            FLeader.AddSphere(TMagicSphere.msLife);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:614
    public void SelFireClick()
    {
        General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(TMagicSphere.msFire))
        {
            FLeader.AddSphere(TMagicSphere.msFire);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:624
    public void SelDeathClick()
    {
        General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(TMagicSphere.msDeath))
        {
            FLeader.AddSphere(TMagicSphere.msDeath);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:634
    public void SelWaterClick()
    {
        General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(TMagicSphere.msWater))
        {
            FLeader.AddSphere(TMagicSphere.msWater);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:644
    public void SelAirClick()
    {
        General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(TMagicSphere.msAir))
        {
            FLeader.AddSphere(TMagicSphere.msAir);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:654
    public void SelEarthClick()
    {
        General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(TMagicSphere.msEarth))
        {
            FLeader.AddSphere(TMagicSphere.msEarth);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:664
    public void DeleteSphereClick(int Slot)
    {
        General.PlayButtonSound(0);
        var sphereCount = FLeader.GetSphereCount();
        if (Slot < sphereCount)
        {
            FLeader.DeleteSphereIndex(Slot);
        }

        UpdateSpheres();
    }

    // AoWLeaderSetup.pas:672
    public void RaceCBChange()
    {
        General.PlayButtonSound(0);
        var objectValue = RaceCB.Items.Objects[RaceCB.Selected];
        FLeader.SetRace((sbyte)ObjectToInt(objectValue));
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
    public void SelectedAbilitiesCostMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility Ability;
        if (Button == TMouseButton.mbRight && SelectedAbilities.Selected != -1)
        {
            var objectValue = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
            Ability = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (Ability != null)
            {
                var name = Ability.GetName(null);
                SetInfoBanner(AoWInfoBannerUnit.InfoBanner, name, Ability.FDescription);
            }
        }
    }

    // AoWLeaderSetup.pas:737
    public void AvailableAbilitiesMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility Ability;
        if (Button == TMouseButton.mbRight && AvailableAbilities.Selected != -1)
        {
            var objectValue = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
            Ability = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (Ability != null)
            {
                var name = Ability.GetName(null);
                SetInfoBanner(AoWInfoBannerUnit.InfoBanner, name, Ability.FDescription);
            }
        }
    }
}

public static partial class AoWLeaderSetupUnit
{
    public static TLeaderSetupWin LeaderSetupWin;
}
