// Translated from AoWMagic.pas.
using Aow.Rtl;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTabPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.E1GameFlow.E1GameFlow;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.aowHelpWin.aowHelpWin;
using static Aow.Units.E2Globals.E2Globals;

namespace Aow.Units.AoWMagic
{
    // enchantment sort, global sort, power sort
    public partial class TMagicWin : TDataModule
    {
        public TMagicWin(TComponent owner) : base(owner)
        { }

        public TAOWWindow Dlg;
        public TAOWLabel OWTitle;
        public TAOWImage OWTitleIc;
        public TAOWTabPanel HeaderTab;
        public TAoWFrame OWRPillar;
        public TAoWFrame OWLPillar;
        public TAOWPanel T1Pnl;
        public TAOWLabel ResearchPoints;
        public TAOWLabel ResLbl;
        public TAOWPanel T2Pnl;
        public TAOWPanel T2LBPnl;
        public TAOWListBox EnchantmentList;
        public TAOWButton EnchantmentSortBtn;
        public TAOWListBox UpkeepList;
        public TAOWButton UpkeepSortBtn;
        public TAOWListBox BindingList;
        public TAOWButton BindingSortBtn;
        public TAOWPanel ResPnl;
        public TAOWLabel NetMana;
        public TAOWLabel NetMamaLbl;
        public TAOWPanel NetManaPnl;
        public TAOWPanel T3Pnl;
        public TAOWPanel T3LBPnl;
        public TAOWListBox GlobalEnchantmentList;
        public TAOWButton GlobalEnchantmentSortBtn;
        public TAOWListBox GlobalUpkeepList;
        public TAOWButton GlobalUpkeepSortBtn;
        public TAOWListBox GlobalNameList;
        public TAOWButton GlobalNameSortBtn;
        public TAOWPanel RightPnl;
        public TAOWLabel SpellName;
        public TAOWButton GlobalDispelBtn;
        public TAOWButton DispelBtn;
        public TAOWButton ResearchBtn;
        public TAOWLabel ResearchTurnsLeft;
        public TAOWLabel SpellClass;
        public TAOWPanel SpellPnl;
        public TAOWLabel SpellCost;
        public TAOWImage SpellIcn;
        public TAOWButton Minimize;
        public TAOWButton ChangePowerBut;
        public TAOWLabel Power;
        public TAOWLabel PowerLbl;
        public TAOWPanel PowePnl;
        public TAOWPanel LeftPnl;
        public TAOWPanel NonePnl;
        public TAOWLabel NoneMsg;
        public TAOWPanel T4Pnl;
        public TAOWPanel T4LBPnl;
        public TAOWListBox PowerValueList;
        public TAOWButton PowerValueSortBtn;
        public TAOWListBox PowerSourceList;
        public TAOWButton PowerSourceSortBtn;
        public TAOWPanel T2BottonPnl;
        public TAOWVScrollBar EnchantmentSB;
        public TAOWVScrollBar GlobalEnchantmentSB;
        public TAOWVScrollBar PowerSB;
        public TAOWPanel T3BottomPnl;
        public TAOWPanel LeftFr;
        public TAOWPanel T2ButtonPnl;
        public TAOWPanel T3ButtonPnl;
        public TIvTranslator Translator;
        public TAOWButton Help;
        public int FEnchantmentSort;
        public int FGlobalEnchantmentSort;
        public int FPowerSort;
        // AoWMagic.pas:133
        public void SelectEnchantment(int Index)
        {
            if (Index >= EnchantmentList.Items.Count)
            {
                Index = EnchantmentList.Items.Count - 1;
            }

            EnchantmentList.Selected = Index;
            BindingList.Selected = Index;
            UpkeepList.Selected = Index;
            DispelBtn.Visible = Index != -1;
        }

        // AoWMagic.pas:142
        public void SelectGlobalEnchantment(int Index)
        {
            string Reason = "";
            TGlobalEnchantment Enchantment;
            if (Index >= GlobalEnchantmentList.Items.Count)
            {
                Index = GlobalEnchantmentList.Items.Count - 1;
            }

            GlobalEnchantmentList.Selected = Index;
            GlobalNameList.Selected = Index;
            GlobalUpkeepList.Selected = Index;
            if (Index != -1)
            {
                Enchantment = (TGlobalEnchantment)GlobalEnchantmentList.Items.Objects[Index];
                var canRemove = Enchantment.CanRemove(AoWHSMap.FSeatedPlayer, ref Reason);
                GlobalDispelBtn.Visible = canRemove;
            }
            else
            {
                GlobalDispelBtn.Visible = false;
            }
        }

        // AoWMagic.pas:160
        public void SelectPowerSource(int Index)
        {
            if (Index >= PowerSourceList.Items.Count)
            {
                Index = PowerSourceList.Items.Count - 1;
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
                    SortStringsAscending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                    break;
                case 1:
                    SortStringsDescending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                    break;
                case 2:
                    SortStringsAscending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                    break;
                case 3:
                    SortStringsDescending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                    break;
                case 4:
                    SortStringsNumericalAscending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                    break;
                case 5:
                    SortStringsNumericalDescending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                    break;
            }
        }

        // AoWMagic.pas:179
        public void SortGlobalEnchantments()
        {
            switch (FGlobalEnchantmentSort)
            {
                case 0:
                    SortStringsAscending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                    break;
                case 1:
                    SortStringsDescending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                    break;
                case 2:
                    SortStringsAscending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                    break;
                case 3:
                    SortStringsDescending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                    break;
                case 4:
                    SortStringsNumericalAscending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                    break;
                case 5:
                    SortStringsNumericalDescending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                    break;
            }
        }

