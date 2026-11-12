// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public partial class PlayerStructureResource : StructureResource
{
    public PlayerStructureResource(ECustomNode owner) : base(owner)
    {
        Claimed = true;
        FlagX = 20;
        FlagY = -35;
    }

    public new const int ClassId = 131917;

    public bool Claimed;
    public int FlagX;
    public int FlagY;
    public FlagControl FlagControl;

    // Structur.PAS:2456
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        if (stream.StorageMode().Contains(StorageModeElement.Read))
        {
            GenerateFlagPositions();
        }
    }

    // Structur.PAS:2422
    // (claimed, flag position, flag control)

    // Structur.PAS:2467
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 536870920)
        {
            ActivateMessage();
        }
    }

    // Structur.PAS:2440
    public override int ResourceUserClassId() => 131916;

    // Structur.PAS:2473
    public override MapObject PlaceResourceUser(HSMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        MapObject result = base.PlaceResourceUser(hexSpriteMap, hexX, hexY, hexLevel);
        if (result != null && AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Editting))
        {
            ((PlayerStructure)result).SetPlayer(AoWHSMap.Current.EditorPlayer);
        }

        return result;
    }

    // Structur.PAS:2445
    public virtual void GenerateFlagPositions()
    {
        if (TerrainBound)
        {
            switch (FlagType)
            {
                case 1:
                    FlagX = 20;
                    FlagY = -30;
                    break;
                case 2:
                    FlagX = 35;
                    FlagY = -50;
                    break;
                case 3:
                    FlagX = 5;
                    FlagY = -45;
                    break;
                case 4:
                    FlagX = 30;
                    FlagY = -60;
                    break;
            }
        }
    }

    // Structur.PAS:2462
    public void ActivateMessage()
    {
        FlagControl = ((AoWHSSet)Engine.HexSpriteSet).FlagControl;
    }

    // intf_P1_09.inc:300

}
