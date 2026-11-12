// Translated from AoWDisjunctionDlg.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// caster, sort order, map location
public partial class DisjunctionDialog : DataModule
{
    public DisjunctionDialog(Component owner) : base(owner)
    { }

    public Image TitleIcon;
    public Label Title;
    public Frame BCeltL;
    public Button CancelBtn;
    public Button CastBtn;
    public Window Dlg;
    public Label DisjunctionChance;
    public Label DisjunctionChanceLbl;
    public Panel DisjunctionChancePnl;
    public Label DisjunctionMana;
    public Label DisjunctionManaLbl;
    public Panel DisjunctionManaPnl;
    public Panel T4LBPnl;
    public ListBox OwnerList;
    public ListBox EnchantmentList;
    public Button Sort;
    public Button SortEnchantment;
    public VerticalScrollBar EnchantmentsScrollBar;
    public Label Spell;
    public HorizontalScrollBar ChanceSlider;
    public Button Close;
    public IvTranslator Translator;
    public Panel MapViewPnl;
    public UserDrawBox MapView;
    public DisjunctionSpellCaster Caster;
    public int SortOrder;
    public sbyte MapX;
    public sbyte MapY;
    public sbyte MapL;

    // AoWDisjunctionDlg.pas:179
    public void ClearCaster()
    {
        DialogHosts.General.Manager.OverlayPriority = 0;
        if (Caster != null)
        {
            Caster.Release();
            Caster = null;
        }
    }

    // AoWDisjunctionDlg.pas:189
    public void UpdateEnchantments()
    {
        GlobalMagicControl magic;
        string reason = "";
        magic = AoWHSMap.Current.GlobalMagicControl;
        EnchantmentList.Items.Clear();
        OwnerList.Items.Clear();
        for (int I = 0, last = magic.Enchantments.Count - 1; I <= last; I++)
        {
            if (magic.Enchantments.GetEnchantments(I).PlayerIndex != AoWHSMap.Current.SeatedPlayer)
            {
                if (magic.Enchantments.GetEnchantments(I).VisibleForPlayer(AoWHSMap.Current.SeatedPlayer))
                {
                    if (magic.Enchantments.GetEnchantments(I).CanDispel(AoWHSMap.Current.SeatedPlayer, ref reason))
                    {
                        reason = magic.Enchantments.GetEnchantments(I).GetName();
                        var name = magic.Enchantments.GetEnchantments(I).GetName();
                        var enchantmentEnchantmentId = magic.Enchantments.GetEnchantments(I);
                        EnchantmentList.Items.AddObject(name, IntToObject(enchantmentEnchantmentId._enchantmentId));
                        var enchantmentPlayerIndex = magic.Enchantments.GetEnchantments(I);
                        var player = AoWHSMap.Current.Players.GetPlayers(enchantmentPlayerIndex.PlayerIndex);
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
        switch (SortOrder)
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
        if (Caster.GetEnoughMana())
        {
            DisjunctionMana.TextColor = 12632256;
        }
        else
        {
            DisjunctionMana.TextColor = 255;
        }

        var spell = AoWHSSet.Current.SpellControl.GetSpell(51);
        var mana = Caster.GetMana();
        var fSpellControlSpell = AoWHSSet.Current.SpellControl.GetSpell(51);
        var fCasterMana = Caster.GetMana();
        DisjunctionMana.Text = IntToStr(spell.BaseManaCost) + "+" + IntToStr(mana) + "=" + IntToStr(fSpellControlSpell.BaseManaCost + fCasterMana);
        DisjunctionChance.Text = IntToStr(Caster.DispelChance) + "%";
        var dispelChanceMax = Caster.GetDispelChanceMax();
        ChanceSlider.Max = dispelChanceMax;
        ChanceSlider.Position = Caster.DispelChance;
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
            Caster.SetEnchantmentId(ObjectToInt(objectValue));
            enchantment = Caster.SelectedEnchantment();
            if (enchantment != null)
            {
                location = enchantment.GetLocation();
                MapX = location.X;
                MapY = location.Y;
                MapL = location.MapLevel;
            }
            else
            {
                MapX = -1;
            }

            if (MapX == -1)
            {
                Caster.GetSource().GetLocation(ref MapX, ref MapY, ref MapL);
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
        if (Caster != null)
        {
            Caster.Release();
        }

        Caster = caster;
        Caster.AddRef();
        UpdateEnchantments();
        SelectEnchantment(0);
        DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    // AoWDisjunctionDlg.pas:76
    public void CancelBtnClick()
    {
        Dlg.Visible = false;
    }

    // AoWDisjunctionDlg.pas:82
    public void CastBtnClick()
    {
        string reason = "";
        if (Caster.CanCast(ref reason))
        {
            if (Caster.Cast())
            {
                Dlg.Visible = false;
            }
        }
        else
        {
            AoWHSMap.Current.ShowMessage(reason);
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
        Caster.SetDispelChance(ChanceSlider.Position);
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
        if (SortOrder == 0)
        {
            SortOrder = 1;
        }
        else
        {
            SortOrder = 0;
        }

        SortEnchantments();
    }

    // AoWDisjunctionDlg.pas:128
    public void SortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (SortOrder == 2)
        {
            SortOrder = 3;
        }
        else
        {
            SortOrder = 2;
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
            Dlg.Visible = false;
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
        R = new Rect(Dlg.WinLeft + sender.WinLeft + 1, Dlg.WinTop + sender.WinTop + 1, Dlg.WinLeft + sender.WinLeft + sender.WinWidth - 2, Dlg.WinTop + sender.WinTop + sender.WinHeight - 2);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        HexGeometry.HexToPixel(MapX, MapY, ref X, ref Y);
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
        AoWHSMap.Current.SetViewPort(X, Y, MapL, R.Right - R.Left, R.Bottom - R.Top);
        AoWHSMap.Current.UpdateStaticScene();
        AoWHSMap.Current.UpdateDynamicScene();
        AoWHSMap.Current.DrawStaticScene(ref mapSurfaceDescription);
        AoWHSMap.Current.DrawDynamicScene(ref mapSurfaceDescription);
    }

    // AoWDisjunctionDlg.pas:170
    public void MapViewUpdate()
    {
        DialogHosts.General.Manager.OverlayPriority = 49;
        MapView.Drawn = MapView.Drawn && (AoWHSMap.Current.Frame & 1) != 0;
    }
}
