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
public partial class THeroUpgradeDlg : TDataModule
{
    public THeroUpgradeDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWButton CancelBtn;
    public TAoWFrame BCeltL;
    public TAOWButton Done;
    public TAOWLabel Title;
    public TAOWButton Close;
    public TAOWImage TitleIcon;
    public TAOWImage CeltM;
    public TAOWImage CeltL;
    public TAOWImage CeltR;
    public TAoWFrame TCeltL;
    public TAOWImage TCelt;
    public TAOWPanel UpgradePnl;
    public TAOWLabel HeaderLbl;
    public TAOWLabel SkillPointsLbl;
    public TAOWListBox AvailableAbilities;
    public TAOWVScrollBar AvailableAbilitiesSB;
    public TAOWListBox SelectedAbilities;
    public TAOWVScrollBar SelectedAbilitiesSB;
    public TAOWButton SelectedAbilitiesSort;
    public TAOWButton AvailableAbilityCostSort;
    public TAOWListBox AvailableAbilitiesCost;
    public TAOWButton AvailableAbilitiesSort;
    public TAOWButton AddAbilityBtn;
    public TAOWButton RemoveBtn;
    public TAOWImage FaceFrame;
    public TAOWEdit UnitName;
    public TAOWEdit Alignment;
    public TAOWPanel TopPnl;
    public TAOWPanel StatisicsPnl;
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
    public TAOWButton SelectedAbilitiesCostSort;
    public TAOWListBox SelectedAbilitiesCost;
    public TAOWPanel InfoPnl;
    public TAOWLabel LevelLbl;
    public TAOWLabel SkillPoints;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FOnClose;
    public THeroUpgradeEventLog FReferencedHero;
    public int FSelectedSort;
    public int FAvailableSort;
    public THero FHero;
    public THero FEditedHero;

