#nullable disable
// Translated from AoWDisjunctionDlg.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// caster, sort order, map location
public partial class DisjunctionDialog : UIModule
{
    public DisjunctionDialog(UIContext ui) : base(ui)
    { }

    public Window Window { get; private set; }
    public Label DisjunctionChanceLabel { get; private set; }
    public Label DisjunctionManaLabel { get; private set; }
    public ListBox OwnerList { get; private set; }
    public ListBox EnchantmentList { get; private set; }
    public VerticalScrollBar EnchantmentsScrollBar { get; private set; }
    public HorizontalScrollBar ChanceSlider { get; private set; }
    public UserDrawBox MapView { get; private set; }

    private DisjunctionSpellCaster _caster;
    private int _sortOrder;

    private MapCoordinate Location { get; set; }

    // AoWDisjunctionDlg.pas:179
    public void ClearCaster()
    {
        UI.General.Manager.OverlayPriority = 0;
        _caster?.Release();
        _caster = null;
    }

    // AoWDisjunctionDlg.pas:189
    public void UpdateEnchantments()
    {
        GlobalMagicControl magic;
        string reason = "";
        magic = StrategicMap.Current.GlobalMagicControl;
        EnchantmentList.Items.Clear();
        OwnerList.Items.Clear();
        for (int i = 0, last = magic.Enchantments.Count - 1; i <= last; i++)
        {
            if (magic.Enchantments.GetEnchantments(i).PlayerIndex != StrategicMap.Current.SeatedPlayer)
            {
                if (magic.Enchantments.GetEnchantments(i).VisibleForPlayer(StrategicMap.Current.SeatedPlayer))
                {
                    if (magic.Enchantments.GetEnchantments(i).CanDispel(StrategicMap.Current.SeatedPlayer, ref reason))
                    {
                        reason = magic.Enchantments.GetEnchantments(i).GetName();
                        string name = magic.Enchantments.GetEnchantments(i).GetName();
                        GlobalEnchantment enchantmentEnchantmentId = magic.Enchantments.GetEnchantments(i);
                        EnchantmentList.Items.AddObject(name, IntToObject(enchantmentEnchantmentId.EnchantmentId));
                        GlobalEnchantment enchantmentPlayerIndex = magic.Enchantments.GetEnchantments(i);
                        Player player = StrategicMap.Current.Players.GetPlayers(enchantmentPlayerIndex.PlayerIndex);
                        OwnerList.Items.Add(player.Leader.HeroName);
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
                StringSorting.SortStringsAscending(EnchantmentList.Items, [OwnerList.Items]);
                break;
            case 1:
                StringSorting.SortStringsDescending(EnchantmentList.Items, [OwnerList.Items]);
                break;
            case 2:
                StringSorting.SortStringsAscending(OwnerList.Items, [EnchantmentList.Items]);
                break;
            case 3:
                StringSorting.SortStringsDescending(OwnerList.Items, [EnchantmentList.Items]);
                break;
        }
    }

    // AoWDisjunctionDlg.pas:222
    public void UpdateChance()
    {
        DisjunctionManaLabel.TextColor = _caster.GetEnoughMana() ? 12632256 : 255;

        Spell spell = GamePack.Current.SpellControl.GetSpell(51);
        int mana = _caster.GetMana();
        Spell fSpellControlSpell = GamePack.Current.SpellControl.GetSpell(51);
        int fCasterMana = _caster.GetMana();
        DisjunctionManaLabel.Text = IntToStr(spell.BaseManaCost) + "+" + IntToStr(mana) + "=" + IntToStr(fSpellControlSpell.BaseManaCost + fCasterMana);
        DisjunctionChanceLabel.Text = IntToStr(_caster.DispelChance) + "%";
        int dispelChanceMax = _caster.GetDispelChanceMax();
        ChanceSlider.Max = dispelChanceMax;
        ChanceSlider.Position = _caster.DispelChance;
    }

    // AoWDisjunctionDlg.pas:233
    public void SelectEnchantment(int index)
    {
        GlobalEnchantment enchantment;
        MapCoordinate location;
        if (index >= 0 && index < EnchantmentList.Items.Count)
        {
            EnchantmentList.Selected = index;
            OwnerList.Selected = index;
            object objectValue = EnchantmentList.Items.Objects[index];
            _caster.SetEnchantmentId(ObjectToInt(objectValue));
            enchantment = _caster.SelectedEnchantment();
            if (enchantment != null)
            {
                location = enchantment.GetLocation();
                Location = new MapCoordinate(location.X, location.Y, location.MapLevel);
            }
            else
            {
                Location = Location with { X = -1 };
            }

            if (Location.X == -1)
            {
                {
                    MapCoordinate coordinate41 = _caster.GetSource().GetLocation();
                    sbyte coordinateX = coordinate41.X;
                    sbyte coordinateY = coordinate41.Y;
                    sbyte coordinateMapLevel = coordinate41.MapLevel;
                    Location = new MapCoordinate(coordinateX, coordinateY, coordinateMapLevel);
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
        _caster?.Release();

        _caster = caster;
        _caster.AddRef();
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
            StrategicMap.Current.ShowMessage(reason);
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
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    // AoWDisjunctionDlg.pas:148
    public void MapViewDraw(UIComponent sender, LockedSurface lockedSurface)
    {
        Rect r;
        int w;
        int h;
        r = new Rect(Window.WinLeft + sender.WinLeft + 1, Window.WinTop + sender.WinTop + 1, Window.WinLeft + sender.WinLeft + sender.WinWidth - 2, Window.WinTop + sender.WinTop + sender.WinHeight - 2);
        w = r.Right - r.Left;
        h = r.Bottom - r.Top;
        HexGeometry.HexToPixel(Location.X, Location.Y, out int x, out int y);
        x = x + 16 - (int)(((uint)w) >> 1);
        y = y + 16 - (int)(((uint)h) >> 1);
        if (x < 16)
        {
            x = 16;
        }

        if (y < 16)
        {
            y = 16;
        }

        LockedSurface.CreateFrom(lockedSurface, r, out LockedSurface mapSurfaceDescription);
        StrategicMap.Current.SetViewPort(x, y, Location.MapLevel, r.Right - r.Left, r.Bottom - r.Top);
        StrategicMap.Current.UpdateStaticScene();
        StrategicMap.Current.UpdateDynamicScene();
        StrategicMap.Current.DrawStaticScene(ref mapSurfaceDescription);
        StrategicMap.Current.DrawDynamicScene(ref mapSurfaceDescription);
    }

    // AoWDisjunctionDlg.pas:170
    public void MapViewUpdate()
    {
        UI.General.Manager.OverlayPriority = 49;
        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
    }
}
