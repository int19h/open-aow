// Translated from AoWHeroUpgradeDlg.pas.
using Aow.Game.Combat;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWHeroUpgradeDlg.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// close callback, referenced hero, list sort orders, live hero, editable copy
public partial class HeroUpgradeDlg : DataModule
{
    public HeroUpgradeDlg(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWButton CancelBtn;
    public AoWFrame BCeltL;
    public AOWButton Done;
    public AOWLabel Title;
    public AOWButton Close;
    public AOWImage TitleIcon;
    public AOWImage CeltM;
    public AOWImage CeltL;
    public AOWImage CeltR;
    public AoWFrame TCeltL;
    public AOWImage TCelt;
    public AOWPanel UpgradePnl;
    public AOWLabel HeaderLbl;
    public AOWLabel SkillPointsLbl;
    public AOWListBox AvailableAbilities;
    public AOWVScrollBar AvailableAbilitiesSB;
    public AOWListBox SelectedAbilities;
    public AOWVScrollBar SelectedAbilitiesSB;
    public AOWButton SelectedAbilitiesSort;
    public AOWButton AvailableAbilityCostSort;
    public AOWListBox AvailableAbilitiesCost;
    public AOWButton AvailableAbilitiesSort;
    public AOWButton AddAbilityBtn;
    public AOWButton RemoveBtn;
    public AOWImage FaceFrame;
    public AOWEdit UnitName;
    public AOWEdit Alignment;
    public AOWPanel TopPnl;
    public AOWPanel StatisicsPnl;
    public AOWLabel AttValue;
    public AOWLabel AttLbl;
    public AOWPanel AttPnl;
    public AOWLabel DamValue;
    public AOWLabel DamLbl;
    public AOWPanel DamPnl;
    public AOWLabel DefValue;
    public AOWLabel DefLbl;
    public AOWPanel DefPnl;
    public AOWLabel ResValue;
    public AOWLabel ResLbl;
    public AOWPanel ResPnl;
    public AOWLabel HitValue;
    public AOWLabel HitLbl;
    public AOWPanel HitPnl;
    public AOWLabel MovValue;
    public AOWLabel MovLbl;
    public AOWPanel MovPnl;
    public AOWLabel AttI;
    public AOWLabel DamI;
    public AOWLabel DefI;
    public AOWLabel ResI;
    public AOWLabel HitI;
    public AOWLabel MovI;
    public AOWLabel AttA;
    public AOWLabel DamA;
    public AOWLabel DefA;
    public AOWLabel ResA;
    public AOWLabel HitA;
    public AOWLabel MovA;
    public AOWLabel AttP;
    public AOWLabel DamP;
    public AOWLabel DefP;
    public AOWLabel ResP;
    public AOWLabel HitP;
    public AOWLabel MovP;
    public AOWLabel AttB;
    public AOWLabel DamB;
    public AOWLabel DefB;
    public AOWLabel ResB;
    public AOWLabel HitB;
    public AOWLabel MovB;
    public AOWButton AttUp;
    public AOWButton AttDn;
    public AOWButton DamUp;
    public AOWButton DamDn;
    public AOWButton DefUp;
    public AOWButton DefDn;
    public AOWButton ResUp;
    public AOWButton ResDn;
    public AOWButton HitUp;
    public AOWButton HitDn;
    public AOWButton MovUp;
    public AOWButton MovDn;
    public AOWImage AttIc;
    public AOWImage DamIcn;
    public AOWImage DefIc;
    public AOWImage ResIc;
    public AOWImage HitIc;
    public AOWImage MovIc;
    public AOWButton SelectedAbilitiesCostSort;
    public AOWListBox SelectedAbilitiesCost;
    public AOWPanel InfoPnl;
    public AOWLabel LevelLbl;
    public AOWLabel SkillPoints;
    public IvTranslator Translator;
    public ExecuteDoneEvent FOnClose;
    public HeroUpgradeEventLog FReferencedHero;
    public int FSelectedSort;
    public int FAvailableSort;
    public Hero FHero;
    public Hero FEditedHero;

    // AoWHeroUpgradeDlg.pas:287
    public void ClearHero()
    {
        ExecuteDoneEvent OnClose;
        HeroUpgradeEventLog Hero;
        OnClose = FOnClose;
        Hero = FReferencedHero;
        FOnClose = null;
        FReferencedHero = null;
        if (OnClose != null)
        {
            OnClose(Hero);
        }

        if (Hero != null)
        {
            Hero.Release();
        }

        FEditedHero?.Dispose();
        FEditedHero = null;
    }

    // AoWHeroUpgradeDlg.pas:280
    public bool ValidateHero()
    {
        FHero = AoWHSMap.Current.FUnitControl.FindUnit<Hero>(FReferencedHero.FUnitID);
        bool Result = FHero != null;
        if (!Result)
        {
            Dlg.Visible = false;
        }

        return Result;
    }

    // AoWHeroUpgradeDlg.pas:257
    public void UpdateStatistics()
    {
        var inherentAttack = FHero.GetInherentAttack();
        AttB.Text = IntToStr(inherentAttack);
        var inherentDefense = FHero.GetInherentDefense();
        DefB.Text = IntToStr(inherentDefense);
        var inherentDamage = FHero.GetInherentDamage();
        DamB.Text = IntToStr(inherentDamage);
        var inherentResistance = FHero.GetInherentResistance();
        ResB.Text = IntToStr(inherentResistance);
        var hits = FHero.GetHits();
        HitB.Text = IntToStr(hits);
        var moves = FHero.GetMoves();
        MovB.Text = IntToStr(moves);
        AttA.Text = IntToStr(FEditedHero.FUnitAttack - FHero.FUnitAttack);
        DefA.Text = IntToStr(FEditedHero.FUnitDefense - FHero.FUnitDefense);
        DamA.Text = IntToStr(FEditedHero.FUnitDamage - FHero.FUnitDamage);
        ResA.Text = IntToStr(FEditedHero.FUnitResistance - FHero.FUnitResistance);
        HitA.Text = IntToStr(FEditedHero.FUnitHits - FHero.FUnitHits);
        MovA.Text = IntToStr(FEditedHero.FUnitMoves - FHero.FUnitMoves);
        var fEditedHeroInherentAttack = FEditedHero.GetInherentAttack();
        AttValue.Text = IntToStr(fEditedHeroInherentAttack);
        var fEditedHeroInherentDefense = FEditedHero.GetInherentDefense();
        DefValue.Text = IntToStr(fEditedHeroInherentDefense);
        var fEditedHeroInherentDamage = FEditedHero.GetInherentDamage();
        DamValue.Text = IntToStr(fEditedHeroInherentDamage);
        var fEditedHeroInherentResistance = FEditedHero.GetInherentResistance();
        ResValue.Text = IntToStr(fEditedHeroInherentResistance);
        var fEditedHeroHits = FEditedHero.GetHits();
        HitValue.Text = IntToStr(fEditedHeroHits);
        var fEditedHeroMoves = FEditedHero.GetMoves();
        MovValue.Text = IntToStr(fEditedHeroMoves);
        var skillPoints = FEditedHero.GetSkillPoints();
        var fHeroSkillPoints = FHero.GetSkillPoints();
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
        switch (FSelectedSort)
        {
            case 0:
                Engine.Nodes.StringSorting.SortStringsAscending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                break;
            case 1:
                Engine.Nodes.StringSorting.SortStringsDescending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                break;
            case 2:
                Engine.Nodes.StringSorting.SortStringsNumericalAscending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                break;
            case 3:
                Engine.Nodes.StringSorting.SortStringsNumericalDescending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                break;
        }
    }

    // AoWHeroUpgradeDlg.pas:232
    public void SortAvailable()
    {
        switch (FAvailableSort)
        {
            case 0:
                Engine.Nodes.StringSorting.SortStringsAscending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                break;
            case 1:
                Engine.Nodes.StringSorting.SortStringsDescending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                break;
            case 2:
                Engine.Nodes.StringSorting.SortStringsNumericalAscending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                break;
            case 3:
                Engine.Nodes.StringSorting.SortStringsNumericalDescending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                break;
        }
    }

    // AoWHeroUpgradeDlg.pas:170
    public void RefreshAbilities()
    {
        string Name;
        string Cost;
        Ability Ability;
        AvailableAbilities.Items.Clear();
        AvailableAbilitiesCost.Items.Clear();
        for (int I = 1, last = AoWHSSet.Current.FAbilityControl.GetCount() - 1; I <= last; I++)
        {
            Ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
            if (Ability != null && Ability.CanExpand(FEditedHero) && Ability.FSelectionTypes.Contains(AbilitySelectionType.HeroUpgrade))
            {
                var expandName = Ability.ExpandName(FEditedHero);
                AvailableAbilities.Items.AddObject(expandName, IntToObject(I));
                var expandCost = Ability.ExpandCost(FEditedHero);
                AvailableAbilitiesCost.Items.Add(IntToStr(expandCost));
            }
        }

        SelectedAbilities.Items.Clear();
        SelectedAbilitiesCost.Items.Clear();
        for (int I = 1, last = FEditedHero.GetAbCount() - 1; I <= last; I++)
        {
            if (FEditedHero.GetInherentAbility(I))
            {
                Ability = AoWHSSet.Current.FAbilityControl.GetAbility(I);
                Name = Ability.GetName(FEditedHero);
                if (FHero.GetAbEnabled(I) && FHero.GetInherentAbilityLevel(I) == FEditedHero.GetInherentAbilityLevel(I))
                {
                    Name = Name + " (" + AbilityRules.TranslateRStr(GameText.FixedRStr) + ")";
                    Cost = "-";
                }
                else
                {
                    Cost = IntToStr(Ability.GetSkillPoints(FEditedHero) - Ability.GetSkillPoints(FHero));
                }

                SelectedAbilitiesCost.Items.Add(Cost);
                SelectedAbilities.Items.AddObject(Name, IntToObject(I));
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
    public void SetAvailableIndex(int Index)
    {
        AvailableAbilities.Selected = Index;
        AvailableAbilitiesCost.Selected = Index;
    }

    // AoWHeroUpgradeDlg.pas:220
    public void SetSelectedIndex(int Index)
    {
        SelectedAbilities.Selected = Index;
        SelectedAbilitiesCost.Selected = Index;
    }

    // AoWHeroUpgradeDlg.pas:212
    public bool CanAddAbility(int AbilityID)
    {
        bool Result;
        if (!AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).CanExpand(FEditedHero) || AoWHSSet.Current.FAbilityControl.GetAbility(AbilityID).ExpandCost(FEditedHero) > FEditedHero.GetSkillPoints())
        {
            Result = false;
        }
        else
        {
            Result = true;
        }

        return Result;
    }

    // AoWHeroUpgradeDlg.pas:302
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWHeroUpgradeDlg.pas:308
    public void FaceFramePreDraw(AoWComponent Sender, SurfaceDesc SurfDesc)
    {
        Rect R;
        if (ValidateHero())
        {
            var image = Sender;
            R = new Rect(TopPnl.WinLeft + image.WinLeft + 2, TopPnl.WinTop + image.WinTop, TopPnl.WinLeft + image.WinLeft + image.WinWidth - 2, TopPnl.WinTop + image.WinTop + image.WinHeight - 2);

            FHero.GetFace().ShowStretched(R, SurfDesc);
        }
    }

    // AoWHeroUpgradeDlg.pas:323
    public void AttUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitAttack((sbyte)(FEditedHero.FUnitAttack + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:330
    public void AttDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitAttack != FHero.FUnitAttack)
        {
            FEditedHero.SetUnitAttack((sbyte)(FEditedHero.FUnitAttack - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:338
    public void DefUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitDefense((sbyte)(FEditedHero.FUnitDefense + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:345
    public void DefDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitDefense != FHero.FUnitDefense)
        {
            FEditedHero.SetUnitDefense((sbyte)(FEditedHero.FUnitDefense - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:353
    public void DamUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitDamage((sbyte)(FEditedHero.FUnitDamage + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:360
    public void DamDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitDamage != FHero.FUnitDamage)
        {
            FEditedHero.SetUnitDamage((sbyte)(FEditedHero.FUnitDamage - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:368
    public void ResUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitResistance((sbyte)(FEditedHero.FUnitResistance + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:375
    public void ResDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitResistance != FHero.FUnitResistance)
        {
            FEditedHero.SetUnitResistance((sbyte)(FEditedHero.FUnitResistance - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:383
    public void HitUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitHits((sbyte)(FEditedHero.FUnitHits + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:390
    public void HitDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitHits != FHero.FUnitHits)
        {
            FEditedHero.SetUnitHits((sbyte)(FEditedHero.FUnitHits - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:398
    public void MovUpClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitMoves((sbyte)(FEditedHero.FUnitMoves + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:405
    public void MovDnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitMoves != FHero.FUnitMoves)
        {
            FEditedHero.SetUnitMoves((sbyte)(FEditedHero.FUnitMoves - 1));
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
        AOWListBox List;
        int Index;
        int AbilityID;
        DialogHosts.General.PlayButtonSound(0);
        List = AvailableAbilities;
        Index = List.Selected;
        if (Index != -1)
        {
            AbilityID = ObjectToInt(List.Items.Objects[Index]);
            if (CanAddAbility(AbilityID))
            {
                FEditedHero.ExpandAbility(AbilityID);
                RefreshAbilities();
                RefreshStatistics();
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:465
    public void RemoveBtnClick()
    {
        AOWListBox List;
        int Index;
        int AbilityID;
        DialogHosts.General.PlayButtonSound(0);
        List = SelectedAbilities;
        Index = List.Selected;
        if (Index != -1)
        {
            AbilityID = ObjectToInt(List.Items.Objects[Index]);
            if (FHero.GetInherentAbility(AbilityID))
            {
                if (FHero.GetInherentAbilityLevel(AbilityID) == FEditedHero.GetInherentAbilityLevel(AbilityID))
                {
                    return;
                }
            }

            FEditedHero.RemoveAbility(AbilityID);
            RefreshAbilities();
            RefreshStatistics();
        }
    }

    // AoWHeroUpgradeDlg.pas:485
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

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:493
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

        SortSelected();
    }

    // AoWHeroUpgradeDlg.pas:501
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

        SortAvailable();
    }

    // AoWHeroUpgradeDlg.pas:509
    public void AvailableAbilityCostSortClick()
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

        SortAvailable();
    }

    // AoWHeroUpgradeDlg.pas:517
    public void DoneClick()
    {
        HeroUpgradeTE Event;
        if (ValidateHero())
        {
            if (!AoWHSMap.Current.FPlayers.GetPlayers(FHero.FPlayer).GetBusy())
            {
                Event = new HeroUpgradeTE(null);
                Event.Setup(FHero);
                Event.FAttack = (sbyte)(FEditedHero.FUnitAttack - FHero.FUnitAttack);
                Event.FDefense = (sbyte)(FEditedHero.FUnitDefense - FHero.FUnitDefense);
                Event.FDamage = (sbyte)(FEditedHero.FUnitDamage - FHero.FUnitDamage);
                Event.FResistance = (sbyte)(FEditedHero.FUnitResistance - FHero.FUnitResistance);
                Event.FHits = (sbyte)(FEditedHero.FUnitHits - FHero.FUnitHits);
                Event.FMoves = (sbyte)(FEditedHero.FUnitMoves - FHero.FUnitMoves);
                Event.FAbilities.AssignAb(FEditedHero);
                AoWHSMap.Current.FTokenManager.GetTokenControl(FHero.FPlayer).AddEvent(Event);
                Event.Release();
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

    // AoWHeroUpgradeDlg.pas:556
    public void SelectedAbilitiesMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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

    // AoWHeroUpgradeDlg.pas:568
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowHeroUpgrade(HeroUpgradeDlg Dialog, HeroUpgradeEventLog Event, ExecuteDoneEvent Completed)
    {
        Dialog.FReferencedHero = Event;
        Dialog.FReferencedHero.AddRef();
        Dialog.FOnClose = Completed;
        if (Dialog.ValidateHero())
        {
            Dialog.FEditedHero = (Hero)Dialog.FHero.Copy(GameTables.AoWEngine);
            Dialog.UpdateStatistics();
            Dialog.RefreshAbilities();
            Dialog.UnitName.Text = Dialog.FHero.FHeroName;
            var level = Dialog.FHero.GetLevel();
            var levelText = AbilityRules.TranslateRStr(GameText.LevelRStr);
            Dialog.LevelLbl.Text = levelText + " " + IntToStr(level);
            if (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel > 1)
            {
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(ScenarioText.XMadeYLevelsRStr), [Dialog.FHero.FHeroName, (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel)]);
                Dialog.HeaderLbl.Text = format + "!";
            }
            else
            {
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(ScenarioText.XMadeALevelRStr), [Dialog.FHero.FHeroName]);
                Dialog.HeaderLbl.Text = format + "!";
            }

            Dialog.Dlg.Visible = true;
        }
    }

}
