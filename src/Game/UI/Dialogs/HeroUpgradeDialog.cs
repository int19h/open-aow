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
    public Button CancelBtn;
    public Frame BCeltL;
    public Button Done;
    public Label Title;
    public Button Close;
    public Image TitleIcon;
    public Image CeltM;
    public Image CeltL;
    public Image CeltR;
    public Frame TCeltL;
    public Image TCelt;
    public Panel UpgradePnl;
    public Label HeaderLbl;
    public Label SkillPointsLbl;
    public ListBox AvailableAbilities;
    public VerticalScrollBar AvailableAbilitiesSB;
    public ListBox SelectedAbilities;
    public VerticalScrollBar SelectedAbilitiesSB;
    public Button SelectedAbilitiesSort;
    public Button AvailableAbilityCostSort;
    public ListBox AvailableAbilitiesCost;
    public Button AvailableAbilitiesSort;
    public Button AddAbilityBtn;
    public Button RemoveBtn;
    public Image FaceFrame;
    public Edit UnitName;
    public Edit Alignment;
    public Panel TopPnl;
    public Panel StatisicsPnl;
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
    public Button SelectedAbilitiesCostSort;
    public ListBox SelectedAbilitiesCost;
    public Panel InfoPnl;
    public Label LevelLbl;
    public Label SkillPoints;
    public IvTranslator Translator;
    public ExecuteDoneEvent OnClose;
    public HeroUpgradeEventLog ReferencedHero;
    public int SelectedSort;
    public int AvailableSort;
    public Hero Hero;
    public Hero EditedHero;

    // AoWHeroUpgradeDlg.pas:287
    public void ClearHero()
    {
        ExecuteDoneEvent onClose;
        HeroUpgradeEventLog hero;
        onClose = OnClose;
        hero = ReferencedHero;
        OnClose = null;
        ReferencedHero = null;
        if (onClose != null)
        {
            onClose(hero);
        }

        if (hero != null)
        {
            hero.Release();
        }

        EditedHero?.Dispose();
        EditedHero = null;
    }

    // AoWHeroUpgradeDlg.pas:280
    public bool ValidateHero()
    {
        Hero = AoWHSMap.Current.UnitControl.FindUnit<Hero>(ReferencedHero.UnitId);
        bool result = Hero != null;
        if (!result)
        {
            Dlg.Visible = false;
        }

        return result;
    }

    // AoWHeroUpgradeDlg.pas:257
    public void UpdateStatistics()
    {
        var inherentAttack = Hero.GetInherentAttack();
        AttB.Text = IntToStr(inherentAttack);
        var inherentDefense = Hero.GetInherentDefense();
        DefB.Text = IntToStr(inherentDefense);
        var inherentDamage = Hero.GetInherentDamage();
        DamB.Text = IntToStr(inherentDamage);
        var inherentResistance = Hero.GetInherentResistance();
        ResB.Text = IntToStr(inherentResistance);
        var hits = Hero.GetHits();
        HitB.Text = IntToStr(hits);
        var moves = Hero.GetMoves();
        MovB.Text = IntToStr(moves);
        AttA.Text = IntToStr(EditedHero.UnitAttack - Hero.UnitAttack);
        DefA.Text = IntToStr(EditedHero.UnitDefense - Hero.UnitDefense);
        DamA.Text = IntToStr(EditedHero.UnitDamage - Hero.UnitDamage);
        ResA.Text = IntToStr(EditedHero.UnitResistance - Hero.UnitResistance);
        HitA.Text = IntToStr(EditedHero.UnitHits - Hero.UnitHits);
        MovA.Text = IntToStr(EditedHero.UnitMoves - Hero.UnitMoves);
        var fEditedHeroInherentAttack = EditedHero.GetInherentAttack();
        AttValue.Text = IntToStr(fEditedHeroInherentAttack);
        var fEditedHeroInherentDefense = EditedHero.GetInherentDefense();
        DefValue.Text = IntToStr(fEditedHeroInherentDefense);
        var fEditedHeroInherentDamage = EditedHero.GetInherentDamage();
        DamValue.Text = IntToStr(fEditedHeroInherentDamage);
        var fEditedHeroInherentResistance = EditedHero.GetInherentResistance();
        ResValue.Text = IntToStr(fEditedHeroInherentResistance);
        var fEditedHeroHits = EditedHero.GetHits();
        HitValue.Text = IntToStr(fEditedHeroHits);
        var fEditedHeroMoves = EditedHero.GetMoves();
        MovValue.Text = IntToStr(fEditedHeroMoves);
        var skillPoints = EditedHero.GetSkillPoints();
        var fHeroSkillPoints = Hero.GetSkillPoints();
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
        switch (SelectedSort)
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
        switch (AvailableSort)
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
            if (ability != null && ability.CanExpand(EditedHero) && ability.SelectionTypes.Contains(AbilitySelectionType.HeroUpgrade))
            {
                var expandName = ability.ExpandName(EditedHero);
                AvailableAbilities.Items.AddObject(expandName, IntToObject(I));
                var expandCost = ability.ExpandCost(EditedHero);
                AvailableAbilitiesCost.Items.Add(IntToStr(expandCost));
            }
        }

        SelectedAbilities.Items.Clear();
        SelectedAbilitiesCost.Items.Clear();
        for (int I = 1, last = EditedHero.GetAbCount() - 1; I <= last; I++)
        {
            if (EditedHero.GetInherentAbility(I))
            {
                ability = AoWHSSet.Current.AbilityControl.GetAbility(I);
                name = ability.GetName(EditedHero);
                if (Hero.GetAbEnabled(I) && Hero.GetInherentAbilityLevel(I) == EditedHero.GetInherentAbilityLevel(I))
                {
                    name = name + " (" + AbilityRules.TranslateResourceString(GameText.s_fixedText) + ")";
                    cost = "-";
                }
                else
                {
                    cost = IntToStr(ability.GetSkillPoints(EditedHero) - ability.GetSkillPoints(Hero));
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
        if (!AoWHSSet.Current.AbilityControl.GetAbility(abilityId).CanExpand(EditedHero) || AoWHSSet.Current.AbilityControl.GetAbility(abilityId).ExpandCost(EditedHero) > EditedHero.GetSkillPoints())
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

            Hero.GetFace().ShowStretched(R, lockedSurface);
        }
    }

    // AoWHeroUpgradeDlg.pas:323
    public void AttUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        EditedHero.SetUnitAttack((sbyte)(EditedHero.UnitAttack + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:330
    public void AttDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (EditedHero.UnitAttack != Hero.UnitAttack)
        {
            EditedHero.SetUnitAttack((sbyte)(EditedHero.UnitAttack - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:338
    public void DefUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        EditedHero.SetUnitDefense((sbyte)(EditedHero.UnitDefense + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:345
    public void DefDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (EditedHero.UnitDefense != Hero.UnitDefense)
        {
            EditedHero.SetUnitDefense((sbyte)(EditedHero.UnitDefense - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:353
    public void DamUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        EditedHero.SetUnitDamage((sbyte)(EditedHero.UnitDamage + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:360
    public void DamDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (EditedHero.UnitDamage != Hero.UnitDamage)
        {
            EditedHero.SetUnitDamage((sbyte)(EditedHero.UnitDamage - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:368
    public void ResUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        EditedHero.SetUnitResistance((sbyte)(EditedHero.UnitResistance + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:375
    public void ResDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (EditedHero.UnitResistance != Hero.UnitResistance)
        {
            EditedHero.SetUnitResistance((sbyte)(EditedHero.UnitResistance - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:383
    public void HitUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        EditedHero.SetUnitHits((sbyte)(EditedHero.UnitHits + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:390
    public void HitDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (EditedHero.UnitHits != Hero.UnitHits)
        {
            EditedHero.SetUnitHits((sbyte)(EditedHero.UnitHits - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:398
    public void MovUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        EditedHero.SetUnitMoves((sbyte)(EditedHero.UnitMoves + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:405
    public void MovDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (EditedHero.UnitMoves != Hero.UnitMoves)
        {
            EditedHero.SetUnitMoves((sbyte)(EditedHero.UnitMoves - 1));
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
                EditedHero.ExpandAbility(abilityId);
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
            if (Hero.GetInherentAbility(abilityId))
            {
                if (Hero.GetInherentAbilityLevel(abilityId) == EditedHero.GetInherentAbilityLevel(abilityId))
                {
                    return;
                }
            }

            EditedHero.RemoveAbility(abilityId);
            RefreshAbilities();
            RefreshStatistics();
        }
    }

    // AoWHeroUpgradeDlg.pas:485
    public void SelectedAbilitiesSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (SelectedSort == 0)
        {
            SelectedSort = 1;
        }
        else
        {
            SelectedSort = 0;
        }

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:493
    public void SelectedAbilitiesCostSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (SelectedSort == 2)
        {
            SelectedSort = 3;
        }
        else
        {
            SelectedSort = 2;
        }

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:501
    public void AvailableAbilitiesSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (AvailableSort == 0)
        {
            AvailableSort = 1;
        }
        else
        {
            AvailableSort = 0;
        }

        SortAvailable();
    }

    // AoWHeroUpgradeDlg.pas:509
    public void AvailableAbilityCostSortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (AvailableSort == 2)
        {
            AvailableSort = 3;
        }
        else
        {
            AvailableSort = 2;
        }

        SortAvailable();
    }

    // AoWHeroUpgradeDlg.pas:517
    public void DoneClick()
    {
        HeroUpgradeTokenEvent eventValue;
        if (ValidateHero())
        {
            if (!AoWHSMap.Current.Players.GetPlayers(Hero.Player).GetBusy())
            {
                eventValue = new HeroUpgradeTokenEvent(null);
                eventValue.Setup(Hero);
                eventValue.Attack = (sbyte)(EditedHero.UnitAttack - Hero.UnitAttack);
                eventValue.Defense = (sbyte)(EditedHero.UnitDefense - Hero.UnitDefense);
                eventValue.Damage = (sbyte)(EditedHero.UnitDamage - Hero.UnitDamage);
                eventValue.Resistance = (sbyte)(EditedHero.UnitResistance - Hero.UnitResistance);
                eventValue.Hits = (sbyte)(EditedHero.UnitHits - Hero.UnitHits);
                eventValue.Moves = (sbyte)(EditedHero.UnitMoves - Hero.UnitMoves);
                eventValue.Abilities.AssignAbility(EditedHero);
                AoWHSMap.Current.TokenManager.GetTokenControl(Hero.Player).AddEvent(eventValue);
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
        dialog.ReferencedHero = eventValue;
        dialog.ReferencedHero.AddRef();
        dialog.OnClose = completed;
        if (dialog.ValidateHero())
        {
            dialog.EditedHero = (Hero)dialog.Hero.Copy(GameTables.GameEngine);
            dialog.UpdateStatistics();
            dialog.RefreshAbilities();
            dialog.UnitName.Text = dialog.Hero.HeroName;
            var level = dialog.Hero.GetLevel();
            var levelText = AbilityRules.TranslateResourceString(GameText.s_levelText);
            dialog.LevelLbl.Text = levelText + " " + IntToStr(level);
            if (dialog.ReferencedHero.NewLevel - dialog.ReferencedHero.OldLevel > 1)
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(ScenarioText.s_madeLevelsFormat), [dialog.Hero.HeroName, (dialog.ReferencedHero.NewLevel - dialog.ReferencedHero.OldLevel)]);
                dialog.HeaderLbl.Text = format + "!";
            }
            else
            {
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(ScenarioText.s_madeLevelFormat), [dialog.Hero.HeroName]);
                dialog.HeaderLbl.Text = format + "!";
            }

            dialog.Dlg.Visible = true;
        }
    }

}
