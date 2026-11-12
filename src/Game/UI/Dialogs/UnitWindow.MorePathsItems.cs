#nullable disable
using Aow.Game.Audio;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.UI.Dialogs;

public partial class UnitWindow
{
    // AoWUnitWindow.pas:773. The cursor belongs to the rewritten main form.
    public void Gnd1Change(CheckBox check)
    {
        var hero = (Hero)UI.General.SelectedUnit;
        if (hero is null || StrategicMap.Current.Players.GetPlayers(hero.Player).GetBusy() || !check.Checked) return;
        _cursorImage = null;
        var selected = s_selectedItem;
        if (check != selected)
        {
            if (selected is not null)
            {
                AudioPlayback.PlaySample("SFX\\Events\\NewEvent.Wav");
                if (check.Tag != -1) hero.SwapItem(selected.Tag, check.Tag);
                else
                {
                    HeroItemPosition position = check == HeadSlotCheckBox ? HeroItemPosition.HeadSlot :
                        check == TorsoSlotCheckBox ? HeroItemPosition.TorsoSlot :
                        check == AttackSlotCheckBox ? HeroItemPosition.AttackSlot :
                        check == DefendSlotCheckBox ? HeroItemPosition.DefenseSlot :
                        check == RightRingCheckBox ? HeroItemPosition.RightRingSlot :
                        check == LeftRingCheckBox ? HeroItemPosition.LeftRingSlot :
                        check == Backpack1CheckBox ? HeroItemPosition.InventorySlot1 :
                        check == Backpack2CheckBox ? HeroItemPosition.InventorySlot2 :
                        check == Backpack3CheckBox ? HeroItemPosition.InventorySlot3 :
                        check == Backpack4CheckBox ? HeroItemPosition.InventorySlot4 :
                        check == Backpack5CheckBox ? HeroItemPosition.InventorySlot5 :
                        check == Backpack6CheckBox ? HeroItemPosition.InventorySlot6 :
                        check == Backpack7CheckBox ? HeroItemPosition.InventorySlot7 :
                        check == Backpack8CheckBox ? HeroItemPosition.InventorySlot8 : HeroItemPosition.Ground;
                    hero.PlaceItem(selected.Tag, position);
                }
                check.Checked = false;
                selected.Checked = false;
                ClearItemSelection();
            }
            else if (check.Tag != -1)
            {
                s_selectedItem = check;
                _highlight = 50;
                _highlightStep = 10;
                var item = hero.Inventory.FindItem(check.Tag) ?? hero.HeroItems.FindItem(check.Tag) ?? hero.GetItemsOnGround()?.FindItem(check.Tag);
                if (item is not null)
                {
                    _cursorImage = item.GetImage();
                    _itemType = item.ItemType;
                    ItemUseButton.Visible = item.ItemType == ItemType.Scroll;
                }
                else
                {
                    _itemType = ItemType.Use;
                    ItemUseButton.Visible = false;
                }
                _blinkDelay = 2;
                _itemVisible = true;
            }
            else check.Checked = false;
        }
        else
        {
            selected.Checked = false;
            ClearItemSelection();
        }
        GameHost.MainForm.FCursorImage = _cursorImage;
    }

    private void ClearItemSelection()
    {
        s_selectedItem = null;
        MainWindow.Cursor = 0;
        _itemType = ItemType.Use;
        ItemUseButton.Visible = false;
        _itemVisible = false;
    }
}
