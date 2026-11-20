// Translated from CombatILTer.pas.
using Aow.Engine.Nodes;

using Aow.Game.World.Map;

namespace Aow.Game.World.Terrain;

public class CombatFixedImageLibraryTerrain(CustomNode owner) : CombatImageLibraryTerrain(owner)
{

    // CombatILTer.pas:191
    public new const int ClassId = 2_228_536;

    // CombatILTer.pas:197
    public override bool CanChangeTerrain(in HexSpriteSenderInfo info)
    {
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            bool validTerrainType = ValidTerrainType(info.MapField.TerrainType);
            if (validTerrainType)
            {
                validTerrainType = GetMostFrequentMapFieldTerrainType(defaultValue: base.Resource.TerrainList.GetTerrain(info.HexNumber), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY) == info.MapField.TerrainType;
            }

            return validTerrainType;
        }
        else
        {
            return true;
        }
    }

    // CombatILTer.pas:206
    public override void ChangeTerrain(in HexSpriteTerrainInfo info)
    {
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                ImageIndex = GetTerrainTypeImage(info.TerrainType);
            }
        }
    }

    // CombatILTer.pas:216
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber) >= 0)
        {
            if (ValidTerrainType(info.TerrainType))
            {
                TerrainType = info.TerrainType;
                ImageIndex = GetTerrainTypeImage(info.TerrainType);
            }
            else
            {
                throw new InvalidOperationException("TILFixedTerrain Failure");
            }
        }
    }

    // CombatILTer.pas:194
    public override sbyte ForceTerrainType(MapLevel mapLevel, sbyte hexX, sbyte hexY) => -1;

    public new class Resource(CustomNode owner) : CombatImageLibraryTerrainResource(owner)
    {

        // CombatILTer.pas:227
        public new const int ClassId = 2_228_537;

        // CombatILTer.pas:233
        public override int ResourceUserClassId() => CombatFixedImageLibraryTerrain.ClassId;

        // CombatILTer.pas:230
        public override string ResourceEditName() => "Combat FixedILTerrain";

    }
}
