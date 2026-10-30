// Translated from AoWMagic.pas.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
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
using Aow.Units.AoWTabPanel;
using static Aow.Units.AoWTabPanel.AoWTabPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.E1GameFlow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWPowerDlg;
using static Aow.Units.AoWPowerDlg.AoWPowerDlg;
using Aow.Units.aowHelpWin;
using static Aow.Units.aowHelpWin.aowHelpWin;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;

namespace Aow.Units.AoWMagic
{
    // { TMagicWin: size $14C; $140 enchantment sort, $144 global sort, $148 power sort. VMT 0042C190. }
    public partial class TMagicWin : Aow.Rtl.TDataModule
    {
        public TMagicWin(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel OWTitle;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage OWTitleIc;
        // { +004C }
        public Aow.Units.AoWTabPanel.TAOWTabPanel HeaderTab;
        // { +0050 }
        public Aow.Units.AoWFrame.TAoWFrame OWRPillar;
        // { +0054 }
        public Aow.Units.AoWFrame.TAoWFrame OWLPillar;
        // { +0058 }
        public Aow.Units.AoWPanel.TAOWPanel T1Pnl;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel ResearchPoints;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel ResLbl;
        // { +0064 }
        public Aow.Units.AoWPanel.TAOWPanel T2Pnl;
        // { +0068 }
        public Aow.Units.AoWPanel.TAOWPanel T2LBPnl;
        // { +006C }
        public Aow.Units.AoWListBox.TAOWListBox EnchantmentList;
        // { +0070 }
        public Aow.Units.AoWButton.TAOWButton EnchantmentSortBtn;
        // { +0074 }
        public Aow.Units.AoWListBox.TAOWListBox UpkeepList;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton UpkeepSortBtn;
        // { +007C }
        public Aow.Units.AoWListBox.TAOWListBox BindingList;
        // { +0080 }
        public Aow.Units.AoWButton.TAOWButton BindingSortBtn;
        // { +0084 }
        public Aow.Units.AoWPanel.TAOWPanel ResPnl;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel NetMana;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel NetMamaLbl;
        // { +0090 }
        public Aow.Units.AoWPanel.TAOWPanel NetManaPnl;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel T3Pnl;
        // { +0098 }
        public Aow.Units.AoWPanel.TAOWPanel T3LBPnl;
        // { +009C }
        public Aow.Units.AoWListBox.TAOWListBox GlobalEnchantmentList;
        // { +00A0 }
        public Aow.Units.AoWButton.TAOWButton GlobalEnchantmentSortBtn;
        // { +00A4 }
        public Aow.Units.AoWListBox.TAOWListBox GlobalUpkeepList;
        // { +00A8 }
        public Aow.Units.AoWButton.TAOWButton GlobalUpkeepSortBtn;
        // { +00AC }
        public Aow.Units.AoWListBox.TAOWListBox GlobalNameList;
        // { +00B0 }
        public Aow.Units.AoWButton.TAOWButton GlobalNameSortBtn;
        // { +00B4 }
        public Aow.Units.AoWPanel.TAOWPanel RightPnl;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel SpellName;
        // { +00BC }
        public Aow.Units.AoWButton.TAOWButton GlobalDispelBtn;
        // { +00C0 }
        public Aow.Units.AoWButton.TAOWButton DispelBtn;
        // { +00C4 }
        public Aow.Units.AoWButton.TAOWButton ResearchBtn;
        // { +00C8 }
        public Aow.Units.AoWLabel.TAOWLabel ResearchTurnsLeft;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel SpellClass;
        // { +00D0 }
        public Aow.Units.AoWPanel.TAOWPanel SpellPnl;
        // { +00D4 }
        public Aow.Units.AoWLabel.TAOWLabel SpellCost;
        // { +00D8 }
        public Aow.Units.AoWImage.TAOWImage SpellIcn;
        // { +00DC }
        public Aow.Units.AoWButton.TAOWButton Minimize;
        // { +00E0 }
        public Aow.Units.AoWButton.TAOWButton ChangePowerBut;
        // { +00E4 }
        public Aow.Units.AoWLabel.TAOWLabel Power;
        // { +00E8 }
        public Aow.Units.AoWLabel.TAOWLabel PowerLbl;
        // { +00EC }
        public Aow.Units.AoWPanel.TAOWPanel PowePnl;
        // { +00F0 }
        public Aow.Units.AoWPanel.TAOWPanel LeftPnl;
        // { +00F4 }
        public Aow.Units.AoWPanel.TAOWPanel NonePnl;
        // { +00F8 }
        public Aow.Units.AoWLabel.TAOWLabel NoneMsg;
        // { +00FC }
        public Aow.Units.AoWPanel.TAOWPanel T4Pnl;
        // { +0100 }
        public Aow.Units.AoWPanel.TAOWPanel T4LBPnl;
        // { +0104 }
        public Aow.Units.AoWListBox.TAOWListBox PowerValueList;
        // { +0108 }
        public Aow.Units.AoWButton.TAOWButton PowerValueSortBtn;
        // { +010C }
        public Aow.Units.AoWListBox.TAOWListBox PowerSourceList;
        // { +0110 }
        public Aow.Units.AoWButton.TAOWButton PowerSourceSortBtn;
        // { +0114 }
        public Aow.Units.AoWPanel.TAOWPanel T2BottonPnl;
        // { +0118 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar EnchantmentSB;
        // { +011C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar GlobalEnchantmentSB;
        // { +0120 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar PowerSB;
        // { +0124 }
        public Aow.Units.AoWPanel.TAOWPanel T3BottomPnl;
        // { +0128 }
        public Aow.Units.AoWPanel.TAOWPanel LeftFr;
        // { +012C }
        public Aow.Units.AoWPanel.TAOWPanel T2ButtonPnl;
        // { +0130 }
        public Aow.Units.AoWPanel.TAOWPanel T3ButtonPnl;
        // { +0134 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0138 }
        public Aow.Units.AoWButton.TAOWButton Help;
        // { +013C }
        public int FEnchantmentSort;
        // { $140 }
        public int FGlobalEnchantmentSort;
        // { $144 }
        public int FPowerSort;
        // AoWMagic.pas:133
        // { $148 }
        public void SelectEnchantment(int Index)
        {
            var call__1 = EnchantmentList.Items.Count;
            if (Index >= call__1)
            {
                var call__2 = EnchantmentList.Items.Count;
                Index = call__2 - 1;
            }

            EnchantmentList.Selected = Index;
            BindingList.Selected = Index;
            UpkeepList.Selected = Index;
            DispelBtn.Visible = Index != -1;
        }

        // AoWMagic.pas:142
        public void SelectGlobalEnchantment(int Index)
        {
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.AoWE.TGlobalEnchantment Enchantment = default;
            var call__3 = GlobalEnchantmentList.Items.Count;
            if (Index >= call__3)
            {
                var call__4 = GlobalEnchantmentList.Items.Count;
                Index = call__4 - 1;
            }

            GlobalEnchantmentList.Selected = Index;
            GlobalNameList.Selected = Index;
            GlobalUpkeepList.Selected = Index;
            if (Index != -1)
            {
                var call__5 = GlobalEnchantmentList.Items.Objects[Index];
                Enchantment = (Aow.Units.AoWE.TGlobalEnchantment)call__5;
                var call__6 = Enchantment.CanRemove(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), ref Reason);
                GlobalDispelBtn.Visible = call__6;
            }
            else
            {
                GlobalDispelBtn.Visible = false;
            }
        }