    // AoWHeroUpgradeDlg.pas:287
    public void ClearHero()
    {
        TExecuteDoneEvent OnClose;
        THeroUpgradeEventLog Hero;
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
        FHero = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FUnitControl.FindUnit<THero>(FReferencedHero.FUnitID);
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
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                break;
            case 1:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsDescending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                break;
            case 2:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalAscending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                break;
            case 3:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalDescending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                break;
        }
    }

    // AoWHeroUpgradeDlg.pas:232
    public void SortAvailable()
    {
        switch (FAvailableSort)
        {
            case 0:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                break;
            case 1:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsDescending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                break;
            case 2:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalAscending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                break;
            case 3:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalDescending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                break;
        }
    }

    // AoWHeroUpgradeDlg.pas:170
    public void RefreshAbilities()
    {
        string Name;
        string Cost;
        TAbility Ability;
        AvailableAbilities.Items.Clear();
        AvailableAbilitiesCost.Items.Clear();
        for (int I = 1, last = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetCount() - 1; I <= last; I++)
        {
            Ability = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(I);
            if (Ability != null && Ability.CanExpand(FEditedHero) && Ability.FSelectionTypes.Contains(TAbilitySelectionType.astHeroUpgrade))
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
                Ability = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(I);
                Name = Ability.GetName(FEditedHero);
                if (FHero.GetAbEnabled(I) && FHero.GetInherentAbilityLevel(I) == FEditedHero.GetInherentAbilityLevel(I))
                {
                    Name = Name + " (" + Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.FixedRStr) + ")";
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
        if (!Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(AbilityID).CanExpand(FEditedHero) || Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(AbilityID).ExpandCost(FEditedHero) > FEditedHero.GetSkillPoints())
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWHeroUpgradeDlg.pas:308
    public void FaceFramePreDraw(TAoWComponent Sender, TSurfaceDesc SurfDesc)
    {
        TRect R;
        if (ValidateHero())
        {
            var image = Sender;
            R = Rect(TopPnl.WinLeft + image.WinLeft + 2, TopPnl.WinTop + image.WinTop, TopPnl.WinLeft + image.WinLeft + image.WinWidth - 2, TopPnl.WinTop + image.WinTop + image.WinHeight - 2);

            FHero.GetFace().ShowStretched(R, SurfDesc);
        }
    }

    // AoWHeroUpgradeDlg.pas:323
    public void AttUpClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitAttack((sbyte)(FEditedHero.FUnitAttack + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:330
    public void AttDnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitAttack != FHero.FUnitAttack)
        {
            FEditedHero.SetUnitAttack((sbyte)(FEditedHero.FUnitAttack - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:338
    public void DefUpClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitDefense((sbyte)(FEditedHero.FUnitDefense + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:345
    public void DefDnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitDefense != FHero.FUnitDefense)
        {
            FEditedHero.SetUnitDefense((sbyte)(FEditedHero.FUnitDefense - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:353
    public void DamUpClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitDamage((sbyte)(FEditedHero.FUnitDamage + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:360
    public void DamDnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitDamage != FHero.FUnitDamage)
        {
            FEditedHero.SetUnitDamage((sbyte)(FEditedHero.FUnitDamage - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:368
    public void ResUpClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitResistance((sbyte)(FEditedHero.FUnitResistance + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:375
    public void ResDnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitResistance != FHero.FUnitResistance)
        {
            FEditedHero.SetUnitResistance((sbyte)(FEditedHero.FUnitResistance - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:383
    public void HitUpClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitHits((sbyte)(FEditedHero.FUnitHits + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:390
    public void HitDnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        if (FEditedHero.FUnitHits != FHero.FUnitHits)
        {
            FEditedHero.SetUnitHits((sbyte)(FEditedHero.FUnitHits - 1));
        }

        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:398
    public void MovUpClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        FEditedHero.SetUnitMoves((sbyte)(FEditedHero.FUnitMoves + 1));
        RefreshStatistics();
    }

    // AoWHeroUpgradeDlg.pas:405
    public void MovDnClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        TAOWListBox List;
        int Index;
        int AbilityID;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        TAOWListBox List;
        int Index;
        int AbilityID;
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        THeroUpgradeTE Event;
        if (ValidateHero())
        {
            if (!Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(FHero.FPlayer).GetBusy())
            {
                Event = new THeroUpgradeTE(null);
                Event.Setup(FHero);
                Event.FAttack = (sbyte)(FEditedHero.FUnitAttack - FHero.FUnitAttack);
                Event.FDefense = (sbyte)(FEditedHero.FUnitDefense - FHero.FUnitDefense);
                Event.FDamage = (sbyte)(FEditedHero.FUnitDamage - FHero.FUnitDamage);
                Event.FResistance = (sbyte)(FEditedHero.FUnitResistance - FHero.FUnitResistance);
                Event.FHits = (sbyte)(FEditedHero.FUnitHits - FHero.FUnitHits);
                Event.FMoves = (sbyte)(FEditedHero.FUnitMoves - FHero.FUnitMoves);
                Event.FAbilities.AssignAb(FEditedHero);
                Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FTokenManager.GetTokenControl(FHero.FPlayer).AddEvent(Event);
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
    public void AvailableAbilitiesMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility Ability;
        if (Button == TMouseButton.mbRight && AvailableAbilities.Selected != -1)
        {
            var objectValue = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
            Ability = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (Ability != null)
            {
                var name = Ability.GetName(null);
                Aow.Game.Ui.Dialogs.TInfoBanner.SetInfoBanner(Aow.Game.Ui.Dialogs.DialogHosts.InfoBanner, name, Ability.FDescription);
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:556
    public void SelectedAbilitiesMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility Ability;
        if (Button == TMouseButton.mbRight && SelectedAbilities.Selected != -1)
        {
            var objectValue = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
            Ability = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (Ability != null)
            {
                var name = Ability.GetName(null);
                Aow.Game.Ui.Dialogs.TInfoBanner.SetInfoBanner(Aow.Game.Ui.Dialogs.DialogHosts.InfoBanner, name, Ability.FDescription);
            }
        }
    }

    // AoWHeroUpgradeDlg.pas:568
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    internal static void ShowHeroUpgrade(THeroUpgradeDlg Dialog, THeroUpgradeEventLog Event, TExecuteDoneEvent Completed)
    {
        Dialog.FReferencedHero = Event;
        Dialog.FReferencedHero.AddRef();
        Dialog.FOnClose = Completed;
        if (Dialog.ValidateHero())
        {
            Dialog.FEditedHero = (THero)Dialog.FHero.Copy(Aow.Game.Magic.Abilities.GameTables.AoWEngine);
            Dialog.UpdateStatistics();
            Dialog.RefreshAbilities();
            Dialog.UnitName.Text = Dialog.FHero.FHeroName;
            var level = Dialog.FHero.GetLevel();
            var levelText = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LevelRStr);
            Dialog.LevelLbl.Text = levelText + " " + IntToStr(level);
            if (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel > 1)
            {
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.ScenarioText.XMadeYLevelsRStr), [Dialog.FHero.FHeroName, (Dialog.FReferencedHero.FNewLevel - Dialog.FReferencedHero.FOldLevel)]);
                Dialog.HeaderLbl.Text = format + "!";
            }
            else
            {
                var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Ui.Dialogs.ScenarioText.XMadeALevelRStr), [Dialog.FHero.FHeroName]);
                Dialog.HeaderLbl.Text = format + "!";
            }

            Dialog.Dlg.Visible = true;
        }
    }

}
