#nullable disable warnings
// Translated from AoWTer.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.World.Map;
using Aow.Game.World.Terrain;

namespace Aow.Game.World.Hexes;

public class Terrain(CustomNode owner) : TerrainMapObject(owner)
{

    // AoWTer.pas:89
    public override void MessageProc(ref EngineMessage message, EngineObject? sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985)
        {
            try
            {
                if (base.Resource.DecorationType == 3)
                {
                    ValidateDeadTreeRegrow();
                }
            }
            catch
            {
            }
        }
    }

    // AoWTer.pas:100
    // { Original ignores regrowth failures. }
    public override void MapFieldMessageProc(ref MapFieldMessage message)
    {
        base.MapFieldMessageProc(ref message);
        if (message.MessageId == 4386)
        {
            if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                message.Result = GetResource<Resource>().MapFieldResponse;
                return;
            }
        }
        else
        {
            if (message.MessageId == 4357)
            {
                if (base.Resource.TerrainList.Terrain[message.HexNumber] >= 0)
                {
                    ValidateDenseVegetationBurned(message.Param[0].As<Fire>());
                }
            }
        }
    }

    // AoWTer.pas:67
    public void ValidateDeadTreeRegrow()
    {
        if (Owner.FindOwnedChild(Fire.ClassId) is null)
        {
            if (StrategicMap.Current.Random(20) == 0)
            {
                TerrainMapObjectResource? r = FindCompatibleResource(base.Resource.TerrainType, overlayType: 1, decorationType: 0);
                if (r is not null)
                {
                    MapField oldOwner = Owner;
                    RemoveFromMap();
                    SetResource(r);
                    Owner = oldOwner;
                    _ = PlaceOnMap();
                    if (base.Resource.Level <= 15)
                    {
                        base.Resource.Engine.HexSpriteMap.StaticSceneInvalid = true;
                    }

                    OverrideTerrain();
                }
            }
        }
    }

    // AoWTer.pas:45. Message $1105 carries a TFire whose byte $1E is FAge.
    public void ValidateDenseVegetationBurned(Fire? info)
    {
        if (info.Age < 2 || base.Resource.TerrainList.Overlay[0] != 1)
        {
            return;
        }

        TerrainMapObjectResource? resource = FindCompatibleResource(base.Resource.TerrainType, overlayType: 3, decorationType: 3);
        if (resource is null)
        {
            return;
        }

        UpdateTerrain(Owner.Owner, HexX, HexY);
        MapField owner = Owner;
        RemoveFromMap();
        SetResource(resource);
        Owner = owner;
        _ = PlaceOnMap();
        if (base.Resource.Level <= 15)
        {
            base.Resource.Engine.HexSpriteMap.StaticSceneInvalid = true;
        }

        OverrideTerrain();
    }

    // MapFieldResponse (stream id 20)
    public new class Resource(CustomNode owner) : TerrainMapObjectResource(owner)
    {
        public int MapFieldResponse { get; private set; }

        // AoWTer.pas:116
        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            MapFieldResponse = persister.Int32("MapFieldResponse", value: MapFieldResponse);
        }
    }
}
