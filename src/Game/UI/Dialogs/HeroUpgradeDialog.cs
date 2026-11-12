// Translated from AoWHeroUpgradeDlg.pas.
using Aow.Engine.Nodes;

using Aow.Game.Commands;
using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// close callback, referenced hero, list sort orders, live hero, editable copy
public partial class HeroUpgradeDialog : DataModule
{
    public HeroUpgradeDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label HeaderLabel;
    public ListBox AvailableAbilitiesList;
    public VerticalScrollBar AvailableAbilitiesScrollBar;
    public ListBox SelectedAbilitiesList;
    public VerticalScrollBar SelectedAbilitiesScrollBar;
    public ListBox AvailableAbilitiesCostList;
    public Edit UnitNameEdit;
    public Panel TopPanel;
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
    public ListBox SelectedAbilitiesCostList;
    public Label LevelLabel;
    public Label SkillPointsLabel;
    private event ExecuteDoneEvent OnClose;
    private HeroUpgradeEventLog _referencedHero;
    private int _selectedSort;
    private int _availableSort;
    private Hero _hero;
    private Hero _editedHero;

    // AoWHeroUpgradeDlg.pas:287
    public void ClearHero()
    {
        ExecuteDoneEvent onClose;
        HeroUpgradeEventLog hero;
        onClose = OnClose;
        hero = _referencedHero;
        OnClose = null;
        _referencedHero = null;
        if (onClose != null)
        {
            onClose(hero);
        }

        if (hero != null)
        {
            hero.Release();
        }

        _editedHero?.Dispose();
        _editedHero = null;
    }

    // AoWHeroUpgradeDlg.pas:280
    public bool ValidateHero()
    {
        _hero = StrategicMap.Current.UnitControl.FindUnit<Hero>(_referencedHero.UnitId);
        bool result = _hero != null;
        if (!result)
        {
            Window.Visible = false;
        }

        return result;
    }

    // AoWHeroUpgradeDlg.pas:257
    public void UpdateStatistics()
    {
        var inherentAttack = _hero.GetInherentAttack();
        AttackBaseLabel.Text = IntToStr(inherentAttack);
        var inherentDefense = _hero.GetInherentDefense();
        DefenseBaseLabel.Text = IntToStr(inherentDefense);
        var inherentDamage = _hero.GetInherentDamage();
        DamageBaseLabel.Text = IntToStr(inherentDamage);
        var inherentResistance = _hero.GetInherentResistance();
        ResistanceBaseLabel.Text = IntToStr(inherentResistance);
        var hits = _hero.GetHits();
        HitsBaseLabel.Text = IntToStr(hits);
        var moves = _hero.GetMoves();
        MovesBaseLabel.Text = IntToStr(moves);
        AttackAddedLabel.Text = IntToStr(_editedHero.UnitAttack - _hero.UnitAttack);
        DefenseAddedLabel.Text = IntToStr(_editedHero.UnitDefense - _hero.UnitDefense);
        DamageAddedLabel.Text = IntToStr(_editedHero.UnitDamage - _hero.UnitDamage);
        ResistanceAddedLabel.Text = IntToStr(_editedHero.UnitResistance - _hero.UnitResistance);
        HitsAddedLabel.Text = IntToStr(_editedHero.UnitHits - _hero.UnitHits);
        MovesAddedLabel.Text = IntToStr(_editedHero.UnitMoves - _hero.UnitMoves);
        var fEditedHeroInherentAttack = _editedHero.GetInherentAttack();
        AttackValueLabel.Text = IntToStr(fEditedHeroInherentAttack);
        var fEditedHeroInherentDefense = _editedHero.GetInherentDefense();
        DefenseValueLabel.Text = IntToStr(fEditedHeroInherentDefense);
        var fEditedHeroInherentDamage = _editedHero.GetInherentDamage();
        DamageValueLabel.Text = IntToStr(fEditedHeroInherentDamage);
        var fEditedHeroInherentResistance = _editedHero.GetInherentResistance();
        ResistanceValueLabel.Text = IntToStr(fEditedHeroInherentResistance);
        var fEditedHeroHits = _editedHero.GetHits();
        HitsValueLabel.Text = IntToStr(fEditedHeroHits);
        var fEditedHeroMoves = _editedHero.GetMoves();
        MovesValueLabel.Text = IntToStr(fEditedHeroMoves);
        var skillPoints = _editedHero.GetSkillPoints();
        var fHeroSkillPoints = _hero.GetSkillPoints();
        SkillPointsLabel.Text = IntToStr(skillPoints) + "/" + IntToStr(fHeroSkillPoints);
    }

