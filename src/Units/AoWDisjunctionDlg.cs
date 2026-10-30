// Translated from AoWDisjunctionDlg.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWHScroll;
using static Aow.Units.AoWHScroll.AoWHScroll;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.GlobalSpells;
using static Aow.Units.GlobalSpells.GlobalSpells;
using Aow.Units.E3Globals;
using static Aow.Units.E3Globals.E3Globals;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWDisjunctionDlg
{
    // { TDisjunctionDlg (instance size $B0, VMT 0043285C): $A4 caster, $A8 sort order, $AC..$AE map location. }
    public partial class TDisjunctionDlg : Aow.Rtl.TDataModule
    {
        public TDisjunctionDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel Title;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton CastBtn;
        // { +0054 }
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel DisjunctionChance;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel DisjunctionChanceLbl;
        // { +0060 }
        public Aow.Units.AoWPanel.TAOWPanel DisjunctionChancePnl;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel DisjunctionMana;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel DisjunctionManaLbl;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel DisjunctionManaPnl;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel T4LBPnl;
        // { +0074 }
        public Aow.Units.AoWListBox.TAOWListBox OwnerList;
        // { +0078 }
        public Aow.Units.AoWListBox.TAOWListBox EnchantmentList;
        // { +007C }
        public Aow.Units.AoWButton.TAOWButton Sort;
        // { +0080 }
        public Aow.Units.AoWButton.TAOWButton SortEnchantment;
        // { +0084 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar EnchantmentsScrollBar;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel Spell;
        // { +008C }
        public Aow.Units.AoWHScroll.TAOWHScrollBar ChanceSlider;
        // { +0090 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +0094 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0098 }
        public Aow.Units.AoWPanel.TAOWPanel MapViewPnl;
        // { +009C }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox MapView;
        // { +00A0 }
        public Aow.Units.GlobalSpells.TDisjunctionSpellCaster FCaster;
        // { $A4 }
        public int FSortOrder;
        // { $A8 }
        public sbyte FMapX;
        // { $AC }
        public sbyte FMapY;
        // { $AD }
        public sbyte FMapL;
        // { $AE }
        public Aow.Units.FixedArray_byte_1 F_DisjunctionDlg_AF;
        // AoWDisjunctionDlg.pas:179
        public void ClearCaster()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            if (FCaster != null)
            {
                FCaster.Release();
                FCaster = null;
            }
        }

        // AoWDisjunctionDlg.pas:189
        public void UpdateEnchantments()
        {
            Aow.Units.AoWE.TGlobalMagicControl Magic = default;
            int I = default;
            Aow.Rtl.AnsiString Reason = default;
            Magic = Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl;
            EnchantmentList.Items.Clear();
            OwnerList.Items.Clear();
            {
                I = 0;
                int end__1 = Magic.FEnchantments.Count - 1;
                for (; I <= end__1; I += 1)
                {
                    var call__2 = Magic.FEnchantments.GetEnchantments(I);
                    if (call__2.FPlayerIndex != Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
                    {
                        var call__3 = Magic.FEnchantments.GetEnchantments(I);
                        var call__4 = call__3.VisibleForPlayer(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        if (call__4)
                        {
                            var call__5 = Magic.FEnchantments.GetEnchantments(I);
                            var call__6 = call__5.CanDispel(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), ref Reason);
                            if (call__6)
                            {
                                var call__7 = Magic.FEnchantments.GetEnchantments(I);
                                var call__8 = call__7.GetName();
                                Reason = call__8;
                                var call__9 = Magic.FEnchantments.GetEnchantments(I);
                                var call__10 = call__9.GetName();
                                var call__11 = Magic.FEnchantments.GetEnchantments(I);
                                EnchantmentList.Items.AddObject(call__10, Aow.Rtl.Rtl.IntToObject((int)call__11.FEnchantmentID));
                                var call__12 = Magic.FEnchantments.GetEnchantments(I);
                                var call__13 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__12.FPlayerIndex);
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
                    Aow.Units.Engine.Engine.SortStringsAscending(EnchantmentList.Items, [OwnerList.Items]);
                    break;
                case 1:
                    Aow.Units.Engine.Engine.SortStringsDescending(EnchantmentList.Items, [OwnerList.Items]);
                    break;
                case 2:
                    Aow.Units.Engine.Engine.SortStringsAscending(OwnerList.Items, [EnchantmentList.Items]);
                    break;
                case 3:
                    Aow.Units.Engine.Engine.SortStringsDescending(OwnerList.Items, [EnchantmentList.Items]);
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

            var call__15 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(51);
            var call__16 = FCaster.GetMana();
            var call__17 = Aow.Units.AoWE.AoWE.AoWHSSet.FSpellControl.GetSpell(51);
            var call__18 = FCaster.GetMana();
            DisjunctionMana.Text = (((Aow.Rtl.Rtl.IntToStr(call__15.FBaseManaCost) + AnsiString.FromBytes(43)) + Aow.Rtl.Rtl.IntToStr(call__16)) + AnsiString.FromBytes(61)) + Aow.Rtl.Rtl.IntToStr(call__17.FBaseManaCost + call__18);
            DisjunctionChance.Text = Aow.Rtl.Rtl.IntToStr(FCaster.FDispelChance) + AnsiString.FromBytes(37);
            var call__19 = FCaster.GetDispelChanceMax();
            ChanceSlider.Max = call__19;
            ChanceSlider.Position = FCaster.FDispelChance;
        }

        // AoWDisjunctionDlg.pas:233
        public void SelectEnchantment(int Index)
        {
            Aow.Units.AoWE.TGlobalEnchantment Enchantment = default;
            Aow.Units.HSEngine.TXYL Location = default;
            bool bool__20 = (Index >= 0);
            if (bool__20)
            {
                var call__21 = EnchantmentList.Items.Count;
                bool__20 = (Index < call__21);
            }

            if (bool__20)
            {
                EnchantmentList.Selected = Index;
                OwnerList.Selected = Index;
                var call__22 = EnchantmentList.Items.Objects[Index];
                FCaster.SetEnchantmentID(Aow.Rtl.Rtl.ObjectToInt(call__22));
                var call__23 = FCaster.SelectedEnchantment();
                Enchantment = call__23;
                if (Enchantment != null)
                {
                    Location = Enchantment.GetLocation();
                    FMapX = unchecked((sbyte)Location.X);
                    FMapY = unchecked((sbyte)Location.Y);
                    FMapL = unchecked((sbyte)Location.L);
                }
                else
                {
                    FMapX = unchecked((sbyte)-1);
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
        public void Execute(Aow.Units.GlobalSpells.TDisjunctionSpellCaster Caster)
        {
            if (FCaster != null)
            {
                FCaster.Release();
            }

            FCaster = Caster;
            FCaster.AddRef();
            UpdateEnchantments();
            SelectEnchantment(0);
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
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
            Aow.Rtl.AnsiString Reason = default;
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
                Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(Reason);
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
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
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
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
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                Dlg.Visible = false;
            }
        }

        // AoWDisjunctionDlg.pas:148
        public void MapViewDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            int X = default;
            int Y = default;
            Aow.Units.GFXE.TSurfaceDesc MapSD = default;
            Aow.Rtl.TRect R = default;
            int W = default;
            int H = default;
            var call__27 = Aow.Rtl.Rtl.Rect((Dlg.WinLeft + ((Aow.Units.AoWImage.TAOWImage)Sender).WinLeft) + 1, (Dlg.WinTop + ((Aow.Units.AoWImage.TAOWImage)Sender).WinTop) + 1, ((Dlg.WinLeft + ((Aow.Units.AoWImage.TAOWImage)Sender).WinLeft) + ((Aow.Units.AoWImage.TAOWImage)Sender).WinWidth) - 2, ((Dlg.WinTop + ((Aow.Units.AoWImage.TAOWImage)Sender).WinTop) + ((Aow.Units.AoWImage.TAOWImage)Sender).WinHeight) - 2);
            R = call__27;
            W = R.Right - R.Left;
            H = R.Bottom - R.Top;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FMapX, FMapY, ref X, ref Y);
            X = (X + 16) - ((int)(((uint)W) >> 1));
            Y = (Y + 16) - ((int)(((uint)H) >> 1));
            if (X < 16)
            {
                X = 16;
            }

            if (Y < 16)
            {
                Y = 16;
            }

            Aow.Units.GFXE.GFXE.CreateSDfromSD(SD, R, ref MapSD);
            Aow.Units.AoWE.AoWE.AoWHSMap.SetViewPort(X, Y, FMapL, R.Right - R.Left, R.Bottom - R.Top);
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateStaticScene();
            Aow.Units.AoWE.AoWE.AoWHSMap.UpdateDynamicScene();
            Aow.Units.AoWE.AoWE.AoWHSMap.DrawStaticScene(ref MapSD);
            Aow.Units.AoWE.AoWE.AoWHSMap.DrawDynamicScene(ref MapSD);
        }

        // AoWDisjunctionDlg.pas:170
        public void MapViewUpdate()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
            MapView.FDrawn = MapView.FDrawn && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 1) != 0);
        }
    }

    public static partial class AoWDisjunctionDlg
    {
    }
}
