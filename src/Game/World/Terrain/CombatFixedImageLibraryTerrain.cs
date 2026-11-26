#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
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
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null)
        {
            bool validTerrainType = ValidTerrainType(info.MapField.TerrainNumber);
            if (validTerrainType)
            {
                validTerrainType = GetMostFrequentMapFieldTerrainType(defaultValue: base.Resource.TerrainList.GetTerrain(info.HexNumber).ToOriginal(), mapLevel: Owner.Owner, hexX: StoredHexX, hexY: StoredHexY) == info.MapField.TerrainNumber;
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
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null)
        {
            if (ValidTerrainType(info.TerrainNumber))
            {
                TerrainType = info.TerrainNumber;
                ImageIndex = GetTerrainTypeImage(info.TerrainNumber);
            }
        }
    }

    // CombatILTer.pas:216
    public override void TerrainChanged(in HexSpriteTerrainInfo info)
    {
        if (base.Resource.TerrainList.GetTerrain(info.HexNumber).Value is not null)
        {
            if (ValidTerrainType(info.TerrainNumber))
            {
                TerrainType = info.TerrainNumber;
                ImageIndex = GetTerrainTypeImage(info.TerrainNumber);
            }
            else
            {
                throw new InvalidOperationException("TILFixedTerrain Failure");
            }
        }
    }

    // CombatILTer.pas:194
    public override int ForceTerrainType(MapLevel mapLevel, int hexX, int hexY) => -1;

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
