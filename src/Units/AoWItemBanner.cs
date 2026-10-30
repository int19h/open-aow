// Translated from AoWItemBanner.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AOWMenu;
using static Aow.Units.AOWMenu.AOWMenu;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;

namespace Aow.Units.AoWItemBanner
{
    // { TItemBanner (instance size 120, VMT 00406950) }
    public partial class TItemBanner : Aow.Rtl.TDataModule
    {
        public TItemBanner(Aow.Rtl.TComponent AOwner) : base(AOwner) { }

        public Aow.Units.AoWLabel.TAOWLabel ItemNameLbl;
        // { +0044 }
        public Aow.Units.AOWMenu.TAOWMenu IBannerPopup;
        // { +0048 }
        public Aow.Units.AoWImage.TAOWImage AttackIcon;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage DefenseIcon;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage DamageIcon;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage ResistanceIcon;
        // { +0058 }
        public Aow.Units.AoWLabel.TAOWLabel Attack;
        // { +005C }
        public Aow.Units.AoWLabel.TAOWLabel Defense;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel Damage;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel Resistance;
        // { +0068 }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox DrawBox;
        // { +006C }
        public Aow.Units.AoWListBox.TAOWListBox AbLB;
        // { +0070 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar AbVSB;
        // AoWItemBanner.pas:46
        // { +0074 }
        public void IBannerPopupShow(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TItem Item = default;
            int X = default;
            int Y = default;
            var call__1 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.FindItem(Aow.Units.AoWItemBanner.AoWItemBanner.ItemID);
            Item = call__1;
            if (Item != null)
            {
                ItemNameLbl.Text = Item.FOwnerName;
                AttackIcon.Visible = false;
                DamageIcon.Visible = false;
                DefenseIcon.Visible = false;
                ResistanceIcon.Visible = false;
                Attack.Visible = false;
                Damage.Visible = false;
                Defense.Visible = false;
                Resistance.Visible = false;
                X = 30;
                Y = 100;
                if (Item.FAttack != 0)
                {
                    Attack.Text = Aow.Rtl.Rtl.IntToStr(Item.FAttack);
                    Attack.Visible = true;
                    AttackIcon.Visible = true;
                    AttackIcon.WinLeft = X + 10;
                    Attack.WinLeft = X + 30;
                    AttackIcon.WinTop = Y;
                    Attack.WinTop = Y + 5;
                    X += 50;
                    if (X == 130)
                    {
                        X = 30;
                        Y += 20;
                    }
                }

                if (Item.FDamage != 0)
                {
                    Damage.Text = Aow.Rtl.Rtl.IntToStr(Item.FDamage);
                    Damage.Visible = true;
                    DamageIcon.Visible = true;
                    DamageIcon.WinLeft = X + 10;
                    Damage.WinLeft = X + 30;
                    DamageIcon.WinTop = Y;
                    Damage.WinTop = Y + 5;
                    X += 50;
                    if (X == 130)
                    {
                        X = 30;
                        Y += 20;
                    }
                }

                if (Item.FDefense != 0)
                {
                    Defense.Text = Aow.Rtl.Rtl.IntToStr(Item.FDefense);
                    Defense.Visible = true;
                    DefenseIcon.Visible = true;
                    DefenseIcon.WinLeft = X + 10;
                    Defense.WinLeft = X + 30;
                    DefenseIcon.WinTop = Y;
                    Defense.WinTop = Y + 5;
                    X += 50;
                    if (X == 130)
                    {
                        X = 30;
                        Y += 20;
                    }
                }

                if (Item.FResistance != 0)
                {
                    Resistance.Text = Aow.Rtl.Rtl.IntToStr(Item.FResistance);
                    Resistance.Visible = true;
                    ResistanceIcon.Visible = true;
                    ResistanceIcon.WinLeft = X + 10;
                    Resistance.WinLeft = X + 30;
                    ResistanceIcon.WinTop = Y;
                    Resistance.WinTop = Y + 5;
                    X += 50;
                    if (X == 130)
                    {
                        X = 30;
                        Y += 20;
                    }
                }

                if (X != 30)
                {
                    Y += 20;
                }

                AbLB.Items.Clear();
                AbLB.Alignment.TopOffset = Y + 10;
                {
                    X = 1;
                    byte end__2 = 169;
                    for (; X <= end__2; X += 1)
                    {
                        if (Item.GetAbEnabled(X))
                        {
                            var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(X);
                            var call__4 = call__3.GetName(Item);
                            AbLB.Items.AddObject(call__4, Aow.Rtl.Rtl.IntToObject((int)X));
                        }
                    }
                }

                var call__5 = AbLB.Items.Count;
                IBannerPopup.WinHeight = (call__5 * AbLB.ItemHeight) + (Y + 50);
                if (IBannerPopup.WinHeight > 200)
                {
                    IBannerPopup.WinHeight = 200;
                }
            }
        }

        // AoWItemBanner.pas:140
        public void IBannerPopupMouseDown(int A1, byte A2, int A3, int A4, int A5)
        {
            if (A2 == 1)
            {
                Aow.Units.AoWItemBanner.AoWItemBanner.ItemBanner.IBannerPopup.Visible = false;
            }
        }

        // AoWItemBanner.pas:145
        public void DrawBoxPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            Aow.Units.AoWE.TItem Item = default;
            var call__6 = Aow.Units.AoWE.AoWE.AoWHSMap.FItemControl.FindItem(Aow.Units.AoWItemBanner.AoWItemBanner.ItemID);
            Item = call__6;
            if (Item != null)
            {
                Item.Show(DrawBox.WinLeft, DrawBox.WinTop, SurfDesc);
            }
        }
    }

    public static partial class AoWItemBanner
    {
        public static Aow.Units.AoWItemBanner.TItemBanner ItemBanner;
        public static int ItemID;
    }
}
