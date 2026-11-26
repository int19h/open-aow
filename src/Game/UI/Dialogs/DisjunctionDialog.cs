#nullable disable warnings
// Translated from AoWDisjunctionDlg.pas.
using System.Globalization;
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Graphics;
using Aow.Rtl;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// caster, sort order, map location
public sealed partial class DisjunctionDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label DisjunctionChanceLabel { get; private set; }
    public Label DisjunctionManaLabel { get; private set; }
    public ListBox OwnerList { get; private set; }
    public ListBox EnchantmentList { get; private set; }
    public VerticalScrollBar EnchantmentsScrollBar { get; private set; }
    public HorizontalScrollBar ChanceSlider { get; private set; }
    public UserDrawBox MapView { get; private set; }

    private DisjunctionSpellCaster? _caster;
    private int _sortOrder;

    private MapCoordinate Location { get; set; }

    // AoWDisjunctionDlg.pas:179
    public void ClearCaster()
    {
        UI.General.Manager.OverlayPriority = 0;
        _ = (_caster?.Release());
        _caster = null;
    }

    // AoWDisjunctionDlg.pas:189
    public void UpdateEnchantments()
    {
        string reason = "";
        GlobalMagicControl? magic = UI.Engine.StrategicMap.GlobalMagicControl;
        EnchantmentList.Items.Clear();
        OwnerList.Items.Clear();
        for (int i = 0, last = magic.Enchantments.Count - 1; i <= last; i++)
        {
            StrategicMap strategicMap = UI.Engine.StrategicMap;
            if (magic.Enchantments.GetEnchantments(i).PlayerIndex != strategicMap.SeatedPlayer)
            {
                if (magic.Enchantments.GetEnchantments(i).VisibleForPlayer(strategicMap.SeatedPlayer))
                {
                    if (magic.Enchantments.GetEnchantments(i).CanDispel(UI.Engine.StrategicMap.SeatedPlayer, ref reason))
                    {
                        reason = magic.Enchantments.GetEnchantments(i).GetName();
                        string name = magic.Enchantments.GetEnchantments(i).GetName();
                        GlobalEnchantment enchantmentEnchantmentId = magic.Enchantments.GetEnchantments(i);
                        _ = EnchantmentList.Items.AddObject(name, item: IntToObject(enchantmentEnchantmentId.EnchantmentId));
                        GlobalEnchantment enchantmentPlayerIndex = magic.Enchantments.GetEnchantments(i);
                        Player? player = UI.Engine.StrategicMap.Players.GetPlayers(enchantmentPlayerIndex.PlayerIndex);
                        _ = OwnerList.Items.Add(player.Leader.HeroName);
                    }
                }
            }
        }

        SortEnchantments();
        UpdateChance();
    }

    // AoWDisjunctionDlg.pas:212
    public void SortEnchantments()
    {
        switch (_sortOrder)
        {
            case 0:
                EnchantmentList.Items.SortStringsAscending(linkedStrings: [OwnerList.Items]);
                break;
            case 1:
                EnchantmentList.Items.SortStringsDescending(linkedStrings: [OwnerList.Items]);
                break;
            case 2:
                OwnerList.Items.SortStringsAscending(linkedStrings: [EnchantmentList.Items]);
                break;
            case 3:
                OwnerList.Items.SortStringsDescending(linkedStrings: [EnchantmentList.Items]);
                break;
            default:
                break;
        }
    }

    // AoWDisjunctionDlg.pas:222
    public void UpdateChance()
    {
        DisjunctionManaLabel.TextColor = _caster.GetEnoughMana() ? Color.Silver : Color.Red;

        Spell? spell = UI.Engine.GamePack.SpellControl[51];
        int mana = _caster.GetMana();
        Spell? fSpellControlSpell = UI.Engine.GamePack.SpellControl[51];
        int fCasterMana = _caster.GetMana();
        DisjunctionManaLabel.Text = $"{spell.BaseManaCost.ToString(CultureInfo.InvariantCulture)}+{mana.ToString(CultureInfo.InvariantCulture)}={(fSpellControlSpell.BaseManaCost + fCasterMana).ToString(CultureInfo.InvariantCulture)}";
        DisjunctionChanceLabel.Text = $"{_caster.DispelChance.ToString(CultureInfo.InvariantCulture)}%";
        int dispelChanceMax = _caster.DispelChanceMax;
        ChanceSlider.Max = dispelChanceMax;
        ChanceSlider.Position = _caster.DispelChance;
    }

    // AoWDisjunctionDlg.pas:233
    public void SelectEnchantment(int index)
    {
        if (index >= 0 && index < EnchantmentList.Items.Count)
        {
            EnchantmentList.Selected = index;
            OwnerList.Selected = index;
            object? objectValue = EnchantmentList.Items.Objects[index];
            _caster.SetEnchantmentId(ObjectToInt(objectValue));
            GlobalEnchantment? enchantment = _caster.SelectedEnchantment();
            if (enchantment is not null)
            {
                MapCoordinate location = enchantment.GetLocation();
                Location = new MapCoordinate(location.X, location.Y, location.MapLevel);
            }
            else
            {
                Location = Location with { X = -1 };
            }

            if (Location.X == -1)
            {
                {
                    MapCoordinate coordinate41 = _caster.Source.GetLocation();
                    int coordinateX = coordinate41.X;
                    int coordinateY = coordinate41.Y;
                    int coordinateMapLevel = coordinate41.MapLevel;
                    Location = new MapCoordinate((sbyte)coordinateX, (sbyte)coordinateY, (sbyte)coordinateMapLevel);
                }
            }

            UpdateChance();
        }
        else
        {
            EnchantmentList.Selected = -1;
            OwnerList.Selected = -1;
        }
    }

    // AoWDisjunctionDlg.pas:262
    public void Execute(DisjunctionSpellCaster caster)
    {
        _ = (_caster?.Release());

        _caster = caster;
        _ = _caster.AddRef();
        UpdateEnchantments();
        SelectEnchantment(0);
        UI.General.Manager.OverlayPriority = 49;
        Window.Visible = true;
    }

    // AoWDisjunctionDlg.pas:76
    public void CancelBtnClick() => Window.Visible = false;

    // AoWDisjunctionDlg.pas:82
    public void CastBtnClick()
    {
        string reason = "";
        if (_caster.CanCast(ref reason))
        {
            if (_caster.Cast())
            {
                Window.Visible = false;
            }
        }
        else
        {
            UI.Engine.StrategicMap.ShowMessage(reason);
        }
    }

    // AoWDisjunctionDlg.pas:94
    public void EnchantmentListChange() => SelectEnchantment(EnchantmentList.Selected);

    // AoWDisjunctionDlg.pas:100
    public void OwnerListChange() => SelectEnchantment(OwnerList.Selected);

    // AoWDisjunctionDlg.pas:106
    public void ChanceSliderChange()
    {
        _caster.SetDispelChance(ChanceSlider.Position);
        UpdateChance();
    }

    // AoWDisjunctionDlg.pas:113
    public void EnchantmentsScrollBarChange()
    {
        EnchantmentList.ListOffset = EnchantmentsScrollBar.Position;
        OwnerList.ListOffset = EnchantmentsScrollBar.Position;
    }

    // AoWDisjunctionDlg.pas:120
    public void SortEnchantmentClick()
    {
        UI.General.PlayButtonSound(0);
        _sortOrder = _sortOrder == 0 ? 1 : 0;

        SortEnchantments();
    }

    // AoWDisjunctionDlg.pas:128
    public void SortClick()
    {
        UI.General.PlayButtonSound(0);
        _sortOrder = _sortOrder == 2 ? 3 : 2;

        SortEnchantments();
    }

    // AoWDisjunctionDlg.pas:136
    public void DlgHide() => ClearCaster();

    // AoWDisjunctionDlg.pas:142
    public void DlgKeyDown(in int key, ShiftStates shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    // AoWDisjunctionDlg.pas:148
    public void MapViewDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect r = new(Window.WinLeft + sender.WinLeft + 1, Window.WinTop + sender.WinTop + 1, Window.WinLeft + sender.WinLeft + sender.WinWidth - 2, Window.WinTop + sender.WinTop + sender.WinHeight - 2);
        int w = r.Right - r.Left;
        int h = r.Bottom - r.Top;
        HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
        x = x + 16 - (w >>> 1);
        y = y + 16 - (h >>> 1);
        if (x < 16)
        {
            x = 16;
        }

        if (y < 16)
        {
            y = 16;
        }

        LockedSurface mapSurfaceDescription = LockedSurface.CreateFrom(lockedSurface, r);
        _ = UI.Engine.StrategicMap.SetViewPort(x, y, lHitPoints: Location.MapLevel, r.Right - r.Left, r.Bottom - r.Top);
        UI.Engine.StrategicMap.UpdateStaticScene();
        UI.Engine.StrategicMap.UpdateDynamicScene();
        UI.Engine.StrategicMap.DrawStaticScene(in mapSurfaceDescription);
        UI.Engine.StrategicMap.DrawDynamicScene(ref mapSurfaceDescription);
    }

    // AoWDisjunctionDlg.pas:170
    public void MapViewUpdate()
    {
        UI.General.Manager.OverlayPriority = 49;
        MapView.IsDrawn = MapView.IsDrawn && (UI.Engine.StrategicMap.Frame & 1) != 0;
    }
}
