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

    public Label ItemNameLabel;
    public Menu BannerMenu;
    public Image AttackIcon;
    public Image DefenseIcon;
    public Image DamageIcon;
    public Image ResistanceIcon;
    public Label AttackLabel;
    public Label DefenseLabel;
    public Label DamageLabel;
    public Label ResistanceLabel;
    public UserDrawBox ItemView;
    public ListBox AbilityList;

    // AoWItemBanner.pas:46
    public void IBannerPopupShow()
    {
        Item item;
        int X;
        int Y;
        item = AoWHSMap.Current.ItemControl.FindItem(ItemId);
        if (item != null)
        {
            ItemNameLabel.Text = item.OwnerName;
            AttackIcon.Visible = false;
            DamageIcon.Visible = false;
            DefenseIcon.Visible = false;
            ResistanceIcon.Visible = false;
            AttackLabel.Visible = false;
            DamageLabel.Visible = false;
            DefenseLabel.Visible = false;
            ResistanceLabel.Visible = false;
            X = 30;
            Y = 100;
            if (item.Attack != 0)
            {
                AttackLabel.Text = IntToStr(item.Attack);
                AttackLabel.Visible = true;
                AttackIcon.Visible = true;
                AttackIcon.WinLeft = X + 10;
                AttackLabel.WinLeft = X + 30;
                AttackIcon.WinTop = Y;
                AttackLabel.WinTop = Y + 5;
                X += 50;
                if (X == 130)
                {
                    X = 30;
                    Y += 20;
                }
            }

            if (item.Damage != 0)
            {
                DamageLabel.Text = IntToStr(item.Damage);
                DamageLabel.Visible = true;
                DamageIcon.Visible = true;
                DamageIcon.WinLeft = X + 10;
                DamageLabel.WinLeft = X + 30;
                DamageIcon.WinTop = Y;
                DamageLabel.WinTop = Y + 5;
                X += 50;
                if (X == 130)
                {
                    X = 30;
                    Y += 20;
                }
            }

            if (item.Defense != 0)
            {
                DefenseLabel.Text = IntToStr(item.Defense);
                DefenseLabel.Visible = true;
                DefenseIcon.Visible = true;
                DefenseIcon.WinLeft = X + 10;
                DefenseLabel.WinLeft = X + 30;
                DefenseIcon.WinTop = Y;
                DefenseLabel.WinTop = Y + 5;
                X += 50;
                if (X == 130)
                {
                    X = 30;
                    Y += 20;
                }
            }

            if (item.Resistance != 0)
            {
                ResistanceLabel.Text = IntToStr(item.Resistance);
                ResistanceLabel.Visible = true;
                ResistanceIcon.Visible = true;
                ResistanceIcon.WinLeft = X + 10;
                ResistanceLabel.WinLeft = X + 30;
                ResistanceIcon.WinTop = Y;
                ResistanceLabel.WinTop = Y + 5;
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

            AbilityList.Items.Clear();
            AbilityList.Alignment.TopOffset = Y + 10;
            X = 1;
            for (; X <= 169; X += 1)
            {
                if (item.GetAbEnabled(X))
                {
                    var name = AoWHSSet.Current.AbilityControl.GetAbility(X).GetName(item);
                    AbilityList.Items.AddObject(name, IntToObject(X));
                }
            }

            var count = AbilityList.Items.Count;
            BannerMenu.WinHeight = count * AbilityList.ItemHeight + (Y + 50);
            if (BannerMenu.WinHeight > 200)
            {
                BannerMenu.WinHeight = 200;
            }
        }
    }

    // AoWItemBanner.pas:140
    public void IBannerPopupMouseDown(int sender, byte button, int shift, int X, int Y)
    {
        if (button == 1)
        {
            DialogHosts.ItemBanner.BannerMenu.Visible = false;
        }
    }

    // AoWItemBanner.pas:145
    public void DrawBoxPreDraw(LockedSurface lockedSurface)
    {
        Item item;
        item = AoWHSMap.Current.ItemControl.FindItem(ItemId);
        if (item != null)
        {
            item.Show(ItemView.WinLeft, ItemView.WinTop, lockedSurface);
        }
    }

    internal static int ItemId { get; set; }

}
