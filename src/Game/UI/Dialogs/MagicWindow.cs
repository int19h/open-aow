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
    private int _enchantmentSort;
    private int _globalEnchantmentSort;
    private int _powerSort;

    // AoWMagic.pas:133
    public void SelectEnchantment(int index)
    {
        if (index >= EnchantmentList.Items.Count)
        {
            index = EnchantmentList.Items.Count - 1;
        }

        EnchantmentList.Selected = index;
        BindingList.Selected = index;
        UpkeepList.Selected = index;
        DispelBtn.Visible = index != -1;
    }

    // AoWMagic.pas:142
    public void SelectGlobalEnchantment(int index)
    {
        string reason = "";
        GlobalEnchantment enchantment;
        if (index >= GlobalEnchantmentList.Items.Count)
        {
            index = GlobalEnchantmentList.Items.Count - 1;
        }

        GlobalEnchantmentList.Selected = index;
        GlobalNameList.Selected = index;
        GlobalUpkeepList.Selected = index;
        if (index != -1)
        {
            enchantment = (GlobalEnchantment)GlobalEnchantmentList.Items.Objects[index];
            var canRemove = enchantment.CanRemove(AoWHSMap.Current.SeatedPlayer, ref reason);
            GlobalDispelBtn.Visible = canRemove;
        }
        else
        {
            GlobalDispelBtn.Visible = false;
        }
    }

    // AoWMagic.pas:160
    public void SelectPowerSource(int index)
    {
        if (index >= PowerSourceList.Items.Count)
        {
            index = PowerSourceList.Items.Count - 1;
        }

        PowerSourceList.Selected = index;
        PowerValueList.Selected = index;
    }

    // AoWMagic.pas:167
    public void SortEnchantments()
    {
        switch (_enchantmentSort)
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
        switch (_globalEnchantmentSort)
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
        switch (_powerSort)
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
    public void EnchantmentListMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right)
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
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.DispelEnchantment((Enchantment)enchantment);
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
            AoWHSMap.Current.GlobalMagicControl.RemoveEnchantment(AoWHSMap.Current.SeatedPlayer, (GlobalEnchantment)globalEnchantment);
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
        DialogHosts.PowerDialog.Execute();
    }

    // AoWMagic.pas:296
    public void SpellIcnDraw(Image sender, LockedSurface surface)
    {
        PlayerMagicControl magic;
        Spell spell;
        int X;
        int Y;
        magic = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl;
        if (magic.ResearchSpellId != 0)
        {
            spell = AoWHSSet.Current.SpellControl.GetSpell(magic.ResearchSpellId);
            if (spell != null)
            {
                if (spell.CastImages.GetImageSequence(10) != null)
                {
                    X = sender.Window.WinLeft;
                    Y = sender.Window.WinTop + sender.WinTop;
                    spell.CastImages.Show(10, sender.WinLeft + X, Y, 0, surface);
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
    public void PowerValueListMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right)
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
        if (_enchantmentSort == 0)
        {
            _enchantmentSort = 1;
        }
        else
        {
            _enchantmentSort = 0;
        }

        SortEnchantments();
    }

    // AoWMagic.pas:363
    public void BindingSortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_enchantmentSort == 2)
        {
            _enchantmentSort = 3;
        }
        else
        {
            _enchantmentSort = 2;
        }

        SortEnchantments();
    }

    // AoWMagic.pas:372
    public void UpkeepSortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_enchantmentSort == 4)
        {
            _enchantmentSort = 5;
        }
        else
        {
            _enchantmentSort = 4;
        }

        SortEnchantments();
    }

    // AoWMagic.pas:381
    public void GlobalEnchantmentSortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_globalEnchantmentSort == 0)
        {
            _globalEnchantmentSort = 1;
        }
        else
        {
            _globalEnchantmentSort = 0;
        }

        SortGlobalEnchantments();
    }

    // AoWMagic.pas:390
    public void GlobalNameSortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        // { Original name/upkeep buttons also toggle the enchantment-name order. }
        if (_globalEnchantmentSort == 0)
        {
            _globalEnchantmentSort = 1;
        }
        else
        {
            _globalEnchantmentSort = 0;
        }

        SortGlobalEnchantments();
    }

    // AoWMagic.pas:400
    public void GlobalUpkeepSortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        // { Original name/upkeep buttons also toggle the enchantment-name order. }
        if (_globalEnchantmentSort == 0)
        {
            _globalEnchantmentSort = 1;
        }
        else
        {
            _globalEnchantmentSort = 0;
        }

        SortGlobalEnchantments();
    }

    // AoWMagic.pas:410
    public void PowerSourceSortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_powerSort == 0)
        {
            _powerSort = 1;
        }
        else
        {
            _powerSort = 0;
        }

        SortPowerSources();
    }

    // AoWMagic.pas:419
    public void PowerValueSortBtnClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_powerSort == 2)
        {
            _powerSort = 3;
        }
        else
        {
            _powerSort = 2;
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
    public void GlobalEnchantmentMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right)
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

    internal static void ShowMagic(MagicWindow window)
    {
        if (!window.Dlg.Visible)
        {
            RefreshMagicWindow(window);
            RefreshGlobalMagic(window);
        }

        window.Dlg.Visible = true;
    }

    internal static void HideMagic(MagicWindow window)
    {
        if (window.Dlg.Visible)
        {
            window.Dlg.Visible = false;
        }
    }

    internal static void RefreshMagicWindow(MagicWindow window)
    {
        PlayerMagicControl control;
        int turns;
        Spell spell;
        if (AoWHSMap.Current.SeatedPlayer != -1)
        {
            control = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl;
            window.EnchantmentList.Items.Clear();
            window.BindingList.Items.Clear();
            window.UpkeepList.Items.Clear();
            for (int I = 0, last = control.Enchantments.Count - 1; I <= last; I++)
            {
                var name = control.Enchantments.GetEnchantments(I).GetName();
                var enchantment = control.Enchantments.GetEnchantments(I);
                window.EnchantmentList.Items.AddObject(name, enchantment);
                var binding = control.Enchantments.GetEnchantments(I).GetBinding();
                window.BindingList.Items.Add(binding);
                var enchantmentUpkeep = control.Enchantments.GetEnchantments(I);
                window.UpkeepList.Items.Add(IntToStr(enchantmentUpkeep.Upkeep));
            }

            window.PowerSourceList.Items.Clear();
            window.PowerValueList.Items.Clear();
            for (int I = 0, last = control.PowerSources.Count - 1; I <= last; I++)
            {
                if (control.PowerSources.GetItems(I).Power() > 0)
                {
                    var name = control.PowerSources.GetItems(I).Name();
                    var powerSource = control.PowerSources.GetItems(I);
                    window.PowerSourceList.Items.AddObject(name, powerSource);
                    var power = control.PowerSources.GetItems(I).Power();
                    window.PowerValueList.Items.Add(IntToStr(power));
                }
            }

            var controlPower = control.GetPower();
            window.Power.Text = IntToStr(controlPower);
            var manaIncome = control.GetManaIncome();
            var manaUpkeep = control.GetManaUpkeep();
            var controlManaIncome = control.GetManaIncome();
            var controlManaUpkeep = control.GetManaUpkeep();
            window.NetMana.Text = IntToStr(manaIncome) + "-" + IntToStr(manaUpkeep) + "=" + IntToStr(controlManaIncome - controlManaUpkeep);
            var netPowerToResearch = control.GetNetPowerToResearch();
            window.ResearchPoints.Text = IntToStr(netPowerToResearch);
            if (control.ResearchSpellId != 0)
            {
                spell = AoWHSSet.Current.SpellControl.GetSpell(control.ResearchSpellId);
                window.SpellName.Text = spell.SpellName;
                var magicSphere = AbilityRules.TranslateResourceString(GameTables.s_magicSphereTexts[(int)spell.MagicSphere]);
                var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                window.SpellClass.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                window.SpellCost.Text = castingCost + ": " + IntToStr(spell.BaseManaCost) + " " + mana;
                turns = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.GetResearchTurnsLeft();
                if (turns < 999)
                {
                    var formatTurnsLeftString = DurationText.FormatTurnsLeftString(turns);
                    window.ResearchTurnsLeft.Text = formatTurnsLeftString;
                }
                else
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.TurnsFormat), [(byte)63]);
                    window.ResearchTurnsLeft.Text = format;
                }

                window.SpellPnl.Visible = true;
            }
            else
            {
                window.SpellPnl.Visible = false;
            }

            window.SortEnchantments();
            window.SelectEnchantment(window.EnchantmentList.Selected);
        }

        window.Dlg.Drawn = false;
    }

    internal static void RefreshGlobalMagic(MagicWindow window)
    {
        GlobalMagicControl control;
        control = AoWHSMap.Current.GlobalMagicControl;
        window.GlobalEnchantmentList.Items.Clear();
        window.GlobalNameList.Items.Clear();
        window.GlobalUpkeepList.Items.Clear();
        for (int I = 0, last = control.Enchantments.Count - 1; I <= last; I++)
        {
            if (control.Enchantments.GetEnchantments(I).VisibleForPlayer(AoWHSMap.Current.SeatedPlayer))
            {
                var name = control.Enchantments.GetEnchantments(I).GetName();
                var enchantment = control.Enchantments.GetEnchantments(I);
                window.GlobalEnchantmentList.Items.AddObject(name, enchantment);
                var enchantmentPlayerIndex = control.Enchantments.GetEnchantments(I);
                var player = AoWHSMap.Current.Players.GetPlayers(enchantmentPlayerIndex.PlayerIndex);
                window.GlobalNameList.Items.Add(player.Leader.HeroName);
                var upkeep = control.Enchantments.GetEnchantments(I).GetUpkeep();
                window.GlobalUpkeepList.Items.Add(IntToStr(upkeep));
            }
        }

        window.SortGlobalEnchantments();
        window.SelectGlobalEnchantment(window.GlobalEnchantmentList.Selected);
        window.Dlg.Drawn = false;
    }

    internal static void ShowMagicEvent(MagicWindow window, PlayerMagicEventLog eventValue, ExecuteDoneEvent done)
    {
        TaskBar.ShowMagicWindow(window);
        if (done != null)
        {
            done(eventValue);
        }
    }

}
