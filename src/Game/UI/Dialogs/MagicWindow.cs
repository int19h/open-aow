#nullable disable warnings
// Translated from AoWMagic.pas.
using System.Globalization;
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

namespace Aow.Game.UI.Dialogs;

// enchantment sort, global sort, power sort
public sealed partial class MagicWindow(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label ResearchPointsLabel { get; private set; }
    public ListBox<Enchantment> EnchantmentList { get; private set; }
    public ListBox UpkeepList { get; private set; }
    public ListBox BindingList { get; private set; }
    public Label NetManaLabel { get; private set; }
    public ListBox<GlobalEnchantment> GlobalEnchantmentList { get; private set; }
    public ListBox GlobalUpkeepList { get; private set; }
    public ListBox GlobalNameList { get; private set; }
    public Label SpellNameLabel { get; private set; }
    public Button GlobalDispelButton { get; private set; }
    public Button DispelButton { get; private set; }
    public Label ResearchTurnsLeftLabel { get; private set; }
    public Label SpellClassLabel { get; private set; }
    public Panel SpellPanel { get; private set; }
    public Label SpellCostLabel { get; private set; }
    public Label PowerLabel { get; private set; }
    public ListBox PowerValueList { get; private set; }
    public ListBox<PowerSource> PowerSourceList { get; private set; }
    public VerticalScrollBar EnchantmentScrollBar { get; private set; }
    public VerticalScrollBar GlobalEnchantmentScrollBar { get; private set; }
    public VerticalScrollBar PowerScrollBar { get; private set; }

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
        if (index >= GlobalEnchantmentList.Items.Count)
        {
            index = GlobalEnchantmentList.Items.Count - 1;
        }

        GlobalEnchantmentList.Selected = index;
        GlobalNameList.Selected = index;
        GlobalUpkeepList.Selected = index;
        if (index != -1)
        {
            GlobalEnchantment? enchantment = GlobalEnchantmentList.Items.Objects[index];
            string reason = "";
            bool canRemove = enchantment.CanRemove(StrategicMap.Current.SeatedPlayer, ref reason);
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
                EnchantmentList.Items.SortStringsAscending(linkedStrings: [BindingList.Items, UpkeepList.Items]);
                break;
            case 1:
                EnchantmentList.Items.SortStringsDescending(linkedStrings: [BindingList.Items, UpkeepList.Items]);
                break;
            case 2:
                BindingList.Items.SortStringsAscending(linkedStrings: [EnchantmentList.Items, UpkeepList.Items]);
                break;
            case 3:
                BindingList.Items.SortStringsDescending(linkedStrings: [EnchantmentList.Items, UpkeepList.Items]);
                break;
            case 4:
                UpkeepList.Items.SortStringsNumericalAscending(linkedStrings: [EnchantmentList.Items, BindingList.Items]);
                break;
            case 5:
                UpkeepList.Items.SortStringsNumericalDescending(linkedStrings: [EnchantmentList.Items, BindingList.Items]);
                break;
            default:
                break;
        }
    }

    // AoWMagic.pas:179
    public void SortGlobalEnchantments()
    {
        switch (_globalEnchantmentSort)
        {
            case 0:
                GlobalEnchantmentList.Items.SortStringsAscending(linkedStrings: [GlobalNameList.Items, GlobalUpkeepList.Items]);
                break;
            case 1:
                GlobalEnchantmentList.Items.SortStringsDescending(linkedStrings: [GlobalNameList.Items, GlobalUpkeepList.Items]);
                break;
            case 2:
                GlobalNameList.Items.SortStringsAscending(linkedStrings: [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                break;
            case 3:
                GlobalNameList.Items.SortStringsDescending(linkedStrings: [GlobalEnchantmentList.Items, GlobalUpkeepList.Items]);
                break;
            case 4:
                GlobalUpkeepList.Items.SortStringsNumericalAscending(linkedStrings: [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                break;
            case 5:
                GlobalUpkeepList.Items.SortStringsNumericalDescending(linkedStrings: [GlobalEnchantmentList.Items, GlobalNameList.Items]);
                break;
            default:
                break;
        }
    }

    // AoWMagic.pas:191
    public void SortPowerSources()
    {
        switch (_powerSort)
        {
            case 0:
                PowerSourceList.Items.SortStringsAscending(linkedStrings: [PowerValueList.Items]);
                break;
            case 1:
                PowerSourceList.Items.SortStringsDescending(linkedStrings: [PowerValueList.Items]);
                break;
            case 2:
                PowerValueList.Items.SortStringsNumericalAscending(linkedStrings: [PowerSourceList.Items]);
                break;
            case 3:
                PowerValueList.Items.SortStringsNumericalDescending(linkedStrings: [PowerSourceList.Items]);
                break;
            default:
                break;
        }
    }

    // AoWMagic.pas:203
    public void EnchantmentListDoubleClick()
    {
        bool condition = EnchantmentList.Selected >= 0;
        if (condition)
        {
            int count = EnchantmentList.Items.Count;
            condition = EnchantmentList.Selected < count;
        }

        if (condition)
        {
            EnchantmentList.Items.Objects[EnchantmentList.Selected].Select();
        }
    }

    // AoWMagic.pas:210
    public void EnchantmentListMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (button == MouseButton.Right)
        {
            bool condition = EnchantmentList.Selected >= 0;
            if (condition)
            {
                int count = EnchantmentList.Items.Count;
                condition = EnchantmentList.Selected < count;
            }

            if (condition)
            {
                _ = EnchantmentList.Items.Objects[EnchantmentList.Selected].CenterToLocation();
            }
        }
    }

    // AoWMagic.pas:218
    public void DispelBtnClick()
    {
        UI.General.PlayButtonSound(0);
        bool condition = EnchantmentList.Selected >= 0;
        if (condition)
        {
            int count = EnchantmentList.Items.Count;
            condition = EnchantmentList.Selected < count;
        }

        if (condition)
        {
            Enchantment? enchantment = EnchantmentList.Items.Objects[EnchantmentList.Selected];
            _ = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.DispelEnchantment(enchantment);
        }
    }

    // AoWMagic.pas:227
    public void EnchantmentListChange() => SelectEnchantment(EnchantmentList.Selected);

    // AoWMagic.pas:233
    public void BindingListChange() => SelectEnchantment(BindingList.Selected);

    // AoWMagic.pas:239
    public void UpkeepListChange() => SelectEnchantment(UpkeepList.Selected);

    // AoWMagic.pas:245
    public void GlobalDispelBtnClick()
    {
        UI.General.PlayButtonSound(0);
        bool condition = GlobalEnchantmentList.Selected >= 0;
        if (condition)
        {
            int count = GlobalEnchantmentList.Items.Count;
            condition = GlobalEnchantmentList.Selected < count;
        }

        if (condition)
        {
            GlobalEnchantment? globalEnchantment = GlobalEnchantmentList.Items.Objects[GlobalEnchantmentList.Selected];
            _ = StrategicMap.Current.GlobalMagicControl.RemoveEnchantment(StrategicMap.Current.SeatedPlayer, enchantment: globalEnchantment);
        }
    }

    // AoWMagic.pas:254
    public void GlobalEnchantmentListChange() => SelectGlobalEnchantment(GlobalEnchantmentList.Selected);

    // AoWMagic.pas:260
    public void GlobalNameListChange() => SelectGlobalEnchantment(GlobalNameList.Selected);

    // AoWMagic.pas:266
    public void GlobalUpkeepListChange() => SelectGlobalEnchantment(GlobalUpkeepList.Selected);

    // AoWMagic.pas:272
    public void PowerSourceListChange() => SelectPowerSource(PowerSourceList.Selected);

    // AoWMagic.pas:278
    public void PowerValueListChange() => SelectPowerSource(PowerValueList.Selected);

    // AoWMagic.pas:284
    public void ResearchBtnClick()
    {
        UI.General.PlayButtonSound(0);
        UI.SpellBook.ShowResearch();
    }

    // AoWMagic.pas:290
    public void ChangePowerButClick()
    {
        UI.General.PlayButtonSound(0);
        UI.PowerDialog.Execute();
    }

    // AoWMagic.pas:296
    public void SpellIcnDraw(Image sender, LockedSurface surface)
    {
        PlayerMagicControl magic = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
        if (magic.ResearchSpellId != 0)
        {
            Spell? spell = GamePack.Current.SpellControl[magic.ResearchSpellId];
            if (spell is not null)
            {
                if (spell.CastImages.GetImageSequence(10) is not null)
                {
                    int x = sender.Window.AbsoluteLeft;
                    int y = sender.Window.AbsoluteTop + sender.WinTop;
                    _ = spell.CastImages.Show(10, sender.WinLeft + x, y, frame: 0, lockedSurface: surface);
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
            int count = PowerSourceList.Items.Count;
            condition = PowerSourceList.Selected < count;
        }

        if (condition)
        {
            PowerSourceList.Items.Objects[PowerSourceList.Selected].Select();
        }
    }

    // AoWMagic.pas:323
    public void PowerValueListMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (button == MouseButton.Right)
        {
            if (PowerSourceList.Selected >= 0)
            {
                int count = PowerSourceList.Items.Count;
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
        UI.General.PlayButtonSound(0);
        _enchantmentSort = _enchantmentSort == 0 ? 1 : 0;

        SortEnchantments();
    }

    // AoWMagic.pas:363
    public void BindingSortBtnClick()
    {
        UI.General.PlayButtonSound(0);
        _enchantmentSort = _enchantmentSort == 2 ? 3 : 2;

        SortEnchantments();
    }

    // AoWMagic.pas:372
    public void UpkeepSortBtnClick()
    {
        UI.General.PlayButtonSound(0);
        _enchantmentSort = _enchantmentSort == 4 ? 5 : 4;

        SortEnchantments();
    }

    // AoWMagic.pas:381
    public void GlobalEnchantmentSortBtnClick()
    {
        UI.General.PlayButtonSound(0);
        _globalEnchantmentSort = _globalEnchantmentSort == 0 ? 1 : 0;

        SortGlobalEnchantments();
    }

    // AoWMagic.pas:390
    public void GlobalNameSortBtnClick()
    {
        UI.General.PlayButtonSound(0);
        // { Original name/upkeep buttons also toggle the enchantment-name order. }
        _globalEnchantmentSort = _globalEnchantmentSort == 0 ? 1 : 0;

        SortGlobalEnchantments();
    }

    // AoWMagic.pas:400
    public void GlobalUpkeepSortBtnClick()
    {
        UI.General.PlayButtonSound(0);
        // { Original name/upkeep buttons also toggle the enchantment-name order. }
        _globalEnchantmentSort = _globalEnchantmentSort == 0 ? 1 : 0;

        SortGlobalEnchantments();
    }

    // AoWMagic.pas:410
    public void PowerSourceSortBtnClick()
    {
        UI.General.PlayButtonSound(0);
        _powerSort = _powerSort == 0 ? 1 : 0;

        SortPowerSources();
    }

    // AoWMagic.pas:419
    public void PowerValueSortBtnClick()
    {
        UI.General.PlayButtonSound(0);
        _powerSort = _powerSort == 2 ? 3 : 2;

        SortPowerSources();
    }

    // AoWMagic.pas:428
    public void HeaderTabChange() => UI.General.PlayButtonSound(0);

    // AoWMagic.pas:434
    public void DlgResize()
    {
        Window.Alignment.AlignWidth = AlignWidth.Left;
        Window.Alignment.AlignHeight = AlignHeight.Top;
        Window.Alignment.LeftOffset = Window.WinLeft;
        Window.Alignment.TopOffset = Window.WinTop;
    }

    // AoWMagic.pas:447
    public void DlgShow() => UI.General.RegisterDialog(Window);

    // AoWMagic.pas:453
    public void DlgHide() => UI.General.UnregisterDialog(Window);

    // AoWMagic.pas:459
    public void HelpClick() => UI.HelpWindow.ShowHelp(506);

    // AoWMagic.pas:470
    public void MinimizeClick()
    {
        UI.General.PlayButtonSound(0);
        HideMagic(this);
    }

    // AoWMagic.pas:462
    public void GlobalEnchantmentMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (button == MouseButton.Right)
        {
            bool condition = GlobalEnchantmentList.Selected >= 0;
            if (condition)
            {
                int count = GlobalEnchantmentList.Items.Count;
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
        if (StrategicMap.Current.SeatedPlayer != -1)
        {
            PlayerMagicControl control = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl;
            window.EnchantmentList.Items.Clear();
            window.BindingList.Items.Clear();
            window.UpkeepList.Items.Clear();
            for (int i = 0, last = control.Enchantments.Count - 1; i <= last; i++)
            {
                string name = control.Enchantments.GetEnchantments(i).GetName();
                Enchantment enchantment = control.Enchantments.GetEnchantments(i);
                _ = window.EnchantmentList.Items.AddObject(name, item: enchantment);
                string binding = control.Enchantments.GetEnchantments(i).GetBinding();
                _ = window.BindingList.Items.Add(binding);
                Enchantment enchantmentUpkeep = control.Enchantments.GetEnchantments(i);
                _ = window.UpkeepList.Items.Add(enchantmentUpkeep.Upkeep.ToString(CultureInfo.InvariantCulture));
            }

            window.PowerSourceList.Items.Clear();
            window.PowerValueList.Items.Clear();
            for (int i = 0, last = control.PowerSources.Count - 1; i <= last; i++)
            {
                if (control.PowerSources.GetItems(i).Power() > 0)
                {
                    string name = control.PowerSources.GetItems(i).Name();
                    PowerSource powerSource = control.PowerSources.GetItems(i);
                    _ = window.PowerSourceList.Items.AddObject(name, item: powerSource);
                    int power = control.PowerSources.GetItems(i).Power();
                    _ = window.PowerValueList.Items.Add(power.ToString(CultureInfo.InvariantCulture));
                }
            }

            int controlPower = control.GetPower();
            window.PowerLabel.Text = controlPower.ToString(CultureInfo.InvariantCulture);
            int manaIncome = control.GetManaIncome();
            int manaUpkeep = control.GetManaUpkeep();
            int controlManaIncome = control.GetManaIncome();
            int controlManaUpkeep = control.GetManaUpkeep();
            window.NetManaLabel.Text = $"{manaIncome.ToString(CultureInfo.InvariantCulture)}-{manaUpkeep.ToString(CultureInfo.InvariantCulture)}={(controlManaIncome - controlManaUpkeep).ToString(CultureInfo.InvariantCulture)}";
            int netPowerToResearch = control.GetNetPowerToResearch();
            window.ResearchPointsLabel.Text = netPowerToResearch.ToString(CultureInfo.InvariantCulture);
            if (control.ResearchSpellId != 0)
            {
                Spell? spell = GamePack.Current.SpellControl[control.ResearchSpellId];
                window.SpellNameLabel.Text = spell.SpellName;
                string magicSphere = window.UI.Engine.TranslateResourceString(GameTables.MagicSphereTexts[(int)spell.MagicSphere]);
                string level = window.UI.Engine.TranslateResourceString(GameText.LevelText);
                window.SpellClassLabel.Text = $"{magicSphere}, {level} {spell.SpellLevel.ToString(CultureInfo.InvariantCulture)}";
                string castingCost = window.UI.Engine.TranslateResourceString(GameText.CastingCostText);
                string mana = window.UI.Engine.TranslateResourceString(GameText.ManaText);
                window.SpellCostLabel.Text = $"{castingCost}: {spell.BaseManaCost.ToString(CultureInfo.InvariantCulture)} {mana}";
                int turns = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).MagicControl.GetResearchTurnsLeft();
                if (turns < 999)
                {
                    string formatTurnsLeftString = DurationText.FormatTurnsLeftString(window.UI.Engine, turns: turns);
                    window.ResearchTurnsLeftLabel.Text = formatTurnsLeftString;
                }
                else
                {
                    string format = AbilityRules.Format(window.UI.Engine.TranslateResourceString(GameText.TurnsFormat), arguments: [(byte)63]);
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

        window.Window.IsDrawn = false;
    }

    internal static void RefreshGlobalMagic(MagicWindow window)
    {
        GlobalMagicControl? control = StrategicMap.Current.GlobalMagicControl;
        window.GlobalEnchantmentList.Items.Clear();
        window.GlobalNameList.Items.Clear();
        window.GlobalUpkeepList.Items.Clear();
        for (int i = 0, last = control.Enchantments.Count - 1; i <= last; i++)
        {
            if (control.Enchantments.GetEnchantments(i).VisibleForPlayer(StrategicMap.Current.SeatedPlayer))
            {
                string name = control.Enchantments.GetEnchantments(i).GetName();
                GlobalEnchantment enchantment = control.Enchantments.GetEnchantments(i);
                _ = window.GlobalEnchantmentList.Items.AddObject(name, item: enchantment);
                GlobalEnchantment enchantmentPlayerIndex = control.Enchantments.GetEnchantments(i);
                Player? player = StrategicMap.Current.Players.GetPlayers(enchantmentPlayerIndex.PlayerIndex);
                _ = window.GlobalNameList.Items.Add(player.Leader.HeroName);
                int upkeep = control.Enchantments.GetEnchantments(i).GetUpkeep();
                _ = window.GlobalUpkeepList.Items.Add(upkeep.ToString(CultureInfo.InvariantCulture));
            }
        }

        window.SortGlobalEnchantments();
        window.SelectGlobalEnchantment(window.GlobalEnchantmentList.Selected);
        window.Window.IsDrawn = false;
    }

    internal static void ShowMagicEvent(MagicWindow window, PlayerMagicEventLog eventValue, ExecuteDoneEvent done)
    {
        TaskBar.ShowMagicWindow(window);
        done?.Invoke(eventValue);
    }
}
