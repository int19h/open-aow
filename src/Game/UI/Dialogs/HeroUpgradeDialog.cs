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
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// close callback, referenced hero, list sort orders, live hero, editable copy
public partial class HeroUpgradeDialog : DataModule
{
    public HeroUpgradeDialog(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label HeaderLbl;
    public ListBox AvailableAbilities;
    public VerticalScrollBar AvailableAbilitiesSB;
    public ListBox SelectedAbilities;
    public VerticalScrollBar SelectedAbilitiesSB;
    public ListBox AvailableAbilitiesCost;
    public Edit UnitName;
    public Panel TopPnl;
    public Label AttValue;
    public Label DamValue;
    public Label DefValue;
    public Label ResValue;
    public Label HitValue;
    public Label MovValue;
    public Label AttA;
    public Label DamA;
    public Label DefA;
    public Label ResA;
    public Label HitA;
    public Label MovA;
    public Label AttB;
    public Label DamB;
    public Label DefB;
    public Label ResB;
    public Label HitB;
    public Label MovB;
    public ListBox SelectedAbilitiesCost;
    public Label LevelLbl;
    public Label SkillPoints;
    private ExecuteDoneEvent _onClose;
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
        onClose = _onClose;
        hero = _referencedHero;
        _onClose = null;
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
        _hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(_referencedHero.UnitId);
        bool result = _hero != null;
        if (!result)
        {
            Dlg.Visible = false;
        }

        return result;
    }