        // AoWMagic.pas:160
        public void SelectPowerSource(int Index)
        {
            var call__7 = PowerSourceList.Items.Count;
            if (Index >= call__7)
            {
                var call__8 = PowerSourceList.Items.Count;
                Index = call__8 - 1;
            }

            PowerSourceList.Selected = Index;
            PowerValueList.Selected = Index;
        }

        // AoWMagic.pas:167
        public void SortEnchantments()
        {
            switch (FEnchantmentSort)
            {
                case 0:
                    Aow.Units.Engine.Engine.SortStringsAscending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsAscending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsDescending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                    break;
                case 4:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                    break;
                case 5:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                    break;
            }
        }

        // AoWMagic.pas:179
        public void SortGlobalEnchantments()
        {
            switch (FGlobalEnchantmentSort)
            {
                case 0:
                    Aow.Units.Engine.Engine.SortStringsAscending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsAscending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsDescending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                    break;
                case 4:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                    break;
                case 5:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                    break;
            }
        }

        // AoWMagic.pas:191
        public void SortPowerSources()
        {
            switch (FPowerSort)
            {
                case 0:
                    Aow.Units.Engine.Engine.SortStringsAscending(PowerSourceList.Items, [PowerValueList.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(PowerSourceList.Items, [PowerValueList.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsNumericalAscending(PowerValueList.Items, [PowerSourceList.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsNumericalDescending(PowerValueList.Items, [PowerSourceList.Items]);
                    break;
            }
        }

        // AoWMagic.pas:203
        public void EnchantmentListDoubleClick()
        {
            bool bool__9 = (EnchantmentList.Selected >= 0);
            if (bool__9)
            {
                var call__10 = EnchantmentList.Items.Count;
                bool__9 = (EnchantmentList.Selected < call__10);
            }

            if (bool__9)
            {
                var call__11 = EnchantmentList.Items.Objects[EnchantmentList.Selected];
                ((Aow.Units.AoWE.TEnchantment)call__11).Select();
            }
        }

        // AoWMagic.pas:210
        public void EnchantmentListMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (Button == Aow.Rtl.TMouseButton.mbRight)
            {
                bool bool__12 = (EnchantmentList.Selected >= 0);
                if (bool__12)
                {
                    var call__13 = EnchantmentList.Items.Count;
                    bool__12 = (EnchantmentList.Selected < call__13);
                }

                if (bool__12)
                {
                    var call__14 = EnchantmentList.Items.Objects[EnchantmentList.Selected];
                    ((Aow.Units.AoWE.TEnchantment)call__14).CenterToLocation();
                }
            }
        }

        // AoWMagic.pas:218
        public void DispelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            bool bool__15 = (EnchantmentList.Selected >= 0);
            if (bool__15)
            {
                var call__16 = EnchantmentList.Items.Count;
                bool__15 = (EnchantmentList.Selected < call__16);
            }

            if (bool__15)
            {
                var call__17 = EnchantmentList.Items.Objects[EnchantmentList.Selected];
                var call__18 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                call__18.FMagicControl.DispelEnchantment((Aow.Units.AoWE.TEnchantment)call__17);
            }
        }

        // AoWMagic.pas:227
        public void EnchantmentListChange()
        {
            SelectEnchantment(EnchantmentList.Selected);
        }

        // AoWMagic.pas:233
        public void BindingListChange()
        {
            SelectEnchantment(BindingList.Selected);
        }

        // AoWMagic.pas:239
        public void UpkeepListChange()
        {
            SelectEnchantment(UpkeepList.Selected);
        }

        // AoWMagic.pas:245
        public void GlobalDispelBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            bool bool__19 = (GlobalEnchantmentList.Selected >= 0);
            if (bool__19)
            {
                var call__20 = GlobalEnchantmentList.Items.Count;
                bool__19 = (GlobalEnchantmentList.Selected < call__20);
            }

            if (bool__19)
            {
                var call__21 = GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected];
                Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.RemoveEnchantment(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), (Aow.Units.AoWE.TGlobalEnchantment)call__21);
            }
        }

        // AoWMagic.pas:254
        public void GlobalEnchantmentListChange()
        {
            SelectGlobalEnchantment(GlobalEnchantmentList.Selected);
        }

        // AoWMagic.pas:260
        public void GlobalNameListChange()
        {
            SelectGlobalEnchantment(GlobalNameList.Selected);
        }

        // AoWMagic.pas:266
        public void GlobalUpkeepListChange()
        {
            SelectGlobalEnchantment(GlobalUpkeepList.Selected);
        }

        // AoWMagic.pas:272
        public void PowerSourceListChange()
        {
            SelectPowerSource(PowerSourceList.Selected);
        }

        // AoWMagic.pas:278
        public void PowerValueListChange()
        {
            SelectPowerSource(PowerValueList.Selected);
        }

        // AoWMagic.pas:284
        public void ResearchBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.AoWExeForms.AoWExeForms.SpellBook.ShowResearch();
        }

