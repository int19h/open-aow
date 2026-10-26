// Generated from AoWHeroUpgradeDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWInfoBanner;
using static Aow.Units.AoWInfoBanner.AoWInfoBanner;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWHeroUpgradeDlg
{
    // { THeroUpgradeDlg: size $1E0; $1C4 close callback, $1CC referenced hero,
//     $1D0/$1D4 list sort orders, $1D8 live hero, $1DC editable copy. VMT 004458C8. }
    public partial class THeroUpgradeDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton Done;
        // { +0050 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0054 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage CeltM;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage CeltL;
        // { +0064 }
        public Aow.Units.AoWImage.TAOWImage CeltR;
        // { +0068 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +006C }
        public Aow.Units.AoWImage.TAOWImage TCelt;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel UpgradePnl;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel HeaderLbl;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel SkillPointsLbl;
        // { +007C }
        public Aow.Units.AoWListBox.TAOWListBox AvailableAbilities;
        // { +0080 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AvailableAbilitiesSB;
        // { +0084 }
        public Aow.Units.AoWListBox.TAOWListBox SelectedAbilities;
        // { +0088 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar SelectedAbilitiesSB;
        // { +008C }
        public Aow.Units.AoWButton.TAOWButton SelectedAbilitiesSort;
        // { +0090 }
        public Aow.Units.AoWButton.TAOWButton AvailableAbilityCostSort;
        // { +0094 }
        public Aow.Units.AoWListBox.TAOWListBox AvailableAbilitiesCost;
        // { +0098 }
        public Aow.Units.AoWButton.TAOWButton AvailableAbilitiesSort;
        // { +009C }
        public Aow.Units.AoWButton.TAOWButton AddAbilityBtn;
        // { +00A0 }
        public Aow.Units.AoWButton.TAOWButton RemoveBtn;
        // { +00A4 }
        public Aow.Units.AoWImage.TAOWImage FaceFrame;
        // { +00A8 }
        public Aow.Units.AoWEdit.TAOWEdit UnitName;
        // { +00AC }
        public Aow.Units.AoWEdit.TAOWEdit Alignment;
        // { +00B0 }
        public Aow.Units.AoWPanel.TAOWPanel TopPnl;
        // { +00B4 }
        public Aow.Units.AoWPanel.TAOWPanel StatisicsPnl;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel AttValue;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel AttLbl;
        // { +00C0 }
        public Aow.Units.AoWPanel.TAOWPanel AttPnl;
        // { +00C4 }
        public Aow.Units.AoWLabel.TAOWLabel DamValue;
        // { +00C8 }
        public Aow.Units.AoWLabel.TAOWLabel DamLbl;
        // { +00CC }
        public Aow.Units.AoWPanel.TAOWPanel DamPnl;
        // { +00D0 }
        public Aow.Units.AoWLabel.TAOWLabel DefValue;
        // { +00D4 }
        public Aow.Units.AoWLabel.TAOWLabel DefLbl;
        // { +00D8 }
        public Aow.Units.AoWPanel.TAOWPanel DefPnl;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel ResValue;
        // { +00E0 }
        public Aow.Units.AoWLabel.TAOWLabel ResLbl;
        // { +00E4 }
        public Aow.Units.AoWPanel.TAOWPanel ResPnl;
        // { +00E8 }
        public Aow.Units.AoWLabel.TAOWLabel HitValue;
        // { +00EC }
        public Aow.Units.AoWLabel.TAOWLabel HitLbl;
        // { +00F0 }
        public Aow.Units.AoWPanel.TAOWPanel HitPnl;
        // { +00F4 }
        public Aow.Units.AoWLabel.TAOWLabel MovValue;
        // { +00F8 }
        public Aow.Units.AoWLabel.TAOWLabel MovLbl;
        // { +00FC }
        public Aow.Units.AoWPanel.TAOWPanel MovPnl;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel AttI;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel DamI;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel DefI;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel ResI;
        // { +0110 }
        public Aow.Units.AoWLabel.TAOWLabel HitI;
        // { +0114 }
        public Aow.Units.AoWLabel.TAOWLabel MovI;
        // { +0118 }
        public Aow.Units.AoWLabel.TAOWLabel AttA;
        // { +011C }
        public Aow.Units.AoWLabel.TAOWLabel DamA;
        // { +0120 }
        public Aow.Units.AoWLabel.TAOWLabel DefA;
        // { +0124 }
        public Aow.Units.AoWLabel.TAOWLabel ResA;
        // { +0128 }
        public Aow.Units.AoWLabel.TAOWLabel HitA;
        // { +012C }
        public Aow.Units.AoWLabel.TAOWLabel MovA;
        // { +0130 }
        public Aow.Units.AoWLabel.TAOWLabel AttP;
        // { +0134 }
        public Aow.Units.AoWLabel.TAOWLabel DamP;
        // { +0138 }
        public Aow.Units.AoWLabel.TAOWLabel DefP;
        // { +013C }
        public Aow.Units.AoWLabel.TAOWLabel ResP;
        // { +0140 }
        public Aow.Units.AoWLabel.TAOWLabel HitP;
        // { +0144 }
        public Aow.Units.AoWLabel.TAOWLabel MovP;
        // { +0148 }
        public Aow.Units.AoWLabel.TAOWLabel AttB;
        // { +014C }
        public Aow.Units.AoWLabel.TAOWLabel DamB;
        // { +0150 }
        public Aow.Units.AoWLabel.TAOWLabel DefB;
        // { +0154 }
        public Aow.Units.AoWLabel.TAOWLabel ResB;
        // { +0158 }
        public Aow.Units.AoWLabel.TAOWLabel HitB;
        // { +015C }
        public Aow.Units.AoWLabel.TAOWLabel MovB;
        // { +0160 }
        public Aow.Units.AoWButton.TAOWButton AttUp;
        // { +0164 }
        public Aow.Units.AoWButton.TAOWButton AttDn;
        // { +0168 }
        public Aow.Units.AoWButton.TAOWButton DamUp;
        // { +016C }
        public Aow.Units.AoWButton.TAOWButton DamDn;
        // { +0170 }
        public Aow.Units.AoWButton.TAOWButton DefUp;
        // { +0174 }
        public Aow.Units.AoWButton.TAOWButton DefDn;
        // { +0178 }
        public Aow.Units.AoWButton.TAOWButton ResUp;
        // { +017C }
        public Aow.Units.AoWButton.TAOWButton ResDn;
        // { +0180 }
        public Aow.Units.AoWButton.TAOWButton HitUp;
        // { +0184 }
        public Aow.Units.AoWButton.TAOWButton HitDn;
        // { +0188 }
        public Aow.Units.AoWButton.TAOWButton MovUp;
        // { +018C }
        public Aow.Units.AoWButton.TAOWButton MovDn;
        // { +0190 }
        public Aow.Units.AoWImage.TAOWImage AttIc;
        // { +0194 }
        public Aow.Units.AoWImage.TAOWImage DamIcn;
        // { +0198 }
        public Aow.Units.AoWImage.TAOWImage DefIc;
        // { +019C }
        public Aow.Units.AoWImage.TAOWImage ResIc;
        // { +01A0 }
        public Aow.Units.AoWImage.TAOWImage HitIc;
        // { +01A4 }
        public Aow.Units.AoWImage.TAOWImage MovIc;
        // { +01A8 }
        public Aow.Units.AoWButton.TAOWButton SelectedAbilitiesCostSort;
        // { +01AC }
        public Aow.Units.AoWListBox.TAOWListBox SelectedAbilitiesCost;
        // { +01B0 }
        public Aow.Units.AoWPanel.TAOWPanel InfoPnl;
        // { +01B4 }
        public Aow.Units.AoWLabel.TAOWLabel LevelLbl;
        // { +01B8 }
        public Aow.Units.AoWLabel.TAOWLabel SkillPoints;
        // { +01BC }
        public Aow.Rtl.TIvTranslator Translator;
        // { +01C0 }
        public Aow.Units.EventLog.TExecuteDoneEvent FOnClose;
        // { $1C4 }
        public Aow.Units.AoWE.THeroUpgradeEventLog FReferencedHero;
        // { $1CC }
        public int FSelectedSort;
        // { $1D0 }
        public int FAvailableSort;
        // { $1D4 }
        public Aow.Units.AoWE.THero FHero;
        // { $1D8 }
        public Aow.Units.AoWE.THero FEditedHero;
        // AoWHeroUpgradeDlg.pas:287
        // { $1DC }
        public void ClearHero()
        {
            Aow.Units.EventLog.TExecuteDoneEvent OnClose = default;
            Aow.Units.AoWE.THeroUpgradeEventLog Hero = default;
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

            FEditedHero.Free();
            FEditedHero = null;
        }

        // AoWHeroUpgradeDlg.pas:280
        public bool ValidateHero()
        {
            bool Result = default;
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FUnitControl.FindUnit(FReferencedHero.FUnitID);
            FHero = (Aow.Units.AoWE.THero)call__1;
            Result = FHero != null;
            if (!Result)
            {
                Dlg.Visible = false;
            }

            return Result;
        }

        // AoWHeroUpgradeDlg.pas:257
        public void UpdateStatistics()
        {
            var call__2 = FHero.GetInherentAttack();
            AttB.Text = Aow.Rtl.Rtl.IntToStr(call__2);
            var call__3 = FHero.GetInherentDefense();
            DefB.Text = Aow.Rtl.Rtl.IntToStr(call__3);
            var call__4 = FHero.GetInherentDamage();
            DamB.Text = Aow.Rtl.Rtl.IntToStr(call__4);
            var call__5 = FHero.GetInherentResistance();
            ResB.Text = Aow.Rtl.Rtl.IntToStr(call__5);
            var call__6 = FHero.GetHits();
            HitB.Text = Aow.Rtl.Rtl.IntToStr(call__6);
            var call__7 = FHero.GetMoves();
            MovB.Text = Aow.Rtl.Rtl.IntToStr(call__7);
            AttA.Text = Aow.Rtl.Rtl.IntToStr(FEditedHero.FUnitAttack - FHero.FUnitAttack);
            DefA.Text = Aow.Rtl.Rtl.IntToStr(FEditedHero.FUnitDefense - FHero.FUnitDefense);
            DamA.Text = Aow.Rtl.Rtl.IntToStr(FEditedHero.FUnitDamage - FHero.FUnitDamage);
            ResA.Text = Aow.Rtl.Rtl.IntToStr(FEditedHero.FUnitResistance - FHero.FUnitResistance);
            HitA.Text = Aow.Rtl.Rtl.IntToStr(FEditedHero.FUnitHits - FHero.FUnitHits);
            MovA.Text = Aow.Rtl.Rtl.IntToStr(FEditedHero.FUnitMoves - FHero.FUnitMoves);
            var call__8 = FEditedHero.GetInherentAttack();
            AttValue.Text = Aow.Rtl.Rtl.IntToStr(call__8);
            var call__9 = FEditedHero.GetInherentDefense();
            DefValue.Text = Aow.Rtl.Rtl.IntToStr(call__9);
            var call__10 = FEditedHero.GetInherentDamage();
            DamValue.Text = Aow.Rtl.Rtl.IntToStr(call__10);
            var call__11 = FEditedHero.GetInherentResistance();
            ResValue.Text = Aow.Rtl.Rtl.IntToStr(call__11);
            var call__12 = FEditedHero.GetHits();
            HitValue.Text = Aow.Rtl.Rtl.IntToStr(call__12);
            var call__13 = FEditedHero.GetMoves();
            MovValue.Text = Aow.Rtl.Rtl.IntToStr(call__13);
            var call__14 = FEditedHero.GetSkillPoints();
            var call__15 = FHero.GetSkillPoints();
            SkillPoints.Text = (Aow.Rtl.Rtl.IntToStr(call__14) + AnsiString.FromBytes(47)) + Aow.Rtl.Rtl.IntToStr(call__15);
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
                    Aow.Units.Engine.Engine.SortStringsAscending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(SelectedAbilities.Items, [SelectedAbilitiesCost.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(SelectedAbilitiesCost.Items, [SelectedAbilities.Items]);
                    break;
            }
        }

        // AoWHeroUpgradeDlg.pas:232
        public void SortAvailable()
        {
            switch (FAvailableSort)
            {
                case 0:
                    Aow.Units.Engine.Engine.SortStringsAscending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(AvailableAbilities.Items, [AvailableAbilitiesCost.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(AvailableAbilitiesCost.Items, [AvailableAbilities.Items]);
                    break;
            }
        }

        // AoWHeroUpgradeDlg.pas:170
        public void RefreshAbilities()
        {
            Aow.Rtl.AnsiString Name = default;
            Aow.Rtl.AnsiString Cost = default;
            Aow.Units.AoWE.TAbility Ability = default;
            int I = default;
            AvailableAbilities.Items.Clear();
            AvailableAbilitiesCost.Items.Clear();
            {
                I = 1;
                int end__16 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetCount() - 1;
                for (; I <= end__16; I += 1)
                {
                    var call__17 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                    Ability = call__17;
                    bool bool__18 = (Ability != null);
                    if (bool__18)
                    {
                        var call__19 = Ability.CanExpand(FEditedHero);
                        bool__18 = call__19;
                    }

                    if (bool__18 && Ability.FSelectionTypes.Contains(Aow.Units.AoWE.TAbilitySelectionType.astHeroUpgrade))
                    {
                        var call__20 = Ability.ExpandName(FEditedHero);
                        AvailableAbilities.Items.AddObject(call__20, Aow.Rtl.Rtl.IntToObject((int)I));
                        var call__21 = Ability.ExpandCost(FEditedHero);
                        AvailableAbilitiesCost.Items.Add(Aow.Rtl.Rtl.IntToStr(call__21));
                    }
                }
            }

            SelectedAbilities.Items.Clear();
            SelectedAbilitiesCost.Items.Clear();
            {
                I = 1;
                int end__22 = FEditedHero.GetAbCount() - 1;
                for (; I <= end__22; I += 1)
                {
                    var call__23 = FEditedHero.GetInherentAbility(I);
                    if (call__23)
                    {
                        var call__24 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(I);
                        Ability = call__24;
                        var call__25 = Ability.GetName(FEditedHero);
                        Name = call__25;
                        var call__26 = FHero.GetAbEnabled(I);
                        bool bool__27 = call__26;
                        if (bool__27)
                        {
                            var call__28 = FHero.GetInherentAbilityLevel(I);
                            var call__29 = FEditedHero.GetInherentAbilityLevel(I);
                            bool__27 = (call__28 == call__29);
                        }

                        if (bool__27)
                        {
                            Name = ((Name + ((AnsiString)" (")) + Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FixedRStr)) + AnsiString.FromBytes(41);
                            Cost = AnsiString.FromBytes(45);
                        }
                        else
                        {
                            var call__30 = Ability.GetSkillPoints(FEditedHero);
                            var call__31 = Ability.GetSkillPoints(FHero);
                            Cost = Aow.Rtl.Rtl.IntToStr(call__30 - call__31);
                        }

                        SelectedAbilitiesCost.Items.Add(Cost);
                        SelectedAbilities.Items.AddObject(Name, Aow.Rtl.Rtl.IntToObject((int)I));
                    }
                }
            }

            var call__32 = AvailableAbilities.Items.Count;
            AvailableAbilitiesSB.Max = call__32 - 1;
            var call__33 = SelectedAbilities.Items.Count;
            SelectedAbilitiesSB.Max = call__33 - 1;
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
            bool Result = default;
            var call__34 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(AbilityID);
            var call__35 = call__34.CanExpand(FEditedHero);
            bool bool__36 = !call__35;
            if (!bool__36)
            {
                var call__37 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(AbilityID);
                var call__38 = call__37.ExpandCost(FEditedHero);
                var call__39 = FEditedHero.GetSkillPoints();
                bool__36 = (call__38 > call__39);
            }

            if (bool__36)
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Dlg.Visible = false;
        }

        // AoWHeroUpgradeDlg.pas:308
        public void FaceFramePreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Rtl.TRect R = default;
            if (ValidateHero())
            {
                {
                    var w__40 = (Aow.Units.AoWImage.TAOWImage)Sender;
                    var call__41 = Aow.Rtl.Rtl.Rect((TopPnl.WinLeft + w__40.WinLeft) + 2, TopPnl.WinTop + w__40.WinTop, ((TopPnl.WinLeft + w__40.WinLeft) + w__40.WinWidth) - 2, ((TopPnl.WinTop + w__40.WinTop) + w__40.WinHeight) - 2);
                    R = call__41;
                }

                var call__42 = FHero.GetFace();
                call__42.ShowStretched(R, SurfDesc);
            }
        }

        // AoWHeroUpgradeDlg.pas:323
        public void AttUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FEditedHero.SetUnitAttack(unchecked((sbyte)(FEditedHero.FUnitAttack + 1)));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:330
        public void AttDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEditedHero.FUnitAttack != FHero.FUnitAttack)
            {
                FEditedHero.SetUnitAttack(unchecked((sbyte)(FEditedHero.FUnitAttack - 1)));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:338
        public void DefUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FEditedHero.SetUnitDefense(unchecked((sbyte)(FEditedHero.FUnitDefense + 1)));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:345
        public void DefDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEditedHero.FUnitDefense != FHero.FUnitDefense)
            {
                FEditedHero.SetUnitDefense(unchecked((sbyte)(FEditedHero.FUnitDefense - 1)));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:353
        public void DamUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FEditedHero.SetUnitDamage(unchecked((sbyte)(FEditedHero.FUnitDamage + 1)));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:360
        public void DamDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEditedHero.FUnitDamage != FHero.FUnitDamage)
            {
                FEditedHero.SetUnitDamage(unchecked((sbyte)(FEditedHero.FUnitDamage - 1)));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:368
        public void ResUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FEditedHero.SetUnitResistance(unchecked((sbyte)(FEditedHero.FUnitResistance + 1)));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:375
        public void ResDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEditedHero.FUnitResistance != FHero.FUnitResistance)
            {
                FEditedHero.SetUnitResistance(unchecked((sbyte)(FEditedHero.FUnitResistance - 1)));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:383
        public void HitUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FEditedHero.SetUnitHits(unchecked((sbyte)(FEditedHero.FUnitHits + 1)));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:390
        public void HitDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEditedHero.FUnitHits != FHero.FUnitHits)
            {
                FEditedHero.SetUnitHits(unchecked((sbyte)(FEditedHero.FUnitHits - 1)));
            }

            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:398
        public void MovUpClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            FEditedHero.SetUnitMoves(unchecked((sbyte)(FEditedHero.FUnitMoves + 1)));
            RefreshStatistics();
        }

        // AoWHeroUpgradeDlg.pas:405
        public void MovDnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEditedHero.FUnitMoves != FHero.FUnitMoves)
            {
                FEditedHero.SetUnitMoves(unchecked((sbyte)(FEditedHero.FUnitMoves - 1)));
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
            Aow.Units.AoWListBox.TAOWListBox List = default;
            int Index = default;
            int AbilityID = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            List = AvailableAbilities;
            Index = List.Selected;
            if (Index != -1)
            {
                var call__43 = List.Items.Objects[Index];
                AbilityID = Aow.Rtl.Rtl.ObjectToInt(call__43);
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
            Aow.Units.AoWListBox.TAOWListBox List = default;
            int Index = default;
            int AbilityID = default;
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            List = SelectedAbilities;
            Index = List.Selected;
            if (Index != -1)
            {
                var call__44 = List.Items.Objects[Index];
                AbilityID = Aow.Rtl.Rtl.ObjectToInt(call__44);
                var call__45 = FHero.GetInherentAbility(AbilityID);
                if (call__45)
                {
                    var call__46 = FHero.GetInherentAbilityLevel(AbilityID);
                    var call__47 = FEditedHero.GetInherentAbilityLevel(AbilityID);
                    if (call__46 == call__47)
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
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
            Aow.Units.AoWE.THeroUpgradeTE Event = default;
            if (ValidateHero())
            {
                var call__48 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FHero.FPlayer);
                var call__49 = call__48.GetBusy();
                if (!call__49)
                {
                    Event = Aow.Units.AoWE.THeroUpgradeTE.Create(null);
                    Event.Setup(FHero);
                    Event.FAttack = unchecked((sbyte)(FEditedHero.FUnitAttack - FHero.FUnitAttack));
                    Event.FDefense = unchecked((sbyte)(FEditedHero.FUnitDefense - FHero.FUnitDefense));
                    Event.FDamage = unchecked((sbyte)(FEditedHero.FUnitDamage - FHero.FUnitDamage));
                    Event.FResistance = unchecked((sbyte)(FEditedHero.FUnitResistance - FHero.FUnitResistance));
                    Event.FHits = unchecked((sbyte)(FEditedHero.FUnitHits - FHero.FUnitHits));
                    Event.FMoves = unchecked((sbyte)(FEditedHero.FUnitMoves - FHero.FUnitMoves));
                    Event.FAbilities.AssignAb(FEditedHero);
                    var call__50 = Aow.Units.AoWE.AoWE.AoWHSMap.FTokenManager.GetTokenControl(FHero.FPlayer);
                    call__50.AddEvent(Event);
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
        public void AvailableAbilitiesMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility Ability = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (AvailableAbilities.Selected != -1))
            {
                var call__51 = AvailableAbilities.Items.Objects[AvailableAbilities.Selected];
                var call__52 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__51));
                Ability = call__52;
                if (Ability != null)
                {
                    var call__53 = Ability.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__53, Ability.FDescription);
                }
            }
        }

        // AoWHeroUpgradeDlg.pas:556
        public void SelectedAbilitiesMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility Ability = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (SelectedAbilities.Selected != -1))
            {
                var call__54 = SelectedAbilities.Items.Objects[SelectedAbilities.Selected];
                var call__55 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__54));
                Ability = call__55;
                if (Ability != null)
                {
                    var call__56 = Ability.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__56, Ability.FDescription);
                }
            }
        }

        // AoWHeroUpgradeDlg.pas:568
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWHeroUpgradeDlg
    {
    }
}
