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

    public new const int ClassID = 131698;

    public int FItemID;
    public byte FAction;
    // 0 = use
    public int FHeroID;

    // Army.PAS:4185
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref FItemID, 20);
        Stream.rwData(Bytes.Of(ref FAction, 1), 1, 21);
        Stream.rwInteger(ref FHeroID, 22);
    }

    // Army.PAS:4193
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        ExecuteResult Result;
        bool Done;
        Item Item;
        Hero Hero;
        base.Execute(TE);
        Done = false;
        Item = AoWHSMap.Current.FItemControl.FindItem(FItemID);
        if (Item != null)
        {
            switch (FAction)
            {
                case 0:
                    Hero = AoWHSMap.Current.FHeroControl.FindID(FHeroID);
                    if (Hero != null)
                    {
                        Done = Item.ExecuteUse(Hero);
                    }
                    else
                    {
                        Done = false;
                    }

                    break;
            }
        }

        if (Done)
        {
            Result = ExecuteResult.Done;
        }
        else
        {
            Result = ExecuteResult.DoneHalt;
        }

        return Result;
    }

    // NetworkE.pas:230

}