    // AoWHeroUpgradeDlg.pas:252
    public void RefreshStatistics()
    {
        UpdateStatistics();
    }

    // AoWHeroUpgradeDlg.pas:242
    public void SortSelected()
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

    // AoWHeroUpgradeDlg.pas:232
    public void SortAvailable()
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

    // AoWHeroUpgradeDlg.pas:170
    public void RefreshAbilities()
    {
        string name;
        string cost;
        Ability ability;
        AvailableAbilitiesList.Items.Clear();
        AvailableAbilitiesCostList.Items.Clear();
        for (int I = 1, last = GamePack.Current.AbilityControl.GetCount() - 1; I <= last; I++)
        {
            ability = GamePack.Current.AbilityControl.GetAbility(I);
            if (ability != null && ability.CanExpand(_editedHero) && ability.SelectionTypes.Contains(AbilitySelectionType.HeroUpgrade))
            {
                var expandName = ability.ExpandName(_editedHero);
                AvailableAbilitiesList.Items.AddObject(expandName, IntToObject(I));
                var expandCost = ability.ExpandCost(_editedHero);
                AvailableAbilitiesCostList.Items.Add(IntToStr(expandCost));
            }
        }

        SelectedAbilitiesList.Items.Clear();
        SelectedAbilitiesCostList.Items.Clear();
        for (int I = 1, last = _editedHero.GetAbCount() - 1; I <= last; I++)
        {
            if (_editedHero.GetInherentAbility(I))
            {
                ability = GamePack.Current.AbilityControl.GetAbility(I);
                name = ability.GetName(_editedHero);
                if (_hero.GetAbEnabled(I) && _hero.GetInherentAbilityLevel(I) == _editedHero.GetInherentAbilityLevel(I))
                {
                    name = name + " (" + AbilityRules.TranslateResourceString(GameText.FixedText) + ")";
                    cost = "-";
                }
                else
                {
                    cost = IntToStr(ability.GetSkillPoints(_editedHero) - ability.GetSkillPoints(_hero));
                }

                SelectedAbilitiesCostList.Items.Add(cost);
                SelectedAbilitiesList.Items.AddObject(name, IntToObject(I));
            }
        }

        var count = AvailableAbilitiesList.Items.Count;
        AvailableAbilitiesScrollBar.Max = count - 1;
        var itemsCount = SelectedAbilitiesList.Items.Count;
        SelectedAbilitiesScrollBar.Max = itemsCount - 1;
        SortAvailable();
        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:226
    public void SetAvailableIndex(int index)
    {
        AvailableAbilitiesList.Selected = index;
        AvailableAbilitiesCostList.Selected = index;
    }

    // AoWHeroUpgradeDlg.pas:220
    public void SetSelectedIndex(int index)
    {
        SelectedAbilitiesList.Selected = index;
        SelectedAbilitiesCostList.Selected = index;
    }

    // AoWHeroUpgradeDlg.pas:212
    public bool CanAddAbility(int abilityId)
    {
        bool result;
        if (!GamePack.Current.AbilityControl.GetAbility(abilityId).CanExpand(_editedHero) || GamePack.Current.AbilityControl.GetAbility(abilityId).ExpandCost(_editedHero) > _editedHero.GetSkillPoints())
        {
            result = false;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // AoWHeroUpgradeDlg.pas:302
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWHeroUpgradeDlg.pas:308
    public void FaceFramePreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect R;
        if (ValidateHero())
        {
            var image = sender;
            R = new Rect(TopPanel.WinLeft + image.WinLeft + 2, TopPanel.WinTop + image.WinTop, TopPanel.WinLeft + image.WinLeft + image.WinWidth - 2, TopPanel.WinTop + image.WinTop + image.WinHeight - 2);

            _hero.GetFace().ShowStretched(R, lockedSurface);
        }
    }

    // AoWHeroUpgradeDlg.pas:323
    public void AttUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        _editedHero.SetUnitAttack((sbyte)(_editedHero.UnitAttack + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:330
    public void AttDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_editedHero.UnitAttack != _hero.UnitAttack)
        {
            _editedHero.SetUnitAttack((sbyte)(_editedHero.UnitAttack - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:338
    public void DefUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        _editedHero.SetUnitDefense((sbyte)(_editedHero.UnitDefense + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:345
    public void DefDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_editedHero.UnitDefense != _hero.UnitDefense)
        {
            _editedHero.SetUnitDefense((sbyte)(_editedHero.UnitDefense - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:353
    public void DamUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        _editedHero.SetUnitDamage((sbyte)(_editedHero.UnitDamage + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:360
    public void DamDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_editedHero.UnitDamage != _hero.UnitDamage)
        {
            _editedHero.SetUnitDamage((sbyte)(_editedHero.UnitDamage - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:368
    public void ResUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        _editedHero.SetUnitResistance((sbyte)(_editedHero.UnitResistance + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:375
    public void ResDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_editedHero.UnitResistance != _hero.UnitResistance)
        {
            _editedHero.SetUnitResistance((sbyte)(_editedHero.UnitResistance - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:383
    public void HitUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        _editedHero.SetUnitHits((sbyte)(_editedHero.UnitHits + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:390
    public void HitDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_editedHero.UnitHits != _hero.UnitHits)
        {
            _editedHero.SetUnitHits((sbyte)(_editedHero.UnitHits - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:398
    public void MovUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        _editedHero.SetUnitMoves((sbyte)(_editedHero.UnitMoves + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:405
    public void MovDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_editedHero.UnitMoves != _hero.UnitMoves)
        {
            _editedHero.SetUnitMoves((sbyte)(_editedHero.UnitMoves - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:413
    public void SelectedAbilitiesChange()
    {
        SetSelectedIndex(SelectedAbilitiesList.Selected);
    }

    // AoWHeroUpgradeDlg.pas:418
    public void SelectedAbilitiesCostChange()
    {
        SetSelectedIndex(SelectedAbilitiesCostList.Selected);
    }

    // AoWHeroUpgradeDlg.pas:423
    public void SelectedAbilitiesSBChange()
    {
        SelectedAbilitiesList.ListOffset = SelectedAbilitiesScrollBar.Position;
        SelectedAbilitiesCostList.ListOffset = SelectedAbilitiesScrollBar.Position;
    }

    // AoWHeroUpgradeDlg.pas:429
    public void AvailableAbilitiesChange()
    {
        SetAvailableIndex(AvailableAbilitiesList.Selected);
    }

    // AoWHeroUpgradeDlg.pas:434
    public void AvailableAbilitiesCostChange()
    {
        SetAvailableIndex(AvailableAbilitiesCostList.Selected);
    }

    // AoWHeroUpgradeDlg.pas:439
    public void AvailableAbilitiesSBChange()
    {
        AvailableAbilitiesList.ListOffset = AvailableAbilitiesScrollBar.Position;
        AvailableAbilitiesCostList.ListOffset = AvailableAbilitiesScrollBar.Position;
    }

    // AoWHeroUpgradeDlg.pas:445
    public void AddAbilityBtnClick()
    {
        ListBox list;
        int index;
        int abilityId;
        DialogHosts.General.PlayButtonSound(0);
        list = AvailableAbilitiesList;
        index = list.Selected;
        if (index != -1)
        {
            abilityId = ObjectToInt(list.Items.Objects[index]);
            if (CanAddAbility(abilityId))
            {
                _editedHero.ExpandAbility(abilityId);
                RefreshAbilities();
                RefreshStatistics();
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:465
    public void RemoveBtnClick()
    {
        ListBox list;
        int index;
        int abilityId;
        DialogHosts.General.PlayButtonSound(0);
        list = SelectedAbilitiesList;
        index = list.Selected;
        if (index != -1)
        {
            abilityId = ObjectToInt(list.Items.Objects[index]);
            if (_hero.GetInherentAbility(abilityId))
            {
                if (_hero.GetInherentAbilityLevel(abilityId) == _editedHero.GetInherentAbilityLevel(abilityId))
                {
                    return;
                }
            }

            _editedHero.RemoveAbility(abilityId);
            RefreshAbilities();
            RefreshStatistics();
        }
    }

    // AoWHeroUpgradeDlg.pas:485
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

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:493
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

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:501
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

        SortAvailable();
    }

    // AoWHeroUpgradeDlg.pas:509
    public void AvailableAbilityCostSortClick()
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

        SortAvailable();
    }

    // AoWHeroUpgradeDlg.pas:517
    public void DoneClick()
    {
        HeroUpgradeTokenEvent eventValue;
        if (ValidateHero())
        {
            if (!StrategicMap.Current.Players.GetPlayers(_hero.Player).GetBusy())
            {
                eventValue = new HeroUpgradeTokenEvent(null);
                eventValue.Setup(_hero);
                eventValue.Attack = (sbyte)(_editedHero.UnitAttack - _hero.UnitAttack);
                eventValue.Defense = (sbyte)(_editedHero.UnitDefense - _hero.UnitDefense);
                eventValue.Damage = (sbyte)(_editedHero.UnitDamage - _hero.UnitDamage);
                eventValue.Resistance = (sbyte)(_editedHero.UnitResistance - _hero.UnitResistance);
                eventValue.Hits = (sbyte)(_editedHero.UnitHits - _hero.UnitHits);
                eventValue.Moves = (sbyte)(_editedHero.UnitMoves - _hero.UnitMoves);
                eventValue.Abilities.AssignAbility(_editedHero);
                StrategicMap.Current.TokenManager.GetTokenControl(_hero.Player).AddEvent(eventValue);
                eventValue.Release();
                Window.Visible = false;
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:539
    public void DlgHide()
    {
        ClearHero();
    }

    // AoWHeroUpgradeDlg.pas:544
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
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, ability.Description);
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:556
    public void SelectedAbilitiesMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (button == MouseButton.Right && SelectedAbilitiesList.Selected != -1)
        {
            var objectValue = SelectedAbilitiesList.Items.Objects[SelectedAbilitiesList.Selected];
            ability = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (ability != null)
            {
                var name = ability.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, ability.Description);
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:568
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowHeroUpgrade(HeroUpgradeDialog dialog, HeroUpgradeEventLog eventValue, ExecuteDoneEvent completed)
    {
        dialog._referencedHero = eventValue;
        dialog._referencedHero.AddRef();
        dialog.OnClose = completed;
        if (dialog.ValidateHero())
        {
            dialog._editedHero = (Hero)dialog._hero.Copy(GameTables.GameEngine);
            dialog.UpdateStatistics();
            dialog.RefreshAbilities();
            dialog.UnitNameEdit.Text = dialog._hero.HeroName;
            var level = dialog._hero.GetLevel();
            var levelText = AbilityRules.TranslateResourceString(GameText.LevelText);
            dialog.LevelLabel.Text = levelText + " " + IntToStr(level);
            if (dialog._referencedHero.NewLevel - dialog._referencedHero.OldLevel > 1)
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(ScenarioText.MadeLevelsFormat), [dialog._hero.HeroName, (dialog._referencedHero.NewLevel - dialog._referencedHero.OldLevel)]);
                dialog.HeaderLabel.Text = format + "!";
            }
            else
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(ScenarioText.MadeLevelFormat), [dialog._hero.HeroName]);
                dialog.HeaderLabel.Text = format + "!";
            }

            dialog.Window.Visible = true;
        }
    }

}
