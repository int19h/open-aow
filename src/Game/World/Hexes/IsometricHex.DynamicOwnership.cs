using Aow.Game.Draw;

using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.World.Hexes;

public partial class IsometricHex
{
    protected void UpdateTransitionCore(Type peer, bool water, Action<int, int> shore = null)
    {
        OpenSides = 0;
        Sides &= 0xC0;
        var field = Owner;
        var level = field.Owner;
        HexNumberWalkData walk = default;
        HexNumberWalkData.InitializeHexNumberWalk(ref walk, field.HexX, field.HexY, 1);
        while (walk.HexNumber <= 6)
        {
            if (walk.X >= 0 && walk.Y >= 0 && walk.X < level.Width && walk.Y < level.Height)
            {
                var neighbour = level.GetField(walk.X, walk.Y);
                var sprite = neighbour.FindOwnedHexSprite(peer);
                bool peerFound = sprite != null;
                sprite ??= neighbour.FindOwnedHexSprite(typeof(BorderHex));
                if (sprite == null)
                {
                    OpenSides |= (byte)(1 << (walk.HexNumber - 1));
                    if (water) shore(walk.HexNumber, neighbour.TerrainType switch { 3 => 40, 5 => 80, _ => 0 });
                }
                else if ((!water || peerFound) && neighbour.TerrainType != GetTerrain(0))
                    Sides |= (byte)(1 << (walk.HexNumber - 1));
            }
            HexNumberWalkData.WalkToNextHexNumber(ref walk);
        }
        if (water || OpenSides != 0)
        {
            if (Dynamic == null) Dynamic = new DynamicIsometricHex(this);
        }
        else if (Dynamic != null)
        {
            Dynamic?.Dispose();
            Dynamic = null;
            Sides = (byte)(RtlUnit.Random(4) << 6);
            _resource.Engine.HexSpriteMap.InvalidateStaticScene(field.HexX, field.HexY, field.HexLevel);
        }
    }
}
