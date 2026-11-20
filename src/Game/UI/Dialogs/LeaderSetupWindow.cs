#nullable disable
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

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// setup control, phase, fixed spheres, working leader, selected sort, available sort
public partial class LeaderSetupWindow(UIContext ui) : UIModule(ui)
{
    public ListBox SelectedAbilitiesList { get; private set; }
    public Panel Phase2Panel { get; private set; }
    public Panel LeaderMainPanel { get; private set; }
    public Button NextButton { get; private set; }
    public Button DefaultButton { get; private set; }
    public Button ClearButton { get; private set; }
    public Button PreviousButton { get; private set; }
    public ListBox AvailableAbilitiesList { get; private set; }
    public Label AttackValueLabel { get; private set; }
    public Label DamageValueLabel { get; private set; }
    public Label DefenseValueLabel { get; private set; }
    public Label ResistanceValueLabel { get; private set; }
    public Label HitsValueLabel { get; private set; }
    public Label MovesValueLabel { get; private set; }
    public Label AttackAddedLabel { get; private set; }
    public Label DamageAddedLabel { get; private set; }
    public Label DefenseAddedLabel { get; private set; }
    public Label ResistanceAddedLabel { get; private set; }
    public Label HitsAddedLabel { get; private set; }
    public Label MovesAddedLabel { get; private set; }
    public Label AttackBaseLabel { get; private set; }
    public Label DamageBaseLabel { get; private set; }
    public Label DefenseBaseLabel { get; private set; }
    public Label ResistanceBaseLabel { get; private set; }
    public Label HitsBaseLabel { get; private set; }
    public Label MovesBaseLabel { get; private set; }
    public Label SkillPointsLabel { get; private set; }
    public Label UnitAttackLabel { get; private set; }
    public Label UnitDefenseLabel { get; private set; }
    public Label UnitMovesLabel { get; private set; }
    public Label UnitDamageLabel { get; private set; }
    public Label UnitResistanceLabel { get; private set; }
    public Label UnitHitsLabel { get; private set; }
    public Panel StatisticsPanel { get; private set; }
    public ListBox UnitAbilitiesList { get; private set; }
    public Panel AbilityPanel { get; private set; }
    public Label LeaderNameLabel { get; private set; }
    public Image LeftShield { get; private set; }
    public Image RightShield { get; private set; }
    public Image MainFace { get; private set; }
    public Panel Phase1Panel { get; private set; }
    public Edit NameEdit { get; private set; }
    public Label RaceLabel { get; private set; }
    public Label Phase1Header { get; private set; }
    public VerticalScrollBar AvailableAbilitiesScrollBar { get; private set; }
    public VerticalScrollBar SelectedAbilitiesScrollBar { get; private set; }
    public Panel Phase3Panel { get; private set; }
    public Button SelectDeathButton { get; private set; }
    public Button SelectLifeButton { get; private set; }
    public Button SelectFireButton { get; private set; }
    public Button SelectEarthButton { get; private set; }
    public Button SelectAirButton { get; private set; }
    public Button SelectWaterButton { get; private set; }
    public Button SelectedSphere4Button { get; private set; }
    public Button SelectedSphere3Button { get; private set; }
    public Button SelectedSphere2Button { get; private set; }
    public Button SelectedSphere1Button { get; private set; }
    public Button SelectedSphere5Button { get; private set; }
    public Button SelectedSphere6Button { get; private set; }
    public Button SelectedSphere7Button { get; private set; }
    public Label SelectedSpheresLabel { get; private set; }
    public Label LeaderLabel { get; private set; }
    public Label SideNameLabel { get; private set; }
    public ListBox SelectedAbilitiesCostList { get; private set; }
    public ListBox AvailableAbilitiesCostList { get; private set; }
    public Frame MainFaceFrame { get; private set; }
    public Label Phase2Header { get; private set; }
    public Label Phase3Header { get; private set; }
    public Panel LeaderPanel { get; private set; }
    public ComboBox RaceComboBox { get; private set; }

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
        _ = (Setup?.Release());

