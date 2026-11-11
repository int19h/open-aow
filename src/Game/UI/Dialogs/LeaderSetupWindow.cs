// Translated from AoWLeaderSetup.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// setup control, phase, fixed spheres, working leader, selected sort, available sort
public partial class LeaderSetupWindow : DataModule
{
    public LeaderSetupWindow(Component owner) : base(owner)
    { }

    public ListBox SelectedAbilities;
    public Panel Phase2Pnl;
    public Panel LMainPnl;
    public Button NextBtn;
    public Button DefaultBtn;
    public Button ClearBtn;
    public Button PrevBtn;
    public Panel EditStatisticsPnl;
    public ListBox AvailableAbilities;
    public Label AttValue;
    public Label AttLbl;
    public Panel AttPnl;
    public Label DamValue;
    public Label DamLbl;
    public Panel DamPnl;
    public Label DefValue;
    public Label DefLbl;
    public Panel DefPnl;
    public Label ResValue;
    public Label ResLbl;
    public Panel ResPnl;
    public Label HitValue;
    public Label HitLbl;
    public Panel HitPnl;
    public Label MovValue;
    public Label MovLbl;
    public Panel MovPnl;
    public Label AttI;
    public Label DamI;
    public Label DefI;
    public Label ResI;
    public Label HitI;
    public Label MovI;
    public Label AttA;
    public Label DamA;
    public Label DefA;
    public Label ResA;
    public Label HitA;
    public Label MovA;
    public Label AttP;
    public Label DamP;
    public Label DefP;
    public Label ResP;
    public Label HitP;
    public Label MovP;
    public Label AttB;
    public Label DamB;
    public Label DefB;
    public Label ResB;
    public Label HitB;
    public Label MovB;
    public Label SkillPointsLbl;
    public Image UAttIcn;
    public Image UDefIcn;
    public Image UMovIcn;
    public Image UDamIcn;
    public Image UResIcn;
    public Image UHitIcn;
    public Label UAttack;
    public Label UDefense;
    public Label UMoves;
    public Label UDamage;
    public Label UResistance;
    public Label UHits;
    public Panel StatPnl;
    public ListBox UAbilities;
    public Panel AbilityPnl;
    public Panel UStatP1;
    public Panel UStatP2;
    public Label LeaderName;
    public Image ShieldL;
    public Image ShieldR;
    public Panel ButtonPnl;
    public Panel LeftPnl;
    public Panel RightPnl;
    public Label LeaderStatsLbl;
    public Label LeaderAbilLbl;
    public Label FullAbilLbl;
    public Button RemoveAbility;
    public Button AddAbility;
    public Image MainFace;
    public Panel Phase1Pnl;
    public Image Face4;
    public Image Face5;
    public Image Face6;
    public Image Face7;
    public Image Face3;
    public Image Face2;
    public Image Face1;
    public Edit NameEdit;
    public Label NameLbl;
    public Label FaceLbl;
    public Label RaceLbl;
    public Label Phase1Header;
    public VerticalScrollBar UAbilSB;
    public VerticalScrollBar AvailableAbilitiesLB;
    public VerticalScrollBar SelectedAbilitiesLB;
    public Panel Phase3Pnl;
    public Label SpheresLbl;
    public Button SelDeath;
    public Panel SpherePnl;
    public Button SelLife;
    public Button SelFire;
    public Button SelEarth;
    public Button SelAir;
    public Button SelWater;
    public Label LifeLbl;
    public Button Sel4;
    public Panel SphereSelectionPnl;
    public Button Sel3;
    public Button Sel2;
    public Button Sel1;
    public Button Sel5;
    public Button Sel6;
    public Button Sel7;
    public Label DeathLbl;
    public Label EarthLbl;
    public Label WaterLbl;
    public Label FireLbl;
    public Label AirLbl;
    public Label SelectedSpheresLbl;
    public Button FaceNextBtn;
    public Button FacePrevBtn;
    public Label LeaderLbl;
    public Label SideNameLbl;
    public ListBox SelectedAbilitiesCost;
    public ListBox AvailableAbilitiesCost;
    public Button AttUp;
    public Button AttDn;
    public Button DamUp;
    public Button DamDn;
    public Button DefUp;
    public Button DefDn;
    public Button ResUp;
    public Button ResDn;
    public Button HitUp;
    public Button HitDn;
    public Button MovUp;
    public Button MovDn;
    public Image AttIc;
    public Image DamIcn;
    public Image DefIc;
    public Image ResIc;
    public Image HitIc;
    public Image MovIc;
    public Button SBase;
    public Button SExtra;
    public Button SFinal;
    public Button SelectedAbilitiesSort;
    public Button SelectedAbilitiesCostSort;
    public Button AvailableAbilitiesSort;
    public Button AvailableAbilitiesCostSort;
    public Frame MainFaceFr;
    public Frame FFr1;
    public Frame FFr7;
    public Frame FFr2;
    public Frame FFr6;
    public Frame FFr3;
    public Frame FFr5;
    public Frame FFr4;
    public Label Phase2Header;
    public Label Phase3Header;
    public Panel LeaderPnl;
    public ComboBox RaceCB;
    public IvTranslator Translator;
    public LeaderSetupControl FSetup;
    public int FPhase;
    public int FFixedSpheres;
    public Leader FLeader;
    public int FSelectedSort;
    public int FAvailableSort;

