#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;

namespace Aow.Game.World.Structures;

public class PlayerStructureResource(CustomNode owner) : StructureResource(owner)
{

    public new const int ClassId = 131_917;

    public bool Claimed { get; protected init; } = true;
    public int FlagX { get; private set; } = 20;
    public int FlagY { get; private set; } = -35;
    public FlagControl FlagControl { get; private set; }

    // Structur.PAS:2456
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        // The translate stream's mode holds read, so the original generates the positions there too.
        if (persister.Loading || persister.Translating())
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
    public override int ResourceUserClassId() => PlayerStructure.ClassId;

    // Structur.PAS:2473
    public override PlayerStructure PlaceResourceUser(GameMap hexSpriteMap, sbyte hexX, sbyte hexY, sbyte hexLevel)
    {
        // The user class is a player structure (131916) or one of its kinds. Another class throws.
        PlayerStructure result = PlaceCreatedUser<PlayerStructure>(hexSpriteMap, hexX, hexY, hexLevel);
        if (result is not null && StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Editting))
        {
            result.SetPlayer(StrategicMap.Current.EditorPlayer);
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
                default:
                    break;
            }
        }
    }

    // Structur.PAS:2462
    public void ActivateMessage() => FlagControl = Engine.HexSpriteSet.FlagControl;
}
