#nullable disable warnings
// Translated from SmokeCloud.pas.
using Aow.Engine.Nodes;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;

namespace Aow.Game.Magic.Spells;

// Cloud
public class SmokeCloudEnchantment : GlobalEnchantment
{
    // SmokeCloud.pas:99
    public new const int ClassId = 135_748;

    public SmokeCloud Cloud { get; init; }

    // SmokeCloud.pas:91
    public SmokeCloudEnchantment(CustomNode? owner) : base(owner)
    {
        EnchantmentMode = (GlobalEnchantmentModes)(ulong)((Options)(ulong)EnchantmentMode & ~(Options.Bit0 | Options.Bit1));
        DispelMana = 25;
    }

    // SmokeCloud.pas:102
    public override string GetName() => GamePack.Current.SpellControl[74].SpellName;

    // SmokeCloud.pas:156
    public override bool VisibleForPlayer(sbyte player)
    {
        HexNumberWalkData hexNumberWalkData = default;
        bool result = PlayerIndex == player;
        ref HexNumberWalkData walk = ref hexNumberWalkData;
        int hexY = Cloud.HexY;
        int hexX = Cloud.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(out hexNumberWalkData, hexX, hexY, radius: 0);
        while (!result && walk.Radius <= 3)
        {
            result = StrategicMap.Current.VisibleForPlayer(walk.X, walk.Y, Cloud.HexLevel, player);
            HexNumberWalkData.WalkToNextHexNumber(ref hexNumberWalkData);
        }

        return result;
    }

    // The unused fourth byte in the original register result has no defined value.
    public override MapCoordinate GetLocation() => new() { X = Cloud.HexX, Y = Cloud.HexY, MapLevel = Cloud.HexLevel };

    public override bool Deactivate(byte player)
    {
        if (((ulong)EnchantmentStatus & 1) == 0)
        {
            return false;
        }

        bool result = base.Deactivate(player);
        if (((ulong)Cloud.Status & 0x20) != 0)
        {
            Cloud?.Dispose();
        }

        return result;
    }

    [Flags]
    public enum Options : byte
    {
        None = 0,
        Bit0 = 1 << 0,
        Bit1 = 1 << 1,
        Bit2 = 1 << 2,
        Bit3 = 1 << 3,
        Bit4 = 1 << 4,
        Bit5 = 1 << 5,
        Bit6 = 1 << 6,
        Bit7 = 1 << 7,
    }
}