    // AoWLeaderSetup.pas:1050
    public void Execute(LeaderSetupControl Control)
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
        LeaderPnl.ILTexture = DialogHosts.IntGfxMod.TitleScreen;
        LeaderPnl.ILIndexTexture = 4;
        RaceCB.Visible = FSetup.FItems.Count != 0 && FSetup.FMode.Contains(LeaderSetupModeElement.Campaign);
        RaceLbl.Visible = RaceCB.Visible;
        if (RaceCB.Visible)
        {
            RaceCB.Items.Clear();
            for (int I = 0, last = FSetup.FItems.Count - 1; I <= last; I++)
            {
                var item = FSetup.FItems[I];
                var raceNames = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[item]);
                var item2 = FSetup.FItems[I];
                RaceCB.Items.AddObject(raceNames, IntToObject(item2));
            }

            var indexOf = FSetup.FItems.IndexOf(FSetup.FOriginalLeader.FRace);
            RaceCB.Selected = indexOf;
        }

        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[FSetup.FOriginalLeader.FRace]);
        SideNameLbl.Text = raceNamesPlural;
        ShieldL.CurrentImage = FSetup.FOriginalLeader.FRace + 13;
        ShieldR.CurrentImage = FSetup.FOriginalLeader.FRace + 13;
        ShieldR.Visible = !FSetup.FMode.Contains(LeaderSetupModeElement.Campaign);
        LoadLeader();
        SaveLeader();
        if (FSetup.FMode.Contains(LeaderSetupModeElement.Look))
        {
            SetPhase(1);
        }
        else
        {
            if (FSetup.FMode.Contains(LeaderSetupModeElement.Settings))
            {
                SetPhase(2);
            }
            else
            {
                if (FSetup.FMode.Contains(LeaderSetupModeElement.Spheres))
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
        LeaderSetupControl Control;
        SaveLeader();
        Control = FSetup;
        Control.FStatus = LeaderSetupControl.LeaderSetupStatus.Done;
        FSetup = null;
        LeaderPnl.Visible = false;
        DialogHosts.SetupEvents.UpdatePlayerName(Control.FOriginalLeader.FHeroName);
        Control.Done();
        Control.Release();
    }

    // AoWLeaderSetup.pas:1097
    public void Cancel()
    {
        LeaderSetupControl Control;
        SaveLeader();
        Control = FSetup;
        Control.FStatus = LeaderSetupControl.LeaderSetupStatus.Canceled;
        FSetup = null;
        LeaderPnl.Visible = false;
        Control.Done();
        Control.Release();
    }

    // AoWLeaderSetup.pas:750
    public void LoadLeader()
    {
        FLeader.Assign(FSetup.FOriginalLeader, GameTables.AoWEngine);
    }

    // AoWLeaderSetup.pas:755
    public void SaveLeader()
    {
        Leader Leader;
        FSetup.FOriginalLeader.Assign(FLeader, GameTables.AoWEngine);
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
        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[Leader.FRace]);
        SideNameLbl.Text = raceNamesPlural;
        ShieldL.CurrentImage = Leader.FRace + 13;
        ShieldR.CurrentImage = Leader.FRace + 13;
    }

    // AoWLeaderSetup.pas:775
    public void UpdateSkillPoints()
    {
        var skillPoints = FLeader.GetSkillPoints();
        var skillPointsText = AbilityRules.TranslateRStr(SkillPointsRStr);
        var skillPointsMax = FLeader.GetSkillPointsMax();
        SkillPointsLbl.Text = skillPointsText + " " + IntToStr(skillPoints) + "/" + IntToStr(skillPointsMax);
    }

    // AoWLeaderSetup.pas:781
    public void UpdateStatistics()
    {
        HeroResource Res;
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
                StringSorting.SortStringsAscending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                break;
            case 2:
                StringSorting.SortStringsNumericalAscending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                break;
            case 3:
                StringSorting.SortStringsNumericalDescending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                break;
        }
    }

    // AoWLeaderSetup.pas:830
    public void SortAvailableAbilities()
    {
        switch (FAvailableSort)
        {
            case 0:
                StringSorting.SortStringsAscending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                break;
            case 2:
                StringSorting.SortStringsNumericalAscending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                break;
            case 3:
                StringSorting.SortStringsNumericalDescending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                break;
        }
    }

    // AoWLeaderSetup.pas:852
    public void UpdateAbilities()
    {
        string Name;
        string Cost;
        Ability Ability;
        AvailableAbilities.Items.Clear();
        AvailableAbilitiesCost.Items.Clear();
        for (int I = 0, last = AoWHSSet.Current.FAbilityControl.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSSet.Current.FAbilityControl.GetAbility(I) != null && (AoWHSSet.Current.FAbilityControl.GetAbility(I).FSelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of())
            {
                if (AoWHSSet.Current.FAbilityControl.GetAbility(I).CanExpand(FLeader))
                {
                    var expandName = AoWHSSet.Current.FAbilityControl.GetAbility(I).ExpandName(FLeader);
                    AvailableAbilities.Items.AddObject(expandName, IntToObject(I));
                    var expandCost = AoWHSSet.Current.FAbilityControl.GetAbility(I).ExpandCost(FLeader);
                    AvailableAbilitiesCost.Items.Add(IntToStr(expandCost));
                }
            }
        }

        SelectedAbilities.Items.Clear();
        SelectedAbilitiesCost.Items.Clear();
        for (int I = 0, last = FLeader.GetAbCount() - 1; I <= last; I++)
        {
            Ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
            if (FLeader.GetAbSet(I) && (FLeader.FHeroResource.FAbilities.GetAbSet(I) || (Ability.FSelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of()))
            {
                Name = AoWHSSet.Current.FAbilityControl.GetAbility(I).GetName(FLeader);
                if (FLeader.FHeroResource.FAbilities.GetAbSet(I) && FLeader.FHeroResource.FAbilities.GetAbLevel(I) == FLeader.GetAbilityLevel(I))
                {
                    Name = Name + " (" + AbilityRules.TranslateRStr(GameText.FixedRStr) + ")";
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
    public bool CanAddAbility(int AbilityID) => AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).CanExpand(FLeader) && AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).ExpandCost(FLeader) <= FLeader.GetSkillPoints();

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
        LeaderLbl.Visible = !FSetup.FMode.Contains(LeaderSetupModeElement.Campaign);
        SideNameLbl.Visible = !FSetup.FMode.Contains(LeaderSetupModeElement.Campaign);
        ShieldL.Visible = !FSetup.FMode.Contains(LeaderSetupModeElement.Campaign) || Value > 1;
        ShieldR.Visible = !FSetup.FMode.Contains(LeaderSetupModeElement.Campaign) || Value > 1;
        MainFaceFr.Visible = !FSetup.FMode.Contains(LeaderSetupModeElement.Campaign) || Value > 1;
        MainFace.Visible = MainFaceFr.Visible;
        if (Value == 3 || (Value == 2 && (FSetup.FMode & Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Spheres)) == Set<LeaderSetupModeElement>.Of()) || (Value == 1 && (FSetup.FMode & Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Settings, LeaderSetupModeElement.Spheres)) == Set<LeaderSetupModeElement>.Of()))
        {
            var finish = AbilityRules.TranslateRStr(FinishRStr);
            NextBtn.Text = finish;
        }
        else
        {
            var next = AbilityRules.TranslateRStr(NextRStr);
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
        var step1CustomizeLeader = AbilityRules.TranslateRStr(Step1CustomizeLeaderRStr);
        Phase1Header.Text = step1CustomizeLeader;
        var step2AssignSkillPoints = AbilityRules.TranslateRStr(Step2AssignSkillPointsRStr);
        Phase2Header.Text = step2AssignSkillPoints;
        var step3AssignSpheres = AbilityRules.TranslateRStr(Step3AssignSpheresRStr);
        Phase3Header.Text = step3AssignSpheres;
        SetFixedSpheres(0);
    }

    // AoWLeaderSetup.pas:943
    public void UpdateSpheres()
    {
        Button Btn;
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
        var selectedSpheres = AbilityRules.TranslateRStr(SelectedSpheresRStr);
        SelectedSpheresLbl.Text = selectedSpheres + " (" + IntToStr(sphereCount) + "/" + IntToStr(FLeader.FSpherePicksMax) + ")";
        if (FLeader.CanAddSphere(MagicSphere.Life))
        {
            SelLife.ILIndexUp = 27;
            SelLife.FCaptureInput = true;
        }
        else
        {
            SelLife.ILIndexUp = 28;
            SelLife.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(MagicSphere.Death))
        {
            SelDeath.ILIndexUp = 30;
            SelDeath.FCaptureInput = true;
        }
        else
        {
            SelDeath.ILIndexUp = 31;
            SelDeath.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(MagicSphere.Fire))
        {
            SelFire.ILIndexUp = 42;
            SelFire.FCaptureInput = true;
        }
        else
        {
            SelFire.ILIndexUp = 43;
            SelFire.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(MagicSphere.Water))
        {
            SelWater.ILIndexUp = 39;
            SelWater.FCaptureInput = true;
        }
        else
        {
            SelWater.ILIndexUp = 40;
            SelWater.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(MagicSphere.Earth))
        {
            SelEarth.ILIndexUp = 33;
            SelEarth.FCaptureInput = true;
        }
        else
        {
            SelEarth.ILIndexUp = 34;
            SelEarth.FCaptureInput = false;
        }

        if (FLeader.CanAddSphere(MagicSphere.Air))
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
        FLeader = new Leader(null);
    }

    // AoWLeaderSetup.pas:274
    public void LeaderSetupWinDestroy()
    {
        FLeader?.Dispose();
    }

    // AoWLeaderSetup.pas:279
    public void AddAbilityClick()
    {
        int AbilityID;
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
    public void DrawSelectionFace(Frame Sender, LockedSurface SD, int Offset)
    {
        LeaderSetupControl Setup;
        int Index;
        Rect R;
        FaceResource Face;
        Setup = FSetup;
        Index = FLeader.FFaceIndex;
        while (Offset < 0)
        {
            Offset += 1;
            Index = AoWHSSet.Current.FFaceResources.PrevFace(Index);
        }

        while (Offset > 0)
        {
            Offset -= 1;
            Index = AoWHSSet.Current.FFaceResources.NextFace(Index);
        }

        var image = Sender;
        R = new Rect(Phase1Pnl.WinLeft + image.WinLeft + 2, Phase1Pnl.WinTop + image.WinTop, Phase1Pnl.WinLeft + image.WinLeft + image.WinWidth - 2, Phase1Pnl.WinTop + image.WinTop + image.WinHeight - 2);

        if (Setup.FOriginalLeader.GetFace() != null)
        {
            Face = AoWHSSet.Current.FFaceResources.GetFaceResource(Index);
            Face.AddRef();
            Face.GetFaceImage().ShowStretched(R, SD);
            Face.Release();
        }
    }

    // AoWLeaderSetup.pas:345
    public void FacePrevBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var prevFace = AoWHSSet.Current.FFaceResources.PrevFace(FLeader.FFaceIndex);
        FLeader.SetFaceIndex(prevFace);
        DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWLeaderSetup.pas:352
    public void FaceNextBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var nextFace = AoWHSSet.Current.FFaceResources.NextFace(FLeader.FFaceIndex);
        FLeader.SetFaceIndex(nextFace);
        DialogHosts.TitleWindow.TitleWin.FDrawn = false;
    }

    // AoWLeaderSetup.pas:359
    public void PrevBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        switch (FPhase)
        {
            case 1:
                if (FSetup.FMode.Contains(LeaderSetupModeElement.Campaign))
                {
                    Cancel();
                }

                break;
            case 2:
                SaveLeader();
                if (FSetup.FMode.Contains(LeaderSetupModeElement.Look))
                {
                    SetPhase(1);
                }

                break;
            case 3:
                SaveLeader();
                if (FSetup.FMode.Contains(LeaderSetupModeElement.Settings))
                {
                    SetPhase(2);
                }
                else
                {
                    if (FSetup.FMode.Contains(LeaderSetupModeElement.Look))
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
        DialogHosts.General.PlayButtonSound(0);
        switch (FPhase)
        {
            case 1:
                SaveLeader();
                if (FSetup.FMode.Contains(LeaderSetupModeElement.Settings))
                {
                    SetPhase(2);
                }
                else
                {
                    if (FSetup.FMode.Contains(LeaderSetupModeElement.Spheres))
                    {
                        SetPhase(3);
                    }
                    else
                    {
                        Finish();
                    }
                }

                DialogHosts.TitleWindow.TitleWin.FDrawn = false;
                break;
            case 2:
                SaveLeader();
                if (FSetup.FMode.Contains(LeaderSetupModeElement.Spheres))
                {
                    SetPhase(3);
                }
                else
                {
                    Finish();
                }

                DialogHosts.TitleWindow.TitleWin.FDrawn = false;
                break;
            case 3:
                if (FLeader.GetSphereCount() == FLeader.FSpherePicksMax)
                {
                    Finish();
                }
                else
                {
                    MessageDialog.ResetMessageDialog(DialogHosts.MessageDlg);
                    DialogHosts.MessageDlg.MessageMemo.Items.Add("");
                    var pleaseAssignAllSpheres = AbilityRules.TranslateRStr(PleaseAssignAllSpheresRStr);
                    DialogHosts.MessageDlg.MessageMemo.Items.Add(pleaseAssignAllSpheres);
                    DialogHosts.MessageDlg.OKPnl.Visible = true;
                    DialogHosts.MessageDlg.Dlg.Visible = true;
                }

                break;
        }
    }

    // AoWLeaderSetup.pas:405
    public void MainFaceFrPreDraw(Frame Sender, LockedSurface SD)
    {
        LeaderSetupControl Setup;
        Rect R;
        Setup = FSetup;
        if (FPhase > 1)
        {
            var frame = Sender;
            R = new Rect(LMainPnl.WinLeft + frame.WinLeft + 2, LMainPnl.WinTop + frame.WinTop, LMainPnl.WinLeft + frame.WinLeft + frame.WinWidth - 2, LMainPnl.WinTop + frame.WinTop + frame.WinHeight - 2);

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
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitAttack((sbyte)(FLeader.FUnitAttack + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:466
    public void AttDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitAttack((sbyte)(FLeader.FUnitAttack - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:473
    public void DefUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitDefense((sbyte)(FLeader.FUnitDefense + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:480
    public void DefDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitDefense((sbyte)(FLeader.FUnitDefense - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:487
    public void DamUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitDamage((sbyte)(FLeader.FUnitDamage + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:494
    public void DamDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitDamage((sbyte)(FLeader.FUnitDamage - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:501
    public void ResUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitResistance((sbyte)(FLeader.FUnitResistance + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:508
    public void ResDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitResistance((sbyte)(FLeader.FUnitResistance - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:515
    public void HitUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitHits((sbyte)(FLeader.FUnitHits + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:522
    public void HitDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitHits((sbyte)(FLeader.FUnitHits - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:529
    public void MovUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitMoves((sbyte)(FLeader.FUnitMoves + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:536
    public void MovDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FLeader.SetUnitMoves((sbyte)(FLeader.FUnitMoves - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:543
    public void ClearBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.TitleWindow.TitleWin.FDrawn = false;
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
        DialogHosts.General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(MagicSphere.Life))
        {
            FLeader.AddSphere(MagicSphere.Life);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:614
    public void SelFireClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(MagicSphere.Fire))
        {
            FLeader.AddSphere(MagicSphere.Fire);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:624
    public void SelDeathClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(MagicSphere.Death))
        {
            FLeader.AddSphere(MagicSphere.Death);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:634
    public void SelWaterClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(MagicSphere.Water))
        {
            FLeader.AddSphere(MagicSphere.Water);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:644
    public void SelAirClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(MagicSphere.Air))
        {
            FLeader.AddSphere(MagicSphere.Air);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:654
    public void SelEarthClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FLeader.CanAddSphere(MagicSphere.Earth))
        {
            FLeader.AddSphere(MagicSphere.Earth);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:664
    public void DeleteSphereClick(int Slot)
    {
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
        var objectValue = RaceCB.Items.Objects[RaceCB.Selected];
        FLeader.SetRace((sbyte)ObjectToInt(objectValue));
    }

    // AoWLeaderSetup.pas:678
    public void SelectedAbilitiesSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWLeaderSetup.pas:715
    public void SExtraClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWLeaderSetup.pas:720
    public void SFinalClick()
    {
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWLeaderSetup.pas:725
    public void SelectedAbilitiesCostMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        Ability Ability;
        if (Button == MouseButton.Right && SelectedAbilities.Selected != -1)
        {
            var objectValue = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
            Ability = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (Ability != null)
            {
                var name = Ability.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, Ability.FDescription);
            }
        }
    }

    // AoWLeaderSetup.pas:737
    public void AvailableAbilitiesMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        Ability Ability;
        if (Button == MouseButton.Right && AvailableAbilities.Selected != -1)
        {
            var objectValue = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
            Ability = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (Ability != null)
            {
                var name = Ability.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, Ability.FDescription);
            }
        }
    }

    private static readonly string FinishRStr = "Finish";
    private static readonly string NextRStr = "Next";
    private static readonly string PleaseAssignAllSpheresRStr = "Please assign all spheres!";
    private static readonly string SelectedSpheresRStr = "Selected Spheres";
    private static readonly string SkillPointsRStr = "Skill Points";
    private static readonly string Step1CustomizeLeaderRStr = "Step 1: Customize Leader";
    private static readonly string Step2AssignSkillPointsRStr = "Step 2: Assign Skill Points";
    private static readonly string Step3AssignSpheresRStr = "Step 3: Assign Spheres";
}