        // AoWMagic.pas:191
        public void SortPowerSources()
        {
            switch (FPowerSort)
            {
                case 0:
                    SortStringsAscending(PowerSourceList.Items, [PowerValueList.Items]);
                    break;
                case 1:
                    SortStringsDescending(PowerSourceList.Items, [PowerValueList.Items]);
                    break;
                case 2:
                    SortStringsNumericalAscending(PowerValueList.Items, [PowerSourceList.Items]);
                    break;
                case 3:
                    SortStringsNumericalDescending(PowerValueList.Items, [PowerSourceList.Items]);
                    break;
            }
        }

        // AoWMagic.pas:203
        public void EnchantmentListDoubleClick()
        {
            bool condition = EnchantmentList.Selected >= 0;
            if (condition)
            {
                var count = EnchantmentList.Items.Count;
                condition = EnchantmentList.Selected < count;
            }

            if (condition)
            {
                ((TEnchantment)EnchantmentList.Items.Objects[EnchantmentList.Selected]).Select();
            }
        }

        // AoWMagic.pas:210
        public void EnchantmentListMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (Button == TMouseButton.mbRight)
            {
                bool condition = EnchantmentList.Selected >= 0;
                if (condition)
                {
                    var count = EnchantmentList.Items.Count;
                    condition = EnchantmentList.Selected < count;
                }

                if (condition)
                {
                    ((TEnchantment)EnchantmentList.Items.Objects[EnchantmentList.Selected]).CenterToLocation();
                }
            }
        }

        // AoWMagic.pas:218
        public void DispelBtnClick()
        {
            General.PlayButtonSound(0);
            bool condition = EnchantmentList.Selected >= 0;
            if (condition)
            {
                var count = EnchantmentList.Items.Count;
                condition = EnchantmentList.Selected < count;
            }

            if (condition)
            {
                var objectValue = EnchantmentList.Items.Objects[EnchantmentList.Selected];
                AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl.DispelEnchantment((TEnchantment)objectValue);
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
            General.PlayButtonSound(0);
            bool condition = GlobalEnchantmentList.Selected >= 0;
            if (condition)
            {
                var count = GlobalEnchantmentList.Items.Count;
                condition = GlobalEnchantmentList.Selected < count;
            }

            if (condition)
            {
                var objectValue = GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected];
                AoWHSMap.FGlobalMagicControl.RemoveEnchantment(AoWHSMap.FSeatedPlayer, (TGlobalEnchantment)objectValue);
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
            General.PlayButtonSound(0);
            SpellBook.ShowResearch();
        }

        // AoWMagic.pas:290
        public void ChangePowerButClick()
        {
            General.PlayButtonSound(0);
            PowerDlg.Execute();
        }

        // AoWMagic.pas:296
        public void SpellIcnDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TPlayerMagicControl Magic;
            TSpell Spell;
            int X;
            int Y;
            Magic = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FMagicControl;
            if (Magic.FResearchSpellID != 0)
            {
                Spell = AoWHSSet.FSpellControl.GetSpell(Magic.FResearchSpellID);
                if (Spell != null)
                {
                    if (Spell.FCastImages.GetImageSequence(10) != null)
                    {
                        var image = (TAOWImage)Sender;
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
            bool condition = PowerSourceList.Selected >= 0;
            if (condition)
            {
                var count = PowerSourceList.Items.Count;
                condition = PowerSourceList.Selected < count;
            }

            if (condition)
            {
                ((TPowerSource)PowerSourceList.Items.Objects[PowerSourceList.Selected]).Select();
            }
        }

        // AoWMagic.pas:323
        public void PowerValueListMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (Button == TMouseButton.mbRight)
            {
                if (PowerSourceList.Selected >= 0)
                {
                    var count = PowerSourceList.Items.Count;
                    if (PowerSourceList.Selected < count)
                    {
                        ((TPowerSource)PowerSourceList.Items.Objects[PowerSourceList.Selected]).CenterToLocation();
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
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
            General.PlayButtonSound(0);
        }

        // AoWMagic.pas:434
        public void DlgResize()
        {
            this.Dlg.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
            this.Dlg.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            this.Dlg.Alignment.LeftOffset = this.Dlg.WinLeft;
            this.Dlg.Alignment.TopOffset = this.Dlg.WinTop;
        }

        // AoWMagic.pas:447
        public void DlgShow()
        {
            General.RegisterDialog(Dlg);
        }

        // AoWMagic.pas:453
        public void DlgHide()
        {
            General.UnregisterDialog(Dlg);
        }

        // AoWMagic.pas:459
        public void HelpClick()
        {
            ShowHelp(506);
        }

        // AoWMagic.pas:470
        public void MinimizeClick()
        {
            General.PlayButtonSound(0);
            HideMagic(this);
        }

        // AoWMagic.pas:462
        public void GlobalEnchantmentMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (Button == TMouseButton.mbRight)
            {
                bool condition = GlobalEnchantmentList.Selected >= 0;
                if (condition)
                {
                    var count = GlobalEnchantmentList.Items.Count;
                    condition = GlobalEnchantmentList.Selected < count;
                }

                if (condition)
                {
                    ((TGlobalEnchantment)GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected]).CenterToLocation();
                }
            }
        }
    }

    public static partial class AoWMagic
    {
    }
}
