// Translated from AoWTC.pas.
using Aow.Engine.Nodes;

using Aow.Game.Draw;

using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// Positions: the unit positions, eight per army position, each with the direction the unit faces
public partial class UnitPositionControl : EngineObject
{
    public DefaultItemList<UnitPosition> Positions { get; private set; }

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
        Positions = new DefaultItemList<UnitPosition> { Default = UnitPosition.None };
    }

    // AoWTC.pas:4990
    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        UnitPositionHexSprite hexSprite;
        int x = default;
        int y = default;
        int l;
        base.MessageProc(ref message, sender);
        if (message.MessageId == 65541)
        {
            l = message.Data[1];
            HexGeometry.PixelToHex(message.Payload.Int, message.Data[0], out x, out y);
            if (CombatMap.Current.MapContainer.GetField(x, y, l).TerrainType != 15)
            {
                hexSprite = (UnitPositionHexSprite)CombatMap.Current.MapContainer.GetField(x, y, l).FindOwnedChild(2228483);
                if (hexSprite == null)
                {
                    hexSprite = new UnitPositionHexSprite(null) { Holder = CreationHolder() };
                    if (!CombatMap.Current.PlaceHex(hexSprite, x, y, l))
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
    public UnitPosition GetUnitPosition(int position, int index)
    {
        int i = position * 8 + index;
        return i < Positions.Count ? Positions[i] : UnitPosition.None;
    }

    // AoWTC.pas:4963
    public void SetUnitPosition(int position, int index, UnitPosition value)
    {
        if (position < 7 && index < 8)
        {
            Positions[position * 8 + index] = value;
        }
    }

    // AoWTC.pas:4969
    public bool ValidatePosition(ref sbyte position, ref sbyte index)
    {
        bool result = Positions[position * 8 + index].Location.X == -1;
        if (!result)
        {
            index = 0;
            while (Positions[position * 8 + index].Location.X != -1 && index != 8)
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
                while (Positions[position * 8 + index].Location.X != -1 && index != 8)
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
