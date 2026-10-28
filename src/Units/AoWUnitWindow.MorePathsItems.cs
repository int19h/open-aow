#nullable disable
using Aow.Rtl;
using Aow.Units.AoWE;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWUnitWindow;

public partial class TUnitWindow
{
    // AoWUnitWindow.pas:773. The cursor belongs to the rewritten main form.
    public void Gnd1Change(TObject sender)
    {
        var check = (TAOWCheckBox)sender;
        var hero = (THero)General.FSelectedUnit;
        if (hero is null || AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(hero.FPlayer).GetBusy() || !check.Checked) return;
        FCursorImage = null;
        var selected = AoWUnitWindow.SelectedItem;
        if (check != selected)
        {
            if (selected is not null)
            {
                Sound.Sound.PlaySample((AnsiString)"SFX\\Events\\NewEvent.Wav");
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
                AoWUnitWindow.SelectedItem = check;
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
        Aow.Platform.GameHost.MainForm.FCursorImage = FCursorImage;
    }

    private void ClearItemSelection()
    {
        AoWUnitWindow.SelectedItem = null;
        UnitWin.Cursor = 0;
        FItemType = TItemType.itUse;
        ItemUseBtn.Visible = false;
        FItemVisible = false;
    }
}
