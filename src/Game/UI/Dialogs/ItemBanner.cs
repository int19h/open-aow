#nullable disable
// Translated from AoWItemBanner.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

public partial class ItemBanner : UIModule
{
    public ItemBanner(UIContext ui) : base(ui)
    { }

    public Label ItemNameLabel { get; private set; }
    public Menu BannerMenu { get; private set; }
    public Image AttackIcon { get; private set; }
    public Image DefenseIcon { get; private set; }
    public Image DamageIcon { get; private set; }
    public Image ResistanceIcon { get; private set; }
    public Label AttackLabel { get; private set; }
    public Label DefenseLabel { get; private set; }
    public Label DamageLabel { get; private set; }
    public Label ResistanceLabel { get; private set; }
    public UserDrawBox ItemView { get; private set; }
    public ListBox AbilityList { get; private set; }

    // AoWItemBanner.pas:46
    public void IBannerPopupShow()
    {
        Item item;
        int x;
        int y;
        item = StrategicMap.Current.ItemControl.FindItem(ItemId);
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
            x = 30;
            y = 100;
            if (item.Attack != 0)
            {
                AttackLabel.Text = IntToStr(item.Attack);
                AttackLabel.Visible = true;
                AttackIcon.Visible = true;
                AttackIcon.WinLeft = x + 10;
                AttackLabel.WinLeft = x + 30;
                AttackIcon.WinTop = y;
                AttackLabel.WinTop = y + 5;
                x += 50;
                if (x == 130)
                {
                    x = 30;
                    y += 20;
                }
            }

            if (item.Damage != 0)
            {
                DamageLabel.Text = IntToStr(item.Damage);
                DamageLabel.Visible = true;
                DamageIcon.Visible = true;
                DamageIcon.WinLeft = x + 10;
                DamageLabel.WinLeft = x + 30;
                DamageIcon.WinTop = y;
                DamageLabel.WinTop = y + 5;
                x += 50;
                if (x == 130)
                {
                    x = 30;
                    y += 20;
                }
            }

            if (item.Defense != 0)
            {
                DefenseLabel.Text = IntToStr(item.Defense);
                DefenseLabel.Visible = true;
                DefenseIcon.Visible = true;
                DefenseIcon.WinLeft = x + 10;
                DefenseLabel.WinLeft = x + 30;
                DefenseIcon.WinTop = y;
                DefenseLabel.WinTop = y + 5;
                x += 50;
                if (x == 130)
                {
                    x = 30;
                    y += 20;
                }
            }

            if (item.Resistance != 0)
            {
                ResistanceLabel.Text = IntToStr(item.Resistance);
                ResistanceLabel.Visible = true;
                ResistanceIcon.Visible = true;
                ResistanceIcon.WinLeft = x + 10;
                ResistanceLabel.WinLeft = x + 30;
                ResistanceIcon.WinTop = y;
                ResistanceLabel.WinTop = y + 5;
                x += 50;
                if (x == 130)
                {
                    x = 30;
                    y += 20;
                }
            }

            if (x != 30)
            {
                y += 20;
            }

            AbilityList.Items.Clear();
            AbilityList.Alignment.TopOffset = y + 10;
            x = 1;
            for (; x <= 169; x += 1)
            {
                if (item.GetAbEnabled(x))
                {
                    string name = GamePack.Current.AbilityControl.GetAbility(x).GetName(item);
                    _ = AbilityList.Items.AddObject(name, IntToObject(x));
                }
            }

            int count = AbilityList.Items.Count;
            BannerMenu.WinHeight = (count * AbilityList.ItemHeight) + y + 50;
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
            UI.ItemBanner.BannerMenu.Visible = false;
        }
    }

    // AoWItemBanner.pas:145
    public void DrawBoxPreDraw(LockedSurface lockedSurface)
    {
        Item item;
        item = StrategicMap.Current.ItemControl.FindItem(ItemId);
        item?.Show(ItemView.WinLeft, ItemView.WinTop, lockedSurface);
    }

    internal static int ItemId { get; set; }

}
