using Aow.Rtl;
using Aow.Units.HSEngine;

namespace Aow.Units.SmokeCloud;

public partial class TSmokeCloudEnchantment
{
    // The unused fourth byte in the original register result has no defined value.
    public override TXYL GetLocation() => new() { X = FCloud.GetXhx(), Y = FCloud.GetYhx(), L = FCloud.GetLhx() };
    public override bool Deactivate(byte player)
    {
        if ((FEnchantmentStatus.Image & 1) == 0) return false;
        bool result = base.Deactivate(player);
        if ((FCloud.FStatus.Image & 0x20) != 0) FCloud.Free();
        return result;
    }
}
