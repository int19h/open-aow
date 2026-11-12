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
        var hero = (Hero)DialogHosts.General.SelectedUnit;
        if (hero is null || AoWHSMap.Current.Players.GetPlayers(hero.Player).GetBusy() || !check.Checked) return;
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
                s_selectedItem = check;
                _highlight = 50;
                _highlightStep = 10;
                var item = hero.Inventory.FindItem(check.Tag) ?? hero.HeroItems.FindItem(check.Tag) ?? hero.GetItemsOnGround()?.FindItem(check.Tag);
                if (item is not null)
                {
                    _cursorImage = item.GetImage();
                    _itemType = item.ItemType;
                    ItemUseBtn.Visible = item.ItemType == ItemType.Scroll;
                }
                else
                {
                    _itemType = ItemType.Use;
                    ItemUseBtn.Visible = false;
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
        UnitWin.Cursor = 0;
        _itemType = ItemType.Use;
        ItemUseBtn.Visible = false;
        _itemVisible = false;
    }
}
