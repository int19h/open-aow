// Translated from AoWDisjunctionDlg.pas.
using Aow.Engine.Nodes;
using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;

using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// caster, sort order, map location
public partial class DisjunctionDlg : DataModule
{
    public DisjunctionDlg(Component owner) : base(owner)
    { }

    public AOWImage TitleIcon;
    public AOWLabel Title;
    public AoWFrame BCeltL;
    public AOWButton CancelBtn;
    public AOWButton CastBtn;
    public AOWWindow Dlg;
    public AOWLabel DisjunctionChance;
    public AOWLabel DisjunctionChanceLbl;
    public AOWPanel DisjunctionChancePnl;
    public AOWLabel DisjunctionMana;
    public AOWLabel DisjunctionManaLbl;
    public AOWPanel DisjunctionManaPnl;
    public AOWPanel T4LBPnl;
    public AOWListBox OwnerList;
    public AOWListBox EnchantmentList;
    public AOWButton Sort;
    public AOWButton SortEnchantment;
    public AOWVScrollBar EnchantmentsScrollBar;
    public AOWLabel Spell;
    public AOWHScrollBar ChanceSlider;
    public AOWButton Close;
    public IvTranslator Translator;
    public AOWPanel MapViewPnl;
    public TaowUserDrawBox MapView;
    public DisjunctionSpellCaster FCaster;
    public int FSortOrder;
    public sbyte FMapX;
    public sbyte FMapY;
    public sbyte FMapL;

    // AoWDisjunctionDlg.pas:179
    public void ClearCaster()
    {
        DialogHosts.General.Manager.OverlayPriority = 0;
        if (FCaster != null)
        {
            FCaster.Release();
            FCaster = null;
        }
    }

    // AoWDisjunctionDlg.pas:189
    public void UpdateEnchantments()
    {
        GlobalMagicControl Magic;
        string Reason = "";
        Magic = AoWHSMap.Current.FGlobalMagicControl;
        EnchantmentList.Items.Clear();
        OwnerList.Items.Clear();
        for (int I = 0, last = Magic.FEnchantments.Count - 1; I <= last; I++)
        {
            if (Magic.FEnchantments.GetEnchantments(I).FPlayerIndex != AoWHSMap.Current.FSeatedPlayer)
            {
                if (Magic.FEnchantments.GetEnchantments(I).VisibleForPlayer(AoWHSMap.Current.FSeatedPlayer))
                {
                    if (Magic.FEnchantments.GetEnchantments(I).CanDispel(AoWHSMap.Current.FSeatedPlayer, ref Reason))
                    {
                        Reason = Magic.FEnchantments.GetEnchantments(I).GetName();
                        var name = Magic.FEnchantments.GetEnchantments(I).GetName();
                        var enchantmentEnchantmentID = Magic.FEnchantments.GetEnchantments(I);
                        EnchantmentList.Items.AddObject(name, IntToObject(enchantmentEnchantmentID.FEnchantmentID));
                        var enchantmentPlayerIndex = Magic.FEnchantments.GetEnchantments(I);
                        var player = AoWHSMap.Current.FPlayers.GetPlayers(enchantmentPlayerIndex.FPlayerIndex);
                        OwnerList.Items.Add(player.FLeader.FHeroName);
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
        switch (FSortOrder)
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
        if (FCaster.GetEnoughMana())
        {
            DisjunctionMana.TextColor = 12632256;
        }
        else
        {
            DisjunctionMana.TextColor = 255;
        }

        var spell = AoWHSSet.Current.FSpellControl.GetSpell(51);
        var mana = FCaster.GetMana();
        var fSpellControlSpell = AoWHSSet.Current.FSpellControl.GetSpell(51);
        var fCasterMana = FCaster.GetMana();
        DisjunctionMana.Text = IntToStr(spell.FBaseManaCost) + "+" + IntToStr(mana) + "=" + IntToStr(fSpellControlSpell.FBaseManaCost + fCasterMana);
        DisjunctionChance.Text = IntToStr(FCaster.FDispelChance) + "%";
        var dispelChanceMax = FCaster.GetDispelChanceMax();
        ChanceSlider.Max = dispelChanceMax;
        ChanceSlider.Position = FCaster.FDispelChance;
    }

    // AoWDisjunctionDlg.pas:233
    public void SelectEnchantment(int Index)
    {
        GlobalEnchantment Enchantment;
        XYL Location;
        if (Index >= 0 && Index < EnchantmentList.Items.Count)
        {
            EnchantmentList.Selected = Index;
            OwnerList.Selected = Index;
            var objectValue = EnchantmentList.Items.Objects[Index];
            FCaster.SetEnchantmentID(ObjectToInt(objectValue));
            Enchantment = FCaster.SelectedEnchantment();
            if (Enchantment != null)
            {
                Location = Enchantment.GetLocation();
                FMapX = Location.X;
                FMapY = Location.Y;
                FMapL = Location.L;
            }
            else
            {
                FMapX = -1;
            }

            if (FMapX == -1)
            {
                FCaster.GetSource().GetLocation(ref FMapX, ref FMapY, ref FMapL);
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
    public void Execute(DisjunctionSpellCaster Caster)
    {
        if (FCaster != null)
        {
            FCaster.Release();
        }

        FCaster = Caster;
        FCaster.AddRef();
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
        string Reason = "";
        if (FCaster.CanCast(ref Reason))
        {
            if (FCaster.Cast())
            {
                Dlg.Visible = false;
            }
        }
        else
        {
            AoWHSMap.Current.ShowMessage(Reason);
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
        FCaster.SetDispelChance(ChanceSlider.Position);
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
        if (FSortOrder == 0)
        {
            FSortOrder = 1;
        }
        else
        {
            FSortOrder = 0;
        }

        SortEnchantments();
    }

    // AoWDisjunctionDlg.pas:128
    public void SortClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FSortOrder == 2)
        {
            FSortOrder = 3;
        }
        else
        {
            FSortOrder = 2;
        }

        SortEnchantments();
    }

    // AoWDisjunctionDlg.pas:136
    public void DlgHide()
    {
        ClearCaster();
    }

    // AoWDisjunctionDlg.pas:142
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    // AoWDisjunctionDlg.pas:148
    public void MapViewDraw(AoWComponent Sender, SurfaceDesc SD)
    {
        int X = default;
        int Y = default;
        SurfaceDesc MapSD = default;
        Rect R;
        int W;
        int H;
        R = new Rect(Dlg.WinLeft + Sender.WinLeft + 1, Dlg.WinTop + Sender.WinTop + 1, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth - 2, Dlg.WinTop + Sender.WinTop + Sender.WinHeight - 2);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        HexGeometry.HXtoHP(FMapX, FMapY, ref X, ref Y);
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

        SurfaceDesc.CreateSDfromSD(SD, R, ref MapSD);
        AoWHSMap.Current.SetViewPort(X, Y, FMapL, R.Right - R.Left, R.Bottom - R.Top);
        AoWHSMap.Current.UpdateStaticScene();
        AoWHSMap.Current.UpdateDynamicScene();
        AoWHSMap.Current.DrawStaticScene(ref MapSD);
        AoWHSMap.Current.DrawDynamicScene(ref MapSD);
    }

    // AoWDisjunctionDlg.pas:170
    public void MapViewUpdate()
    {
        DialogHosts.General.Manager.OverlayPriority = 49;
        MapView.FDrawn = MapView.FDrawn && (AoWHSMap.Current.Frame & 1) != 0;
    }
}
