// Translated from AoWMagic.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.Events;

using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// enchantment sort, global sort, power sort
public partial class MagicWindow : DataModule
{
    public MagicWindow(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Label OWTitle;
    public Image OWTitleIc;
    public TabPanel HeaderTab;
    public Frame OWRPillar;
    public Frame OWLPillar;
    public Panel T1Pnl;
    public Label ResearchPoints;
    public Label ResLbl;
    public Panel T2Pnl;
    public Panel T2LBPnl;
    public ListBox EnchantmentList;
    public Button EnchantmentSortBtn;
    public ListBox UpkeepList;
    public Button UpkeepSortBtn;
    public ListBox BindingList;
    public Button BindingSortBtn;
    public Panel ResPnl;
    public Label NetMana;
    public Label NetMamaLbl;
    public Panel NetManaPnl;
    public Panel T3Pnl;
    public Panel T3LBPnl;
    public ListBox GlobalEnchantmentList;
    public Button GlobalEnchantmentSortBtn;
    public ListBox GlobalUpkeepList;
    public Button GlobalUpkeepSortBtn;
    public ListBox GlobalNameList;
    public Button GlobalNameSortBtn;
    public Panel RightPnl;
    public Label SpellName;
    public Button GlobalDispelBtn;
    public Button DispelBtn;
    public Button ResearchBtn;
    public Label ResearchTurnsLeft;
    public Label SpellClass;
    public Panel SpellPnl;
    public Label SpellCost;
    public Image SpellIcn;
    public Button Minimize;
    public Button ChangePowerBut;
    public Label Power;
    public Label PowerLbl;
    public Panel PowePnl;
    public Panel LeftPnl;
    public Panel NonePnl;
    public Label NoneMsg;
    public Panel T4Pnl;
    public Panel T4LBPnl;
    public ListBox PowerValueList;
    public Button PowerValueSortBtn;
    public ListBox PowerSourceList;
    public Button PowerSourceSortBtn;
    public Panel T2BottonPnl;
    public VerticalScrollBar EnchantmentSB;
    public VerticalScrollBar GlobalEnchantmentSB;
    public VerticalScrollBar PowerSB;
    public Panel T3BottomPnl;
    public Panel LeftFr;
    public Panel T2ButtonPnl;
    public Panel T3ButtonPnl;
    public IvTranslator Translator;
    public Button Help;
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
        GlobalEnchantment Enchantment;
        if (Index >= GlobalEnchantmentList.Items.Count)
        {
            Index = GlobalEnchantmentList.Items.Count - 1;
        }

        GlobalEnchantmentList.Selected = Index;
        GlobalNameList.Selected = Index;
        GlobalUpkeepList.Selected = Index;
        if (Index != -1)
        {
            Enchantment = (GlobalEnchantment)GlobalEnchantmentList.Items.Objects[Index];
            var canRemove = Enchantment.CanRemove(AoWHSMap.Current.FSeatedPlayer, ref Reason);
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
                StringSorting.SortStringsAscending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(EnchantmentList.Items, [BindingList.Items, UpkeepList.Items]);
                break;
            case 2:
                StringSorting.SortStringsAscending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                break;
            case 3:
                StringSorting.SortStringsDescending(BindingList.Items, [EnchantmentList.Items, UpkeepList.Items]);
                break;
            case 4:
                StringSorting.SortStringsNumericalAscending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                break;
            case 5:
                StringSorting.SortStringsNumericalDescending(UpkeepList.Items, [EnchantmentList.Items, BindingList.Items]);
                break;
        }
    }

