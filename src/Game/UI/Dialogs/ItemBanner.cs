// Translated from AoWItemBanner.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class ItemBanner : DataModule
{
    public ItemBanner(Component owner) : base(owner)
    { }

    public Label ItemNameLbl;
    public Menu IBannerPopup;
    public Image AttackIcon;
    public Image DefenseIcon;
    public Image DamageIcon;
    public Image ResistanceIcon;
    public Label Attack;
    public Label Defense;
    public Label Damage;
    public Label Resistance;
    public UserDrawBox DrawBox;
    public ListBox AbLB;
    public VerticalScrollBar AbVSB;

    // AoWItemBanner.pas:46
    public void IBannerPopupShow()
    {
        Item item;
        int X;
        int Y;
        item = AoWHSMap.Current.ItemControl.FindItem(s_itemId);
        if (item != null)
        {
            ItemNameLbl.Text = item.OwnerName;
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
            if (item.Attack != 0)
            {
                Attack.Text = IntToStr(item.Attack);
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

            if (item.Damage != 0)
            {
                Damage.Text = IntToStr(item.Damage);
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

            if (item.Defense != 0)
            {
                Defense.Text = IntToStr(item.Defense);
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

            if (item.Resistance != 0)
            {
                Resistance.Text = IntToStr(item.Resistance);
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
                if (item.GetAbEnabled(X))
                {
                    var name = AoWHSSet.Current.AbilityControl.GetAbility(X).GetName(item);
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
    public void IBannerPopupMouseDown(int sender, byte button, int shift, int X, int Y)
    {
        if (button == 1)
        {
            DialogHosts.ItemBanner.IBannerPopup.Visible = false;
        }
    }

    // AoWItemBanner.pas:145
    public void DrawBoxPreDraw(LockedSurface lockedSurface)
    {
        Item item;
        item = AoWHSMap.Current.ItemControl.FindItem(s_itemId);
        if (item != null)
        {
            item.Show(DrawBox.WinLeft, DrawBox.WinTop, lockedSurface);
        }
    }

    internal static int s_itemId;

}
