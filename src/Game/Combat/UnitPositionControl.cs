// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// Positions: packed MapCoordinate entries, eight per army position
public partial class UnitPositionControl : EngineObject
{
    public MapCoordinateList Positions;

    // AoWTC.pas:4943
    public override void Dispose()
    {
        Positions?.Dispose();
        Positions = null;
        base.Dispose();
    }

    // AoWTC.pas:4936
    public UnitPositionControl(ECustomNode owner) : base(owner)
    {
        Positions = new MapCoordinateList();
        var packMapCoordinate = HexGeometry.PackMapCoordinate(-1, -1, -1);
        Positions.Default = packMapCoordinate;
    }

    // AoWTC.pas:4990
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        UnitPositionHexSprite hexSprite;
        int X = default;
        int Y = default;
        int L;
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65541)
        {
            L = message.Data[1];
            HexGeometry.PixelToHex(message.Payload.Int, message.Data[0], ref X, ref Y);
            if (CombatMap.Current.MapContainer.GetField(X, Y, L).TerrainType != 15)
            {
                hexSprite = (UnitPositionHexSprite)CombatMap.Current.MapContainer.GetField(X, Y, L).FindOwnedChild(2228483);
                if (hexSprite == null)
                {
                    hexSprite = new UnitPositionHexSprite(null);
                    if (!CombatMap.Current.PlaceHex(hexSprite, X, Y, L))
                    {
                        hexSprite?.Dispose();
                        hexSprite = null;
                    }
                }

                if (hexSprite != null)
                {
                }
            }
        }
    }

    // AoWTC.pas:4950
    public MapCoordinate GetUnitPosition(int position, int index)
    {
        MapCoordinate result = default;
        int I;
        I = position * 8 + index;
        if (I < Positions.Count)
        {
            result = Positions[I];
        }
        else
        {
            result.X = -1;
            result.Y = -1;
            result.MapLevel = -1;
        }

        return result;
    }

    // AoWTC.pas:4963
    public void SetUnitPosition(int position, int index, MapCoordinate value)
    {
        if (position < 7 && index < 8)
        {
            Positions[position * 8 + index] = value;
        }
    }

    // AoWTC.pas:4969
    public bool ValidatePosition(ref sbyte position, ref sbyte index)
    {
        bool result = Positions[position * 8 + index].X == -1;
        if (!result)
        {
            index = 0;
            while (Positions[position * 8 + index].X != -1 && index != 8)
            {
                index = (sbyte)(index + 1);
            }

            result = index != 8;
        }

        if (!result)
        {
            position = 0;
            while (!result && position < 7)
            {
                while (Positions[position * 8 + index].X != -1 && index != 8)
                {
                    index = (sbyte)(index + 1);
                }

                result = index != 8;
                if (!result)
                {
                    index = 0;
                    position = (sbyte)(position + 1);
                }
            }
        }

        return result;
    }

}