    // AoWHeroUpgradeDlg.pas:257
    public void UpdateStatistics()
    {
        var inherentAttack = _hero.GetInherentAttack();
        AttB.Text = IntToStr(inherentAttack);
        var inherentDefense = _hero.GetInherentDefense();
        DefB.Text = IntToStr(inherentDefense);
        var inherentDamage = _hero.GetInherentDamage();
        DamB.Text = IntToStr(inherentDamage);
        var inherentResistance = _hero.GetInherentResistance();
        ResB.Text = IntToStr(inherentResistance);
        var hits = _hero.GetHits();
        HitB.Text = IntToStr(hits);
        var moves = _hero.GetMoves();
        MovB.Text = IntToStr(moves);
        AttA.Text = IntToStr(_editedHero.UnitAttack - _hero.UnitAttack);
        DefA.Text = IntToStr(_editedHero.UnitDefense - _hero.UnitDefense);
        DamA.Text = IntToStr(_editedHero.UnitDamage - _hero.UnitDamage);
        ResA.Text = IntToStr(_editedHero.UnitResistance - _hero.UnitResistance);
        HitA.Text = IntToStr(_editedHero.UnitHits - _hero.UnitHits);
        MovA.Text = IntToStr(_editedHero.UnitMoves - _hero.UnitMoves);
        var fEditedHeroInherentAttack = _editedHero.GetInherentAttack();
        AttValue.Text = IntToStr(fEditedHeroInherentAttack);
        var fEditedHeroInherentDefense = _editedHero.GetInherentDefense();
        DefValue.Text = IntToStr(fEditedHeroInherentDefense);
        var fEditedHeroInherentDamage = _editedHero.GetInherentDamage();
        DamValue.Text = IntToStr(fEditedHeroInherentDamage);
        var fEditedHeroInherentResistance = _editedHero.GetInherentResistance();
        ResValue.Text = IntToStr(fEditedHeroInherentResistance);
        var fEditedHeroHits = _editedHero.GetHits();
        HitValue.Text = IntToStr(fEditedHeroHits);
        var fEditedHeroMoves = _editedHero.GetMoves();
        MovValue.Text = IntToStr(fEditedHeroMoves);
        var skillPoints = _editedHero.GetSkillPoints();
        var fHeroSkillPoints = _hero.GetSkillPoints();
        SkillPoints.Text = IntToStr(skillPoints) + "/" + IntToStr(fHeroSkillPoints);
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

    // AoWHeroUpgradeDlg.pas:232
    public void SortAvailable()
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

    // AoWHeroUpgradeDlg.pas:170
    public void RefreshAbilities()
    {
        string name;
        string cost;
        Ability ability;
        AvailableAbilities.Items.Clear();
        AvailableAbilitiesCost.Items.Clear();
        for (int I = 1, last = AoWHSSet.Current.AbilityControl.GetCount() - 1; I <= last; I++)
        {
            ability = AoWHSSet.Current.AbilityControl.GetAbility(I);
            if (ability != null && ability.CanExpand(_editedHero) && ability.SelectionTypes.Contains(AbilitySelectionType.HeroUpgrade))
            {
                var expandName = ability.ExpandName(_editedHero);
                AvailableAbilities.Items.AddObject(expandName, IntToObject(I));
                var expandCost = ability.ExpandCost(_editedHero);
                AvailableAbilitiesCost.Items.Add(IntToStr(expandCost));
            }
        }

        SelectedAbilities.Items.Clear();
        SelectedAbilitiesCost.Items.Clear();
        for (int I = 1, last = _editedHero.GetAbCount() - 1; I <= last; I++)
        {
            if (_editedHero.GetInherentAbility(I))
            {
                ability = AoWHSSet.Current.AbilityControl.GetAbility(I);
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

                SelectedAbilitiesCost.Items.Add(cost);
                SelectedAbilities.Items.AddObject(name, IntToObject(I));
            }
        }

        var count = AvailableAbilities.Items.Count;
        AvailableAbilitiesSB.Max = count - 1;
        var itemsCount = SelectedAbilities.Items.Count;
        SelectedAbilitiesSB.Max = itemsCount - 1;
        SortAvailable();
        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:226
    public void SetAvailableIndex(int index)
    {
        AvailableAbilities.Selected = index;
        AvailableAbilitiesCost.Selected = index;
    }

    // AoWHeroUpgradeDlg.pas:220
    public void SetSelectedIndex(int index)
    {
        SelectedAbilities.Selected = index;
        SelectedAbilitiesCost.Selected = index;
    }

    // AoWHeroUpgradeDlg.pas:212
    public bool CanAddAbility(int abilityId)
    {
        bool result;
        if (!AoWHSSet.Current.AbilityControl.GetAbility(abilityId).CanExpand(_editedHero) || AoWHSSet.Current.AbilityControl.GetAbility(abilityId).ExpandCost(_editedHero) > _editedHero.GetSkillPoints())
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
        Dlg.Visible = false;
    }

    // AoWHeroUpgradeDlg.pas:308
    public void FaceFramePreDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect R;
        if (ValidateHero())
        {
            var image = sender;
            R = new Rect(TopPnl.WinLeft + image.WinLeft + 2, TopPnl.WinTop + image.WinTop, TopPnl.WinLeft + image.WinLeft + image.WinWidth - 2, TopPnl.WinTop + image.WinTop + image.WinHeight - 2);

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
        ListBox list;
        int index;
        int abilityId;
        DialogHosts.General.PlayButtonSound(0);
        list = AvailableAbilities;
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
        list = SelectedAbilities;
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
            if (!AoWHSMap.Current.Players.GetPlayers(_hero.Player).GetBusy())
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
                AoWHSMap.Current.TokenManager.GetTokenControl(_hero.Player).AddEvent(eventValue);
                eventValue.Release();
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

    // AoWHeroUpgradeDlg.pas:556
    public void SelectedAbilitiesMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
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

    // AoWHeroUpgradeDlg.pas:568
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowHeroUpgrade(HeroUpgradeDialog dialog, HeroUpgradeEventLog eventValue, ExecuteDoneEvent completed)
    {
        dialog._referencedHero = eventValue;
        dialog._referencedHero.AddRef();
        dialog._onClose = completed;
        if (dialog.ValidateHero())
        {
            dialog._editedHero = (Hero)dialog._hero.Copy(GameTables.GameEngine);
            dialog.UpdateStatistics();
            dialog.RefreshAbilities();
            dialog.UnitName.Text = dialog._hero.HeroName;
            var level = dialog._hero.GetLevel();
            var levelText = AbilityRules.TranslateResourceString(GameText.LevelText);
            dialog.LevelLbl.Text = levelText + " " + IntToStr(level);
            if (dialog._referencedHero.NewLevel - dialog._referencedHero.OldLevel > 1)
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(ScenarioText.MadeLevelsFormat), [dialog._hero.HeroName, (dialog._referencedHero.NewLevel - dialog._referencedHero.OldLevel)]);
                dialog.HeaderLbl.Text = format + "!";
            }
            else
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(ScenarioText.MadeLevelFormat), [dialog._hero.HeroName]);
                dialog.HeaderLbl.Text = format + "!";
            }

            dialog.Dlg.Visible = true;
        }
    }

}
