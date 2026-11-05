// Translated from AoWDisjunctionDlg.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWHScroll;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.aowUserDrawBox;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.GlobalSpells;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWDisjunctionDlg
{
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
            TGlobalMagicControl Magic = default;
            int I = default;
            AnsiString Reason = default;
            Magic = AoWHSMap.FGlobalMagicControl;
            EnchantmentList.Items.Clear();
            OwnerList.Items.Clear();
            {
                I = 0;
                int end__1 = Magic.FEnchantments.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = Magic.FEnchantments.GetEnchantments(I);
                    if (call__2.FPlayerIndex != AoWHSMap.FSeatedPlayer)
                    {
                        var call__3 = Magic.FEnchantments.GetEnchantments(I);
                        var call__4 = call__3.VisibleForPlayer(AoWHSMap.FSeatedPlayer);
                        if (call__4)
                        {
                            var call__5 = Magic.FEnchantments.GetEnchantments(I);
                            var call__6 = call__5.CanDispel(AoWHSMap.FSeatedPlayer, ref Reason);
                            if (call__6)
                            {
                                var call__7 = Magic.FEnchantments.GetEnchantments(I);
                                var call__8 = call__7.GetName();
                                Reason = call__8;
                                var call__9 = Magic.FEnchantments.GetEnchantments(I);
                                var call__10 = call__9.GetName();
                                var call__11 = Magic.FEnchantments.GetEnchantments(I);
                                EnchantmentList.Items.AddObject(call__10, IntToObject(call__11.FEnchantmentID));
                                var call__12 = Magic.FEnchantments.GetEnchantments(I);
                                var call__13 = AoWHSMap.FPlayers.GetPlayers(call__12.FPlayerIndex);
                                OwnerList.Items.Add(call__13.FLeader.FHeroName);
                            }
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
            var call__14 = FCaster.GetEnoughMana();
            if (call__14)
            {
                DisjunctionMana.TextColor = 12632256;
            }
            else
            {
                DisjunctionMana.TextColor = 255;
            }

            var call__15 = AoWHSSet.FSpellControl.GetSpell(51);
            var call__16 = FCaster.GetMana();
            var call__17 = AoWHSSet.FSpellControl.GetSpell(51);
            var call__18 = FCaster.GetMana();
            DisjunctionMana.Text = IntToStr(call__15.FBaseManaCost) + AnsiString.FromBytes(43) + IntToStr(call__16) + AnsiString.FromBytes(61) + IntToStr(call__17.FBaseManaCost + call__18);
            DisjunctionChance.Text = IntToStr(FCaster.FDispelChance) + AnsiString.FromBytes(37);
            var call__19 = FCaster.GetDispelChanceMax();
            ChanceSlider.Max = call__19;
            ChanceSlider.Position = FCaster.FDispelChance;
        }

        // AoWDisjunctionDlg.pas:233
        public void SelectEnchantment(int Index)
        {
            TGlobalEnchantment Enchantment = default;
            TXYL Location = default;
            bool bool__20 = Index >= 0;
            if (bool__20)
            {
                var call__21 = EnchantmentList.Items.Count;
                bool__20 = Index < call__21;
            }

            if (bool__20)
            {
                EnchantmentList.Selected = Index;
                OwnerList.Selected = Index;
                var call__22 = EnchantmentList.Items.Objects[Index];
                FCaster.SetEnchantmentID(ObjectToInt(call__22));
                var call__23 = FCaster.SelectedEnchantment();
                Enchantment = call__23;
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
                    var call__24 = FCaster.GetSource();
                    call__24.GetLocation(ref FMapX, ref FMapY, ref FMapL);
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
            AnsiString Reason = default;
            var call__25 = FCaster.CanCast(ref Reason);
            if (call__25)
            {
                var call__26 = FCaster.Cast();
                if (call__26)
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
            TRect R = default;
            int W = default;
            int H = default;
            var call__27 = Rect(Dlg.WinLeft + ((TAOWImage)Sender).WinLeft + 1, Dlg.WinTop + ((TAOWImage)Sender).WinTop + 1, Dlg.WinLeft + ((TAOWImage)Sender).WinLeft + ((TAOWImage)Sender).WinWidth - 2, Dlg.WinTop + ((TAOWImage)Sender).WinTop + ((TAOWImage)Sender).WinHeight - 2);
            R = call__27;
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

    public static partial class AoWDisjunctionDlg
    {
    }
}
