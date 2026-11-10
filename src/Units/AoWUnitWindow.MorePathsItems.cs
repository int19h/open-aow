#nullable disable
using Aow.Game.Audio;
using Aow.Game.Host;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using static Aow.Game.Ui.Dialogs.AoWWinManUnit;

namespace Aow.Game.Ui.Dialogs;

public partial class TUnitWindow
{
    // AoWUnitWindow.pas:773. The cursor belongs to the rewritten main form.
    public void Gnd1Change(TAOWCheckBox check)
    {
        var hero = (THero)General.FSelectedUnit;
        if (hero is null || AoWEUnit.AoWHSMap.FPlayers.GetPlayers(hero.FPlayer).GetBusy() || !check.Checked) return;
        FCursorImage = null;
        var selected = AoWUnitWindowUnit.SelectedItem;
        if (check != selected)
        {
            if (selected is not null)
            {
                SoundUnit.PlaySample("SFX\\Events\\NewEvent.Wav");
                if (check.Tag != -1) hero.SwapItem(selected.Tag, check.Tag);
                else
                {
                    THeroItemPosition position = check == Head ? THeroItemPosition.ipHeadSlot :
                        check == Torso ? THeroItemPosition.ipTorsoSlot :
                        check == Attack ? THeroItemPosition.ipAttackSlot :
                        check == Defend ? THeroItemPosition.ipDefenseSlot :
                        check == RingR ? THeroItemPosition.ipRightRingSlot :
                        check == RingL ? THeroItemPosition.ipLeftRingSlot :
                        check == BP1 ? THeroItemPosition.ipInventorySlot1 :
                        check == BP2 ? THeroItemPosition.ipInventorySlot2 :
                        check == BP3 ? THeroItemPosition.ipInventorySlot3 :
                        check == BP4 ? THeroItemPosition.ipInventorySlot4 :
                        check == BP5 ? THeroItemPosition.ipInventorySlot5 :
                        check == BP6 ? THeroItemPosition.ipInventorySlot6 :
                        check == BP7 ? THeroItemPosition.ipInventorySlot7 :
                        check == BP8 ? THeroItemPosition.ipInventorySlot8 : THeroItemPosition.ipGround;
                    hero.PlaceItem(selected.Tag, position);
                }
                check.Checked = false;
                selected.Checked = false;
                ClearItemSelection();
            }
            else if (check.Tag != -1)
            {
                AoWUnitWindowUnit.SelectedItem = check;
                FHighlight = 50;
                FHighlightStep = 10;
                var item = hero.FInventory.FindItem(check.Tag) ?? hero.FHeroItems.FindItem(check.Tag) ?? hero.GetItemsOnGround()?.FindItem(check.Tag);
                if (item is not null)
                {
                    FCursorImage = item.GetImage();
                    FItemType = item.FItemType;
                    ItemUseBtn.Visible = item.FItemType == TItemType.itScroll;
                }
                else
                {
                    FItemType = TItemType.itUse;
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
        AoWUnitWindowUnit.SelectedItem = null;
        UnitWin.Cursor = 0;
        FItemType = TItemType.itUse;
        ItemUseBtn.Visible = false;
        FItemVisible = false;
    }
}
