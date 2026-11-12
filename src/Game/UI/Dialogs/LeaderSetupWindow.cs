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

    public LeaderSetupControl Setup { get; set; }

    private int _phase;
    private int _fixedSpheres;

    public Leader Leader { get; set; }

    private int _selectedSort;
    private int _availableSort;

    // AoWLeaderSetup.pas:1050
    public void Execute(LeaderSetupControl control)
    {
        Reset();
        if (Setup != null)
        {
            Setup.Release();
        }

        Setup = control;
        Setup.AddRef();
        _selectedSort = 0;
        _availableSort = 0;
        LeaderPnl.ImageLibraryTexture = DialogHosts.UIGraphics.TitleScreen;
        LeaderPnl.ImageLibraryIndexTexture = 4;
        RaceCB.Visible = Setup.Items.Count != 0 && Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        RaceLbl.Visible = RaceCB.Visible;
        if (RaceCB.Visible)
        {
            RaceCB.Items.Clear();
            for (int I = 0, last = Setup.Items.Count - 1; I <= last; I++)
            {
                var item = Setup.Items[I];
                var raceNames = AbilityRules.TranslateResourceString(GameTables.s_raceNames[item]);
                var item2 = Setup.Items[I];
                RaceCB.Items.AddObject(raceNames, IntToObject(item2));
            }

            var indexOf = Setup.Items.IndexOf(Setup.OriginalLeader.Race);
            RaceCB.Selected = indexOf;
        }

        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[Setup.OriginalLeader.Race]);
        SideNameLbl.Text = raceNamesPlural;
        ShieldL.CurrentImage = Setup.OriginalLeader.Race + 13;
        ShieldR.CurrentImage = Setup.OriginalLeader.Race + 13;
        ShieldR.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        LoadLeader();
        SaveLeader();
        if (Setup.Mode.Contains(LeaderSetupModeElement.Look))
        {
            SetPhase(1);
        }
        else
        {
            if (Setup.Mode.Contains(LeaderSetupModeElement.Settings))
            {
                SetPhase(2);
            }
            else
            {
                if (Setup.Mode.Contains(LeaderSetupModeElement.Spheres))
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
        LeaderSetupControl control;
        SaveLeader();
        control = Setup;
        control.Status = LeaderSetupControl.LeaderSetupStatus.Done;
        Setup = null;
        LeaderPnl.Visible = false;
        DialogHosts.SetupEvents.UpdatePlayerName(control.OriginalLeader.HeroName);
        control.Done();
        control.Release();
    }

    // AoWLeaderSetup.pas:1097
    public void Cancel()
    {
        LeaderSetupControl control;
        SaveLeader();
        control = Setup;
        control.Status = LeaderSetupControl.LeaderSetupStatus.Canceled;
        Setup = null;
        LeaderPnl.Visible = false;
        control.Done();
        control.Release();
    }

    // AoWLeaderSetup.pas:750
    public void LoadLeader()
    {
        Leader.Assign(Setup.OriginalLeader, GameTables.GameEngine);
    }

    // AoWLeaderSetup.pas:755
    public void SaveLeader()
    {
        Leader leader;
        Setup.OriginalLeader.Assign(Leader, GameTables.GameEngine);
        leader = Setup.OriginalLeader;
        LeaderName.Text = leader.HeroName;
        var inherentAttack = leader.GetInherentAttack();
        UAttack.Text = IntToStr(inherentAttack);
        var inherentDefense = leader.GetInherentDefense();
        UDefense.Text = IntToStr(inherentDefense);
        var inherentDamage = leader.GetInherentDamage();
        UDamage.Text = IntToStr(inherentDamage);
        var inherentResistance = leader.GetInherentResistance();
        UResistance.Text = IntToStr(inherentResistance);
        var hits = leader.GetHits();
        UHits.Text = IntToStr(hits);
        var moves = leader.GetMoves();
        UMoves.Text = IntToStr(moves);
        UAbilities.Items.Clear();
        leader.ListAbilities(UAbilities.Items);
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[leader.Race]);
        SideNameLbl.Text = raceNamesPlural;
        ShieldL.CurrentImage = leader.Race + 13;
        ShieldR.CurrentImage = leader.Race + 13;
    }

    // AoWLeaderSetup.pas:775
    public void UpdateSkillPoints()
    {
        var skillPoints = Leader.GetSkillPoints();
        var skillPointsText = AbilityRules.TranslateResourceString(s_skillPointsText);
        var skillPointsMax = Leader.GetSkillPointsMax();
        SkillPointsLbl.Text = skillPointsText + " " + IntToStr(skillPoints) + "/" + IntToStr(skillPointsMax);
    }

    // AoWLeaderSetup.pas:781
    public void UpdateStatistics()
    {
        HeroResource resource;
        resource = Leader.HeroResource;
        AttB.Text = IntToStr(resource.Attack);
        DefB.Text = IntToStr(resource.Defense);
        DamB.Text = IntToStr(resource.Damage);
        ResB.Text = IntToStr(resource.Resistance);
        HitB.Text = IntToStr(resource.Hits);
        MovB.Text = IntToStr(resource.Moves);
        AttA.Text = IntToStr(Leader.UnitAttack);
        DefA.Text = IntToStr(Leader.UnitDefense);
        DamA.Text = IntToStr(Leader.UnitDamage);
        ResA.Text = IntToStr(Leader.UnitResistance);
        HitA.Text = IntToStr(Leader.UnitHits);
        MovA.Text = IntToStr(Leader.UnitMoves);
        var inherentAttack = Leader.GetInherentAttack();
        AttValue.Text = IntToStr(inherentAttack);
        var inherentDefense = Leader.GetInherentDefense();
        DefValue.Text = IntToStr(inherentDefense);
        var inherentDamage = Leader.GetInherentDamage();
        DamValue.Text = IntToStr(inherentDamage);
        var inherentResistance = Leader.GetInherentResistance();
        ResValue.Text = IntToStr(inherentResistance);
        var hits = Leader.GetHits();
        HitValue.Text = IntToStr(hits);
        var moves = Leader.GetMoves();
        MovValue.Text = IntToStr(moves);
        UpdateSkillPoints();
    }

    // AoWLeaderSetup.pas:807
    public void UpdateLeader()
    {
        NameEdit.Text = Leader.HeroName;
        UpdateAbilities();
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:814
    public void SortSelectedAbilities()
    {
        switch (_selectedSort)
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
        switch (_availableSort)
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
        string name;
        string cost;
        Ability ability;
        AvailableAbilities.Items.Clear();
        AvailableAbilitiesCost.Items.Clear();
        for (int I = 0, last = AoWHSSet.Current.AbilityControl.GetCount() - 1; I <= last; I++)
        {
            if (AoWHSSet.Current.AbilityControl.GetAbility(I) != null && (AoWHSSet.Current.AbilityControl.GetAbility(I).SelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of())
            {
                if (AoWHSSet.Current.AbilityControl.GetAbility(I).CanExpand(Leader))
                {
                    var expandName = AoWHSSet.Current.AbilityControl.GetAbility(I).ExpandName(Leader);
                    AvailableAbilities.Items.AddObject(expandName, IntToObject(I));
                    var expandCost = AoWHSSet.Current.AbilityControl.GetAbility(I).ExpandCost(Leader);
                    AvailableAbilitiesCost.Items.Add(IntToStr(expandCost));
                }
            }
        }

        SelectedAbilities.Items.Clear();
        SelectedAbilitiesCost.Items.Clear();
        for (int I = 0, last = Leader.GetAbCount() - 1; I <= last; I++)
        {
            ability = AoWHSSet.Current.AbilityControl.GetAbility(I);
            if (Leader.GetAbilitySetAt(I) && (Leader.HeroResource.Abilities.GetAbilitySetAt(I) || (ability.SelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of()))
            {
                name = AoWHSSet.Current.AbilityControl.GetAbility(I).GetName(Leader);
                if (Leader.HeroResource.Abilities.GetAbilitySetAt(I) && Leader.HeroResource.Abilities.GetAbLevel(I) == Leader.GetAbilityLevel(I))
                {
                    name = name + " (" + AbilityRules.TranslateResourceString(GameText.FixedText) + ")";
                    cost = "-";
                }
                else
                {
                    cost = IntToStr(ability.GetSkillPoints(Leader) - ability.GetSkillPoints(Leader.HeroResource.Abilities));
                }

                SelectedAbilitiesCost.Items.Add(cost);
                SelectedAbilities.Items.AddObject(name, IntToObject(I));
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
    public void SelectAvailableAbility(int index)
    {
        AvailableAbilities.Selected = index;
        AvailableAbilitiesCost.Selected = index;
    }

    // AoWLeaderSetup.pas:824
    public void SelectSelectedAbility(int index)
    {
        SelectedAbilities.Selected = index;
        SelectedAbilitiesCost.Selected = index;
    }

    // AoWLeaderSetup.pas:846
    public bool CanAddAbility(int abilityId) => AoWHSSet.Current.AbilityControl.GetAbility(abilityId).CanExpand(Leader) && AoWHSSet.Current.AbilityControl.GetAbility(abilityId).ExpandCost(Leader) <= Leader.GetSkillPoints();

    // AoWLeaderSetup.pas:913
    public void SetPhase(int value)
    {
        _phase = value;
        Phase1Pnl.Visible = value == 1;
        Phase2Pnl.Visible = value == 2;
        Phase3Pnl.Visible = value == 3;
        Phase1Header.Visible = value == 1;
        Phase2Header.Visible = value == 2;
        Phase3Header.Visible = value == 3;
        StatPnl.Visible = value > 2;
        AbilityPnl.Visible = value > 2;
        LeaderName.Visible = value > 1;
        LeaderLbl.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        SideNameLbl.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        ShieldL.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign) || value > 1;
        ShieldR.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign) || value > 1;
        MainFaceFr.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign) || value > 1;
        MainFace.Visible = MainFaceFr.Visible;
        if (value == 3 || (value == 2 && (Setup.Mode & Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Spheres)) == Set<LeaderSetupModeElement>.Of()) || (value == 1 && (Setup.Mode & Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Settings, LeaderSetupModeElement.Spheres)) == Set<LeaderSetupModeElement>.Of()))
        {
            var finish = AbilityRules.TranslateResourceString(s_finishText);
            NextBtn.Text = finish;
        }
        else
        {
            var next = AbilityRules.TranslateResourceString(s_nextText);
            NextBtn.Text = next;
        }

        switch (value)
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
    public void SetFixedSpheres(int value)
    {
        _fixedSpheres = value;
        UpdateSpheres();
    }

    // AoWLeaderSetup.pas:901
    public void Reset()
    {
        ClearBtn.Visible = true;
        DefaultBtn.Visible = true;
        PrevBtn.Visible = true;
        NextBtn.Visible = true;
        var step1CustomizeLeader = AbilityRules.TranslateResourceString(s_step1CustomizeLeaderText);
        Phase1Header.Text = step1CustomizeLeader;
        var step2AssignSkillPoints = AbilityRules.TranslateResourceString(s_step2AssignSkillPointsText);
        Phase2Header.Text = step2AssignSkillPoints;
        var step3AssignSpheres = AbilityRules.TranslateResourceString(s_step3AssignSpheresText);
        Phase3Header.Text = step3AssignSpheres;
        SetFixedSpheres(0);
    }

    // AoWLeaderSetup.pas:943
    public void UpdateSpheres()
    {
        Button button;
        for (int I = 0; I <= 6; I++)
        {
            switch (I)
            {
                case 0:
                    button = Sel1;
                    break;
                case 1:
                    button = Sel2;
                    break;
                case 2:
                    button = Sel3;
                    break;
                case 3:
                    button = Sel4;
                    break;
                case 4:
                    button = Sel5;
                    break;
                case 5:
                    button = Sel6;
                    break;
                case 6:
                    button = Sel7;
                    break;
                default:
                    button = null;
                    break;
            }

            if (I < Leader.GetSphereCount())
            {
                button.Visible = true;
                switch ((int)Leader.GetSpheres(I) & 127)
                {
                    case 1:
                        button.ImageLibraryIndexDown = 28;
                        button.ImageLibraryIndexLit = 29;
                        button.ImageLibraryIndexUp = 27;
                        break;
                    case 5:
                        button.ImageLibraryIndexDown = 43;
                        button.ImageLibraryIndexLit = 44;
                        button.ImageLibraryIndexUp = 42;
                        break;
                    case 3:
                        button.ImageLibraryIndexDown = 34;
                        button.ImageLibraryIndexLit = 35;
                        button.ImageLibraryIndexUp = 33;
                        break;
                    case 4:
                        button.ImageLibraryIndexDown = 37;
                        button.ImageLibraryIndexLit = 38;
                        button.ImageLibraryIndexUp = 36;
                        break;
                    case 6:
                        button.ImageLibraryIndexDown = 40;
                        button.ImageLibraryIndexLit = 41;
                        button.ImageLibraryIndexUp = 39;
                        break;
                    case 2:
                        button.ImageLibraryIndexDown = 31;
                        button.ImageLibraryIndexLit = 32;
                        button.ImageLibraryIndexUp = 30;
                        break;
                }
            }
            else
            {
                button.Visible = false;
            }

            if (I < _fixedSpheres)
            {
                button.CaptureInput = false;
                button.ImageLibraryIndexUp = button.ImageLibraryIndexUp + 1;
            }
            else
            {
                button.CaptureInput = true;
            }
        }

        var sphereCount = Leader.GetSphereCount();
        var selectedSpheres = AbilityRules.TranslateResourceString(s_selectedSpheresText);
        SelectedSpheresLbl.Text = selectedSpheres + " (" + IntToStr(sphereCount) + "/" + IntToStr(Leader.SpherePicksMax) + ")";
        if (Leader.CanAddSphere(MagicSphere.Life))
        {
            SelLife.ImageLibraryIndexUp = 27;
            SelLife.CaptureInput = true;
        }
        else
        {
            SelLife.ImageLibraryIndexUp = 28;
            SelLife.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Death))
        {
            SelDeath.ImageLibraryIndexUp = 30;
            SelDeath.CaptureInput = true;
        }
        else
        {
            SelDeath.ImageLibraryIndexUp = 31;
            SelDeath.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Fire))
        {
            SelFire.ImageLibraryIndexUp = 42;
            SelFire.CaptureInput = true;
        }
        else
        {
            SelFire.ImageLibraryIndexUp = 43;
            SelFire.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Water))
        {
            SelWater.ImageLibraryIndexUp = 39;
            SelWater.CaptureInput = true;
        }
        else
        {
            SelWater.ImageLibraryIndexUp = 40;
            SelWater.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Earth))
        {
            SelEarth.ImageLibraryIndexUp = 33;
            SelEarth.CaptureInput = true;
        }
        else
        {
            SelEarth.ImageLibraryIndexUp = 34;
            SelEarth.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Air))
        {
            SelAir.ImageLibraryIndexUp = 36;
            SelAir.CaptureInput = true;
        }
        else
        {
            SelAir.ImageLibraryIndexUp = 37;
            SelAir.CaptureInput = false;
        }

        SelLife.Drawn = false;
        SelDeath.Drawn = false;
        SelFire.Drawn = false;
        SelWater.Drawn = false;
        SelEarth.Drawn = false;
        SelAir.Drawn = false;
    }

    // AoWLeaderSetup.pas:269
    public void LeaderSetupWinCreate()
    {
        Leader = new Leader(null);
    }

    // AoWLeaderSetup.pas:274
    public void LeaderSetupWinDestroy()
    {
        Leader?.Dispose();
    }

    // AoWLeaderSetup.pas:279
    public void AddAbilityClick()
    {
        int abilityId;
        DialogHosts.General.PlayButtonSound(0);
        if (AvailableAbilities.Selected != -1)
        {
            abilityId = ObjectToInt(AvailableAbilities.Items.Objects[AvailableAbilities.Selected]);
            if (CanAddAbility(abilityId))
            {
                Leader.ExpandAbility(abilityId);
                UpdateAbilities();
                UpdateLeader();
            }
        }
    }

    // AoWLeaderSetup.pas:296
    public void RemoveAbilityClick()
    {
        int abilityId;
        DialogHosts.General.PlayButtonSound(0);
        if (SelectedAbilities.Selected != -1)
        {
            abilityId = ObjectToInt(SelectedAbilities.Items.Objects[SelectedAbilities.Selected]);
            if (Leader.HeroResource.Abilities.GetAbilitySetAt(abilityId) && Leader.HeroResource.Abilities.GetAbLevel(abilityId) == Leader.GetAbilityLevel(abilityId))
            {
                return;
            }

            Leader.RemoveAbility(abilityId);
            UpdateAbilities();
            UpdateLeader();
        }
    }

    // AoWLeaderSetup.pas:312
    public void DrawSelectionFace(Frame sender, LockedSurface lockedSurface, int offset)
    {
        LeaderSetupControl setup;
        int index;
        Rect R;
        FaceResource face;
        setup = Setup;
        index = Leader.FaceIndex;
        while (offset < 0)
        {
            offset += 1;
            index = AoWHSSet.Current.FaceResources.PreviousFace(index);
        }

        while (offset > 0)
        {
            offset -= 1;
            index = AoWHSSet.Current.FaceResources.NextFace(index);
        }

        var image = sender;
        R = new Rect(Phase1Pnl.WinLeft + image.WinLeft + 2, Phase1Pnl.WinTop + image.WinTop, Phase1Pnl.WinLeft + image.WinLeft + image.WinWidth - 2, Phase1Pnl.WinTop + image.WinTop + image.WinHeight - 2);

        if (setup.OriginalLeader.GetFace() != null)
        {
            face = AoWHSSet.Current.FaceResources.GetFaceResource(index);
            face.AddRef();
            face.GetFaceImage().ShowStretched(R, lockedSurface);
            face.Release();
        }
    }

    // AoWLeaderSetup.pas:345
    public void FacePrevBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var previousFace = AoWHSSet.Current.FaceResources.PreviousFace(Leader.FaceIndex);
        Leader.SetFaceIndex(previousFace);
        DialogHosts.TitleWindow.TitleWin.Drawn = false;
    }

    // AoWLeaderSetup.pas:352
    public void FaceNextBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        var nextFace = AoWHSSet.Current.FaceResources.NextFace(Leader.FaceIndex);
        Leader.SetFaceIndex(nextFace);
        DialogHosts.TitleWindow.TitleWin.Drawn = false;
    }

    // AoWLeaderSetup.pas:359
    public void PrevBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        switch (_phase)
        {
            case 1:
                if (Setup.Mode.Contains(LeaderSetupModeElement.Campaign))
                {
                    Cancel();
                }

                break;
            case 2:
                SaveLeader();
                if (Setup.Mode.Contains(LeaderSetupModeElement.Look))
                {
                    SetPhase(1);
                }

                break;
            case 3:
                SaveLeader();
                if (Setup.Mode.Contains(LeaderSetupModeElement.Settings))
                {
                    SetPhase(2);
                }
                else
                {
                    if (Setup.Mode.Contains(LeaderSetupModeElement.Look))
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
        switch (_phase)
        {
            case 1:
                SaveLeader();
                if (Setup.Mode.Contains(LeaderSetupModeElement.Settings))
                {
                    SetPhase(2);
                }
                else
                {
                    if (Setup.Mode.Contains(LeaderSetupModeElement.Spheres))
                    {
                        SetPhase(3);
                    }
                    else
                    {
                        Finish();
                    }
                }

                DialogHosts.TitleWindow.TitleWin.Drawn = false;
                break;
            case 2:
                SaveLeader();
                if (Setup.Mode.Contains(LeaderSetupModeElement.Spheres))
                {
                    SetPhase(3);
                }
                else
                {
                    Finish();
                }

                DialogHosts.TitleWindow.TitleWin.Drawn = false;
                break;
            case 3:
                if (Leader.GetSphereCount() == Leader.SpherePicksMax)
                {
                    Finish();
                }
                else
                {
                    MessageDialog.ResetMessageDialog(DialogHosts.MessageDialog);
                    DialogHosts.MessageDialog.MessageMemo.Items.Add("");
                    var pleaseAssignAllSpheres = AbilityRules.TranslateResourceString(s_pleaseAssignAllSpheresText);
                    DialogHosts.MessageDialog.MessageMemo.Items.Add(pleaseAssignAllSpheres);
                    DialogHosts.MessageDialog.OKPnl.Visible = true;
                    DialogHosts.MessageDialog.Dlg.Visible = true;
                }

                break;
        }
    }

    // AoWLeaderSetup.pas:405
    public void MainFaceFrPreDraw(Frame sender, LockedSurface lockedSurface)
    {
        LeaderSetupControl setup;
        Rect R;
        setup = Setup;
        if (_phase > 1)
        {
            var frame = sender;
            R = new Rect(LMainPnl.WinLeft + frame.WinLeft + 2, LMainPnl.WinTop + frame.WinTop, LMainPnl.WinLeft + frame.WinLeft + frame.WinWidth - 2, LMainPnl.WinTop + frame.WinTop + frame.WinHeight - 2);

            if (setup.OriginalLeader.GetFace() != null)
            {
                setup.OriginalLeader.GetFace().ShowStretched(R, lockedSurface);
            }
        }
    }

    // AoWLeaderSetup.pas:422
    public void NameEditChange()
    {
        Leader.HeroName = NameEdit.Text;
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
        Leader.SetUnitAttack((sbyte)(Leader.UnitAttack + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:466
    public void AttDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitAttack((sbyte)(Leader.UnitAttack - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:473
    public void DefUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitDefense((sbyte)(Leader.UnitDefense + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:480
    public void DefDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitDefense((sbyte)(Leader.UnitDefense - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:487
    public void DamUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitDamage((sbyte)(Leader.UnitDamage + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:494
    public void DamDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitDamage((sbyte)(Leader.UnitDamage - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:501
    public void ResUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitResistance((sbyte)(Leader.UnitResistance + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:508
    public void ResDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitResistance((sbyte)(Leader.UnitResistance - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:515
    public void HitUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitHits((sbyte)(Leader.UnitHits + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:522
    public void HitDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitHits((sbyte)(Leader.UnitHits - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:529
    public void MovUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitMoves((sbyte)(Leader.UnitMoves + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:536
    public void MovDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Leader.SetUnitMoves((sbyte)(Leader.UnitMoves - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:543
    public void ClearBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        switch (_phase)
        {
            case 1:
                {
                    var leader = Leader;
                    leader.SetNickname("");
                    leader.HeroName = "";
                    UpdateLeader();
                }

                break;
            case 2:
                {
                    var leader = Leader;
                    leader.AssignAbility(leader.HeroResource.Abilities);
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
                Leader.ClearSpheres();
                UpdateSpheres();
                break;
        }
    }

    // AoWLeaderSetup.pas:568
    public void DefaultBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.TitleWindow.TitleWin.Drawn = false;
        switch (_phase)
        {
            case 1:
                {
                    var leader = Leader;
                    leader.SetNickname(Setup.Leader.GetNickname());
                    leader.HeroName = Setup.Leader.HeroName;
                    leader.SetFaceIndex(Setup.Leader.FaceIndex);
                    UpdateLeader();
                }

                break;
            case 2:
                {
                    var leader = Leader;
                    leader.AssignAbility(Setup.Leader);
                    leader.SetUnitAttack(Setup.Leader.UnitAttack);
                    leader.SetUnitDefense(Setup.Leader.UnitDefense);
                    leader.SetUnitDamage(Setup.Leader.UnitDamage);
                    leader.SetUnitHits(Setup.Leader.UnitHits);
                    leader.SetUnitMoves(Setup.Leader.UnitMoves);
                    leader.SetUnitResistance(Setup.Leader.UnitResistance);
                    UpdateLeader();
                }

                break;
            case 3:
                {
                    var leader = Leader;
                    leader.ClearSpheres();
                    for (int I = 0, last = Setup.Leader.GetSphereCount() - 1; I <= last; I++)
                    {
                        leader.AddSphere(Setup.Leader.GetSpheres(I));
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
        if (Leader.CanAddSphere(MagicSphere.Life))
        {
            Leader.AddSphere(MagicSphere.Life);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:614
    public void SelFireClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Fire))
        {
            Leader.AddSphere(MagicSphere.Fire);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:624
    public void SelDeathClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Death))
        {
            Leader.AddSphere(MagicSphere.Death);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:634
    public void SelWaterClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Water))
        {
            Leader.AddSphere(MagicSphere.Water);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:644
    public void SelAirClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Air))
        {
            Leader.AddSphere(MagicSphere.Air);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:654
    public void SelEarthClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Earth))
        {
            Leader.AddSphere(MagicSphere.Earth);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:664
    public void DeleteSphereClick(int slot)
    {
        DialogHosts.General.PlayButtonSound(0);
        var sphereCount = Leader.GetSphereCount();
        if (slot < sphereCount)
        {
            Leader.DeleteSphereIndex(slot);
        }

        UpdateSpheres();
    }

    // AoWLeaderSetup.pas:672
    public void RaceCBChange()
    {
        DialogHosts.General.PlayButtonSound(0);
        var objectValue = RaceCB.Items.Objects[RaceCB.Selected];
        Leader.SetRace((sbyte)ObjectToInt(objectValue));
    }

    // AoWLeaderSetup.pas:678
    public void SelectedAbilitiesSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_selectedSort == 0)
        {
            _selectedSort = 1;
        }
        else
        {
            _selectedSort = 0;
        }

        SortSelectedAbilities();
    }

    // AoWLeaderSetup.pas:686
    public void SelectedAbilitiesCostSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_selectedSort == 2)
        {
            _selectedSort = 3;
        }
        else
        {
            _selectedSort = 2;
        }

        SortSelectedAbilities();
    }

    // AoWLeaderSetup.pas:694
    public void AvailableAbilitiesSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_availableSort == 0)
        {
            _availableSort = 1;
        }
        else
        {
            _availableSort = 0;
        }

        SortAvailableAbilities();
    }

    // AoWLeaderSetup.pas:702
    public void AvailableAbilitiesCostSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_availableSort == 2)
        {
            _availableSort = 3;
        }
        else
        {
            _availableSort = 2;
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
    public void SelectedAbilitiesCostMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (button == MouseButton.Right && SelectedAbilities.Selected != -1)
        {
            var objectValue = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
            ability = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (ability != null)
            {
                var name = ability.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, ability.Description);
            }
        }
    }

    // AoWLeaderSetup.pas:737
    public void AvailableAbilitiesMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (button == MouseButton.Right && AvailableAbilities.Selected != -1)
        {
            var objectValue = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
            ability = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (ability != null)
            {
                var name = ability.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, ability.Description);
            }
        }
    }

    private static readonly string s_finishText = "Finish";
    private static readonly string s_nextText = "Next";
    private static readonly string s_pleaseAssignAllSpheresText = "Please assign all spheres!";
    private static readonly string s_selectedSpheresText = "Selected Spheres";
    private static readonly string s_skillPointsText = "Skill Points";
    private static readonly string s_step1CustomizeLeaderText = "Step 1: Customize Leader";
    private static readonly string s_step2AssignSkillPointsText = "Step 2: Assign Skill Points";
    private static readonly string s_step3AssignSpheresText = "Step 3: Assign Spheres";
}
