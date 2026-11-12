// Translated from AoWDisjunctionDlg.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// caster, sort order, map location
public partial class DisjunctionDialog : DataModule
{
    public DisjunctionDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label DisjunctionChanceLabel;
    public Label DisjunctionManaLabel;
    public ListBox OwnerList;
    public ListBox EnchantmentList;
    public VerticalScrollBar EnchantmentsScrollBar;
    public HorizontalScrollBar ChanceSlider;
    public UserDrawBox MapView;
    private DisjunctionSpellCaster _caster;
    private int _sortOrder;
    private sbyte _mapX;
    private sbyte _mapY;
    private sbyte _mapL;

    // AoWDisjunctionDlg.pas:179
    public void ClearCaster()
    {
        DialogHosts.General.Manager.OverlayPriority = 0;
        if (_caster != null)
        {
            _caster.Release();
            _caster = null;
        }
    }

    // AoWDisjunctionDlg.pas:189
    public void UpdateEnchantments()
    {
        GlobalMagicControl magic;
        string reason = "";
        magic = StrategicMap.Current.GlobalMagicControl;
        EnchantmentList.Items.Clear();
        OwnerList.Items.Clear();
        for (int I = 0, last = magic.Enchantments.Count - 1; I <= last; I++)
        {
            if (magic.Enchantments.GetEnchantments(I).PlayerIndex != StrategicMap.Current.SeatedPlayer)
            {
                if (magic.Enchantments.GetEnchantments(I).VisibleForPlayer(StrategicMap.Current.SeatedPlayer))
                {
                    if (magic.Enchantments.GetEnchantments(I).CanDispel(StrategicMap.Current.SeatedPlayer, ref reason))
                    {
                        reason = magic.Enchantments.GetEnchantments(I).GetName();
                        var name = magic.Enchantments.GetEnchantments(I).GetName();
                        var enchantmentEnchantmentId = magic.Enchantments.GetEnchantments(I);
                        EnchantmentList.Items.AddObject(name, IntToObject(enchantmentEnchantmentId._enchantmentId));
                        var enchantmentPlayerIndex = magic.Enchantments.GetEnchantments(I);
                        var player = StrategicMap.Current.Players.GetPlayers(enchantmentPlayerIndex.PlayerIndex);
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
        if (_caster.GetEnoughMana())
        {
            DisjunctionManaLabel.TextColor = 12632256;
        }
        else
        {
            DisjunctionManaLabel.TextColor = 255;
        }

        var spell = GamePack.Current.SpellControl.GetSpell(51);
        var mana = _caster.GetMana();
        var fSpellControlSpell = GamePack.Current.SpellControl.GetSpell(51);
        var fCasterMana = _caster.GetMana();
        DisjunctionManaLabel.Text = IntToStr(spell.BaseManaCost) + "+" + IntToStr(mana) + "=" + IntToStr(fSpellControlSpell.BaseManaCost + fCasterMana);
        DisjunctionChanceLabel.Text = IntToStr(_caster.DispelChance) + "%";
        var dispelChanceMax = _caster.GetDispelChanceMax();
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
            var objectValue = EnchantmentList.Items.Objects[index];
            _caster.SetEnchantmentId(ObjectToInt(objectValue));
            enchantment = _caster.SelectedEnchantment();
            if (enchantment != null)
            {
                location = enchantment.GetLocation();
                _mapX = location.X;
                _mapY = location.Y;
                _mapL = location.MapLevel;
            }
            else
            {
                _mapX = -1;
            }

            if (_mapX == -1)
            {
                _caster.GetSource().GetLocation(ref _mapX, ref _mapY, ref _mapL);
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
        if (_caster != null)
        {
            _caster.Release();
        }

        _caster = caster;
        _caster.AddRef();
        UpdateEnchantments();
        SelectEnchantment(0);
        DialogHosts.General.Manager.OverlayPriority = 49;
        Window.Visible = true;
    }

    // AoWDisjunctionDlg.pas:76
    public void CancelBtnClick()
    {
        Window.Visible = false;
    }

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
    public void EnchantmentListChange()
    {
        SelectEnchantment(EnchantmentList.Selected);
    }

    // AoWDisjunctionDlg.pas:100
    public void OwnerListChange()
    {
        SelectEnchantment(OwnerList.Selected);
    }

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
        DialogHosts.General.PlayButtonSound(0);
        if (_sortOrder == 0)
        {
            _sortOrder = 1;
        }
        else
        {
            _sortOrder = 0;
        }

        SortEnchantments();
    }

    // AoWDisjunctionDlg.pas:128
    public void SortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_sortOrder == 2)
        {
            _sortOrder = 3;
        }
        else
        {
            _sortOrder = 2;
        }

        SortEnchantments();
    }

    // AoWDisjunctionDlg.pas:136
    public void DlgHide()
    {
        ClearCaster();
    }

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
        int X = default;
        int Y = default;
        LockedSurface mapSurfaceDescription = default;
        Rect R;
        int W;
        int H;
        R = new Rect(Window.WinLeft + sender.WinLeft + 1, Window.WinTop + sender.WinTop + 1, Window.WinLeft + sender.WinLeft + sender.WinWidth - 2, Window.WinTop + sender.WinTop + sender.WinHeight - 2);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        HexGeometry.HexToPixel(_mapX, _mapY, ref X, ref Y);
        X = X + 16 - (int)(((uint)W) >> 1);
        Y = Y + 16 - (int)(((uint)H) >> 1);
        if (X < 16)
        {
            X = 16;
        }

        if (Y < 16)
        {
            Y = 16;
        }

        LockedSurface.CreateFrom(lockedSurface, R, ref mapSurfaceDescription);
        StrategicMap.Current.SetViewPort(X, Y, _mapL, R.Right - R.Left, R.Bottom - R.Top);
        StrategicMap.Current.UpdateStaticScene();
        StrategicMap.Current.UpdateDynamicScene();
        StrategicMap.Current.DrawStaticScene(ref mapSurfaceDescription);
        StrategicMap.Current.DrawDynamicScene(ref mapSurfaceDescription);
    }

    // AoWDisjunctionDlg.pas:170
    public void MapViewUpdate()
    {
        DialogHosts.General.Manager.OverlayPriority = 49;
        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
    }
}
