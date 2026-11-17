// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Game.Magic.Abilities;
using Aow.Game.Net;
using Aow.Game.World.Map;

namespace Aow.Game.Commands;

// item ID, action, hero ID
public partial class ItemTokenEvent(CustomNode owner) : TokenEvent(owner)
{
    public new const int ClassId = 131698;

    public int ItemId { get; set; }

    private byte _action;

    // 0 = use
    public int HeroId { get; set; }

    // Army.PAS:4185
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        ItemId = persister.Int32("ItemId", value: ItemId);
        _action = persister.RawByte("Action", value: _action);
        HeroId = persister.Int32("HeroId", value: HeroId);
    }

    // Army.PAS:4193
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        bool done;
        Item item;
        Hero hero;
        _ = base.Execute(tokenEvent);
        done = false;
        item = StrategicMap.Current.ItemControl.FindItem(ItemId);
        if (item != null)
        {
            switch (_action)
            {
                case 0:
                    hero = StrategicMap.Current.HeroControl.FindId(HeroId);
                    done = hero != null && item.ExecuteUse(hero);

                    break;
                default:
                    break;
            }
        }

        result = done ? ExecuteResult.Done : ExecuteResult.DoneHalt;

        return result;
    }

    // NetworkE.pas:230

}
