#nullable disable
using Aow.Game.Draw;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class IsometricHex
{
    protected void UpdateTransitionCore(Type peer, bool water, Action<int, int> shore = null)
    {
        OpenSides = 0;
        Sides &= 0b1100_0000;
        MapField field = Owner;
        MapLevel level = field.Owner;
        HexNumberWalkData.InitializeHexNumberWalk(out HexNumberWalkData walk, field.HexX, field.HexY, radius: 1);
        while (walk.HexNumber <= 6)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                MapField neighbour = level.GetField(walk.X, walk.Y);
                HexSprite sprite = neighbour.FindOwnedHexSprite(peer);
                bool peerFound = sprite is not null;
                sprite ??= neighbour.FindOwnedHexSprite(typeof(BorderHex));
                if (sprite is null)
                {
                    OpenSides |= (byte)(1 << (walk.HexNumber - 1));
                    if (water)
                    {
                        shore(walk.HexNumber, neighbour.TerrainType switch { 3 => 40, 5 => 80, _ => 0 });
                    }
                }
                else if ((!water || peerFound) && neighbour.TerrainType != GetTerrain(0))
                {
                    Sides |= (byte)(1 << (walk.HexNumber - 1));
                }
            }
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        if (water || OpenSides != 0)
        {
            Dynamic ??= new DynamicIsometricHex(this);
        }
        else if (Dynamic is not null)
        {
            Dynamic?.Dispose();
            Dynamic = null;
            Sides = (byte)(DelphiRandom.Random(4) << 6);
            Resource.Engine.HexSpriteMap.InvalidateStaticScene(field.HexX, field.HexY, field.HexLevel);
        }
    }
}
