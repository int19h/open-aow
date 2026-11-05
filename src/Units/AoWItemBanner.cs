// Translated from AoWItemBanner.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.GFXE;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
using Aow.Units.AOWMenu;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWVScroll;
using Aow.Units.aowUserDrawBox;

namespace Aow.Units.AoWItemBanner;

public partial class TItemBanner : TDataModule
{
    public TItemBanner(TComponent owner) : base(owner)
    { }

    public TAOWLabel ItemNameLbl;
    public TAOWMenu IBannerPopup;
    public TAOWImage AttackIcon;
    public TAOWImage DefenseIcon;
    public TAOWImage DamageIcon;
    public TAOWImage ResistanceIcon;
    public TAOWLabel Attack;
    public TAOWLabel Defense;
    public TAOWLabel Damage;
    public TAOWLabel Resistance;
    public TaowUserDrawBox DrawBox;
    public TAOWListBox AbLB;
    public TAOWVScrollBar AbVSB;
    // AoWItemBanner.pas:46
    public void IBannerPopupShow(TObject Sender)
    {
        TItem Item;
        int X;
        int Y;
        Item = AoWHSMap.FItemControl.FindItem(AoWItemBannerUnit.ItemID);
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
                Attack.Text = IntToStr(Item.FAttack);
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
                Damage.Text = IntToStr(Item.FDamage);
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
                Defense.Text = IntToStr(Item.FDefense);
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
                Resistance.Text = IntToStr(Item.FResistance);
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
            X = 1;
            for (; X <= 169; X += 1)
            {
                if (Item.GetAbEnabled(X))
                {
                    var name = AoWHSSet.FAbilityControl.GetAbility(X).GetName(Item);
                    AbLB.Items.AddObject(name, IntToObject(X));
                }
            }

            var count = AbLB.Items.Count;
            IBannerPopup.WinHeight = count * AbLB.ItemHeight + (Y + 50);
            if (IBannerPopup.WinHeight > 200)
            {
                IBannerPopup.WinHeight = 200;
            }
        }
    }

    // AoWItemBanner.pas:140
    public void IBannerPopupMouseDown(int Sender, byte Button, int Shift, int X, int Y)
    {
        if (Button == 1)
        {
            AoWItemBannerUnit.ItemBanner.IBannerPopup.Visible = false;
        }
    }

    // AoWItemBanner.pas:145
    public void DrawBoxPreDraw(TObject Sender, TSurfaceDesc SurfDesc)
    {
        TItem Item;
        Item = AoWHSMap.FItemControl.FindItem(AoWItemBannerUnit.ItemID);
        if (Item != null)
        {
            Item.Show(DrawBox.WinLeft, DrawBox.WinTop, SurfDesc);
        }
    }
}

public static partial class AoWItemBannerUnit
{
    public static TItemBanner ItemBanner;
    public static int ItemID;
}