    // AoWMagic.pas:179
    public void SortGlobalEnchantments()
    {
        switch (FGlobalEnchantmentSort)
        {
            case 0:
                StringSorting.SortStringsAscending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(GlobalEnchantmentList.Items, [GlobalNameList.Items, GlobalUpkeepList.Items]);
                break;
            case 2:
                StringSorting.SortStringsAscending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                break;
            case 3:
                StringSorting.SortStringsDescending(GlobalNameList.Items, [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                break;
            case 4:
                StringSorting.SortStringsNumericalAscending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                break;
            case 5:
                StringSorting.SortStringsNumericalDescending(GlobalUpkeepList.Items, [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                break;
        }
    }

    // AoWMagic.pas:191
    public void SortPowerSources()
    {
        switch (FPowerSort)
        {
            case 0:
                StringSorting.SortStringsAscending(PowerSourceList.Items, [PowerValueList.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(PowerSourceList.Items, [PowerValueList.Items]);
                break;
            case 2:
                StringSorting.SortStringsNumericalAscending(PowerValueList.Items, [PowerSourceList.Items]);
                break;
            case 3:
                StringSorting.SortStringsNumericalDescending(PowerValueList.Items, [PowerSourceList.Items]);
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
            ((Enchantment)EnchantmentList.Items.Objects[EnchantmentList.Selected]).Select();
        }
    }

    // AoWMagic.pas:210
    public void EnchantmentListMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Button == MouseButton.Right)
        {
            bool condition = EnchantmentList.Selected >= 0;
            if (condition)
            {
                var count = EnchantmentList.Items.Count;
                condition = EnchantmentList.Selected < count;
            }

            if (condition)
            {
                ((Enchantment)EnchantmentList.Items.Objects[EnchantmentList.Selected]).CenterToLocation();
            }
        }
    }

    // AoWMagic.pas:218
    public void DispelBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        bool condition = EnchantmentList.Selected >= 0;
        if (condition)
        {
            var count = EnchantmentList.Items.Count;
            condition = EnchantmentList.Selected < count;
        }

        if (condition)
        {
            var enchantment = EnchantmentList.Items.Objects[EnchantmentList.Selected];
            AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.DispelEnchantment((Enchantment)enchantment);
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
        DialogHosts.General.PlayButtonSound(0);
        bool condition = GlobalEnchantmentList.Selected >= 0;
        if (condition)
        {
            var count = GlobalEnchantmentList.Items.Count;
            condition = GlobalEnchantmentList.Selected < count;
        }

        if (condition)
        {
            var globalEnchantment = GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected];
            AoWHSMap.Current.FGlobalMagicControl.RemoveEnchantment(AoWHSMap.Current.FSeatedPlayer, (GlobalEnchantment)globalEnchantment);
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
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.SpellBook.ShowResearch();
    }

    // AoWMagic.pas:290
    public void ChangePowerButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        DialogHosts.PowerDlg.Execute();
    }

    // AoWMagic.pas:296
    public void SpellIcnDraw(Image Sender, LockedSurface Surface)
    {
        PlayerMagicControl Magic;
        Spell Spell;
        int X;
        int Y;
        Magic = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl;
        if (Magic.FResearchSpellID != 0)
        {
            Spell = AoWHSSet.Current.FSpellControl.GetSpell(Magic.FResearchSpellID);
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
            ((PowerSource)PowerSourceList.Items.Objects[PowerSourceList.Selected]).Select();
        }
    }

    // AoWMagic.pas:323
    public void PowerValueListMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Button == MouseButton.Right)
        {
            if (PowerSourceList.Selected >= 0)
            {
                var count = PowerSourceList.Items.Count;
                if (PowerSourceList.Selected < count)
                {
                    ((PowerSource)PowerSourceList.Items.Objects[PowerSourceList.Selected]).CenterToLocation();
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
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
        DialogHosts.General.PlayButtonSound(0);
    }

    // AoWMagic.pas:434
    public void DlgResize()
    {
        this.Dlg.Alignment.AlignWidth = AlignWidth.Left;
        this.Dlg.Alignment.AlignHeight = AlignHeight.Top;
        this.Dlg.Alignment.LeftOffset = this.Dlg.WinLeft;
        this.Dlg.Alignment.TopOffset = this.Dlg.WinTop;
    }

    // AoWMagic.pas:447
    public void DlgShow()
    {
        DialogHosts.General.RegisterDialog(Dlg);
    }

    // AoWMagic.pas:453
    public void DlgHide()
    {
        DialogHosts.General.UnregisterDialog(Dlg);
    }

    // AoWMagic.pas:459
    public void HelpClick()
    {
        HelpWindow.ShowHelp(506);
    }

    // AoWMagic.pas:470
    public void MinimizeClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        HideMagic(this);
    }

    // AoWMagic.pas:462
    public void GlobalEnchantmentMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (Button == MouseButton.Right)
        {
            bool condition = GlobalEnchantmentList.Selected >= 0;
            if (condition)
            {
                var count = GlobalEnchantmentList.Items.Count;
                condition = GlobalEnchantmentList.Selected < count;
            }

            if (condition)
            {
                ((GlobalEnchantment)GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected]).CenterToLocation();
            }
        }
    }

    internal static void ShowMagic(MagicWindow Window)
    {
        if (!Window.Dlg.Visible)
        {
            RefreshMagicWindow(Window);
            RefreshGlobalMagic(Window);
        }

        Window.Dlg.Visible = true;
    }

    internal static void HideMagic(MagicWindow Window)
    {
        if (Window.Dlg.Visible)
        {
            Window.Dlg.Visible = false;
        }
    }

    internal static void RefreshMagicWindow(MagicWindow Window)
    {
        PlayerMagicControl Control;
        int Turns;
        Spell Spell;
        if (AoWHSMap.Current.FSeatedPlayer != -1)
        {
            Control = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl;
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
                Spell = AoWHSSet.Current.FSpellControl.GetSpell(Control.FResearchSpellID);
                Window.SpellName.Text = Spell.FSpellName;
                var magicSphere = AbilityRules.TranslateRStr(GameTables.TMagicSphereRStr[(int)Spell.FMagicSphere]);
                var level = AbilityRules.TranslateRStr(GameText.LevelRStr);
                Window.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(Spell.FSpellLevel);
                var castingCost = AbilityRules.TranslateRStr(GameText.CastingCostRStr);
                var mana = AbilityRules.TranslateRStr(GameText.ManaRStr);
                Window.SpellCost.Text = castingCost + ": " + IntToStr(Spell.FBaseManaCost) + " " + mana;
                Turns = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FMagicControl.GetResearchTurnsLeft();
                if (Turns < 999)
                {
                    var fmtTurnsLeftStr = DurationText.FmtTurnsLeftStr(Turns);
                    Window.ResearchTurnsLeft.Text = fmtTurnsLeftStr;
                }
                else
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.XTurnsRStr), [(byte)63]);
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

    internal static void RefreshGlobalMagic(MagicWindow Window)
    {
        GlobalMagicControl Control;
        Control = AoWHSMap.Current.FGlobalMagicControl;
        Window.GlobalEnchantmentList.Items.Clear();
        Window.GlobalNameList.Items.Clear();
        Window.GlobalUpkeepList.Items.Clear();
        for (int I = 0, last = Control.FEnchantments.Count - 1; I <= last; I++)
        {
            if (Control.FEnchantments.GetEnchantments(I).VisibleForPlayer(AoWHSMap.Current.FSeatedPlayer))
            {
                var name = Control.FEnchantments.GetEnchantments(I).GetName();
                var enchantment = Control.FEnchantments.GetEnchantments(I);
                Window.GlobalEnchantmentList.Items.AddObject(name, enchantment);
                var enchantmentPlayerIndex = Control.FEnchantments.GetEnchantments(I);
                var player = AoWHSMap.Current.FPlayers.GetPlayers(enchantmentPlayerIndex.FPlayerIndex);
                Window.GlobalNameList.Items.Add(player.FLeader.FHeroName);
                var upkeep = Control.FEnchantments.GetEnchantments(I).GetUpkeep();
                Window.GlobalUpkeepList.Items.Add(IntToStr(upkeep));
            }
        }

        Window.SortGlobalEnchantments();
        Window.SelectGlobalEnchantment(Window.GlobalEnchantmentList.Selected);
        Window.Dlg.FDrawn = false;
    }

    internal static void ShowMagicEvent(MagicWindow Window, PlayerMagicEventLog Event, ExecuteDoneEvent Done)
    {
        TaskBar.ShowMagicWindow(Window);
        if (Done != null)
        {
            Done(Event);
        }
    }

}
