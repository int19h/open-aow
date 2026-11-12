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

    public Window Window;
    public Label ResearchPointsLabel;
    public ListBox<Enchantment> EnchantmentList;
    public ListBox UpkeepList;
    public ListBox BindingList;
    public Label NetManaLabel;
    public ListBox<GlobalEnchantment> GlobalEnchantmentList;
    public ListBox GlobalUpkeepList;
    public ListBox GlobalNameList;
    public Label SpellNameLabel;
    public Button GlobalDispelButton;
    public Button DispelButton;
    public Label ResearchTurnsLeftLabel;
    public Label SpellClassLabel;
    public Panel SpellPanel;
    public Label SpellCostLabel;
    public Label PowerLabel;
    public ListBox PowerValueList;
    public ListBox<PowerSource> PowerSourceList;
    public VerticalScrollBar EnchantmentScrollBar;
    public VerticalScrollBar GlobalEnchantmentScrollBar;
    public VerticalScrollBar PowerScrollBar;
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
        DispelButton.Visible = index != -1;
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
            enchantment = GlobalEnchantmentList.Items.Objects[index];
            var canRemove = enchantment.CanRemove(AoWHSMap.Current.SeatedPlayer, ref reason);
            GlobalDispelButton.Visible = canRemove;
        }
        else
        {
            GlobalDispelButton.Visible = false;
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
            EnchantmentList.Items.Objects[EnchantmentList.Selected].Select();
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
                EnchantmentList.Items.Objects[EnchantmentList.Selected].CenterToLocation();
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
            AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.DispelEnchantment(enchantment);
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
            AoWHSMap.Current.GlobalMagicControl.RemoveEnchantment(AoWHSMap.Current.SeatedPlayer, globalEnchantment);
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
            PowerSourceList.Items.Objects[PowerSourceList.Selected].Select();
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
                    PowerSourceList.Items.Objects[PowerSourceList.Selected].CenterToLocation();
                }
            }
        }
    }

    // AoWMagic.pas:331
    public void PowerSBChange()
    {
        PowerSourceList.ListOffset = PowerScrollBar.Position;
        PowerValueList.ListOffset = PowerScrollBar.Position;
    }

    // AoWMagic.pas:338
    public void GlobalEnchantmentSBChange()
    {
        GlobalEnchantmentList.ListOffset = GlobalEnchantmentScrollBar.Position;
        GlobalNameList.ListOffset = GlobalEnchantmentScrollBar.Position;
        GlobalUpkeepList.ListOffset = GlobalEnchantmentScrollBar.Position;
    }

    // AoWMagic.pas:346
    public void EnchantmentSBChange()
    {
        EnchantmentList.ListOffset = EnchantmentScrollBar.Position;
        BindingList.ListOffset = EnchantmentScrollBar.Position;
        UpkeepList.ListOffset = EnchantmentScrollBar.Position;
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
        this.Window.Alignment.AlignWidth = AlignWidth.Left;
        this.Window.Alignment.AlignHeight = AlignHeight.Top;
        this.Window.Alignment.LeftOffset = this.Window.WinLeft;
        this.Window.Alignment.TopOffset = this.Window.WinTop;
    }

    // AoWMagic.pas:447
    public void DlgShow()
    {
        DialogHosts.General.RegisterDialog(Window);
    }

    // AoWMagic.pas:453
    public void DlgHide()
    {
        DialogHosts.General.UnregisterDialog(Window);
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
                GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected].CenterToLocation();
            }
        }
    }

    internal static void ShowMagic(MagicWindow window)
    {
        if (!window.Window.Visible)
        {
            RefreshMagicWindow(window);
            RefreshGlobalMagic(window);
        }

        window.Window.Visible = true;
    }

    internal static void HideMagic(MagicWindow window)
    {
        if (window.Window.Visible)
        {
            window.Window.Visible = false;
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
            window.PowerLabel.Text = IntToStr(controlPower);
            var manaIncome = control.GetManaIncome();
            var manaUpkeep = control.GetManaUpkeep();
            var controlManaIncome = control.GetManaIncome();
            var controlManaUpkeep = control.GetManaUpkeep();
            window.NetManaLabel.Text = IntToStr(manaIncome) + "-" + IntToStr(manaUpkeep) + "=" + IntToStr(controlManaIncome - controlManaUpkeep);
            var netPowerToResearch = control.GetNetPowerToResearch();
            window.ResearchPointsLabel.Text = IntToStr(netPowerToResearch);
            if (control.ResearchSpellId != 0)
            {
                spell = AoWHSSet.Current.SpellControl.GetSpell(control.ResearchSpellId);
                window.SpellNameLabel.Text = spell.SpellName;
                var magicSphere = AbilityRules.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                var level = AbilityRules.TranslateResourceString(GameText.LevelText);
                window.SpellClassLabel.Text = magicSphere + ", " + level + " " + IntToStr(spell.SpellLevel);
                var castingCost = AbilityRules.TranslateResourceString(GameText.CastingCostText);
                var mana = AbilityRules.TranslateResourceString(GameText.ManaText);
                window.SpellCostLabel.Text = castingCost + ": " + IntToStr(spell.BaseManaCost) + " " + mana;
                turns = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).MagicControl.GetResearchTurnsLeft();
                if (turns < 999)
                {
                    var formatTurnsLeftString = DurationText.FormatTurnsLeftString(turns);
                    window.ResearchTurnsLeftLabel.Text = formatTurnsLeftString;
                }
                else
                {
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.TurnsFormat), [(byte)63]);
                    window.ResearchTurnsLeftLabel.Text = format;
                }

                window.SpellPanel.Visible = true;
            }
            else
            {
                window.SpellPanel.Visible = false;
            }

            window.SortEnchantments();
            window.SelectEnchantment(window.EnchantmentList.Selected);
        }

        window.Window.Drawn = false;
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
        window.Window.Drawn = false;
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