        // AoWMagic.pas:290
        public void ChangePowerButClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.E2Globals.E2Globals.PowerDlg.Execute();
        }

        // AoWMagic.pas:296
        public void SpellIcnDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Units.AoWE.TPlayerMagicControl Magic = default;
            Aow.Units.AoWE.TSpell Spell = default;
            int X = default;
            int Y = default;
            var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            Magic = call__22.FMagicControl;
            if (Magic.FResearchSpellID != 0)
            {
                var call__23 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(Magic.FResearchSpellID);
                Spell = call__23;
                if (Spell != null)
                {
                    var call__24 = Spell.FCastImages.GetImageSequence(10);
                    if (call__24 != null)
                    {
                        var image = (Aow.Units.AoWImage.TAOWImage)Sender;
                        X = image.FAOWWindow.WinLeft;
                        Y = image.FAOWWindow.WinTop + image.WinTop;
                        Spell.FCastImages.Show(10, image.WinLeft + X, Y, 0, Surface);
                    }
                }
            }
        }

        // AoWMagic.pas:316
        public void PowerSourceListDoubleClick()
        {
            bool bool__26 = (PowerSourceList.Selected >= 0);
            if (bool__26)
            {
                var call__27 = PowerSourceList.Items.Count;
                bool__26 = (PowerSourceList.Selected < call__27);
            }

            if (bool__26)
            {
                var call__28 = PowerSourceList.Items.Objects[PowerSourceList.Selected];
                ((Aow.Units.AoWE.TPowerSource)call__28).Select();
            }
        }

        // AoWMagic.pas:323
        public void PowerValueListMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            // { Tried count-first comparison distinct first selection cast; no exact match. }
            if (Button == Aow.Rtl.TMouseButton.mbRight)
            {
                if (PowerSourceList.Selected >= 0)
                {
                    var call__29 = PowerSourceList.Items.Count;
                    if (PowerSourceList.Selected < call__29)
                    {
                        var call__30 = PowerSourceList.Items.Objects[PowerSourceList.Selected];
                        ((Aow.Units.AoWE.TPowerSource)call__30).CenterToLocation();
                    }
                }
            }
        }

        // AoWMagic.pas:331
        public void PowerSBChange()
        {
            PowerSourceList.ListOffset = PowerSB.Position;
            PowerValueList.ListOffset = PowerSB.Position;
        }

        // AoWMagic.pas:338
        public void GlobalEnchantmentSBChange()
        {
            GlobalEnchantmentList.ListOffset = GlobalEnchantmentSB.Position;
            GlobalNameList.ListOffset = GlobalEnchantmentSB.Position;
            GlobalUpkeepList.ListOffset = GlobalEnchantmentSB.Position;
        }

        // AoWMagic.pas:346
        public void EnchantmentSBChange()
        {
            EnchantmentList.ListOffset = EnchantmentSB.Position;
            BindingList.ListOffset = EnchantmentSB.Position;
            UpkeepList.ListOffset = EnchantmentSB.Position;
        }

        // AoWMagic.pas:354
        public void EnchantmentSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEnchantmentSort == 0)
            {
                FEnchantmentSort = 1;
            }
            else
            {
                FEnchantmentSort = 0;
            }

            SortEnchantments();
        }

        // AoWMagic.pas:363
        public void BindingSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEnchantmentSort == 2)
            {
                FEnchantmentSort = 3;
            }
            else
            {
                FEnchantmentSort = 2;
            }

            SortEnchantments();
        }

        // AoWMagic.pas:372
        public void UpkeepSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FEnchantmentSort == 4)
            {
                FEnchantmentSort = 5;
            }
            else
            {
                FEnchantmentSort = 4;
            }

            SortEnchantments();
        }

        // AoWMagic.pas:381
        public void GlobalEnchantmentSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FGlobalEnchantmentSort == 0)
            {
                FGlobalEnchantmentSort = 1;
            }
            else
            {
                FGlobalEnchantmentSort = 0;
            }

            SortGlobalEnchantments();
        }

        // AoWMagic.pas:390
        public void GlobalNameSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            // { Original name/upkeep buttons also toggle the enchantment-name order. }
            if (FGlobalEnchantmentSort == 0)
            {
                FGlobalEnchantmentSort = 1;
            }
            else
            {
                FGlobalEnchantmentSort = 0;
            }

            SortGlobalEnchantments();
        }

        // AoWMagic.pas:400
        public void GlobalUpkeepSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            // { Original name/upkeep buttons also toggle the enchantment-name order. }
            if (FGlobalEnchantmentSort == 0)
            {
                FGlobalEnchantmentSort = 1;
            }
            else
            {
                FGlobalEnchantmentSort = 0;
            }

            SortGlobalEnchantments();
        }

        // AoWMagic.pas:410
        public void PowerSourceSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FPowerSort == 0)
            {
                FPowerSort = 1;
            }
            else
            {
                FPowerSort = 0;
            }

            SortPowerSources();
        }

        // AoWMagic.pas:419
        public void PowerValueSortBtnClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            if (FPowerSort == 2)
            {
                FPowerSort = 3;
            }
            else
            {
                FPowerSort = 2;
            }

            SortPowerSources();
        }

        // AoWMagic.pas:428
        public void HeaderTabChange()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
        }

        // AoWMagic.pas:434
        public void DlgResize()
        {
            this.Dlg.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
            this.Dlg.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            this.Dlg.Alignment.LeftOffset = this.Dlg.WinLeft;
            this.Dlg.Alignment.TopOffset = this.Dlg.WinTop;
        }

        // AoWMagic.pas:447
        public void DlgShow()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.RegisterDialog(Dlg);
        }

        // AoWMagic.pas:453
        public void DlgHide()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.UnregisterDialog(Dlg);
        }

        // AoWMagic.pas:459
        public void HelpClick()
        {
            Aow.Units.aowHelpWin.aowHelpWin.ShowHelp(506);
        }

        // AoWMagic.pas:470
        public void MinimizeClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            Aow.Units.E1GameFlow.E1GameFlow.HideMagic(this);
        }

        // AoWMagic.pas:462
        public void GlobalEnchantmentMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (Button == Aow.Rtl.TMouseButton.mbRight)
            {
                bool bool__32 = (GlobalEnchantmentList.Selected >= 0);
                if (bool__32)
                {
                    var call__33 = GlobalEnchantmentList.Items.Count;
                    bool__32 = (GlobalEnchantmentList.Selected < call__33);
                }

                if (bool__32)
                {
                    var call__34 = GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected];
                    ((Aow.Units.AoWE.TGlobalEnchantment)call__34).CenterToLocation();
                }
            }
        }
    }

    public static partial class AoWMagic
    {
    }
}
