// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class PlayerStructureResource : StructureResource
{
    public PlayerStructureResource(ECustomNode owner) : base(owner)
    {
        FClaimed = true;
        FFlagX = 20;
        FFlagY = -35;
    }

    public new const int ClassID = 131917;

    public bool FClaimed;
    public int FFlagX;
    public int FFlagY;
    public FlagControl FFlagControl;

    // Structur.PAS:2456
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        if (Stream.StorageMode().Contains(StorageModeElement.Read))
        {
            GenerateFlagPositions();
        }
    }

    // Structur.PAS:2422
    // (claimed, flag position, flag control)

    // Structur.PAS:2467
    public override void MsgProc(ref EMsg Msg, EObject Sender)
    {
        base.MsgProc(ref Msg, Sender);
        if (Msg.MsgID == 536870920)
        {
            ActivateMsg();
        }
    }

    // Structur.PAS:2440
    public override int ResourceUserClassID() => 131916;

    // Structur.PAS:2473
    public override MapObject PlaceResourceUser(HSMap HSMap, sbyte Xhx, sbyte Yhx, sbyte Lhx)
    {
        MapObject Result = base.PlaceResourceUser(HSMap, Xhx, Yhx, Lhx);
        if (Result != null && AoWHSMap.Current.Status.Contains(HSMapStatusElement.Editting))
        {
            ((PlayerStructure)Result).SetPlayer(AoWHSMap.Current.FEditorPlayer);
        }

        return Result;
    }

    // Structur.PAS:2445
    public virtual void GenerateFlagPositions()
    {
        if (FTerrainBound)
        {
            switch (FFlagType)
            {
                case 1:
                    FFlagX = 20;
                    FFlagY = -30;
                    break;
                case 2:
                    FFlagX = 35;
                    FFlagY = -50;
                    break;
                case 3:
                    FFlagX = 5;
                    FFlagY = -45;
                    break;
                case 4:
                    FFlagX = 30;
                    FFlagY = -60;
                    break;
            }
        }
    }

    // Structur.PAS:2462
    public void ActivateMsg()
    {
        FFlagControl = ((AoWHSSet)Engine.HSSet).FFlagControl;
    }

    // intf_P1_09.inc:300

}
