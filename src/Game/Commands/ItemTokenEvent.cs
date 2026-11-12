// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Magic.Abilities;

using Aow.Game.Net;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Commands;

// item ID, action, hero ID
public partial class ItemTokenEvent : TokenEvent
{
    public ItemTokenEvent(ECustomNode owner) : base(owner)
    { }

    public new const int ClassId = 131698;

    public int ItemId;
    public byte Action;
    // 0 = use
    public int HeroId;

    // Army.PAS:4185
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref ItemId, 20);
        stream.rwData(Bytes.Of(ref Action, 1), 1, 21);
        stream.rwInteger(ref HeroId, 22);
    }

    // Army.PAS:4193
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        ExecuteResult result;
        bool done;
        Item item;
        Hero hero;
        base.Execute(tokenEvent);
        done = false;
        item = AoWHSMap.Current.ItemControl.FindItem(ItemId);
        if (item != null)
        {
            switch (Action)
            {
                case 0:
                    hero = AoWHSMap.Current.HeroControl.FindId(HeroId);
                    if (hero != null)
                    {
                        done = item.ExecuteUse(hero);
                    }
                    else
                    {
                        done = false;
                    }

                    break;
            }
        }

        if (done)
        {
            result = ExecuteResult.Done;
        }
        else
        {
            result = ExecuteResult.DoneHalt;
        }

        return result;
    }

    // NetworkE.pas:230

}
