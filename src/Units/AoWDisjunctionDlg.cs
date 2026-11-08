// Translated from AoWDisjunctionDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.aowUserDrawBox;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.GFXE;
using Aow.Units.GlobalSpells;
using Aow.Units.HSEngine;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.Engine.EngineUnit;
using static Aow.Units.GFXE.GFXEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;

namespace Aow.Units.AoWDisjunctionDlg;

// caster, sort order, map location
public partial class TDisjunctionDlg : TDataModule
{
    public TDisjunctionDlg(TComponent owner) : base(owner)
    { }

    public TAOWImage TitleIcon;
    public TAOWLabel Title;
    public TAoWFrame BCeltL;
    public TAOWButton CancelBtn;
    public TAOWButton CastBtn;
    public TAOWWindow Dlg;
    public TAOWLabel DisjunctionChance;
    public TAOWLabel DisjunctionChanceLbl;
    public TAOWPanel DisjunctionChancePnl;
    public TAOWLabel DisjunctionMana;
    public TAOWLabel DisjunctionManaLbl;
    public TAOWPanel DisjunctionManaPnl;
    public TAOWPanel T4LBPnl;
    public TAOWListBox OwnerList;
    public TAOWListBox EnchantmentList;
    public TAOWButton Sort;
    public TAOWButton SortEnchantment;
    public TAOWVScrollBar EnchantmentsScrollBar;
    public TAOWLabel Spell;
    public TAOWHScrollBar ChanceSlider;
    public TAOWButton Close;
    public TIvTranslator Translator;
    public TAOWPanel MapViewPnl;
    public TaowUserDrawBox MapView;
    public TDisjunctionSpellCaster FCaster;
    public int FSortOrder;
    public sbyte FMapX;
    public sbyte FMapY;
    public sbyte FMapL;

    // AoWDisjunctionDlg.pas:179
    public void ClearCaster()
    {
        General.Manager.OverlayPriority = 0;
        if (FCaster != null)
        {
            FCaster.Release();
            FCaster = null;
        }
    }

    // AoWDisjunctionDlg.pas:189
    public void UpdateEnchantments()
    {
        TGlobalMagicControl Magic;
        string Reason = "";
        Magic = AoWHSMap.FGlobalMagicControl;
        EnchantmentList.Items.Clear();
        OwnerList.Items.Clear();
        for (int I = 0, last = Magic.FEnchantments.Count - 1; I <= last; I++)
        {
            if (Magic.FEnchantments.GetEnchantments(I).FPlayerIndex != AoWHSMap.FSeatedPlayer)
            {
                if (Magic.FEnchantments.GetEnchantments(I).VisibleForPlayer(AoWHSMap.FSeatedPlayer))
                {
                    if (Magic.FEnchantments.GetEnchantments(I).CanDispel(AoWHSMap.FSeatedPlayer, ref Reason))
                    {
                        Reason = Magic.FEnchantments.GetEnchantments(I).GetName();
                        var name = Magic.FEnchantments.GetEnchantments(I).GetName();
                        var enchantmentEnchantmentID = Magic.FEnchantments.GetEnchantments(I);
                        EnchantmentList.Items.AddObject(name, IntToObject(enchantmentEnchantmentID.FEnchantmentID));
                        var enchantmentPlayerIndex = Magic.FEnchantments.GetEnchantments(I);
                        var player = AoWHSMap.FPlayers.GetPlayers(enchantmentPlayerIndex.FPlayerIndex);
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
                SortStringsAscending(EnchantmentList.Items, [OwnerList.Items]);
                break;
            case 1:
                SortStringsDescending(EnchantmentList.Items, [OwnerList.Items]);
                break;
            case 2:
                SortStringsAscending(OwnerList.Items, [EnchantmentList.Items]);
                break;
            case 3:
                SortStringsDescending(OwnerList.Items, [EnchantmentList.Items]);
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

        var spell = AoWHSSet.FSpellControl.GetSpell(51);
        var mana = FCaster.GetMana();
        var fSpellControlSpell = AoWHSSet.FSpellControl.GetSpell(51);
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
        TGlobalEnchantment Enchantment;
        TXYL Location;
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
    public void Execute(TDisjunctionSpellCaster Caster)
    {
        if (FCaster != null)
        {
            FCaster.Release();
        }

        FCaster = Caster;
        FCaster.AddRef();
        UpdateEnchantments();
        SelectEnchantment(0);
        General.Manager.OverlayPriority = 49;
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
            AoWHSMap.ShowMessage(Reason);
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
        General.PlayButtonSound(0);
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
        General.PlayButtonSound(0);
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
    public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    // AoWDisjunctionDlg.pas:148
    public void MapViewDraw(TAoWComponent Sender, TSurfaceDesc SD)
    {
        int X = default;
        int Y = default;
        TSurfaceDesc MapSD = default;
        TRect R;
        int W;
        int H;
        R = Rect(Dlg.WinLeft + Sender.WinLeft + 1, Dlg.WinTop + Sender.WinTop + 1, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth - 2, Dlg.WinTop + Sender.WinTop + Sender.WinHeight - 2);
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        HXtoHP(FMapX, FMapY, ref X, ref Y);
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

        CreateSDfromSD(SD, R, ref MapSD);
        AoWHSMap.SetViewPort(X, Y, FMapL, R.Right - R.Left, R.Bottom - R.Top);
        AoWHSMap.UpdateStaticScene();
        AoWHSMap.UpdateDynamicScene();
        AoWHSMap.DrawStaticScene(ref MapSD);
        AoWHSMap.DrawDynamicScene(ref MapSD);
    }

    // AoWDisjunctionDlg.pas:170
    public void MapViewUpdate()
    {
        General.Manager.OverlayPriority = 49;
        MapView.FDrawn = MapView.FDrawn && (AoWHSMap.Frame & 1) != 0;
    }
}

public static partial class AoWDisjunctionDlgUnit
{
}