        Setup = control;
        _ = Setup.AddRef();
        _selectedSort = 0;
        _availableSort = 0;
        LeaderPanel.ImageLibraryTexture = UI.Graphics.TitleScreen;
        LeaderPanel.ImageLibraryIndexTexture = 4;
        RaceComboBox.Visible = Setup.Items.Count != 0 && Setup.Mode.Contains(LeaderSetupModeElement.Campaign);
        RaceLabel.Visible = RaceComboBox.Visible;
        if (RaceComboBox.Visible)
        {
            RaceComboBox.Items.Clear();
            for (int i = 0, last = Setup.Items.Count - 1; i <= last; i++)
            {
                int item = Setup.Items[i];
                string raceNames = UI.Engine.TranslateResourceString(GameTables.RaceNames[item]);
                int item2 = Setup.Items[i];
                _ = RaceComboBox.Items.AddObject(raceNames, IntToObject(item2));
            }

            int indexOf = Setup.Items.IndexOf(Setup.OriginalLeader.Race);
            RaceComboBox.Selected = indexOf;
        }

        string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[Setup.OriginalLeader.Race]);
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
        SaveLeader();
        LeaderSetupControl control = Setup;
        control.Status = LeaderSetupControl.LeaderSetupStatus.Done;
        Setup = null;
        LeaderPanel.Visible = false;
        UI.SetupEvents.UpdatePlayerName(control.OriginalLeader.HeroName);
        _ = control.Done();
        _ = control.Release();
    }

    // AoWLeaderSetup.pas:1097
    public void Cancel()
    {
        SaveLeader();
        LeaderSetupControl control = Setup;
        control.Status = LeaderSetupControl.LeaderSetupStatus.Canceled;
        Setup = null;
        LeaderPanel.Visible = false;
        _ = control.Done();
        _ = control.Release();
    }

    // AoWLeaderSetup.pas:750
    public void LoadLeader() => Leader.Assign(Setup.OriginalLeader, UI.Engine);

    // AoWLeaderSetup.pas:755
    public void SaveLeader()
    {
        Setup.OriginalLeader.Assign(Leader, UI.Engine);
        Leader leader = Setup.OriginalLeader;
        LeaderNameLabel.Text = leader.HeroName;
        sbyte inherentAttack = leader.GetInherentAttack();
        UnitAttackLabel.Text = IntToStr(inherentAttack);
        sbyte inherentDefense = leader.GetInherentDefense();
        UnitDefenseLabel.Text = IntToStr(inherentDefense);
        sbyte inherentDamage = leader.GetInherentDamage();
        UnitDamageLabel.Text = IntToStr(inherentDamage);
        sbyte inherentResistance = leader.InherentResistance;
        UnitResistanceLabel.Text = IntToStr(inherentResistance);
        sbyte hits = leader.GetHits();
        UnitHitsLabel.Text = IntToStr(hits);
        sbyte moves = leader.Moves;
        UnitMovesLabel.Text = IntToStr(moves);
        UnitAbilitiesList.Items.Clear();
        leader.ListAbilities(UnitAbilitiesList.Items);
        string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[leader.Race]);
        SideNameLabel.Text = raceNamesPlural;
        LeftShield.CurrentImage = leader.Race + 13;
        RightShield.CurrentImage = leader.Race + 13;
    }

    // AoWLeaderSetup.pas:775
    public void UpdateSkillPoints()
    {
        int skillPoints = Leader.GetSkillPoints();
        string skillPointsText = UI.Engine.TranslateResourceString(s_skillPointsText);
        int skillPointsMax = Leader.SkillPointsMax;
        SkillPointsLabel.Text = $"{skillPointsText} {IntToStr(skillPoints)}/{IntToStr(skillPointsMax)}";
    }

    // AoWLeaderSetup.pas:781
    public void UpdateStatistics()
    {
        HeroResource resource = Leader.HeroResource;
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
        sbyte inherentAttack = Leader.GetInherentAttack();
        AttackValueLabel.Text = IntToStr(inherentAttack);
        sbyte inherentDefense = Leader.GetInherentDefense();
        DefenseValueLabel.Text = IntToStr(inherentDefense);
        sbyte inherentDamage = Leader.GetInherentDamage();
        DamageValueLabel.Text = IntToStr(inherentDamage);
        sbyte inherentResistance = Leader.InherentResistance;
        ResistanceValueLabel.Text = IntToStr(inherentResistance);
        sbyte hits = Leader.GetHits();
        HitsValueLabel.Text = IntToStr(hits);
        sbyte moves = Leader.Moves;
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
            default:
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
            default:
                break;
        }
    }

    // AoWLeaderSetup.pas:852
    public void UpdateAbilities()
    {
        AvailableAbilitiesList.Items.Clear();
        AvailableAbilitiesCostList.Items.Clear();
        for (int i = 0, last = GamePack.Current.AbilityControl.GetCount() - 1; i <= last; i++)
        {
            if (GamePack.Current.AbilityControl[i] is not null && (GamePack.Current.AbilityControl[i].SelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of())
            {
                if (GamePack.Current.AbilityControl[i].CanExpand(Leader))
                {
                    string expandName = GamePack.Current.AbilityControl[i].ExpandName(Leader);
                    _ = AvailableAbilitiesList.Items.AddObject(expandName, IntToObject(i));
                    int expandCost = GamePack.Current.AbilityControl[i].ExpandCost(Leader);
                    _ = AvailableAbilitiesCostList.Items.Add(IntToStr(expandCost));
                }
            }
        }

        SelectedAbilitiesList.Items.Clear();
        SelectedAbilitiesCostList.Items.Clear();
        for (int i = 0, last = Leader.OwnAbilityCount - 1; i <= last; i++)
        {
            Ability ability = GamePack.Current.AbilityControl[i];
            if (Leader.GetAbilitySetAt(i) && (Leader.HeroResource.Abilities.GetAbilitySetAt(i) || (ability.SelectionTypes & Set<AbilitySelectionType>.Of(AbilitySelectionType.CustomizeLeader, AbilitySelectionType.HeroUpgrade)) != Set<AbilitySelectionType>.Of()))
            {
                string name = GamePack.Current.AbilityControl[i].GetName(Leader);
                string cost;
                if (Leader.HeroResource.Abilities.GetAbilitySetAt(i) && Leader.HeroResource.Abilities.GetOwnAbilityLevel(i) == Leader.GetAbilityLevel(i))
                {
                    name = $"{name} ({UI.Engine.TranslateResourceString(GameText.FixedText)})";
                    cost = "-";
                }
                else
                {
                    cost = IntToStr(ability.GetSkillPoints(Leader) - ability.GetSkillPoints(Leader.HeroResource.Abilities));
                }

                _ = SelectedAbilitiesCostList.Items.Add(cost);
                _ = SelectedAbilitiesList.Items.AddObject(name, IntToObject(i));
            }
        }

        int count = AvailableAbilitiesList.Items.Count;
        AvailableAbilitiesScrollBar.Max = count - 1;
        int itemsCount = SelectedAbilitiesList.Items.Count;
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
    public bool CanAddAbility(int abilityId) => GamePack.Current.AbilityControl[abilityId].CanExpand(Leader) && GamePack.Current.AbilityControl[abilityId].ExpandCost(Leader) <= Leader.GetSkillPoints();

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
            string finish = UI.Engine.TranslateResourceString(s_finishText);
            NextButton.Text = finish;
        }
        else
        {
            string next = UI.Engine.TranslateResourceString(s_nextText);
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
            default:
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
        string step1CustomizeLeader = UI.Engine.TranslateResourceString(s_step1CustomizeLeaderText);
        Phase1Header.Text = step1CustomizeLeader;
        string step2AssignSkillPoints = UI.Engine.TranslateResourceString(s_step2AssignSkillPointsText);
        Phase2Header.Text = step2AssignSkillPoints;
        string step3AssignSpheres = UI.Engine.TranslateResourceString(s_step3AssignSpheresText);
        Phase3Header.Text = step3AssignSpheres;
        SetFixedSpheres(0);
    }

    // AoWLeaderSetup.pas:943
    public void UpdateSpheres()
    {
        for (int i = 0; i <= 6; i++)
        {
            Button button = i switch
            {
                0 => SelectedSphere1Button,
                1 => SelectedSphere2Button,
                2 => SelectedSphere3Button,
                3 => SelectedSphere4Button,
                4 => SelectedSphere5Button,
                5 => SelectedSphere6Button,
                6 => SelectedSphere7Button,
                _ => null,
            };
            if (i < Leader.SphereCount)
            {
                button.Visible = true;
                switch ((int)Leader.GetSpheres(i) & 127)
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
                    default:
                        break;
                }
            }
            else
            {
                button.Visible = false;
            }

            if (i < _fixedSpheres)
            {
                button.CaptureInput = false;
                button.ImageLibraryIndexUp++;
            }
            else
            {
                button.CaptureInput = true;
            }
        }

        int sphereCount = Leader.SphereCount;
        string selectedSpheres = UI.Engine.TranslateResourceString(s_selectedSpheresText);
        SelectedSpheresLabel.Text = $"{selectedSpheres} ({IntToStr(sphereCount)}/{IntToStr(Leader.SpherePicksMax)})";
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
    public void LeaderSetupWinCreate() => Leader = new Leader(null) { Holder = UI.Engine };

    // AoWLeaderSetup.pas:274
    public void LeaderSetupWinDestroy() => Leader?.Dispose();

    // AoWLeaderSetup.pas:279
    public void AddAbilityClick()
    {
        UI.General.PlayButtonSound(0);
        if (AvailableAbilitiesList.Selected != -1)
        {
            int abilityId = ObjectToInt(AvailableAbilitiesList.Items.Objects[AvailableAbilitiesList.Selected]);
            if (CanAddAbility(abilityId))
            {
                _ = Leader.ExpandAbility(abilityId);
                UpdateAbilities();
                UpdateLeader();
            }
        }
    }

    // AoWLeaderSetup.pas:296
    public void RemoveAbilityClick()
    {
        UI.General.PlayButtonSound(0);
        if (SelectedAbilitiesList.Selected != -1)
        {
            int abilityId = ObjectToInt(SelectedAbilitiesList.Items.Objects[SelectedAbilitiesList.Selected]);
            if (Leader.HeroResource.Abilities.GetAbilitySetAt(abilityId) && Leader.HeroResource.Abilities.GetOwnAbilityLevel(abilityId) == Leader.GetAbilityLevel(abilityId))
            {
                return;
            }

            _ = Leader.RemoveAbility(abilityId);
            UpdateAbilities();
            UpdateLeader();
        }
    }

    // AoWLeaderSetup.pas:312
    public void DrawSelectionFace(Frame sender, LockedSurface lockedSurface, int offset)
    {
        LeaderSetupControl setup = Setup;
        int index = Leader.FaceIndex;
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

        Frame image = sender;
        Rect r = new(Phase1Panel.AbsoluteLeft + image.WinLeft + 2, Phase1Panel.AbsoluteTop + image.WinTop, Phase1Panel.AbsoluteLeft + image.WinLeft + image.WinWidth - 2, Phase1Panel.AbsoluteTop + image.WinTop + image.WinHeight - 2);

        if (setup.OriginalLeader.GetFace() is not null)
        {
            FaceResource face = GamePack.Current.FaceResources.GetFaceResource(index);
            _ = face.AddRef();
            face.FaceImage.ShowStretched(r, lockedSurface);
            _ = face.Release();
        }
    }

    // AoWLeaderSetup.pas:345
    public void FacePrevBtnClick()
    {
        UI.General.PlayButtonSound(0);
        int previousFace = GamePack.Current.FaceResources.PreviousFace(Leader.FaceIndex);
        Leader.SetFaceIndex(previousFace);
        UI.TitleWindow.MainWindow.Drawn = false;
    }

    // AoWLeaderSetup.pas:352
    public void FaceNextBtnClick()
    {
        UI.General.PlayButtonSound(0);
        int nextFace = GamePack.Current.FaceResources.NextFace(Leader.FaceIndex);
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
            default:
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
                if (Leader.SphereCount == Leader.SpherePicksMax)
                {
                    Finish();
                }
                else
                {
                    MessageDialog.ResetMessageDialog(UI.MessageDialog);
                    _ = UI.MessageDialog.MessageMemo.Items.Add("");
                    string pleaseAssignAllSpheres = UI.Engine.TranslateResourceString(s_pleaseAssignAllSpheresText);
                    _ = UI.MessageDialog.MessageMemo.Items.Add(pleaseAssignAllSpheres);
                    UI.MessageDialog.OKPanel.Visible = true;
                    UI.MessageDialog.Window.Visible = true;
                }

                break;
            default:
                break;
        }
    }

    // AoWLeaderSetup.pas:405
    public void MainFaceFrPreDraw(Frame sender, LockedSurface lockedSurface)
    {
        LeaderSetupControl setup = Setup;
        if (_phase > 1)
        {
            Frame frame = sender;
            Rect r = new(LeaderMainPanel.AbsoluteLeft + frame.WinLeft + 2, LeaderMainPanel.AbsoluteTop + frame.WinTop, LeaderMainPanel.AbsoluteLeft + frame.WinLeft + frame.WinWidth - 2, LeaderMainPanel.AbsoluteTop + frame.WinTop + frame.WinHeight - 2);

            setup.OriginalLeader.GetFace()?.ShowStretched(r, lockedSurface);
        }
    }

    // AoWLeaderSetup.pas:422
    public void NameEditChange() => Leader.HeroName = NameEdit.Text;

    // AoWLeaderSetup.pas:427
    public void SelectedAbilitiesCostChange() => SelectSelectedAbility(SelectedAbilitiesCostList.Selected);

    // AoWLeaderSetup.pas:432
    public void SelectedAbilitiesChange() => SelectSelectedAbility(SelectedAbilitiesList.Selected);

    // AoWLeaderSetup.pas:437
    public void AvailableAbilitiesCostChange() => SelectAvailableAbility(AvailableAbilitiesCostList.Selected);

    // AoWLeaderSetup.pas:442
    public void AvailableAbilitiesChange() => SelectAvailableAbility(AvailableAbilitiesList.Selected);

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
                    Leader leader = Leader;
                    leader.SetNickname("");
                    leader.HeroName = "";
                    UpdateLeader();
                }

                break;
            case 2:
                {
                    Leader leader = Leader;
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
            default:
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
                    Leader leader = Leader;
                    leader.SetNickname(Setup.Leader.GetNickname());
                    leader.HeroName = Setup.Leader.HeroName;
                    leader.SetFaceIndex(Setup.Leader.FaceIndex);
                    UpdateLeader();
                }

                break;
            case 2:
                {
                    Leader leader = Leader;
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
                    Leader leader = Leader;
                    leader.ClearSpheres();
                    for (int i = 0, last = Setup.Leader.SphereCount - 1; i <= last; i++)
                    {
                        _ = leader.AddSphere(Setup.Leader.GetSpheres(i));
                    }

                    UpdateLeader();
                    UpdateSpheres();
                }

                break;
            default:
                break;
        }
    }

    // AoWLeaderSetup.pas:604
    public void SelLifeClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Life))
        {
            _ = Leader.AddSphere(MagicSphere.Life);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:614
    public void SelFireClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Fire))
        {
            _ = Leader.AddSphere(MagicSphere.Fire);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:624
    public void SelDeathClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Death))
        {
            _ = Leader.AddSphere(MagicSphere.Death);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:634
    public void SelWaterClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Water))
        {
            _ = Leader.AddSphere(MagicSphere.Water);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:644
    public void SelAirClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Air))
        {
            _ = Leader.AddSphere(MagicSphere.Air);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:654
    public void SelEarthClick()
    {
        UI.General.PlayButtonSound(0);
        if (Leader.CanAddSphere(MagicSphere.Earth))
        {
            _ = Leader.AddSphere(MagicSphere.Earth);
            UpdateSpheres();
        }
    }

    // AoWLeaderSetup.pas:664
    public void DeleteSphereClick(int slot)
    {
        UI.General.PlayButtonSound(0);
        int sphereCount = Leader.SphereCount;
        if (slot < sphereCount)
        {
            _ = Leader.DeleteSphereIndex(slot);
        }

        UpdateSpheres();
    }

    // AoWLeaderSetup.pas:672
    public void RaceCBChange()
    {
        UI.General.PlayButtonSound(0);
        object objectValue = RaceComboBox.Items.Objects[RaceComboBox.Selected];
        Leader.SetRace((sbyte)ObjectToInt(objectValue));
    }

    // AoWLeaderSetup.pas:678
    public void SelectedAbilitiesSortClick()
    {
        UI.General.PlayButtonSound(0);
        _selectedSort = _selectedSort == 0 ? 1 : 0;

        SortSelectedAbilities();
    }

    // AoWLeaderSetup.pas:686
    public void SelectedAbilitiesCostSortClick()
    {
        UI.General.PlayButtonSound(0);
        _selectedSort = _selectedSort == 2 ? 3 : 2;

        SortSelectedAbilities();
    }

    // AoWLeaderSetup.pas:694
    public void AvailableAbilitiesSortClick()
    {
        UI.General.PlayButtonSound(0);
        _availableSort = _availableSort == 0 ? 1 : 0;

        SortAvailableAbilities();
    }

    // AoWLeaderSetup.pas:702
    public void AvailableAbilitiesCostSortClick()
    {
        UI.General.PlayButtonSound(0);
        _availableSort = _availableSort == 2 ? 3 : 2;

        SortAvailableAbilities();
    }

    // AoWLeaderSetup.pas:710
    public void SBaseClick() => UI.General.PlayButtonSound(0);

    // AoWLeaderSetup.pas:715
    public void SExtraClick() => UI.General.PlayButtonSound(0);

    // AoWLeaderSetup.pas:720
    public void SFinalClick() => UI.General.PlayButtonSound(0);

    // AoWLeaderSetup.pas:725
    public void SelectedAbilitiesCostMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right && SelectedAbilitiesList.Selected != -1)
        {
            object objectValue = SelectedAbilitiesList.Items.Objects[SelectedAbilitiesList.Selected];
            Ability ability = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
            if (ability is not null)
            {
                InfoBanner.SetInfoBanner(caption: ability.GetName(null), banner: UI.InfoBanner, description: ability.Description);
            }
        }
    }

    // AoWLeaderSetup.pas:737
    public void AvailableAbilitiesMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right && AvailableAbilitiesList.Selected != -1)
        {
            object objectValue = AvailableAbilitiesList.Items.Objects[AvailableAbilitiesList.Selected];
            Ability ability = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
            if (ability is not null)
            {
                InfoBanner.SetInfoBanner(caption: ability.GetName(null), banner: UI.InfoBanner, description: ability.Description);
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
