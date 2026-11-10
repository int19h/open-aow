// Translated from AoWMagic.pas.
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Events;
using Aow.Game.Host;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Global;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Grids;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWMagic.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

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
            var canRemove = Enchantment.CanRemove(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer, ref Reason);
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
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                break;
            case 1:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsDescending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                break;
            case 2:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                break;
            case 3:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsDescending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                break;
            case 4:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalAscending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                break;
            case 5:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalDescending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                break;
        }
    }

    // AoWMagic.pas:179
    public void SortGlobalEnchantments()
    {
        switch (FGlobalEnchantmentSort)
        {
            case 0:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                break;
            case 1:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsDescending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                break;
            case 2:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                break;
            case 3:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsDescending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                break;
            case 4:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalAscending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                break;
            case 5:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalDescending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                break;
        }
    }

    // AoWMagic.pas:191
    public void SortPowerSources()
    {
        switch (FPowerSort)
        {
            case 0:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsAscending(PowerSourceList.Items, [PowerValueList.Items]);
                break;
            case 1:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsDescending(PowerSourceList.Items, [PowerValueList.Items]);
                break;
            case 2:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalAscending(PowerValueList.Items, [PowerSourceList.Items]);
                break;
            case 3:
                Aow.Game.Engine.Nodes.StringSorting.SortStringsNumericalDescending(PowerValueList.Items, [PowerSourceList.Items]);
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
    public void EnchantmentListMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        bool condition = EnchantmentList.Selected >= 0;
        if (condition)
        {
            var count = EnchantmentList.Items.Count;
            condition = EnchantmentList.Selected < count;
        }

        if (condition)
        {
            var enchantment = EnchantmentList.Items.Objects[EnchantmentList.Selected];
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.DispelEnchantment((TEnchantment)enchantment);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        bool condition = GlobalEnchantmentList.Selected >= 0;
        if (condition)
        {
            var count = GlobalEnchantmentList.Items.Count;
            condition = GlobalEnchantmentList.Selected < count;
        }

        if (condition)
        {
            var globalEnchantment = GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected];
            Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGlobalMagicControl.RemoveEnchantment(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer, (TGlobalEnchantment)globalEnchantment);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.SpellBook.ShowResearch();
    }

    // AoWMagic.pas:290
    public void ChangePowerButClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.DialogHosts.PowerDlg.Execute();
    }

    // AoWMagic.pas:296
    public void SpellIcnDraw(TAOWImage Sender, TSurfaceDesc Surface)
    {
        TPlayerMagicControl Magic;
        TSpell Spell;
        int X;
        int Y;
        Magic = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl;
        if (Magic.FResearchSpellID != 0)
        {
            Spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(Magic.FResearchSpellID);
            if (Spell != null)
            {
                if (Spell.FCastImages.GetImageSequence(10) != null)
                {
                    X = Sender.FAOWWindow.WinLeft;
                    Y = Sender.FAOWWindow.WinTop + Sender.WinTop;
                    Spell.FCastImages.Show(10, Sender.WinLeft + X, Y, 0, Surface);
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
    public void PowerValueListMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        Aow.Game.Ui.Dialogs.DialogHosts.General.RegisterDialog(Dlg);
    }

    // AoWMagic.pas:453
    public void DlgHide()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.UnregisterDialog(Dlg);
    }

    // AoWMagic.pas:459
    public void HelpClick()
    {
        Aow.Game.Ui.Dialogs.THelpWindow.ShowHelp(506);
    }

    // AoWMagic.pas:470
    public void MinimizeClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
        Aow.Game.Ui.Dialogs.TMagicWin.HideMagic(this);
    }

    // AoWMagic.pas:462
    public void GlobalEnchantmentMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
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

    internal static void ShowMagic(TMagicWin Window)
    {
        if (!Window.Dlg.Visible)
        {
            Aow.Game.Ui.Dialogs.TMagicWin.RefreshMagicWindow(Window);
            Aow.Game.Ui.Dialogs.TMagicWin.RefreshGlobalMagic(Window);
        }

        Window.Dlg.Visible = true;
    }

    internal static void HideMagic(TMagicWin Window)
    {
        if (Window.Dlg.Visible)
        {
            Window.Dlg.Visible = false;
        }
    }

    internal static void RefreshMagicWindow(TMagicWin Window)
    {
        TPlayerMagicControl Control;
        int Turns;
        TSpell Spell;
        if (Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer != -1)
        {
            Control = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl;
            Window.EnchantmentList.Items.Clear();
            Window.BindingList.Items.Clear();
            Window.UpkeepList.Items.Clear();
            for (int I = 0, last = Control.FEnchantments.Count - 1; I <= last; I++)
            {
                var name = Control.FEnchantments.GetEnchantments(I).GetName();
                var enchantment = Control.FEnchantments.GetEnchantments(I);
                Window.EnchantmentList.Items.AddObject(name, enchantment);
                var binding = Control.FEnchantments.GetEnchantments(I).GetBinding();
                Window.BindingList.Items.Add(binding);
                var enchantmentUpkeep = Control.FEnchantments.GetEnchantments(I);
                Window.UpkeepList.Items.Add(IntToStr(enchantmentUpkeep.FUpkeep));
            }

            Window.PowerSourceList.Items.Clear();
            Window.PowerValueList.Items.Clear();
            for (int I = 0, last = Control.FPowerSources.Count - 1; I <= last; I++)
            {
                if (Control.FPowerSources.GetItems(I).Power() > 0)
                {
                    var name = Control.FPowerSources.GetItems(I).Name();
                    var powerSource = Control.FPowerSources.GetItems(I);
                    Window.PowerSourceList.Items.AddObject(name, powerSource);
                    var power = Control.FPowerSources.GetItems(I).Power();
                    Window.PowerValueList.Items.Add(IntToStr(power));
                }
            }

            var controlPower = Control.GetPower();
            Window.Power.Text = IntToStr(controlPower);
            var manaIncome = Control.GetManaIncome();
            var manaUpkeep = Control.GetManaUpkeep();
            var controlManaIncome = Control.GetManaIncome();
            var controlManaUpkeep = Control.GetManaUpkeep();
            Window.NetMana.Text = IntToStr(manaIncome) + "-" + IntToStr(manaUpkeep) + "=" + IntToStr(controlManaIncome - controlManaUpkeep);
            var netPowerToResearch = Control.GetNetPowerToResearch();
            Window.ResearchPoints.Text = IntToStr(netPowerToResearch);
            if (Control.FResearchSpellID != 0)
            {
                Spell = Aow.Game.Engine.TAoWHSSet.AoWHSSet.FSpellControl.GetSpell(Control.FResearchSpellID);
                Window.SpellName.Text = Spell.FSpellName;
                var magicSphere = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                var level = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.LevelRStr);
                Window.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                var castingCost = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.CastingCostRStr);
                var mana = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.ManaRStr);
                Window.SpellCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
                Turns = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer).FMagicControl.GetResearchTurnsLeft();
                if (Turns < 999)
                {
                    var fmtTurnsLeftStr = Aow.Game.Magic.Abilities.DurationText.FmtTurnsLeftStr(Turns);
                    Window.ResearchTurnsLeft.Text = fmtTurnsLeftStr;
                }
                else
                {
                    var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(Aow.Game.Magic.Abilities.GameText.XTurnsRStr), [(byte)63]);
                    Window.ResearchTurnsLeft.Text = format;
                }

                Window.SpellPnl.Visible = true;
            }
            else
            {
                Window.SpellPnl.Visible = false;
            }

            Window.SortEnchantments();
            Window.SelectEnchantment(Window.EnchantmentList.Selected);
        }

        Window.Dlg.FDrawn = false;
    }

    internal static void RefreshGlobalMagic(TMagicWin Window)
    {
        TGlobalMagicControl Control;
        Control = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FGlobalMagicControl;
        Window.GlobalEnchantmentList.Items.Clear();
        Window.GlobalNameList.Items.Clear();
        Window.GlobalUpkeepList.Items.Clear();
        for (int I = 0, last = Control.FEnchantments.Count - 1; I <= last; I++)
        {
            if (Control.FEnchantments.GetEnchantments(I).VisibleForPlayer(Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FSeatedPlayer))
            {
                var name = Control.FEnchantments.GetEnchantments(I).GetName();
                var enchantment = Control.FEnchantments.GetEnchantments(I);
                Window.GlobalEnchantmentList.Items.AddObject(name, enchantment);
                var enchantmentPlayerIndex = Control.FEnchantments.GetEnchantments(I);
                var player = Aow.Game.World.Map.TAoWHSMap.AoWHSMap.FPlayers.GetPlayers(enchantmentPlayerIndex.FPlayerIndex);
                Window.GlobalNameList.Items.Add(player.FLeader.FHeroName);
                var upkeep = Control.FEnchantments.GetEnchantments(I).GetUpkeep();
                Window.GlobalUpkeepList.Items.Add(IntToStr(upkeep));
            }
        }

        Window.SortGlobalEnchantments();
        Window.SelectGlobalEnchantment(Window.GlobalEnchantmentList.Selected);
        Window.Dlg.FDrawn = false;
    }

    internal static void ShowMagicEvent(TMagicWin Window, TPlayerMagicEventLog Event, TExecuteDoneEvent Done)
    {
        Aow.Game.Ui.Dialogs.TTBWindow.ShowMagicWindow(Window);
        if (Done != null)
        {
            Done(Event);
        }
    }

}
