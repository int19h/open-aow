#nullable disable
using Aow.Game.Audio;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;

namespace Aow.Game.Ui.Dialogs;

public partial class UnitWindow
{
    // AoWUnitWindow.pas:773. The cursor belongs to the rewritten main form.
    public void Gnd1Change(AOWCheckBox check)
    {
        var hero = (Hero)DialogHosts.General.FSelectedUnit;
        if (hero is null || World.Map.AoWHSMap.Current.FPlayers.GetPlayers(hero.FPlayer).GetBusy() || !check.Checked) return;
        FCursorImage = null;
        var selected = SelectedItem;
        if (check != selected)
        {
            if (selected is not null)
            {
                AudioPlayback.PlaySample("SFX\\Events\\NewEvent.Wav");
                if (check.Tag != -1) hero.SwapItem(selected.Tag, check.Tag);
                else
                {
                    HeroItemPosition position = check == Head ? HeroItemPosition.HeadSlot :
                        check == Torso ? HeroItemPosition.TorsoSlot :
                        check == Attack ? HeroItemPosition.AttackSlot :
                        check == Defend ? HeroItemPosition.DefenseSlot :
                        check == RingR ? HeroItemPosition.RightRingSlot :
                        check == RingL ? HeroItemPosition.LeftRingSlot :
                        check == BP1 ? HeroItemPosition.InventorySlot1 :
                        check == BP2 ? HeroItemPosition.InventorySlot2 :
                        check == BP3 ? HeroItemPosition.InventorySlot3 :
                        check == BP4 ? HeroItemPosition.InventorySlot4 :
                        check == BP5 ? HeroItemPosition.InventorySlot5 :
                        check == BP6 ? HeroItemPosition.InventorySlot6 :
                        check == BP7 ? HeroItemPosition.InventorySlot7 :
                        check == BP8 ? HeroItemPosition.InventorySlot8 : HeroItemPosition.Ground;
                    hero.PlaceItem(selected.Tag, position);
                }
                check.Checked = false;
                selected.Checked = false;
                ClearItemSelection();
            }
            else if (check.Tag != -1)
            {
                SelectedItem = check;
                FHighlight = 50;
                FHighlightStep = 10;
                var item = hero.FInventory.FindItem(check.Tag) ?? hero.FHeroItems.FindItem(check.Tag) ?? hero.GetItemsOnGround()?.FindItem(check.Tag);
                if (item is not null)
                {
                    FCursorImage = item.GetImage();
                    FItemType = item.FItemType;
                    ItemUseBtn.Visible = item.FItemType == ItemType.Scroll;
                }
                else
                {
                    FItemType = ItemType.Use;
                    ItemUseBtn.Visible = false;
                }
                FBlinkDelay = 2;
                FItemVisible = true;
            }
            else check.Checked = false;
        }
        else
        {
            selected.Checked = false;
            ClearItemSelection();
        }
        GameHost.MainForm.FCursorImage = FCursorImage;
    }

    private void ClearItemSelection()
    {
        SelectedItem = null;
        UnitWin.Cursor = 0;
        FItemType = ItemType.Use;
        ItemUseBtn.Visible = false;
        FItemVisible = false;
    }
}
