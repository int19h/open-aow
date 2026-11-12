// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public partial class CombatFixedImageLibraryTerrain : CombatImageLibraryTerrain
{
    public CombatFixedImageLibraryTerrain(ECustomNode owner) : base(owner)
    { }

    // CombatILTer.pas:191
    public new const int ClassId = 2228536;

    // CombatILTer.pas:197
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        bool result;
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            bool validTerrainType = ValidTerrainType(info.MapField.TerrainType);
            if (validTerrainType)
            {
                var terrain = _resource.TerrainList.GetTerrain(info.HexNumber);
                validTerrainType = GetMostFrequentMapFieldTerrainType((MapLevel)Owner.Owner, _hexX, _hexY, terrain) == info.MapField.TerrainType;
            }

            result = validTerrainType;
        }
        else
        {
            result = true;
        }

        return result;
    }

    // CombatILTer.pas:206
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                _terrainType = info.TerrainType;
                _imageIndex = GetTerrainTypeImage(info.TerrainType);
            }
        }
    }

    // CombatILTer.pas:216
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (_resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                _terrainType = info.TerrainType;
                _imageIndex = GetTerrainTypeImage(info.TerrainType);
            }
            else
            {
                throw new InvalidOperationException("TILFixedTerrain Failure");
            }
        }
    }

    // CombatILTer.pas:194
    public override sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY) => -1;

    public new partial class Resource : CombatImageLibraryTerrainResource
    {
        public Resource(ECustomNode owner) : base(owner)
        { }

        // CombatILTer.pas:227
        public new const int ClassId = 2228537;

        // CombatILTer.pas:233
        public override int ResourceUserClassId() => 2228536;

        // CombatILTer.pas:230
        public override string ResourceEditName() => "Combat FixedILTerrain";

    }

}
