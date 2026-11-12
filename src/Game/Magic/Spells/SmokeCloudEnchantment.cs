// Translated from SmokeCloud.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// Cloud
public partial class SmokeCloudEnchantment : GlobalEnchantment
{
    // SmokeCloud.pas:99
    public new const int ClassId = 135748;

    public SmokeCloud Cloud;

    // SmokeCloud.pas:91
    public SmokeCloudEnchantment(ECustomNode owner) : base(owner)
    {
        EnchantmentMode = Set<GlobalEnchantmentModeElement>.FromMask((Set<OptionsElement>.FromMask(EnchantmentMode.Mask) - Set<OptionsElement>.Of(OptionsElement.Bit0, OptionsElement.Bit1)).Mask);
        DispelMana = 25;
    }

    // SmokeCloud.pas:102
    public override string GetName() => AoWHSSet.Current.SpellControl.GetSpell(74).SpellName;

    // SmokeCloud.pas:156
    public override bool VisibleForPlayer(sbyte player)
    {
        HexNumberWalkData Walk = default;
        bool result = PlayerIndex == player;
        ref var walk = ref Walk;
        var hexY = Cloud.HexY;
        var hexX = Cloud.HexX;
        HexNumberWalkData.InitializeHexNumberWalk(ref Walk, hexX, hexY, 0);
        while (!result && walk.Radius <= 3)
        {
            result = AoWHSMap.Current.VisibleForPlayer(walk.X, walk.Y, Cloud.HexLevel, player);
            HexNumberWalkData.WalkToNextHexNumber(ref Walk);
        }

        return result;
    }

    // The unused fourth byte in the original register result has no defined value.
    public override MapCoordinate GetLocation() => new() { X = Cloud.GetHexX(), Y = Cloud.GetHexY(), MapLevel = Cloud.GetHexLevel() };

    public override bool Deactivate(byte player)
    {
        if ((EnchantmentStatus.Image & 1) == 0) return false;
        bool result = base.Deactivate(player);
        if ((Cloud._status.Image & 0x20) != 0) Cloud?.Dispose();
        return result;
    }

    public enum OptionsElement : byte
    {
        Bit0 = 0, Bit1 = 1, Bit2 = 2, Bit3 = 3, Bit4 = 4, Bit5 = 5, Bit6 = 6, Bit7 = 7
    }
}
