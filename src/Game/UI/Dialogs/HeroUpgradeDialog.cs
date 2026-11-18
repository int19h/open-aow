#nullable disable
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

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// close callback, referenced hero, list sort orders, live hero, editable copy
public partial class HeroUpgradeDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label HeaderLabel { get; private set; }
    public ListBox AvailableAbilitiesList { get; private set; }
    public VerticalScrollBar AvailableAbilitiesScrollBar { get; private set; }
    public ListBox SelectedAbilitiesList { get; private set; }
    public VerticalScrollBar SelectedAbilitiesScrollBar { get; private set; }
    public ListBox AvailableAbilitiesCostList { get; private set; }
    public Edit UnitNameEdit { get; private set; }
    public Panel TopPanel { get; private set; }
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
    public ListBox SelectedAbilitiesCostList { get; private set; }
    public Label LevelLabel { get; private set; }
    public Label SkillPointsLabel { get; private set; }

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
        onClose?.Invoke(hero);

        _ = (hero?.Release());

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
        sbyte inherentAttack = _hero.GetInherentAttack();
        AttackBaseLabel.Text = IntToStr(inherentAttack);
        sbyte inherentDefense = _hero.GetInherentDefense();
        DefenseBaseLabel.Text = IntToStr(inherentDefense);
        sbyte inherentDamage = _hero.GetInherentDamage();
        DamageBaseLabel.Text = IntToStr(inherentDamage);
        sbyte inherentResistance = _hero.GetInherentResistance();
        ResistanceBaseLabel.Text = IntToStr(inherentResistance);
        sbyte hits = _hero.GetHits();
        HitsBaseLabel.Text = IntToStr(hits);
        sbyte moves = _hero.GetMoves();
        MovesBaseLabel.Text = IntToStr(moves);
        AttackAddedLabel.Text = IntToStr(_editedHero.UnitAttack - _hero.UnitAttack);
        DefenseAddedLabel.Text = IntToStr(_editedHero.UnitDefense - _hero.UnitDefense);
        DamageAddedLabel.Text = IntToStr(_editedHero.UnitDamage - _hero.UnitDamage);
        ResistanceAddedLabel.Text = IntToStr(_editedHero.UnitResistance - _hero.UnitResistance);
        HitsAddedLabel.Text = IntToStr(_editedHero.UnitHits - _hero.UnitHits);
        MovesAddedLabel.Text = IntToStr(_editedHero.UnitMoves - _hero.UnitMoves);
        sbyte fEditedHeroInherentAttack = _editedHero.GetInherentAttack();
        AttackValueLabel.Text = IntToStr(fEditedHeroInherentAttack);
        sbyte fEditedHeroInherentDefense = _editedHero.GetInherentDefense();
        DefenseValueLabel.Text = IntToStr(fEditedHeroInherentDefense);
        sbyte fEditedHeroInherentDamage = _editedHero.GetInherentDamage();
        DamageValueLabel.Text = IntToStr(fEditedHeroInherentDamage);
        sbyte fEditedHeroInherentResistance = _editedHero.GetInherentResistance();
        ResistanceValueLabel.Text = IntToStr(fEditedHeroInherentResistance);
        sbyte fEditedHeroHits = _editedHero.GetHits();
        HitsValueLabel.Text = IntToStr(fEditedHeroHits);
        sbyte fEditedHeroMoves = _editedHero.GetMoves();
        MovesValueLabel.Text = IntToStr(fEditedHeroMoves);
        int skillPoints = _editedHero.GetSkillPoints();
        int fHeroSkillPoints = _hero.GetSkillPoints();
        SkillPointsLabel.Text = IntToStr(skillPoints) + "/" + IntToStr(fHeroSkillPoints);
    }

    // AoWHeroUpgradeDlg.pas:252
    public void RefreshStatistics() => UpdateStatistics();

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
            default:
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
            default:
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
        for (int i = 1, last = GamePack.Current.AbilityControl.GetCount() - 1; i <= last; i++)
        {
            ability = GamePack.Current.AbilityControl.GetAbility(i);
            if (ability != null && ability.CanExpand(_editedHero) && ability.SelectionTypes.Contains(AbilitySelectionType.HeroUpgrade))
            {
                string expandName = ability.ExpandName(_editedHero);
                _ = AvailableAbilitiesList.Items.AddObject(expandName, IntToObject(i));
                int expandCost = ability.ExpandCost(_editedHero);
                _ = AvailableAbilitiesCostList.Items.Add(IntToStr(expandCost));
            }
        }

        SelectedAbilitiesList.Items.Clear();
        SelectedAbilitiesCostList.Items.Clear();
        for (int i = 1, last = _editedHero.GetOwnAbilityCount() - 1; i <= last; i++)
        {
            if (_editedHero.GetInherentAbility(i))
            {
                ability = GamePack.Current.AbilityControl.GetAbility(i);
                name = ability.GetName(_editedHero);
                if (_hero.GetOwnAbilityEnabled(i) && _hero.GetInherentAbilityLevel(i) == _editedHero.GetInherentAbilityLevel(i))
                {
                    name = name + " (" + UI.Engine.TranslateResourceString(GameText.FixedText) + ")";
                    cost = "-";
                }
                else
                {
                    cost = IntToStr(ability.GetSkillPoints(_editedHero) - ability.GetSkillPoints(_hero));
                }

                _ = SelectedAbilitiesCostList.Items.Add(cost);
                _ = SelectedAbilitiesList.Items.AddObject(name, IntToObject(i));
            }
        }

        int count = AvailableAbilitiesList.Items.Count;
        AvailableAbilitiesScrollBar.Max = count - 1;
        int itemsCount = SelectedAbilitiesList.Items.Count;
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
        bool result = GamePack.Current.AbilityControl.GetAbility(abilityId).CanExpand(_editedHero) && GamePack.Current.AbilityControl.GetAbility(abilityId).ExpandCost(_editedHero) <= _editedHero.GetSkillPoints();
        return result;
    }

    // AoWHeroUpgradeDlg.pas:302
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWHeroUpgradeDlg.pas:308
    public void FaceFramePreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect r;
        if (ValidateHero())
        {
            UIComponent image = sender;
            r = new Rect(TopPanel.AbsoluteLeft + image.WinLeft + 2, TopPanel.AbsoluteTop + image.WinTop, TopPanel.AbsoluteLeft + image.WinLeft + image.WinWidth - 2, TopPanel.AbsoluteTop + image.WinTop + image.WinHeight - 2);

            _hero.GetFace().ShowStretched(r, lockedSurface);
        }
    }

    // AoWHeroUpgradeDlg.pas:323
    public void AttUpClick()
    {
        UI.General.PlayButtonSound(0);
        _editedHero.SetUnitAttack((sbyte)(_editedHero.UnitAttack + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:330
    public void AttDnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_editedHero.UnitAttack != _hero.UnitAttack)
        {
            _editedHero.SetUnitAttack((sbyte)(_editedHero.UnitAttack - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:338
    public void DefUpClick()
    {
        UI.General.PlayButtonSound(0);
        _editedHero.SetUnitDefense((sbyte)(_editedHero.UnitDefense + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:345
    public void DefDnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_editedHero.UnitDefense != _hero.UnitDefense)
        {
            _editedHero.SetUnitDefense((sbyte)(_editedHero.UnitDefense - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:353
    public void DamUpClick()
    {
        UI.General.PlayButtonSound(0);
        _editedHero.SetUnitDamage((sbyte)(_editedHero.UnitDamage + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:360
    public void DamDnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_editedHero.UnitDamage != _hero.UnitDamage)
        {
            _editedHero.SetUnitDamage((sbyte)(_editedHero.UnitDamage - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:368
    public void ResUpClick()
    {
        UI.General.PlayButtonSound(0);
        _editedHero.SetUnitResistance((sbyte)(_editedHero.UnitResistance + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:375
    public void ResDnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_editedHero.UnitResistance != _hero.UnitResistance)
        {
            _editedHero.SetUnitResistance((sbyte)(_editedHero.UnitResistance - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:383
    public void HitUpClick()
    {
        UI.General.PlayButtonSound(0);
        _editedHero.SetUnitHits((sbyte)(_editedHero.UnitHits + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:390
    public void HitDnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_editedHero.UnitHits != _hero.UnitHits)
        {
            _editedHero.SetUnitHits((sbyte)(_editedHero.UnitHits - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:398
    public void MovUpClick()
    {
        UI.General.PlayButtonSound(0);
        _editedHero.SetUnitMoves((sbyte)(_editedHero.UnitMoves + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:405
    public void MovDnClick()
    {
        UI.General.PlayButtonSound(0);
        if (_editedHero.UnitMoves != _hero.UnitMoves)
        {
            _editedHero.SetUnitMoves((sbyte)(_editedHero.UnitMoves - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:413
    public void SelectedAbilitiesChange() => SetSelectedIndex(SelectedAbilitiesList.Selected);

    // AoWHeroUpgradeDlg.pas:418
    public void SelectedAbilitiesCostChange() => SetSelectedIndex(SelectedAbilitiesCostList.Selected);

    // AoWHeroUpgradeDlg.pas:423
    public void SelectedAbilitiesSBChange()
    {
        SelectedAbilitiesList.ListOffset = SelectedAbilitiesScrollBar.Position;
        SelectedAbilitiesCostList.ListOffset = SelectedAbilitiesScrollBar.Position;
    }

    // AoWHeroUpgradeDlg.pas:429
    public void AvailableAbilitiesChange() => SetAvailableIndex(AvailableAbilitiesList.Selected);

    // AoWHeroUpgradeDlg.pas:434
    public void AvailableAbilitiesCostChange() => SetAvailableIndex(AvailableAbilitiesCostList.Selected);

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
        UI.General.PlayButtonSound(0);
        list = AvailableAbilitiesList;
        index = list.Selected;
        if (index != -1)
        {
            abilityId = ObjectToInt(list.Items.Objects[index]);
            if (CanAddAbility(abilityId))
            {
                _ = _editedHero.ExpandAbility(abilityId);
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
        UI.General.PlayButtonSound(0);
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

            _ = _editedHero.RemoveAbility(abilityId);
            RefreshAbilities();
            RefreshStatistics();
        }
    }

    // AoWHeroUpgradeDlg.pas:485
    public void SelectedAbilitiesSortClick()
    {
        UI.General.PlayButtonSound(0);
        _selectedSort = _selectedSort == 0 ? 1 : 0;

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:493
    public void SelectedAbilitiesCostSortClick()
    {
        UI.General.PlayButtonSound(0);
        _selectedSort = _selectedSort == 2 ? 3 : 2;

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:501
    public void AvailableAbilitiesSortClick()
    {
        UI.General.PlayButtonSound(0);
        _availableSort = _availableSort == 0 ? 1 : 0;

        SortAvailable();
    }

    // AoWHeroUpgradeDlg.pas:509
    public void AvailableAbilityCostSortClick()
    {
        UI.General.PlayButtonSound(0);
        _availableSort = _availableSort == 2 ? 3 : 2;

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
                eventValue = new HeroUpgradeTokenEvent(null) { Holder = UI.Engine };
                eventValue.Setup(_hero);
                eventValue.Attack = (sbyte)(_editedHero.UnitAttack - _hero.UnitAttack);
                eventValue.Defense = (sbyte)(_editedHero.UnitDefense - _hero.UnitDefense);
                eventValue.Damage = (sbyte)(_editedHero.UnitDamage - _hero.UnitDamage);
                eventValue.Resistance = (sbyte)(_editedHero.UnitResistance - _hero.UnitResistance);
                eventValue.Hits = (sbyte)(_editedHero.UnitHits - _hero.UnitHits);
                eventValue.Moves = (sbyte)(_editedHero.UnitMoves - _hero.UnitMoves);
                eventValue.Abilities.AssignAbility(_editedHero);
                StrategicMap.Current.TokenManager.GetTokenControl(_hero.Player).AddEvent(eventValue);
                _ = eventValue.Release();
                Window.Visible = false;
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:539
    public void DlgHide() => ClearHero();

    // AoWHeroUpgradeDlg.pas:544
    public void AvailableAbilitiesMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (button == MouseButton.Right && AvailableAbilitiesList.Selected != -1)
        {
            object objectValue = AvailableAbilitiesList.Items.Objects[AvailableAbilitiesList.Selected];
            ability = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (ability != null)
            {
                InfoBanner.SetInfoBanner(caption: ability.GetName(null), banner: UI.InfoBanner, description: ability.Description);
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:556
    public void SelectedAbilitiesMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability ability;
        if (button == MouseButton.Right && SelectedAbilitiesList.Selected != -1)
        {
            object objectValue = SelectedAbilitiesList.Items.Objects[SelectedAbilitiesList.Selected];
            ability = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (ability != null)
            {
                InfoBanner.SetInfoBanner(caption: ability.GetName(null), banner: UI.InfoBanner, description: ability.Description);
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:568
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    internal static void ShowHeroUpgrade(HeroUpgradeDialog dialog, HeroUpgradeEventLog eventValue, ExecuteDoneEvent completed)
    {
        dialog._referencedHero = eventValue;
        _ = dialog._referencedHero.AddRef();
        dialog.OnClose = completed;
        if (dialog.ValidateHero())
        {
            dialog._editedHero = dialog._hero.Copy<Hero>(dialog.UI.Engine);
            dialog.UpdateStatistics();
            dialog.RefreshAbilities();
            dialog.UnitNameEdit.Text = dialog._hero.HeroName;
            sbyte level = dialog._hero.GetLevel();
            string levelText = dialog.UI.Engine.TranslateResourceString(GameText.LevelText);
            dialog.LevelLabel.Text = levelText + " " + IntToStr(level);
            if (dialog._referencedHero.NewLevel - dialog._referencedHero.OldLevel > 1)
            {
                string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(ScenarioText.MadeLevelsFormat), [dialog._hero.HeroName, (dialog._referencedHero.NewLevel - dialog._referencedHero.OldLevel)]);
                dialog.HeaderLabel.Text = format + "!";
            }
            else
            {
                string format = AbilityRules.Format(dialog.UI.Engine.TranslateResourceString(ScenarioText.MadeLevelFormat), [dialog._hero.HeroName]);
                dialog.HeaderLabel.Text = format + "!";
            }

            dialog.Window.Visible = true;
        }
    }

}
