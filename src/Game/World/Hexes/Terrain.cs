// Translated from AoWTer.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.World.Map;
using Aow.Game.World.Terrain;

namespace Aow.Game.World.Hexes;

public partial class Terrain : TerrainMapObject
{
    public Terrain(ECustomNode owner) : base(owner)
    { }

    // AoWTer.pas:89
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        if (message.MessageId == 537001985)
        {
            try
            {
                if (((TerrainMapObjectResource)_resource).DecorationType == 3)
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
            if (_resource.TerrainList.Terrain[message.HexNumber] >= 0)
            {
                message.Result = ((Resource)_resource).MapFieldResponse;
                return;
            }
        }
        else
        {
            if (message.MessageId == 4357)
            {
                if (_resource.TerrainList.Terrain[message.HexNumber] >= 0)
                {
                    ValidateDenseVegetationBurned(message.Param[0].As<Fire>());
                }
            }
        }
    }

    // AoWTer.pas:67
    public void ValidateDeadTreeRegrow()
    {
        TerrainMapObjectResource R;
        MapField oldOwner;
        if (Owner.FindOwnedChild(132990) == null)
        {
            if (StrategicMap.Current.Random(20) == 0)
            {
                R = FindCompatibleResource(((TerrainMapObjectResource)_resource).TerrainType, 1, 0);
                if (R != null)
                {
                    oldOwner = Owner;
                    RemoveFromMap();
                    SetResource(R);
                    Owner = oldOwner;
                    PlaceOnMap();
                    if (_resource.Level <= 15)
                    {
                        _resource.Engine.HexSpriteMap.StaticSceneInvalid = true;
                    }

                    OverrideTerrain();
                }
            }
        }
    }

    // AoWTer.pas:45. Message $1105 carries a TFire whose byte $1E is FAge.
    public void ValidateDenseVegetationBurned(Fire info)
    {
        if (info.Age < 2 || _resource.TerrainList.Overlay[0] != 1) return;
        var resource = FindCompatibleResource(((TerrainMapObjectResource)_resource).TerrainType, 3, 3);
        if (resource == null) return;
        UpdateTerrain(Owner.Owner, GetHexX(), GetHexY());
        var owner = Owner;
        RemoveFromMap();
        SetResource(resource);
        Owner = owner;
        PlaceOnMap();
        if (_resource.Level <= 15) _resource.Engine.HexSpriteMap.StaticSceneInvalid = true;
        OverrideTerrain();
    }

    // MapFieldResponse (stream id 20)
    public new partial class Resource : TerrainMapObjectResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        public int MapFieldResponse;

        // AoWTer.pas:116
        public override void ReadWrite(StorageStream stream)
        {
            base.ReadWrite(stream);
            stream.Int32(ref MapFieldResponse, 20);
        }

        public override void PersistFields(IPersister persister)
        {
            base.PersistFields(persister);
            MapFieldResponse = persister.Int32("MapFieldResponse", value: MapFieldResponse);
        }
    }
}
