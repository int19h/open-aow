// Translated from SmokeCloud.pas.
using Aow.Engine.Nodes;

using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// FCloud
public partial class SmokeCloudEnchantment : GlobalEnchantment
{
    // SmokeCloud.pas:99
    public new const int ClassID = 135748;

    public SmokeCloud FCloud;

    // SmokeCloud.pas:91
    public SmokeCloudEnchantment(ECustomNode owner) : base(owner)
    {
        FEnchantmentMode = Set<GlobalEnchantmentModeElement>.FromMask((Set<CloudEnchantmentOptionsElement>.FromMask(FEnchantmentMode.Mask) - Set<CloudEnchantmentOptionsElement>.Of(CloudEnchantmentOptionsElement.Bit0, CloudEnchantmentOptionsElement.Bit1)).Mask);
        FDispelMana = 25;
    }

    // SmokeCloud.pas:102
    public override string GetName() => AoWHSSet.Current.FSpellControl.GetSpell(74).FSpellName;

    // SmokeCloud.pas:156
    public override bool VisibleForPlayer(sbyte Player)
    {
        HNWalkData Walk = default;
        bool Result = FPlayerIndex == Player;
        ref var walk = ref Walk;
        var yhx = FCloud.Yhx;
        var xhx = FCloud.Xhx;
        HNWalkData.InitHNWalk(ref Walk, xhx, yhx, 0);
        while (!Result && walk.Radius <= 3)
        {
            Result = AoWHSMap.Current.VisibleForPlayer(walk.X, walk.Y, FCloud.Lhx, Player);
            HNWalkData.WalkToNextHN(ref Walk);
        }

        return Result;
    }

    // The unused fourth byte in the original register result has no defined value.
    public override XYL GetLocation() => new() { X = FCloud.GetXhx(), Y = FCloud.GetYhx(), L = FCloud.GetLhx() };

    public override bool Deactivate(byte player)
    {
        if ((FEnchantmentStatus.Image & 1) == 0) return false;
        bool result = base.Deactivate(player);
        if ((FCloud.FStatus.Image & 0x20) != 0) FCloud?.Dispose();
        return result;
    }

    public enum CloudEnchantmentOptionsElement : byte
    {
        Bit0 = 0, Bit1 = 1, Bit2 = 2, Bit3 = 3, Bit4 = 4, Bit5 = 5, Bit6 = 6, Bit7 = 7
    }
}
