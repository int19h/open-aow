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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// setup control, phase, fixed spheres, working leader, selected sort, available sort
public partial class LeaderSetupWindow : UIModule
{
    public LeaderSetupWindow(UIContext ui) : base(ui)
    { }

    public ListBox SelectedAbilitiesList;
    public Panel Phase2Panel;
    public Panel LeaderMainPanel;
    public Button NextButton;
    public Button DefaultButton;
    public Button ClearButton;
    public Button PreviousButton;
    public ListBox AvailableAbilitiesList;
    public Label AttackValueLabel;
    public Label DamageValueLabel;
    public Label DefenseValueLabel;
    public Label ResistanceValueLabel;
    public Label HitsValueLabel;
    public Label MovesValueLabel;
    public Label AttackAddedLabel;
    public Label DamageAddedLabel;
    public Label DefenseAddedLabel;
    public Label ResistanceAddedLabel;
    public Label HitsAddedLabel;
    public Label MovesAddedLabel;
    public Label AttackBaseLabel;
    public Label DamageBaseLabel;
    public Label DefenseBaseLabel;
    public Label ResistanceBaseLabel;
    public Label HitsBaseLabel;
    public Label MovesBaseLabel;
    public Label SkillPointsLabel;
    public Label UnitAttackLabel;
    public Label UnitDefenseLabel;
    public Label UnitMovesLabel;
    public Label UnitDamageLabel;
    public Label UnitResistanceLabel;
    public Label UnitHitsLabel;
    public Panel StatisticsPanel;
    public ListBox UnitAbilitiesList;
    public Panel AbilityPanel;
    public Label LeaderNameLabel;
    public Image LeftShield;
    public Image RightShield;
    public Image MainFace;
    public Panel Phase1Panel;
    public Edit NameEdit;
    public Label RaceLabel;
    public Label Phase1Header;
    public VerticalScrollBar AvailableAbilitiesScrollBar;
    public VerticalScrollBar SelectedAbilitiesScrollBar;
    public Panel Phase3Panel;
    public Button SelectDeathButton;
    public Button SelectLifeButton;
    public Button SelectFireButton;
    public Button SelectEarthButton;
    public Button SelectAirButton;
    public Button SelectWaterButton;
    public Button SelectedSphere4Button;
    public Button SelectedSphere3Button;
    public Button SelectedSphere2Button;
    public Button SelectedSphere1Button;
    public Button SelectedSphere5Button;
    public Button SelectedSphere6Button;
    public Button SelectedSphere7Button;
    public Label SelectedSpheresLabel;
    public Label LeaderLabel;
    public Label SideNameLabel;
    public ListBox SelectedAbilitiesCostList;
    public ListBox AvailableAbilitiesCostList;
    public Frame MainFaceFrame;
    public Label Phase2Header;
    public Label Phase3Header;
    public Panel LeaderPanel;
    public ComboBox RaceComboBox;

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
        LeaderPanel.ImageLibraryTexture = UI.Graphics.TitleScreen;
        LeaderPanel.ImageLibraryIndexTexture = 4;
        RaceComboBox.Visible = Setup.Items.Count != 0 && Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        RaceLabel.Visible = RaceComboBox.Visible;
        if (RaceComboBox.Visible)
        {
            RaceComboBox.Items.Clear();
            for (int I = 0, last = Setup.Items.Count - 1; I <= last; I++)
            {
                var item = Setup.Items[I];
                var raceNames = AbilityRules.TranslateResourceString(GameTables.RaceNames[item]);
                var item2 = Setup.Items[I];
                RaceComboBox.Items.AddObject(raceNames, IntToObject(item2));
            }

            var indexOf = Setup.Items.IndexOf(Setup.OriginalLeader.Race);
            RaceComboBox.Selected = indexOf;
        }

        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[Setup.OriginalLeader.Race]);
        SideNameLabel.Text = raceNamesPlural;
        LeftShield.CurrentImage = Setup.OriginalLeader.Race + 13;
        RightShield.CurrentImage = Setup.OriginalLeader.Race + 13;
        RightShield.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
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

        LeaderPanel.Visible = true;
    }

    // AoWLeaderSetup.pas:1083
    public void Finish()
    {
        LeaderSetupControl control;
        SaveLeader();
        control = Setup;
        control.Status = LeaderSetupControl.LeaderSetupStatus.Done;
        Setup = null;
        LeaderPanel.Visible = false;
        UI.SetupEvents.UpdatePlayerName(control.OriginalLeader.HeroName);
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
        LeaderPanel.Visible = false;
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
        LeaderNameLabel.Text = leader.HeroName;
        var inherentAttack = leader.GetInherentAttack();
        UnitAttackLabel.Text = IntToStr(inherentAttack);
        var inherentDefense = leader.GetInherentDefense();
        UnitDefenseLabel.Text = IntToStr(inherentDefense);
        var inherentDamage = leader.GetInherentDamage();
        UnitDamageLabel.Text = IntToStr(inherentDamage);
        var inherentResistance = leader.GetInherentResistance();
        UnitResistanceLabel.Text = IntToStr(inherentResistance);
        var hits = leader.GetHits();
        UnitHitsLabel.Text = IntToStr(hits);
        var moves = leader.GetMoves();
        UnitMovesLabel.Text = IntToStr(moves);
        UnitAbilitiesList.Items.Clear();
        leader.ListAbilities(UnitAbilitiesList.Items);
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[leader.Race]);
        SideNameLabel.Text = raceNamesPlural;
        LeftShield.CurrentImage = leader.Race + 13;
        RightShield.CurrentImage = leader.Race + 13;
    }

    // AoWLeaderSetup.pas:775
    public void UpdateSkillPoints()
    {
        var skillPoints = Leader.GetSkillPoints();
        var skillPointsText = AbilityRules.TranslateResourceString(s_skillPointsText);
        var skillPointsMax = Leader.GetSkillPointsMax();
        SkillPointsLabel.Text = skillPointsText + " " + IntToStr(skillPoints) + "/" + IntToStr(skillPointsMax);
    }

    // AoWLeaderSetup.pas:781
    public void UpdateStatistics()
    {
        HeroResource resource;
        resource = Leader.HeroResource;
        AttackBaseLabel.Text = IntToStr(resource.Attack);
        DefenseBaseLabel.Text = IntToStr(resource.Defense);
        DamageBaseLabel.Text = IntToStr(resource.Damage);
        ResistanceBaseLabel.Text = IntToStr(resource.Resistance);
        HitsBaseLabel.Text = IntToStr(resource.Hits);
        MovesBaseLabel.Text = IntToStr(resource.Moves);
        AttackAddedLabel.Text = IntToStr(Leader.UnitAttack);
        DefenseAddedLabel.Text = IntToStr(Leader.UnitDefense);
        DamageAddedLabel.Text = IntToStr(Leader.UnitDamage);
        ResistanceAddedLabel.Text = IntToStr(Leader.UnitResistance);
        HitsAddedLabel.Text = IntToStr(Leader.UnitHits);
        MovesAddedLabel.Text = IntToStr(Leader.UnitMoves);
        var inherentAttack = Leader.GetInherentAttack();
        AttackValueLabel.Text = IntToStr(inherentAttack);
        var inherentDefense = Leader.GetInherentDefense();
        DefenseValueLabel.Text = IntToStr(inherentDefense);
        var inherentDamage = Leader.GetInherentDamage();
        DamageValueLabel.Text = IntToStr(inherentDamage);
        var inherentResistance = Leader.GetInherentResistance();
        ResistanceValueLabel.Text = IntToStr(inherentResistance);
        var hits = Leader.GetHits();
        HitsValueLabel.Text = IntToStr(hits);
        var moves = Leader.GetMoves();
        MovesValueLabel.Text = IntToStr(moves);
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
                StringSorting.SortStringsAscending(SelectedAbilitiesList.Items, [SelectedAbilitiesCostList.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(SelectedAbilitiesList.Items, [SelectedAbilitiesCostList.Items]);
                break;
            case 2:
                StringSorting.SortStringsNumericalAscending(SelectedAbilitiesCostList.Items, [SelectedAbilitiesList.Items]);
                break;
            case 3:
                StringSorting.SortStringsNumericalDescending(SelectedAbilitiesCostList.Items, [SelectedAbilitiesList.Items]);
                break;
        }
    }

    // AoWLeaderSetup.pas:830
    public void SortAvailableAbilities()
    {
        switch (_availableSort)
        {
            case 0:
                StringSorting.SortStringsAscending(AvailableAbilitiesList.Items, [AvailableAbilitiesCostList.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(AvailableAbilitiesList.Items, [AvailableAbilitiesCostList.Items]);
                break;
            case 2:
                StringSorting.SortStringsNumericalAscending(AvailableAbilitiesCostList.Items, [AvailableAbilitiesList.Items]);
                break;
            case 3:
                StringSorting.SortStringsNumericalDescending(AvailableAbilitiesCostList.Items, [AvailableAbilitiesList.Items]);
                break;
        }
    }

    // AoWLeaderSetup.pas:852
    public void UpdateAbilities()
    {
        string name;
        string cost;
        Ability ability;
        AvailableAbilitiesList.Items.Clear();
        AvailableAbilitiesCostList.Items.Clear();
        for (int I = 0, last = GamePack.Current.AbilityControl.GetCount() - 1; I <= last; I++)
        {
            if (GamePack.Current.AbilityControl.GetAbility(I) != null && (GamePack.Current.AbilityControl.GetAbility(I).SelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of())
            {
                if (GamePack.Current.AbilityControl.GetAbility(I).CanExpand(Leader))
                {
                    var expandName = GamePack.Current.AbilityControl.GetAbility(I).ExpandName(Leader);
                    AvailableAbilitiesList.Items.AddObject(expandName, IntToObject(I));
                    var expandCost = GamePack.Current.AbilityControl.GetAbility(I).ExpandCost(Leader);
                    AvailableAbilitiesCostList.Items.Add(IntToStr(expandCost));
                }
            }
        }

        SelectedAbilitiesList.Items.Clear();
        SelectedAbilitiesCostList.Items.Clear();
        for (int I = 0, last = Leader.GetAbCount() - 1; I <= last; I++)
        {
            ability = GamePack.Current.AbilityControl.GetAbility(I);
            if (Leader.GetAbilitySetAt(I) && (Leader.HeroResource.Abilities.GetAbilitySetAt(I) || (ability.SelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of()))
            {
                name = GamePack.Current.AbilityControl.GetAbility(I).GetName(Leader);
                if (Leader.HeroResource.Abilities.GetAbilitySetAt(I) && Leader.HeroResource.Abilities.GetAbLevel(I) == Leader.GetAbilityLevel(I))
                {
                    name = name + " (" + AbilityRules.TranslateResourceString(GameText.FixedText) + ")";
                    cost = "-";
                }
                else
                {
                    cost = IntToStr(ability.GetSkillPoints(Leader) - ability.GetSkillPoints(Leader.HeroResource.Abilities));
                }

                SelectedAbilitiesCostList.Items.Add(cost);
                SelectedAbilitiesList.Items.AddObject(name, IntToObject(I));
            }
        }

        var count = AvailableAbilitiesList.Items.Count;
        AvailableAbilitiesScrollBar.Max = count - 1;
        var itemsCount = SelectedAbilitiesList.Items.Count;
        SelectedAbilitiesScrollBar.Max = itemsCount - 1;
        SortAvailableAbilities();
        SortSelectedAbilities();
    }

    // AoWLeaderSetup.pas:840
    public void SelectAvailableAbility(int index)
    {
        AvailableAbilitiesList.Selected = index;
        AvailableAbilitiesCostList.Selected = index;
    }

    // AoWLeaderSetup.pas:824
    public void SelectSelectedAbility(int index)
    {
        SelectedAbilitiesList.Selected = index;
        SelectedAbilitiesCostList.Selected = index;
    }

    // AoWLeaderSetup.pas:846
    public bool CanAddAbility(int abilityId) => GamePack.Current.AbilityControl.GetAbility(abilityId).CanExpand(Leader) && GamePack.Current.AbilityControl.GetAbility(abilityId).ExpandCost(Leader) <= Leader.GetSkillPoints();

    // AoWLeaderSetup.pas:913
    public void SetPhase(int value)
    {
        _phase = value;
        Phase1Panel.Visible = value == 1;
        Phase2Panel.Visible = value == 2;
        Phase3Panel.Visible = value == 3;
        Phase1Header.Visible = value == 1;
        Phase2Header.Visible = value == 2;
        Phase3Header.Visible = value == 3;
        StatisticsPanel.Visible = value > 2;
        AbilityPanel.Visible = value > 2;
        LeaderNameLabel.Visible = value > 1;
        LeaderLabel.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        SideNameLabel.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        LeftShield.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign) || value > 1;
        RightShield.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign) || value > 1;
        MainFaceFrame.Visible = !Setup.Mode.Contains(LeaderSetupModeElement.Campaign) || value > 1;
        MainFace.Visible = MainFaceFrame.Visible;
        if (value == 3 || (value == 2 && (Setup.Mode & Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Spheres)) == Set<LeaderSetupModeElement>.Of()) || (value == 1 && (Setup.Mode & Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Settings, LeaderSetupModeElement.Spheres)) == Set<LeaderSetupModeElement>.Of()))
        {
            var finish = AbilityRules.TranslateResourceString(s_finishText);
            NextButton.Text = finish;
        }
        else
        {
            var next = AbilityRules.TranslateResourceString(s_nextText);
            NextButton.Text = next;
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
        ClearButton.Visible = true;
        DefaultButton.Visible = true;
        PreviousButton.Visible = true;
        NextButton.Visible = true;
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
                    button = SelectedSphere1Button;
                    break;
                case 1:
                    button = SelectedSphere2Button;
                    break;
                case 2:
                    button = SelectedSphere3Button;
                    break;
                case 3:
                    button = SelectedSphere4Button;
                    break;
                case 4:
                    button = SelectedSphere5Button;
                    break;
                case 5:
                    button = SelectedSphere6Button;
                    break;
                case 6:
                    button = SelectedSphere7Button;
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
        SelectedSpheresLabel.Text = selectedSpheres + " (" + IntToStr(sphereCount) + "/" + IntToStr(Leader.SpherePicksMax) + ")";
        if (Leader.CanAddSphere(MagicSphere.Life))
        {
            SelectLifeButton.ImageLibraryIndexUp = 27;
            SelectLifeButton.CaptureInput = true;
        }
        else
        {
            SelectLifeButton.ImageLibraryIndexUp = 28;
            SelectLifeButton.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Death))
        {
            SelectDeathButton.ImageLibraryIndexUp = 30;
            SelectDeathButton.CaptureInput = true;
        }
        else
        {
            SelectDeathButton.ImageLibraryIndexUp = 31;
            SelectDeathButton.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Fire))
        {
            SelectFireButton.ImageLibraryIndexUp = 42;
            SelectFireButton.CaptureInput = true;
        }
        else
        {
            SelectFireButton.ImageLibraryIndexUp = 43;
            SelectFireButton.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Water))
        {
            SelectWaterButton.ImageLibraryIndexUp = 39;
            SelectWaterButton.CaptureInput = true;
        }
        else
        {
            SelectWaterButton.ImageLibraryIndexUp = 40;
            SelectWaterButton.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Earth))
        {
            SelectEarthButton.ImageLibraryIndexUp = 33;
            SelectEarthButton.CaptureInput = true;
        }
        else
        {
            SelectEarthButton.ImageLibraryIndexUp = 34;
            SelectEarthButton.CaptureInput = false;
        }

        if (Leader.CanAddSphere(MagicSphere.Air))
        {
            SelectAirButton.ImageLibraryIndexUp = 36;
            SelectAirButton.CaptureInput = true;
        }
        else
        {
            SelectAirButton.ImageLibraryIndexUp = 37;
            SelectAirButton.CaptureInput = false;
        }

        SelectLifeButton.Drawn = false;
        SelectDeathButton.Drawn = false;
        SelectFireButton.Drawn = false;
        SelectWaterButton.Drawn = false;
        SelectEarthButton.Drawn = false;
        SelectAirButton.Drawn = false;
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
        UI.General.PlayButtonSound(0);
        if (AvailableAbilitiesList.Selected != -1)
        {
            abilityId = ObjectToInt(AvailableAbilitiesList.Items.Objects[AvailableAbilitiesList.Selected]);
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
        UI.General.PlayButtonSound(0);
        if (SelectedAbilitiesList.Selected != -1)
        {
            abilityId = ObjectToInt(SelectedAbilitiesList.Items.Objects[SelectedAbilitiesList.Selected]);
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
            index = GamePack.Current.FaceResources.PreviousFace(index);
        }

        while (offset > 0)
        {
            offset -= 1;
            index = GamePack.Current.FaceResources.NextFace(index);
        }

        var image = sender;
        R = new Rect(Phase1Panel.WinLeft + image.WinLeft + 2, Phase1Panel.WinTop + image.WinTop, Phase1Panel.WinLeft + image.WinLeft + image.WinWidth - 2, Phase1Panel.WinTop + image.WinTop + image.WinHeight - 2);

        if (setup.OriginalLeader.GetFace() != null)
        {
            face = GamePack.Current.FaceResources.GetFaceResource(index);
            face.AddRef();
            face.GetFaceImage().ShowStretched(R, lockedSurface);
            face.Release();
        }
    }

    // AoWLeaderSetup.pas:345
    public void FacePrevBtnClick()
    {
        UI.General.PlayButtonSound(0);
        var previousFace = GamePack.Current.FaceResources.PreviousFace(Leader.FaceIndex);
        Leader.SetFaceIndex(previousFace);
        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWLeaderSetup.pas:352
    public void FaceNextBtnClick()
    {
        UI.General.PlayButtonSound(0);
        var nextFace = GamePack.Current.FaceResources.NextFace(Leader.FaceIndex);
        Leader.SetFaceIndex(nextFace);
        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWLeaderSetup.pas:359
    public void PrevBtnClick()
    {
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
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

                UI.TitleWindow.MainWindow.Drawn = false;
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

                UI.TitleWindow.MainWindow.Drawn = false;
                break;
            case 3:
                if (Leader.GetSphereCount() == Leader.SpherePicksMax)
                {
                    Finish();
                }
                else
                {
                    MessageDialog.ResetMessageDialog(UI.MessageDialog);
                    UI.MessageDialog.MessageMemo.Items.Add("");
                    var pleaseAssignAllSpheres = AbilityRules.TranslateResourceString(s_pleaseAssignAllSpheresText);
                    UI.MessageDialog.MessageMemo.Items.Add(pleaseAssignAllSpheres);
                    UI.MessageDialog.OKPanel.Visible = true;
                    UI.MessageDialog.Window.Visible = true;
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
            R = new Rect(LeaderMainPanel.WinLeft + frame.WinLeft + 2, LeaderMainPanel.WinTop + frame.WinTop, LeaderMainPanel.WinLeft + frame.WinLeft + frame.WinWidth - 2, LeaderMainPanel.WinTop + frame.WinTop + frame.WinHeight - 2);

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
        SelectSelectedAbility(SelectedAbilitiesCostList.Selected);
    }

    // AoWLeaderSetup.pas:432
    public void SelectedAbilitiesChange()
    {
        SelectSelectedAbility(SelectedAbilitiesList.Selected);
    }

    // AoWLeaderSetup.pas:437
    public void AvailableAbilitiesCostChange()
    {
        SelectAvailableAbility(AvailableAbilitiesCostList.Selected);
    }

    // AoWLeaderSetup.pas:442
    public void AvailableAbilitiesChange()
    {
        SelectAvailableAbility(AvailableAbilitiesList.Selected);
    }

    // AoWLeaderSetup.pas:447
    public void SelectedAbilitiesLBChange()
    {
        SelectedAbilitiesList.ListOffset = SelectedAbilitiesScrollBar.Position;
        SelectedAbilitiesCostList.ListOffset = SelectedAbilitiesScrollBar.Position;
    }

    // AoWLeaderSetup.pas:453
    public void AvailableAbilitiesLBChange()
    {
        AvailableAbilitiesList.ListOffset = AvailableAbilitiesScrollBar.Position;
        AvailableAbilitiesCostList.ListOffset = AvailableAbilitiesScrollBar.Position;
    }

    // AoWLeaderSetup.pas:459
    public void AttUpClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitAttack((sbyte)(Leader.UnitAttack + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:466
    public void AttDnClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitAttack((sbyte)(Leader.UnitAttack - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:473
    public void DefUpClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitDefense((sbyte)(Leader.UnitDefense + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:480
    public void DefDnClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitDefense((sbyte)(Leader.UnitDefense - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:487
    public void DamUpClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitDamage((sbyte)(Leader.UnitDamage + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:494
    public void DamDnClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitDamage((sbyte)(Leader.UnitDamage - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:501
    public void ResUpClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitResistance((sbyte)(Leader.UnitResistance + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:508
    public void ResDnClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitResistance((sbyte)(Leader.UnitResistance - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:515
    public void HitUpClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitHits((sbyte)(Leader.UnitHits + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:522
    public void HitDnClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitHits((sbyte)(Leader.UnitHits - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:529
    public void MovUpClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitMoves((sbyte)(Leader.UnitMoves + 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:536
    public void MovDnClick()
    {
        UI.General.PlayButtonSound(0);
        Leader.SetUnitMoves((sbyte)(Leader.UnitMoves - 1));
        UpdateStatistics();
    }

    // AoWLeaderSetup.pas:543
    public void ClearBtnClick()
    {
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
        UI.TitleWindow.MainWindow.Drawn = false;
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
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Life))
        {
            Leader.AddSphere(MagicSphere.Life);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:614
    public void SelFireClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Fire))
        {
            Leader.AddSphere(MagicSphere.Fire);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:624
    public void SelDeathClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Death))
        {
            Leader.AddSphere(MagicSphere.Death);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:634
    public void SelWaterClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Water))
        {
            Leader.AddSphere(MagicSphere.Water);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:644
    public void SelAirClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Air))
        {
            Leader.AddSphere(MagicSphere.Air);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:654
    public void SelEarthClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Earth))
        {
            Leader.AddSphere(MagicSphere.Earth);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:664
    public void DeleteSphereClick(int slot)
    {
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
        var objectValue = RaceComboBox.Items.Objects[RaceComboBox.Selected];
        Leader.SetRace((sbyte)ObjectToInt(objectValue));
    }

    // AoWLeaderSetup.pas:678
    public void SelectedAbilitiesSortClick()
    {
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
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
        UI.General.PlayButtonSound(0);
    }

    // AoWLeaderSetup.pas:715
    public void SExtraClick()
    {
        UI.General.PlayButtonSound(0);
    }

    // AoWLeaderSetup.pas:720
    public void SFinalClick()
    {
        UI.General.PlayButtonSound(0);
    }

    // AoWLeaderSetup.pas:725
    public void SelectedAbilitiesCostMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (button == MouseButton.Right && SelectedAbilitiesList.Selected != -1)
        {
            var objectValue = SelectedAbilitiesList.Items.Objects[SelectedAbilitiesList.Selected];
            ability = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (ability != null)
            {
                var name = ability.GetName(null);
                InfoBanner.SetInfoBanner(UI.InfoBanner, name, ability.Description);
            }
        }
    }

    // AoWLeaderSetup.pas:737
    public void AvailableAbilitiesMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (button == MouseButton.Right && AvailableAbilitiesList.Selected != -1)
        {
            var objectValue = AvailableAbilitiesList.Items.Objects[AvailableAbilitiesList.Selected];
            ability = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (ability != null)
            {
                var name = ability.GetName(null);
                InfoBanner.SetInfoBanner(UI.InfoBanner, name, ability.Description);
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
